using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Evdev Rock Band Drumkit")]
    internal class EvdevFourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<XInputFourLaneDrumkitState>, IInputStateCallbackReceiver
    {
        private bool m_HasFlags;
        private int m_SantrollerProtocol;
        private TranslateStateHandler<EvdevState, TranslatedFourLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevFourLaneDrumkit>(SantrollerDeviceType.RockBandDrums);
            EvdevLayoutFinder.RegisterXInput<EvdevFourLaneDrumkit>(8, "Xbox 360 Wireless Rock Band Drum Kit");
            EvdevLayoutFinder.RegisterXInput<EvdevFourLaneDrumkit>(8, 0x1BAD, 0x0003);
            EvdevLayoutFinder.RegisterXInput<EvdevFourLaneDrumkit>(8, 0x1BAD, 0x0130);
            EvdevLayoutFinder.RegisterXInput<EvdevFourLaneDrumkit>(8, 0x15E4, 0x0130);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedFourLaneState TranslateState(ref EvdevState state)
        {
            if (m_SantrollerProtocol != 0)
                return EvdevSantrollerState.FourLane(ref state, m_SantrollerProtocol, ref m_HasFlags);
            var raw = state.ToXInput();
            var drums = *(XInputFourLaneDrumkitState*)&raw;
            return TranslatingFourLaneDrumkit_Flags<XInputFourLaneDrumkitState>.TranslateState(ref drums, ref m_HasFlags);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            if (EvdevLayoutFinder.GetCapabilities(this).driver == "hid-generic")
                m_SantrollerProtocol = EvdevSantrollerState.Validate(this, SantrollerDeviceType.RockBandDrums);
            else
                EvdevLayoutFinder.RequireXInputAxes(this);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFourLaneState>.VerifyDevice(this);
        }
    }
}
