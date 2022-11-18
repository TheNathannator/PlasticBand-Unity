using UnityEngine;

namespace PlasticBand
{
    /// <summary>
    /// Assorted utility functions.
    /// </summary>
    internal static class Utilities
    {
        public static bool TryParseJson<TData>(string json, out TData data)
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    data = JsonUtility.FromJson<TData>(json);
                    return true;
                }
                catch
                {
                    // Fall through to end
                }
            }

            data = default;
            return false;
        }
    }
}