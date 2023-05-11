using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitState), displayName = "Santroller XInput Rock Band Drumkit")]
    internal class SantrollerXInputFourLaneDrumkit : XInputFourLaneDrumkit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputFourLaneDrumkit>(
                SantrollerDeviceType.Drums, SantrollerRhythmType.RockBand);
        }
    }
}
