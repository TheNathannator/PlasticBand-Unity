using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Keyboard/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputProKeyboardState : IProKeyboardState
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "start", layout = "Button", bit = 4)]
        [InputControl(name = "select", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y")]
        public XInputButton buttons;

        // NOTE: Fields with InputControl attributes on them *MUST* be public,
        // otherwise the input system will not see them.
        // The following fields are intended to be private, and are named as such.

        [InputControl(name = "key1",  layout = "Button", bit = 7)]
        [InputControl(name = "key2",  layout = "Button", bit = 6)]
        [InputControl(name = "key3",  layout = "Button", bit = 5)]
        [InputControl(name = "key4",  layout = "Button", bit = 4)]
        [InputControl(name = "key5",  layout = "Button", bit = 3)]
        [InputControl(name = "key6",  layout = "Button", bit = 2)]
        [InputControl(name = "key7",  layout = "Button", bit = 1)]
        [InputControl(name = "key8",  layout = "Button", bit = 0)]
        public byte m_KeyMask1;

        [InputControl(name = "key9",  layout = "Button", bit = 7)]
        [InputControl(name = "key10", layout = "Button", bit = 6)]
        [InputControl(name = "key11", layout = "Button", bit = 5)]
        [InputControl(name = "key12", layout = "Button", bit = 4)]
        [InputControl(name = "key13", layout = "Button", bit = 3)]
        [InputControl(name = "key14", layout = "Button", bit = 2)]
        [InputControl(name = "key15", layout = "Button", bit = 1)]
        [InputControl(name = "key16", layout = "Button", bit = 0)]
        public byte m_KeyMask2;

        [InputControl(name = "key17", layout = "Button", bit = 7)]
        [InputControl(name = "key18", layout = "Button", bit = 6)]
        [InputControl(name = "key19", layout = "Button", bit = 5)]
        [InputControl(name = "key20", layout = "Button", bit = 4)]
        [InputControl(name = "key21", layout = "Button", bit = 3)]
        [InputControl(name = "key22", layout = "Button", bit = 2)]
        [InputControl(name = "key23", layout = "Button", bit = 1)]
        [InputControl(name = "key24", layout = "Button", bit = 0)]
        public byte m_KeyMask3;

        [InputControl(name = "key25", layout = "Button", bit = 7)]
        // TODO: Velocities are ignored currently until they can be paired with keys
        // [InputControl(name = "velocity1", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte m_VelocityField1;

        // [InputControl(name = "velocity2", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte m_VelocityField2;

        // [InputControl(name = "velocity3", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte m_VelocityField3;

        // [InputControl(name = "velocity4", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte m_VelocityField;

        // [InputControl(name = "velocity5", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte m_VelocityField5;

        [InputControl(name = "overdrive", layout = "Button", bit = 7)]
        public byte m_OverdriveField;

        // TODO: The normalization here needs verification
        [InputControl(name = "analogPedal", layout = "IntAxis", format = "BIT", sizeInBits = 7, parameters = "minValue=0x00,maxValue=0x7F,zeroPoint=0x00")]
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte m_PedalField;

        // Bytes after this point are not reported via XInput on Windows.

        [InputControl(name = "touchStrip", layout = "IntAxis", format = "BIT", sizeInBits = 7, parameters = "minValue=0x00,maxValue=0x7F,zeroPoint=0x00")]
        public byte m_TouchField;

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

        public bool key1 { get => (m_KeyMask1 & (1 << 7)) != 0; set => m_KeyMask1.SetBit(1 << 7, value); }
        public bool key2 { get => (m_KeyMask1 & (1 << 6)) != 0; set => m_KeyMask1.SetBit(1 << 6, value); }
        public bool key3 { get => (m_KeyMask1 & (1 << 5)) != 0; set => m_KeyMask1.SetBit(1 << 5, value); }
        public bool key4 { get => (m_KeyMask1 & (1 << 4)) != 0; set => m_KeyMask1.SetBit(1 << 4, value); }
        public bool key5 { get => (m_KeyMask1 & (1 << 3)) != 0; set => m_KeyMask1.SetBit(1 << 3, value); }
        public bool key6 { get => (m_KeyMask1 & (1 << 2)) != 0; set => m_KeyMask1.SetBit(1 << 2, value); }
        public bool key7 { get => (m_KeyMask1 & (1 << 1)) != 0; set => m_KeyMask1.SetBit(1 << 1, value); }
        public bool key8 { get => (m_KeyMask1 & (1 << 0)) != 0; set => m_KeyMask1.SetBit(1 << 0, value); }
        public bool key9 { get => (m_KeyMask2 & (1 << 7)) != 0; set => m_KeyMask2.SetBit(1 << 7, value); }
        public bool key10 { get => (m_KeyMask2 & (1 << 6)) != 0; set => m_KeyMask2.SetBit(1 << 6, value); }
        public bool key11 { get => (m_KeyMask2 & (1 << 5)) != 0; set => m_KeyMask2.SetBit(1 << 5, value); }
        public bool key12 { get => (m_KeyMask2 & (1 << 4)) != 0; set => m_KeyMask2.SetBit(1 << 4, value); }
        public bool key13 { get => (m_KeyMask2 & (1 << 3)) != 0; set => m_KeyMask2.SetBit(1 << 3, value); }
        public bool key14 { get => (m_KeyMask2 & (1 << 2)) != 0; set => m_KeyMask2.SetBit(1 << 2, value); }
        public bool key15 { get => (m_KeyMask2 & (1 << 1)) != 0; set => m_KeyMask2.SetBit(1 << 1, value); }
        public bool key16 { get => (m_KeyMask2 & (1 << 0)) != 0; set => m_KeyMask2.SetBit(1 << 0, value); }
        public bool key17 { get => (m_KeyMask3 & (1 << 7)) != 0; set => m_KeyMask3.SetBit(1 << 7, value); }
        public bool key18 { get => (m_KeyMask3 & (1 << 6)) != 0; set => m_KeyMask3.SetBit(1 << 6, value); }
        public bool key19 { get => (m_KeyMask3 & (1 << 5)) != 0; set => m_KeyMask3.SetBit(1 << 5, value); }
        public bool key20 { get => (m_KeyMask3 & (1 << 4)) != 0; set => m_KeyMask3.SetBit(1 << 4, value); }
        public bool key21 { get => (m_KeyMask3 & (1 << 3)) != 0; set => m_KeyMask3.SetBit(1 << 3, value); }
        public bool key22 { get => (m_KeyMask3 & (1 << 2)) != 0; set => m_KeyMask3.SetBit(1 << 2, value); }
        public bool key23 { get => (m_KeyMask3 & (1 << 1)) != 0; set => m_KeyMask3.SetBit(1 << 1, value); }
        public bool key24 { get => (m_KeyMask3 & (1 << 0)) != 0; set => m_KeyMask3.SetBit(1 << 0, value); }
        public bool key25 { get => (m_VelocityField1 & (1 << 7)) != 0; set => m_VelocityField1.SetBit(1 << 7, value); }

        public bool overdrive
        {
            get => (m_OverdriveField & 0x80) != 0;
            set => m_OverdriveField.SetBit(0x80, value);
        }

        public bool digitalPedal
        {
            get => (m_PedalField & 0x80) != 0;
            set => m_PedalField.SetBit(0x80, value);
        }

        public byte analogPedal
        {
            get => (byte)(m_PedalField & 0x7F);
            set => m_PedalField.SetMask(value, 0x7F, 0);
        }

        public byte touchStrip
        {
            get => (byte)(m_TouchField & 0x7F);
            set => m_TouchField.SetMask(value, 0x7F, 0);
        }
    }

    [InputControlLayout(stateType = typeof(XInputProKeyboardState), displayName = "XInput Rock Band Pro Keyboard")]
    internal class XInputProKeyboard : ProKeyboard
    {
        internal new static void Initialize()
        {
            // MadCatz Rock Band 3 Pro Keyboard for Xbox 360
            // MadCatz Rock Band 3 MIDI Pro Adapter for Xbox 360 (Keys mode)
            XInputLayoutFinder.RegisterLayout<XInputProKeyboard>(XInputDeviceSubType.ProKeyboard);
        }
    }
}
