using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedProGuitarState), displayName = "Santroller v2 Evdev Rock Band Pro Guitar")]
    internal class EvdevSantrollerV2ProGuitar : TranslatingProGuitar<XInputProGuitarState>, IInputStateCallbackReceiver
    {
        private static readonly int[] s_Keys =
        {
            0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E
        };

        private TranslateStateHandler<EvdevState, TranslatedProGuitarState> m_EvdevTranslator;

        internal new static void Initialize()
        {
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2ProGuitar>(SantrollerDeviceType.ProGuitarMustang, 2);
            EvdevLayoutFinder.RegisterSantroller<EvdevSantrollerV2ProGuitar>(SantrollerDeviceType.ProGuitarSquire, 2);
        }

        unsafe void IInputStateCallbackReceiver.OnNextUpdate()
        {
            using (var buffer = StateEvent.From(this, out var eventPtr))
            {
                ref var translated = ref *(TranslatedProGuitarState*)StateEvent.From(eventPtr)->state;
                translated.buttons &= ~(uint)(TranslatedProGuitarButtonMask.Strum1 | TranslatedProGuitarButtonMask.Strum2 |
                    TranslatedProGuitarButtonMask.Strum3 | TranslatedProGuitarButtonMask.Strum4 |
                    TranslatedProGuitarButtonMask.Strum5 | TranslatedProGuitarButtonMask.Strum6);
                InputState.Change(this, eventPtr);
            }
        }

        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<EvdevState, TranslatedProGuitarState>.OnStateEvent(this, eventPtr, m_EvdevTranslator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<EvdevState, TranslatedProGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_EvdevTranslator);

        private TranslatedProGuitarState TranslateState(ref EvdevState state)
        {
            XInputButton buttons = 0;
            if (state.GetKey(0x130)) buttons |= XInputButton.A;
            if (state.GetKey(0x131)) buttons |= XInputButton.B;
            if (state.GetKey(0x133)) buttons |= XInputButton.X;
            if (state.GetKey(0x134)) buttons |= XInputButton.Y;
            if (state.GetKey(0x13B)) buttons |= XInputButton.Start;
            if (state.GetKey(0x13A)) buttons |= XInputButton.Back;
            if (state.GetKey(0x13C)) buttons |= XInputButton.Guide;
            if (state.GetAxis(0x10) < 0) buttons |= XInputButton.DpadLeft;
            if (state.GetAxis(0x10) > 0) buttons |= XInputButton.DpadRight;
            if (state.GetAxis(0x11) < 0) buttons |= XInputButton.DpadUp;
            if (state.GetAxis(0x11) > 0) buttons |= XInputButton.DpadDown;

            short leftStickY = (short)state.GetAxis(1);
            short rightStickX = (short)state.GetAxis(3);
            short rightStickY = (short)state.GetAxis(4);

            var guitar = new XInputProGuitarState()
            {
                buttons = buttons,
                velocity1 = (byte)(leftStickY & 0x7F),
                velocity2 = (byte)((leftStickY >> 8) & 0x7F),
                velocity3 = (byte)(rightStickX & 0x7F),
                velocity4 = (byte)((rightStickX >> 8) & 0x7F),
                velocity5 = (byte)(rightStickY & 0x7F),
                velocity6 = (byte)((rightStickY >> 8) & 0x7F),
                green = (leftStickY & 0x80) != 0,
                red = ((leftStickY >> 8) & 0x80) != 0,
                yellow = (rightStickX & 0x80) != 0,
                blue = ((rightStickX >> 8) & 0x80) != 0,
                orange = (rightStickY & 0x80) != 0,
            };

            ushort frets1 = (ushort)((byte)state.GetAxis(2) | ((byte)state.GetAxis(5) << 8));
            ushort frets2 = (ushort)state.GetAxis(0);
            guitar.fret1 = (byte)frets1.GetMask(0x1F, 0);
            guitar.fret2 = (byte)frets1.GetMask(0x1F, 5);
            guitar.fret3 = (byte)frets1.GetMask(0x1F, 10);
            guitar.fret4 = (byte)frets2.GetMask(0x1F, 0);
            guitar.fret5 = (byte)frets2.GetMask(0x1F, 5);
            guitar.fret6 = (byte)frets2.GetMask(0x1F, 10);
            guitar.solo = (frets2 & 0x8000) != 0;

            return base.TranslateState(ref guitar);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            var caps = EvdevLayoutFinder.GetCapabilities(this);
            caps.RequireAxes(0, 1, 2, 3, 4, 5, 0x10, 0x11);
            caps.RequireKeys(s_Keys);
            m_EvdevTranslator = TranslateState;
            StateTranslator<EvdevState, TranslatedProGuitarState>.VerifyDevice(this);
        }
    }
}
