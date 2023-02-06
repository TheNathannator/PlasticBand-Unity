using System;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class BuildPackageWindow : EditorWindow
{
    [MenuItem("Window/Build/Build Package", false, 2100)]
    public static void Init()
    {
        GetWindow<BuildPackageWindow>();
    }

    const string packagePath = "Packages/com.thenathannator.plasticband";
    string outputPath_GUI;
    string outputPath;
    string tempDir;
    bool startBuild;
    Task copyTask;
    Task removeIgnoredTask;
    Task deleteTask;
    PackRequest packRequest;

    void OnGUI()
    {
        outputPath_GUI = EditorGUILayout.TextField("Output Path", outputPath_GUI);
        if (GUILayout.Button("Build") && !startBuild && copyTask == null && removeIgnoredTask == null && deleteTask == null && packRequest == null)
        {
            outputPath = outputPath_GUI;
            startBuild = true;
        }

        if (deleteTask != null)
        {
            GUILayout.Label("Waiting for temp folder to be deleted...");
        }
        else if (startBuild)
        {
            GUILayout.Label("Starting build...");
        }
        else if (copyTask != null)
        {
            GUILayout.Label("Copying package files to temp directory...");
        }
        else if (removeIgnoredTask != null)
        {
            GUILayout.Label("Removing \"Ignored\" folders...");
        }
        else if (packRequest != null)
        {
            GUILayout.Label("Creating package...");
        }
    }

    void Update()
    {
        // Don't start while the delete task is running
        if (deleteTask != null)
        {
            if (deleteTask.IsCompleted)
            {
                if (deleteTask.IsFaulted)
                {
                    Debug.LogError("Deleting temp directory failed!\n" + UnrollAggregateException(deleteTask.Exception));
                }
                deleteTask = null;
            }
        }
        // Start the build process
        else if (startBuild)
        {
            try
            {
                if (!Directory.Exists(outputPath))
                {
                    throw new DirectoryNotFoundException("Output path must be a directory!");
                }

                tempDir = Path.Combine(outputPath, "buildTemp");
                copyTask = CopyDirectory(Path.GetFullPath(packagePath), tempDir);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while starting copy!\n" + ex);
            }
            finally
            {
                startBuild = false;
            }
        }
        // Wait until copy task is done
        else if (copyTask != null && copyTask.IsCompleted)
        {
            try
            {
                if (!copyTask.IsFaulted)
                {
                    removeIgnoredTask = RemoveIgnored(tempDir);
                }
                else
                {
                    Debug.LogError("Copying to temp directory failed!\n" + UnrollAggregateException(copyTask.Exception));
                    deleteTask = DeleteDirectory(tempDir);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while starting \"Ignore\" removal task!\n" + ex);
                deleteTask = DeleteDirectory(tempDir);
            }
            finally
            {
                copyTask = null;
            }
        }
        // Wait until remove task is done
        else if (removeIgnoredTask != null && removeIgnoredTask.IsCompleted)
        {
            try
            {
                if (!removeIgnoredTask.IsFaulted)
                {
                    string packOutput = Path.Combine(outputPath, "Builds");
                    Debug.Log($"Creating package for {tempDir} at {packOutput}");
                    packRequest = Client.Pack(tempDir, packOutput);
                }
                else
                {
                    Debug.LogError("Removing \"Ignored\" folders failed!\n" + UnrollAggregateException(removeIgnoredTask.Exception));
                    deleteTask = DeleteDirectory(tempDir);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while starting pack request!\n" + ex);
                deleteTask = DeleteDirectory(tempDir);
            }
            finally
            {
                removeIgnoredTask = null;
            }
        }
        // Wait until packaging is done
        else if (packRequest != null && packRequest.IsCompleted)
        {
            string message;
            if (packRequest.Status == StatusCode.Success)
            {
                message = "Successfully packed!\nTarball has been output to " + packRequest.Result.tarballPath;
            }
            else if (packRequest.Status == StatusCode.Failure)
            {
                if (packRequest.Error == null) // Thanks Unity
                {
                    // A domain reload occured, ignore
                    packRequest = null;
                    return;
                }
                else
                {
                    message = "Failed to pack!\nMessage: " + packRequest.Error.message + "\nError code: " + packRequest.Error.errorCode;
                }
            }
            else
            {
                message = "Unhandled status code! " + packRequest.Status;
            }

            EditorUtility.DisplayDialog("Build Result", message, "OK");

            packRequest = null;
            deleteTask = DeleteDirectory(tempDir);
        }
    }

    Task CopyDirectory(string copyFrom, string copyTo)
    {
        Debug.Log($"Copying package files from {copyFrom} to temp directory {copyTo}");
        return Task.Run(() => {
            if (!Directory.Exists(copyFrom))
            {
                throw new DirectoryNotFoundException("Could not find package folder!");
            }

            if (!Directory.Exists(copyTo))
            {
                Directory.CreateDirectory(copyTo);
            }

            var dirInfo = new DirectoryInfo(copyFrom);
            foreach (var fileInfo in dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                string path = fileInfo.FullName;
                string dir = fileInfo.Directory.FullName;
                string newDir;
                if (dir.Length > copyFrom.Length)
                {
                    string shortDir = dir.Substring(copyFrom.Length + 1);
                    newDir = Path.Combine(copyTo, shortDir);
                    if (!Directory.Exists(newDir))
                    {
                        Debug.Log($"Creating directory {newDir}");
                        Directory.CreateDirectory(newDir);
                    }
                }
                else
                {
                    newDir = copyTo;
                }

                string newPath = Path.Combine(newDir, fileInfo.Name);
                Debug.Log($"Copying from {path} to {newPath}");
                File.Copy(path, newPath, true);
            }
        });
    }

    Task DeleteDirectory(string directory)
    {
        Debug.Log($"Removing temp folder {directory}");
        return Task.Run(() => {
            if (!Directory.Exists(directory))
            {
                return;
            }

            Directory.Delete(directory, true);
        });
    }

    Task RemoveIgnored(string directory)
    {
        Debug.Log($"Removing \"Ignored\" folders from {directory}");
        // Ensure all active file handles are disposed
        GC.Collect();
        return Task.Run(() => {
            foreach (string path in Directory.EnumerateDirectories(directory, "Ignored", SearchOption.AllDirectories))
            {
                Debug.Log($"Deleting directory {path}");
                Directory.Delete(path, true);
                string noEndSeparator = path.TrimEnd(Path.DirectorySeparatorChar);
                string metaPath = noEndSeparator + ".meta";
                Debug.Log($"Deleting file {metaPath}");
                File.Delete(metaPath);
            }
        });
    }

    Exception UnrollAggregateException(AggregateException ex)
    {
        if (ex.InnerExceptions.Count < 2)
        {
            return ex.InnerException;
        }

        return ex;
    }
}
