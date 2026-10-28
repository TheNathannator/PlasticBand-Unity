using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Santroller v2 Evdev Rock Band Guitar")]
    internal class EvdevSantrollerV2RockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, TranslatedRockBandGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2RockBandGuitar>(SantrollerDeviceType.RockBandGuitar, 2);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
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
            if (state.GetAxis(0x11) < 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadUp;
            if (state.GetAxis(0x11) > 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadRight;

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

            return new TranslatedRockBandGuitarState()
            {
                buttons = (ushort)buttons,
                frets = (byte)(!solo ? frets : FiveFret.None),
                soloFrets = (byte)(solo ? frets : FiveFret.None),
                whammy = (byte)((state.GetAxis(3) >> 8) + 0x80),
                tilt = (sbyte)(state.GetAxis(4) >> 8),
                pickupSwitch = (byte)RockBandGuitarState.GetPickupSwitchNotch((byte)state.GetAxis(2)),
            };
        }
    }
}
