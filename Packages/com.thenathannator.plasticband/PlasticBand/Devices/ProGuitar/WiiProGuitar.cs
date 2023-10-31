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
    internal struct WiiProGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedProGuitarState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedProGuitarButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedProGuitarButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedProGuitarButton.West, displayName = "1")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedProGuitarButton.North, displayName = "2")]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedProGuitarButton.Start, displayName = "Plus", shortDisplayName = "+")]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedProGuitarButton.Select, displayName = "Minus", shortDisplayName = "-")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedProGuitarButton.System, displayName = "System")]
        public TranslatedProGuitarState state;
    }

    [InputControlLayout(stateType = typeof(WiiProGuitarLayout), displayName = "Wii Rock Band Pro Guitar")]
    internal class WiiProGuitar : TranslatingProGuitar<PS3WiiProGuitarState_NoReportId>
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

    [InputControlLayout(stateType = typeof(WiiProGuitarLayout), displayName = "Wii Rock Band Pro Guitar", hideInUI = true)]
    internal class WiiProGuitar_ReportId : TranslatingProGuitar<PS3WiiProGuitarState_ReportId> { }
}