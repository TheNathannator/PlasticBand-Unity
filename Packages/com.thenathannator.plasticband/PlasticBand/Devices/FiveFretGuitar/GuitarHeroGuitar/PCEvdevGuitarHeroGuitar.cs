#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "PC Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class PCEvdevGuitarHeroGuitar : PS3EvdevGuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Z, "whammy")
                .WithAbs(input_abs_code.ABS_RY, "tilt")
                .WithKey(input_key_code.BTN_TR, "spPedal");

            // RedOctane Guitar Hero World Tour Guitar for PC
            EvdevLayoutFinder.RegisterStaticLayout<PCEvdevGuitarHeroGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1430, 0x474C)
            );
        }
    }
}
#endif
