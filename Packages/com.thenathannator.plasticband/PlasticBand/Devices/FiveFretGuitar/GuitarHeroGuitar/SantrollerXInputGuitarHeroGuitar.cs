using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/Xbox%20360.md
namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputGuitarHeroGuitarState), displayName = "Santroller XInput Guitar Hero Guitar")]
    internal class SantrollerXInputGuitarHeroGuitar : XInputGuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputGuitarHeroGuitar>(XInputController.DeviceSubType.GuitarAlternate);
        }
    }
}
