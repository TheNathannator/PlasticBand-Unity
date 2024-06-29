using System;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

namespace PlasticBand
{
    internal static class Logging
    {
        public static void Message(string message)
            => Debug.Log($"[PlasticBand] {message}");

        public static void Warning(string message)
            => Debug.LogWarning($"[PlasticBand] {message}");

        public static void Error(string message)
            => Debug.LogError($"[PlasticBand] {message}");

        public static void Exception(string message, Exception ex)
        {
            Debug.LogError($"[PlasticBand] {message}");
            Debug.LogException(ex);
        }

        [Conditional("PLASTICBAND_VERBOSE_LOGGING")]
        public static void Verbose(string message)
            => Message(message);
    }
}