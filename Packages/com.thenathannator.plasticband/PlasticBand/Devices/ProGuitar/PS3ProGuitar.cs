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
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 27)]
    internal unsafe struct PS3WiiProGuitarState_NoReportId : IProGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public PS3Button buttons;
        public HidDpad dpad;

        private fixed byte unused1[2];

        private readonly ushort m_Frets1;
        private readonly ushort m_Frets2;

        private readonly byte m_Velocity1;
        private readonly byte m_Velocity2;
        private readonly byte m_Velocity3;
        private readonly byte m_Velocity4;
        private readonly byte m_Velocity5;
        private readonly byte m_Velocity6;

        // TODO: Auto-calibration sensor support
        private readonly byte m_AutoCal_Microphone; // NOTE: When the sensor isn't activated, this
        private readonly byte m_AutoCal_Light; // and this just duplicate the tilt axis

        private readonly byte m_Tilt;

        private readonly byte m_Pedal;

        public bool west => (buttons & PS3Button.Square) != 0;
        public bool south => (buttons & PS3Button.Cross) != 0;
        public bool east => (buttons & PS3Button.Circle) != 0;
        public bool north => (buttons & PS3Button.Triangle) != 0;

        public bool select => (buttons & PS3Button.Select) != 0;
        public bool start => (buttons & PS3Button.Start) != 0;
        public bool system => (buttons & PS3Button.PlayStation) != 0;

        public bool dpadUp => dpad.IsUp();
        public bool dpadRight => dpad.IsRight();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();

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

        // Tilt is 0x40 when inactive, 0x7F when active
        public bool tilt => m_Tilt != 0x40;

        public bool digitalPedal => (m_Pedal & 0x80) != 0;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiProGuitarState_ReportId : IProGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiProGuitarState_NoReportId state;

        public bool west => state.west;
        public bool south => state.south;
        public bool east => state.east;
        public bool north => state.north;

        public bool select => state.select;
        public bool start => state.start;
        public bool system => state.system;

        public bool dpadUp => state.dpadUp;
        public bool dpadRight => state.dpadRight;
        public bool dpadDown => state.dpadDown;
        public bool dpadLeft => state.dpadLeft;

        public bool green => state.green;
        public bool red => state.red;
        public bool yellow => state.yellow;
        public bool blue => state.blue;
        public bool orange => state.orange;
        public bool solo => state.solo;

        public ushort frets1 => state.frets1;
        public ushort frets2 => state.frets2;

        public byte velocity1 => state.velocity1;
        public byte velocity2 => state.velocity2;
        public byte velocity3 => state.velocity3;
        public byte velocity4 => state.velocity4;
        public byte velocity5 => state.velocity5;
        public byte velocity6 => state.velocity6;

        public bool tilt => state.tilt;

        public bool digitalPedal => state.digitalPedal;
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
