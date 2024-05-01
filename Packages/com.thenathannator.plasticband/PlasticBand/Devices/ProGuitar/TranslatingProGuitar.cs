using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedProGuitarButton
    {
        South = 0,
        East = 1,
        West = 2,
        North = 3,

        DpadUp = 4,
        DpadDown = 5,
        DpadLeft = 6,
        DpadRight = 7,

        Start = 8,
        Select = 9,
        System = 10,

        Tilt = 11,

        Strum1 = 16,
        Strum2 = 17,
        Strum3 = 18,
        Strum4 = 19,
        Strum5 = 20,
        Strum6 = 21,

        Green = 22,
        Red = 23,
        Yellow = 24,
        Blue = 25,
        Orange = 26,

        SoloGreen = 27,
        SoloRed = 28,
        SoloYellow = 29,
        SoloBlue = 30,
        SoloOrange = 31,
    }

    [Flags]
    internal enum TranslatedProGuitarButtonMask : uint
    {
        None = 0,

        South = 1 << TranslatedProGuitarButton.South,
        East = 1 << TranslatedProGuitarButton.East,
        West = 1 << TranslatedProGuitarButton.West,
        North = 1 << TranslatedProGuitarButton.North,

        DpadUp = 1 << TranslatedProGuitarButton.DpadUp,
        DpadDown = 1 << TranslatedProGuitarButton.DpadDown,
        DpadLeft = 1 << TranslatedProGuitarButton.DpadLeft,
        DpadRight = 1 << TranslatedProGuitarButton.DpadRight,

        Start = 1 << TranslatedProGuitarButton.Start,
        Select = 1 << TranslatedProGuitarButton.Select,
        System = 1 << TranslatedProGuitarButton.System,

        Tilt = 1 << TranslatedProGuitarButton.Tilt,

        Strum1 = 1 << TranslatedProGuitarButton.Strum1,
        Strum2 = 1 << TranslatedProGuitarButton.Strum2,
        Strum3 = 1 << TranslatedProGuitarButton.Strum3,
        Strum4 = 1 << TranslatedProGuitarButton.Strum4,
        Strum5 = 1 << TranslatedProGuitarButton.Strum5,
        Strum6 = 1 << TranslatedProGuitarButton.Strum6,

        Green = 1 << TranslatedProGuitarButton.Green,
        Red = 1 << TranslatedProGuitarButton.Red,
        Yellow = 1 << TranslatedProGuitarButton.Yellow,
        Blue = 1 << TranslatedProGuitarButton.Blue,
        Orange = 1 << TranslatedProGuitarButton.Orange,

        SoloGreen = 1 << TranslatedProGuitarButton.SoloGreen,
        SoloRed = 1 << TranslatedProGuitarButton.SoloRed,
        SoloYellow = 1 << TranslatedProGuitarButton.SoloYellow,
        SoloBlue = 1 << TranslatedProGuitarButton.SoloBlue,
        SoloOrange = 1u << TranslatedProGuitarButton.SoloOrange,
    }

    /// <summary>
    /// The format which <see cref="TranslatingProGuitar{TState}"/>s translate state into.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedProGuitarState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('G', 'T', 'R', 'P');
        public FourCC format => Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedProGuitarButton.South)]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedProGuitarButton.East)]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedProGuitarButton.West)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedProGuitarButton.North)]

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedProGuitarButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedProGuitarButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedProGuitarButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedProGuitarButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedProGuitarButton.DpadRight)]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedProGuitarButton.Start)]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedProGuitarButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedProGuitarButton.System)]

        [InputControl(name = "tilt", layout = "Button", bit = (int)TranslatedProGuitarButton.Tilt, noisy = true)]

        [InputControl(name = "strum1", layout = "Button", bit = (int)TranslatedProGuitarButton.Strum1)]
        [InputControl(name = "strum2", layout = "Button", bit = (int)TranslatedProGuitarButton.Strum2)]
        [InputControl(name = "strum3", layout = "Button", bit = (int)TranslatedProGuitarButton.Strum3)]
        [InputControl(name = "strum4", layout = "Button", bit = (int)TranslatedProGuitarButton.Strum4)]
        [InputControl(name = "strum5", layout = "Button", bit = (int)TranslatedProGuitarButton.Strum5)]
        [InputControl(name = "strum6", layout = "Button", bit = (int)TranslatedProGuitarButton.Strum6)]

        [InputControl(name = "greenFret", layout = "Button", bit = (int)TranslatedProGuitarButton.Green)]
        [InputControl(name = "redFret", layout = "Button", bit = (int)TranslatedProGuitarButton.Red)]
        [InputControl(name = "yellowFret", layout = "Button", bit = (int)TranslatedProGuitarButton.Yellow)]
        [InputControl(name = "blueFret", layout = "Button", bit = (int)TranslatedProGuitarButton.Blue)]
        [InputControl(name = "orangeFret", layout = "Button", bit = (int)TranslatedProGuitarButton.Orange)]

        [InputControl(name = "soloGreen", layout = "Button", bit = (int)TranslatedProGuitarButton.SoloGreen)]
        [InputControl(name = "soloRed", layout = "Button", bit = (int)TranslatedProGuitarButton.SoloRed)]
        [InputControl(name = "soloYellow", layout = "Button", bit = (int)TranslatedProGuitarButton.SoloYellow)]
        [InputControl(name = "soloBlue", layout = "Button", bit = (int)TranslatedProGuitarButton.SoloBlue)]
        [InputControl(name = "soloOrange", layout = "Button", bit = (int)TranslatedProGuitarButton.SoloOrange)]
        public uint buttons;

        // For better space usage, these remain in their compressed form
        [InputControl(name = "fret1", layout = "Integer", format = "BIT", bit = 0, sizeInBits = 5)]
        [InputControl(name = "fret2", layout = "Integer", format = "BIT", bit = 5, sizeInBits = 5)]
        [InputControl(name = "fret3", layout = "Integer", format = "BIT", bit = 10, sizeInBits = 5)]
        public ushort frets1;

        [InputControl(name = "fret4", layout = "Integer", format = "BIT", bit = 0, sizeInBits = 5)]
        [InputControl(name = "fret5", layout = "Integer", format = "BIT", bit = 5, sizeInBits = 5)]
        [InputControl(name = "fret6", layout = "Integer", format = "BIT", bit = 10, sizeInBits = 5)]
        public ushort frets2;

        // [InputControl(name = "analogPedal", layout = "Axis", format = "BIT", sizeInBits = 7)] // TODO: is this a thing?
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte pedal;
    }

    /// <summary>
    /// A <see cref="ProGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedProGuitarState"/> format.
    /// </summary>
    internal abstract class TranslatingProGuitar<TState> : ProGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IProGuitarState, IInputStateTypeInfo
    {
        private TranslateStateHandler<TState, TranslatedProGuitarState> m_Translator;

        private byte m_PreviousVelocity1;
        private byte m_PreviousVelocity2;
        private byte m_PreviousVelocity3;
        private byte m_PreviousVelocity4;
        private byte m_PreviousVelocity5;
        private byte m_PreviousVelocity6;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Translator = TranslateState;
            StateTranslator<TState, TranslatedProGuitarState>.VerifyDevice(this);
        }

        unsafe void IInputStateCallbackReceiver.OnNextUpdate()
        {
            // Strumming must be reset at the beginning of the update, else it will persist forever
            using (var buffer = StateEvent.From(this, out var eventPtr))
            {
                ref var state = ref *(TranslatedProGuitarState*)buffer.GetUnsafePtr();
                state.buttons &= ~(uint)(TranslatedProGuitarButton.Strum1 | TranslatedProGuitarButton.Strum2 |
                    TranslatedProGuitarButton.Strum3 | TranslatedProGuitarButton.Strum4 |
                    TranslatedProGuitarButton.Strum5 | TranslatedProGuitarButton.Strum6);
                InputState.Change(this, eventPtr);
            }
        }

        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedProGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedProGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedProGuitarState TranslateState(ref TState state)
        {
            var translated = new TranslatedProGuitarState()
            {
                frets1 = state.frets1,
                frets2 = state.frets2,
            };

            // Face buttons
            var buttons = TranslatedProGuitarButtonMask.None;
            if (state.south) buttons |= TranslatedProGuitarButtonMask.South; // A, cross
            if (state.east) buttons |= TranslatedProGuitarButtonMask.East; // B, circle
            if (state.west) buttons |= TranslatedProGuitarButtonMask.West; // X, square, 1
            if (state.north) buttons |= TranslatedProGuitarButtonMask.North; // Y, square, 2

            if (state.dpadUp) buttons |= TranslatedProGuitarButtonMask.DpadUp;
            if (state.dpadDown) buttons |= TranslatedProGuitarButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedProGuitarButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedProGuitarButtonMask.DpadRight;

            if (state.start) buttons |= TranslatedProGuitarButtonMask.Start;
            if (state.select) buttons |= TranslatedProGuitarButtonMask.Select;
            if (state.system) buttons |= TranslatedProGuitarButtonMask.System;

            // Strings
            byte velocity1 = state.velocity1;
            byte velocity2 = state.velocity2;
            byte velocity3 = state.velocity3;
            byte velocity4 = state.velocity4;
            byte velocity5 = state.velocity5;
            byte velocity6 = state.velocity6;

            // The string velocity is apparently not very reliable and can be confusing to work with,
            // so only whether or not the strum has changed is exposed until more research can be done
            bool strum1 = velocity1 != m_PreviousVelocity1 && velocity1 != 0;
            bool strum2 = velocity2 != m_PreviousVelocity2 && velocity2 != 0;
            bool strum3 = velocity3 != m_PreviousVelocity3 && velocity3 != 0;
            bool strum4 = velocity4 != m_PreviousVelocity4 && velocity4 != 0;
            bool strum5 = velocity5 != m_PreviousVelocity5 && velocity5 != 0;
            bool strum6 = velocity6 != m_PreviousVelocity6 && velocity6 != 0;

            m_PreviousVelocity1 = velocity1;
            m_PreviousVelocity2 = velocity2;
            m_PreviousVelocity3 = velocity3;
            m_PreviousVelocity4 = velocity4;
            m_PreviousVelocity5 = velocity5;
            m_PreviousVelocity6 = velocity6;

            if (strum1) buttons |= TranslatedProGuitarButtonMask.Strum1;
            if (strum2) buttons |= TranslatedProGuitarButtonMask.Strum2;
            if (strum3) buttons |= TranslatedProGuitarButtonMask.Strum3;
            if (strum4) buttons |= TranslatedProGuitarButtonMask.Strum4;
            if (strum5) buttons |= TranslatedProGuitarButtonMask.Strum5;
            if (strum6) buttons |= TranslatedProGuitarButtonMask.Strum6;

            // Emulated frets
            bool green = state.green;
            bool red = state.red;
            bool yellow = state.yellow;
            bool blue = state.blue;
            bool orange = state.orange;

            if (!state.solo)
            {
                if (green) buttons |= TranslatedProGuitarButtonMask.Green;
                if (red) buttons |= TranslatedProGuitarButtonMask.Red;
                if (yellow) buttons |= TranslatedProGuitarButtonMask.Yellow;
                if (blue) buttons |= TranslatedProGuitarButtonMask.Blue;
                if (orange) buttons |= TranslatedProGuitarButtonMask.Orange;
            }
            else
            {
                if (green) buttons |= TranslatedProGuitarButtonMask.SoloGreen;
                if (red) buttons |= TranslatedProGuitarButtonMask.SoloRed;
                if (yellow) buttons |= TranslatedProGuitarButtonMask.SoloYellow;
                if (blue) buttons |= TranslatedProGuitarButtonMask.SoloBlue;
                if (orange) buttons |= TranslatedProGuitarButtonMask.SoloOrange;
            }

            if (state.tilt) buttons |= TranslatedProGuitarButtonMask.Tilt;

            translated.buttons = (uint)buttons;

            // Pedal port
            translated.pedal.SetBit(0x80, state.digitalPedal);
            // translated.pedal.SetMask(0x7F, state.analogPedal);

            return translated;
        }
    }
}