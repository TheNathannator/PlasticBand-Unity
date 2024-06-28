using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.LowLevel
{
    using static XInputController;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputGamepad : IInputStateTypeInfo
    {
        public static readonly FourCC Format = new FourCC('X', 'I', 'N', 'P');
        public FourCC format => Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0, displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", format = "BIT", bit = 4, displayName = "Start", usage = "Menu")]
        [InputControl(name = "selectButton", layout = "Button", format = "BIT", bit = 5, displayName = "Back")]
        [InputControl(name = "leftStickClick", layout = "Button", format = "BIT", bit = 6, displayName = "Left Stick Click")]
        [InputControl(name = "rightStickClick", layout = "Button", format = "BIT", bit = 7, displayName = "Right Stick Click")]

        [InputControl(name = "leftBumper", layout = "Button", format = "BIT", bit = 8, displayName = "Left Bumper")]
        [InputControl(name = "rightBumper", layout = "Button", format = "BIT", bit = 9, displayName = "Right Bumper")]
        [InputControl(name = "guide", layout = "Button", format = "BIT", bit = 10, displayName = "Xbox")]

        [InputControl(name = "buttonA", layout = "Button", format = "BIT", bit = 12, displayName = "A", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "buttonB", layout = "Button", format = "BIT", bit = 13, displayName = "B", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "buttonX", layout = "Button", format = "BIT", bit = 14, displayName = "X")]
        [InputControl(name = "buttonY", layout = "Button", format = "BIT", bit = 15, displayName = "Y")]
        public ushort buttons;

        [InputControl(name = "leftTrigger", layout = "Button", format = "BYTE", displayName = "Left Trigger")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "Button", format = "BYTE", displayName = "Right Trigger")]
        public byte rightTrigger;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2S", displayName = "Left Stick")]
        [InputControl(name = "leftStick/x", format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/left", format = "SHRT")]
        [InputControl(name = "leftStick/right", format = "SHRT")]

        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "leftStick/down", format = "SHRT", offset = 2)]
        public short leftStickX;
        public short leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2S", displayName = "Right Stick")]
        [InputControl(name = "rightStick/x", format = "SHRT", parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/left", format = "SHRT")]
        [InputControl(name = "rightStick/right", format = "SHRT")]

        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "rightStick/down", format = "SHRT", offset = 2)]
        public short rightStickX;
        public short rightStickY;
    }

    internal struct XInputVibration
    {
        public short leftMotor;
        public short rightMotor;
    }

    // This doesn't match the actual XINPUT_CAPABILITIES,
    /// rather it's the format reported by Unity for the device capabilities JSON.
    internal struct XInputCapabilities
    {
        // public int userIndex; // We don't need to read this in, and it's not necessary to be set
        public int type;
        public DeviceSubType subType;
        public DeviceFlags flags;
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

    internal enum XInputNonStandardSubType
    {
        StageKit = 9,
        ProKeyboard = 15,
        Turntable = 23,
        ProGuitar = 25,
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