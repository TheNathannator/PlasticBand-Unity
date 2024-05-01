using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputRockBandGuitarState : IRockBandGuitarState_Flags
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        private byte m_PickupSwitch;

        private fixed byte unused[5];

        private short m_Whammy;
        private short m_Tilt;

        public bool green
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool red
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool yellow
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
        }

        public bool blue
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool orange
        {
            get => (buttons & XInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(XInputButton.LeftShoulder, value);
        }

        public bool solo
        {
            get => (buttons & XInputButton.LeftThumb) != 0;
            set => buttons.SetBit(XInputButton.LeftThumb, value);
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

        // Ranges from -32768 to 32767
        public byte whammy
        {
            get => (byte)((m_Whammy >> 8) + 0x80);
            set => m_Whammy = (short)(((value - 0x80) << 8) | value);
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt >> 8);
            set => m_Tilt = (short)((value << 8) | (byte)value);
        }

        public int pickupSwitch
        {
            get => RockBandGuitarState.GetPickupSwitchNotch(m_PickupSwitch);
            set => m_PickupSwitch = RockBandGuitarState.SetPickupSwitchNotch(value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputRockBandGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedRockBandGuitarState.Format;

        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.Select, displayName = "Back")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.System, displayName = "Guide")]
        public TranslatedRockBandGuitarState state;
    }

    [InputControlLayout(stateType = typeof(XInputRockBandGuitarLayout), displayName = "XInput Rock Band Guitar")]
    internal class XInputRockBandGuitar : TranslatingRockBandGuitar_Flags<XInputRockBandGuitarState>
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputRockBandGuitar>(XInputController.DeviceSubType.Guitar);

            // TODO: Determine if this has any input differences
            XInputLayoutFinder.RegisterLayout<XInputRockBandGuitar>(XInputController.DeviceSubType.GuitarBass);
        }
    }
}
