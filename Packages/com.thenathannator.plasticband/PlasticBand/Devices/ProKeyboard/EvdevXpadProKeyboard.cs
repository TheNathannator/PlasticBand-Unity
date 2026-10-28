using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    // Xpad's standard trigger and stick events preserve the keyboard key bitmap,
    // overdrive and pedal bytes. Velocity bytes are retained in state but have no
    // public InputControl; the XInput layout does not expose velocity1..5. XInput
    // also omits the touch strip, so xpad has no corresponding evdev field.
    [InputControlLayout(stateType = typeof(XInputProKeyboardState), displayName = "Evdev XInput Rock Band Pro Keyboard")]
    internal class EvdevXpadProKeyboard : ProKeyboard, IInputStateCallbackReceiver
    {
        private TranslateStateHandler<EvdevState, XInputProKeyboardState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevXpadProKeyboard>(15);
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
            if (state.GetAxis(0x10) < 0 || state.GetKey(0x222)) buttons |= (ushort)XInputButton.DpadLeft;
            if (state.GetAxis(0x10) > 0 || state.GetKey(0x223)) buttons |= (ushort)XInputButton.DpadRight;
            if (state.GetAxis(0x11) < 0 || state.GetKey(0x220)) buttons |= (ushort)XInputButton.DpadUp;
            if (state.GetAxis(0x11) > 0 || state.GetKey(0x221)) buttons |= (ushort)XInputButton.DpadDown;

            short leftStickY = (short)~state.GetAxis(1);
            short rightStickY = (short)~state.GetAxis(4);

            return new XInputProKeyboardState
            {
                buttons = buttons,
                keys1 = (byte)state.GetAxis(2),
                keys2 = (byte)state.GetAxis(5),
                keys3 = (byte)state.GetAxis(0),
                velocity1 = (byte)(state.GetAxis(0) >> 8),
                velocity2 = (byte)leftStickY,
                velocity3 = (byte)(leftStickY >> 8),
                velocity4 = (byte)state.GetAxis(3),
                velocity5 = (byte)(state.GetAxis(3) >> 8),
                overdrive = (byte)rightStickY,
                pedal = (byte)(rightStickY >> 8),
                touchPad = 0,
            };
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            EvdevLayoutFinder.RequireXInputAxes(this);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, XInputProKeyboardState>.VerifyDevice(this);
        }
    }
}
