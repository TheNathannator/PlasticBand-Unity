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
    // Frets are taken from the face buttons, which RB4 guitars mirror them onto.
    // hid-sony maps whammy and tilt onto the trigger axes.
    [InputControlLayout(displayName = "PS4/PS5 Evdev Rock Band Guitar", hideInUI = true)]
    internal class PS4EvdevRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonyCross, "greenFret")
                .WithKey(SonyCircle, "redFret")
                .WithKey(SonyTriangle, "yellowFret")
                .WithKey(SonySquare, "blueFret")
                .WithKey(SonyButton5, "orangeFret")
                .WithAbs(input_abs_code.ABS_Z, Unsigned("whammy"))
                .WithAbs(input_abs_code.ABS_RZ, Unsigned("tilt"));

            // PDP Riffmaster for PS4
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x0E6F, 0x024A);
            // PDP Jaguar for PS4 (Bluetooth)
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x0E6F, 0x0173);
            // MadCatz Stratocaster for PS4 (Bluetooth)
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x0738, 0x8261);
            // CRKD Gibson SG for PS4 (wired and dongle)
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x3651, 0x1500);
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x3651, 0x5500);

            // PDP Riffmaster for PS5
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x0E6F, 0x0249);
            // CRKD Gibson SG for PS5 (wired and dongle)
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x3651, 0x1600);
            RegisterLayout<PS4EvdevRockBandGuitar>(mapping, 0x3651, 0x5600);
        }
    }
}
#endif
