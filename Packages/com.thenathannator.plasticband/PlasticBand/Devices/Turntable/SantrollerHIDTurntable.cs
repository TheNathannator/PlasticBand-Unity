using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/PS3.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3TurntableState_ReportId), displayName = "Santroller HID Turntable")]
    internal class SantrollerHIDTurntable : PS3Turntable
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDTurntable>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.DjHeroTurntable, null, nameof(SantrollerHIDTurntable));
        }
    }
}
