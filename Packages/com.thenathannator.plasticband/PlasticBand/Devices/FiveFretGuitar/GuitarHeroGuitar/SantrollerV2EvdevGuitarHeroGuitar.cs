#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller V2 Guitar Hero Guitar", hideInUI = true)]
    internal class SantrollerV2EvdevGuitarHeroGuitar : EvdevGuitarHeroGuitar<SantrollerV2EvdevGuitarHeroGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code_alt.BTN_A;
            m_RedFret = input_key_code_alt.BTN_B;
            m_YellowFret = input_key_code_alt.BTN_Y;
            m_BlueFret = input_key_code_alt.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;
            m_SpPedal = input_key_code.BTN_TR;

            m_Whammy = input_abs_code.ABS_RX;
            m_Tilt = input_abs_code.ABS_RY;

            m_WhammyOptions = EvdevAxisOption.Unsigned;
            m_TiltOptions = EvdevAxisOption.Inverted;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code_alt.BTN_A, "greenFret")
                .WithKey(input_key_code_alt.BTN_B, "redFret")
                .WithKey(input_key_code_alt.BTN_Y, "yellowFret")
                .WithKey(input_key_code_alt.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "spPedal")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_RX, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RY, "tilt", EvdevAxisOption.Inverted)
                .WithAbs(input_abs_code.ABS_X, "touchFrets");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerV2EvdevGuitarHeroGuitar>(
                mappings,
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.GuitarHeroGuitar)
            );
        }

        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
        {
            int touchBar = m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_X);
            return TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[(byte)(touchBar ^ 0x80)];
        }
    }
}
#endif
