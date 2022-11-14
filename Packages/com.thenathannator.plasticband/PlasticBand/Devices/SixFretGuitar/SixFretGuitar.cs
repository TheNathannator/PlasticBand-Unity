using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// Dummy struct used to give SixFretGuitars some controls in their layout.
    /// </summary>
    internal unsafe struct SixFretGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('6', 'F', 'G', 'T');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4, displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", bit = 0, displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/down", bit = 1, displayName = "Down/Strum Down")]
        [InputControl(name = "dpad/left", bit = 2, displayName = "Left")]
        [InputControl(name = "dpad/right", bit = 3, displayName = "Right")]
        [InputControl(name = "strumUp", layout = "Button", bit = 0, displayName = "Strum Up")]
        [InputControl(name = "strumDown", layout = "Button", bit = 1, displayName = "Strum Down")]

        [InputControl(name = "black1", layout = "Button", bit = 4, displayName = "Black 1", usages = new[] { "PrimaryAction", "Submit" })]
        [InputControl(name = "black2", layout = "Button", bit = 5, displayName = "Black 2", usages = new[] { "Back", "Cancel" })]
        [InputControl(name = "black3", layout = "Button", bit = 6, displayName = "Black 3")]
        [InputControl(name = "white1", layout = "Button", bit = 7, displayName = "White 1")]
        [InputControl(name = "white2", layout = "Button", bit = 8, displayName = "White 2")]
        [InputControl(name = "white3", layout = "Button", bit = 9, displayName = "White 3")]

        [InputControl(name = "startButton", layout = "Button", bit = 10, displayName = "Start", usage = "Menu")]
        [InputControl(name = "selectButton", layout = "Button", bit = 11, displayName = "Hero Power")]
        [InputControl(name = "ghtvButton", layout = "Button", bit = 12, displayName = "GHTV Button")]
        public ushort buttons;

        [InputControl(name = "whammy", layout = "Axis", displayName = "Whammy")]
        public byte whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true, displayName = "Tilt")]
        public byte tilt;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 6-fret guitar controller.
    /// </summary>
    [InputControlLayout(stateType = typeof(SixFretGuitarState), displayName = "6-Fret Guitar")]
    public class SixFretGuitar : BaseDevice<SixFretGuitar>
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<SixFretGuitar>();
        }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        public ButtonControl black1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        public ButtonControl black2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        public ButtonControl black3 { get; private set; }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        public ButtonControl white1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        public ButtonControl white2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        public ButtonControl white3 { get; private set; }

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

        /// <summary>
        /// The GHTV button on the guitar.
        /// </summary>
        public ButtonControl ghtvButton { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            black1 = GetChildControl<ButtonControl>("black1");
            black2 = GetChildControl<ButtonControl>("black2");
            black3 = GetChildControl<ButtonControl>("black3");
            white1 = GetChildControl<ButtonControl>("white1");
            white2 = GetChildControl<ButtonControl>("white2");
            white3 = GetChildControl<ButtonControl>("white3");

            strumUp = GetChildControl<ButtonControl>("strumUp");
            strumDown = GetChildControl<ButtonControl>("strumDown");

            dpad = GetChildControl<DpadControl>("dpad");

            tilt = GetChildControl<AxisControl>("tilt");
            whammy = GetChildControl<AxisControl>("whammy");

            startButton = GetChildControl<ButtonControl>("startButton");
            selectButton = GetChildControl<ButtonControl>("selectButton");
            ghtvButton = GetChildControl<ButtonControl>("ghtvButton");
        }
    }
}
