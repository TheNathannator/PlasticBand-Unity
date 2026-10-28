using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Santroller Evdev Guitar Hero Drumkit")]
    internal class EvdevSantrollerFiveLaneDrumkit : FiveLaneDrumkit, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x13A, 0x13B, 0x13C
        };

        private TranslateStateHandler<EvdevState, TranslatedFiveLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerFiveLaneDrumkit>(SantrollerDeviceType.GuitarHeroDrums, 1);
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

        private unsafe TranslatedFiveLaneState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < s_Keys.Length; bit++)
            {
                if (state.GetKey(s_Keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }

            var legacy = new SantrollerFiveLaneDrumkitState()
            {
                buttons = (SantrollerFiveLaneDrumkitState.Button)buttons,
                dpad = state.GetDpad(),
                greenVelocity = (byte)state.GetAxis(0),
                redVelocity = (byte)state.GetAxis(1),
                yellowVelocity = (byte)state.GetAxis(3),
                blueVelocity = (byte)state.GetAxis(4),
                orangeVelocity = (byte)state.GetAxis(2),
                kickVelocity = (byte)state.GetAxis(5),
            };
            return TranslatingFiveLaneDrumkit<SantrollerFiveLaneDrumkitState>.TranslateState(ref legacy);
        }
    }
}

