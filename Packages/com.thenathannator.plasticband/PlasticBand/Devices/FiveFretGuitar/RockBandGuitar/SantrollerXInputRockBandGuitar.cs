using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputRockBandGuitarState), displayName = "Santroller XInput Rock Band Guitar")]
    internal class SantrollerXInputRockBandGuitar : XInputRockBandGuitar
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputRockBandGuitar>(XInputController.DeviceSubType.Guitar);
        }
    }
}
