using System.Runtime.CompilerServices;
using PlasticBand.Controls;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    internal enum HidDpad : byte
    {
        Up = 0,
        UpRight = 1,
        Right = 2,
        DownRight = 3,
        Down = 4,
        DownLeft = 5,
        Left = 6,
        UpLeft = 7,
        Neutral = 8,
    }

    /// <summary>
    /// Various definitions for HID devices.
    /// </summary>
    internal static class HidDefinitions
    {
        public const string InterfaceName = "HID";

        public static readonly FourCC InputFormat = new FourCC('H', 'I', 'D');
        public static readonly FourCC OutputFormat = new FourCC('H', 'I', 'D', 'O');

        /// <summary>
        /// Converts a signed, left-origin X-axis value represented as an unsigned byte
        /// into a signed, left-origin X-axis value represented as a signed byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte GetByteX(byte value)
        {
            return (sbyte)IntegerAxisControl.Convert(
                value,
                byte.MinValue, byte.MaxValue, 128,
                sbyte.MinValue, sbyte.MaxValue, 0
            );
        }

        /// <summary>
        /// Converts a signed, top-origin Y-axis value represented as an unsigned byte
        /// into a signed, bottom-origin Y-axis value represented as a signed byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte GetInvertedByteY(byte value)
        {
            return (sbyte)IntegerAxisControl.Convert(
                value,
                byte.MaxValue, byte.MinValue, 128,
                sbyte.MinValue, sbyte.MaxValue, 0
            );
        }

        /// <summary>
        /// Converts a signed, left-origin X-axis value represented as a signed byte
        /// into a signed, left-origin X-axis value represented as an unsigned byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetByteX(sbyte value)
        {
            return (byte)IntegerAxisControl.Convert(
                value,
                sbyte.MinValue, sbyte.MaxValue, 0,
                byte.MinValue, byte.MaxValue, 128
            );
        }

        /// <summary>
        /// Converts a signed, bottom-origin Y-axis value represented as a signed byte
        /// into a signed, top-origin Y-axis value represented as an unsigned byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetInvertedByteY(sbyte value)
        {
            return (byte)IntegerAxisControl.Convert(
                value,
                sbyte.MinValue, sbyte.MaxValue, 0,
                byte.MaxValue, byte.MinValue, 128
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUp(this HidDpad dpad) => dpad == HidDpad.UpLeft || dpad == HidDpad.Up || dpad == HidDpad.UpRight;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRight(this HidDpad dpad) => dpad == HidDpad.UpRight || dpad == HidDpad.Right || dpad == HidDpad.DownRight;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDown(this HidDpad dpad) => dpad == HidDpad.DownLeft || dpad == HidDpad.Down || dpad == HidDpad.DownRight;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeft(this HidDpad dpad) => dpad == HidDpad.UpLeft || dpad == HidDpad.Left || dpad == HidDpad.DownLeft;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetUp(ref this HidDpad dpad, bool pressed)
        {
            dpad = dpad.ChangeUp(pressed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetRight(ref this HidDpad dpad, bool pressed)
        {
            dpad = dpad.ChangeRight(pressed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDown(ref this HidDpad dpad, bool pressed)
        {
            dpad = dpad.ChangeDown(pressed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetLeft(ref this HidDpad dpad, bool pressed)
        {
            dpad = dpad.ChangeLeft(pressed);
        }

        public static HidDpad ChangeUp(this HidDpad dpad, bool pressed)
        {
            if (dpad.IsUp() != pressed)
            {
                if (pressed)
                {
                    if (dpad.IsLeft())
                        dpad = HidDpad.UpLeft;
                    else if (dpad.IsRight())
                        dpad = HidDpad.UpRight;
                    else
                        dpad = HidDpad.Up;
                }
                else
                {
                    if (dpad.IsLeft())
                        dpad = HidDpad.Left;
                    else if (dpad.IsRight())
                        dpad = HidDpad.Right;
                    else
                        dpad = HidDpad.Neutral;
                }
            }

            return dpad;
        }

        public static HidDpad ChangeRight(this HidDpad dpad, bool pressed)
        {
            if (dpad.IsRight() != pressed)
            {
                if (pressed)
                {
                    if (dpad.IsUp())
                        dpad = HidDpad.UpRight;
                    else if (dpad.IsDown())
                        dpad = HidDpad.DownRight;
                    else
                        dpad = HidDpad.Right;
                }
                else
                {
                    if (dpad.IsUp())
                        dpad = HidDpad.Up;
                    else if (dpad.IsDown())
                        dpad = HidDpad.Down;
                    else
                        dpad = HidDpad.Neutral;
                }
            }

            return dpad;
        }

        public static HidDpad ChangeDown(this HidDpad dpad, bool pressed)
        {
            if (dpad.IsDown() != pressed)
            {
                if (pressed)
                {
                    if (dpad.IsLeft())
                        dpad = HidDpad.DownLeft;
                    else if (dpad.IsRight())
                        dpad = HidDpad.DownRight;
                    else
                        dpad = HidDpad.Down;
                }
                else
                {
                    if (dpad.IsLeft())
                        dpad = HidDpad.Left;
                    else if (dpad.IsRight())
                        dpad = HidDpad.Right;
                    else
                        dpad = HidDpad.Neutral;
                }
            }

            return dpad;
        }

        public static HidDpad ChangeLeft(this HidDpad dpad, bool pressed)
        {
            if (dpad.IsLeft() != pressed)
            {
                if (pressed)
                {
                    if (dpad.IsUp())
                        dpad = HidDpad.UpLeft;
                    else if (dpad.IsDown())
                        dpad = HidDpad.DownLeft;
                    else
                        dpad = HidDpad.Left;
                }
                else
                {
                    if (dpad.IsUp())
                        dpad = HidDpad.Up;
                    else if (dpad.IsDown())
                        dpad = HidDpad.Down;
                    else
                        dpad = HidDpad.Neutral;
                }
            }

            return dpad;
        }
    }
}