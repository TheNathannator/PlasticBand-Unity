using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 4-lane (Rock Band) drumkit controller.
    /// </summary>
    [InputControlLayout(displayName = "4-Lane Drumkit")]
    public class FourLaneDrumkit : BaseDevice<FourLaneDrumkit>
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FourLaneDrumkit>();
        }

        /// <summary>
        /// The drumkit's d-pad.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The bottom face button on the drumkit.
        /// </summary>
        [InputControl(name = "buttonSouth", displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button on the drumkit.
        /// </summary>
        [InputControl(name = "buttonEast", displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button on the drumkit.
        /// </summary>
        [InputControl(name = "buttonWest", displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button on the drumkit.
        /// </summary>
        [InputControl(name = "buttonNorth", displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The Start button on the drumkit.
        /// </summary>
        [InputControl(name = "startButton", displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the drumkit.
        /// </summary>
        [InputControl(name = "selectButton", displayName = "Back")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The red pad on the drumkit.
        /// </summary>
        [InputControl(name = "redPad", displayName = "Red Pad")]
        public ButtonControl redPad { get; private set; }

        /// <summary>
        /// The yellow pad on the drumkit.
        /// </summary>
        [InputControl(name = "yellowPad", displayName = "Yellow Pad")]
        public ButtonControl yellowPad { get; private set; }

        /// <summary>
        /// The blue pad on the drumkit.
        /// </summary>
        [InputControl(name = "bluePad", displayName = "Blue Pad")]
        public ButtonControl bluePad { get; private set; }

        /// <summary>
        /// The green pad on the drumkit.
        /// </summary>
        [InputControl(name = "greenPad", displayName = "Green Pad")]
        public ButtonControl greenPad { get; private set; }

        /// <summary>
        /// The yellow cymbal on the drumkit.
        /// </summary>
        [InputControl(name = "yellowCymbal", displayName = "Yellow Cymbal")]
        public ButtonControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue cymbal on the drumkit.
        /// </summary>
        [InputControl(name = "blueCymbal", displayName = "Blue Cymbal")]
        public ButtonControl blueCymbal { get; private set; }

        /// <summary>
        /// The green cymbal on the drumkit.
        /// </summary>
        [InputControl(name = "greenCymbal", displayName = "Green Cymbal")]
        public ButtonControl greenCymbal { get; private set; }

        /// <summary>
        /// The first of two kick pedals on the drumkit.
        /// </summary>
        [InputControl(name = "kick1", displayName = "Kick 1")]
        public ButtonControl kick1 { get; private set; }

        /// <summary>
        /// The second of two kick pedals on the drumkit.
        /// </summary>
        [InputControl(name = "kick2", displayName = "Kick 2")]
        public ButtonControl kick2 { get; private set; }

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

            redPad = GetChildControl<ButtonControl>("redPad");
            yellowPad = GetChildControl<ButtonControl>("yellowPad");
            bluePad = GetChildControl<ButtonControl>("bluePad");
            greenPad = GetChildControl<ButtonControl>("greenPad");

            yellowCymbal = GetChildControl<ButtonControl>("yellowCymbal");
            blueCymbal = GetChildControl<ButtonControl>("blueCymbal");
            greenCymbal = GetChildControl<ButtonControl>("greenCymbal");

            kick1 = GetChildControl<ButtonControl>("kick1");
            kick2 = GetChildControl<ButtonControl>("kick2");
        }
    }
}
