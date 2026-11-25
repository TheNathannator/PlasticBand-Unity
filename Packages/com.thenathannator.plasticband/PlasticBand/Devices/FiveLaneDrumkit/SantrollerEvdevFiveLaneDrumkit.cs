#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Santroller Evdev Guitar Hero Drumkit", hideInUI = true)]
    internal class SantrollerEvdevFiveLaneDrumkit : EvdevFiveLaneDrumkit<SantrollerEvdevFiveLaneDrumkit>
    {
        internal new static void Initialize()
        {
            m_RedPad = input_key_code.BTN_B;
            m_YellowCymbal = input_key_code.BTN_Y;
            m_BluePad = input_key_code.BTN_X;
            m_OrangeCymbal = input_key_code.BTN_TL;
            m_GreenPad = input_key_code.BTN_A;
            m_Kick = input_key_code.BTN_TR;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_B, "redPad")
                .WithKey(input_key_code.BTN_Y, "yellowCymbal")
                .WithKey(input_key_code.BTN_X, "bluePad")
                .WithKey(input_key_code.BTN_TL, "orangeCymbal")
                .WithKey(input_key_code.BTN_A, "greenPad")
                .WithKey(input_key_code.BTN_TR, "kick")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Y, "redPadVelocity")
                .WithAbs(input_abs_code.ABS_RX, "yellowCymbalVelocity")
                .WithAbs(input_abs_code.ABS_RY, "bluePadVelocity")
                .WithAbs(input_abs_code.ABS_Z, "orangeCymbalVelocity")
                .WithAbs(input_abs_code.ABS_X, "greenPadVelocity")
                .WithAbs(input_abs_code.ABS_RZ, "kickVelocity");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerEvdevFiveLaneDrumkit>(
                mappings,
                SantrollerLayoutFinder.GetEvdevV1HidMatcher(SantrollerDeviceType.GuitarHeroDrums)
            );
        }

        protected override unsafe void ReadVelocities(
            StateEvent* stateEvent,
            out byte red, out byte yellow, out byte blue,
            out byte orange, out byte green, out byte kick
        )
        {
            red = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y);
            yellow = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RX);
            blue = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RY);
            orange = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Z);
            green = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X);
            kick = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RZ);
        }
    }
}
#endif
