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
    internal struct XInputFourLaneDrumkitState : IFourLaneDrumkitState_FlagButtons,
        IFourLaneDrumkitState_SharedVelocities
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        private unsafe fixed byte unused[2];

        public short redVelocity;
        public short yellowVelocity;
        public short blueVelocity;
        public short greenVelocity;

        public bool south
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool east
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool west
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool north
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
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

        byte IFourLaneDrumkitState_SharedVelocities.redVelocity
        {
            get => GetVelocity_Positive(redVelocity);
            set => redVelocity = SetVelocity_Positive(value);
        }

        byte IFourLaneDrumkitState_SharedVelocities.yellowVelocity
        {
            get => GetVelocity_Negative(yellowVelocity);
            set => yellowVelocity = SetVelocity_Negative(value);
        }

        byte IFourLaneDrumkitState_SharedVelocities.blueVelocity
        {
            get => GetVelocity_Positive(blueVelocity);
            set => blueVelocity = SetVelocity_Positive(value);
        }

        byte IFourLaneDrumkitState_SharedVelocities.greenVelocity
        {
            get => GetVelocity_Negative(greenVelocity);
            set => greenVelocity = SetVelocity_Negative(value);
        }

        private static byte GetVelocity_Positive(short velocity)
            => (byte)(255 - (byte)(velocity >> 7));

        private static byte GetVelocity_Negative(short velocity)
        {
            if (velocity == 0)
                return 255;

            return (byte)(255 - (byte)((~velocity) >> 7));
        }

        private static short SetVelocity_Positive(byte value)
            => (short)(32767 - ((value << 7) | (value >> 1)));

        private static short SetVelocity_Negative(byte value)
            => (short)(-32767 + ((value << 7) | (value >> 1)));
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
            // No matcher since special differentiation must be done
            InputSystem.RegisterLayout<XInputFourLaneDrumkit>();

            // Except for the ION drumkit, which reports capability info we can rely on to match it
            // This is necessary to prevent it from being identified as a 5-lane drumkit, due to a hardware
            // quirk where it constantly holds the right-stick click input while a pedal is plugged in
            XInputLayoutFinder.RegisterLayout<XInputFourLaneDrumkit>(
                XInputController.DeviceSubType.DrumKit, 0x15E4, 0x0130);
        }
    }
}
