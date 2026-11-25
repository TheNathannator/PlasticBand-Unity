#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller Evdev Rock Band Drumkit", hideInUI = true)]
    internal class SantrollerEvdevFourLaneDrumkit : EvdevFourLaneDrumkit<SantrollerEvdevFourLaneDrumkit>
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
                .WithAbs(input_abs_code.ABS_X, "greenPad")
                .WithAbs(input_abs_code.ABS_Y, "redPad")
                .WithAbs(input_abs_code.ABS_RX, "yellowPad")
                .WithAbs(input_abs_code.ABS_RY, "bluePad")
                .WithAbs(input_abs_code.ABS_Z, "greenCymbal")
                .WithAbs(input_abs_code.ABS_RZ, "yellowCymbal")
                .WithAbs(input_abs_code.ABS_THROTTLE, "blueCymbal");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerEvdevFourLaneDrumkit>(
                mappings,
                SantrollerLayoutFinder.GetEvdevV1HidMatcher(SantrollerDeviceType.RockBandDrums)
            );
        }

        // Santroller V1 kits report each pad and cymbal's velocity separately
        protected override unsafe void ReadVelocities(StateEvent* stateEvent, FourLanePad pads, ref TranslatedFourLaneState translated)
        {
            translated.greenPad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X);
            translated.redPad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y);
            translated.yellowPad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RX);
            translated.bluePad = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RY);
            translated.greenCymbal = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Z);
            translated.yellowCymbal = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RZ);
            translated.blueCymbal = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_THROTTLE);
        }
    }
}
#endif
