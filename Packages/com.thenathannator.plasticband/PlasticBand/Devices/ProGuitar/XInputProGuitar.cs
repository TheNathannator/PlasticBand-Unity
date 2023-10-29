using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Guitar/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputProGuitarState : IProGuitarState
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        private readonly ushort m_Frets1;
        private readonly ushort m_Frets2;

        private readonly byte m_Velocity1;
        private readonly byte m_Velocity2;
        private readonly byte m_Velocity3;
        private readonly byte m_Velocity4;
        private readonly byte m_Velocity5;
        private readonly byte m_Velocity6;

        public bool south => (buttons & XInputButton.A) != 0;
        public bool east => (buttons & XInputButton.B) != 0;
        public bool west => (buttons & XInputButton.X) != 0;
        public bool north => (buttons & XInputButton.Y) != 0;

        public bool dpadUp => (buttons & XInputButton.DpadUp) != 0;
        public bool dpadDown => (buttons & XInputButton.DpadDown) != 0;
        public bool dpadLeft => (buttons & XInputButton.DpadLeft) != 0;
        public bool dpadRight => (buttons & XInputButton.DpadRight) != 0;

        public bool start => (buttons & XInputButton.Start) != 0;
        public bool select => (buttons & XInputButton.Back) != 0;
        public bool system => (buttons & XInputButton.Guide) != 0;

        public bool green => (m_Velocity1 & 0x80) != 0;
        public bool red => (m_Velocity2 & 0x80) != 0;
        public bool yellow => (m_Velocity3 & 0x80) != 0;
        public bool blue => (m_Velocity4 & 0x80) != 0;
        public bool orange => (m_Velocity5 & 0x80) != 0;
        public bool solo => (m_Frets2 & 0x8000) != 0;

        public ushort frets1 => (ushort)(m_Frets1 & 0x7FFF);
        public ushort frets2 => (ushort)(m_Frets2 & 0x7FFF);

        public byte velocity1 => (byte)(m_Velocity1 & 0x7F);
        public byte velocity2 => (byte)(m_Velocity2 & 0x7F);
        public byte velocity3 => (byte)(m_Velocity3 & 0x7F);
        public byte velocity4 => (byte)(m_Velocity4 & 0x7F);
        public byte velocity5 => (byte)(m_Velocity5 & 0x7F);
        public byte velocity6 => (byte)(m_Velocity6 & 0x7F);

        // These inputs are not exposed in XInput unfortunately
        public bool tilt => false;
        public bool digitalPedal => false;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputProGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedProGuitarState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedProGuitarButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedProGuitarButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedProGuitarButton.West, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedProGuitarButton.North, displayName = "Y")]

        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedProGuitarButton.Select, displayName = "Back")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedProGuitarButton.System, displayName = "Guide")]
        public TranslatedProGuitarState state;
    }

    [InputControlLayout(stateType = typeof(XInputProGuitarLayout), displayName = "XInput Rock Band Pro Guitar")]
    internal class XInputProGuitar : ProGuitar
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputProGuitar>(XInputNonStandardSubType.ProGuitar);
        }
    }
}
