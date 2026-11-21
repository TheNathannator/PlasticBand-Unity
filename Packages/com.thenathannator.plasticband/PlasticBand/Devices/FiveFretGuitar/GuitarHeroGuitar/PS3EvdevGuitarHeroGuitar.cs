#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Mappings follow hid-sony's instrument mapping.
    // Not supported (requires state translation): touch/slider bar
    [InputControlLayout(displayName = "PS3 Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class PS3EvdevGuitarHeroGuitar : GuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonyCross, "greenFret")
                .WithKey(SonyCircle, "redFret")
                .WithKey(SonySquare, "yellowFret")
                .WithKey(SonyTriangle, "blueFret")
                .WithKey(SonyButton5, "orangeFret")
                .WithKey(SonyButton6, "spPedal")
                .WithAbs(input_abs_code.ABS_Z, "whammy")
                .WithAbs(input_abs_code.ABS_RY, Inverted("tilt"));

            // RedOctane Guitar Hero guitar dongle for PS3
            RegisterLayout<PS3EvdevGuitarHeroGuitar>(mapping, 0x12BA, 0x0100);
            // RedOctane Guitar Hero guitar dongle for PC
            RegisterLayout<PS3EvdevGuitarHeroGuitar>(mapping, 0x1430, 0x474C);
        }
    }
}
#endif
