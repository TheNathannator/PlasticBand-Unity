using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputProKeyboardState), displayName = "Santroller v2 Evdev Rock Band Pro Keyboard")]
    internal class EvdevSantrollerV2ProKeyboard : ProKeyboard, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, XInputProKeyboardState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2ProKeyboard>(SantrollerDeviceType.ProKeyboard, 2);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, XInputProKeyboardState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, XInputProKeyboardState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private XInputProKeyboardState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            if (state.GetKey(0x130)) buttons |= (ushort)XInputButton.A;
            if (state.GetKey(0x131)) buttons |= (ushort)XInputButton.B;
            if (state.GetKey(0x133)) buttons |= (ushort)XInputButton.X;
            if (state.GetKey(0x134)) buttons |= (ushort)XInputButton.Y;
            if (state.GetKey(0x136)) buttons |= (ushort)XInputButton.LeftShoulder;
            if (state.GetKey(0x137)) buttons |= (ushort)XInputButton.RightShoulder;
            if (state.GetKey(0x13A)) buttons |= (ushort)XInputButton.Back;
            if (state.GetKey(0x13B)) buttons |= (ushort)XInputButton.Start;
            if (state.GetKey(0x13C)) buttons |= (ushort)XInputButton.Guide;
            if (state.GetKey(0x13D)) buttons |= (ushort)XInputButton.LeftThumb;
            if (state.GetKey(0x13E)) buttons |= (ushort)XInputButton.RightThumb;
            if (state.GetAxis(0x10) < 0) buttons |= (ushort)XInputButton.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= (ushort)XInputButton.DpadRight;
            if (state.GetAxis(0x11) < 0) buttons |= (ushort)XInputButton.DpadUp;
            if (state.GetAxis(0x11) > 0) buttons |= (ushort)XInputButton.DpadDown;

            return new XInputProKeyboardState
            {
                buttons = buttons,
                keys1 = (byte)state.GetAxis(2),
                keys2 = (byte)state.GetAxis(5),
                keys3 = (byte)state.GetAxis(0),
                velocity1 = (byte)(state.GetAxis(0) >> 8),
                velocity2 = (byte)state.GetAxis(1),
                velocity3 = (byte)(state.GetAxis(1) >> 8),
                velocity4 = (byte)state.GetAxis(3),
                velocity5 = (byte)(state.GetAxis(3) >> 8),
                overdrive = (byte)state.GetAxis(4),
                pedal = (byte)(state.GetAxis(4) >> 8),
                touchPad = 0,
            };
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, XInputProKeyboardState>.VerifyDevice(this);
        }
    }
}
