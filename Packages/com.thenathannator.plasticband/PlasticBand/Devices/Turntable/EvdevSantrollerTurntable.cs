using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller Evdev DJ Hero Turntable")]
    internal class EvdevSantrollerTurntable : Turntable, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x13A, 0x13B, 0x13C, 0x132, 0x135, 0x136, 0x137, 0x138, 0x139
        };

        private TranslateStateHandler<EvdevState, TranslatedTurntableState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerTurntable>(SantrollerDeviceType.DjHeroTurntable, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 3, 4, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedTurntableState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedTurntableState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedTurntableState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedTurntableState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < s_Keys.Length; bit++)
            {
                if (state.GetKey(s_Keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }

            var legacy = new SantrollerHIDTurntableState()
            {
                buttons = (SantrollerHIDTurntableState.Button)buttons,
                dpad = state.GetDpad(),
                leftVelocity = (sbyte)(state.GetAxis(0) - 128),
                rightVelocity = (sbyte)(state.GetAxis(1) - 128),
                effectsDial = (ushort)(state.GetAxis(3) << 8),
                crossfader = (sbyte)(state.GetAxis(4) - 128),
            };
            return TranslatingTurntable<SantrollerHIDTurntableState>.TranslateState(ref legacy);
        }
    }
}
