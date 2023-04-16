using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputTurntableState), displayName = "Santroller XInput Turntable")]
    internal class SantrollerXInputTurntable : XInputTurntable
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputTurntable>(XInputNonStandardSubType.Turntable);
        }
    }
}
