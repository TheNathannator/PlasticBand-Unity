using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Evdev PlayStation Guitar Hero Live Guitar")]
    internal class EvdevSonySixFretGuitar : SixFretGuitar, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private bool m_PS4;
        private TranslateStateHandler<EvdevState, TranslatedSixFretState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevSonySixFretGuitar>("sony", 0x12BA, 0x074B);
            EvdevLayoutFinder.Register<EvdevSonySixFretGuitar>("sony", 0x1430, 0x07BB);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            m_PS4 = capabilities.vendorId == 0x1430;
            capabilities.RequireAxes(1, 5, 0x10, 0x11);
            if (m_PS4)
                capabilities.RequireAxes(2);
            else
                capabilities.RequireAxes(4);
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
            if (m_PS4)
            {
                var guitar = new PS4SixFretGuitarState_NoReportId()
                {
                    buttons1 = EvdevSonyState.GetButtons(ref state, m_ButtonBase),
                    buttons2 = EvdevSonyState.GetSystemButtons(ref state, m_ButtonBase),
                    strumBar = (byte)state.GetAxis(1),
                    whammy = (byte)((state.GetAxis(2) - 0x80) * 2),
                    tilt = (sbyte)(state.GetAxis(5) - 0x80),
                };
                return TranslatingSixFretGuitar<PS4SixFretGuitarState_NoReportId>.TranslateState(ref guitar);
            }
            var ps3 = new PS3WiiUSixFretGuitarState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase),
                dpad = state.GetDpad(),
                strumBar = (byte)state.GetAxis(1),
                whammy = (byte)((state.GetAxis(5) - 0x80) * 2),
                tilt = (sbyte)(((state.GetAxis(4) & 0x3FF) >> 2) - 0x80),
            };
            return TranslatingSixFretGuitar<PS3WiiUSixFretGuitarState_NoReportId>.TranslateState(ref ps3);
        }
    }
}
