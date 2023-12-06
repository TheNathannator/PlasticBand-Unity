using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    internal enum TranslatedFiveLaneButton
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
    internal enum TranslatedFiveLaneButtonMask : ushort
    {
        None = 0,

        South = 1 << TranslatedFiveLaneButton.South,
        East = 1 << TranslatedFiveLaneButton.East,
        West = 1 << TranslatedFiveLaneButton.West,
        North = 1 << TranslatedFiveLaneButton.North,

        DpadUp = 1 << TranslatedFiveLaneButton.DpadUp,
        DpadDown = 1 << TranslatedFiveLaneButton.DpadDown,
        DpadLeft = 1 << TranslatedFiveLaneButton.DpadLeft,
        DpadRight = 1 << TranslatedFiveLaneButton.DpadRight,

        Start = 1 << TranslatedFiveLaneButton.Start,
        Select = 1 << TranslatedFiveLaneButton.Select,
        System = 1 << TranslatedFiveLaneButton.System,
    }

    /// <summary>
    /// The format which <see cref="TranslatingFiveLaneDrumkit{TState}"/>s translate state into.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TranslatedFiveLaneState : IInputStateTypeInfo
    {
        public static FourCC Format => new FourCC('D', 'R', 'M', '5');
        public FourCC format => Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFiveLaneButton.South)]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFiveLaneButton.East)]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFiveLaneButton.West)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFiveLaneButton.North)]

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = (int)TranslatedFiveLaneButton.DpadUp, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = (int)TranslatedFiveLaneButton.DpadUp)]
        [InputControl(name = "dpad/down", bit = (int)TranslatedFiveLaneButton.DpadDown)]
        [InputControl(name = "dpad/left", bit = (int)TranslatedFiveLaneButton.DpadLeft)]
        [InputControl(name = "dpad/right", bit = (int)TranslatedFiveLaneButton.DpadRight)]

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.Start)]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.Select)]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.System)]
        public ushort buttons;

        private const string kPadParameters = "intPressPoint=1,minValue=0,maxValue=255";

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte redPad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte yellowCymbal;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte bluePad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte orangeCymbal;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte greenPad;

        [InputControl(layout = "IntButton", format = "BYTE", parameters = kPadParameters)]
        public byte kick;
    }

    /// <summary>
    /// A <see cref="FiveLaneDrumkit"/> which translates its state data into a common
    /// <see cref="TranslatedFiveLaneState"/> format.
    /// </summary>
    // This is done to greatly simplify velocity support and distinguishing between pads and face buttons.
    // Doing these via only custom control types would be very duplicative and hacky, and I'd rather ensure things are
    // done properly than wrestle with using mechanisms designed for simple, trivial state on more complex state.
    internal abstract class TranslatingFiveLaneDrumkit<TState> : FiveLaneDrumkit, IInputStateCallbackReceiver
        where TState : unmanaged, IFiveLaneDrumkitState, IInputStateTypeInfo
    {
        private static readonly TranslateStateHandler<TState, TranslatedFiveLaneState> s_Translator = TranslateState;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator<TState, TranslatedFiveLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<TState, TranslatedFiveLaneState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<TState, TranslatedFiveLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        private static TranslatedFiveLaneState TranslateState(ref TState state)
        {
            var translated = new TranslatedFiveLaneState()
            {
                redPad       = state.red_east ? state.redVelocity : (byte)0,
                yellowCymbal = state.yellow_north ? state.yellowVelocity : (byte)0,
                bluePad      = state.blue_west ? state.blueVelocity : (byte)0,
                orangeCymbal = state.orange ? state.orangeVelocity : (byte)0,
                greenPad     = state.green_south ? state.greenVelocity : (byte)0,
                kick         = state.kick ? state.kickVelocity : (byte)0,

                // TODO: Determine if these kits need the same velocity limit as RB kits do
                // If they do, we can't reliably detect face button presses and will have to remove them from the layout
                // redPad       = state.red_east ? Math.Max(state.redVelocity, (byte)1) : (byte)0,
                // yellowCymbal = state.yellow_north ? Math.Max(state.yellowVelocity, (byte)1) : (byte)0,
                // bluePad      = state.blue_west ? Math.Max(state.blueVelocity, (byte)1) : (byte)0,
                // orangeCymbal = state.orange ? Math.Max(state.orangeVelocity, (byte)1) : (byte)0,
                // greenPad     = state.green_south ? Math.Max(state.greenVelocity, (byte)1) : (byte)0,
                // kick         = state.kick ? Math.Max(state.kickVelocity, (byte)1) : (byte)0,
            };

            // Face buttons; these are ignored if the corresponding pad/cymbal is also active
            var buttons = TranslatedFiveLaneButtonMask.None;
            // A, cross
            if (translated.greenPad == 0 && state.green_south) buttons |= TranslatedFiveLaneButtonMask.South;
            // B, circle
            if (translated.redPad == 0 && state.red_east) buttons |= TranslatedFiveLaneButtonMask.East;
            // X, square
            if (translated.bluePad == 0 && state.blue_west) buttons |= TranslatedFiveLaneButtonMask.West;
            // Y, triangle
            if (translated.yellowCymbal == 0 && state.yellow_north) buttons |= TranslatedFiveLaneButtonMask.North;

            if (state.dpadUp) buttons |= TranslatedFiveLaneButtonMask.DpadUp;
            if (state.dpadDown) buttons |= TranslatedFiveLaneButtonMask.DpadDown;
            if (state.dpadLeft) buttons |= TranslatedFiveLaneButtonMask.DpadLeft;
            if (state.dpadRight) buttons |= TranslatedFiveLaneButtonMask.DpadRight;

            if (state.start) buttons |= TranslatedFiveLaneButtonMask.Start;
            if (state.select) buttons |= TranslatedFiveLaneButtonMask.Select;
            if (state.system) buttons |= TranslatedFiveLaneButtonMask.System;

            translated.buttons = (ushort)buttons;
            return translated;
        }
    }
}