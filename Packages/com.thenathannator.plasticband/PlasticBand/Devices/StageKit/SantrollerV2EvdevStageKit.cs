#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(displayName = "Santroller V2 Stage Kit", hideInUI = true)]
    internal class SantrollerV2EvdevStageKit : StageKit
    {
        internal new static void Initialize()
        {
            var mapping = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad")
                .WithKey(input_key_code_alt.BTN_A, "buttonSouth")
                .WithKey(input_key_code_alt.BTN_B, "buttonEast")
                .WithKey(input_key_code_alt.BTN_X, "buttonWest")
                .WithKey(input_key_code_alt.BTN_Y, "buttonNorth")
                .WithKey(input_key_code.BTN_TL, "leftShoulder")
                .WithKey(input_key_code.BTN_TR, "rightShoulder")
                .WithKey(input_key_code.BTN_THUMB_L, "leftStickPress")
                .WithKey(input_key_code.BTN_THUMB_R, "rightStickPress")
                .WithAbsButton(input_abs_code.ABS_Z, "leftTrigger")
                .WithAbsButton(input_abs_code.ABS_RZ, "rightTrigger")
                .WithStick(input_abs_code.ABS_X, "leftStick")
                .WithStick(input_abs_code.ABS_RX, "rightStick");

            EvdevLayoutFinder.RegisterLayout<SantrollerV2EvdevStageKit>(
                mapping,
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.StageKit)
            );
        }
    }
}
#endif
