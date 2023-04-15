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
    /// The state format for Santroller GHL devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDSixFretGuitarState : IInputStateTypeInfo
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

        [InputControl(name = "tilt", layout = "Axis")]
        public byte tilt;

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x7F,maxValue=0,nullValue=0x80")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x81,maxValue=0xFF,nullValue=0x80")]
        public byte strumBar;

        public byte unused2;

        // TODO: See if any normalization is needed
        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        public fixed byte unused3[14];

        public fixed short unused4[3];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3/Wii U GHL guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(SantrollerHIDSixFretGuitarState), displayName = "Santroller Device in 6-Fret Guitar Mode")]
    public class SantrollerHIDSixFretGuitar : SixFretGuitar
    {
        /// <summary>
        /// The current <see cref="SantrollerHIDSixFretGuitar"/>.
        /// </summary>
        public static new SantrollerHIDSixFretGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerHIDSixFretGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerHIDSixFretGuitar> all => s_AllDevices;
        private static readonly List<SantrollerHIDSixFretGuitar> s_AllDevices = new List<SantrollerHIDSixFretGuitar>();

        /// <summary>
        /// Registers <see cref="SantrollerHIDSixFretGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDSixFretGuitar>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.LiveGuitar, null, nameof(SantrollerHIDSixFretGuitar));
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerHIDSixFretGuitar"/>.
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