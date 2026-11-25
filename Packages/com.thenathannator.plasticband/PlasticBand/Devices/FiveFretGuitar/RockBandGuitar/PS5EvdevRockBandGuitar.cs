#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Rock Band Guitar for PS5", hideInUI = true)]
    internal class PS5EvdevRockBandGuitar : PS4EvdevRockBandGuitar
    {
        internal new static void Initialize()
        {
            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_X, "yellowFret")
                .WithKey(input_key_code.BTN_Y, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_THUMB_L, "solo")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Z, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RZ, "tilt", EvdevAxisOption.Unsigned);

            // PDP Riffmaster for PS5
            EvdevLayoutFinder.RegisterStaticLayout<PS5EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x0E6F, 0x0249)
            );

            // CRKD Gibson SG for PS5 (wired and dongle)
            EvdevLayoutFinder.RegisterStaticLayout<PS5EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x3651, 0x1600)
            );
            EvdevLayoutFinder.RegisterStaticLayout<PS5EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x3651, 0x5600)
            );
        }
    }
}
#endif
