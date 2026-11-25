#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class SantrollerEvdevGuitarHeroGuitar : EvdevGuitarHeroGuitar<SantrollerEvdevGuitarHeroGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_A;
            m_RedFret = input_key_code.BTN_B;
            m_YellowFret = input_key_code.BTN_Y;
            m_BlueFret = input_key_code.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;
            m_SpPedal = input_key_code.BTN_TR;

            m_Whammy = input_abs_code.ABS_X;
            m_Tilt = input_abs_code.ABS_RX;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "spPedal")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_X, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RX, "tilt")
                .WithAbs(input_abs_code.ABS_Y, "touchFrets");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerEvdevGuitarHeroGuitar>(
                mappings,
                SantrollerLayoutFinder.GetEvdevV1HidMatcher(SantrollerDeviceType.GuitarHeroGuitar)
            );
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => (byte)m_StateHandler.ReadAbs(stateEvent, m_Whammy);

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_Tilt) - 0x80);

        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
            => TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[(byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y)];
    }
}
#endif
