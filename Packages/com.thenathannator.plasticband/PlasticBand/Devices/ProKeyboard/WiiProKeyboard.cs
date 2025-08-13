using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Keyboard/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiProKeyboardState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "1 Button", shortDisplayName = "1")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "A Button", shortDisplayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "B Button", shortDisplayName = "B")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "2 Button", shortDisplayName = "2")]
        [InputControl(name = "selectButton", layout = "Button", bit = 8, displayName = "Minus", shortDisplayName = "-")]
        [InputControl(name = "startButton", layout = "Button", bit = 9, displayName = "Plus", shortDisplayName = "+")]
        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "System")]
        public PS3ProKeyboardState_NoReportId state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiProKeyboardState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public WiiProKeyboardState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(WiiProKeyboardState_NoReportId), displayName = "Wii Rock Band Pro Keyboard")]
    internal class WiiProKeyboard : ProKeyboard
    {
        internal new static void Initialize()
        {
            // Keyboard
            HidLayoutFinder.RegisterLayout<WiiProKeyboard_ReportId, WiiProKeyboard>(0x1BAD, 0x3330);

            // MIDI Pro Adapter
            HidLayoutFinder.RegisterLayout<WiiProKeyboard_ReportId, WiiProKeyboard>(0x1BAD, 0x3338);
        }
    }

    [InputControlLayout(stateType = typeof(WiiProKeyboardState_ReportId), displayName = "Wii Rock Band Pro Keyboard", hideInUI = true)]
    internal class WiiProKeyboard_ReportId : WiiProKeyboard { }
}