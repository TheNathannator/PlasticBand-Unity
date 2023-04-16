using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/Xbox%20360.md

namespace PlasticBand.Devices
{
    using XInputFlags = XInputController.DeviceFlags;

    [InputControlLayout(stateType = typeof(XInputSixFretGuitarState), displayName = "Santroller XInput Guitar Hero Live Guitar")]
    internal class SantrollerXInputSixFretGuitar : XInputSixFretGuitar
    {
        internal new static void Initialize()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<SantrollerXInputSixFretGuitar>(matches:
                SantrollerLayoutFinder.GetXInputMatcher((int)XInputController.DeviceSubType.GuitarAlternate)
                // Annoyingly, GHL guitars do not have a unique subtype. So, we have to use some other information to identify them.
                // so we use the flags as the distinguisher.
                .WithCapability("flags", (int)(XInputFlags.VoiceSupported | XInputFlags.PluginModulesSupported | XInputFlags.NoNavigation)) // 28
            );
#endif
        }
    }
}
