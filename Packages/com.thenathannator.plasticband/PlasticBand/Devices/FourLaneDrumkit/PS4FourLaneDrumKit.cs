using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS4.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS4 Rock Band drumkits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4FourLaneDrumkitState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        private fixed byte unused1[4];

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        // Handled below, FourLaneDrums doesn't have these
        // [InputControl(name = "buttonWest", layout = "Button", bit = 4, displayName = "Square")]
        // [InputControl(name = "buttonSouth", layout = "Button", bit = 5, displayName = "Cross")]
        // [InputControl(name = "buttonEast", layout = "Button", bit = 6, displayName = "Circle")]
        // [InputControl(name = "buttonNorth", layout = "Button", bit = 7, displayName = "Triangle")]

        [InputControl(name = "kick1", layout = "Button", bit = 8)]
        [InputControl(name = "kick2", layout = "Button", bit = 9)]

        [InputControl(name = "selectButton", layout = "Button", bit = 12)]
        [InputControl(name = "startButton", layout = "Button", bit = 13)]
        public ushort buttons1;

        [InputControl(name = "psButton", layout = "Button", bit = 0, displayName = "PlayStation")]
        public byte buttons2;

        private fixed byte unused2[35];

        // TODO: Currently these just act like buttons, when velocity support is implemented for the other drumkits
        // this needs to be adjusted to match how those will then behave
        [InputControl(name = "redPad", layout = "ButtonAxisPair", offset = 0)]
        [InputControl(name = "redPad/button", offset = 5, bit = 6)] // buttonEast (Circle)
        [InputControl(name = "redPad/axis", layout = "DiscreteButton", format = "BYTE", offset = 43, parameters = "minValue=1, maxValue=255")]
        public byte redPadVelocity;

        [InputControl(name = "bluePad", layout = "ButtonAxisPair", offset = 0)]
        [InputControl(name = "bluePad/button", offset = 5, bit = 4)] // buttonWest (Square)
        [InputControl(name = "bluePad/axis", layout = "DiscreteButton", format = "BYTE", offset = 44, parameters = "minValue=1, maxValue=255")]
        public byte bluePadVelocity;

        [InputControl(name = "yellowPad", layout = "ButtonAxisPair", offset = 0)]
        [InputControl(name = "redPad/button", offset = 5, bit = 7)] // buttonNorth (Triangle)
        [InputControl(name = "redPad/axis", layout = "DiscreteButton", format = "BYTE", offset = 45, parameters = "minValue=1, maxValue=255")]
        public byte yellowPadVelocity;

        [InputControl(name = "greenPad", layout = "ButtonAxisPair", offset = 0)]
        [InputControl(name = "redPad/button", offset = 5, bit = 5)] // buttonSouth (Cross)
        [InputControl(name = "redPad/axis", layout = "DiscreteButton", format = "BYTE", offset = 46, parameters = "minValue=1, maxValue=255")]
        public byte greenPadVelocity;

        [InputControl(name = "yellowCymbal", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        public byte yellowCymbalVelocity;

        [InputControl(name = "blueCymbal", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        public byte blueCymbalVelocity;

        [InputControl(name = "greenCymbal", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        public byte greenCymbalVelocity;

        private fixed byte unused3[28];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS4 Rock Band drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS4FourLaneDrumkitState), displayName = "Harmonix Drum Kit for PlayStation(R)4")]
    public class PS4FourLaneDrumkit : FourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="PS4FourLaneDrumkit"/>.
        /// </summary>
        public static new PS4FourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS4FourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS4FourLaneDrumkit> all => s_AllDevices;
        private static readonly List<PS4FourLaneDrumkit> s_AllDevices = new List<PS4FourLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="PS4FourLaneDrumkit"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            // MadCatz
            InputSystem.RegisterLayout<PS4FourLaneDrumkit>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x0738)
                .WithCapability("productId", 0x8262)
            );

            // PDP
            // Product ID is not known yet
            // InputSystem.RegisterLayout<PS4FourLaneDrumkit>(matches: new InputDeviceMatcher()
            //     .WithInterface("HID")
            //     .WithCapability("vendorId", 0x0E6F)
            //     .WithCapability("productId", 0x0173)
            // );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS4FourLaneDrumkit"/>.
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
