using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedRockBandGuitarButton
    {
        DpadUp = 0,
        DpadDown = 1,
        DpadLeft = 2,
        DpadRight = 3,

        Start = 5,
        Select = 6,
        System = 7,
    }

    [Flags]
    internal enum TranslatedRockBandGuitarButtonMask : ushort
    {
        None = 0,

        DpadUp = 1 << TranslatedRockBandGuitarButton.DpadUp,
        DpadDown = 1 << TranslatedRockBandGuitarButton.DpadDown,
        DpadLeft = 1 << TranslatedRockBandGuitarButton.DpadLeft,
        DpadRight = 1 << TranslatedRockBandGuitarButton.DpadRight,

        Start = 1 << TranslatedRockBandGuitarButton.Start,
        Select = 1 << TranslatedRockBandGuitarButton.Select,
        System = 1 << TranslatedRockBandGuitarButton.System,
    }

    /// <summary>
    /// The format which <see cref="TranslatingRockBandGuitar"/>s translate state into.
    /// </summary>
    /// <seealso cref="TranslatingRockBandGuitar_Flags{TState}"/>
    /// <seealso cref="TranslatingRockBandGuitar_Distinct{TState}"/>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedRockBandGuitarState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('R', 'B', 'G', 'T');
        public FourCC format => Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedRockBandGuitarButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedRockBandGuitarButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedRockBandGuitarButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedRockBandGuitarButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedRockBandGuitarButton.DpadRight)]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.Start)]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.System)]
        public ushort buttons;

        [InputControl(name = "greenFret", layout = "Button", bit = 0)]
        [InputControl(name = "redFret", layout = "Button", bit = 1)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        public byte frets;

        [InputControl(name = "soloGreen", layout = "Button", bit = 0)]
        [InputControl(name = "soloRed", layout = "Button", bit = 1)]
        [InputControl(name = "soloYellow", layout = "Button", bit = 2)]
        [InputControl(name = "soloBlue", layout = "Button", bit = 3)]
        [InputControl(name = "soloOrange", layout = "Button", bit = 4)]
        public byte soloFrets;

        [InputControl(name = "whammy", layout = "IntAxis", parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x00")]
        public byte whammy;

        [InputControl(name = "tilt", layout = "IntAxis", noisy = true, parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        public sbyte tilt;

        [InputControl(name = "pickupSwitch", layout = "Integer")]
        public byte pickupSwitch;
    }

    internal static class TranslatingRockBandGuitar
    {
        internal static TranslatedRockBandGuitarButtonMask TranslateButtons<TState>(ref TState state)
            where TState : unmanaged, IRockBandGuitarState_Base, IInputStateTypeInfo
        {
            var buttons = TranslatedRockBandGuitarButtonMask.None;

            // D-pad
            if (state.dpadUp) buttons |= TranslatedRockBandGuitarButtonMask.DpadUp;
            if (state.dpadDown) buttons |= TranslatedRockBandGuitarButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedRockBandGuitarButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedRockBandGuitarButtonMask.DpadRight;

            // Menu/system
            if (state.start) buttons |= TranslatedRockBandGuitarButtonMask.Start;
            if (state.select) buttons |= TranslatedRockBandGuitarButtonMask.Select;
            if (state.system) buttons |= TranslatedRockBandGuitarButtonMask.System;

            return buttons;
        }

        internal static FiveFret TranslateFrets(bool green, bool red, bool yellow, bool blue, bool orange)
        {
            var frets = FiveFret.None;

            if (green) frets |= FiveFret.Green;
            if (red) frets |= FiveFret.Red;
            if (yellow) frets |= FiveFret.Yellow;
            if (blue) frets |= FiveFret.Blue;
            if (orange) frets |= FiveFret.Orange;

            return frets;
        }

        internal static FiveFret TranslateFrets_Flags<TState>(ref TState state)
            where TState : unmanaged, IRockBandGuitarState_Flags, IInputStateTypeInfo
            => TranslateFrets(state.green, state.red, state.yellow, state.blue, state.orange);
    }

    /// <summary>
    /// A <see cref="RockBandGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedRockBandGuitarState"/> format.
    /// This variant is for guitars that use various flags to indicate fret presses.
    /// </summary>
    // This is done to simplify state handling in the layouts of guitars, as it makes it easier to correlate
    // the fret flags together without having to use custom control types as hacks to accomplish the same thing.
    internal abstract class TranslatingRockBandGuitar_Flags<TState> : RockBandGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IRockBandGuitarState_Flags, IInputStateTypeInfo
    {
        private static readonly TranslateStateHandler<TState, TranslatedRockBandGuitarState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedRockBandGuitarState TranslateState(ref TState state)
        {
            var frets = TranslatingRockBandGuitar.TranslateFrets_Flags(ref state);
            bool solo = state.solo;

            return new TranslatedRockBandGuitarState()
            {
                buttons = (ushort)TranslatingRockBandGuitar.TranslateButtons(ref state),

                // Only allow one set of frets at a time, don't mirror frets
                frets = (byte)(!solo ? frets : FiveFret.None),
                soloFrets = (byte)(solo ? frets : FiveFret.None),

                whammy = state.whammy,
                tilt = state.tilt,
                pickupSwitch = (byte)state.pickupSwitch,
            };
        }
    }

    /// <summary>
    /// A <see cref="RockBandGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedRockBandGuitarState"/> format.
    /// This variant is for guitars that use various flags to indicate fret presses.
    /// </summary>
    // This is done to simplify state handling in the layouts of guitars, as it makes it easier to correlate
    // the fret flags together without having to use custom control types as hacks to accomplish the same thing.
    internal abstract class TranslatingRockBandGuitar_Flags_NullState<TState> : RockBandGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IRockBandGuitarState_Flags, IInputStateTypeInfo
    {
        private TranslateStateHandler<TState, TranslatedRockBandGuitarState> m_Translator;

        private byte m_LastWhammy;
        private int m_LastPickupSwitch;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Translator = TranslateState;
            StateTranslator<TState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedRockBandGuitarState TranslateState(ref TState state)
        {
            if (state.whammy == RockBandGuitarState.kNullValue)
                state.whammy = m_LastWhammy;

            if (state.pickupSwitch < 0)
                state.pickupSwitch = m_LastPickupSwitch;

            m_LastWhammy = state.whammy;
            m_LastPickupSwitch = state.pickupSwitch;

            return TranslatingRockBandGuitar_Flags<TState>.TranslateState(ref state);
        }
    }

    /// <summary>
    /// A <see cref="RockBandGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedRockBandGuitarState"/> format.
    /// This variant is for guitars which directly distinguish the regular and solo frets.
    /// </summary>
    internal abstract class TranslatingRockBandGuitar_Distinct<TState> : RockBandGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IRockBandGuitarState_Distinct, IInputStateTypeInfo
    {
        protected static readonly TranslateStateHandler<TState, TranslatedRockBandGuitarState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedRockBandGuitarState TranslateState(ref TState state)
        {
            return new TranslatedRockBandGuitarState()
            {
                buttons = (ushort)TranslatingRockBandGuitar.TranslateButtons(ref state),

                frets = (byte)TranslatingRockBandGuitar.TranslateFrets(
                    state.green, state.red, state.yellow, state.blue, state.orange
                ),
                soloFrets = (byte)TranslatingRockBandGuitar.TranslateFrets(
                    state.soloGreen, state.soloRed, state.soloYellow, state.soloBlue, state.soloOrange
                ),

                whammy = state.whammy,
                tilt = state.tilt,
                pickupSwitch = (byte)state.pickupSwitch,
            };
        }
    }
}