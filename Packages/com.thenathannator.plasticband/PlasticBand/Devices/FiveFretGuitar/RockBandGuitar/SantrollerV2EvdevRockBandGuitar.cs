#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Santroller V2 devices, exposed through hid-generic/hid-santroller.
    // The HID report mirrors XInput, so Y axes are positive-up and must be inverted
    // relative to evdev's convention. Uses the same key codes as xpad.
    // Not supported (requires state translation): solo frets, pickup switch
    [InputControlLayout(displayName = "Santroller V2 Evdev Rock Band Guitar", hideInUI = true)]
    internal class SantrollerV2EvdevRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithAbs(input_abs_code.ABS_RX, Unsigned("whammy"))
                .WithAbs(input_abs_code.ABS_RY, Inverted("tilt"));
            SantrollerLayoutFinder.RegisterEvdevLayout<SantrollerV2EvdevRockBandGuitar>(mapping, SantrollerDeviceType.RockBandGuitar);
        }
    }
}
#endif
