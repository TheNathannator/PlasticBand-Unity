using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitLayout), displayName = "Evdev PlayStation Guitar Hero Drumkit")]
    internal class EvdevSonyFiveLaneDrumkit : FiveLaneDrumkit, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private TranslateStateHandler<EvdevState, TranslatedFiveLaneState> m_Translator;

        internal new static void Initialize()
            => EvdevLayoutFinder.Register<EvdevSonyFiveLaneDrumkit>("sony", 0x12BA, 0x0120);

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            capabilities.RequireAxes(0x10, 0x11);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFiveLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFiveLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFiveLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedFiveLaneState TranslateState(ref EvdevState state)
        {
            // hid-sony discards the vendor-defined velocity fields; hits are digital.
            var drums = new PS3FiveLaneDrumkitState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase), dpad = state.GetDpad(),
                redVelocity = byte.MaxValue, yellowVelocity = byte.MaxValue,
                blueVelocity = byte.MaxValue, greenVelocity = byte.MaxValue,
                orangeVelocity = byte.MaxValue, kickVelocity = byte.MaxValue,
            };
            return TranslatingFiveLaneDrumkit<PS3FiveLaneDrumkitState_NoReportId>.TranslateState(ref drums);
        }
    }
}
