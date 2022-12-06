using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 5-lane (Guitar Hero) drumkit controller.
    /// </summary>
    [InputControlLayout(displayName = "5-Lane Drumkit")]
    public class FiveLaneDrumkit : BaseDevice<FiveLaneDrumkit>
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FiveLaneDrumkit>();
        }

        /// <summary>
        /// The drumkit's d-pad.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

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
        [InputControl(name = "redPad", displayName = "Red Pad", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redPad { get; private set; }

        /// <summary>
        /// The yellow pad on the drumkit.
        /// </summary>
        [InputControl(name = "yellowCymbal", displayName = "Yellow Cymbal")]
        public ButtonControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue pad on the drumkit.
        /// </summary>
        [InputControl(name = "bluePad", displayName = "Blue Pad")]
        public ButtonControl bluePad { get; private set; }

        /// <summary>
        /// The green pad on the drumkit.
        /// </summary>
        [InputControl(name = "orangeCymbal", displayName = "Orange Cymbal")]
        public ButtonControl orangeCymbal { get; private set; }

        /// <summary>
        /// The yellow cymbal on the drumkit.
        /// </summary>
        [InputControl(name = "greenPad", displayName = "Green Pad", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenPad { get; private set; }

        /// <summary>
        /// The kick pedal on the drumkit.
        /// </summary>
        [InputControl(name = "kick", displayName = "Kick")]
        public ButtonControl kick { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>("dpad");

            startButton = GetChildControl<ButtonControl>("startButton");
            selectButton = GetChildControl<ButtonControl>("selectButton");

            redPad = GetChildControl<ButtonControl>("redPad");
            yellowCymbal = GetChildControl<ButtonControl>("yellowCymbal");
            bluePad = GetChildControl<ButtonControl>("bluePad");
            orangeCymbal = GetChildControl<ButtonControl>("orangeCymbal");
            greenPad = GetChildControl<ButtonControl>("greenPad");

            kick = GetChildControl<ButtonControl>("kick");
        }
    }
}
