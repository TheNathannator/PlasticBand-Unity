#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Santroller V1 devices in HID mode, exposed through hid-generic/hid-santroller.
    // The firmware picks button usages that hid-input maps to the same-named evdev keys,
    // but the button assignments vary per device type. Axes are 8-bit.
    // Not supported (requires state translation): pickup switch.
    // Solo frets are reported as distinct buttons, so they are mapped directly.
    [InputControlLayout(displayName = "Santroller Evdev Rock Band Guitar", hideInUI = true)]
    internal class SantrollerEvdevRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "soloGreen")
                .WithKey(input_key_code.BTN_TL2, "soloRed")
                .WithKey(input_key_code.BTN_TR2, "soloYellow")
                .WithKey(input_key_code.BTN_C, "soloBlue")
                .WithKey(input_key_code.BTN_Z, "soloOrange")
                .WithAbs(input_abs_code.ABS_X, Unsigned("whammy"))
                .WithAbs(input_abs_code.ABS_RX, "tilt");
            SantrollerLayoutFinder.RegisterEvdevV1HidLayout<SantrollerEvdevRockBandGuitar>(mapping, SantrollerDeviceType.RockBandGuitar);
        }
    }
}
#endif
