using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/PS3%20and%20Wii%20U.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS3/Wii U GHL devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiUSixFretGuitarState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "white1", layout = "Button", bit = 0)]
        [InputControl(name = "black1", layout = "Button", bit = 1)]
        [InputControl(name = "black2", layout = "Button", bit = 2)]
        [InputControl(name = "black3", layout = "Button", bit = 3)]

        [InputControl(name = "white2", layout = "Button", bit = 4)]
        [InputControl(name = "white3", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]
        [InputControl(name = "ghtvButton", layout = "Button", bit = 10)]

        [InputControl(name = "syncButton", layout = "Button", bit = 12, displayName = "D-pad Center")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 15)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=15,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        public byte unused1;

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x7F,maxValue=0,nullValue=0x80")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x81,maxValue=0xFF,nullValue=0x80")]
        public byte strumBar;

        public byte unused2;

        // TODO: See if any normalization is needed
        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        public fixed byte unused3[12];

        // TODO: See if any additional normalization is needed
        [InputControl(name = "tilt", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short tilt;

        public fixed short unused4[3];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3/Wii U GHL guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3WiiUSixFretGuitarState), displayName = "PS3/Wii U 6-Fret Guitar")]
    public class PS3WiiUSixFretGuitar : PokedSixFretGuitar
    {
        /// <summary>
        /// The current <see cref="PS3WiiUSixFretGuitar"/>.
        /// </summary>
        public static new PS3WiiUSixFretGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3WiiUSixFretGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3WiiUSixFretGuitar> all => s_AllDevices;
        private static readonly List<PS3WiiUSixFretGuitar> s_AllDevices = new List<PS3WiiUSixFretGuitar>();

        /// <summary>
        /// Registers <see cref="PS3WiiUSixFretGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3WiiUSixFretGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x12BA)
                .WithCapability("productId", 0x074B)
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3WiiUSixFretGuitar"/>.
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

        /// <summary>
        /// Magic data to be sent periodically to unlock full input data.
        /// </summary>
        private static PS3OutputCommand s_PokeCommand = new PS3OutputCommand(
            0x02, // TODO: Determine if this report ID is correct/necessary
            0x02,
            new byte[PS3OutputCommand.kDataSize] { 0x08, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        protected override void OnPoke() => device.ExecuteCommand(ref s_PokeCommand);
    }
}