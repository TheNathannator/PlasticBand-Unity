using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// Dummy struct used to give RockBandGuitars some controls in their layout.
    /// </summary>
    internal unsafe struct RockBandGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('G', 'H', 'G', 'T');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0, alias = "strumUp")]
        [InputControl(name = "dpad/down", bit = 1, alias = "strumDown")]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "greenFret", layout = "Button", bit = 4)]
        [InputControl(name = "redFret", layout = "Button", bit = 5)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 6)]
        [InputControl(name = "blueFret", layout = "Button", bit = 7)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]

        [InputControl(name = "soloGreen", layout = "Button", bit = 9, displayName = "Solo Green Fret")]
        [InputControl(name = "soloRed", layout = "Button", bit = 10, displayName = "Solo Red Fret")]
        [InputControl(name = "soloYellow", layout = "Button", bit = 11, displayName = "Solo Yellow Fret")]
        [InputControl(name = "soloBlue", layout = "Button", bit = 12, displayName = "Solo Blue Fret")]
        [InputControl(name = "soloOrange", layout = "Button", bit = 13, displayName = "Solo Orange Fret")]

        [InputControl(name = "startButton", layout = "Button", bit = 14)]
        [InputControl(name = "selectButton", layout = "Button", bit = 15)]
        public ushort buttons;

        [InputControl(name = "pickupSwitch", layout = "Axis", displayName = "Pickup Switch")]
        public byte pickupSwitch;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;

        [InputControl(name = "whammy", layout = "Axis")]
        public short whammy;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band 5-fret guitar.
    /// Has some additional features that aren't available on all 5-fret guitars.
    /// </summary>
    [InputControlLayout(stateType = typeof(RockBandGuitarState), displayName = "Rock Band 5-Fret Guitar")]
    public class RockBandGuitar : FiveFretGuitar
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<RockBandGuitar>();
        }

        /// <summary>
        /// The green solo fret near the bottom of the guitar's neck.
        /// </summary>
        public ButtonControl soloGreen { get; private set; }

        /// <summary>
        /// The red solo fret near the bottom of the guitar's neck.
        /// </summary>
        public ButtonControl soloRed { get; private set; }

        /// <summary>
        /// The yellow solo fret near the bottom of the guitar's neck.
        /// </summary>
        public ButtonControl soloYellow { get; private set; }

        /// <summary>
        /// The blue solo fret near the bottom of the guitar's neck.
        /// </summary>
        public ButtonControl soloBlue { get; private set; }

        /// <summary>
        /// The orange solo fret near the bottom of the guitar's neck.
        /// </summary>
        public ButtonControl soloOrange { get; private set; }

        /// <summary>
        /// The pickup switch on the guitar.
        /// </summary>
        public AxisControl pickupSwitch { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            soloGreen = GetChildControl<ButtonControl>("soloGreen");
            soloRed = GetChildControl<ButtonControl>("soloRed");
            soloYellow = GetChildControl<ButtonControl>("soloYellow");
            soloBlue = GetChildControl<ButtonControl>("soloBlue");
            soloOrange = GetChildControl<ButtonControl>("soloOrange");

            pickupSwitch = GetChildControl<AxisControl>("pickupSwitch");
        }
    }
}
