#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), hideInUI = true)]
    internal class XpadEvdevGuitarHeroGuitar : EvdevGuitarHeroGuitar<XpadEvdevGuitarHeroGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_A;
            m_RedFret = input_key_code.BTN_B;
            m_YellowFret = input_key_code.BTN_Y;
            m_BlueFret = input_key_code.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;
            m_SpPedal = input_key_code.BTN_TR;

            m_Whammy = input_abs_code.ABS_RX;
            m_Tilt = input_abs_code.ABS_RY;

            m_WhammyOptions = EvdevAxisOption.Unsigned;
            m_TiltOptions = EvdevAxisOption.None;

            var xpadMappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "spPedal")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_RX, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RY, "tilt")
                .WithAbs(input_abs_code.ABS_X, "touchFrets");

            // RedOctane Guitar Hero Guitar for Xbox 360 (all models)
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevGuitarHeroGuitar>(
                xpadMappings,
                new EvdevDeviceMatcher()
                    .WithDriver(EvdevDefinitions.DriverXpad)
                    .WithXusbSubtype(XInputDeviceSubType.GuitarAlternate)
            );

            // Santroller devices in XInput mode
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevGuitarHeroGuitar_GH5>(
                xpadMappings,
                SantrollerLayoutFinder.GetEvdevXpadMatcher(XInputDeviceSubType.GuitarAlternate, SantrollerDeviceType.GuitarHeroGuitar)
            );
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevGuitarHeroGuitar_GH5>(
                xpadMappings,
                SantrollerLayoutFinder.GetEvdevV2XpadMatcher(XInputDeviceSubType.GuitarAlternate)
            );

            // RedOctane Guitar Hero 5 and Band Hero Guitar for Xbox 360
            // Needs special handling for its slider bar
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevGuitarHeroGuitar_GH5>(
                xpadMappings,
                new EvdevDeviceMatcher()
                    .WithDriver(EvdevDefinitions.DriverXpad)
                    .WithXusbSubtype(XInputDeviceSubType.GuitarAlternate)
                    .WithHardwareIds(0x1430, 0x0705)
            );
        }

        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
        {
            int touchBar = m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X);
            byte rawTouchBar = (byte)((touchBar >> 8) ^ 0x80);
            return TranslatingGuitarHeroGuitar.s_WtTouchFrets[(rawTouchBar & 0xF0) >> 4];
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), hideInUI = true)]
    internal class XpadEvdevGuitarHeroGuitar_GH5 : XpadEvdevGuitarHeroGuitar
    {
        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
        {
            int touchBar = m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X);
            return TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[(byte)(touchBar ^ 0x80)];
        }
    }
}
#endif
