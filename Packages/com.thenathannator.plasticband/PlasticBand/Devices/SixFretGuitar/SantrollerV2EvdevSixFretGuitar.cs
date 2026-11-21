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
    [InputControlLayout(displayName = "Santroller V2 Evdev Guitar Hero Live Guitar", hideInUI = true)]
    internal class SantrollerV2EvdevSixFretGuitar : SixFretGuitar
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
                // Same placement as XInput GHL guitars: tilt on right stick X, whammy on right stick Y.
                // Verified against firmware output (Tools/EvdevTests).
                .WithAbs(input_abs_code.ABS_RX, "tilt")
                .WithAbs(input_abs_code.ABS_RY, Unsigned("whammy"));
            SantrollerLayoutFinder.RegisterEvdevLayout<SantrollerV2EvdevSixFretGuitar>(mapping, SantrollerDeviceType.LiveGuitar);
        }
    }
}
#endif
