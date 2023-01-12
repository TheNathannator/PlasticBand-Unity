using System.Collections.Generic;
using System.Diagnostics;
using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
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

        internal static void Initialize()
        {
            InputSystem.RegisterLayout<Turntable>();
        }

        /// <summary>
        /// The D-pad in the navigation button compartment.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The Start button in the navigation button compartment.
        /// </summary>
        [InputControl(name = "startButton", displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button in the navigation button compartment.
        /// </summary>
        [InputControl(name = "selectButton", displayName = "Back")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The bottom face button in the navigation button compartment.
        /// </summary>
        [InputControl(name = "buttonSouth", displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button in the navigation button compartment.
        /// </summary>
        [InputControl(name = "buttonEast", displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button in the navigation button compartment.
        /// </summary>
        [InputControl(name = "buttonWest", displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button in the navigation button compartment.
        /// </summary>
        [InputControl(name = "buttonNorth", displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The euphoria button on the deck.
        /// </summary>
        [InputControl(name = "euphoria", displayName = "Euphoria")]
        public ButtonControl euphoria { get; private set; }

        /// <summary>
        /// The green button on the left turntable.
        /// </summary>
        [InputControl(name = "leftTableGreen", displayName = "Left Turntable Green", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl leftTableGreen { get; private set; }

        /// <summary>
        /// The red button on the left turntable.
        /// </summary>
        [InputControl(name = "leftTableRed", displayName = "Left Turntable Red", usages = new[] { "Back", "Cancel" })]
        public ButtonControl leftTableRed { get; private set; }

        /// <summary>
        /// The blue button on the left turntable.
        /// </summary>
        [InputControl(name = "leftTableBlue", displayName = "Left Turntable Blue")]
        public ButtonControl leftTableBlue { get; private set; }

        /// <summary>
        /// The green button on the right turntable.
        /// </summary>
        [InputControl(name = "rightTableGreen", displayName = "Right Turntable Green", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl rightTableGreen { get; private set; }

        /// <summary>
        /// The red button on the right turntable.
        /// </summary>
        [InputControl(name = "rightTableRed", displayName = "Right Turntable Red", usages = new[] { "Back", "Cancel" })]
        public ButtonControl rightTableRed { get; private set; }

        /// <summary>
        /// The blue button on the right turntable.
        /// </summary>
        [InputControl(name = "rightTableBlue", displayName = "Right Turntable Blue")]
        public ButtonControl rightTableBlue { get; private set; }

        /// <summary>
        /// The rotational speed of the left turntable.
        /// </summary>
        [InputControl(name = "leftTableVelocity", noisy = true, displayName = "Left Turntable Velocity")]
        public AxisControl leftTableVelocity { get; private set; }

        /// <summary>
        /// The rotational speed of the right turntable.
        /// </summary>
        [InputControl(name = "rightTableVelocity", noisy = true, displayName = "Right Turntable Velocity")]
        public AxisControl rightTableVelocity { get; private set; }

        /// <summary>
        /// The effects dial on the deck.
        /// </summary>
        [InputControl(name = "effectsDial", displayName = "Effects Dial")]
        public AxisControl effectsDial { get; private set; }

        /// <summary>
        /// The cross-fader on the deck.
        /// </summary>
        [InputControl(name = "crossFader", displayName = "Crossfader")]
        public AxisControl crossFader { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>("dpad");

            startButton = GetChildControl<ButtonControl>("startButton");
            selectButton = GetChildControl<ButtonControl>("selectButton");

            buttonSouth = GetChildControl<ButtonControl>("buttonSouth");
            buttonEast = GetChildControl<ButtonControl>("buttonEast");
            buttonWest = GetChildControl<ButtonControl>("buttonWest");
            buttonNorth = GetChildControl<ButtonControl>("buttonNorth");

            euphoria = GetChildControl<ButtonControl>("euphoria");

            leftTableGreen = GetChildControl<ButtonControl>("leftTableGreen");
            leftTableRed = GetChildControl<ButtonControl>("leftTableRed");
            leftTableBlue = GetChildControl<ButtonControl>("leftTableBlue");

            rightTableGreen = GetChildControl<ButtonControl>("rightTableGreen");
            rightTableRed = GetChildControl<ButtonControl>("rightTableRed");
            rightTableBlue = GetChildControl<ButtonControl>("rightTableBlue");

            leftTableVelocity = GetChildControl<AxisControl>("leftTableVelocity");
            rightTableVelocity = GetChildControl<AxisControl>("rightTableVelocity");

            effectsDial = GetChildControl<AxisControl>("effectsDial");
            crossFader = GetChildControl<AxisControl>("crossFader");
        }

        /// <summary>
        /// Sets this device as the current <see cref="Turntable"/>.
        /// </summary>
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
