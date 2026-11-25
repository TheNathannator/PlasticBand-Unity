#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Santroller V2 Evdev Guitar Hero Live Guitar", hideInUI = true)]
    internal class SantrollerV2EvdevSixFretGuitar : EvdevSixFretGuitar<SantrollerV2EvdevSixFretGuitar>
    {
        internal new static void Initialize()
        {
            m_Black1 = input_key_code.BTN_A;
            m_Black2 = input_key_code.BTN_B;
            m_Black3 = input_key_code.BTN_Y;
            m_White1 = input_key_code.BTN_X;
            m_White2 = input_key_code.BTN_TL;
            m_White3 = input_key_code.BTN_TR;
            m_Ghtv = input_key_code.BTN_THUMB_L;
            m_Select = input_key_code.BTN_SELECT;

            m_Whammy = input_abs_code.ABS_RY;
            m_Tilt = input_abs_code.ABS_RX;

            // These mappings are only required for device validation, but may as well fill them out properly
            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "black1")
                .WithKey(input_key_code.BTN_B, "black2")
                .WithKey(input_key_code.BTN_Y, "black3")
                .WithKey(input_key_code.BTN_X, "white1")
                .WithKey(input_key_code.BTN_TL, "white2")
                .WithKey(input_key_code.BTN_TR, "white3")
                .WithKey(input_key_code.BTN_THUMB_L, "ghtvButton")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_RY, "whammy", EvdevAxisOption.Unsigned | EvdevAxisOption.Inverted)
                .WithAbs(input_abs_code.ABS_RX, "tilt");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerV2EvdevSixFretGuitar>(
                mappings,
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.LiveGuitar)
            );
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => XInputDefinitions.GetAxisUnsignedY((short)m_StateHandler.ReadAbs(stateEvent, m_Whammy));

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => XInputDefinitions.GetAxisX((short)m_StateHandler.ReadAbs(stateEvent, m_Tilt));
    }
}
#endif
