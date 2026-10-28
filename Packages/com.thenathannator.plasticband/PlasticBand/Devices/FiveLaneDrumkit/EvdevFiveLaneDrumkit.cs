using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Evdev Guitar Hero Drumkit")]
    internal class EvdevFiveLaneDrumkit : TranslatingFiveLaneDrumkit<XInputFiveLaneDrumkitState>, IInputStateCallbackReceiver
    {
        private int m_SantrollerProtocol;
        private TranslateStateHandler<EvdevState, TranslatedFiveLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevFiveLaneDrumkit>(SantrollerDeviceType.GuitarHeroDrums);
            EvdevLayoutFinder.RegisterXInput<EvdevFiveLaneDrumkit>(8, "Xbox 360 Wireless Guitar Hero Drum Kit");
            EvdevLayoutFinder.RegisterXInput<EvdevFiveLaneDrumkit>(8, 0x1430, 0x0805);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFiveLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFiveLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedFiveLaneState TranslateState(ref EvdevState state)
        {
            if (m_SantrollerProtocol != 0)
                return EvdevSantrollerState.FiveLane(ref state, m_SantrollerProtocol);
            var raw = state.ToXInput();
            var drums = *(XInputFiveLaneDrumkitState*)&raw;
            return TranslatingFiveLaneDrumkit<XInputFiveLaneDrumkitState>.TranslateState(ref drums);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            if (EvdevLayoutFinder.GetCapabilities(this).driver == "hid-generic")
                m_SantrollerProtocol = EvdevSantrollerState.Validate(this, SantrollerDeviceType.GuitarHeroDrums);
            else
                EvdevLayoutFinder.RequireXInputAxes(this);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFiveLaneState>.VerifyDevice(this);
        }
    }
}
