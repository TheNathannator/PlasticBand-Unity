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
    [InputControlLayout(stateType = typeof(XInputGuitarHeroGuitarState), displayName = "Santroller XInput Guitar Hero Guitar")]
    internal class SantrollerXInputGuitarHeroGuitar : XInputGuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputGuitarHeroGuitar>(XInputController.DeviceSubType.GuitarAlternate);
        }
    }
}
