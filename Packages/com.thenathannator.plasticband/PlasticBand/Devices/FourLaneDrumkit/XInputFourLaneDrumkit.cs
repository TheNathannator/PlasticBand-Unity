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
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputFourLaneDrumkitState : IInputStateTypeInfo
    {
        const string kPadParameters = "redBit=13,yellowBit=15,blueBit=14,greenBit=12,padBit=7,cymbalBit=9";
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        // TODO: D-pad up/down should be ignored when hitting yellow or blue cymbal, but
        // it needs to be ignored without interfering with pad detection
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]
        [InputControl(name = "kick2", layout = "Button", bit = 6)]

        [InputControl(name = "kick1", layout = "Button", bit = 8)]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y")]

        [InputControl(name = "redPad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "yellowPad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "bluePad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "greenPad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "yellowCymbal", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "blueCymbal", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "greenCymbal", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        public ushort buttons;

        public fixed byte unused[2];

        // TODO: More proper velocity mappings
        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Velocity")]
        public short redVelocity;

        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Velocity")]
        public short yellowVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Velocity")]
        public short blueVelocity;

        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Velocity")]
        public short greenVelocity;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput 4-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitState), displayName = "XInput Rock Band Drumkit")]
    public class XInputFourLaneDrumkit : FourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="XInputFourLaneDrumkit"/>.
        /// </summary>
        public static new XInputFourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputFourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputFourLaneDrumkit> all => s_AllDevices;
        private static readonly List<XInputFourLaneDrumkit> s_AllDevices = new List<XInputFourLaneDrumkit>();

        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputFourLaneDrumkit>(XInputController.DeviceSubType.DrumKit);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputFourLaneDrumkit"/>.
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
