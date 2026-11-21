#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Not supported (requires state translation): touch/slider bar
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    // NOTE: GHL guitars share the GuitarAlternate subtype; known GHL guitars are matched by
    // vendor/product ID in XInputEvdevSixFretGuitar, which takes priority over this layout.
    [InputControlLayout(displayName = "XInput Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class XInputEvdevGuitarHeroGuitar : GuitarHeroGuitar
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
                .WithAbs(input_abs_code.ABS_RX, Unsigned("whammy"))
                .WithAbs(input_abs_code.ABS_RY, "tilt");

            RegisterXpadLayout<XInputEvdevGuitarHeroGuitar>(mapping, XInputDeviceSubType.GuitarAlternate);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevGuitarHeroGuitar>(mapping,
                XInputDeviceSubType.GuitarAlternate, SantrollerDeviceType.GuitarHeroGuitar);
        }
    }
}
#endif
