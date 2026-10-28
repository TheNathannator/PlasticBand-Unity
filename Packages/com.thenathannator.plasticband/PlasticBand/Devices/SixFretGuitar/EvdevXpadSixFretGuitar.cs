using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Evdev XInput Guitar Hero Live Guitar")]
    internal class EvdevXpadSixFretGuitar : SixFretGuitar, IInputStateCallbackReceiver
    {
        private TranslateStateHandler<EvdevState, TranslatedSixFretState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevXpadSixFretGuitar>("xpad", 0x1430, 0x079B);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.GetCapabilities(this).RequireAxes(1, 3, 4);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedSixFretState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedSixFretState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedSixFretState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedSixFretState TranslateState(ref EvdevState state)
        {
            short strum = (short)~state.GetAxis(1);
            var translated = new TranslatedSixFretState()
            {
                whammy = (byte)(state.GetAxis(4) >> 7),
                tilt = (sbyte)(state.GetAxis(3) >> 8),
            };

            var frets = SixFret.None;
            if (state.GetKey(0x130)) frets |= SixFret.Black1;
            if (state.GetKey(0x131)) frets |= SixFret.Black2;
            if (state.GetKey(0x134)) frets |= SixFret.Black3;
            if (state.GetKey(0x133)) frets |= SixFret.White1;
            if (state.GetKey(0x136)) frets |= SixFret.White2;
            if (state.GetKey(0x137)) frets |= SixFret.White3;
            translated.frets = (byte)frets;

            var buttons = TranslatedSixFretButtonMask.None;
            if (strum > 0 || state.GetAxis(0x11) < 0 || state.GetKey(0x220)) buttons |= TranslatedSixFretButtonMask.DpadUp;
            if (strum < 0 || state.GetAxis(0x11) > 0 || state.GetKey(0x221)) buttons |= TranslatedSixFretButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0 || state.GetKey(0x222)) buttons |= TranslatedSixFretButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0 || state.GetKey(0x223)) buttons |= TranslatedSixFretButtonMask.DpadRight;

            if (state.GetKey(0x13B)) buttons |= TranslatedSixFretButtonMask.Start;
            if (state.GetKey(0x13A)) buttons |= TranslatedSixFretButtonMask.Select;
            if (state.GetKey(0x13D)) buttons |= TranslatedSixFretButtonMask.GHTV;
            if (state.GetKey(0x13C)) buttons |= TranslatedSixFretButtonMask.System;
            translated.buttons = (ushort)buttons;

            return translated;
        }
    }
}
