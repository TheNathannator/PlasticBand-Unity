using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitState), displayName = "Santroller XInput Guitar Hero Drumkit")]
    internal class SantrollerXInputFiveLaneDrumkit : XInputFiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputFiveLaneDrumkit>(
                SantrollerDeviceType.Drums, SantrollerRhythmType.GuitarHero);
        }
    }
}
