using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PS4RockBandGuitarLayout), displayName = "Evdev PlayStation 4 Rock Band Guitar")]
    internal class EvdevSonyPS4RockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private TranslateStateHandler<EvdevState, TranslatedRockBandGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevSonyPS4RockBandGuitar>("sony", 0x0738, 0x8261);
            EvdevLayoutFinder.Register<EvdevSonyPS4RockBandGuitar>("sony", 0x0E6F, 0x0173);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            capabilities.RequireAxes(2, 5, 0x10, 0x11);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedRockBandGuitarState TranslateState(ref EvdevState state)
        {
            var buttons = EvdevSonyState.GetButtons(ref state, m_ButtonBase);
            bool solo = (buttons & PS4Button1.L3) != 0;
            byte frets = EvdevSonyState.GetFrets(buttons);
            // Only the shared fret buttons and solo flag survive HID parsing.
            var guitar = new PS4RockBandGuitarState_NoReportId()
            {
                buttons1 = buttons,
                buttons2 = EvdevSonyState.GetSystemButtons(ref state, m_ButtonBase),
                frets = solo ? (byte)0 : frets, soloFrets = solo ? frets : (byte)0,
                whammy = (byte)state.GetAxis(2), tilt = (sbyte)(state.GetAxis(5) >> 1),
            };
            return TranslatingRockBandGuitar_Distinct<PS4RockBandGuitarState_NoReportId>.TranslateState(ref guitar);
        }
    }
}
