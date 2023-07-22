using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Guitar/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiProGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "System")]
        public PS3ProGuitarState_NoReportId state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiProGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public WiiProGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(WiiProGuitarState_NoReportId), displayName = "Wii Rock Band Pro Guitar")]
    internal class WiiProGuitar : ProGuitar
    {
        internal new static void Initialize()
        {
            // Mustang
            HidLayoutFinder.RegisterLayout<WiiProGuitar_ReportId, WiiProGuitar>(0x1BAD, 0x3430);

            // MIDI Pro Adapter (Mustang)
            HidLayoutFinder.RegisterLayout<WiiProGuitar_ReportId, WiiProGuitar>(0x1BAD, 0x3438);

            // Squire
            HidLayoutFinder.RegisterLayout<WiiProGuitar_ReportId, WiiProGuitar>(0x1BAD, 0x3530);

            // MIDI Pro Adapter (Squire)
            HidLayoutFinder.RegisterLayout<WiiProGuitar_ReportId, WiiProGuitar>(0x1BAD, 0x3538);
        }
    }

    [InputControlLayout(stateType = typeof(WiiProGuitarState_ReportId), hideInUI = true)]
    internal class WiiProGuitar_ReportId : WiiProGuitar { }
}