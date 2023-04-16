using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState_ReportId), displayName = "Santroller HID Rock Band Drum Kit")]
    internal class SantrollerHIDFourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDFourLaneDrumkit>(
                SantrollerDeviceType.Drums, SantrollerRhythmType.RockBand);
        }
    }
}
