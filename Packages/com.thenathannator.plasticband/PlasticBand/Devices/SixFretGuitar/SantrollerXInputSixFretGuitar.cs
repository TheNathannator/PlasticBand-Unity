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
    [InputControlLayout(stateType = typeof(XInputSixFretGuitarState), displayName = "Santroller XInput Guitar Hero Live Guitar")]
    internal class SantrollerXInputSixFretGuitar : XInputSixFretGuitar
    {
        internal new static void Initialize()
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<SantrollerXInputSixFretGuitar>(matches: new InputDeviceMatcher()
                // Annoyingly, GHL guitars do not have a unique subtype. So, we have to use some other information to identify them.
                .WithInterface(XInputOther.kInterfaceName)
                .WithCapability("subType", XInputController.DeviceSubType.GuitarAlternate)
                .WithCapability("leftStickX", SantrollerLayoutFinder.SantrollerVendorID)
                .WithCapability("leftStickY", SantrollerLayoutFinder.SantrollerProductID)
                // so we use the flags as the distinguisher.
                .WithCapability("flags", (int)(XInputFlags.VoiceSupported | XInputFlags.PluginModulesSupported | XInputFlags.NoNavigation)) // 28
            );
            #endif
        }
    }
}
