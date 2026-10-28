using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS3TurntableLayout), displayName = "Evdev PlayStation DJ Hero Turntable")]
    internal class EvdevSonyTurntable : Turntable, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private TranslateStateHandler<EvdevState, TranslatedTurntableState> m_Translator;

        internal new static void Initialize()
            => EvdevLayoutFinder.Register<EvdevSonyTurntable>("sony", 0x12BA, 0x0140);

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            capabilities.RequireAxes(0, 1, 3, 4, 5);
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
            // Despite the driver's usage names, platter bits land on RX and crossfader on RZ.
            var table = new PS3TurntableState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase), dpad = state.GetDpad(),
                leftVelocity = (sbyte)(state.GetAxis(0) - 0x80),
                rightVelocity = (sbyte)(state.GetAxis(1) - 0x80),
                effectsDial = (ushort)(state.GetAxis(4) << 6),
                crossfader = (sbyte)((state.GetAxis(5) >> 2) - 0x80),
                tableButtons = (short)state.GetAxis(3),
            };
            return TranslatingTurntable<PS3TurntableState_NoReportId>.TranslateState(ref table);
        }
    }
}
