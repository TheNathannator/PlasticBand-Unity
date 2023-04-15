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
    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState_ReportId), displayName = "Santroller device in Guitar Hero Drum Mode")]
    internal class SantrollerHIDFourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDFourLaneDrumkit>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.Drums, SantrollerRhythmType.GuitarHero, nameof(SantrollerHIDFourLaneDrumkit));
        }
    }
}
