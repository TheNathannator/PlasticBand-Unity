using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/PS3%20and%20Wii%20U.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS3WiiUSixFretGuitarState_NoReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        public PS3Button buttons;

        [FieldOffset(2)]
        public HidDpad dpad;

        [FieldOffset(4)]
        public byte strumBar;

        [FieldOffset(6)]
        private byte m_Whammy;

        [FieldOffset(19)]
        private short m_Tilt;

        public bool white1
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

        public bool black1
        {
            get => (buttons & PS3Button.Cross) != 0;
            set => buttons.SetBit(PS3Button.Cross, value);
        }

        public bool black2
        {
            get => (buttons & PS3Button.Circle) != 0;
            set => buttons.SetBit(PS3Button.Circle, value);
        }

        public bool black3
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool white2
        {
            get => (buttons & PS3Button.L2) != 0;
            set => buttons.SetBit(PS3Button.L2, value);
        }

        public bool white3
        {
            get => (buttons & PS3Button.R2) != 0;
            set => buttons.SetBit(PS3Button.R2, value);
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

        public bool ghtv
        {
            get => (buttons & PS3Button.L3) != 0;
            set => buttons.SetBit(PS3Button.L3, value);
        }

        // The stick up/down values on PS3 controllers are inverted compared to what might be expected
        // 0x00 = max up, 0xFF = max down
        public bool strumUp
        {
            get => strumBar < 0x80;
            set => strumBar = value ? (byte)0x00 : (byte)0x80;
        }

        public bool strumDown
        {
            get => strumBar > 0x80;
            set => strumBar = value ? (byte)0xFF : (byte)0x80;
        }

        // TODO: Needs verification
        // Whammy ranges from 0x80 to 0xFF
        public byte whammy
        {
            get => (byte)((m_Whammy - 0x80) * 2);
            set => m_Whammy = (byte)((value / 2) + 0x80);
        }

        // Tilt is a 10-bit number centered at 0x200
        public sbyte tilt
        {
            get => (sbyte)(((m_Tilt & 0x3FF) >> 2) - 0x80);
            set => m_Tilt = (short)((value + 0x80) << 2);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiUSixFretGuitarState_ReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiUSixFretGuitarState_NoReportId state;

        public bool black1 { get => state.black1; set => state.black1 = value; }
        public bool black2 { get => state.black2; set => state.black2 = value; }
        public bool black3 { get => state.black3; set => state.black3 = value; }
        public bool white1 { get => state.white1; set => state.white1 = value; }
        public bool white2 { get => state.white2; set => state.white2 = value; }
        public bool white3 { get => state.white3; set => state.white3 = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }

        public bool select { get => state.select; set => state.select = value; }
        public bool start { get => state.start; set => state.start = value; }
        public bool ghtv { get => state.ghtv; set => state.ghtv = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool strumUp { get => state.strumUp; set => state.strumUp = value; }
        public bool strumDown { get => state.strumDown; set => state.strumDown = value; }

        public byte whammy { get => state.whammy; set => state.whammy = value; }
        public sbyte tilt { get => state.tilt; set => state.tilt = value; }
    }

    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "PS3/Wii U Guitar Hero Live Guitar")]
    internal class PS3WiiUSixFretGuitar : TranslatingSixFretGuitar<PS3WiiUSixFretGuitarState_NoReportId>,
        IInputUpdateCallbackReceiver
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3WiiUSixFretGuitar_ReportId, PS3WiiUSixFretGuitar>(0x12BA, 0x074B);
        }

        // Magic data to be sent periodically to unlock full input data
        internal static PS3OutputCommand s_PokeCommand = new PS3OutputCommand(
            0x02, // TODO: Determine if this report ID is correct/necessary
            0x02,
            new byte[PS3OutputCommand.kDataSize] { 0x08, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Poker = new SixFretPoker<PS3OutputCommand>(this, s_PokeCommand);
        }

        private SixFretPoker<PS3OutputCommand> m_Poker;

        void IInputUpdateCallbackReceiver.OnUpdate() => m_Poker.OnUpdate();
    }

    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "PS3/Wii U Guitar Hero Live Guitar", hideInUI = true)]
    internal class PS3WiiUSixFretGuitar_ReportId : TranslatingSixFretGuitar<PS3WiiUSixFretGuitarState_ReportId>,
        IInputUpdateCallbackReceiver
    {
        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Poker = new SixFretPoker<PS3OutputCommand>(this, PS3WiiUSixFretGuitar.s_PokeCommand);
        }

        private SixFretPoker<PS3OutputCommand> m_Poker;

        void IInputUpdateCallbackReceiver.OnUpdate() => m_Poker.OnUpdate();
    }
}