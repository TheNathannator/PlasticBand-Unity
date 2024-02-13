using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Guitar/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiProGuitarState_NoReportId : IProGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public PS3Button buttons;
        public HidDpad dpad;

        private fixed byte unused1[2];

        private ushort m_Frets1;
        private ushort m_Frets2;

        private byte m_Velocity1;
        private byte m_Velocity2;
        private byte m_Velocity3;
        private byte m_Velocity4;
        private byte m_Velocity5;
        private byte m_Velocity6;

        // TODO: Auto-calibration sensor support
        private readonly byte m_AutoCal_Microphone; // NOTE: When the sensor isn't activated, this
        private readonly byte m_AutoCal_Light; // and this just duplicate the tilt axis

        private byte m_Tilt;

        private byte m_Pedal;

        public bool west
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

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

        public bool north
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool select
        {
            get => (buttons & PS3Button.Select) != 0;
            set => buttons.SetBit(PS3Button.Select, value);
        }

        public bool start
        {
            get => (buttons & PS3Button.Start) != 0;
            set => buttons.SetBit(PS3Button.Start, value);
        }

        public bool system
        {
            get => (buttons & PS3Button.PlayStation) != 0;
            set => buttons.SetBit(PS3Button.PlayStation, value);
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

        // Tilt is 0x40 when inactive, 0x7F when active
        public bool tilt
        {
            get => m_Tilt > 0x40;
            set => m_Tilt = value ? (byte)0x7F : (byte)0x40;
        }

        public bool digitalPedal
        {
            get => (m_Pedal & 0x80) != 0;
            set => m_Pedal.SetBit(0x80, value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiProGuitarState_ReportId : IProGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiProGuitarState_NoReportId state;

        public bool west { get => state.west; set => state.west = value; }
        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool north { get => state.north; set => state.north = value; }

        public bool select { get => state.select; set => state.select = value; }
        public bool start { get => state.start; set => state.start = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }

        public bool green { get => state.green; set => state.green = value; }
        public bool red { get => state.red; set => state.red = value; }
        public bool yellow { get => state.yellow; set => state.yellow = value; }
        public bool blue { get => state.blue; set => state.blue = value; }
        public bool orange { get => state.orange; set => state.orange = value; }
        public bool solo { get => state.solo; set => state.solo = value; }

        public ushort frets1 => state.frets1;
        public ushort frets2 => state.frets2;

        public byte fret1 { get => state.fret1; set => state.fret1 = value; }
        public byte fret2 { get => state.fret2; set => state.fret2 = value; }
        public byte fret3 { get => state.fret3; set => state.fret3 = value; }
        public byte fret4 { get => state.fret4; set => state.fret4 = value; }
        public byte fret5 { get => state.fret5; set => state.fret5 = value; }
        public byte fret6 { get => state.fret6; set => state.fret6 = value; }

        public byte velocity1 { get => state.velocity1; set => state.velocity1 = value; }
        public byte velocity2 { get => state.velocity2; set => state.velocity2 = value; }
        public byte velocity3 { get => state.velocity3; set => state.velocity3 = value; }
        public byte velocity4 { get => state.velocity4; set => state.velocity4 = value; }
        public byte velocity5 { get => state.velocity5; set => state.velocity5 = value; }
        public byte velocity6 { get => state.velocity6; set => state.velocity6 = value; }

        public bool tilt { get => state.tilt; set => state.tilt = value; }

        public bool digitalPedal { get => state.digitalPedal; set => state.digitalPedal = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3ProGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedProGuitarState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedProGuitarButton.South, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedProGuitarButton.East, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedProGuitarButton.West, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedProGuitarButton.North, displayName = "Triangle")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedProGuitarButton.System, displayName = "PlayStation")]
        public TranslatedProGuitarState state;
    }

    [InputControlLayout(stateType = typeof(PS3ProGuitarLayout), displayName = "PlayStation 3 Rock Band Pro Guitar")]
    internal class PS3ProGuitar : TranslatingProGuitar<PS3WiiProGuitarState_NoReportId>
    {
        internal new static void Initialize()
        {
            // Mustang
            HidLayoutFinder.RegisterLayout<PS3ProGuitar_ReportId, PS3ProGuitar>(0x12BA, 0x2430);

            // MIDI Pro Adapter (Mustang)
            HidLayoutFinder.RegisterLayout<PS3ProGuitar_ReportId, PS3ProGuitar>(0x12BA, 0x2438);

            // Squire
            HidLayoutFinder.RegisterLayout<PS3ProGuitar_ReportId, PS3ProGuitar>(0x12BA, 0x2530);

            // MIDI Pro Adapter (Squire)
            HidLayoutFinder.RegisterLayout<PS3ProGuitar_ReportId, PS3ProGuitar>(0x12BA, 0x2538);
        }
    }

    [InputControlLayout(stateType = typeof(PS3ProGuitarLayout), displayName = "PlayStation 3 Rock Band Pro Guitar", hideInUI = true)]
    internal class PS3ProGuitar_ReportId : TranslatingProGuitar<PS3WiiProGuitarState_ReportId> { }
}
