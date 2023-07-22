using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Wii%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiFourLaneDrumkitState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "System")]
        public PS3FourLaneDrumkitState_NoReportId state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiFourLaneDrumkitState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public WiiFourLaneDrumkitState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(WiiFourLaneDrumkitState_NoReportId), displayName = "Wii Rock Band Drumkit")]
    internal class WiiFourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            // RB1
            HidLayoutFinder.RegisterLayout<WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit>(0x1BAD, 0x0005);

            // RB2 and later
            HidLayoutFinder.RegisterLayout<WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit>(0x1BAD, 0x3110);

            // MIDI Pro Adapter
            HidLayoutFinder.RegisterLayout<WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit>(0x1BAD, 0x3118);
        }
    }

    [InputControlLayout(stateType = typeof(WiiFourLaneDrumkitState_ReportId), hideInUI = true)]
    internal class WiiFourLaneDrumkit_ReportId : WiiFourLaneDrumkit { }
}