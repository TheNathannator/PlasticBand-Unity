using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/PS4.md

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4SixFretGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte unused1;

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x7F,maxValue=0,nullValue=0x80")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BYTE", parameters = "minValue=0x81,maxValue=0xFF,nullValue=0x80")]
        public byte strumBar;

        [InputControl(name = "whammy", layout = "Axis", parameters = "normalize,normalizeMin=0.5,normalizeMax=1,normalizeZero=0.5")]
        public byte whammy;

        [InputControl(name = "tilt", layout = "Axis", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte tilt;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "white1", layout = "Button", bit = 4)]
        [InputControl(name = "black1", layout = "Button", bit = 5)]
        [InputControl(name = "black2", layout = "Button", bit = 6)]
        [InputControl(name = "black3", layout = "Button", bit = 7)]
        [InputControl(name = "white2", layout = "Button", bit = 8)]
        [InputControl(name = "white3", layout = "Button", bit = 9)]

        [InputControl(name = "startButton", layout = "Button", bit = 13)]
        [InputControl(name = "ghtvButton", layout = "Button", bit = 14)]
        [InputControl(name = "selectButton", layout = "Button", bit = 15)]
        public ushort buttons1;

        [InputControl(name = "dpadCenter", layout = "Button", bit = 0)]
        public byte button2;

        public fixed byte unused2[57];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4SixFretGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4SixFretGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS4SixFretGuitarState_NoReportId), hideInUI = true)]
    internal class PS4SixFretGuitar_NoReportId : PS4SixFretGuitar { }

    [InputControlLayout(stateType = typeof(PS4SixFretGuitarState_ReportId), hideInUI = true)]
    internal class PS4SixFretGuitar_ReportId : PS4SixFretGuitar { }
}

namespace PlasticBand.Devices
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && HIDROGEN_FORCE_REPORT_IDS)
    using DefaultState = PS4SixFretGuitarState_ReportId;
#else
    using DefaultState = PS4SixFretGuitarState_NoReportId;
#endif

    [InputControlLayout(stateType = typeof(DefaultState), displayName = "PlayStation 4 Guitar Hero Live Guitar")]
    internal class PS4SixFretGuitar : PokedSixFretGuitar
    {
        internal new static void Initialize()
        {
            HidReportIdLayoutFinder.RegisterLayout<PS4SixFretGuitar,
                PS4SixFretGuitar_ReportId, PS4SixFretGuitar_NoReportId>(0x1430, 0x07BB);
        }

        // Magic data to be sent periodically to unlock full input data.
        private static PS3OutputCommand s_PokeCommand = new PS3OutputCommand(
            0x30, // TODO: Determine if this report ID is correct/necessary
            0x02,
            new byte[PS3OutputCommand.kDataSize] { 0x08, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        protected override void OnPoke() => device.ExecuteCommand(ref s_PokeCommand);
    }
}