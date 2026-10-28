using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Santroller Evdev Rock Band Guitar")]
    internal class EvdevSantrollerRockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x138, 0x139, 0x132, 0x135, 0x13A, 0x13B, 0x13C
        };

        private TranslateStateHandler<EvdevState, TranslatedRockBandGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerRockBandGuitar>(SantrollerDeviceType.RockBandGuitar, 1);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 3, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private unsafe TranslatedRockBandGuitarState TranslateState(ref EvdevState state)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < s_Keys.Length; bit++)
            {
                if (state.GetKey(s_Keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }

            var legacy = new SantrollerHIDRockBandGuitarState()
            {
                buttons = (SantrollerHIDRockBandGuitarState.Button)buttons,
                dpad = state.GetDpad(),
                whammy = (byte)state.GetAxis(0),
                pickupSwitch = (byte)state.GetAxis(1),
                tilt = (sbyte)(state.GetAxis(3) - 128),
            };
            return TranslatingRockBandGuitar_Distinct<SantrollerHIDRockBandGuitarState>.TranslateState(ref legacy);
        }
    }
}

