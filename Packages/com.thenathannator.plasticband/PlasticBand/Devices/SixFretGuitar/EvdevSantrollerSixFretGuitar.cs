using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Santroller Evdev Guitar Hero Live Guitar")]
    internal class EvdevSantrollerSixFretGuitar : SixFretGuitar, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x13A, 0x13B, 0x13D, 0x13C
        };

        private TranslateStateHandler<EvdevState, TranslatedSixFretState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerSixFretGuitar>(SantrollerDeviceType.LiveGuitar, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedSixFretState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedSixFretState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedSixFretState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedSixFretState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < s_Keys.Length; bit++)
            {
                if (state.GetKey(s_Keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }

            var legacy = new SantrollerHIDSixFretGuitarState()
            {
                buttons = (SantrollerHIDSixFretGuitarState.Button)buttons,
                dpad = state.GetDpad(),
                whammy = (byte)state.GetAxis(0),
                tilt = (sbyte)(state.GetAxis(1) - 128),
            };
            return TranslatingSixFretGuitar<SantrollerHIDSixFretGuitarState>.TranslateState(ref legacy);
        }
    }
}
