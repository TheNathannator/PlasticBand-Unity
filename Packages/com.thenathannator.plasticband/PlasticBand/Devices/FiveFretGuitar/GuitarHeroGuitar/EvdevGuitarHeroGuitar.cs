using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Evdev Guitar Hero Guitar")]
    internal class EvdevGuitarHeroGuitar : GuitarHeroGuitar, IInputStateCallbackReceiver
    {
        private EvdevCapabilities m_Capabilities;
        private int m_ButtonBase;
        private TranslateStateHandler<EvdevState, TranslatedGuitarHeroGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevGuitarHeroGuitar>(SantrollerDeviceType.GuitarHeroGuitar);
            EvdevLayoutFinder.RegisterXInput<EvdevGuitarHeroGuitar>(7);
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("xpad", 0x1430, 0x4748);
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("xpad", 0x1430, 0x4734);
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("xpad", 0x1430, 0x0705);
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("xpad", 0x1430, 0x0706);
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("Xbox 360 Wireless Alternate Guitar");
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("sony", 0x12BA, 0x0100);
            EvdevLayoutFinder.Register<EvdevGuitarHeroGuitar>("sony", 0x1430, 0x474C);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Capabilities = EvdevLayoutFinder.GetCapabilities(this);
            if (m_Capabilities.driver == "hid-generic")
                EvdevSantrollerState.Validate(this, SantrollerDeviceType.GuitarHeroGuitar);
            else if (m_Capabilities.driver == "sony")
            {
                m_ButtonBase = m_Capabilities.SonyButtonBase;
                m_Capabilities.RequireAxes(2, 4, 5, 0x10, 0x11);
            }
            else
                m_Capabilities.RequireAxes(0, 1, 3, 4);
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
            if (m_Capabilities.driver == "hid-generic")
                return EvdevSantrollerState.GuitarHero(ref state, m_Capabilities.santrollerProtocol);
            if (m_Capabilities.driver == "xpad")
            {
                var raw = state.ToXInput();
                var guitar = *(XInputGuitarHeroGuitarState*)&raw;
                if ((m_Capabilities.vendorId == 0x1430 && m_Capabilities.productId == 0x0705) ||
                    (m_Capabilities.vendorId == 0x1209 &&
                        (m_Capabilities.productId == 0x2882 || m_Capabilities.productId == 0x2883)))
                {
                    var gh5 = new XInputGuitarHeroGuitarState_GH5() { state = guitar };
                    return TranslatingGuitarHeroGuitar_Discrete<XInputGuitarHeroGuitarState_GH5>.TranslateState(ref gh5);
                }
                return TranslatingGuitarHeroGuitar_Ranges<XInputGuitarHeroGuitarState>.TranslateState(ref guitar);
            }

            var sony = new PS3GuitarHeroGuitarState_NoReportId()
            {
                buttons = state.GetSonyButtons(m_ButtonBase),
                dpad = state.GetDpad(),
                m_Whammy = (byte)state.GetAxis(2),
                m_TouchBar = (byte)state.GetAxis(5),
                m_Tilt = (short)state.GetAxis(4),
            };
            if (description.product == "Guitar Hero5 for PlayStation (R) 3")
                return TranslatingGuitarHeroGuitar_Discrete<PS3GuitarHeroGuitarState_NoReportId>.TranslateState(ref sony);
            return TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_NoReportId>.TranslateState(ref sony);
        }
    }
}
