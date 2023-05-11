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
            // Annoyingly, GHL guitars do not have a unique subtype. So, we have to use some other information to identify them.
            XInputLayoutFinder.RegisterLayout<XInputSixFretGuitar>(XInputController.DeviceSubType.GuitarAlternate,
                // Strangely, they report the No Navigation flag. Most likely none of the other guitars report this information,
                // so we check for it here.
                (capabilities, state) => (capabilities.flags & XInputController.DeviceFlags.NoNavigation) != 0,
                SantrollerLayoutFinder.GetXInputMatcher((int)XInputController.DeviceSubType.GuitarAlternate));
        }
    }
}
