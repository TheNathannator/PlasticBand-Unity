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
    [InputControlLayout(displayName = "Santroller Evdev Guitar Hero Live Guitar", hideInUI = true)]
    internal class SantrollerEvdevSixFretGuitar : SixFretGuitar
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "black1")
                .WithKey(input_key_code.BTN_B, "black2")
                .WithKey(input_key_code.BTN_Y, "black3")
                .WithKey(input_key_code.BTN_X, "white1")
                .WithKey(input_key_code.BTN_TL, "white2")
                .WithKey(input_key_code.BTN_TR, "white3")
                .WithKey(input_key_code.BTN_THUMB_L, "ghtvButton")
                .WithAbs(input_abs_code.ABS_X, Unsigned("whammy"))
                .WithAbs(input_abs_code.ABS_Y, Inverted("tilt"));
            SantrollerLayoutFinder.RegisterEvdevV1HidLayout<SantrollerEvdevSixFretGuitar>(mapping, SantrollerDeviceType.LiveGuitar);
        }
    }
}
#endif
