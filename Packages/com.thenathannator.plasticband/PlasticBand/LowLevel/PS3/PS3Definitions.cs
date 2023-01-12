using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for standard PS3 controllers.
    /// Note that this most likely does not reflect the actual PS3 controller input state.
    /// This is meant for use as a base for the states of other PS3 devices, which follow a common report structure.
    /// </summary>
    // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-controllers.html
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3Gamepad : IInputStateTypeInfo
    {
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
        [InputControl(name = "leftStick/x", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "leftStick/left", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/right", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]
        public byte leftStickX;

        [InputControl(name = "leftStick/y", format = "BYTE", parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "leftStick/up", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/down", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
        public byte leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "rightStick/left", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/right", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]
        public byte rightStickX;

        [InputControl(name = "rightStick/y", format = "BYTE", parameters = "invert,normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "rightStick/up", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/down", format = "BYTE", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
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

        [InputControl(name = "accelX", layout = "Axis", format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort accelX;

        [InputControl(name = "accelZ", layout = "Axis", format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort accelZ;

        [InputControl(name = "accelY", layout = "Axis", format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort accelY;

        [InputControl(name = "gyro", layout = "Axis", format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public ushort gyro;
    }
}
