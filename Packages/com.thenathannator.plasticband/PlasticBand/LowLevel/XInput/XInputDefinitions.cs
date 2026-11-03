using System;
using System.Runtime.InteropServices;
using PlasticBand.Controls;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    [Flags]
    internal enum XInputButton : ushort
    {
        DpadUp = 0x0001,
        DpadDown = 0x0002,
        DpadLeft = 0x0004,
        DpadRight = 0x0008,
        Start = 0x0010,
        Back = 0x0020,
        LeftThumb = 0x0040,
        RightThumb = 0x0080,
        LeftShoulder = 0x0100,
        RightShoulder = 0x0200,
        Guide = 0x0400,
        A = 0x1000,
        B = 0x2000,
        X = 0x4000,
        Y = 0x8000
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputGamepad : IGamepadState
    {
        public static readonly FourCC Format = new FourCC('X', 'I', 'N', 'P');
        public FourCC format => Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0, displayName = "D-Pad")]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "start", layout = "Button", format = "BIT", bit = 4, displayName = "Menu", usage = "Menu")]
        [InputControl(name = "select", layout = "Button", format = "BIT", bit = 5, displayName = "View")]
        [InputControl(name = "leftStickPress", layout = "Button", format = "BIT", bit = 6, displayName = "Left Stick Press")]
        [InputControl(name = "rightStickPress", layout = "Button", format = "BIT", bit = 7, displayName = "Right Stick Press")]

        [InputControl(name = "leftShoulder", layout = "Button", format = "BIT", bit = 8, displayName = "Left Bumper")]
        [InputControl(name = "rightShoulder", layout = "Button", format = "BIT", bit = 9, displayName = "Right Bumper")]
        [InputControl(name = "systemButton", layout = "Button", format = "BIT", bit = 10, displayName = "Guide")]

        [InputControl(name = "buttonSouth", layout = "Button", format = "BIT", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", format = "BIT", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", format = "BIT", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", format = "BIT", bit = 15, displayName = "Y")]
        public XInputButton buttons;

        [InputControl(name = "leftTrigger", layout = "IntButton", format = "BYTE", parameters = "minValue=0x00,maxValue=0xFF", displayName = "Left Trigger")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "IntButton", format = "BYTE", parameters = "minValue=0x00,maxValue=0xFF", displayName = "Right Trigger")]
        public byte rightTrigger;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2S", displayName = "Left Stick")]
        [InputControl(name = "leftStick/x", offset = 0, layout = "IntAxis", format = "SHRT", parameters = "minValue=-32768,maxValue=32767,zeroPoint=0")]
        [InputControl(name = "leftStick/left", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        [InputControl(name = "leftStick/right", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", offset = 2, layout = "IntAxis", format = "SHRT", parameters = "minValue=-32768,maxValue=32767,zeroPoint=0")]
        [InputControl(name = "leftStick/up", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        [InputControl(name = "leftStick/down", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        public short leftStickX;
        public short leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2S", displayName = "Right Stick")]
        [InputControl(name = "rightStick/x", offset = 0, layout = "IntAxis", format = "SHRT", parameters = "minValue=-32768,maxValue=32767,zeroPoint=0")]
        [InputControl(name = "rightStick/left", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        [InputControl(name = "rightStick/right", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", offset = 2, layout = "IntAxis", format = "SHRT", parameters = "minValue=-32768,maxValue=32767,zeroPoint=0")]
        [InputControl(name = "rightStick/up", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        [InputControl(name = "rightStick/down", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        public short rightStickX;
        public short rightStickY;

        public bool start
        {
            get => (buttons & XInputButton.Start) != 0;
            set => buttons.SetBit(XInputButton.Start, value);
        }

        public bool select
        {
            get => (buttons & XInputButton.Back) != 0;
            set => buttons.SetBit(XInputButton.Back, value);
        }

        public bool system
        {
            get => (buttons & XInputButton.Guide) != 0;
            set => buttons.SetBit(XInputButton.Guide, value);
        }

        public bool share
        {
            get => false;
            set { }
        }

        public bool south
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool east
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool west
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool north
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
        }

        public bool dpadUp
        {
            get => (buttons & XInputButton.DpadUp) != 0;
            set => buttons.SetBit(XInputButton.DpadUp, value);
        }

        public bool dpadDown
        {
            get => (buttons & XInputButton.DpadDown) != 0;
            set => buttons.SetBit(XInputButton.DpadDown, value);
        }

        public bool dpadLeft
        {
            get => (buttons & XInputButton.DpadLeft) != 0;
            set => buttons.SetBit(XInputButton.DpadLeft, value);
        }

        public bool dpadRight
        {
            get => (buttons & XInputButton.DpadRight) != 0;
            set => buttons.SetBit(XInputButton.DpadRight, value);
        }

        public bool leftShoulder
        {
            get => (buttons & XInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(XInputButton.LeftShoulder, value);
        }

        public bool rightShoulder
        {
            get => (buttons & XInputButton.RightShoulder) != 0;
            set => buttons.SetBit(XInputButton.RightShoulder, value);
        }

        public bool leftTriggerPress
        {
            get => false;
            set { }
        }

        public bool rightTriggerPress
        {
            get => false;
            set { }
        }

        public bool leftStickPress
        {
            get => (buttons & XInputButton.LeftThumb) != 0;
            set => buttons.SetBit(XInputButton.LeftThumb, value);
        }

        public bool rightStickPress
        {
            get => (buttons & XInputButton.RightThumb) != 0;
            set => buttons.SetBit(XInputButton.RightThumb, value);
        }

        byte IGamepadState.leftTrigger
        {
            get => leftTrigger;
            set => leftTrigger = value;
        }

        byte IGamepadState.rightTrigger
        {
            get => rightTrigger;
            set => rightTrigger = value;
        }

        sbyte IGamepadState.leftStickX
        {
            get => XInputDefinitions.GetAxisX(leftStickX);
            set => leftStickX = XInputDefinitions.SetAxisX(value);
        }

        sbyte IGamepadState.leftStickY
        {
            get => XInputDefinitions.GetAxisY(leftStickY);
            set => leftStickY = XInputDefinitions.SetAxisY(value);
        }

        sbyte IGamepadState.rightStickX
        {
            get => XInputDefinitions.GetAxisX(rightStickX);
            set => rightStickX = XInputDefinitions.SetAxisX(value);
        }

        sbyte IGamepadState.rightStickY
        {
            get => XInputDefinitions.GetAxisY(rightStickY);
            set => rightStickY = XInputDefinitions.SetAxisY(value);
        }
    }

    [Serializable]
    internal struct XInputVibration
    {
        public short leftMotor;
        public short rightMotor;
    }

    internal enum XInputDeviceType
    {
        Gamepad = 1,
    }

    internal enum XInputDeviceSubType
    {
        Unknown = 0,
        Gamepad = 1,
        Wheel = 2,
        ArcadeStick = 3,
        FlightStick = 4,
        DancePad = 5,
        Guitar = 6,
        GuitarAlternate = 7,
        DrumKit = 8,
        StageKit = 9,
        GuitarBass = 11,
        ProKeyboard = 15,
        ArcadePad = 19,
        Turntable = 23,
        ProGuitar = 25,
    }

    [Flags]
    public enum XInputDeviceFlags
    {
        None = 0,

        ForceFeedbackSupported = 0x01,
        Wireless = 0x02,
        VoiceSupported = 0x04,
        PluginModulesSupported = 0x08,
        NoNavigation = 0x10
    }

    // This doesn't match the actual XINPUT_CAPABILITIES,
    // rather it's the format reported by Unity for the device capabilities JSON.
    [Serializable]
    internal struct XInputCapabilities
    {
        // public int userIndex; // We don't need to read this in, and it's not necessary to be set
        public XInputDeviceType type;
        public XInputDeviceSubType subType;
        public XInputDeviceFlags flags;
        public XInputGamepad gamepad;
        public XInputVibration vibration;
    }

    /* Example JSON of reported capabilities info:
    {
        "userIndex": 0,
        "type": 1,
        "subType": 15,
        "flags": 6,
        "gamepad":
        {
            "buttons": 62271,
            "leftTrigger": 0,
            "rightTrigger": 0,
            "leftStickX": 7085,
            "leftStickY": 4912,
            "rightStickX": 4,
            "rightStickY": 0
        },
        "vibration":
        {
            "leftMotor": 0,
            "rightMotor": 0
        }
    }
    */

    internal static class XInputDefinitions
    {
        public static sbyte GetAxisX(short value)
        {
            return (sbyte)IntegerAxisControl.Convert(
                value,
                short.MinValue, short.MaxValue, 0,
                sbyte.MinValue, sbyte.MaxValue, 0
            );
        }

        public static sbyte GetAxisY(short value)
        {
            return GetAxisX(value);
        }

        public static short SetAxisX(sbyte value)
        {
            return (short)IntegerAxisControl.Convert(
                value,
                sbyte.MinValue, sbyte.MaxValue, 0,
                short.MinValue, short.MaxValue, 0
            );
        }

        public static short SetAxisY(sbyte value)
        {
            return SetAxisX(value);
        }

        public static byte GetAxisUnsignedX(short value)
        {
            return (byte)IntegerAxisControl.Convert(
                value,
                short.MinValue, short.MaxValue, short.MinValue,
                byte.MinValue, byte.MaxValue, byte.MinValue
            );
        }

        public static byte GetAxisUnsignedY(short value)
        {
            return GetAxisUnsignedX(value);
        }

        public static short SetAxisUnsignedX(byte value)
        {
            return (short)IntegerAxisControl.Convert(
                value,
                byte.MinValue, byte.MaxValue, byte.MinValue,
                short.MinValue, short.MaxValue, short.MinValue
            );
        }

        public static short SetAxisUnsignedY(byte value)
        {
            return SetAxisUnsignedX(value);
        }
    }

    internal static class XInputExtensions
    {
        internal static void SetBit(ref this XInputButton value, XInputButton mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }
}