using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedProGuitarState), displayName = "Evdev Rock Band Pro Guitar")]
    internal class EvdevProGuitar : TranslatingProGuitar<XInputProGuitarState>, IInputStateCallbackReceiver
    {
        private bool m_InvertY = true;
        private TranslateStateHandler<EvdevState, TranslatedProGuitarState> m_EvdevTranslator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterXInput<EvdevProGuitar>(25);
            EvdevLayoutFinder.RegisterSantroller<EvdevProGuitar>(SantrollerDeviceType.ProGuitarMustang, legacy: false);
            EvdevLayoutFinder.RegisterSantroller<EvdevProGuitar>(SantrollerDeviceType.ProGuitarSquire, legacy: false);
        }

        unsafe void IInputStateCallbackReceiver.OnNextUpdate()
        {
            using (var buffer = StateEvent.From(this, out var eventPtr))
            {
                ref var translated = ref *(TranslatedProGuitarState*)StateEvent.From(eventPtr)->state;
                translated.buttons &= ~(uint)(TranslatedProGuitarButtonMask.Strum1 | TranslatedProGuitarButtonMask.Strum2 |
                    TranslatedProGuitarButtonMask.Strum3 | TranslatedProGuitarButtonMask.Strum4 |
                    TranslatedProGuitarButtonMask.Strum5 | TranslatedProGuitarButtonMask.Strum6);
                InputState.Change(this, eventPtr);
            }
        }

        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedProGuitarState>.OnStateEvent(this, eventPtr, m_EvdevTranslator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedProGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_EvdevTranslator);

        private unsafe TranslatedProGuitarState TranslateState(ref EvdevState state)
        {
            var raw = state.ToXInput(m_InvertY);
            var guitar = *(XInputProGuitarState*)&raw;
            return base.TranslateState(ref guitar);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            if (caps.driver == "hid-generic")
            {
                EvdevSantrollerState.Validate(this, (SantrollerDeviceType)caps.subType);
                m_InvertY = false;
            }
            else
                EvdevLayoutFinder.RequireXInputAxes(this);
            m_EvdevTranslator = TranslateState;
            StateTranslator<EvdevState, TranslatedProGuitarState>.VerifyDevice(this);
        }
    }
}
