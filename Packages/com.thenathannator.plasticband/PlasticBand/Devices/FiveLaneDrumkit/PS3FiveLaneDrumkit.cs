using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/PS3.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FiveLaneDrumkitState_NoReportId : IFiveLaneDrumkitState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public PS3Button buttons;
        public HidDpad dpad;

        public fixed byte unused2[8];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        private byte m_YellowVelocity;
        private byte m_RedVelocity;
        private byte m_GreenVelocity;
        private byte m_BlueVelocity;
        private byte m_KickVelocity;
        private byte m_OrangeVelocity;

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

        public bool orange
        {
            get => (buttons & PS3Button.R2) != 0;
            set => buttons.SetBit(PS3Button.R2, value);
        }

        public bool kick
        {
            get => (buttons & PS3Button.L2) != 0;
            set => buttons.SetBit(PS3Button.L2, value);
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

        public byte redVelocity
        {
            get => m_RedVelocity;
            set => m_RedVelocity = value;
        }

        public byte yellowVelocity
        {
            get => m_YellowVelocity;
            set => m_YellowVelocity = value;
        }

        public byte blueVelocity
        {
            get => m_BlueVelocity;
            set => m_BlueVelocity = value;
        }

        public byte orangeVelocity
        {
            get => m_OrangeVelocity;
            set => m_OrangeVelocity = value;
        }

        public byte greenVelocity
        {
            get => m_GreenVelocity;
            set => m_GreenVelocity = value;
        }

        public byte kickVelocity
        {
            get => m_KickVelocity;
            set => m_KickVelocity = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FiveLaneDrumkitState_ReportId : IFiveLaneDrumkitState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3FiveLaneDrumkitState_NoReportId state;

        public bool red_east { get => state.red_east; set => state.red_east = value; }
        public bool yellow_north { get => state.yellow_north; set => state.yellow_north = value; }
        public bool blue_west { get => state.blue_west; set => state.blue_west = value; }
        public bool green_south { get => state.green_south; set => state.green_south = value; }
        public bool orange { get => state.orange; set => state.orange = value; }
        public bool kick { get => state.kick; set => state.kick = value; }

        public bool select { get => state.select; set => state.select = value; }
        public bool start { get => state.start; set => state.start = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public byte redVelocity { get => state.redVelocity; set => state.redVelocity = value; }
        public byte yellowVelocity { get => state.yellowVelocity; set => state.yellowVelocity = value; }
        public byte blueVelocity { get => state.blueVelocity; set => state.blueVelocity = value; }
        public byte orangeVelocity { get => state.orangeVelocity; set => state.orangeVelocity = value; }
        public byte greenVelocity { get => state.greenVelocity; set => state.greenVelocity = value; }
        public byte kickVelocity { get => state.kickVelocity; set => state.kickVelocity = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3FiveLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFiveLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFiveLaneButton.South, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFiveLaneButton.East, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFiveLaneButton.West, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFiveLaneButton.North, displayName = "Triangle")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.System, displayName = "PlayStation")]
        public TranslatedFiveLaneState state;
    }

    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitLayout), displayName = "PlayStation 3 Guitar Hero Drumkit")]
    internal class PS3FiveLaneDrumkit : TranslatingFiveLaneDrumkit<PS3FiveLaneDrumkitState_NoReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3FiveLaneDrumkit_ReportId, PS3FiveLaneDrumkit>(0x12BA, 0x0120);
        }
    }

    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitLayout), displayName = "PlayStation 3 Guitar Hero Drumkit", hideInUI = true)]
    internal class PS3FiveLaneDrumkit_ReportId : TranslatingFiveLaneDrumkit<PS3FiveLaneDrumkitState_ReportId> { }
}
