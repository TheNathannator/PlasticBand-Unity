#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Santroller V2 devices emulating a gamepad, exposed through hid-generic/hid-santroller.
    // Uses the same key codes as xpad.
    // Stick Y axes follow HID convention (down at the maximum) like evdev, except when the profile's
    // "invert Y axis" HID option is set: they then follow XInput's convention and will read inverted.
    // The device reports that through its capabilities, which HIDrogen doesn't currently read.
    // Not supported: the guide and capture buttons (BTN_MODE/BTN_C), which Gamepad has no controls for.
    [InputControlLayout(displayName = "Santroller V2 Evdev Gamepad", hideInUI = true)]
    internal class SantrollerV2EvdevGamepad : Gamepad
    {
        internal static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithKey(input_key_code.BTN_TL, "leftShoulder")
                .WithKey(input_key_code.BTN_TR, "rightShoulder")
                .WithKey(input_key_code.BTN_THUMB_L, "leftStickPress")
                .WithKey(input_key_code.BTN_THUMB_R, "rightStickPress")
                .WithAbsButton(input_abs_code.ABS_Z, "leftTrigger")
                .WithAbsButton(input_abs_code.ABS_RZ, "rightTrigger")
                .WithStick(input_abs_code.ABS_X, "leftStick")
                .WithStick(input_abs_code.ABS_RX, "rightStick");
            SantrollerLayoutFinder.RegisterEvdevLayout<SantrollerV2EvdevGamepad>(mapping, SantrollerDeviceType.Gamepad);
        }
    }
}
#endif
