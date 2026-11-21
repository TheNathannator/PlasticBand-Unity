#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    // XInput GHL guitars share the GuitarAlternate subtype with GH guitars, and xpad doesn't expose the
    // No Navigation flag used to tell them apart, so they are matched by vendor/product ID instead.
    // Strum is reported on both the d-pad and the left stick Y axis; the d-pad is used here.
    [InputControlLayout(displayName = "XInput Evdev Guitar Hero Live Guitar", hideInUI = true)]
    internal class XInputEvdevSixFretGuitar : SixFretGuitar
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
                .WithAbs(input_abs_code.ABS_RX, "tilt")
                .WithAbs(input_abs_code.ABS_RY, "whammy");

            // Activision Guitar Hero Live dongle for Xbox 360
            RegisterXpadLayout<XInputEvdevSixFretGuitar>(mapping, XInputDeviceSubType.GuitarAlternate, 0x1430, 0x070B);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevSixFretGuitar>(mapping,
                XInputDeviceSubType.GuitarAlternate, SantrollerDeviceType.LiveGuitar);
        }
    }
}
#endif
