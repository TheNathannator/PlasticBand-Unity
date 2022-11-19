using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A DJ Hero turntable.
    /// </summary>
    [InputControlLayout(displayName = "DJ Hero Turntable")]
    public class Turntable : BaseDevice<Turntable>, ITurntableHaptics
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<Turntable>();
        }

        /// <summary>
        /// The D-pad in the navigation button compartment.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", displayName = "D-Pad Up")]
        [InputControl(name = "dpad/down", displayName = "D-Pad Down")]
        [InputControl(name = "dpad/left", displayName = "D-Pad Left")]
        [InputControl(name = "dpad/right", displayName = "D-Pad Right")]
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
        /// The current (or saved, if paused) brightness of the Euphoria button's light.
        /// </summary>
        public float EuphoriaBrightness { get; protected set; }
        protected bool isPaused;

        /// <inheritdoc cref="ITurntableHaptics.SetEuphoriaBrightness(float)"/>
        public virtual void SetEuphoriaBrightness(float brightness)
        {
            if (!isPaused)
            {
                EuphoriaBrightness = brightness;
                SendEuphoriaCommand(brightness);
            }
        }

        /// <summary>
        /// Sends a Euphoria light command to the turntable.
        /// </summary>
        protected virtual void SendEuphoriaCommand(float brightness) { }

        /// <summary>
        /// Turns off the euphoria button light while keeping its current state.
        /// </summary>
        public virtual void PauseHaptics()
        {
            if (!isPaused)
            {
                isPaused = true;
                SendEuphoriaCommand(0);
            }
        }

        /// <summary>
        /// Restores the euphoria button light's state.
        /// </summary>
        public virtual void ResumeHaptics()
        {
            if (isPaused)
            {
                isPaused = false;
                SetEuphoriaBrightness(EuphoriaBrightness);
            }
        }

        /// <summary>
        /// Resets the euphoria button light.
        /// </summary>
        public virtual void ResetHaptics()
        {
            isPaused = false;
            SetEuphoriaBrightness(0);
        }
    }
}
