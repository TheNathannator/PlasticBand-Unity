using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitState_ReportId), displayName = "Santroller device in Guitar Hero Drum Mode")]
    internal class SantrollerHIDFiveLaneDrumkit : FiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDFiveLaneDrumkit>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.Drums, SantrollerRhythmType.RockBand, nameof(SantrollerHIDFiveLaneDrumkit));
        }
    }
}
