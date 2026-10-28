using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Evdev Rock Band Guitar")]
    internal class EvdevRockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private EvdevCapabilities m_Capabilities;
        private int m_ButtonBase;
        private byte m_LastWhammy;
        private int m_LastPickup;
        private TranslateStateHandler<EvdevState, TranslatedRockBandGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevRockBandGuitar>(SantrollerDeviceType.RockBandGuitar);
            EvdevLayoutFinder.RegisterXInput<EvdevRockBandGuitar>(6);
            EvdevLayoutFinder.RegisterXInput<EvdevRockBandGuitar>(11);
            EvdevLayoutFinder.Register<EvdevRockBandGuitar>("xpad", 0x1BAD, 0x0002);
            EvdevLayoutFinder.Register<EvdevRockBandGuitar>("Xbox 360 Wireless Guitar");
            EvdevLayoutFinder.Register<EvdevRockBandGuitar>("Xbox 360 Wireless Bass Guitar");
            EvdevLayoutFinder.Register<EvdevRockBandGuitar>("sony", 0x12BA, 0x0200);
            EvdevLayoutFinder.Register<EvdevRockBandGuitar>("sony", 0x1BAD, 0x0004);
            EvdevLayoutFinder.Register<EvdevRockBandGuitar>("sony", 0x1BAD, 0x3010);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Capabilities = EvdevLayoutFinder.GetCapabilities(this);
            if (m_Capabilities.driver == "hid-generic")
                EvdevSantrollerState.Validate(this, SantrollerDeviceType.RockBandGuitar);
            else if (m_Capabilities.driver == "sony")
            {
                m_ButtonBase = m_Capabilities.SonyButtonBase;
                m_Capabilities.RequireAxes(2, 5, 0x10, 0x11);
            }
            else
                m_Capabilities.RequireAxes(2, 3, 4);
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
            if (m_Capabilities.driver == "hid-generic")
                return EvdevSantrollerState.RockBand(ref state, m_Capabilities.santrollerProtocol);
            if (m_Capabilities.driver == "xpad")
            {
                var raw = state.ToXInput();
                var guitar = *(XInputRockBandGuitarState*)&raw;
                return TranslatingRockBandGuitar_Flags<XInputRockBandGuitarState>.TranslateState(ref guitar);
            }

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
