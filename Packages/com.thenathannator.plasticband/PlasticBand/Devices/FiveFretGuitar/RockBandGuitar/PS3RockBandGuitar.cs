using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS3%20and%20Wii.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS3 Guitar Hero guitars.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3RockBandGuitarState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "blueFret", layout = "Button", bit = 0)]
        [InputControl(name = "greenFret", layout = "Button", bit = 1)]
        [InputControl(name = "redFret", layout = "Button", bit = 2)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 3)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [InputControl(name = "tilt", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9, displayName = "Start")]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0042")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0044")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0048")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0041")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0050")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=3,maxValue=5,nullValue=8")]
        public byte dpad;

        public fixed byte unused1[2];

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        // TODO: Define specific ranges for each of the notches
        [InputControl(name = "pickupSwitch", layout = "Axis")]
        public byte pickupSwitch;

        public fixed byte unused2[21];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 Rock Band guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3RockBandGuitarState), displayName = "Harmonix Guitar for PlayStation(R)3")]
    public class PS3RockBandGuitar : RockBandGuitar
    {
        /// <summary>
        /// The current <see cref="PS3RockBandGuitar"/>.
        /// </summary>
        public static new PS3RockBandGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3RockBandGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3RockBandGuitar> all => s_AllDevices;
        private static readonly List<PS3RockBandGuitar> s_AllDevices = new List<PS3RockBandGuitar>();

        /// <summary>
        /// Registers <see cref="PS3RockBandGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3RockBandGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x0200) // "Harmonix Guitar for PlayStation(R)3"
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3RockBandGuitar"/>.
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
