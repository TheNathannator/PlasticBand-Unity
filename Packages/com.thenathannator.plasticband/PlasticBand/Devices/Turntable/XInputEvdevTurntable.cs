#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Not supported (requires state translation): platter buttons (packed into the trigger axes)
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    // Platter velocities are reported in the low byte of the stick axes, which doesn't survive being
    // treated as a plain axis (and is subject to kernel fuzz filtering), so they are not mapped.
    [InputControlLayout(displayName = "XInput Evdev DJ Hero Turntable", hideInUI = true)]
    internal class XInputEvdevTurntable : Turntable
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithAbs(input_abs_code.ABS_RX, "effectsDial")
                .WithAbs(input_abs_code.ABS_RY, "crossfader");

            RegisterXpadLayout<XInputEvdevTurntable>(mapping, XInputDeviceSubType.Turntable);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevTurntable>(mapping,
                XInputDeviceSubType.Turntable, SantrollerDeviceType.DjHeroTurntable);
        }
    }
}
#endif
