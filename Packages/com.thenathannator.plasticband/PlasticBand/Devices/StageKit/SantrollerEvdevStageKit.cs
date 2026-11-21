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
    // Not supported: haptics (lights/fog/strobe), as evdev provides no way to send them
    // The face buttons use different usages from the other device types (South = BTN_X, East = BTN_A, West = BTN_B).
    [InputControlLayout(displayName = "Santroller Evdev Rock Band Stage Kit", hideInUI = true)]
    internal class SantrollerEvdevStageKit : StageKit
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_X, "buttonSouth")
                .WithKey(input_key_code.BTN_A, "buttonEast")
                .WithKey(input_key_code.BTN_B, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithKey(input_key_code.BTN_TL, "leftShoulder")
                .WithKey(input_key_code.BTN_TR, "rightShoulder")
                .WithKey(input_key_code.BTN_THUMB_L, "leftStickPress")
                .WithKey(input_key_code.BTN_THUMB_R, "rightStickPress")
                .WithAbsButton(input_abs_code.ABS_Z, "leftTrigger")
                .WithAbsButton(input_abs_code.ABS_RZ, "rightTrigger")
                .WithStick(input_abs_code.ABS_X, "leftStick")
                .WithStick(input_abs_code.ABS_RX, "rightStick");
            SantrollerLayoutFinder.RegisterEvdevV1HidLayout<SantrollerEvdevStageKit>(mapping, SantrollerDeviceType.StageKit);
        }
    }
}
#endif
