#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Not supported (requires state translation): solo frets, pickup switch
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    [InputControlLayout(displayName = "XInput Evdev Rock Band Guitar", hideInUI = true)]
    internal class XInputEvdevRockBandGuitar : RockBandGuitar
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
                .WithAbs(input_abs_code.ABS_RY, "tilt");

            RegisterXpadLayout<XInputEvdevRockBandGuitar>(mapping, XInputDeviceSubType.Guitar);
            RegisterXpadLayout<XInputEvdevRockBandGuitar>(mapping, XInputDeviceSubType.GuitarBass);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevRockBandGuitar>(mapping,
                XInputDeviceSubType.Guitar, SantrollerDeviceType.RockBandGuitar);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevRockBandGuitar>(mapping,
                XInputDeviceSubType.GuitarBass, SantrollerDeviceType.RockBandGuitar);
        }
    }
}
#endif
