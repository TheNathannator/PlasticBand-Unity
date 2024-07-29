using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct WiiFourLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFourLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFourLaneButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFourLaneButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFourLaneButton.West, displayName = "1")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFourLaneButton.North, displayName = "2")]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedFourLaneButton.Start, displayName = "Plus", shortDisplayName = "+")]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedFourLaneButton.Select, displayName = "Minus", shortDisplayName = "-")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFourLaneButton.System, displayName = "System")]
        public TranslatedFourLaneState state;
    }

    [InputControlLayout(stateType = typeof(WiiFourLaneDrumkitLayout), displayName = "Wii Rock Band Drumkit")]
    internal class WiiFourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_NoReportId>
    {
        internal new static void Initialize()
        {
            // RB1
            HidLayoutFinder.RegisterLayout<WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit>(0x1BAD, 0x0005);

            // RB2 and later
            HidLayoutFinder.RegisterLayout<WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit>(0x1BAD, 0x3110);

            // MIDI Pro Adapter
            HidLayoutFinder.RegisterLayout<WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit>(0x1BAD, 0x3138);
        }
    }

    [InputControlLayout(stateType = typeof(WiiFourLaneDrumkitLayout), displayName = "Wii Rock Band Drumkit", hideInUI = true)]
    internal class WiiFourLaneDrumkit_ReportId : TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_ReportId> { }
}