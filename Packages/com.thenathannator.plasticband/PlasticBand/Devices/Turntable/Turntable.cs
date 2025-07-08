using System;
using System.Collections.Generic;
using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Bitmask of possible button values on a turntable.
    /// </summary>
    [Flags]
    public enum TurntableButton
    {
        None = 0,
        Green = 0x01,
        Red = 0x02,
        Blue = 0x04
    }

    internal interface ITurntableState : IInputStateTypeInfo
    {
        bool south { get; set; }
        bool east { get; set; }
        bool west { get; set; }
        bool north_euphoria { get; set; }

        bool dpadUp { get; set; }
        bool dpadDown { get; set; }
        bool dpadLeft { get; set; }
        bool dpadRight { get; set; }

        bool start { get; set; }
        bool select { get; set; }
        bool system { get; set; }

        bool leftGreen { get; set; }
        bool leftRed { get; set; }
        bool leftBlue { get; set; }

        bool rightGreen { get; set; }
        bool rightRed { get; set; }
        bool rightBlue { get; set; }

        sbyte leftVelocity { get; set; }
        sbyte rightVelocity { get; set; }

        sbyte crossfader { get; set; }
        ushort effectsDial { get; set; }
    }

    /// <summary>
    /// A DJ Hero turntable.
    /// </summary>
    [InputControlLayout(displayName = "DJ Hero Turntable")]
    public class Turntable : InputDevice, ITurntableHaptics
    {
        /// <summary>
        /// The current <see cref="Turntable"/>.
        /// </summary>
        public static Turntable current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="Turntable"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<Turntable> all => s_AllDevices;
        private static readonly List<Turntable> s_AllDevices = new List<Turntable>();

        internal static void Initialize()
        {
            InputSystem.RegisterLayout<Turntable>();
        }

        /// <summary>
        /// The D-pad in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The Start button in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "Select")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The bottom face button in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button in the navigation button compartment.
        /// </summary>
        [InputControl(displayName = "Button North / Euphoria", alias = "euphoria")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The euphoria button on the deck.
        /// </summary>
        public ButtonControl euphoria => buttonNorth;

        /// <summary>
        /// The green button on the left turntable.
        /// </summary>
        [InputControl(displayName = "Left Turntable Green", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl leftTableGreen { get; private set; }

        /// <summary>
        /// The red button on the left turntable.
        /// </summary>
        [InputControl(displayName = "Left Turntable Red", usages = new[] { "Back", "Cancel" })]
        public ButtonControl leftTableRed { get; private set; }

        /// <summary>
        /// The blue button on the left turntable.
        /// </summary>
        [InputControl(displayName = "Left Turntable Blue")]
        public ButtonControl leftTableBlue { get; private set; }

        /// <summary>
        /// The green button on the right turntable.
        /// </summary>
        [InputControl(displayName = "Right Turntable Green", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl rightTableGreen { get; private set; }

        /// <summary>
        /// The red button on the right turntable.
        /// </summary>
        [InputControl(displayName = "Right Turntable Red", usages = new[] { "Back", "Cancel" })]
        public ButtonControl rightTableRed { get; private set; }

        /// <summary>
        /// The blue button on the right turntable.
        /// </summary>
        [InputControl(displayName = "Right Turntable Blue")]
        public ButtonControl rightTableBlue { get; private set; }

        /// <summary>
        /// The rotational speed of the left turntable.
        /// </summary>
        [InputControl(displayName = "Left Turntable Velocity", noisy = true)]
        public AxisControl leftTableVelocity { get; private set; }

        /// <summary>
        /// The rotational speed of the right turntable.
        /// </summary>
        [InputControl(displayName = "Right Turntable Velocity", noisy = true)]
        public AxisControl rightTableVelocity { get; private set; }

        /// <summary>
        /// The absolute rotation of the effects dial on the deck.
        /// </summary>
        /// <remarks>
        /// This value ranges from 0 inclusive to 1 exclusive, where 1 is a full 360-degree rotation.
        /// It is 1 *exclusive* since the dial spins infinitely, and the reported value wraps between
        /// its min/max values, effectively modulo'ing by 1.<br/>
        /// Multiply this value by 360 to get degrees, or by π / 2 to get radians.
        /// </remarks>
        [InputControl(displayName = "Effects Dial")]
        public AxisControl effectsDial { get; private set; }

        /// <summary>
        /// The cross-fader on the deck.
        /// </summary>
        [InputControl(displayName = "Crossfader")]
        public AxisControl crossfader { get; private set; }

        /// <summary>
        /// The number of pads available on the drumkit.
        /// </summary>
        public const int ButtonCount = 3;

        /// <summary>
        /// Retrieves a left table button control by index.<br/>
        /// 0 = green, 1 = red, 2 = blue.
        /// </summary>
        public ButtonControl GetLeftButton(int index)
        {
            switch (index)
            {
                case 0: return leftTableGreen;
                case 1: return leftTableRed;
                case 2: return leftTableBlue;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {ButtonCount}!");
            }
        }

        /// <summary>
        /// Retrieves a left table button control by enum value.
        /// </summary>
        public ButtonControl GetLeftButton(TurntableButton button)
        {
            switch (button)
            {
                case TurntableButton.Green: return leftTableGreen;
                case TurntableButton.Red: return leftTableRed;
                case TurntableButton.Blue: return leftTableBlue;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current left table button states.
        /// </summary>
        public TurntableButton GetLeftButtonMask()
        {
            var frets = TurntableButton.None;
            if (leftTableGreen.isPressed) frets |= TurntableButton.Green;
            if (leftTableRed.isPressed) frets |= TurntableButton.Red;
            if (leftTableBlue.isPressed) frets |= TurntableButton.Blue;
            return frets;
        }

        /// <summary>
        /// Retrives a bitmask of the current left table button states.
        /// </summary>
        public TurntableButton GetLeftButtonMask(InputEventPtr eventPtr)
        {
            var frets = TurntableButton.None;
            if (leftTableGreen.IsPressedInEvent(eventPtr)) frets |= TurntableButton.Green;
            if (leftTableRed.IsPressedInEvent(eventPtr)) frets |= TurntableButton.Red;
            if (leftTableBlue.IsPressedInEvent(eventPtr)) frets |= TurntableButton.Blue;
            return frets;
        }

        /// <summary>
        /// Retrieves a right table button control by index.<br/>
        /// 0 = green, 1 = red, 2 = blue.
        /// </summary>
        public ButtonControl GetRightButton(int index)
        {
            switch (index)
            {
                case 0: return rightTableGreen;
                case 1: return rightTableRed;
                case 2: return rightTableBlue;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {ButtonCount}!");
            }
        }

        /// <summary>
        /// Retrieves a right table button control by enum value.
        /// </summary>
        public ButtonControl GetRightButton(TurntableButton button)
        {
            switch (button)
            {
                case TurntableButton.Green: return rightTableGreen;
                case TurntableButton.Red: return rightTableRed;
                case TurntableButton.Blue: return rightTableBlue;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current right table button states.
        /// </summary>
        public TurntableButton GetRightButtonMask()
        {
            var mask = TurntableButton.None;
            if (rightTableGreen.isPressed) mask |= TurntableButton.Green;
            if (rightTableRed.isPressed) mask |= TurntableButton.Red;
            if (rightTableBlue.isPressed) mask |= TurntableButton.Blue;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the current right table button states.
        /// </summary>
        public TurntableButton GetRightButtonMask(InputEventPtr eventPtr)
        {
            var mask = TurntableButton.None;
            if (rightTableGreen.IsPressedInEvent(eventPtr)) mask |= TurntableButton.Green;
            if (rightTableRed.IsPressedInEvent(eventPtr)) mask |= TurntableButton.Red;
            if (rightTableBlue.IsPressedInEvent(eventPtr)) mask |= TurntableButton.Blue;
            return mask;
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));

            buttonSouth = GetChildControl<ButtonControl>(nameof(buttonSouth));
            buttonEast = GetChildControl<ButtonControl>(nameof(buttonEast));
            buttonWest = GetChildControl<ButtonControl>(nameof(buttonWest));
            buttonNorth = GetChildControl<ButtonControl>(nameof(buttonNorth));

            leftTableGreen = GetChildControl<ButtonControl>(nameof(leftTableGreen));
            leftTableRed = GetChildControl<ButtonControl>(nameof(leftTableRed));
            leftTableBlue = GetChildControl<ButtonControl>(nameof(leftTableBlue));

            rightTableGreen = GetChildControl<ButtonControl>(nameof(rightTableGreen));
            rightTableRed = GetChildControl<ButtonControl>(nameof(rightTableRed));
            rightTableBlue = GetChildControl<ButtonControl>(nameof(rightTableBlue));

            leftTableVelocity = GetChildControl<AxisControl>(nameof(leftTableVelocity));
            rightTableVelocity = GetChildControl<AxisControl>(nameof(rightTableVelocity));

            effectsDial = GetChildControl<AxisControl>(nameof(effectsDial));
            crossfader = GetChildControl<AxisControl>(nameof(crossfader));
        }

        /// <inheritdoc/>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public virtual void PauseHaptics() {}

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public virtual void ResumeHaptics() {}

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public virtual void ResetHaptics() {}

        /// <inheritdoc cref="ITurntableHaptics.SetEuphoriaBlink(bool)"/>
        public virtual void SetEuphoriaBlink(bool enable) {}
    }
}
