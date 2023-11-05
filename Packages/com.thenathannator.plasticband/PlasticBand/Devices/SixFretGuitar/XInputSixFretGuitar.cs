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

        public bool dpadUp => (buttons & XInputButton.DpadUp) != 0;
        public bool dpadDown => (buttons & XInputButton.DpadDown) != 0;
        public bool dpadLeft => (buttons & XInputButton.DpadLeft) != 0;
        public bool dpadRight => (buttons & XInputButton.DpadRight) != 0;

        public bool start => (buttons & XInputButton.Start) != 0;
        public bool select => (buttons & XInputButton.Back) != 0;
        public bool ghtv => (buttons & XInputButton.LeftThumb) != 0;
        public bool system => (buttons & XInputButton.Guide) != 0;

        public bool black1 => (buttons & XInputButton.A) != 0;
        public bool black2 => (buttons & XInputButton.B) != 0;
        public bool black3 => (buttons & XInputButton.Y) != 0;
        public bool white1 => (buttons & XInputButton.X) != 0;
        public bool white2 => (buttons & XInputButton.LeftShoulder) != 0;
        public bool white3 => (buttons & XInputButton.RightShoulder) != 0;

        public bool strumUp => strumBar > 0;
        public bool strumDown => strumBar < 0;

        public byte whammy => (byte)(m_Whammy >> 7);
        public sbyte tilt => (sbyte)(m_Tilt >> 8);
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
                (capabilities, state) => (capabilities.flags & XInputController.DeviceFlags.NoNavigation) != 0);
        }
    }
}