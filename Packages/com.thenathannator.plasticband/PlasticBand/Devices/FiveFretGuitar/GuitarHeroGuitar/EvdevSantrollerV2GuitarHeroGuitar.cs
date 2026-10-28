using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller v2 Evdev Guitar Hero Guitar")]
    internal class EvdevSantrollerV2GuitarHeroGuitar : GuitarHeroGuitar, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, TranslatedGuitarHeroGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2GuitarHeroGuitar>(SantrollerDeviceType.GuitarHeroGuitar, 2);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
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
            var buttons = TranslatedGuitarHeroGuitarButtonMask.None;
            if (state.GetAxis(0x11) < 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadUp;
            if (state.GetAxis(0x11) > 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadRight;

            if (state.GetKey(0x13B)) buttons |= TranslatedGuitarHeroGuitarButtonMask.Start;
            if (state.GetKey(0x13A)) buttons |= TranslatedGuitarHeroGuitarButtonMask.Select;
            if (state.GetKey(0x13C)) buttons |= TranslatedGuitarHeroGuitarButtonMask.System;
            if (state.GetKey(0x137)) buttons |= TranslatedGuitarHeroGuitarButtonMask.Pedal;

            var frets = FiveFret.None;
            if (state.GetKey(0x130)) frets |= FiveFret.Green;
            if (state.GetKey(0x131)) frets |= FiveFret.Red;
            if (state.GetKey(0x134)) frets |= FiveFret.Yellow;
            if (state.GetKey(0x133)) frets |= FiveFret.Blue;
            if (state.GetKey(0x136)) frets |= FiveFret.Orange;

            byte rawTouch = (byte)(state.GetAxis(0) ^ 0x80);

            return new TranslatedGuitarHeroGuitarState()
            {
                buttons = (ushort)buttons,
                frets = (byte)frets,
                touchFrets = (byte)TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[rawTouch],
                whammy = (byte)((state.GetAxis(3) >> 8) + 0x80),
                tilt = (sbyte)(state.GetAxis(4) >> 8),
            };
        }
    }
}
