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

        public bool kick1
        {
            get => (buttons & XInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(XInputButton.LeftShoulder, value);
        }

        public bool kick2
        {
            get => (buttons & XInputButton.LeftThumb) != 0;
            set => buttons.SetBit(XInputButton.LeftThumb, value);
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

        public bool pad
        {
            get => (buttons & XInputButton.RightThumb) != 0;
            set => buttons.SetBit(XInputButton.RightThumb, value);
        }

        public bool cymbal
        {
            get => (buttons & XInputButton.RightShoulder) != 0;
            set => buttons.SetBit(XInputButton.RightShoulder, value);
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

        byte IFourLaneDrumkitState_Flags.redPadVelocity
        {
            get => GetVelocity(redVelocity);
            set => SetVelocity(ref redVelocity, value);
        }

        byte IFourLaneDrumkitState_Flags.yellowPadVelocity
        {
            get => GetVelocity(yellowVelocity);
            set => SetVelocity(ref yellowVelocity, value);
        }

        byte IFourLaneDrumkitState_Flags.bluePadVelocity
        {
            get => GetVelocity(blueVelocity);
            set => SetVelocity(ref blueVelocity, value);
        }

        byte IFourLaneDrumkitState_Flags.greenPadVelocity
        {
            get => GetVelocity(greenVelocity);
            set => SetVelocity(ref greenVelocity, value);
        }

        byte IFourLaneDrumkitState_Flags.yellowCymbalVelocity
        {
            get => GetVelocity(yellowVelocity);
            set => SetVelocity(ref yellowVelocity, value);
        }

        byte IFourLaneDrumkitState_Flags.blueCymbalVelocity
        {
            get => GetVelocity(blueVelocity);
            set => SetVelocity(ref blueVelocity, value);
        }

        byte IFourLaneDrumkitState_Flags.greenCymbalVelocity
        {
            get => GetVelocity(greenVelocity);
            set => SetVelocity(ref greenVelocity, value);
        }

        private static byte GetVelocity(short velocity)
            => (byte)((~velocity & 0x7FFF) >> 7);

        private static void SetVelocity(ref short velocity, byte value)
            => velocity = (short)(~value << 7);
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
