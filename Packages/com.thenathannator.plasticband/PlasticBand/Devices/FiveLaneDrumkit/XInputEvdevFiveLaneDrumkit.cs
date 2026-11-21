#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Not supported (requires state translation): velocities
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    [InputControlLayout(displayName = "XInput Evdev Guitar Hero Drumkit", hideInUI = true)]
    internal class XInputEvdevFiveLaneDrumkit : FiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "greenPad")
                .WithKey(input_key_code.BTN_B, "redPad")
                .WithKey(input_key_code.BTN_X, "bluePad")
                .WithKey(input_key_code.BTN_Y, "yellowCymbal")
                .WithKey(input_key_code.BTN_TR, "orangeCymbal")
                .WithKey(input_key_code.BTN_TL, "kick");

            // RedOctane Guitar Hero 5, Band Hero, and Warriors of Rock Drumkit for Xbox 360
            RegisterXpadLayout<XInputEvdevFiveLaneDrumkit>(mapping, XInputDeviceSubType.DrumKit, 0x1430, 0x0805);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevFiveLaneDrumkit>(mapping,
                XInputDeviceSubType.DrumKit, SantrollerDeviceType.GuitarHeroDrums);
        }
    }
}
#endif
