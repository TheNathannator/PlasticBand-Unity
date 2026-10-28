using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Evdev XInput Rock Band Guitar")]
    internal class EvdevXpadRockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private TranslateStateHandler<EvdevState, TranslatedRockBandGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadRockBandGuitar>(6);
            EvdevLayoutFinder.RegisterXInput<EvdevXpadRockBandGuitar>(11);
            EvdevLayoutFinder.Register<EvdevXpadRockBandGuitar>("xpad", 0x1BAD, 0x0002);
            EvdevLayoutFinder.Register<EvdevXpadRockBandGuitar>("Xbox 360 Wireless Guitar");
            EvdevLayoutFinder.Register<EvdevXpadRockBandGuitar>("Xbox 360 Wireless Bass Guitar");
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.GetCapabilities(this).RequireAxes(2, 3, 4);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedRockBandGuitarState TranslateState(ref EvdevState state)
        {
            var buttons = TranslatedRockBandGuitarButtonMask.None;
            if (state.GetAxis(0x11) < 0 || state.GetKey(0x220)) buttons |= TranslatedRockBandGuitarButtonMask.DpadUp;
            if (state.GetAxis(0x11) > 0 || state.GetKey(0x221)) buttons |= TranslatedRockBandGuitarButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0 || state.GetKey(0x222)) buttons |= TranslatedRockBandGuitarButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0 || state.GetKey(0x223)) buttons |= TranslatedRockBandGuitarButtonMask.DpadRight;

            if (state.GetKey(0x13B)) buttons |= TranslatedRockBandGuitarButtonMask.Start;
            if (state.GetKey(0x13A)) buttons |= TranslatedRockBandGuitarButtonMask.Select;
            if (state.GetKey(0x13C)) buttons |= TranslatedRockBandGuitarButtonMask.System;

            var frets = FiveFret.None;
            if (state.GetKey(0x130)) frets |= FiveFret.Green;
            if (state.GetKey(0x131)) frets |= FiveFret.Red;
            if (state.GetKey(0x134)) frets |= FiveFret.Yellow;
            if (state.GetKey(0x133)) frets |= FiveFret.Blue;
            if (state.GetKey(0x136)) frets |= FiveFret.Orange;

            bool solo = state.GetKey(0x13D);
            short tiltAxis = (short)~state.GetAxis(4);

            return new TranslatedRockBandGuitarState()
            {
                buttons = (ushort)buttons,
                frets = (byte)(!solo ? frets : FiveFret.None),
                soloFrets = (byte)(solo ? frets : FiveFret.None),
                whammy = (byte)((state.GetAxis(3) >> 8) + 0x80),
                tilt = (sbyte)(tiltAxis >> 8),
                pickupSwitch = (byte)RockBandGuitarState.GetPickupSwitchNotch((byte)state.GetAxis(2)),
            };
        }
    }
}
