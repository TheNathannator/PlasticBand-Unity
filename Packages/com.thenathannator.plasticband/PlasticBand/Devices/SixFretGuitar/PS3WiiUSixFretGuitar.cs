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
        private readonly byte m_Whammy;

        [FieldOffset(19)]
        private readonly short m_Tilt;

        public bool dpadUp => dpad.IsUp();
        public bool dpadRight => dpad.IsRight();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();

        public bool select => (buttons & PS3Button.Select) != 0;
        public bool start => (buttons & PS3Button.Start) != 0;
        public bool ghtv => (buttons & PS3Button.L3) != 0;
        public bool system => (buttons & PS3Button.PlayStation) != 0;

        public bool white1 => (buttons & PS3Button.Square) != 0;
        public bool black1 => (buttons & PS3Button.Cross) != 0;
        public bool black2 => (buttons & PS3Button.Circle) != 0;
        public bool black3 => (buttons & PS3Button.Triangle) != 0;
        public bool white2 => (buttons & PS3Button.L2) != 0;
        public bool white3 => (buttons & PS3Button.R2) != 0;

        // The stick up/down values on PS3 controllers are inverted compared to what might be expected
        // 0x00 = max up, 0xFF = max down
        public bool strumUp => strumBar < 0x80;
        public bool strumDown => strumBar > 0x80;

        // TODO: Needs verification
        // Whammy ranges from 0x80 to 0xFF
        public byte whammy => (byte)((m_Whammy - 0x80) * 2);

        // Tilt is a 10-bit number centered at 0x200
        public sbyte tilt => (sbyte)(((m_Tilt & 0x3FF) >> 2) - 0x80);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiUSixFretGuitarState_ReportId : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiUSixFretGuitarState_NoReportId state;

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