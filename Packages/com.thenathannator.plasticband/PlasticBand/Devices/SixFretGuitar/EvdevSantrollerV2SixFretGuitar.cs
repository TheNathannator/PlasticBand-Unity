using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Santroller v2 Evdev Guitar Hero Live Guitar")]
    internal class EvdevSantrollerV2SixFretGuitar : SixFretGuitar, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, TranslatedSixFretState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2SixFretGuitar>(SantrollerDeviceType.LiveGuitar, 2);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
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
            short strum = (short)state.GetAxis(1);
            if (strum > 0 || state.GetAxis(0x11) < 0) buttons |= TranslatedSixFretButtonMask.DpadUp;
            if (strum < 0 || state.GetAxis(0x11) > 0) buttons |= TranslatedSixFretButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0) buttons |= TranslatedSixFretButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= TranslatedSixFretButtonMask.DpadRight;

            if (state.GetKey(0x13B)) buttons |= TranslatedSixFretButtonMask.Start;
            if (state.GetKey(0x13A)) buttons |= TranslatedSixFretButtonMask.Select;
            if (state.GetKey(0x13D)) buttons |= TranslatedSixFretButtonMask.GHTV;
            if (state.GetKey(0x13C)) buttons |= TranslatedSixFretButtonMask.System;
            translated.buttons = (ushort)buttons;

            return translated;
        }
    }
}
