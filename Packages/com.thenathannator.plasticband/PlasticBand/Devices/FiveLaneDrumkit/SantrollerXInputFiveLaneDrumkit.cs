using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitState), displayName = "Santroller XInput Guitar Hero Drum Kit")]
    internal class SantrollerXInputFiveLaneDrumkit : XInputFiveLaneDrumkit
    {
        internal new static void Initialize()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // 4-lane kits and 5-lane kits share the same subtype, they need to be differentiated in another way
            // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
            // realistically that isn't likely to be held when powering on
            XInputLayoutFinder.RegisterLayout<SantrollerXInputFiveLaneDrumkit>(XInputController.DeviceSubType.DrumKit,
                (state) => (state.buttons & (ushort)XInputGamepad.Button.LeftThumb) != 0,
                SantrollerLayoutFinder.GetXInputMatcher((int)XInputController.DeviceSubType.DrumKit)
            );
#endif
        }
    }
}
