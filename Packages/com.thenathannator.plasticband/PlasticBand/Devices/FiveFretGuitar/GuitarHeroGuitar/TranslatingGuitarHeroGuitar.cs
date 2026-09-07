using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedGuitarHeroGuitarButton
    {
        DpadUp = 0,
        DpadDown = 1,
        DpadLeft = 2,
        DpadRight = 3,

        Pedal = 4,

        Start = 5,
        Select = 6,
        System = 7,
    }

    [Flags]
    internal enum TranslatedGuitarHeroGuitarButtonMask : ushort
    {
        None = 0,

        DpadUp = 1 << TranslatedGuitarHeroGuitarButton.DpadUp,
        DpadDown = 1 << TranslatedGuitarHeroGuitarButton.DpadDown,
        DpadLeft = 1 << TranslatedGuitarHeroGuitarButton.DpadLeft,
        DpadRight = 1 << TranslatedGuitarHeroGuitarButton.DpadRight,

        Pedal = 1 << TranslatedGuitarHeroGuitarButton.Pedal,

        Start = 1 << TranslatedGuitarHeroGuitarButton.Start,
        Select = 1 << TranslatedGuitarHeroGuitarButton.Select,
        System = 1 << TranslatedGuitarHeroGuitarButton.System,
    }

    /// <summary>
    /// The format which <see cref="TranslatingGuitarHeroGuitar"/>s translate state into.
    /// </summary>
    /// <seealso cref="TranslatingGuitarHeroGuitar_Ranges{TState}"/>
    /// <seealso cref="TranslatingGuitarHeroGuitar_Discrete{TState}"/>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedGuitarHeroGuitarState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('G', 'H', 'G', 'T');
        public FourCC format => Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedGuitarHeroGuitarButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedGuitarHeroGuitarButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedGuitarHeroGuitarButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedGuitarHeroGuitarButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedGuitarHeroGuitarButton.DpadRight)]

        [InputControl(name = "spPedal", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.Pedal)]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.Start)]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.System, displayName = "System")]
        public ushort buttons;

        [InputControl(name = "greenFret", layout = "Button", bit = 0)]
        [InputControl(name = "redFret", layout = "Button", bit = 1)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        public byte frets;

        [InputControl(name = "touchGreen", layout = "Button", bit = 0)]
        [InputControl(name = "touchRed", layout = "Button", bit = 1)]
        [InputControl(name = "touchYellow", layout = "Button", bit = 2)]
        [InputControl(name = "touchBlue", layout = "Button", bit = 3)]
        [InputControl(name = "touchOrange", layout = "Button", bit = 4)]
        public byte touchFrets;

        [InputControl(name = "whammy", layout = "IntAxis", parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x00")]
        public byte whammy;

        [InputControl(name = "tilt", layout = "IntAxis", noisy = true, parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        public sbyte tilt;
    }

    internal static class TranslatingGuitarHeroGuitar
    {
        // For conciseness
        private const FiveFret G = FiveFret.Green;
        private const FiveFret R = FiveFret.Red;
        private const FiveFret Y = FiveFret.Yellow;
        private const FiveFret B = FiveFret.Blue;
        private const FiveFret O = FiveFret.Orange;

        internal static readonly FiveFret[] s_WtTouchFrets =
        {
            /* 0 */ G,
            /* 1 */ G,
            /* 2 */ G,
            /* 3 */ G | R,
            /* 4 */     R,
            /* 5 */     R,
            /* 6 */     R | Y,
            /* 7 */ FiveFret.None,
            /* 8 */ FiveFret.None,
            /* 9 */         Y,
            /* A */         Y | B,
            /* B */             B,
            /* C */             B,
            /* D */             B | O,
            /* E */             B | O,
            /* F */                 O,
        };

        internal static readonly FiveFret[] s_Gh5TouchFrets;

        static TranslatingGuitarHeroGuitar()
        {
            s_Gh5TouchFrets = new FiveFret[256];
            for (int i = 0; i < s_Gh5TouchFrets.Length; i++)
            {
                s_Gh5TouchFrets[i] = FiveFret.None;
            }

            s_Gh5TouchFrets[0x15] = G;

            s_Gh5TouchFrets[0x30] = G | R;

            s_Gh5TouchFrets[0x4D] =     R;

            s_Gh5TouchFrets[0x65] = G | R | Y;
            s_Gh5TouchFrets[0x66] =     R | Y;

            s_Gh5TouchFrets[0x80] = FiveFret.None;

            s_Gh5TouchFrets[0x99] = G |     Y;
            s_Gh5TouchFrets[0x9A] =         Y;

            s_Gh5TouchFrets[0xAC] = G | R | Y | B;
            s_Gh5TouchFrets[0xAD] = G |     Y | B;
            s_Gh5TouchFrets[0xAE] =     R | Y | B;
            s_Gh5TouchFrets[0xAF] =         Y | B;

            s_Gh5TouchFrets[0xC6] = G | R |     B;
            s_Gh5TouchFrets[0xC7] = G |         B;
            s_Gh5TouchFrets[0xC8] =         R | B;
            s_Gh5TouchFrets[0xC9] =             B;

            s_Gh5TouchFrets[0xDF] = G | R | Y | B | O;
            s_Gh5TouchFrets[0xE0] = G | R |     B | O;
            s_Gh5TouchFrets[0xE1] = G |     Y | B | O;
            s_Gh5TouchFrets[0xE2] = G |         B | O;
            s_Gh5TouchFrets[0xE3] =     R | Y | B | O;
            s_Gh5TouchFrets[0xE4] =     R |     B | O;
            s_Gh5TouchFrets[0xE5] =         Y | B | O;
            s_Gh5TouchFrets[0xE6] =             B | O;

            s_Gh5TouchFrets[0xF8] = G | R | Y |     O;
            s_Gh5TouchFrets[0xF9] = G | R |         O;
            s_Gh5TouchFrets[0xFA] = G |     Y |     O;
            s_Gh5TouchFrets[0xFB] = G |             O;
            s_Gh5TouchFrets[0xFC] =     R | Y |     O;
            s_Gh5TouchFrets[0xFD] =     R |         O;
            s_Gh5TouchFrets[0xFE] =         Y |     O;
            s_Gh5TouchFrets[0xFF] =                 O;
        }

        internal static TranslatedGuitarHeroGuitarButtonMask TranslateButtons<TState>(ref TState state)
            where TState : unmanaged, IGuitarHeroGuitarState, IInputStateTypeInfo
        {
            var buttons = TranslatedGuitarHeroGuitarButtonMask.None;

            // D-pad
            if (state.dpadUp) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadUp;
            if (state.dpadDown) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadRight;

            // Menu/system
            if (state.start) buttons |= TranslatedGuitarHeroGuitarButtonMask.Start;
            if (state.select) buttons |= TranslatedGuitarHeroGuitarButtonMask.Select;
            if (state.system) buttons |= TranslatedGuitarHeroGuitarButtonMask.System;

            // Misc.
            if (state.spPedal) buttons |= TranslatedGuitarHeroGuitarButtonMask.Pedal;

            return buttons;
        }

        internal static FiveFret TranslateFrets<TState>(ref TState state)
            where TState : unmanaged, IGuitarHeroGuitarState, IInputStateTypeInfo
        {
            var frets = FiveFret.None;

            if (state.green) frets |= FiveFret.Green;
            if (state.red) frets |= FiveFret.Red;
            if (state.yellow) frets |= FiveFret.Yellow;
            if (state.blue) frets |= FiveFret.Blue;
            if (state.orange) frets |= FiveFret.Orange;

            return frets;
        }
    }

    /// <summary>
    /// A <see cref="GuitarHeroGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedGuitarHeroGuitarState"/> format.
    /// This variant is for guitars that use limited ranges for adjacent positions on the touch bar.
    /// </summary>
    internal abstract class TranslatingGuitarHeroGuitar_Ranges<TState> : GuitarHeroGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IGuitarHeroGuitarState, IInputStateTypeInfo
    {
        private static readonly TranslateStateHandler<TState, TranslatedGuitarHeroGuitarState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedGuitarHeroGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedGuitarHeroGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedGuitarHeroGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedGuitarHeroGuitarState TranslateState(ref TState state)
        {
            return new TranslatedGuitarHeroGuitarState()
            {
                buttons = (ushort)TranslatingGuitarHeroGuitar.TranslateButtons(ref state),

                frets = (byte) TranslatingGuitarHeroGuitar.TranslateFrets(ref state),
                touchFrets = (byte) TranslatingGuitarHeroGuitar.s_WtTouchFrets[(state.rawTouchBar & 0xF0) >> 4],

                whammy = state.whammy,
                tilt = state.tilt,
            };
        }
    }

    /// <summary>
    /// A <see cref="GuitarHeroGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedGuitarHeroGuitarState"/> format.
    /// This variant is for guitars that use discrete values for every combination on the touch bar.
    /// </summary>
    internal abstract class TranslatingGuitarHeroGuitar_Discrete<TState> : GuitarHeroGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IGuitarHeroGuitarState, IInputStateTypeInfo
    {
        protected static readonly TranslateStateHandler<TState, TranslatedGuitarHeroGuitarState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedGuitarHeroGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedGuitarHeroGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedGuitarHeroGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedGuitarHeroGuitarState TranslateState(ref TState state)
        {
            return new TranslatedGuitarHeroGuitarState()
            {
                buttons = (ushort)TranslatingGuitarHeroGuitar.TranslateButtons(ref state),

                frets = (byte) TranslatingGuitarHeroGuitar.TranslateFrets(ref state),
                touchFrets = (byte) TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[state.rawTouchBar],

                whammy = state.whammy,
                tilt = state.tilt,
            };
        }
    }
}