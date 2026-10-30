using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Keyboard/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3ProKeyboardState_NoReportId : IProKeyboardState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle")]

        [InputControl(name = "select", layout = "Button", bit = 8)]
        [InputControl(name = "start", layout = "Button", bit = 9)]

        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public PS3Button buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public HidDpad dpad;

        private byte unused1;
        private byte unused2;

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
        [InputControl(name = "analogPedal", layout = "IntAxis", format = "BIT", bit = 0, sizeInBits = 7, parameters = "minValue=0x00,maxValue=0x7F,zeroPoint=0x00")]
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte m_PedalField;

        [InputControl(name = "touchStrip", layout = "IntAxis", format = "BIT", bit = 0, sizeInBits = 7, parameters = "minValue=0x00,maxValue=0x7F,zeroPoint=0x00")]
        public byte m_TouchField;

        public bool south
        {
            get => (buttons & PS3Button.Cross) != 0;
            set => buttons.SetBit(PS3Button.Cross, value);
        }

        public bool east
        {
            get => (buttons & PS3Button.Circle) != 0;
            set => buttons.SetBit(PS3Button.Circle, value);
        }

        public bool west
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

        public bool north
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => dpad.SetUp(value);
        }

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => dpad.SetRight(value);
        }

        public bool dpadDown
        {
            get => dpad.IsDown();
            set => dpad.SetDown(value);
        }

        public bool dpadLeft
        {
            get => dpad.IsLeft();
            set => dpad.SetLeft(value);
        }

        public bool start
        {
            get => (buttons & PS3Button.Start) != 0;
            set => buttons.SetBit(PS3Button.Start, value);
        }

        public bool select
        {
            get => (buttons & PS3Button.Select) != 0;
            set => buttons.SetBit(PS3Button.Select, value);
        }

        public bool system
        {
            get => (buttons & PS3Button.PlayStation) != 0;
            set => buttons.SetBit(PS3Button.PlayStation, value);
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3ProKeyboardState_ReportId : IProKeyboardState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3ProKeyboardState_NoReportId state;

        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool west { get => state.west; set => state.west = value; }
        public bool north { get => state.north; set => state.north = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool key1 { get => state.key1; set => state.key1 = value; }
        public bool key2 { get => state.key2; set => state.key2 = value; }
        public bool key3 { get => state.key3; set => state.key3 = value; }
        public bool key4 { get => state.key4; set => state.key4 = value; }
        public bool key5 { get => state.key5; set => state.key5 = value; }
        public bool key6 { get => state.key6; set => state.key6 = value; }
        public bool key7 { get => state.key7; set => state.key7 = value; }
        public bool key8 { get => state.key8; set => state.key8 = value; }
        public bool key9 { get => state.key9; set => state.key9 = value; }
        public bool key10 { get => state.key10; set => state.key10 = value; }
        public bool key11 { get => state.key11; set => state.key11 = value; }
        public bool key12 { get => state.key12; set => state.key12 = value; }
        public bool key13 { get => state.key13; set => state.key13 = value; }
        public bool key14 { get => state.key14; set => state.key14 = value; }
        public bool key15 { get => state.key15; set => state.key15 = value; }
        public bool key16 { get => state.key16; set => state.key16 = value; }
        public bool key17 { get => state.key17; set => state.key17 = value; }
        public bool key18 { get => state.key18; set => state.key18 = value; }
        public bool key19 { get => state.key19; set => state.key19 = value; }
        public bool key20 { get => state.key20; set => state.key20 = value; }
        public bool key21 { get => state.key21; set => state.key21 = value; }
        public bool key22 { get => state.key22; set => state.key22 = value; }
        public bool key23 { get => state.key23; set => state.key23 = value; }
        public bool key24 { get => state.key24; set => state.key24 = value; }
        public bool key25 { get => state.key25; set => state.key25 = value; }

        public bool overdrive { get => state.overdrive; set => state.overdrive = value; }
        public bool digitalPedal { get => state.digitalPedal; set => state.digitalPedal = value; }
        public byte analogPedal { get => state.analogPedal; set => state.analogPedal = value; }
        public byte touchStrip { get => state.touchStrip; set => state.touchStrip = value; }
    }

    [InputControlLayout(stateType = typeof(PS3ProKeyboardState_NoReportId), displayName = "PlayStation 3 Rock Band Pro Keyboard")]
    internal class PS3ProKeyboard : ProKeyboard
    {
        internal new static void Initialize()
        {
            // MadCatz Rock Band 3 Pro Keyboard for PS3
            HidLayoutFinder.RegisterLayout<PS3ProKeyboard_ReportId, PS3ProKeyboard>(0x12BA, 0x2330);

            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Keys mode)
            HidLayoutFinder.RegisterLayout<PS3ProKeyboard_ReportId, PS3ProKeyboard>(0x12BA, 0x2338);
        }
    }

    [InputControlLayout(stateType = typeof(PS3ProKeyboardState_ReportId), displayName = "PlayStation 3 Rock Band Pro Keyboard", hideInUI = true)]
    internal class PS3ProKeyboard_ReportId : PS3ProKeyboard { }
}
