using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "Evdev PlayStation Riffmaster / CRKD Guitar")]
    internal class EvdevSonyRiffmasterGuitar : RiffmasterGuitar, IInputStateCallbackReceiver
    {
        private int m_ButtonBase;
        private TranslateStateHandler<EvdevState, TranslatedRiffmasterGuitarState> m_Translator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x0E6F, 0x024A);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x0E6F, 0x0249);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x3651, 0x1500);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x3651, 0x5500);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x3651, 0x1600);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x3651, 0x5600);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x3958, 0x5500);
            EvdevLayoutFinder.Register<EvdevSonyRiffmasterGuitar>("sony", 0x3958, 0x5600);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var capabilities = EvdevLayoutFinder.GetCapabilities(this);
            m_ButtonBase = capabilities.SonyButtonBase;
            capabilities.RequireAxes(0, 1, 2, 5, 0x10, 0x11);
            m_Translator = TranslateState;
            StateTranslator<EvdevState, TranslatedRiffmasterGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedRiffmasterGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedRiffmasterGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedRiffmasterGuitarState TranslateState(ref EvdevState state)
        {
            var buttons = EvdevSonyState.GetButtons(ref state, m_ButtonBase);
            bool solo = (buttons & PS4Button1.L3) != 0;
            byte frets = EvdevSonyState.GetFrets(buttons);
            var guitar = new PS4RiffmasterGuitarState_NoReportId()
            {
                buttons1 = buttons,
                buttons2 = EvdevSonyState.GetSystemButtons(ref state, m_ButtonBase),
                frets = solo ? (byte)0 : frets, soloFrets = solo ? frets : (byte)0,
                joystickX = (sbyte)(state.GetAxis(0) - 0x80),
                joystickY = (sbyte)(0x7F - state.GetAxis(1)),
                whammy = (byte)state.GetAxis(2), tilt = (sbyte)(state.GetAxis(5) >> 1),
            };
            var translated = TranslatingRiffmasterGuitar<PS4RiffmasterGuitarState_NoReportId>.TranslateState(ref guitar);
            // Without the independent solo report, L3 cannot distinguish click from solo.
            translated.buttons &= (ushort)~TranslatedRiffmasterGuitarButtonMask.JoystickClick;
            return translated;
        }
    }
}
