using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS3 4-lane drumkits.
    /// </summary>
    // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-gh-drums.html
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FiveLaneDrumkitState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D');

        private byte reportId;

        [InputControl(name = "bluePad", layout = "Button", bit = 0)]
        [InputControl(name = "greenPad", layout = "Button", bit = 1)]
        [InputControl(name = "redPad", layout = "Button", bit = 2)]
        [InputControl(name = "yellowCymbal", layout = "Button", bit = 3)]

        [InputControl(name = "kick", layout = "Button", bit = 4)]
        [InputControl(name = "orangeCymbal", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", alias = "strumUp", displayName = "Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", alias = "strumDown", displayName = "Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        private fixed byte unused2[8];

        // TODO: Try and pair velocity with pads directly
        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Velocity")]
        private byte yellowVelocity;

        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Velocity")]
        private byte redVelocity;

        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Velocity")]
        private byte greenVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Velocity")]
        private byte blueVelocity;

        [InputControl(name = "kickVelocity", layout = "Axis", displayName = "Kick Velocity")]
        private byte kickVelocity;

        [InputControl(name = "orangeVelocity", layout = "Axis", displayName = "Orange Velocity")]
        private byte orangeVelocity;

        private fixed byte unused3[10];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 4-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitState), displayName = "RedOctane Drum Kit for PlayStation(R)3")]
    public class PS3FiveLaneDrumkit : FiveLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="PS3FiveLaneDrumkit"/>.
        /// </summary>
        public static new PS3FiveLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3FiveLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3FiveLaneDrumkit> all => s_AllDevices;
        private static List<PS3FiveLaneDrumkit> s_AllDevices = new List<PS3FiveLaneDrumkit>();

        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3FiveLaneDrumkit>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-gh-drums.html#vid-and-pid
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x0120) // "RedOctane Drum Kit for PlayStation(R)3"
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3FiveLaneDrumkit"/>.
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
