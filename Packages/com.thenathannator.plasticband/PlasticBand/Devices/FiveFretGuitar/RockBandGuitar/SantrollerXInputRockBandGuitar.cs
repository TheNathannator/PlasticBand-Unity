using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputRockBandGuitarState), displayName = "Santroller XInput Rock Band Guitar")]
    internal class SantrollerXInputRockBandGuitar : XInputRockBandGuitar
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputRockBandGuitar>(XInputController.DeviceSubType.Guitar);
        }
    }
}
