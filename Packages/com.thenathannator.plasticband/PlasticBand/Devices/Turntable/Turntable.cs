using System;
using System.Collections.Generic;
using System.Diagnostics;
using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [Flags]
    public enum TurntableButtons
    {
        None = 0,
        Green = 0x01,
        Red = 0x02,
        Blue = 0x04
    }

    /// <summary>
    /// A DJ Hero turntable.
    /// </summary>
    [InputControlLayout(displayName = "DJ Hero Turntable")]
    public class Turntable : InputDevice, ITurntableHaptics, IInputUpdateCallbackReceiver
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

        /// <summary>
        /// Registers <see cref="Turntable"/> to the input system.
        /// </summary>
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
        [InputControl(displayName = "Back")]
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
        [InputControl(displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The euphoria button on the deck.
        /// </summary>
        [InputControl(displayName = "Euphoria")]
        public ButtonControl euphoria { get; private set; }

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
        /// The effects dial on the deck.
        /// </summary>
        [InputControl(displayName = "Effects Dial")]
        public AxisControl effectsDial { get; private set; }

        /// <summary>
        /// The cross-fader on the deck.
        /// </summary>
        [InputControl(displayName = "Crossfader")]
        public AxisControl crossFader { get; private set; }

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
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Retrieves a left table button control by enum value.
        /// </summary>
        public ButtonControl GetLeftButton(TurntableButtons button)
        {
            switch (button)
            {
                case TurntableButtons.Green: return leftTableGreen;
                case TurntableButtons.Red: return leftTableRed;
                case TurntableButtons.Blue: return leftTableBlue;
                default: throw new ArgumentOutOfRangeException(nameof(button));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current left table button states.
        /// </summary>
        public TurntableButtons GetLeftButtonMask()
        {
            var frets = TurntableButtons.None;
            if (leftTableGreen.isPressed) frets |= TurntableButtons.Green;
            if (leftTableRed.isPressed) frets |= TurntableButtons.Red;
            if (leftTableBlue.isPressed) frets |= TurntableButtons.Blue;
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
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Retrieves a right table button control by enum value.
        /// </summary>
        public ButtonControl GetRightButton(TurntableButtons button)
        {
            switch (button)
            {
                case TurntableButtons.Green: return rightTableGreen;
                case TurntableButtons.Red: return rightTableRed;
                case TurntableButtons.Blue: return rightTableBlue;
                default: throw new ArgumentOutOfRangeException(nameof(button));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current right table button states.
        /// </summary>
        public TurntableButtons GetRightButtonMask()
        {
            var mask = TurntableButtons.None;
            if (rightTableGreen.isPressed) mask |= TurntableButtons.Green;
            if (rightTableRed.isPressed) mask |= TurntableButtons.Red;
            if (rightTableBlue.isPressed) mask |= TurntableButtons.Blue;
            return mask;
        }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
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

            euphoria = GetChildControl<ButtonControl>(nameof(euphoria));

            leftTableGreen = GetChildControl<ButtonControl>(nameof(leftTableGreen));
            leftTableRed = GetChildControl<ButtonControl>(nameof(leftTableRed));
            leftTableBlue = GetChildControl<ButtonControl>(nameof(leftTableBlue));

            rightTableGreen = GetChildControl<ButtonControl>(nameof(rightTableGreen));
            rightTableRed = GetChildControl<ButtonControl>(nameof(rightTableRed));
            rightTableBlue = GetChildControl<ButtonControl>(nameof(rightTableBlue));

            leftTableVelocity = GetChildControl<AxisControl>(nameof(leftTableVelocity));
            rightTableVelocity = GetChildControl<AxisControl>(nameof(rightTableVelocity));

            effectsDial = GetChildControl<AxisControl>(nameof(effectsDial));
            crossFader = GetChildControl<AxisControl>(nameof(crossFader));
        }

        /// <summary>
        /// Sets this device as the current <see cref="Turntable"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        /// <summary>
        /// Processes when this device is added to the system.
        /// </summary>
        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        /// <summary>
        /// Processes when this device is removed from the system.
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }

        // Timer used for the euphoria effect
        private readonly Stopwatch m_EuphoriaTimer = new Stopwatch();

        // Period length of the euphoria timer
        internal const int kEuphoriaPeriod = 3000;
        // Provided for convenience
        internal const int kEuphoriaPeriodHalf = kEuphoriaPeriod / 2;

        // Value to force the euphoria light to be disabled
        internal const float kEuphoriaForceDisable = -1;

        // States for pausing/resuming haptics
        protected bool m_euphoriaEnabled;
        protected bool m_euphoriaPaused;

        // Handles euphoria effect processing
        void IInputUpdateCallbackReceiver.OnUpdate()
        {
            // Handle state changes
            if (!m_EuphoriaTimer.IsRunning)
            {
                if (!m_euphoriaPaused && m_euphoriaEnabled)
                {
                    m_EuphoriaTimer.Start();
                }
                else
                {
                    return;
                }
            }
            else if (m_euphoriaPaused || !m_euphoriaEnabled)
            {
                m_EuphoriaTimer.Reset();
                OnEuphoriaTick(kEuphoriaForceDisable);
                return;
            }

            long elapsed = m_EuphoriaTimer.ElapsedMilliseconds;
            // End of euphoria period
            if (elapsed >= kEuphoriaPeriod)
            {
                OnEuphoriaTick(0);
                m_EuphoriaTimer.Restart();
            }
            // First half of euphoria period
            // Brightness increases gradually
            else if (elapsed < kEuphoriaPeriodHalf)
            {
                float brightness = (float)elapsed / kEuphoriaPeriodHalf;
                OnEuphoriaTick(brightness);
            }
            // Second half of euphoria period
            // Brightness decreases gradually
            else
            {
                elapsed -= kEuphoriaPeriodHalf;
                float brightness = 1f - ((float)elapsed / kEuphoriaPeriodHalf);
                OnEuphoriaTick(brightness);
            }
        }

        /// <summary>
        /// Handles euphoria effect processing.
        /// </summary>
        protected virtual void OnEuphoriaTick(float brightness) { }

        /// <inheritdoc cref="ITurntableHaptics.SetEuphoriaBlink(bool)"/>
        public void SetEuphoriaBlink(bool enable)
        {
            m_euphoriaEnabled = enable;
        }

        /// <summary>
        /// Temporarily disables the euphoria effect if it is enabled.
        /// </summary>
        /// <remarks>
        /// Note that this only preserves whether or not it was enabled,
        /// as not all turntables support specifying an exact brightness.
        /// </remarks>
        public void PauseHaptics()
        {
            m_euphoriaPaused = true;
        }

        /// <summary>
        /// Restores the euphoria effect's state.
        /// </summary>
        public void ResumeHaptics()
        {
            m_euphoriaPaused = false;
        }

        /// <summary>
        /// Resets the euphoria effect.
        /// </summary>
        public void ResetHaptics()
        {
            m_euphoriaEnabled = false;
            m_euphoriaPaused = false;
        }
    }
}
