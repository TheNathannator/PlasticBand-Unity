using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// Dummy struct used to give FiveFretGuitars some controls in their layout.
    /// </summary>
    internal unsafe struct FiveFretGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('5', 'F', 'G', 'T');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4, displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", bit = 0, displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/down", bit = 1, displayName = "Down/Strum Down")]
        [InputControl(name = "dpad/left", bit = 2, displayName = "Left")]
        [InputControl(name = "dpad/right", bit = 3, displayName = "Right")]
        [InputControl(name = "strumUp", layout = "Button", bit = 0, displayName = "Strum Up")]
        [InputControl(name = "strumDown", layout = "Button", bit = 1, displayName = "Strum Down")]

        [InputControl(name = "greenFret", layout = "Button", bit = 4, displayName = "Green Fret", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "redFret", layout = "Button", bit = 5, displayName = "Red Fret", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "yellowFret", layout = "Button", bit = 6, displayName = "Yellow Fret")]
        [InputControl(name = "blueFret", layout = "Button", bit = 7, displayName = "Blue Fret")]
        [InputControl(name = "orangeFret", layout = "Button", bit = 8, displayName = "Orange Fret")]

        [InputControl(name = "startButton", layout = "Button", bit = 9, displayName = "Start", usage = "Menu")]
        [InputControl(name = "selectButton", layout = "Button", bit = 10, displayName = "Select")]
        public ushort buttons;

        [InputControl(name = "tilt", layout = "Axis", noisy = true, displayName = "Tilt")]
        public byte tilt;

        [InputControl(name = "whammy", layout = "Axis", displayName = "Whammy")]
        public byte whammy;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 5-fret guitar controller.
    /// </summary>
    [InputControlLayout(stateType = typeof(FiveFretGuitarState), displayName = "5-Fret Guitar")]
    public class FiveFretGuitar : BaseDevice<FiveFretGuitar>
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FiveFretGuitar>();
        }

        /// <summary>
        /// The green fret on the guitar.
        /// </summary>
        public ButtonControl greenFret { get; private set; }

        /// <summary>
        /// The red fret on the guitar.
        /// </summary>
        public ButtonControl redFret { get; private set; }

        /// <summary>
        /// The yellow fret on the guitar.
        /// </summary>
        public ButtonControl yellowFret { get; private set; }

        /// <summary>
        /// The blue fret on the guitar.
        /// </summary>
        public ButtonControl blueFret { get; private set; }

        /// <summary>
        /// The orange fret on the guitar.
        /// </summary>
        public ButtonControl orangeFret { get; private set; }

        /// <summary>
        /// The guitar's strum up input.
        /// On most models this is equivalent to the d-pad up input, but on some it may not be.
        /// </summary>
        public ButtonControl strumUp { get; private set; }

        /// <summary>
        /// The guitar's strum down input.
        /// On most models this is equivalent to the d-pad down input, but on some it may not be.
        /// </summary>
        public ButtonControl strumDown { get; private set; }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The guitar's tilt orientation.
        /// </summary>
        public AxisControl tilt { get; private set; }

        /// <summary>
        /// The guitar's whammy bar.
        /// </summary>
        public AxisControl whammy { get; private set; }

        /// <summary>
        /// The Start button on the guitar.
        /// </summary>
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the guitar.
        /// </summary>
        public ButtonControl selectButton { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            greenFret = GetChildControl<ButtonControl>("greenFret");
            redFret = GetChildControl<ButtonControl>("redFret");
            yellowFret = GetChildControl<ButtonControl>("yellowFret");
            blueFret = GetChildControl<ButtonControl>("blueFret");
            orangeFret = GetChildControl<ButtonControl>("orangeFret");

            strumUp = GetChildControl<ButtonControl>("strumUp");
            strumDown = GetChildControl<ButtonControl>("strumDown");

            dpad = GetChildControl<DpadControl>("dpad");

            tilt = GetChildControl<AxisControl>("tilt");
            whammy = GetChildControl<AxisControl>("whammy");

            startButton = GetChildControl<ButtonControl>("startButton");
            selectButton = GetChildControl<ButtonControl>("selectButton");
        }
    }
}
