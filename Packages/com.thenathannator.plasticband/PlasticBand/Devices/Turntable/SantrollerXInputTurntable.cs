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
    [InputControlLayout(stateType = typeof(XInputTurntableState), displayName = "Santroller XInput Turntable")]
    internal class SantrollerXInputTurntable : XInputTurntable
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputTurntable>(XInputNonStandardSubType.Turntable);
        }
    }
}
