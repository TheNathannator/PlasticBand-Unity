using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/PS3.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3TurntableState_ReportId), displayName = "Santroller HID Turntable")]
    internal class SantrollerHIDTurntable : PS3Turntable
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDTurntable>(SantrollerDeviceType.DjHeroTurntable);
        }
    }
}
