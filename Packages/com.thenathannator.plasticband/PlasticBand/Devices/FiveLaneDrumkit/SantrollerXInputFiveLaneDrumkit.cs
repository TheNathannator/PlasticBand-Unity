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
    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitState), displayName = "Santroller device in XInput FiveLaneDrumkit mode")]
    internal class SantrollerXInputFiveLaneDrumkit : XInputFiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<SantrollerXInputFiveLaneDrumkit>();
            // 4-lane kits and 5-lane kits share the same subtype, they need to be differentiated in another way
            // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
            // realistically that isn't likely to be held when powering on
            // May be some more specific capability data that also distinguishes them, but that probably isn't reliable
            XInputLayoutFixup.RegisterLayoutResolver(XInputController.DeviceSubType.DrumKit, (capabilities, state) => {
                if (capabilities.gamepad.leftStickX == SantrollerLayoutFinder.SantrollerVendorID && capabilities.gamepad.leftStickY = SantrollerLayoutFinder.SantrollerProductID && (state.buttons & (ushort)XInputGamepad.Button.LeftThumb) != 0)
                    return typeof(SantrollerXInputFiveLaneDrumkit).Name;

                return null;
            });
            #endif
        }
    }
}
