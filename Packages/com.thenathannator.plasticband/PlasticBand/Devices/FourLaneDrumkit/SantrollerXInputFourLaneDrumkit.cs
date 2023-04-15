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
    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitState), displayName = "Santroller device in XInput FourLaneDrum mode")]
    internal class SantrollerXInputFourLaneDrumkit : XInputFourLaneDrumkit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputFourLaneDrumkit>(XInputController.DeviceSubType.DrumKit);
        }
    }
}
