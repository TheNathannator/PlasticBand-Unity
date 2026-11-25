#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller V2 Evdev Rock Band Drumkit", hideInUI = true)]
    internal class SantrollerV2EvdevFourLaneDrumkit : EvdevFourLaneDrumkit<SantrollerV2EvdevFourLaneDrumkit>
    {
        internal new static void Initialize()
        {
            m_Green = input_key_code.BTN_A;
            m_Red = input_key_code.BTN_B;
            m_Yellow = input_key_code.BTN_Y;
            m_Blue = input_key_code.BTN_X;
            m_Kick1 = input_key_code.BTN_TL;
            m_Kick2 = input_key_code.BTN_THUMB_L;
            m_PadFlag = input_key_code.BTN_THUMB_R;
            m_CymbalFlag = input_key_code.BTN_TR;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad")
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_TL, "kick1")
                .WithKey(input_key_code.BTN_THUMB_L, "kick2")
                .WithKey(input_key_code.BTN_THUMB_R, "padFlag")
                .WithKey(input_key_code.BTN_TR, "cymbalFlag")
                .WithAbs(input_abs_code.ABS_X, "redPad")
                .WithAbs(input_abs_code.ABS_Y, "yellowPad")
                .WithAbs(input_abs_code.ABS_RX, "bluePad")
                .WithAbs(input_abs_code.ABS_RY, "greenPad");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerV2EvdevFourLaneDrumkit>(
                mappings,
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.RockBandDrums)
            );
        }

        protected override unsafe byte ReadRedVelocity(StateEvent* stateEvent)
            => GetVelocity_Positive((short)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X));

        protected override unsafe byte ReadYellowVelocity(StateEvent* stateEvent)
            => GetVelocity_Negative((short)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y));

        protected override unsafe byte ReadBlueVelocity(StateEvent* stateEvent)
            => GetVelocity_Positive((short)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RX));

        protected override unsafe byte ReadGreenVelocity(StateEvent* stateEvent)
            => GetVelocity_Negative((short)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RY));
    }
}
#endif
