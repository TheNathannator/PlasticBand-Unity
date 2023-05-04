using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/PS3.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitState_ReportId), displayName = "Santroller HID Guitar Hero Drumkit")]
    internal class SantrollerHIDFiveLaneDrumkit : FiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDFiveLaneDrumkit>(
                SantrollerDeviceType.Drums, SantrollerRhythmType.GuitarHero);
        }
    }
}
