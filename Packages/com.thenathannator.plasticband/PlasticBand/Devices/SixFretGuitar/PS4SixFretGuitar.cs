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
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 63)]
    internal unsafe struct PS4SixFretGuitarState_NoReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte unused1;

        public byte strumBar;
        private readonly byte m_Whammy;
        private readonly byte m_Tilt;

        public PS4Button1 buttons1;
        public PS4Button2 buttons2;

        public bool dpadUp => buttons1.GetDpad().IsUp();
        public bool dpadRight => buttons1.GetDpad().IsRight();
        public bool dpadDown => buttons1.GetDpad().IsDown();
        public bool dpadLeft => buttons1.GetDpad().IsLeft();

        public bool start => (buttons1 & PS4Button1.Start) != 0;
        public bool ghtv => (buttons1 & PS4Button1.L3) != 0;
        public bool select => (buttons1 & PS4Button1.R3) != 0; // TODO: is this actually correct?
        public bool system => (buttons2 & PS4Button2.PlayStation) != 0;

        public bool white1 => (buttons1 & PS4Button1.Square) != 0;
        public bool black1 => (buttons1 & PS4Button1.Cross) != 0;
        public bool black2 => (buttons1 & PS4Button1.Circle) != 0;
        public bool black3 => (buttons1 & PS4Button1.Triangle) != 0;
        public bool white2 => (buttons1 & PS4Button1.L2) != 0;
        public bool white3 => (buttons1 & PS4Button1.R2) != 0;

        // The stick up/down values on PS4 controllers are inverted compared to what might be expected
        // 0x00 = max up, 0xFF = max down
        public bool strumUp => strumBar < 0x80;
        public bool strumDown => strumBar > 0x80;

        // Whammy ranges from 0x80 to 0xFF
        public byte whammy => (byte)((m_Whammy - 0x80) * 2);

        // Tilt is a 10-bit number centered at 0x200
        public sbyte tilt => (sbyte)(((m_Tilt & 0x3FF) >> 2) - 0x80);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4SixFretGuitarState_ReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4SixFretGuitarState_NoReportId state;

        public bool dpadUp => state.dpadUp;
        public bool dpadRight => state.dpadRight;
        public bool dpadDown => state.dpadDown;
        public bool dpadLeft => state.dpadLeft;

        public bool select => state.select;
        public bool start => state.start;
        public bool ghtv => state.ghtv;
        public bool system => state.system;

        public bool black1 => state.black1;
        public bool black2 => state.black2;
        public bool black3 => state.black3;
        public bool white1 => state.white1;
        public bool white2 => state.white2;
        public bool white3 => state.white3;

        public bool strumUp => state.strumUp;
        public bool strumDown => state.strumDown;

        public byte whammy => state.whammy;
        public sbyte tilt => state.tilt;
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