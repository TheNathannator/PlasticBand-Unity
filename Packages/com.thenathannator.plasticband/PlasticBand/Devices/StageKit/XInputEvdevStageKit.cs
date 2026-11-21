#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Not supported: haptics (lights/fog/strobe), as evdev provides no way to send them
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    [InputControlLayout(displayName = "XInput Evdev Rock Band Stage Kit", hideInUI = true)]
    internal class XInputEvdevStageKit : StageKit
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

            RegisterXpadLayout<XInputEvdevStageKit>(mapping, XInputDeviceSubType.StageKit);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevStageKit>(mapping,
                XInputDeviceSubType.StageKit, SantrollerDeviceType.StageKit);
        }
    }
}
#endif
