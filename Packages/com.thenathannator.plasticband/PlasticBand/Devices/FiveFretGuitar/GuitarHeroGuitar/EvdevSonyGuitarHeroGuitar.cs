using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Evdev PlayStation Guitar Hero Guitar")]
    internal class EvdevSonyGuitarHeroGuitar : GuitarHeroGuitar, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private TranslateStateHandler<EvdevState, TranslatedGuitarHeroGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevSonyGuitarHeroGuitar>("sony", 0x12BA, 0x0100);
            EvdevLayoutFinder.Register<EvdevSonyGuitarHeroGuitar>("sony", 0x1430, 0x474C);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            capabilities.RequireAxes(2, 4, 5, 0x10, 0x11);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedGuitarHeroGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedGuitarHeroGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedGuitarHeroGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedGuitarHeroGuitarState TranslateState(ref EvdevState state)
        {
            var sony = new PS3GuitarHeroGuitarState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase),
                dpad = state.GetDpad(),
                m_Whammy = (byte)state.GetAxis(2),
                m_TouchBar = (byte)state.GetAxis(5),
                m_Tilt = (short)state.GetAxis(4),
            };
            if (description.product == "Guitar Hero5 for PlayStation (R) 3")
                return TranslatingGuitarHeroGuitar_Discrete<PS3GuitarHeroGuitarState_NoReportId>.TranslateState(ref sony);
            return TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_NoReportId>.TranslateState(ref sony);
        }
    }
}

