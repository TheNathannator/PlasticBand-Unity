using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Evdev Guitar Hero Live Guitar")]
    internal class EvdevXpadSixFretGuitar : SixFretGuitar, IInputStateCallbackReceiver
    {
        private int m_SantrollerProtocol;
        private TranslateStateHandler<EvdevState, TranslatedSixFretState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevXpadSixFretGuitar>(SantrollerDeviceType.LiveGuitar);
            EvdevLayoutFinder.Register<EvdevXpadSixFretGuitar>("xpad", 0x1430, 0x079B);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            if (caps.driver == "hid-generic")
                m_SantrollerProtocol = EvdevSantrollerState.Validate(this, SantrollerDeviceType.LiveGuitar);
            else
                caps.RequireAxes(1, 3, 4);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedSixFretState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedSixFretState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedSixFretState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedSixFretState TranslateState(ref EvdevState state)
        {
            if (m_SantrollerProtocol != 0)
                return EvdevSantrollerState.SixFret(ref state, m_SantrollerProtocol);
            var raw = state.ToXInput();
            var guitar = *(XInputSixFretGuitarState*)&raw;
            return TranslatingSixFretGuitar<XInputSixFretGuitarState>.TranslateState(ref guitar);
        }
    }
}
