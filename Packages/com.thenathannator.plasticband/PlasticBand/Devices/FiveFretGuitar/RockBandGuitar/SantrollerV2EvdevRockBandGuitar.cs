#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Santroller V2 Evdev Rock Band Guitar", hideInUI = true)]
    internal class SantrollerV2EvdevRockBandGuitar : EvdevRockBandGuitar<SantrollerV2EvdevRockBandGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_A;
            m_RedFret = input_key_code.BTN_B;
            m_YellowFret = input_key_code.BTN_Y;
            m_BlueFret = input_key_code.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;
            m_Solo = input_key_code.BTN_THUMB_L;

            m_Whammy = input_abs_code.ABS_RX;
            m_Tilt = input_abs_code.ABS_RY;
            m_PickupSwitch = input_abs_code.ABS_Z;

            m_WhammyOptions = EvdevAxisOption.Unsigned;
            m_TiltOptions = EvdevAxisOption.Inverted;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_THUMB_L, "solo")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_RX, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RY, "tilt", EvdevAxisOption.Inverted)
                .WithAbs(input_abs_code.ABS_Z, "pickupSwitch", EvdevAxisOption.Unsigned);

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerV2EvdevRockBandGuitar>(
                mappings,
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.RockBandGuitar)
            );
        }
    }
}
#endif
