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
    // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-rockband-drums.html
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FourLaneDrumkitState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D');

        private byte reportId;

        [InputControl(name = "kick1", layout = "Button", bit = 4)]
        [InputControl(name = "kick2", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        [InputControl(name = "pads", layout = "FourLanePads", format = "USHT", offset = 0, bit = 0,
            parameters = "redBit=2,yellowBit=3,blueBit=0,greenBit=1,padBit=10,cymbalBit=11", displayName = "Kit")]
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

        private fixed byte unused3[12];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 4-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3FourLaneDrumkitState), displayName = "Harmonix Drum Kit for PlayStation(R)3")]
    public class PS3FourLaneDrumkit : FourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="PS3FourLaneDrumkit"/>.
        /// </summary>
        public static new PS3FourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3FourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3FourLaneDrumkit> all => s_AllDevices;
        private static List<PS3FourLaneDrumkit> s_AllDevices = new List<PS3FourLaneDrumkit>();

        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3FourLaneDrumkit>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-rockband-drums.html#vid-and-pid
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x0210) // "Harmonix Drum Kit for PlayStation(R)3"
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3FourLaneDrumkit"/>.
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
