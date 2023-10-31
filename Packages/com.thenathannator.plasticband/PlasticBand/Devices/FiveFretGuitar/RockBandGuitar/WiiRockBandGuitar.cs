using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiRockBandGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "selectButton", layout = "Button", bit = 8, displayName = "Minus", shortDisplayName = "-")]
        [InputControl(name = "startButton", layout = "Button", bit = 9, displayName = "Plus", shortDisplayName = "+")]
        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "System")]
        public PS3RockBandGuitarState_NoReportId state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiRockBandGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public WiiRockBandGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(WiiRockBandGuitarState_NoReportId), displayName = "Wii Rock Band Guitar")]
    internal class WiiRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            // RB1 guitars
            HidLayoutFinder.RegisterLayout<WiiRockBandGuitar_ReportId, WiiRockBandGuitar>(0x1BAD, 0x0004);

            // RB2 and later
            HidLayoutFinder.RegisterLayout<WiiRockBandGuitar_ReportId, WiiRockBandGuitar>(0x1BAD, 0x3010);
        }
    }

    [InputControlLayout(stateType = typeof(WiiRockBandGuitarState_ReportId), displayName = "Wii Rock Band Guitar", hideInUI = true)]
    internal class WiiRockBandGuitar_ReportId : WiiRockBandGuitar { }
}