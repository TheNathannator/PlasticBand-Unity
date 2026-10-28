using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller v2 Evdev DJ Hero Turntable")]
    internal class EvdevSantrollerV2Turntable : Turntable, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, TranslatedTurntableState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2Turntable>(SantrollerDeviceType.DjHeroTurntable, 2);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedTurntableState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedTurntableState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedTurntableState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedTurntableState TranslateState(ref EvdevState state)
        {
            byte leftTrig = (byte)state.GetAxis(2);
            byte rightTrig = (byte)state.GetAxis(5);

            var left = TurntableButton.None;
            var right = TurntableButton.None;
            if ((leftTrig & 0x01) != 0) left |= TurntableButton.Green;
            if ((leftTrig & 0x02) != 0) left |= TurntableButton.Red;
            if ((leftTrig & 0x04) != 0) left |= TurntableButton.Blue;

            if ((rightTrig & 0x01) != 0) right |= TurntableButton.Green;
            if ((rightTrig & 0x02) != 0) right |= TurntableButton.Red;
            if ((rightTrig & 0x04) != 0) right |= TurntableButton.Blue;

            var tableButtons = left | right;

            bool south = state.GetKey(0x130);
            bool east = state.GetKey(0x131);
            bool west = state.GetKey(0x133);
            bool north = state.GetKey(0x134);

            var buttons = TranslatedTurntableButtonMask.None;
            if ((tableButtons & TurntableButton.Green) == 0 && south) buttons |= TranslatedTurntableButtonMask.South;
            if ((tableButtons & TurntableButton.Red) == 0 && east) buttons |= TranslatedTurntableButtonMask.East;
            if ((tableButtons & TurntableButton.Blue) == 0 && west) buttons |= TranslatedTurntableButtonMask.West;
            if (north) buttons |= TranslatedTurntableButtonMask.North_Euphoria;

            if (state.GetAxis(0x11) < 0) buttons |= TranslatedTurntableButtonMask.DpadUp;
            if (state.GetAxis(0x11) > 0) buttons |= TranslatedTurntableButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0) buttons |= TranslatedTurntableButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= TranslatedTurntableButtonMask.DpadRight;

            if (state.GetKey(0x13B)) buttons |= TranslatedTurntableButtonMask.Start;
            if (state.GetKey(0x13A)) buttons |= TranslatedTurntableButtonMask.Select;
            if (state.GetKey(0x13C)) buttons |= TranslatedTurntableButtonMask.System;

            return new TranslatedTurntableState()
            {
                buttons = (ushort)buttons,
                turntableButtons = (byte)((byte)left | ((byte)right << 3)),
                leftTableVelocity = (sbyte)(state.GetAxis(0) & 0xFF),
                rightTableVelocity = (sbyte)(state.GetAxis(1) & 0xFF),
                effectsDial = (ushort)state.GetAxis(3),
                crossfader = (sbyte)(state.GetAxis(4) >> 8),
            };
        }
    }
}
