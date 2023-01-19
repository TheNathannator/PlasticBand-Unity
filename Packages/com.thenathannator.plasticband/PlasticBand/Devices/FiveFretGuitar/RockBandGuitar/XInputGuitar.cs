using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput Guitar devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "strumUp", bit = 0)]
        [InputControl(name = "strumDown", bit = 1)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]

        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x1040")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x2040")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x8040")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x4040")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0140")]
        public ushort buttons;

        // TODO: Define specific ranges for each of the notches
        [InputControl(name = "pickupSwitch", layout = "Axis", parameters = "scale=true,scaleFactor=4")]
        public byte pickupSwitch;

        public fixed byte unused[5];

        [InputControl(name = "whammy", layout = "Axis", defaultState = short.MinValue, parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarState), displayName = "XInput Guitar")]
    public class XInputGuitar : RockBandGuitar
    {
        /// <summary>
        /// The current <see cref="XInputGuitar"/>.
        /// </summary>
        public static new XInputGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputGuitar> all => s_AllDevices;
        private static readonly List<XInputGuitar> s_AllDevices = new List<XInputGuitar>();

        /// <summary>
        /// Registers <see cref="RockBandGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputGuitar>(XInputController.DeviceSubType.Guitar);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputGuitar"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        /// <summary>
        /// Processes when this device is added to the system.
        /// </summary>
        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        /// <summary>
        /// Processes when this device is removed from the system.
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }
    }
}
