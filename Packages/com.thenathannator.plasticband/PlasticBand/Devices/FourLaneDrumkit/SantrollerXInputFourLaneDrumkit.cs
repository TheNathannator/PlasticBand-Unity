using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitState), displayName = "Santroller XInput Rock Band Drum Kit")]
    internal class SantrollerXInputFourLaneDrumkit : XInputFourLaneDrumkit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputFourLaneDrumkit>(XInputController.DeviceSubType.DrumKit);
        }
    }
}
