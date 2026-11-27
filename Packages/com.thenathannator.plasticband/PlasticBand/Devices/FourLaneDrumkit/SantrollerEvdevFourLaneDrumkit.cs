#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller Rock Band Drumkit", hideInUI = true)]
    internal class SantrollerEvdevFourLaneDrumkit : EvdevFourLaneDrumkit<SantrollerEvdevFourLaneDrumkit>
    {
        internal new static void Initialize()
        {
            m_Red = input_key_code_alt.BTN_B;
            m_Yellow = input_key_code_alt.BTN_Y;
            m_Blue = input_key_code_alt.BTN_X;
            m_Green = input_key_code_alt.BTN_A;
            m_PadFlag = input_key_code.BTN_THUMB_R;
            m_CymbalFlag = input_key_code.BTN_TR;
            m_Kick1 = input_key_code.BTN_TL;
            m_Kick2 = input_key_code.BTN_THUMB_L;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad")
                .WithKey(input_key_code_alt.BTN_A, "buttonSouth")
                .WithKey(input_key_code_alt.BTN_B, "buttonEast")
                .WithKey(input_key_code_alt.BTN_X, "buttonWest")
                .WithKey(input_key_code_alt.BTN_Y, "buttonNorth")
                .WithKey(input_key_code.BTN_THUMB_R, "padFlag")
                .WithKey(input_key_code.BTN_TR, "cymbalFlag")
                .WithKey(input_key_code.BTN_TL, "kick1")
                .WithKey(input_key_code.BTN_THUMB_L, "kick2")
                .WithAbs(input_abs_code.ABS_Y, "redPadVelocity")
                .WithAbs(input_abs_code.ABS_RX, "yellowPadVelocity")
                .WithAbs(input_abs_code.ABS_RY, "bluePadVelocity")
                .WithAbs(input_abs_code.ABS_X, "greenPadVelocity")
                .WithAbs(input_abs_code.ABS_RZ, "yellowCymbalVelocity")
                .WithAbs(input_abs_code.ABS_THROTTLE, "blueCymbalVelocity")
                .WithAbs(input_abs_code.ABS_Z, "greenCymbalVelocity");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerEvdevFourLaneDrumkit>(
                mappings,
                SantrollerLayoutFinder.GetEvdevV1HidMatcher(SantrollerDeviceType.RockBandDrums)
            );
        }

        // Santroller V1 kits report each pad and cymbal's velocity separately
        protected override unsafe void ReadVelocities(StateEvent* stateEvent, FourLanePad pads, ref TranslatedFourLaneState translated)
        {
            translated.redPad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y);
            translated.yellowPad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RX);
            translated.bluePad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RY);
            translated.greenPad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X);
            translated.yellowCymbal = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RZ);
            translated.blueCymbal = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_THROTTLE);
            translated.greenCymbal = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Z);
        }
    }
}
#endif
