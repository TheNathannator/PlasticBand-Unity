using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/Xbox%20One.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XboxOneSixFretGuitarState : ISixFretGuitarState, IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x21;

        public byte reportId;
        // This state is identical to that of the PS3/Wii U GHL guitar
        public PS3WiiUSixFretGuitarState_NoReportId baseState;

        public bool black1 { get => baseState.black1; set => baseState.black1 = value; }
        public bool black2 { get => baseState.black2; set => baseState.black2 = value; }
        public bool black3 { get => baseState.black3; set => baseState.black3 = value; }
        public bool white1 { get => baseState.white1; set => baseState.white1 = value; }
        public bool white2 { get => baseState.white2; set => baseState.white2 = value; }
        public bool white3 { get => baseState.white3; set => baseState.white3 = value; }

        public bool dpadUp { get => baseState.dpadUp; set => baseState.dpadUp = value; }
        public bool dpadRight { get => baseState.dpadRight; set => baseState.dpadRight = value; }
        public bool dpadDown { get => baseState.dpadDown; set => baseState.dpadDown = value; }
        public bool dpadLeft { get => baseState.dpadLeft; set => baseState.dpadLeft = value; }

        public bool select { get => baseState.select; set => baseState.select = value; }
        public bool start { get => baseState.start; set => baseState.start = value; }
        public bool ghtv { get => baseState.ghtv; set => baseState.ghtv = value; }
        public bool system { get => baseState.system; set => baseState.system = value; }

        public bool strumUp { get => baseState.strumUp; set => baseState.strumUp = value; }
        public bool strumDown { get => baseState.strumDown; set => baseState.strumDown = value; }

        public byte whammy { get => baseState.whammy; set => baseState.whammy = value; }
        public sbyte tilt { get => baseState.tilt; set => baseState.tilt = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneSixFretGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedSixFretState.Format;

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedSixFretButton.System, displayName = "Guide")]
        public TranslatedSixFretState state;
    }

    [InputControlLayout(stateType = typeof(XboxOneSixFretGuitarLayout), displayName = "Xbox One SixFret Guitar")]
    internal class XboxOneSixFretGuitar : TranslatingSixFretGuitar<XboxOneSixFretGuitarState>,
        IInputStateCallbackReceiver
    {
        internal new static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneSixFretGuitar>(0x1430, 0x079B);
        }

        // Magic data to be sent periodically to unlock full input data
        internal static PS3OutputCommand s_PokeCommand = new PS3OutputCommand(
            0x22,
            0x02,
            new byte[PS3OutputCommand.kDataSize] { 0x08, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00 }
        )
        {
            baseCommand = new InputDeviceCommand(GameInputDefinitions.OutputFormat, PS3OutputCommand.kSize),
        };

        protected override void FinishSetup()
        {
            base.FinishSetup();
            GameInputStateTranslator<XboxOneSixFretGuitarState, TranslatedSixFretState>.VerifyDevice(this);
            m_Poker = new SixFretPoker<PS3OutputCommand>(this, s_PokeCommand);
        }

        private SixFretPoker<PS3OutputCommand> m_Poker;

        void IInputStateCallbackReceiver.OnNextUpdate() => m_Poker.OnUpdate();
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => GameInputStateTranslator<XboxOneSixFretGuitarState, TranslatedSixFretState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => GameInputStateTranslator<XboxOneSixFretGuitarState, TranslatedSixFretState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);
    }
}
