using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedRiffmasterGuitarButton
    {
        DpadUp = 0,
        DpadDown = 1,
        DpadLeft = 2,
        DpadRight = 3,

        LeftStickPress = 4,
        RightStickPress = 5,
        P1 = 6,

        Start = 7,
        Select = 8,
        System = 9,
    }

    [Flags]
    internal enum TranslatedRiffmasterGuitarButtonMask : ushort
    {
        None = 0,

        DpadUp = 1 << TranslatedRiffmasterGuitarButton.DpadUp,
        DpadDown = 1 << TranslatedRiffmasterGuitarButton.DpadDown,
        DpadLeft = 1 << TranslatedRiffmasterGuitarButton.DpadLeft,
        DpadRight = 1 << TranslatedRiffmasterGuitarButton.DpadRight,

        LeftStickPress = 1 << TranslatedRiffmasterGuitarButton.LeftStickPress,
        RightStickPress = 1 << TranslatedRiffmasterGuitarButton.RightStickPress,
        P1 = 1 << TranslatedRiffmasterGuitarButton.P1,

        Start = 1 << TranslatedRiffmasterGuitarButton.Start,
        Select = 1 << TranslatedRiffmasterGuitarButton.Select,
        System = 1 << TranslatedRiffmasterGuitarButton.System,
    }

    /// <summary>
    /// The format which <see cref="TranslatingRiffmasterGuitar{TState}"/>s translate state into.
    /// </summary>
    /// <seealso cref="TranslatingRiffmasterGuitar{TState}"/>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedRiffmasterGuitarState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('R', 'F', 'G', 'T');
        public FourCC format => Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedRiffmasterGuitarButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedRiffmasterGuitarButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedRiffmasterGuitarButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedRiffmasterGuitarButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedRiffmasterGuitarButton.DpadRight)]

        [InputControl(name = "leftStickPress", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.LeftStickPress)]
        [InputControl(name = "rightStickPress", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.RightStickPress)]
        [InputControl(name = "p1Button", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.P1, displayName = "P1")]

        [InputControl(name = "start", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.Start)]
        [InputControl(name = "select", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.System)]
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

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", offset = 0, layout = "IntAxis", format = "SBYT", parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        [InputControl(name = "leftStick/left", offset = 0, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=-128")]
        [InputControl(name = "leftStick/right", offset = 0, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=127")]
        [InputControl(name = "leftStick/y", offset = 1, layout = "IntAxis", format = "SBYT", parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        [InputControl(name = "leftStick/up", offset = 1, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=127")]
        [InputControl(name = "leftStick/down", offset = 1, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=-128")]
        public sbyte leftStickX;
        public sbyte leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", offset = 0, layout = "IntAxis", format = "SBYT", parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        [InputControl(name = "rightStick/left", offset = 0, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=-128")]
        [InputControl(name = "rightStick/right", offset = 0, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=127")]
        [InputControl(name = "rightStick/y", offset = 1, layout = "IntAxis", format = "SBYT", parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        [InputControl(name = "rightStick/up", offset = 1, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=127")]
        [InputControl(name = "rightStick/down", offset = 1, layout = "IntButton", format = "SBYT", parameters = "minValue=0,maxValue=-128")]
        public sbyte rightStickX;
        public sbyte rightStickY;

        [InputControl(name = "whammy", layout = "IntAxis", parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x00")]
        public byte whammy;

        [InputControl(name = "tilt", layout = "IntAxis", noisy = true, parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        public sbyte tilt;

        [InputControl(name = "pickupSwitch", layout = "Integer")]
        public byte pickupSwitch;
    }

    /// <summary>
    /// A <see cref="RiffmasterGuitar"/> which translates its state data into a common
    /// <see cref="TranslatedRiffmasterGuitarState"/> format.
    /// </summary>
    // This is done to properly handle the joystick click, as it conflicts with the solo fret flag.
    internal abstract class TranslatingRiffmasterGuitar<TState> : RiffmasterGuitar, IInputStateCallbackReceiver
        where TState : unmanaged, IRiffmasterGuitarState, IInputStateTypeInfo
    {
        protected static readonly TranslateStateHandler<TState, TranslatedRiffmasterGuitarState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedRiffmasterGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedRiffmasterGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedRiffmasterGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        internal static TranslatedRiffmasterGuitarState TranslateState(ref TState state)
        {
            var buttons = TranslatedRiffmasterGuitarButtonMask.None;

            // D-pad
            if (state.dpadUp) buttons |= TranslatedRiffmasterGuitarButtonMask.DpadUp;
            if (state.dpadDown) buttons |= TranslatedRiffmasterGuitarButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedRiffmasterGuitarButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedRiffmasterGuitarButtonMask.DpadRight;

            // Misc.
            if (state.leftStickPress) buttons |= TranslatedRiffmasterGuitarButtonMask.LeftStickPress;
            if (state.rightStickPress) buttons |= TranslatedRiffmasterGuitarButtonMask.RightStickPress;
            if (state.p1) buttons |= TranslatedRiffmasterGuitarButtonMask.P1;

            // Menu/system
            if (state.start) buttons |= TranslatedRiffmasterGuitarButtonMask.Start;
            if (state.select) buttons |= TranslatedRiffmasterGuitarButtonMask.Select;
            if (state.system) buttons |= TranslatedRiffmasterGuitarButtonMask.System;

            return new TranslatedRiffmasterGuitarState()
            {
                buttons = (ushort)buttons,

                frets = (byte)TranslatingRockBandGuitar.TranslateFrets(
                    state.green, state.red, state.yellow, state.blue, state.orange
                ),
                soloFrets = (byte)TranslatingRockBandGuitar.TranslateFrets(
                    state.soloGreen, state.soloRed, state.soloYellow, state.soloBlue, state.soloOrange
                ),

                leftStickX = state.leftStickX,
                leftStickY = state.leftStickY,

                rightStickX = state.rightStickX,
                rightStickY = state.rightStickY,

                whammy = state.whammy,
                tilt = state.tilt,
                pickupSwitch = (byte)state.pickupSwitch,
            };
        }
    }
}
