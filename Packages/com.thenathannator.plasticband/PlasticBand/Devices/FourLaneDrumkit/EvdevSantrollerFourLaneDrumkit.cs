using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller Evdev Rock Band Drumkit")]
    internal class EvdevSantrollerFourLaneDrumkit : FourLaneDrumkit, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x13E, 0x137, 0x136, 0x13D, 0x13A, 0x13B, 0x13C
        };

        private bool m_HasFlags;
        private TranslateStateHandler<EvdevState, TranslatedFourLaneState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerFourLaneDrumkit>(SantrollerDeviceType.RockBandDrums, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 6, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedFourLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedFourLaneState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < s_Keys.Length; bit++)
            {
                if (state.GetKey(s_Keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }

            var legacy = new SantrollerFourLaneDrumkitState()
            {
                buttons = (SantrollerFourLaneDrumkitState.Button)buttons,
                dpad = state.GetDpad(),
                greenPadVelocity = (byte)state.GetAxis(0),
                redPadVelocity = (byte)state.GetAxis(1),
                yellowPadVelocity = (byte)state.GetAxis(3),
                bluePadVelocity = (byte)state.GetAxis(4),
                greenCymbalVelocity = (byte)state.GetAxis(2),
                yellowCymbalVelocity = (byte)state.GetAxis(5),
                blueCymbalVelocity = (byte)state.GetAxis(6),
            };
            return TranslatingFourLaneDrumkit_Hybrid<SantrollerFourLaneDrumkitState>.TranslateState(ref legacy, ref m_HasFlags);
        }
    }
}

