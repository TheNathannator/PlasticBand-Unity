using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUp(this HidDpad dpad) => dpad == HidDpad.UpLeft || dpad <= HidDpad.UpRight;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRight(this HidDpad dpad) => dpad >= HidDpad.UpRight && dpad <= HidDpad.DownRight;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDown(this HidDpad dpad) => dpad >= HidDpad.DownRight && dpad <= HidDpad.DownLeft;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLeft(this HidDpad dpad) => dpad >= HidDpad.DownLeft && dpad <= HidDpad.UpLeft;

        public static void SetUp(ref this HidDpad dpad, bool pressed)
        {
            if (dpad.IsUp() == pressed)
                return;

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

        public static void SetRight(ref this HidDpad dpad, bool pressed)
        {
            if (dpad.IsRight() == pressed)
                return;

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

        public static void SetDown(ref this HidDpad dpad, bool pressed)
        {
            if (dpad.IsDown() == pressed)
                return;

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

        public static void SetLeft(ref this HidDpad dpad, bool pressed)
        {
            if (dpad.IsLeft() == pressed)
                return;

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
    }
}