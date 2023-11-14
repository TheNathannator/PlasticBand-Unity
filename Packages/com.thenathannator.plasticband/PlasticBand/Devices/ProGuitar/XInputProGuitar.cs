using System;
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

        private ushort m_Frets1;
        private ushort m_Frets2;

        private byte m_Velocity1;
        private byte m_Velocity2;
        private byte m_Velocity3;
        private byte m_Velocity4;
        private byte m_Velocity5;
        private byte m_Velocity6;

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

        public bool green
        {
            get => (m_Velocity1 & 0x80) != 0;
            set => m_Velocity1.SetBit(0x80, value);
        }

        public bool red
        {
            get => (m_Velocity2 & 0x80) != 0;
            set => m_Velocity2.SetBit(0x80, value);
        }

        public bool yellow
        {
            get => (m_Velocity3 & 0x80) != 0;
            set => m_Velocity3.SetBit(0x80, value);
        }

        public bool blue
        {
            get => (m_Velocity4 & 0x80) != 0;
            set => m_Velocity4.SetBit(0x80, value);
        }

        public bool orange
        {
            get => (m_Velocity5 & 0x80) != 0;
            set => m_Velocity5.SetBit(0x80, value);
        }

        public bool solo
        {
            get => (m_Frets2 & 0x8000) != 0;
            set => m_Frets2.SetBit(0x8000, value);
        }

        public ushort frets1 => (ushort)(m_Frets1 & 0x7FFF);
        public ushort frets2 => (ushort)(m_Frets2 & 0x7FFF);

        public byte fret1
        {
            get => (byte)m_Frets1.GetMask(0x1F, 0);
            set => m_Frets1.SetMask(value, 0x1F, 0);
        }

        public byte fret2
        {
            get => (byte)m_Frets1.GetMask(0x1F, 5);
            set => m_Frets1.SetMask(value, 0x1F, 5);
        }

        public byte fret3
        {
            get => (byte)m_Frets1.GetMask(0x1F, 10);
            set => m_Frets1.SetMask(value, 0x1F, 10);
        }

        public byte fret4
        {
            get => (byte)m_Frets2.GetMask(0x1F, 0);
            set => m_Frets2.SetMask(value, 0x1F, 0);
        }

        public byte fret5
        {
            get => (byte)m_Frets2.GetMask(0x1F, 5);
            set => m_Frets2.SetMask(value, 0x1F, 5);
        }

        public byte fret6
        {
            get => (byte)m_Frets2.GetMask(0x1F, 10);
            set => m_Frets2.SetMask(value, 0x1F, 10);
        }

        public byte velocity1
        {
            get => (byte)(m_Velocity1 & 0x7F);
            set => m_Velocity1.SetMask(value, 0x7F, 0);
        }

        public byte velocity2
        {
            get => (byte)(m_Velocity2 & 0x7F);
            set => m_Velocity2.SetMask(value, 0x7F, 0);
        }

        public byte velocity3
        {
            get => (byte)(m_Velocity3 & 0x7F);
            set => m_Velocity3.SetMask(value, 0x7F, 0);
        }

        public byte velocity4
        {
            get => (byte)(m_Velocity4 & 0x7F);
            set => m_Velocity4.SetMask(value, 0x7F, 0);
        }

        public byte velocity5
        {
            get => (byte)(m_Velocity5 & 0x7F);
            set => m_Velocity5.SetMask(value, 0x7F, 0);
        }

        public byte velocity6
        {
            get => (byte)(m_Velocity6 & 0x7F);
            set => m_Velocity6.SetMask(value, 0x7F, 0);
        }

        // These inputs are not exposed in XInput unfortunately
        public bool tilt
        {
            get => false;
            set => throw new NotSupportedException("XInput Pro Guitars do not support tilt.");
        }

        public bool digitalPedal
        {
            get => false;
            set => throw new NotSupportedException("XInput Pro Guitars do not support the digital pedal input.");
        }
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
    internal class XInputProGuitar : TranslatingProGuitar<XInputProGuitarState>
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputProGuitar>(XInputNonStandardSubType.ProGuitar);
        }
    }
}
