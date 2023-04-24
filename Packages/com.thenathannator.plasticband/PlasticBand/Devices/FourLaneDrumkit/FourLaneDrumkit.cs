using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 4-lane (Rock Band) drumkit controller.
    /// </summary>
    [InputControlLayout(displayName = "4-Lane Drumkit")]
    public class FourLaneDrumkit : InputDevice
    {
        /// <summary>
        /// The current <see cref="FourLaneDrumkit"/>.
        /// </summary>
        public static FourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="FourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<FourLaneDrumkit> all => s_AllDevices;
        private static readonly List<FourLaneDrumkit> s_AllDevices = new List<FourLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="FourLaneDrumkit"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FourLaneDrumkit>();
        }

        /// <summary>
        /// The drumkit's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The Start button on the drumkit.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the drumkit.
        /// </summary>
        [InputControl(displayName = "Back")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The red pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Red Pad", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redPad { get; private set; }

        /// <summary>
        /// The yellow pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Yellow Pad")]
        public ButtonControl yellowPad { get; private set; }

        /// <summary>
        /// The blue pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Blue Pad")]
        public ButtonControl bluePad { get; private set; }

        /// <summary>
        /// The green pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Green Pad", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenPad { get; private set; }

        /// <summary>
        /// The yellow cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Yellow Cymbal")]
        public ButtonControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Blue Cymbal")]
        public ButtonControl blueCymbal { get; private set; }

        /// <summary>
        /// The green cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Green Cymbal", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenCymbal { get; private set; }

        /// <summary>
        /// The first of two kick pedals on the drumkit.
        /// </summary>
        [InputControl(displayName = "Kick 1")]
        public ButtonControl kick1 { get; private set; }

        /// <summary>
        /// The second of two kick pedals on the drumkit.
        /// </summary>
        [InputControl(displayName = "Kick 2")]
        public ButtonControl kick2 { get; private set; }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));

            redPad = GetChildControl<ButtonControl>(nameof(redPad));
            yellowPad = GetChildControl<ButtonControl>(nameof(yellowPad));
            bluePad = GetChildControl<ButtonControl>(nameof(bluePad));
            greenPad = GetChildControl<ButtonControl>(nameof(greenPad));

            yellowCymbal = GetChildControl<ButtonControl>(nameof(yellowCymbal));
            blueCymbal = GetChildControl<ButtonControl>(nameof(blueCymbal));
            greenCymbal = GetChildControl<ButtonControl>(nameof(greenCymbal));

            kick1 = GetChildControl<ButtonControl>(nameof(kick1));
            kick2 = GetChildControl<ButtonControl>(nameof(kick2));
        }

        /// <summary>
        /// Sets this device as the current <see cref="FourLaneDrumkit"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        /// <summary>
        /// Processes when this device is added to the system.
        /// </summary>
        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        /// <summary>
        /// Processes when this device is removed from the system.
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }
    }
}
