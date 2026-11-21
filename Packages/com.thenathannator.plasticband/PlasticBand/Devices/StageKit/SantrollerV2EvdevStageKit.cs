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
    // Not supported: haptics (lights/fog/strobe), as evdev provides no way to send them
    // NOTE: stick Y axes will read inverted; stage kits don't have physical sticks,
    // so this isn't worth special-casing.
    [InputControlLayout(displayName = "Santroller V2 Evdev Rock Band Stage Kit", hideInUI = true)]
    internal class SantrollerV2EvdevStageKit : StageKit
    {
        internal new static void Initialize()
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
            SantrollerLayoutFinder.RegisterEvdevLayout<SantrollerV2EvdevStageKit>(mapping, SantrollerDeviceType.StageKit);
        }
    }
}
#endif
