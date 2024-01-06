using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputSixFretGuitarState : ISixFretGuitarState
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        private fixed byte unused[4];

        public short strumBar;

        private short m_Tilt;
        private short m_Whammy;

        public bool black1
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool black2
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool black3
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
        }

        public bool white1
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool white2
        {
            get => (buttons & XInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(XInputButton.LeftShoulder, value);
        }

        public bool white3
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

        public bool ghtv
        {
            get => (buttons & XInputButton.LeftThumb) != 0;
            set => buttons.SetBit(XInputButton.LeftThumb, value);
        }

        public bool system
        {
            get => (buttons & XInputButton.Guide) != 0;
            set => buttons.SetBit(XInputButton.Guide, value);
        }

        public bool strumUp
        {
            get => strumBar > 0;
            set => strumBar = value ? short.MaxValue : (short)0;
        }

        public bool strumDown
        {
            get => strumBar < 0;
            set => strumBar = value ? short.MinValue : (short)0;
        }

        // Whammy ranges from 0 to short.MaxValue
        public byte whammy
        {
            get => (byte)(m_Whammy >> 7);
            set => m_Whammy = (short)(value << 7);
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt >> 8);
            set => m_Tilt = (short)(value << 8);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "XInput Guitar Hero Live Guitar")]
    internal class XInputSixFretGuitar : TranslatingSixFretGuitar<XInputSixFretGuitarState>
    {
        internal new static void Initialize()
        {
            // Annoyingly, GHL guitars do not have a unique subtype. So, we have to use some other information to identify them.
            XInputLayoutFinder.RegisterLayout<XInputSixFretGuitar>(XInputController.DeviceSubType.GuitarAlternate,
                // Strangely, they report the No Navigation flag. Most likely none of the other guitars report this information,
                // so we check for it here.
                (capabilities) => (capabilities.flags & XInputController.DeviceFlags.NoNavigation) != 0);
        }
    }
}