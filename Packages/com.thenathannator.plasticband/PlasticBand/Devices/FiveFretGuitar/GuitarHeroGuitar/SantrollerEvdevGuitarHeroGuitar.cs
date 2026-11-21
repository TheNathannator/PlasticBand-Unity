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
    // Not supported (requires state translation): touch/slider bar
    [InputControlLayout(displayName = "Santroller Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class SantrollerEvdevGuitarHeroGuitar : GuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "spPedal")
                .WithAbs(input_abs_code.ABS_X, Unsigned("whammy"))
                .WithAbs(input_abs_code.ABS_RX, "tilt");
            SantrollerLayoutFinder.RegisterEvdevV1HidLayout<SantrollerEvdevGuitarHeroGuitar>(mapping, SantrollerDeviceType.GuitarHeroGuitar);
        }
    }
}
#endif
