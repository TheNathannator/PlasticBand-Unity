using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Santroller v2 Evdev Guitar Hero Drumkit")]
    internal class EvdevSantrollerV2FiveLaneDrumkit : FiveLaneDrumkit, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, TranslatedFiveLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2FiveLaneDrumkit>(SantrollerDeviceType.GuitarHeroDrums, 2);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFiveLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFiveLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFiveLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedFiveLaneState TranslateState(ref EvdevState state)
        {
            bool green = state.GetKey(0x130);
            bool red = state.GetKey(0x131);
            bool yellow = state.GetKey(0x134);
            bool blue = state.GetKey(0x133);
            bool orange = state.GetKey(0x137);
            bool kick = state.GetKey(0x136);

            byte greenVelocity = (byte)state.GetAxis(1);
            byte redVelocity = (byte)(state.GetAxis(1) >> 8);
            byte yellowVelocity = (byte)state.GetAxis(3);
            byte blueVelocity = (byte)(state.GetAxis(3) >> 8);
            byte orangeVelocity = (byte)state.GetAxis(4);
            byte kickVelocity = (byte)(state.GetAxis(4) >> 8);

            var translated = new TranslatedFiveLaneState()
            {
                greenPad     = green ? greenVelocity : (byte)0,
                redPad       = red ? redVelocity : (byte)0,
                yellowCymbal = yellow ? yellowVelocity : (byte)0,
                bluePad      = blue ? blueVelocity : (byte)0,
                orangeCymbal = orange ? orangeVelocity : (byte)0,
                kick         = kick ? kickVelocity : (byte)0,
            };

            var buttons = TranslatedFiveLaneButtonMask.None;
            if (translated.greenPad == 0 && green) buttons |= TranslatedFiveLaneButtonMask.South;
            if (translated.redPad == 0 && red) buttons |= TranslatedFiveLaneButtonMask.East;
            if (translated.bluePad == 0 && blue) buttons |= TranslatedFiveLaneButtonMask.West;
            if (translated.yellowCymbal == 0 && yellow) buttons |= TranslatedFiveLaneButtonMask.North;

            if (state.GetAxis(0x11) < 0) buttons |= TranslatedFiveLaneButtonMask.DpadUp;
            if (state.GetAxis(0x11) > 0) buttons |= TranslatedFiveLaneButtonMask.DpadDown;
            if (state.GetAxis(0x10) < 0) buttons |= TranslatedFiveLaneButtonMask.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= TranslatedFiveLaneButtonMask.DpadRight;

            if (state.GetKey(0x13B)) buttons |= TranslatedFiveLaneButtonMask.Start;
            if (state.GetKey(0x13A)) buttons |= TranslatedFiveLaneButtonMask.Select;
            if (state.GetKey(0x13C)) buttons |= TranslatedFiveLaneButtonMask.System;

            translated.buttons = (ushort)buttons;
            return translated;
        }
    }
}
