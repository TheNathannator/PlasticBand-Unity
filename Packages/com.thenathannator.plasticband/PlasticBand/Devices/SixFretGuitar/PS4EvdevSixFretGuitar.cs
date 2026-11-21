#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Mappings follow hid-sony's instrument mapping.
    // Not supported (requires state translation): strum, which is reported on the left stick Y axis
    // Whammy rests at the center of its axis, with only the upper half being used.
    [InputControlLayout(displayName = "PS4 Evdev Guitar Hero Live Guitar", hideInUI = true)]
    internal class PS4EvdevSixFretGuitar : SixFretGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonySquare, "white1")
                .WithKey(SonyCross, "black1")
                .WithKey(SonyCircle, "black2")
                .WithKey(SonyTriangle, "black3")
                .WithKey(SonyButton5, "white2")
                .WithKey(SonyButton6, "white3")
                .WithKey(input_key_code.BTN_THUMB_L, "ghtvButton")
                .WithAbs(input_abs_code.ABS_Z, "whammy")
                .WithAbs(input_abs_code.ABS_RZ, "tilt");

            // Activision Guitar Hero Live dongle for PS4
            RegisterLayout<PS4EvdevSixFretGuitar>(mapping, 0x1430, 0x07BB);
        }
    }
}
#endif
