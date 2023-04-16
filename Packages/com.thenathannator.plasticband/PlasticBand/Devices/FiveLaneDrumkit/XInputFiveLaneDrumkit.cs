using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput 5-lane drumkits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputFiveLaneDrumkitState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "kick", layout = "Button", bit = 8)]
        [InputControl(name = "orangeCymbal", layout = "Button", bit = 9)]

        [InputControl(name = "greenPad", layout = "Button", bit = 12)]
        [InputControl(name = "redPad", layout = "Button", bit = 13)]
        [InputControl(name = "bluePad", layout = "Button", bit = 14)]
        [InputControl(name = "yellowCymbal", layout = "Button", bit = 15)]
        public ushort buttons;

        public fixed byte unused[4];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        // - Try and pair velocity with pads directly
        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Velocity")]
        public byte greenVelocity;

        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Velocity")]
        public byte redVelocity;

        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Velocity")]
        public byte yellowVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Velocity")]
        public byte blueVelocity;

        [InputControl(name = "orangeVelocity", layout = "Axis", displayName = "Orange Velocity")]
        public byte orangeVelocity;

        [InputControl(name = "kickVelocity", layout = "Axis", displayName = "Kick Velocity")]
        public byte kickVelocity;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput 5-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitState), displayName = "XInput 5-Lane Drumkit")]
    public class XInputFiveLaneDrumkit : FiveLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="XInputFiveLaneDrumkit"/>.
        /// </summary>
        public static new XInputFiveLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputFiveLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputFiveLaneDrumkit> all => s_AllDevices;
        private static readonly List<XInputFiveLaneDrumkit> s_AllDevices = new List<XInputFiveLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="XInputFiveLaneDrumkit"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            // 4-lane kits and 5-lane kits share the same subtype, they need to be differentiated in another way
            // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
            // realistically that isn't likely to be held when powering on
            // May be some more specific capability data that also distinguishes them, but that probably isn't reliable
            XInputLayoutFinder.RegisterLayout<XInputFiveLaneDrumkit>(XInputController.DeviceSubType.DrumKit,
                (state) => (state.buttons & (ushort)XInputGamepad.Button.LeftThumb) != 0);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputFiveLaneDrumkit"/>.
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
