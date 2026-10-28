using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "Evdev PlayStation Rock Band Drumkit")]
    internal class EvdevSonyFourLaneDrumkit : FourLaneDrumkit, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private bool m_HasFlags;
        private TranslateStateHandler<EvdevState, TranslatedFourLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevSonyFourLaneDrumkit>("sony", 0x12BA, 0x0210);
            EvdevLayoutFinder.Register<EvdevSonyFourLaneDrumkit>("sony", 0x12BA, 0x0218);
            EvdevLayoutFinder.Register<EvdevSonyFourLaneDrumkit>("sony", 0x1BAD, 0x0005);
            EvdevLayoutFinder.Register<EvdevSonyFourLaneDrumkit>("sony", 0x1BAD, 0x3110);
            EvdevLayoutFinder.Register<EvdevSonyFourLaneDrumkit>("sony", 0x1BAD, 0x3138);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            capabilities.RequireAxes(0x10, 0x11);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFourLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedFourLaneState TranslateState(ref EvdevState state)
        {
            // hid-sony discards the vendor-defined velocity fields; hits are digital.
            var drums = new PS3WiiFourLaneDrumkitState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase), dpad = state.GetDpad(),
                redVelocity = byte.MaxValue, yellowVelocity = byte.MaxValue,
                blueVelocity = byte.MaxValue, greenVelocity = byte.MaxValue,
            };
            return TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_NoReportId>.TranslateState(ref drums, ref m_HasFlags);
        }
    }
}
