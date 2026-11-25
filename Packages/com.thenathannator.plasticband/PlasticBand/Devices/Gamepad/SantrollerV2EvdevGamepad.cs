#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(displayName = "Santroller V2 Gamepad", hideInUI = true)]
    internal class SantrollerV2EvdevGamepad : Gamepad
    {
        internal static void Initialize()
        {
            // Note that bluetooth gamepads don't expose the capabilities, so they will always assume the default layout
            EvdevLayoutFinder.RegisterLayout<SantrollerV2EvdevGamepad>(
                GetMappings(EvdevStickOption.None),
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.Gamepad)
            );

            // Follow the invert-y flag if it its available
            EvdevLayoutFinder.RegisterLayout<SantrollerV2EvdevGamepad_XInputYAxis>(
                GetMappings(EvdevStickOption.InvertY),
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.Gamepad)
                    .WithPredicate((caps) => ((SantrollerCapabilities)caps.santrollerCapabilities & SantrollerCapabilities.YAxisInverted) != 0 ? 1 : -1)
            );
        }

        private static EvdevDeviceMapping GetMappings(EvdevStickOption stickOptions)
        {
            return new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad")
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
                .WithStick(input_abs_code.ABS_X, "leftStick", stickOptions)
                .WithStick(input_abs_code.ABS_RX, "rightStick", stickOptions);
        }
    }

    [InputControlLayout(displayName = "Santroller V2 Gamepad", hideInUI = true)]
    internal class SantrollerV2EvdevGamepad_XInputYAxis : SantrollerV2EvdevGamepad
    {
    }
}
#endif
