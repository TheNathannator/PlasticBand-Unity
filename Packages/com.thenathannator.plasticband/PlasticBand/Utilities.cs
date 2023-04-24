using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand
{
    /// <summary>
    /// Assorted utility functions.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Attempts to parse a JSON string into the given type of object.
        /// </summary>
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPressedInEvent(this ButtonControl control, InputEventPtr eventPtr)
            => control.HasValueChangeInEvent(eventPtr) && control.IsValueConsideredPressed(control.ReadValueFromEvent(eventPtr));
    }
}