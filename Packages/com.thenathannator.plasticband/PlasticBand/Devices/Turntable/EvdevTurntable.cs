using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Evdev DJ Hero Turntable")]
    internal class EvdevTurntable : TranslatingTurntable<XInputTurntableState>, IInputStateCallbackReceiver
    {
        private int m_SantrollerProtocol;
        private TranslateStateHandler<EvdevState, TranslatedTurntableState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevTurntable>(SantrollerDeviceType.DjHeroTurntable);
            EvdevLayoutFinder.RegisterXInput<EvdevTurntable>(23);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedTurntableState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedTurntableState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedTurntableState TranslateState(ref EvdevState state)
        {
            if (m_SantrollerProtocol != 0)
                return EvdevSantrollerState.Turntable(ref state, m_SantrollerProtocol);
            var raw = state.ToXInput();
            var turntable = *(XInputTurntableState*)&raw;
            return TranslatingTurntable<XInputTurntableState>.TranslateState(ref turntable);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            if (EvdevLayoutFinder.GetCapabilities(this).driver == "hid-generic")
                m_SantrollerProtocol = EvdevSantrollerState.Validate(this, SantrollerDeviceType.DjHeroTurntable);
            else
                EvdevLayoutFinder.RequireXInputAxes(this);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedTurntableState>.VerifyDevice(this);
        }
    }
}
