using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4SixFretGuitarState_NoReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte unused1;

        public byte strumBar;
        private byte m_Whammy;
        private byte m_Tilt;

        public PS4Button1 buttons1;
        public PS4Button2 buttons2;

        public bool white1
        {
            get => (buttons1 & PS4Button1.Square) != 0;
            set => buttons1.SetBit(PS4Button1.Square, value);
        }

        public bool black1
        {
            get => (buttons1 & PS4Button1.Cross) != 0;
            set => buttons1.SetBit(PS4Button1.Cross, value);
        }

        public bool black2
        {
            get => (buttons1 & PS4Button1.Circle) != 0;
            set => buttons1.SetBit(PS4Button1.Circle, value);
        }

        public bool black3
        {
            get => (buttons1 & PS4Button1.Triangle) != 0;
            set => buttons1.SetBit(PS4Button1.Triangle, value);
        }

        public bool white2
        {
            get => (buttons1 & PS4Button1.L2) != 0;
            set => buttons1.SetBit(PS4Button1.L2, value);
        }

        public bool white3
        {
            get => (buttons1 & PS4Button1.R2) != 0;
            set => buttons1.SetBit(PS4Button1.R2, value);
        }

        public bool dpadUp
        {
            get => buttons1.GetDpad().IsUp();
            set => buttons1.SetDpadUp(value);
        }

        public bool dpadRight
        {
            get => buttons1.GetDpad().IsRight();
            set => buttons1.SetDpadRight(value);
        }

        public bool dpadDown
        {
            get => buttons1.GetDpad().IsDown();
            set => buttons1.SetDpadDown(value);
        }

        public bool dpadLeft
        {
            get => buttons1.GetDpad().IsLeft();
            set => buttons1.SetDpadLeft(value);
        }

        public bool select
        {
            get => (buttons1 & PS4Button1.R3) != 0; // TODO: is this actually correct?
            set => buttons1.SetBit(PS4Button1.R3, value);
        }

        public bool start
        {
            get => (buttons1 & PS4Button1.Start) != 0;
            set => buttons1.SetBit(PS4Button1.Start, value);
        }

        public bool system
        {
            get => (buttons2 & PS4Button2.PlayStation) != 0;
            set => buttons2.SetBit(PS4Button2.PlayStation, value);
        }

        public bool ghtv
        {
            get => (buttons1 & PS4Button1.L3) != 0;
            set => buttons1.SetBit(PS4Button1.L3, value);
        }

        // The stick up/down values on PS4 controllers are inverted compared to what might be expected
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

        // Whammy ranges from 0x80 to 0xFF
        public byte whammy
        {
            get => (byte)((m_Whammy - 0x80) * 2);
            set => m_Whammy = (byte)((value / 2) + 0x80);
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt - 0x80);
            set => m_Tilt = (byte)(value + 0x80);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4SixFretGuitarState_ReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4SixFretGuitarState_NoReportId state;

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

    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "PlayStation 4 Guitar Hero Live Guitar", hideInUI = true)]
    internal class PS4SixFretGuitar_NoReportId : TranslatingSixFretGuitar<PS4SixFretGuitarState_NoReportId>,
        IInputUpdateCallbackReceiver
    {
        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Poker = new SixFretPoker<PS3OutputCommand>(this, PS4SixFretGuitar.s_PokeCommand);
        }

        private SixFretPoker<PS3OutputCommand> m_Poker;

        void IInputUpdateCallbackReceiver.OnUpdate() => m_Poker.OnUpdate();
    }

    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "PlayStation 4 Guitar Hero Live Guitar")]
    internal class PS4SixFretGuitar : TranslatingSixFretGuitar<PS4SixFretGuitarState_ReportId>,
        IInputUpdateCallbackReceiver
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS4SixFretGuitar, PS4SixFretGuitar_NoReportId>(0x1430, 0x07BB, reportIdDefault: true);
        }

        // Magic data to be sent periodically to unlock full input data.
        internal static PS3OutputCommand s_PokeCommand = new PS3OutputCommand(
            0x30, // TODO: Determine if this report ID is correct/necessary
            0x02,
            new byte[PS3OutputCommand.kDataSize] { 0x08, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Poker = new SixFretPoker<PS3OutputCommand>(this, s_PokeCommand);
        }

        private SixFretPoker<PS3OutputCommand> m_Poker;

        void IInputUpdateCallbackReceiver.OnUpdate() => m_Poker.OnUpdate();
    }
}