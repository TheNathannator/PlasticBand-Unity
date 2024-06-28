using System.Runtime.InteropServices;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    internal enum GameInputButton : ushort
    {
        Menu = 1 << 2,
        View = 1 << 3,

        A = 1 << 4,
        B = 1 << 5,
        X = 1 << 6,
        Y = 1 << 7,

        DpadUp = 1 << 8,
        DpadDown = 1 << 9,
        DpadLeft = 1 << 10,
        DpadRight = 1 << 11,

        LeftShoulder = 1 << 12,
        RightShoulder = 1 << 13,
        LeftThumb = 1 << 14,
        RightThumb = 1 << 15,
    }

    internal static class GameInputDefinitions
    {
        public const string InterfaceName = "GameInput";
        public static readonly FourCC InputFormat = new FourCC('G', 'I', 'P');
        public static readonly FourCC OutputFormat = new FourCC('G', 'I', 'P', 'O');
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct GameInputNavigationState : IInputStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;

        [InputControl(name = "start", layout = "Button", format = "BIT", bit = 2, displayName = "Menu")]
        [InputControl(name = "select", layout = "Button", format = "BIT", bit = 3, displayName = "View")]

        [InputControl(name = "buttonSouth", layout = "Button", format = "BIT", bit = 4, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", format = "BIT", bit = 5, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", format = "BIT", bit = 6, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", format = "BIT", bit = 7, displayName = "Y")]

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 8)]
        [InputControl(name = "dpad/up", bit = 8)]
        [InputControl(name = "dpad/down", bit = 9)]
        [InputControl(name = "dpad/left", bit = 10)]
        [InputControl(name = "dpad/right", bit = 11)]

        [InputControl(name = "leftShoulder", layout = "Button", format = "BIT", bit = 12, displayName = "Left Shoulder")]
        [InputControl(name = "rightShoulder", layout = "Button", format = "BIT", bit = 13, displayName = "Right Shoulder")]
        [InputControl(name = "leftStickPress", layout = "Button", format = "BIT", bit = 14, displayName = "Left Stick Press")]
        [InputControl(name = "rightStickPress", layout = "Button", format = "BIT", bit = 15, displayName = "Right Stick Press")]
        public ushort buttons;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneGamepadState : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x20;

        public byte reportId;

        [InputControl(name = "start", layout = "Button", format = "BIT", bit = 2, displayName = "Menu")]
        [InputControl(name = "select", layout = "Button", format = "BIT", bit = 3, displayName = "View")]

        [InputControl(name = "buttonSouth", layout = "Button", format = "BIT", bit = 4, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", format = "BIT", bit = 5, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", format = "BIT", bit = 6, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", format = "BIT", bit = 7, displayName = "Y")]

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 8)]
        [InputControl(name = "dpad/up", bit = 8)]
        [InputControl(name = "dpad/down", bit = 9)]
        [InputControl(name = "dpad/left", bit = 10)]
        [InputControl(name = "dpad/right", bit = 11)]

        [InputControl(name = "leftShoulder", layout = "Button", format = "BIT", bit = 12, displayName = "Left Shoulder")]
        [InputControl(name = "rightShoulder", layout = "Button", format = "BIT", bit = 13, displayName = "Right Shoulder")]
        [InputControl(name = "leftStickPress", layout = "Button", format = "BIT", bit = 14, displayName = "Left Stick Press")]
        [InputControl(name = "rightStickPress", layout = "Button", format = "BIT", bit = 15, displayName = "Right Stick Press")]
        public ushort buttons;

        [InputControl(name = "leftTrigger", layout = "Button", format = "BIT", sizeInBits = 10, displayName = "Left Trigger")]
        public ushort leftTrigger;

        [InputControl(name = "rightTrigger", layout = "Button", format = "BIT", sizeInBits = 10, displayName = "Right Trigger")]
        public ushort rightTrigger;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2S", displayName = "Left Stick")]
        [InputControl(name = "leftStick/x", format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/left", format = "SHRT")]
        [InputControl(name = "leftStick/right", format = "SHRT")]

        [InputControl(name = "leftStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "leftStick/down", format = "SHRT", offset = 2)]
        public short leftStickX;
        public short leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2S", displayName = "Right Stick")]
        [InputControl(name = "rightStick/x", format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/left", format = "SHRT")]
        [InputControl(name = "rightStick/right", format = "SHRT")]

        [InputControl(name = "rightStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "rightStick/down", format = "SHRT", offset = 2)]
        public short rightStickX;
        public short rightStickY;
    }

    internal static class GameInputExtensions
    {
        internal static void SetBit(ref this GameInputButton value, GameInputButton mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }
}