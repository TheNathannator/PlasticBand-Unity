using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
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

        /// <summary>
        /// Attempts to read this <see cref="InputEventPtr"/> as the specified state structure.
        /// </summary>
        /// <remarks>
        /// The retrieved <see cref="StateEvent"/> pointer is returned if this event was a state event,
        /// regardless of read success.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryReadState<TState>(this InputEventPtr eventPtr, out StateEvent* statePtr, out TState state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            if (eventPtr.type != StateEvent.Type)
            {
                statePtr = null;
                state = default;
                return false;
            }

            statePtr = StateEvent.From(eventPtr);
            return statePtr->TryReadState(out state);
        }

        /// <summary>
        /// Attempts to read this <see cref="StateEvent"/> as the specified state structure.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryReadState<TState>(this StateEvent stateEvent, out TState state)
            where TState : unmanaged, IInputStateTypeInfo
        {
            if (stateEvent.stateFormat != default(TState).format || stateEvent.stateSizeInBytes < sizeof(TState))
            {
                state = default;
                return false;
            }

            UnsafeUtility.CopyPtrToStructure(stateEvent.state, out state);
            return true;
        }

        /// <summary>
        /// Determines whether or not the button is pressed in the given state event.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPressedInEvent(this ButtonControl control, InputEventPtr eventPtr)
            => control.ReadValueFromEvent(eventPtr, out float value) && control.IsValueConsideredPressed(value);
    }
}