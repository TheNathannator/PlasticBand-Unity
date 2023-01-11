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
    /// The state format for XInput GuitarAlternate devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarAlternateState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0, displayName = "Up/Strum Up", alias = "strumUp")]
        [InputControl(name = "dpad/down", bit = 1, displayName = "Down/Strum Down", alias = "strumDown")]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]
        [InputControl(name = "spPedal", layout = "Button", bit = 9)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]
        public ushort buttons;

        // Was gonna use these parameters based on my Les Paul, but maybe it would be best to
        // leave that up to calibration systems rather than assuming all guitars will be the same
        // parameters = "normalize=true,normalizeMin=0.1,normalizeMax=0.6,normalizeZero=0.36,clamp=2,clampMin=-1,clampMax=1"
        [InputControl(name = "accelZ", layout = "Axis", noisy = true, parameters = "normalize=true,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte accelZ;

        // parameters = "normalize=true,normalizeMin=0.2,normalizeMax=0.75,normalizeZero=0.45,clamp=2,clampMin=-1,clampMax=1"
        [InputControl(name = "accelX", layout = "Axis", noisy = true, parameters = "normalize=true,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte accelX;

        [InputControl(name = "sliderBar", layout = "GuitarHeroSlider", format = "SHRT", displayName = "Touch/Slider Bar")]
        public short slider;

        private short unused;

        [InputControl(name = "whammy", layout = "Axis", parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        // parameters = "normalize=true,normalizeMin=-0.85,normalizeMax=1,normalizeZero=0,clamp=2,clampMin=-1,clampMax=1"
        [InputControl(name = "accelY", layout = "Axis", noisy = true)]
        public short tilt;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput GuitarAlternate guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarAlternateState), displayName = "XInput Guitar Alternate")]
    public class XInputGuitarAlternate : GuitarHeroGuitar
    {
        /// <summary>
        /// The current <see cref="XInputGuitarAlternate"/>.
        /// </summary>
        public static new XInputGuitarAlternate current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputGuitarAlternate"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputGuitarAlternate> all => s_AllDevices;
        private static List<XInputGuitarAlternate> s_AllDevices = new List<XInputGuitarAlternate>();

        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputGuitarAlternate>(XInputController.DeviceSubType.GuitarAlternate);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputGuitarAlternate"/>.
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
    }
}
