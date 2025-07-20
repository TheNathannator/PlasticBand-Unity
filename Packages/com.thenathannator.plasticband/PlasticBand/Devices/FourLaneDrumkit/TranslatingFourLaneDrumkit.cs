using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedFourLaneButton
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

        Kick1 = 12,
        Kick2 = 13,
    }

    [Flags]
    internal enum TranslatedFourLaneButtonMask : ushort
    {
        None = 0,

        South = 1 << TranslatedFourLaneButton.South,
        East = 1 << TranslatedFourLaneButton.East,
        West = 1 << TranslatedFourLaneButton.West,
        North = 1 << TranslatedFourLaneButton.North,

        DpadUp = 1 << TranslatedFourLaneButton.DpadUp,
        DpadDown = 1 << TranslatedFourLaneButton.DpadDown,
        DpadLeft = 1 << TranslatedFourLaneButton.DpadLeft,
        DpadRight = 1 << TranslatedFourLaneButton.DpadRight,

        Start = 1 << TranslatedFourLaneButton.Start,
        Select = 1 << TranslatedFourLaneButton.Select,
        System = 1 << TranslatedFourLaneButton.System,

        Kick1 = 1 << TranslatedFourLaneButton.Kick1,
        Kick2 = 1 << TranslatedFourLaneButton.Kick2,
    }

    /// <summary>
    /// The format which <see cref="TranslatingFourLaneDrumkit"/>s translate state into.
    /// </summary>
    /// <seealso cref="TranslatingFourLaneDrumkit_Flags{TState}"/>
    /// <seealso cref="TranslatingFourLaneDrumkit_Distinct{TState}"/>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedFourLaneState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('D', 'R', 'M', '4');
        public FourCC format => Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFourLaneButton.South)]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFourLaneButton.East)]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFourLaneButton.West)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFourLaneButton.North)]

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedFourLaneButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedFourLaneButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedFourLaneButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedFourLaneButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedFourLaneButton.DpadRight)]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedFourLaneButton.Start)]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedFourLaneButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFourLaneButton.System)]

        [InputControl(name = "kick1", layout = "Button", bit = (int)TranslatedFourLaneButton.Kick1)]
        [InputControl(name = "kick2", layout = "Button", bit = (int)TranslatedFourLaneButton.Kick2)]
        public ushort buttons;

        private const string kPadParameters = "intPressPoint=1,minValue=0,maxValue=255";

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte redPad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte yellowPad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte bluePad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte greenPad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte yellowCymbal;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte blueCymbal;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte greenCymbal;
    }

    internal static class TranslatingFourLaneDrumkit
    {
        // PlasticBand reference doc:
        // https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/General%20Notes.md#deciphering-pads-and-cymbals
        internal static FourLanePad TranslatePads<TState>(ref TState state, ref bool hasFlags)
            where TState : unmanaged, IFourLaneDrumkitState_FlagButtons
        {
            var pads = FourLanePad.None;

            // Retrieve flag values
            bool red = state.GetRedFlag();
            bool yellow = state.GetYellowFlag();
            bool blue = state.GetBlueFlag();
            bool green = state.GetGreenFlag();
            bool pad = state.pad;
            bool cymbal = state.cymbal;
            bool dpadUp = state.dpadUp;
            bool dpadDown = state.dpadDown;

            // Store whether or not pad/cymbal flags are present
            hasFlags |= pad | cymbal;

            // Pad + cymbal hits can be ambiguous, we need to resolve this
            if (pad && cymbal)
            {
                // There's only ambiguity between pad + cymbal hits of different colors,
                // same-color pad + cymbal can be used directly
                int colorCount = 0;
                if (red) colorCount++;
                if (yellow || dpadUp) colorCount++;
                if (blue || dpadDown) colorCount++;
                if (green || !(dpadUp || dpadDown)) colorCount++;

                if (colorCount > 1)
                {
                    // The d-pad inputs let us resolve the ambiguity of a pad + cymbal hit
                    // Only d-pad is checked here since it is the only unique identifier

                    // Yellow
                    if (dpadUp)
                    {
                        pads |= FourLanePad.YellowCymbal;
                        yellow = false;
                        cymbal = false;
                    }

                    // Blue
                    if (dpadDown)
                    {
                        pads |= FourLanePad.BlueCymbal;
                        blue = false;
                        cymbal = false;
                    }

                    // Green
                    if (!(dpadUp || dpadDown))
                    {
                        pads |= FourLanePad.GreenCymbal;
                        green = false;
                        cymbal = false;
                    }
                }
            }

            // Now that disambiguation has been applied, we can process things normally

            // Check for pad hits
            // Rock Band 1 kits don't send the pad or cymbal flags, so we also check if
            // flags have not been detected and if the cymbal flag is not active
            if (pad || (!cymbal && !hasFlags))
            {
                if (red) pads |= FourLanePad.RedPad;
                if (yellow) pads |= FourLanePad.YellowPad;
                if (blue) pads |= FourLanePad.BluePad;
                if (green) pads |= FourLanePad.GreenPad;
            }

            // Check for cymbal hits
            if (cymbal)
            {
                if (yellow) pads |= FourLanePad.YellowCymbal;
                if (blue) pads |= FourLanePad.BlueCymbal;
                if (green) pads |= FourLanePad.GreenCymbal;
            }

            return pads;
        }

        internal static FourLanePad TranslatePads<TState>(ref TState state)
            where TState : unmanaged, IFourLaneDrumkitState_DistinctVelocities
        {
            var pads = FourLanePad.None;
            if (state.redPadVelocity != 0) pads |= FourLanePad.RedPad;
            if (state.yellowPadVelocity != 0) pads |= FourLanePad.YellowPad;
            if (state.bluePadVelocity != 0) pads |= FourLanePad.BluePad;
            if (state.greenPadVelocity != 0) pads |= FourLanePad.GreenPad;
            if (state.yellowCymbalVelocity != 0) pads |= FourLanePad.YellowCymbal;
            if (state.blueCymbalVelocity != 0) pads |= FourLanePad.BlueCymbal;
            if (state.greenCymbalVelocity != 0) pads |= FourLanePad.GreenCymbal;
            return pads;
        }

        internal static TranslatedFourLaneButtonMask TranslateButtons<TState>(ref TState state, FourLanePad pads)
            where TState : unmanaged, IFourLaneDrumkitState_Base
        {
            var buttons = TranslatedFourLaneButtonMask.None;

            // Face buttons; these are ignored if the corresponding pad/cymbal is also active
            // A, cross
            if ((pads & (FourLanePad.GreenPad | FourLanePad.GreenCymbal)) == 0 && state.south)
                buttons |= TranslatedFourLaneButtonMask.South;
            // B, circle
            if ((pads & FourLanePad.RedPad) == 0 && state.east)
                buttons |= TranslatedFourLaneButtonMask.East;
            // X, square, 1
            if ((pads & (FourLanePad.BluePad | FourLanePad.BlueCymbal)) == 0 && state.west)
                buttons |= TranslatedFourLaneButtonMask.West;
            // Y, triangle, 2
            if ((pads & (FourLanePad.YellowPad | FourLanePad.YellowCymbal)) == 0 && state.north)
                buttons |= TranslatedFourLaneButtonMask.North;

            // D-pad
            // Up/down are ignored if cymbals are active, as they're used
            // as a distinguisher on drumkits which report using flags
            if ((pads & (FourLanePad.YellowCymbal | FourLanePad.BlueCymbal | FourLanePad.GreenCymbal)) == 0)
            {
                if (state.dpadUp) buttons |= TranslatedFourLaneButtonMask.DpadUp;
                if (state.dpadDown) buttons |= TranslatedFourLaneButtonMask.DpadDown;
            }
            if (state.dpadLeft) buttons |= TranslatedFourLaneButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedFourLaneButtonMask.DpadRight;

            if (state.start) buttons |= TranslatedFourLaneButtonMask.Start;
            if (state.select) buttons |= TranslatedFourLaneButtonMask.Select;
            if (state.system) buttons |= TranslatedFourLaneButtonMask.System;

            if (state.kick1) buttons |= TranslatedFourLaneButtonMask.Kick1;
            if (state.kick2) buttons |= TranslatedFourLaneButtonMask.Kick2;

            return buttons;
        }
    }

    /// <summary>
    /// A <see cref="FourLaneDrumkit"/> which translates its state data into a common
    /// <see cref="TranslatedFourLaneState"/> format.
    /// This variant is for drumkits that use various flags to indicate pad/cymbal hits.
    /// </summary>
    // This is done to vastly simplify state handling in the layouts of (most) drumkits, as rather than
    // using custom control types to handle various complexities, including working out which pads/cymbals
    // have been hit and pairing together hits and velocities, the state data is translated to a simpler format
    // ahead of time which the controls then read from.
    internal abstract class TranslatingFourLaneDrumkit_Flags<TState> : FourLaneDrumkit, IInputStateCallbackReceiver
        where TState : unmanaged, IFourLaneDrumkitState_FlagButtons, IFourLaneDrumkitState_SharedVelocities, IInputStateTypeInfo
    {
        private TranslateStateHandler<TState, TranslatedFourLaneState> m_Translator;
        private bool m_HasFlags;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Translator = TranslateState;
            StateTranslator<TState, TranslatedFourLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedFourLaneState TranslateState(ref TState state)
            => TranslateState(ref state, ref m_HasFlags);

        private static void FixCymbalVelocity(FourLanePad pads, FourLanePad colorPads, ref byte redVelocity, ref byte cymbalVelocity)
        {
            if ((pads & colorPads) == colorPads)
            {
                cymbalVelocity = redVelocity;
                redVelocity = 0;
            }
        }

        internal static TranslatedFourLaneState TranslateState(ref TState state, ref bool hasFlags)
        {
            var pads = TranslatingFourLaneDrumkit.TranslatePads(ref state, ref hasFlags);
            var buttons = TranslatingFourLaneDrumkit.TranslateButtons(ref state, pads);

            byte redPad = state.redVelocity;
            byte yellowPad = state.yellowVelocity;
            byte bluePad = state.blueVelocity;
            byte greenPad = state.greenVelocity;
            byte yellowCymbal = yellowPad;
            byte blueCymbal = bluePad;
            byte greenCymbal = greenPad;

            // Pad+cymbal hits of the same color will put the cymbal hit's velocity onto the red velocity axis
            FixCymbalVelocity(pads, FourLanePad.YellowPad | FourLanePad.YellowCymbal, ref redPad, ref yellowCymbal);
            FixCymbalVelocity(pads, FourLanePad.BluePad | FourLanePad.BlueCymbal, ref redPad, ref blueCymbal);
            FixCymbalVelocity(pads, FourLanePad.GreenPad | FourLanePad.GreenCymbal, ref redPad, ref greenCymbal);

            return new TranslatedFourLaneState()
            {
                buttons = (ushort)buttons,

                redPad    = (pads & FourLanePad.RedPad)    != 0 ? Math.Max(redPad, (byte)1) : (byte)0,
                yellowPad = (pads & FourLanePad.YellowPad) != 0 ? Math.Max(yellowPad, (byte)1) : (byte)0,
                bluePad   = (pads & FourLanePad.BluePad)   != 0 ? Math.Max(bluePad, (byte)1) : (byte)0,
                greenPad  = (pads & FourLanePad.GreenPad)  != 0 ? Math.Max(greenPad, (byte)1) : (byte)0,

                yellowCymbal = (pads & FourLanePad.YellowCymbal) != 0 ? Math.Max(yellowCymbal, (byte)1) : (byte)0,
                blueCymbal   = (pads & FourLanePad.BlueCymbal)   != 0 ? Math.Max(blueCymbal, (byte)1) : (byte)0,
                greenCymbal  = (pads & FourLanePad.GreenCymbal)  != 0 ? Math.Max(greenCymbal, (byte)1) : (byte)0,
            };
        }
    }

    /// <summary>
    /// A <see cref="FourLaneDrumkit"/> which translates its state data into a common
    /// <see cref="TranslatedFourLaneState"/> format.
    /// This variant is for drumkits that use various flags to indicate pad/cymbal hits,
    /// but also separate velocity information into individual values.
    /// </summary>
    internal abstract class TranslatingFourLaneDrumkit_Hybrid<TState> : FourLaneDrumkit, IInputStateCallbackReceiver
        where TState : unmanaged, IFourLaneDrumkitState_FlagButtons, IFourLaneDrumkitState_DistinctVelocities, IInputStateTypeInfo
    {
        private TranslateStateHandler<TState, TranslatedFourLaneState> m_Translator;
        private bool m_HasFlags;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Translator = TranslateState;
            StateTranslator<TState, TranslatedFourLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedFourLaneState TranslateState(ref TState state)
            => TranslateState(ref state, ref m_HasFlags);

        internal static TranslatedFourLaneState TranslateState(ref TState state, ref bool hasFlags)
        {
            var pads = TranslatingFourLaneDrumkit.TranslatePads(ref state, ref hasFlags);
            var buttons = TranslatingFourLaneDrumkit.TranslateButtons(ref state, pads);

            return new TranslatedFourLaneState()
            {
                buttons = (ushort)buttons,

                redPad    = state.redPadVelocity,
                yellowPad = state.yellowPadVelocity,
                bluePad   = state.bluePadVelocity,
                greenPad  = state.greenPadVelocity,

                yellowCymbal = state.yellowCymbalVelocity,
                blueCymbal   = state.blueCymbalVelocity,
                greenCymbal  = state.greenCymbalVelocity,
            };
        }
    }

    /// <summary>
    /// A <see cref="FourLaneDrumkit"/> which translates its state data into a common
    /// <see cref="TranslatedFourLaneState"/> format.
    /// This variant is for drumkits which directly distinguish between pads and cymbals.
    /// </summary>
    internal abstract class TranslatingFourLaneDrumkit_Distinct<TState> : FourLaneDrumkit, IInputStateCallbackReceiver
        where TState : unmanaged, IFourLaneDrumkitState_DistinctVelocities, IInputStateTypeInfo
    {
        protected static readonly TranslateStateHandler<TState, TranslatedFourLaneState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedFourLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedFourLaneState TranslateState(ref TState state)
        {
            var pads = TranslatingFourLaneDrumkit.TranslatePads(ref state);
            var buttons = TranslatingFourLaneDrumkit.TranslateButtons(ref state, pads);

            return new TranslatedFourLaneState()
            {
                buttons = (ushort)buttons,

                redPad    = state.redPadVelocity,
                yellowPad = state.yellowPadVelocity,
                bluePad   = state.bluePadVelocity,
                greenPad  = state.greenPadVelocity,

                yellowCymbal = state.yellowCymbalVelocity,
                blueCymbal   = state.blueCymbalVelocity,
                greenCymbal  = state.greenCymbalVelocity,
            };
        }
    }
}