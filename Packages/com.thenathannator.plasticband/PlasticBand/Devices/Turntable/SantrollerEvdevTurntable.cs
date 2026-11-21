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
    // Platter buttons are reported as distinct buttons, so they are mapped directly.
    [InputControlLayout(displayName = "Santroller Evdev DJ Hero Turntable", hideInUI = true)]
    internal class SantrollerEvdevTurntable : Turntable
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithKey(input_key_code.BTN_C, "rightTableGreen")
                .WithKey(input_key_code.BTN_Z, "rightTableRed")
                .WithKey(input_key_code.BTN_TL, "rightTableBlue")
                .WithKey(input_key_code.BTN_TR, "leftTableGreen")
                .WithKey(input_key_code.BTN_TL2, "leftTableRed")
                .WithKey(input_key_code.BTN_TR2, "leftTableBlue")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, Inverted("rightTableVelocity"))
                .WithAbs(input_abs_code.ABS_RX, "effectsDial")
                .WithAbs(input_abs_code.ABS_RY, Inverted("crossfader"));
            SantrollerLayoutFinder.RegisterEvdevV1HidLayout<SantrollerEvdevTurntable>(mapping, SantrollerDeviceType.DjHeroTurntable);
        }
    }
}
#endif
