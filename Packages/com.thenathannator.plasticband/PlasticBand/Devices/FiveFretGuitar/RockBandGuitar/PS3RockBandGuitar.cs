using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiRockBandGuitarState_NoReportId : IRockBandGuitarState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public PS3Button buttons;
        public HidDpad dpad;

        private fixed byte unused1[2];

        private byte m_Whammy;
        private byte m_PickupSwitch;

        public bool green
        {
            get => (buttons & PS3Button.Cross) != 0;
            set => buttons.SetBit(PS3Button.Cross, value);
        }

        public bool red
        {
            get => (buttons & PS3Button.Circle) != 0;
            set => buttons.SetBit(PS3Button.Circle, value);
        }

        public bool yellow
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool blue
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

        public bool orange
        {
            get => (buttons & PS3Button.L2) != 0;
            set => buttons.SetBit(PS3Button.L2, value);
        }

        public bool solo
        {
            get => (buttons & PS3Button.L1) != 0;
            set => buttons.SetBit(PS3Button.L1, value);
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

        public byte whammy
        {
            get => m_Whammy;
            set => m_Whammy = RockBandGuitarState.EnsureNotNull(value);
        }

        public sbyte tilt
        {
            get => (buttons & PS3Button.R2) != 0 ? sbyte.MaxValue : (sbyte)0;
            set => buttons.SetBit(PS3Button.R2, value >= 64);
        }

        public int pickupSwitch
        {
            get => RockBandGuitarState.GetPickupSwitchNotch_NullState(m_PickupSwitch);
            set => m_PickupSwitch = RockBandGuitarState.SetPickupSwitchNotch_NullState(value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiRockBandGuitarState_ReportId : IRockBandGuitarState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiRockBandGuitarState_NoReportId state;

        public bool green { get => state.green; set => state.green = value; }
        public bool red { get => state.red; set => state.red = value; }
        public bool yellow { get => state.yellow; set => state.yellow = value; }
        public bool blue { get => state.blue; set => state.blue = value; }
        public bool orange { get => state.orange; set => state.orange = value; }
        public bool solo { get => state.solo; set => state.solo = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public byte whammy { get => state.whammy; set => state.whammy = value; }
        public sbyte tilt { get => state.tilt; set => state.tilt = value; }
        public int pickupSwitch { get => state.pickupSwitch; set => state.pickupSwitch = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3RockBandGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedRockBandGuitarState.Format;

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.System, displayName = "PlayStation")]
        public TranslatedRockBandGuitarState state;
    }

    [InputControlLayout(stateType = typeof(PS3RockBandGuitarLayout), displayName = "PlayStation 3 Rock Band Guitar")]
    internal class PS3RockBandGuitar : TranslatingRockBandGuitar_Flags_NullState<PS3WiiRockBandGuitarState_NoReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3RockBandGuitar_ReportId, PS3RockBandGuitar>(0x12BA, 0x0200);
        }
    }

    [InputControlLayout(stateType = typeof(PS3RockBandGuitarLayout), displayName = "PlayStation 3 Rock Band Guitar", hideInUI = true)]
    internal class PS3RockBandGuitar_ReportId : TranslatingRockBandGuitar_Flags_NullState<PS3WiiRockBandGuitarState_ReportId> { }
}
