using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputTurntableState : IInputStateTypeInfo
    {
        FourCC IInputStateTypeInfo.format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]
        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5)]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 12)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13)]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, alias = "euphoria")]
        public ushort buttons;

        [InputControl(name = "leftTableGreen", layout = "Button", bit = 0)]
        [InputControl(name = "leftTableRed", layout = "Button", bit = 1)]
        [InputControl(name = "leftTableBlue", layout = "Button", bit = 2)]
        public byte leftTableButtons;

        [InputControl(name = "rightTableGreen", layout = "Button", bit = 0)]
        [InputControl(name = "rightTableRed", layout = "Button", bit = 1)]
        [InputControl(name = "rightTableBlue", layout = "Button", bit = 2)]
        public byte rightTableButtons;

        [InputControl(name = "leftTableVelocity", layout = "Axis", noisy = true, parameters = "minValue=-64,maxValue=64")]
        public short leftTableVelocity;

        [InputControl(name = "rightTableVelocity", layout = "Axis", noisy = true, parameters = "minValue=-64,maxValue=64")]
        public short rightTableVelocity;

        [InputControl(name = "effectsDial", layout = "Axis")]
        public short effectsDial;

        [InputControl(name = "crossFader", layout = "Axis")]
        public short crossFader;
    }
}

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputTurntableState), displayName = "XInput Turntable")]
    internal class XInputTurntable : Turntable
    {
        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputTurntable>((int)XInputNonStandardSubType.Turntable);
        }

        protected override void SendEuphoriaCommand(float brightness)
        {
            var command = new XInputVibrationCommand(0, brightness);
            this.ExecuteCommand(ref command);
        }
    }
}
