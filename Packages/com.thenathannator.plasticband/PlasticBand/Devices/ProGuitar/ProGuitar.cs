using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

// TODO: Solo GRYBO frets
// Not implemented right now because:
// 1. it's not known where the solo fret flag is for Xbox 360 Pro guitars
// 2. it's gonna be a pain to put them together

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band 3 Pro Guitar controller.
    /// </summary>
    [InputControlLayout(displayName = "Pro Guitar")]
    public class ProGuitar : BaseDevice<ProGuitar>
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<ProGuitar>();
        }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The bottom face button on the guitar.
        /// </summary>
        [InputControl(name = "buttonSouth", displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button on the guitar.
        /// </summary>
        [InputControl(name = "buttonEast", displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button on the guitar.
        /// </summary>
        [InputControl(name = "buttonWest", displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button on the guitar.
        /// </summary>
        [InputControl(name = "buttonNorth", displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The Start button on the guitar.
        /// </summary>
        [InputControl(name = "startButton", displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the guitar.
        /// </summary>
        [InputControl(name = "selectButton", displayName = "Back")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 1st string.
        /// </summary>
        [InputControl(name = "fret1", displayName = "String 1 Fret")]
        public IntegerControl fret1 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 2nd string.
        /// </summary>
        [InputControl(name = "fret2", displayName = "String 2 Fret")]
        public IntegerControl fret2 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 3rd string.
        /// </summary>
        [InputControl(name = "fret3", displayName = "String 3 Fret")]
        public IntegerControl fret3 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 4th string.
        /// </summary>
        [InputControl(name = "fret4", displayName = "String 4 Fret")]
        public IntegerControl fret4 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 5th string.
        /// </summary>
        [InputControl(name = "fret5", displayName = "String 5 Fret")]
        public IntegerControl fret5 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 6th string.
        /// </summary>
        [InputControl(name = "fret6", displayName = "String 6 Fret")]
        public IntegerControl fret6 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 1st string.
        /// </summary>
        [InputControl(name = "velocity1", displayName = "String 1 Velocity")]
        public AxisControl velocity1 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 2nd string.
        /// </summary>
        [InputControl(name = "velocity2", displayName = "String 2 Velocity")]
        public AxisControl velocity2 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 3rd string.
        /// </summary>
        [InputControl(name = "velocity3", displayName = "String 3 Velocity")]
        public AxisControl velocity3 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 4th string.
        /// </summary>
        [InputControl(name = "velocity4", displayName = "String 4 Velocity")]
        public AxisControl velocity4 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 5th string.
        /// </summary>
        [InputControl(name = "velocity5", displayName = "String 5 Velocity")]
        public AxisControl velocity5 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 6th string.
        /// </summary>
        [InputControl(name = "velocity6", displayName = "String 6 Velocity")]
        public AxisControl velocity6 { get; private set; }

        /// <summary>
        /// The emulated green fret input on the guitar.
        /// </summary>
        [InputControl(name = "greenFret", displayName = "Green Fret", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenFret { get; private set; }

        /// <summary>
        /// The emulated red fret input on the guitar.
        /// </summary>
        [InputControl(name = "redFret", displayName = "Red Fret", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redFret { get; private set; }

        /// <summary>
        /// The emulated yellow fret input on the guitar.
        /// </summary>
        [InputControl(name = "yellowFret", displayName = "Yellow Fret")]
        public ButtonControl yellowFret { get; private set; }

        /// <summary>
        /// The emulated blue fret input on the guitar.
        /// </summary>
        [InputControl(name = "blueFret", displayName = "Blue Fret")]
        public ButtonControl blueFret { get; private set; }

        /// <summary>
        /// The emulated orange fret input on the guitar.
        /// </summary>
        [InputControl(name = "orangeFret", displayName = "Orange Fret")]
        public ButtonControl orangeFret { get; private set; }

        /// <summary>
        /// The guitar's tilt orientation.
        /// </summary>
        [InputControl(name = "tilt", noisy = true, displayName = "Tilt")]
        public AxisControl tilt { get; private set; }

        /// <summary>
        /// The guitar's whammy bar.
        /// </summary>
        [InputControl(name = "whammy", displayName = "Whammy")]
        public AxisControl whammy { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>("dpad");

            buttonSouth = GetChildControl<ButtonControl>("buttonSouth");
            buttonEast = GetChildControl<ButtonControl>("buttonEast");
            buttonWest = GetChildControl<ButtonControl>("buttonWest");
            buttonNorth = GetChildControl<ButtonControl>("buttonNorth");

            startButton = GetChildControl<ButtonControl>("startButton");
            selectButton = GetChildControl<ButtonControl>("selectButton");

            fret1 = GetChildControl<IntegerControl>("fret1");
            fret2 = GetChildControl<IntegerControl>("fret2");
            fret3 = GetChildControl<IntegerControl>("fret3");
            fret4 = GetChildControl<IntegerControl>("fret4");
            fret5 = GetChildControl<IntegerControl>("fret5");
            fret6 = GetChildControl<IntegerControl>("fret6");

            velocity1 = GetChildControl<AxisControl>("velocity1");
            velocity2 = GetChildControl<AxisControl>("velocity2");
            velocity3 = GetChildControl<AxisControl>("velocity3");
            velocity4 = GetChildControl<AxisControl>("velocity4");
            velocity5 = GetChildControl<AxisControl>("velocity5");
            velocity6 = GetChildControl<AxisControl>("velocity6");

            greenFret = GetChildControl<ButtonControl>("greenFret");
            redFret = GetChildControl<ButtonControl>("redFret");
            yellowFret = GetChildControl<ButtonControl>("yellowFret");
            blueFret = GetChildControl<ButtonControl>("blueFret");
            orangeFret = GetChildControl<ButtonControl>("orangeFret");

            tilt = GetChildControl<AxisControl>("tilt");
            whammy = GetChildControl<AxisControl>("whammy");
        }
    }
}
