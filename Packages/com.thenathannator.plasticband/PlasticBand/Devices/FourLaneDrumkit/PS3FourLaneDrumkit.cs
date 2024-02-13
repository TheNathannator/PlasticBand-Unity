using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3WiiFourLaneDrumkitState_NoReportId : IFourLaneDrumkitState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public PS3Button buttons;
        public HidDpad dpad;

        private unsafe fixed byte unused2[8];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        private byte m_YellowVelocity;
        private byte m_RedVelocity;
        private byte m_GreenVelocity;
        private byte m_BlueVelocity;

        public bool red_east
        {
            get => (buttons & PS3Button.Circle) != 0;
            set => buttons.SetBit(PS3Button.Circle, value);
        }

        public bool yellow_north
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool blue_west
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

        public bool green_south
        {
            get => (buttons & PS3Button.Cross) != 0;
            set => buttons.SetBit(PS3Button.Cross, value);
        }

        public bool kick1
        {
            get => (buttons & PS3Button.L2) != 0;
            set => buttons.SetBit(PS3Button.L2, value);
        }

        public bool kick2
        {
            get => (buttons & PS3Button.R2) != 0;
            set => buttons.SetBit(PS3Button.R2, value);
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

        public bool pad
        {
            get => (buttons & PS3Button.L3) != 0;
            set => buttons.SetBit(PS3Button.L3, value);
        }

        public bool cymbal
        {
            get => (buttons & PS3Button.R3) != 0;
            set => buttons.SetBit(PS3Button.R3, value);
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

        public byte redPadVelocity
        {
            get => (byte)~m_RedVelocity;
            set => m_RedVelocity = (byte)~value;
        }

        public byte yellowPadVelocity
        {
            get => (byte)~m_YellowVelocity;
            set => m_YellowVelocity = (byte)~value;
        }

        public byte bluePadVelocity
        {
            get => (byte)~m_BlueVelocity;
            set => m_BlueVelocity = (byte)~value;
        }

        public byte greenPadVelocity
        {
            get => (byte)~m_GreenVelocity;
            set => m_GreenVelocity = (byte)~value;
        }

        public byte yellowCymbalVelocity
        {
            get => yellowPadVelocity;
            set => yellowPadVelocity = value;
        }

        public byte blueCymbalVelocity
        {
            get => bluePadVelocity;
            set => bluePadVelocity = value;
        }

        public byte greenCymbalVelocity
        {
            get => greenPadVelocity;
            set => greenPadVelocity = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3WiiFourLaneDrumkitState_ReportId : IFourLaneDrumkitState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiFourLaneDrumkitState_NoReportId state;

        public bool blue_west { get => state.blue_west; set => state.blue_west = value; }
        public bool green_south { get => state.green_south; set => state.green_south = value; }
        public bool red_east { get => state.red_east; set => state.red_east = value; }
        public bool yellow_north { get => state.yellow_north; set => state.yellow_north = value; }

        public bool kick1 { get => state.kick1; set => state.kick1 = value; }
        public bool kick2 { get => state.kick2; set => state.kick2 = value; }

        public bool select { get => state.select; set => state.select = value; }
        public bool start { get => state.start; set => state.start = value; }

        public bool pad { get => state.pad; set => state.pad = value; }
        public bool cymbal { get => state.cymbal; set => state.cymbal = value; }

        public bool system { get => state.system; set => state.system = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public byte redPadVelocity { get => state.redPadVelocity; set => state.redPadVelocity = value; }
        public byte yellowPadVelocity { get => state.yellowPadVelocity; set => state.yellowPadVelocity = value; }
        public byte bluePadVelocity { get => state.bluePadVelocity; set => state.bluePadVelocity = value; }
        public byte greenPadVelocity { get => state.greenPadVelocity; set => state.greenPadVelocity = value; }
        public byte yellowCymbalVelocity { get => state.yellowCymbalVelocity; set => state.yellowCymbalVelocity = value; }
        public byte blueCymbalVelocity { get => state.blueCymbalVelocity; set => state.blueCymbalVelocity = value; }
        public byte greenCymbalVelocity { get => state.greenCymbalVelocity; set => state.greenCymbalVelocity = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PSFourLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFourLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFourLaneButton.South, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFourLaneButton.East, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFourLaneButton.West, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFourLaneButton.North, displayName = "Triangle")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFourLaneButton.System, displayName = "PlayStation")]
        public TranslatedFourLaneState state;
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 3 Rock Band Drumkit")]
    internal class PS3FourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_NoReportId>
    {
        internal new static void Initialize()
        {
            // Drumkit
            HidLayoutFinder.RegisterLayout<PS3FourLaneDrumkit_ReportId, PS3FourLaneDrumkit>(0x12BA, 0x0210);

            // MIDI Pro Adapter
            HidLayoutFinder.RegisterLayout<PS3FourLaneDrumkit_ReportId, PS3FourLaneDrumkit>(0x12BA, 0x0218);
        }
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 3 Rock Band Drumkit", hideInUI = true)]
    internal class PS3FourLaneDrumkit_ReportId : TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_ReportId> { }
}
