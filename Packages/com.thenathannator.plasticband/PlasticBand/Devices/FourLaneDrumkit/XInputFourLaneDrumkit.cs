using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputFourLaneDrumkitState : IFourLaneDrumkitState_Flags
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        private unsafe fixed byte unused[2];

        public short redVelocity;
        public short yellowVelocity;
        public short blueVelocity;
        public short greenVelocity;

        public bool south => (buttons & XInputButton.A) != 0;
        public bool east => (buttons & XInputButton.B) != 0;
        public bool west => (buttons & XInputButton.X) != 0;
        public bool north => (buttons & XInputButton.Y) != 0;

        public bool red => east;
        public bool yellow => north;
        public bool blue => west;
        public bool green => south;

        public bool kick1 => (buttons & XInputButton.LeftShoulder) != 0;
        public bool kick2 => (buttons & XInputButton.LeftThumb) != 0;

        public bool start => (buttons & XInputButton.Start) != 0;
        public bool select => (buttons & XInputButton.Back) != 0;
        public bool system => (buttons & XInputButton.Guide) != 0;

        public bool pad => (buttons & XInputButton.RightThumb) != 0;
        public bool cymbal => (buttons & XInputButton.RightShoulder) != 0;

        public bool dpadUp => (buttons & XInputButton.DpadUp) != 0;
        public bool dpadDown => (buttons & XInputButton.DpadDown) != 0;
        public bool dpadLeft => (buttons & XInputButton.DpadLeft) != 0;
        public bool dpadRight => (buttons & XInputButton.DpadRight) != 0;

        public byte redPadVelocity => (byte)((~redVelocity & 0x7FFF) >> 7);
        public byte yellowPadVelocity => (byte)((~yellowVelocity & 0x7FFF) >> 7);
        public byte bluePadVelocity => (byte)((~blueVelocity & 0x7FFF) >> 7);
        public byte greenPadVelocity => (byte)((~greenVelocity & 0x7FFF) >> 7);
        public byte yellowCymbalVelocity => (byte)((~yellowVelocity & 0x7FFF) >> 7);
        public byte blueCymbalVelocity => (byte)((~blueVelocity & 0x7FFF) >> 7);
        public byte greenCymbalVelocity => (byte)((~greenVelocity & 0x7FFF) >> 7);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputFourLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFourLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFourLaneButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFourLaneButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFourLaneButton.West, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFourLaneButton.North, displayName = "Y")]

        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedFourLaneButton.Select, displayName = "Back")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFourLaneButton.System, displayName = "Guide")]
        public TranslatedFourLaneState state;
    }

    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitLayout), displayName = "XInput Rock Band Drumkit")]
    internal class XInputFourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<XInputFourLaneDrumkitState>
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputFourLaneDrumkit>(XInputController.DeviceSubType.DrumKit);
        }
    }
}
