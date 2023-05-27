using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/PS3%20and%20Wii%20U.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiUSixFretGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "white1", layout = "Button", bit = 0)]
        [InputControl(name = "black1", layout = "Button", bit = 1)]
        [InputControl(name = "black2", layout = "Button", bit = 2)]
        [InputControl(name = "black3", layout = "Button", bit = 3)]

        [InputControl(name = "white2", layout = "Button", bit = 4)]
        [InputControl(name = "white3", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]
        [InputControl(name = "ghtvButton", layout = "Button", bit = 10)]

        [InputControl(name = "syncButton", layout = "Button", bit = 12, displayName = "D-pad Center")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 15)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=15,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        public byte unused1;

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x7F,maxValue=0,nullValue=0x80")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x81,maxValue=0xFF,nullValue=0x80")]
        public byte strumBar;

        public byte unused2;

        // TODO: See if any normalization is needed
        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        public fixed byte unused3[12];

        // TODO: See if any additional normalization is needed
        [InputControl(name = "tilt", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short tilt;

        public fixed short unused4[3];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiUSixFretGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiUSixFretGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3WiiUSixFretGuitarState_NoReportId), displayName = "PS3/Wii U Guitar Hero Live Guitar")]
    internal class PS3WiiUSixFretGuitar : SixFretGuitar, IInputUpdateCallbackReceiver
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3WiiUSixFretGuitar_ReportId, PS3WiiUSixFretGuitar>(0x12BA, 0x074B);
        }

        // Magic data to be sent periodically to unlock full input data
        private static PS3OutputCommand s_PokeCommand = new PS3OutputCommand(
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

    [InputControlLayout(stateType = typeof(PS3WiiUSixFretGuitarState_ReportId), hideInUI = true)]
    internal class PS3WiiUSixFretGuitar_ReportId : PS3WiiUSixFretGuitar { }
}