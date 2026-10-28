using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;

namespace PlasticBand.Devices
{
    // Xpad's standard trigger and stick events preserve the keyboard key bitmap,
    // overdrive and pedal bytes. Velocity bytes are retained in state but have no
    // public InputControl; the XInput layout does not expose velocity1..5. XInput
    // also omits the touch strip, so xpad has no corresponding evdev field.
    [InputControlLayout(stateType = typeof(XInputProKeyboardState), displayName = "Evdev Rock Band Pro Keyboard")]
    internal class EvdevProKeyboard : ProKeyboard, IInputStateCallbackReceiver
    {
        private bool m_InvertY = true;
        private TranslateStateHandler<EvdevState, XInputProKeyboardState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevProKeyboard>(15);
            EvdevLayoutFinder.RegisterSantroller<EvdevProKeyboard>(SantrollerDeviceType.ProKeyboard, legacy: false);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, XInputProKeyboardState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, XInputProKeyboardState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private XInputProKeyboardState TranslateState(ref EvdevState state)
        {
            var raw = state.ToXInput(m_InvertY);
            return new XInputProKeyboardState
            {
                buttons = raw.buttons,
                keys1 = (byte)raw.leftTrigger,
                keys2 = (byte)raw.rightTrigger,
                keys3 = (byte)raw.leftStickX,
                velocity1 = (byte)(raw.leftStickX >> 8),
                velocity2 = (byte)raw.leftStickY,
                velocity3 = (byte)(raw.leftStickY >> 8),
                velocity4 = (byte)raw.rightStickX,
                velocity5 = (byte)(raw.rightStickX >> 8),
                overdrive = (byte)raw.rightStickY,
                pedal = (byte)(raw.rightStickY >> 8),
                // The XInput gamepad report has no field for the keyboard touch strip.
                touchPad = 0,
            };
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            if (EvdevLayoutFinder.GetCapabilities(this).driver == "hid-generic")
            {
                EvdevSantrollerState.Validate(this, SantrollerDeviceType.ProKeyboard);
                m_InvertY = false;
            }
            else
                EvdevLayoutFinder.RequireXInputAxes(this);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, XInputProKeyboardState>.VerifyDevice(this);
        }
    }
}
