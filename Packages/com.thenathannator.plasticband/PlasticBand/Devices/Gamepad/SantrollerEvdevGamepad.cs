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
    // Covers Santroller V1 devices emulating a gamepad in HID mode, exposed through hid-generic/hid-santroller.
    // The firmware picks button usages that hid-input maps to the same-named evdev keys (xpad convention,
    // so BTN_X is west), and the axes are 8-bit with Y already positive-down like evdev.
    // Not supported: the guide and capture buttons (BTN_MODE/BTN_C) and pressure axes, which Gamepad has no controls for.
    [InputControlLayout(displayName = "Santroller Evdev Gamepad", hideInUI = true)]
    internal class SantrollerEvdevGamepad : Gamepad
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
            SantrollerLayoutFinder.RegisterEvdevV1HidLayout<SantrollerEvdevGamepad>(mapping, SantrollerDeviceType.Gamepad);
        }
    }
}
#endif
