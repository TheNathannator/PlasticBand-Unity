using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct TurntableState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0, displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", bit = 0, displayName = "D-Pad Up")]
        [InputControl(name = "dpad/down", bit = 1, displayName = "D-Pad Down")]
        [InputControl(name = "dpad/left", bit = 2, displayName = "D-Pad Left")]
        [InputControl(name = "dpad/right", bit = 3, displayName = "D-Pad Right")]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 4, displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "buttonEast", layout = "Button", bit = 5, displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "buttonWest", layout = "Button", bit = 6, displayName = "Button West")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 7, displayName = "Button North")]

        [InputControl(name = "euphoria", layout = "Button", bit = 8, displayName = "Euphoria")]
        [InputControl(name = "startButton", layout = "Button", bit = 12, displayName = "Start", usage = "Menu")]
        [InputControl(name = "selectButton", layout = "Button", bit = 13, displayName = "Back")]
        public ushort buttons;

        [InputControl(name = "leftTableGreen", layout = "Button", bit = 0, displayName = "Left Turntable Green", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "leftTableRed", layout = "Button", bit = 1, displayName = "Left Turntable Red", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "leftTableBlue", layout = "Button", bit = 2, displayName = "Left Turntable Blue")]
        public byte leftTableButtons;

        [InputControl(name = "rightTableGreen", layout = "Button", bit = 0, displayName = "Right Turntable Green", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "rightTableRed", layout = "Button", bit = 1, displayName = "Right Turntable Red", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "rightTableBlue", layout = "Button", bit = 2, displayName = "Right Turntable Blue")]
        public byte rightTableButtons;

        [InputControl(name = "leftTableVelocity", layout = "Axis", noisy = true, displayName = "Left Turntable Velocity")]
        public byte leftTableVelocity;

        [InputControl(name = "rightTableVelocity", layout = "Axis", noisy = true, displayName = "Right Turntable Velocity")]
        public byte rightTableVelocity;

        [InputControl(name = "effectsDial", layout = "Axis", displayName = "Effects Dial")]
        public byte effectsDial;

        [InputControl(name = "crossFader", layout = "Axis", displayName = "Crossfader")]
        public byte crossFader;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A DJ Hero turntable.
    /// </summary>
    [InputControlLayout(stateType = typeof(TurntableState), displayName = "DJ Hero Turntable", isGenericTypeOfDevice = true)]
    public class Turntable : BaseDevice<Turntable>, ITurntableHaptics
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<Turntable>();
        }

        /// <summary>
        /// The D-pad in the navigation button compartment.
        /// </summary>
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The Start button in the navigation button compartment.
        /// </summary>
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button in the navigation button compartment.
        /// </summary>
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The bottom face button in the navigation button compartment.
        /// </summary>
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button in the navigation button compartment.
        /// </summary>
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button in the navigation button compartment.
        /// </summary>
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button in the navigation button compartment.
        /// </summary>
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The euphoria button on the deck.
        /// </summary>
        public ButtonControl euphoria { get; private set; }

        /// <summary>
        /// The green button on the left turntable.
        /// </summary>
        public ButtonControl leftTableGreen { get; private set; }

        /// <summary>
        /// The red button on the left turntable.
        /// </summary>
        public ButtonControl leftTableRed { get; private set; }

        /// <summary>
        /// The blue button on the left turntable.
        /// </summary>
        public ButtonControl leftTableBlue { get; private set; }

        /// <summary>
        /// The green button on the right turntable.
        /// </summary>
        public ButtonControl rightTableGreen { get; private set; }

        /// <summary>
        /// The red button on the right turntable.
        /// </summary>
        public ButtonControl rightTableRed { get; private set; }

        /// <summary>
        /// The blue button on the right turntable.
        /// </summary>
        public ButtonControl rightTableBlue { get; private set; }

        /// <summary>
        /// The rotational speed of the left turntable.
        /// </summary>
        public AxisControl leftTableVelocity { get; private set; }

        /// <summary>
        /// The rotational speed of the right turntable.
        /// </summary>
        public AxisControl rightTableVelocity { get; private set; }

        /// <summary>
        /// The effects dial on the deck.
        /// </summary>
        public AxisControl effectsDial { get; private set; }

        /// <summary>
        /// The cross-fader on the deck.
        /// </summary>
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
