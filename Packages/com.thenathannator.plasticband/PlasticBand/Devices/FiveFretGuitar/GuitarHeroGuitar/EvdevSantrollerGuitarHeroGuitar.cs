using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller Evdev Guitar Hero Guitar")]
    internal class EvdevSantrollerGuitarHeroGuitar : GuitarHeroGuitar, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x13A, 0x13B, 0x13C
        };

        private TranslateStateHandler<EvdevState, TranslatedGuitarHeroGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerGuitarHeroGuitar>(SantrollerDeviceType.GuitarHeroGuitar, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 3, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedGuitarHeroGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedGuitarHeroGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedGuitarHeroGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedGuitarHeroGuitarState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < s_Keys.Length; bit++)
            {
                if (state.GetKey(s_Keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }

            var legacy = new SantrollerHIDGuitarHeroGuitarState()
            {
                buttons = (SantrollerHIDGuitarHeroGuitarState.Button)buttons,
                dpad = state.GetDpad(),
                m_Whammy = (byte)state.GetAxis(0),
                m_TouchBar = (byte)state.GetAxis(1),
                m_Tilt = (byte)state.GetAxis(3),
            };
            return TranslatingGuitarHeroGuitar_Discrete<SantrollerHIDGuitarHeroGuitarState>.TranslateState(ref legacy);
        }
    }
}

