using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Evdev XInput Rock Band Drumkit")]
    internal class EvdevXpadFourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<XInputFourLaneDrumkitState>, IInputStateCallbackReceiver
    {
        private bool m_HasFlags;
        private TranslateStateHandler<EvdevState, TranslatedFourLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadFourLaneDrumkit>(8, "Xbox 360 Wireless Rock Band Drum Kit");
            EvdevLayoutFinder.RegisterXInput<EvdevXpadFourLaneDrumkit>(8, 0x1BAD, 0x0003);
            EvdevLayoutFinder.RegisterXInput<EvdevXpadFourLaneDrumkit>(8, 0x1BAD, 0x0130);
            EvdevLayoutFinder.RegisterXInput<EvdevXpadFourLaneDrumkit>(8, 0x15E4, 0x0130);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedFourLaneState TranslateState(ref EvdevState state)
        {
            XInputButton buttons = 0;
            if (state.GetKey(0x130)) buttons |= XInputButton.A;
            if (state.GetKey(0x131)) buttons |= XInputButton.B;
            if (state.GetKey(0x133)) buttons |= XInputButton.X;
            if (state.GetKey(0x134)) buttons |= XInputButton.Y;
            if (state.GetKey(0x136)) buttons |= XInputButton.LeftShoulder;
            if (state.GetKey(0x137)) buttons |= XInputButton.RightShoulder;
            if (state.GetKey(0x13A)) buttons |= XInputButton.Back;
            if (state.GetKey(0x13B)) buttons |= XInputButton.Start;
            if (state.GetKey(0x13C)) buttons |= XInputButton.Guide;
            if (state.GetKey(0x13D)) buttons |= XInputButton.LeftThumb;
            if (state.GetKey(0x13E)) buttons |= XInputButton.RightThumb;
            if (state.GetAxis(0x10) < 0 || state.GetKey(0x222)) buttons |= XInputButton.DpadLeft;
            if (state.GetAxis(0x10) > 0 || state.GetKey(0x223)) buttons |= XInputButton.DpadRight;
            if (state.GetAxis(0x11) < 0 || state.GetKey(0x220)) buttons |= XInputButton.DpadUp;
            if (state.GetAxis(0x11) > 0 || state.GetKey(0x221)) buttons |= XInputButton.DpadDown;

            var drums = new XInputFourLaneDrumkitState()
            {
                buttons = buttons,
                redVelocity = (short)state.GetAxis(0),
                yellowVelocity = (short)~state.GetAxis(1),
                blueVelocity = (short)state.GetAxis(3),
                greenVelocity = (short)~state.GetAxis(4),
            };
            return TranslatingFourLaneDrumkit_Flags<XInputFourLaneDrumkitState>.TranslateState(ref drums, ref m_HasFlags);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.RequireXInputAxes(this);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFourLaneState>.VerifyDevice(this);
        }
    }
}
