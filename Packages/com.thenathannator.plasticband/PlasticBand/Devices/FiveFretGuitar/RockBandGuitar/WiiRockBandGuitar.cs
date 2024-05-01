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
    internal struct WiiRockBandGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedRockBandGuitarState.Format;

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.Start, displayName = "Plus", shortDisplayName = "+")]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.Select, displayName = "Minus", shortDisplayName = "-")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.System, displayName = "System")]
        public TranslatedRockBandGuitarState state;
    }

    [InputControlLayout(stateType = typeof(WiiRockBandGuitarLayout), displayName = "Wii Rock Band Guitar")]
    internal class WiiRockBandGuitar : TranslatingRockBandGuitar_Flags_NullState<PS3WiiRockBandGuitarState_NoReportId>
    {
        internal new static void Initialize()
        {
            // RB1 guitars
            HidLayoutFinder.RegisterLayout<WiiRockBandGuitar_ReportId, WiiRockBandGuitar>(0x1BAD, 0x0004);

            // RB2 and later
            HidLayoutFinder.RegisterLayout<WiiRockBandGuitar_ReportId, WiiRockBandGuitar>(0x1BAD, 0x3010);
        }
    }

    [InputControlLayout(stateType = typeof(WiiRockBandGuitarLayout), displayName = "Wii Rock Band Guitar", hideInUI = true)]
    internal class WiiRockBandGuitar_ReportId : TranslatingRockBandGuitar_Flags_NullState<PS3WiiRockBandGuitarState_ReportId> { }
}