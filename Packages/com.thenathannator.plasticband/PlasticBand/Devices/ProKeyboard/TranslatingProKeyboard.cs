using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedProKeyboardButton
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
    }

    [Flags]
    internal enum TranslatedProKeyboardButtonMask : ushort
    {
        None = 0,

        South = 1 << TranslatedProKeyboardButton.South,
        East = 1 << TranslatedProKeyboardButton.East,
        West = 1 << TranslatedProKeyboardButton.West,
        North = 1 << TranslatedProKeyboardButton.North,

        DpadUp = 1 << TranslatedProKeyboardButton.DpadUp,
        DpadDown = 1 << TranslatedProKeyboardButton.DpadDown,
        DpadLeft = 1 << TranslatedProKeyboardButton.DpadLeft,
        DpadRight = 1 << TranslatedProKeyboardButton.DpadRight,

        Start = 1 << TranslatedProKeyboardButton.Start,
        Select = 1 << TranslatedProKeyboardButton.Select,
        System = 1 << TranslatedProKeyboardButton.System,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedProKeyboardState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('K', 'E', 'Y', 'B');
        public FourCC format => Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedProKeyboardButton.South)]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedProKeyboardButton.East)]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedProKeyboardButton.West)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedProKeyboardButton.North)]

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedProKeyboardButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedProKeyboardButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedProKeyboardButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedProKeyboardButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedProKeyboardButton.DpadRight)]

        [InputControl(name = "start", layout = "Button", bit = (int)TranslatedProKeyboardButton.Start)]
        [InputControl(name = "select", layout = "Button", bit = (int)TranslatedProKeyboardButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedProKeyboardButton.System)]
        public ushort buttons;

        [InputControl(name = "key1",  layout = "Button", bit = 7)]
        [InputControl(name = "key2",  layout = "Button", bit = 6)]
        [InputControl(name = "key3",  layout = "Button", bit = 5)]
        [InputControl(name = "key4",  layout = "Button", bit = 4)]
        [InputControl(name = "key5",  layout = "Button", bit = 3)]
        [InputControl(name = "key6",  layout = "Button", bit = 2)]
        [InputControl(name = "key7",  layout = "Button", bit = 1)]
        [InputControl(name = "key8",  layout = "Button", bit = 0)]
        public byte keys1;

        [InputControl(name = "key9",  layout = "Button", bit = 7)]
        [InputControl(name = "key10", layout = "Button", bit = 6)]
        [InputControl(name = "key11", layout = "Button", bit = 5)]
        [InputControl(name = "key12", layout = "Button", bit = 4)]
        [InputControl(name = "key13", layout = "Button", bit = 3)]
        [InputControl(name = "key14", layout = "Button", bit = 2)]
        [InputControl(name = "key15", layout = "Button", bit = 1)]
        [InputControl(name = "key16", layout = "Button", bit = 0)]
        public byte keys2;

        [InputControl(name = "key17", layout = "Button", bit = 7)]
        [InputControl(name = "key18", layout = "Button", bit = 6)]
        [InputControl(name = "key19", layout = "Button", bit = 5)]
        [InputControl(name = "key20", layout = "Button", bit = 4)]
        [InputControl(name = "key21", layout = "Button", bit = 3)]
        [InputControl(name = "key22", layout = "Button", bit = 2)]
        [InputControl(name = "key23", layout = "Button", bit = 1)]
        [InputControl(name = "key24", layout = "Button", bit = 0)]
        public byte keys3;

        [InputControl(name = "key25", layout = "Button", bit = 7)]
        public byte velocity1;

        public byte velocity2;
        public byte velocity3;
        public byte velocity4;
        public byte velocity5;

        [InputControl(name = "overdrive", layout = "Button", bit = 7)]
        public byte overdrive;

        [InputControl(name = "analogPedal", layout = "Axis", format = "BIT", sizeInBits = 7, parameters = "normalize,normalizeMin=1,normalizeMax=0,normalizeZero=1")]
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte pedal;

        [InputControl(name = "touchStrip", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte touchStrip;
    }

    internal abstract class TranslatingProKeyboard<TState> : ProKeyboard, IInputStateCallbackReceiver
        where TState : unmanaged, IProKeyboardState, IInputStateTypeInfo
    {
        private static readonly TranslateStateHandler<TState, TranslatedProKeyboardState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedProKeyboardState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedProKeyboardState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedProKeyboardState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedProKeyboardState TranslateState(ref TState state)
        {
            var buttons = TranslatedProKeyboardButtonMask.None;
            if (state.south) buttons |= TranslatedProKeyboardButtonMask.South;
            if (state.east) buttons |= TranslatedProKeyboardButtonMask.East;
            if (state.west) buttons |= TranslatedProKeyboardButtonMask.West;
            if (state.north) buttons |= TranslatedProKeyboardButtonMask.North;

            if (state.dpadUp) buttons |= TranslatedProKeyboardButtonMask.DpadUp;
            if (state.dpadDown) buttons |= TranslatedProKeyboardButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedProKeyboardButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedProKeyboardButtonMask.DpadRight;

            if (state.start) buttons |= TranslatedProKeyboardButtonMask.Start;
            if (state.select) buttons |= TranslatedProKeyboardButtonMask.Select;
            if (state.system) buttons |= TranslatedProKeyboardButtonMask.System;

            return new TranslatedProKeyboardState
            {
                buttons = (ushort)buttons,
                keys1 = state.keys1,
                keys2 = state.keys2,
                keys3 = state.keys3,
                velocity1 = state.velocity1,
                velocity2 = state.velocity2,
                velocity3 = state.velocity3,
                velocity4 = state.velocity4,
                velocity5 = state.velocity5,
                overdrive = state.overdrive,
                pedal = state.pedal,
                touchStrip = state.touchStrip,
            };
        }
    }
}

