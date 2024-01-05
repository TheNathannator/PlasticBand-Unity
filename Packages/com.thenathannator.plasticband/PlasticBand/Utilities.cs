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

        public static void LoggedExecuteCommand<TCommand>(this InputDevice device, ref TCommand command)
            where TCommand : struct, IInputDeviceCommandInfo
        {
            long result = device.ExecuteCommand(ref command);
#if UNITY_EDITOR || PLASTICBAND_VERBOSE_LOGGING
            if (result < 0)
                Debug.LogError($"[Device {device}] Failed to execute command {command.typeStatic}! Error value: {result}");
#endif
        }

        /// <summary>
        /// Determines whether or not the button is pressed in the given state event.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPressedInEvent(this ButtonControl control, InputEventPtr eventPtr)
            => control.ReadValueFromEvent(eventPtr, out float value) && control.IsValueConsideredPressed(value);

        // TODO: Move all of this out into a separate Extensions file
        public static void SetBit(ref this byte value, byte mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= (byte)~mask;
        }

        public static void SetBit(ref this short value, short mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= (short)~mask;
        }

        public static void SetBit(ref this ushort value, ushort mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= (ushort)~mask;
        }

        public static byte GetMask(this byte value, byte mask, int bitOffset)
        {
            return (byte)((value >> bitOffset) & mask);
        }

        public static ushort GetMask(this ushort value, ushort mask, int bitOffset)
        {
            return (ushort)((value >> bitOffset) & mask);
        }

        public static void SetMask(ref this byte field, byte value, byte mask, int bitOffset)
        {
            int maskedField = field & (~mask << bitOffset);
            int shiftedValue = (value & mask) << bitOffset;
            field = (byte)(maskedField | shiftedValue);
        }

        public static void SetMask(ref this ushort field, ushort value, ushort mask, int bitOffset)
        {
            int maskedField = field & (~mask << bitOffset);
            int shiftedValue = (value & mask) << bitOffset;
            field = (ushort)(maskedField | shiftedValue);
        }
    }
}