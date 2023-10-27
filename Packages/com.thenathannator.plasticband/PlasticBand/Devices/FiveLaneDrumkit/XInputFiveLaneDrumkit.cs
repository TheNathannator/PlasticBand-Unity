using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

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

        public bool south => (buttons & XInputButton.A) != 0;
        public bool east => (buttons & XInputButton.B) != 0;
        public bool west => (buttons & XInputButton.X) != 0;
        public bool north => (buttons & XInputButton.Y) != 0;

        public bool red => east;
        public bool yellow => north;
        public bool blue => west;
        public bool green => south;
        public bool orange => (buttons & XInputButton.RightShoulder) != 0;
        public bool kick => (buttons & XInputButton.LeftShoulder) != 0;

        public bool start => (buttons & XInputButton.Start) != 0;
        public bool select => (buttons & XInputButton.Back) != 0;
        public bool system => (buttons & XInputButton.Guide) != 0;

        public bool dpadUp => (buttons & XInputButton.DpadUp) != 0;
        public bool dpadDown => (buttons & XInputButton.DpadDown) != 0;
        public bool dpadLeft => (buttons & XInputButton.DpadLeft) != 0;
        public bool dpadRight => (buttons & XInputButton.DpadRight) != 0;

        byte IFiveLaneDrumkitState.redVelocity => redVelocity;
        byte IFiveLaneDrumkitState.yellowVelocity => yellowVelocity;
        byte IFiveLaneDrumkitState.blueVelocity => blueVelocity;
        byte IFiveLaneDrumkitState.orangeVelocity => orangeVelocity;
        byte IFiveLaneDrumkitState.greenVelocity => greenVelocity;
        byte IFiveLaneDrumkitState.kickVelocity => kickVelocity;
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
            // 4-lane kits and 5-lane kits share the same subtype, they need to be differentiated in another way
            // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
            // realistically that isn't likely to be held when powering on
            // May be some more specific capability data that also distinguishes them, but that probably isn't reliable
            XInputLayoutFinder.RegisterLayout<XInputFiveLaneDrumkit>(XInputController.DeviceSubType.DrumKit,
                (capabilities, state) => (state.buttons & (ushort)XInputButton.LeftThumb) != 0);
        }
    }
}
