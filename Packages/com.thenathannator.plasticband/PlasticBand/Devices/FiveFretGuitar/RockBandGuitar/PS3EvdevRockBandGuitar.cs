#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Mappings follow hid-sony's instrument mapping.
    // Not supported (requires state translation): solo frets, pickup switch
    // Tilt is a digital input on these guitars.
    [InputControlLayout(displayName = "PS3/Wii Evdev Rock Band Guitar", hideInUI = true)]
    internal class PS3EvdevRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonyCross, "greenFret")
                .WithKey(SonyCircle, "redFret")
                .WithKey(SonyTriangle, "yellowFret")
                .WithKey(SonySquare, "blueFret")
                .WithKey(SonyButton5, "orangeFret")
                .WithKey(SonyButton6, "tilt")
                .WithAbs(input_abs_code.ABS_Z, Unsigned("whammy"));

            // Harmonix Rock Band guitar for PS3
            RegisterLayout<PS3EvdevRockBandGuitar>(mapping, 0x12BA, 0x0200);
            // Harmonix Rock Band 1/2 guitars for Wii
            RegisterLayout<PS3EvdevRockBandGuitar>(mapping, 0x1BAD, 0x0004);
            RegisterLayout<PS3EvdevRockBandGuitar>(mapping, 0x1BAD, 0x3010);
        }
    }
}
#endif
