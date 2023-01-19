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
    /// <summary>
    /// The state format for PS3 Guitar Hero guitars.
    /// </summary>
    // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-gh-guitar.html
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3GuitarHeroGuitarState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "yellowFret", layout = "Button", bit = 0)]
        [InputControl(name = "greenFret", layout = "Button", bit = 1)]
        [InputControl(name = "redFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [InputControl(name = "spPedal", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 0x1F)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=0x1F,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 0x1F, parameters = "minValue=7,maxValue=1,nullValue=0x1F,wrapAtValue=7")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 0x1F, parameters = "minValue=3,maxValue=5,nullValue=0x1F")]
        public byte dpad;

        public fixed byte unused1[2];

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        [InputControl(name = "touchGreen", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchRed", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchYellow", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchBlue", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchOrange", layout = "GuitarHeroSlider", format = "BYTE")]
        public byte slider;

        public fixed byte unused2[12];

        // This was the previous version of the control, left this here in case it's still needed
        // [InputControl(name = "tilt", layout = "DiscreteButton", noisy = true, parameters = "minValue=0x0185,maxValue=0x01F7,nullValue=0x0184")]
        [InputControl(name = "tilt", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5", alias = "accelX")]
        public short tilt;

        [InputControl(name = "accelZ", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short accelZ;

        [InputControl(name = "accelY", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short accelY;

        public short unused3;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 Guitar Hero guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarState), displayName = "RedOctane Guitar for PlayStation(R)3")]
    public class PS3GuitarHeroGuitar : GuitarHeroGuitar
    {
        /// <summary>
        /// The current <see cref="PS3GuitarHeroGuitar"/>.
        /// </summary>
        public static new PS3GuitarHeroGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3GuitarHeroGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3GuitarHeroGuitar> all => s_AllDevices;
        private static readonly List<PS3GuitarHeroGuitar> s_AllDevices = new List<PS3GuitarHeroGuitar>();

        /// <summary>
        /// Registers <see cref="PS3GuitarHeroGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3GuitarHeroGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x0100) // "RedOctane Guitar for PlayStation(R)3"
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3GuitarHeroGuitar"/>.
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
