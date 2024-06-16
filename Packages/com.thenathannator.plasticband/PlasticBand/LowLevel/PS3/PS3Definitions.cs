using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/_Templates/PS3%20Base.md

namespace PlasticBand.Devices.LowLevel
{
    [Flags]
    internal enum PS3Button : ushort
    {
        None = 0,

        Square = 0x0001,
        Cross = 0x0002,
        Circle = 0x0004,
        Triangle = 0x0008,
        L2 = 0x0010,
        R2 = 0x0020,
        L1 = 0x0040,
        R1 = 0x0080,
        Select = 0x0100,
        Start = 0x0200,
        L3 = 0x0400,
        R3 = 0x0800,
        PlayStation = 0x1000,
    }

    // For reference when creating layouts for PS3 devices
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3DeviceState : IInputStateTypeInfo
    {
        public const byte StickCenter = 0x7F;

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "buttonWest", layout = "Button", format = "BIT", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", format = "BIT", bit = 1, displayName = "Cross", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "buttonEast", layout = "Button", format = "BIT", bit = 2, displayName = "Circle", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "buttonNorth", layout = "Button", format = "BIT", bit = 3, displayName = "Triangle")]

        [InputControl(name = "l2Press", layout = "Button", format = "BIT", bit = 4, displayName = "L2 Press")]
        [InputControl(name = "r2Press", layout = "Button", format = "BIT", bit = 5, displayName = "R2 Press")]
        [InputControl(name = "l1", layout = "Button", format = "BIT", bit = 6, displayName = "L1")]
        [InputControl(name = "r1", layout = "Button", format = "BIT", bit = 7, displayName = "R1")]

        [InputControl(name = "selectButton", layout = "Button", format = "BIT", bit = 8, displayName = "Select")]
        [InputControl(name = "startButton", layout = "Button", format = "BIT", bit = 9, displayName = "Start", usage = "Menu")]
        [InputControl(name = "l3Press", layout = "Button", format = "BIT", bit = 10, displayName = "L3")]
        [InputControl(name = "r3Press", layout = "Button", format = "BIT", bit = 11, displayName = "R3")]

        [InputControl(name = "psButton", layout = "Button", format = "BIT", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", format = "BYTE", defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "leftStick/left", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/right", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]

        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", format = "BYTE", offset = 2, defaultState = 0x80, parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "leftStick/up", format = "BYTE", offset = 2, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/down", format = "BYTE", offset = 2, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
        public byte leftStickX;
        public byte leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", format = "BYTE", defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "rightStick/left", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/right", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]

        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", format = "BYTE", offset = 2, defaultState = 0x80, parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "rightStick/up", format = "BYTE", offset = 2, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/down", format = "BYTE", offset = 2, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
        public byte rightStickX;
        public byte rightStickY;

        [InputControl(name = "pressure_dpadUp", layout = "Axis")]
        public byte pressure_dpadUp;

        [InputControl(name = "pressure_dpadRight", layout = "Axis")]
        public byte pressure_dpadRight;

        [InputControl(name = "pressure_dpadLeft", layout = "Axis")]
        public byte pressure_dpadLeft;

        [InputControl(name = "pressure_dpadDown", layout = "Axis")]
        public byte pressure_dpadDown;

        [InputControl(name = "pressure_l2", layout = "Axis")]
        public byte pressure_l2;

        [InputControl(name = "pressure_r2", layout = "Axis")]
        public byte pressure_r2;

        [InputControl(name = "pressure_l1", layout = "Axis")]
        public byte pressure_l1;

        [InputControl(name = "pressure_r1", layout = "Axis")]
        public byte pressure_r1;

        [InputControl(name = "pressure_triangle", layout = "Axis")]
        public byte pressure_triangle;

        [InputControl(name = "pressure_circle", layout = "Axis")]
        public byte pressure_circle;

        [InputControl(name = "pressure_cross", layout = "Axis")]
        public byte pressure_cross;

        [InputControl(name = "pressure_square", layout = "Axis")]
        public byte pressure_square;

        [InputControl(name = "accelX", layout = "Axis", format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort accelX;

        [InputControl(name = "accelZ", layout = "Axis", format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort accelZ;

        [InputControl(name = "accelY", layout = "Axis", format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort accelY;

        [InputControl(name = "gyro", layout = "Axis", format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort gyro;
    }

    internal static class PS3Extensions
    {
        public static void SetBit(ref this PS3Button value, PS3Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }
}
