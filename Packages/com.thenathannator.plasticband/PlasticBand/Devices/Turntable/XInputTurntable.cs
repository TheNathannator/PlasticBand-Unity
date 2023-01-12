using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputTurntableState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y / Euphoria", alias = "euphoria")]
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
        /// <summary>
        /// The current <see cref="XInputTurntable"/>.
        /// </summary>
        public static new XInputTurntable current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputTurntable"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputTurntable> all => s_AllDevices;
        private static readonly List<XInputTurntable> s_AllDevices = new List<XInputTurntable>();

        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputTurntable>((int)XInputNonStandardSubType.Turntable);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputTurntable"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }

        protected override void OnEuphoriaTick(float brightness)
        {
            // Handle force-disable value
            if (brightness < 0)
                brightness = 0;

            var command = new XInputVibrationCommand(0, brightness);
            this.ExecuteCommand(ref command);
        }
    }
}
