using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Evdev PlayStation Rock Band Guitar")]
    internal class EvdevSonyRockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private byte m_LastWhammy;
        private int m_LastPickup;
        private TranslateStateHandler<EvdevState, TranslatedRockBandGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevSonyRockBandGuitar>("sony", 0x12BA, 0x0200);
            EvdevLayoutFinder.Register<EvdevSonyRockBandGuitar>("sony", 0x1BAD, 0x0004);
            EvdevLayoutFinder.Register<EvdevSonyRockBandGuitar>("sony", 0x1BAD, 0x3010);
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
            var sony = new PS3WiiRockBandGuitarState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase),
                dpad = state.GetDpad(),
                m_Whammy = (byte)state.GetAxis(2),
                m_PickupSwitch = (byte)state.GetAxis(5),
            };
            if (sony.whammy == RockBandGuitarState.kNullValue)
                sony.whammy = m_LastWhammy;
            if (sony.pickupSwitch < 0)
                sony.pickupSwitch = m_LastPickup;
            m_LastWhammy = sony.whammy;
            m_LastPickup = sony.pickupSwitch;
            return TranslatingRockBandGuitar_Flags<PS3WiiRockBandGuitarState_NoReportId>.TranslateState(ref sony);
        }
    }
}

