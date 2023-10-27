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
    }
}