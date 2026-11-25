#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Guitar Hero Live Guitar for Xbox 360", hideInUI = true)]
    internal class XpadEvdevSixFretGuitar : EvdevSixFretGuitar<XpadEvdevSixFretGuitar>
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

            var xpadMappings = new EvdevDeviceMapping()
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
                .WithAbs(input_abs_code.ABS_Y, "strumBar")
                .WithAbs(input_abs_code.ABS_RY, "whammy")
                .WithAbs(input_abs_code.ABS_RX, "tilt");

            // Activision Guitar Hero Live dongle for Xbox 360
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevSixFretGuitar>(
                xpadMappings,
                new EvdevDeviceMatcher()
                    .WithDriver(EvdevDefinitions.DriverXpad)
                    .WithXusbSubtype(XInputDeviceSubType.GuitarAlternate)
                    .WithHardwareIds(0x1430, 0x070B)
            );

            // Santroller V1 devices in XInput mode
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevSixFretGuitar>(
                xpadMappings,
                SantrollerLayoutFinder.GetEvdevXpadMatcher(XInputDeviceSubType.GuitarAlternate, SantrollerDeviceType.LiveGuitar)
            );
        }

        protected override unsafe void ReadStrum(StateEvent* stateEvent, out bool strumUp, out bool strumDown)
        {
            short strumBar = (short)~m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y);
            strumUp = strumBar > 0;
            strumDown = strumBar < 0;
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => (byte)((short)~m_StateHandler.ReadAbs(stateEvent, m_Whammy) >> 7);

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => XInputDefinitions.GetAxisX((short)m_StateHandler.ReadAbs(stateEvent, m_Tilt));
    }
}
#endif
