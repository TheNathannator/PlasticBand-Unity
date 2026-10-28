using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Evdev XInput Guitar Hero Guitar")]
    internal class EvdevXpadGuitarHeroGuitar : GuitarHeroGuitar, IInputStateCallbackReceiver
    {
        private bool m_IsGh5;
        private TranslateStateHandler<EvdevState, TranslatedGuitarHeroGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadGuitarHeroGuitar>(7);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("xpad", 0x1430, 0x4748);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("xpad", 0x1430, 0x4734);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("xpad", 0x1430, 0x0705);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("xpad", 0x1430, 0x0706);
            EvdevLayoutFinder.Register<EvdevXpadGuitarHeroGuitar>("Xbox 360 Wireless Alternate Guitar");
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 3, 4);
            m_IsGh5 = (caps.vendorId == 0x1430 && caps.productId == 0x0705) ||
                      (caps.vendorId == 0x1209 && (caps.productId == 0x2882 || caps.productId == 0x2883));
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
            if (state.GetAxis(0x11) < 0 || state.GetKey(0x220)) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadUp;
            if (state.GetAxis(0x11) > 0 || state.GetKey(0x221)) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0 || state.GetKey(0x222)) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0 || state.GetKey(0x223)) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadRight;

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

            byte touchFrets;
            if (m_IsGh5)
            {
                byte rawTouch = (byte)(state.GetAxis(0) ^ 0x80);
                touchFrets = (byte)TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[rawTouch];
            }
            else
            {
                byte rawTouch = (byte)((state.GetAxis(0) >> 8) ^ 0x80);
                touchFrets = (byte)TranslatingGuitarHeroGuitar.s_WtTouchFrets[(rawTouch & 0xF0) >> 4];
            }

            short tiltAxis = (short)~state.GetAxis(4);
            return new TranslatedGuitarHeroGuitarState()
            {
                buttons = (ushort)buttons,
                frets = (byte)frets,
                touchFrets = touchFrets,
                whammy = (byte)((state.GetAxis(3) >> 8) + 0x80),
                tilt = (sbyte)(tiltAxis >> 8),
            };
        }
    }
}
