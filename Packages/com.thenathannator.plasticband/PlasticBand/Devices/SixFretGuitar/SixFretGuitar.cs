using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 6-fret guitar controller.
    /// </summary>
    [InputControlLayout(displayName = "6-Fret Guitar")]
    public class SixFretGuitar : BaseDevice<SixFretGuitar>
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<SixFretGuitar>();
        }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        [InputControl(name = "black1", displayName = "Black 1", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl black1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        [InputControl(name = "black2", displayName = "Black 2", usages = new[] { "Back", "Cancel" })]
        public ButtonControl black2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        [InputControl(name = "black3", displayName = "Black 3")]
        public ButtonControl black3 { get; private set; }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        [InputControl(name = "white1", displayName = "White 1")]
        public ButtonControl white1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        [InputControl(name = "white2", displayName = "White 2")]
        public ButtonControl white2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        [InputControl(name = "white3", displayName = "White 3")]
        public ButtonControl white3 { get; private set; }

        /// <summary>
        /// The guitar's strum up input.
        /// On most models this is equivalent to the d-pad up input, but on some it may not be.
        /// </summary>
        [InputControl(name = "strumUp", displayName = "Strum Up")]
        public ButtonControl strumUp { get; private set; }

        /// <summary>
        /// The guitar's strum down input.
        /// On most models this is equivalent to the d-pad down input, but on some it may not be.
        /// </summary>
        [InputControl(name = "strumDown", displayName = "Strum Down")]
        public ButtonControl strumDown { get; private set; }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/down", displayName = "Down/Strum Down")]
        public DpadControl dpad { get; private set; }

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

        /// <summary>
        /// The Start button on the guitar.
        /// </summary>
        [InputControl(name = "startButton", displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the guitar.
        /// </summary>
        [InputControl(name = "selectButton", displayName = "Hero Power")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The GHTV button on the guitar.
        /// </summary>
        [InputControl(name = "ghtvButton", displayName = "GHTV Button")]
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
