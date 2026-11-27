#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Rock Band Guitar for Wii", hideInUI = true)]
    internal class WiiEvdevRockBandGuitar : PS3EvdevRockBandGuitar
    {
        internal new static void Initialize()
        {
            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code_alt.BTN_A, "greenFret")
                .WithKey(input_key_code_alt.BTN_B, "redFret")
                .WithKey(input_key_code_alt.BTN_X, "yellowFret")
                .WithKey(input_key_code_alt.BTN_Y, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "tilt")
                .WithKey(input_key_code.BTN_TL_2, "solo")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Z, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RZ, "pickupSwitch", EvdevAxisOption.Unsigned);

            // Harmonix Rock Band 1/2 guitars for Wii
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x0004)
            );
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x3010)
            );
        }
    }
}
#endif
