using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputFiveLaneDrumkitState : IFiveLaneDrumkitState
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        private unsafe fixed byte unused[4];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        public byte greenVelocity;
        public byte redVelocity;
        public byte yellowVelocity;
        public byte blueVelocity;
        public byte orangeVelocity;
        public byte kickVelocity;

        public bool red_east
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool yellow_north
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
        }

        public bool blue_west
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool green_south
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool orange
        {
            get => (buttons & XInputButton.RightShoulder) != 0;
            set => buttons.SetBit(XInputButton.RightShoulder, value);
        }

        public bool kick
        {
            get => (buttons & XInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(XInputButton.LeftShoulder, value);
        }

        public bool start
        {
            get => (buttons & XInputButton.Start) != 0;
            set => buttons.SetBit(XInputButton.Start, value);
        }

        public bool select
        {
            get => (buttons & XInputButton.Back) != 0;
            set => buttons.SetBit(XInputButton.Back, value);
        }

        public bool system
        {
            get => (buttons & XInputButton.Guide) != 0;
            set => buttons.SetBit(XInputButton.Guide, value);
        }

        public bool dpadUp
        {
            get => (buttons & XInputButton.DpadUp) != 0;
            set => buttons.SetBit(XInputButton.DpadUp, value);
        }

        public bool dpadDown
        {
            get => (buttons & XInputButton.DpadDown) != 0;
            set => buttons.SetBit(XInputButton.DpadDown, value);
        }

        public bool dpadLeft
        {
            get => (buttons & XInputButton.DpadLeft) != 0;
            set => buttons.SetBit(XInputButton.DpadLeft, value);
        }

        public bool dpadRight
        {
            get => (buttons & XInputButton.DpadRight) != 0;
            set => buttons.SetBit(XInputButton.DpadRight, value);
        }

        byte IFiveLaneDrumkitState.redVelocity
        {
            get => redVelocity;
            set => redVelocity = value;
        }

        byte IFiveLaneDrumkitState.yellowVelocity
        {
            get => yellowVelocity;
            set => yellowVelocity = value;
        }

        byte IFiveLaneDrumkitState.blueVelocity
        {
            get => blueVelocity;
            set => blueVelocity = value;
        }

        byte IFiveLaneDrumkitState.orangeVelocity
        {
            get => orangeVelocity;
            set => orangeVelocity = value;
        }

        byte IFiveLaneDrumkitState.greenVelocity
        {
            get => greenVelocity;
            set => greenVelocity = value;
        }

        byte IFiveLaneDrumkitState.kickVelocity
        {
            get => kickVelocity;
            set => kickVelocity = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputFiveLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFiveLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFiveLaneButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFiveLaneButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFiveLaneButton.West, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFiveLaneButton.North, displayName = "Y")]

        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.Select, displayName = "Back")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.System, displayName = "Guide")]
        public TranslatedFiveLaneState state;
    }

    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitLayout), displayName = "XInput Guitar Hero Drumkit")]
    internal class XInputFiveLaneDrumkit : TranslatingFiveLaneDrumkit<XInputFiveLaneDrumkitState>
    {
        internal new static void Initialize()
        {
            // No matcher since special differentiation must be done
            InputSystem.RegisterLayout<XInputFiveLaneDrumkit>();
        }
    }
}
