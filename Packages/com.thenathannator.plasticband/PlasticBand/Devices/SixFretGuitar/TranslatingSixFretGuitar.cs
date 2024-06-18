using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedSixFretButton
    {
        DpadUp = 0,
        DpadDown = 1,
        DpadLeft = 2,
        DpadRight = 3,

        Start = 4,
        Select = 5,
        GHTV = 6,
        System = 7,
    }

    [Flags]
    internal enum TranslatedSixFretButtonMask : ushort
    {
        None = 0,

        DpadUp = 1 << TranslatedSixFretButton.DpadUp,
        DpadDown = 1 << TranslatedSixFretButton.DpadDown,
        DpadLeft = 1 << TranslatedSixFretButton.DpadLeft,
        DpadRight = 1 << TranslatedSixFretButton.DpadRight,

        Start = 1 << TranslatedSixFretButton.Start,
        Select = 1 << TranslatedSixFretButton.Select,
        GHTV = 1 << TranslatedSixFretButton.GHTV,
        System = 1 << TranslatedSixFretButton.System,
    }

    /// <summary>
    /// The format which <see cref="TranslatingSixFretGuitar{TState}"/>s translate state into.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedSixFretState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('G', 'T', 'R', '6');
        public FourCC format => Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedSixFretButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedSixFretButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedSixFretButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedSixFretButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedSixFretButton.DpadRight)]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedSixFretButton.Start)]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedSixFretButton.Select)]
        [InputControl(name = "ghtvButton", layout = "Button", bit = (int)TranslatedSixFretButton.GHTV)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedSixFretButton.System, displayName = "D-pad Center")]
        public ushort buttons;

        [InputControl(name = "black1", layout = "Button", bit = 0)]
        [InputControl(name = "black2", layout = "Button", bit = 1)]
        [InputControl(name = "black3", layout = "Button", bit = 2)]
        [InputControl(name = "white1", layout = "Button", bit = 3)]
        [InputControl(name = "white2", layout = "Button", bit = 4)]
        [InputControl(name = "white3", layout = "Button", bit = 5)]
        public byte frets;

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public sbyte tilt;
    }

    /// <summary>
    /// A <see cref="SixFretGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedSixFretState"/> format.
    /// </summary>
    internal abstract class TranslatingSixFretGuitar<TState> : SixFretGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, ISixFretGuitarState, IInputStateTypeInfo
    {
        protected static readonly TranslateStateHandler<TState, TranslatedSixFretState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedSixFretState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedSixFretState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedSixFretState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        private static TranslatedSixFretState TranslateState(ref TState state)
        {
            var translated = new TranslatedSixFretState()
            {
                whammy = state.whammy,
                tilt = state.tilt,
            };

            var buttons = TranslatedSixFretButtonMask.None;

            if (state.dpadUp || state.strumUp) buttons |= TranslatedSixFretButtonMask.DpadUp;
            if (state.dpadDown || state.strumDown) buttons |= TranslatedSixFretButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedSixFretButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedSixFretButtonMask.DpadRight;

            if (state.start) buttons |= TranslatedSixFretButtonMask.Start;
            if (state.select) buttons |= TranslatedSixFretButtonMask.Select;
            if (state.ghtv) buttons |= TranslatedSixFretButtonMask.GHTV;
            if (state.system) buttons |= TranslatedSixFretButtonMask.System;

            translated.buttons = (ushort)buttons;

            var frets = SixFret.None;

            if (state.black1) frets |= SixFret.Black1;
            if (state.black2) frets |= SixFret.Black2;
            if (state.black3) frets |= SixFret.Black3;
            if (state.white1) frets |= SixFret.White1;
            if (state.white2) frets |= SixFret.White2;
            if (state.white3) frets |= SixFret.White3;

            translated.frets = (byte)frets;

            return translated;
        }
    }
}