using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputSixFretGuitarState), displayName = "Santroller XInput Guitar Hero Live Guitar")]
    internal class SantrollerXInputSixFretGuitar : XInputSixFretGuitar
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<XInputSixFretGuitar>(XInputController.DeviceSubType.GuitarAlternate,
                SantrollerDeviceType.LiveGuitar);
        }
    }
}
