using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 5-lane (Guitar Hero) drumkit controller.
    /// </summary>
    [InputControlLayout(displayName = "5-Lane Drumkit")]
    public class FiveLaneDrumkit : InputDevice
    {
        /// <summary>
        /// The current <see cref="FiveLaneDrumkit"/>.
        /// </summary>
        public static FiveLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="FiveLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<FiveLaneDrumkit> all => s_AllDevices;
        private static readonly List<FiveLaneDrumkit> s_AllDevices = new List<FiveLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="FiveLaneDrumkit"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FiveLaneDrumkit>();
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
        [InputControl(displayName = "Yellow Cymbal")]
        public ButtonControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Blue Pad")]
        public ButtonControl bluePad { get; private set; }

        /// <summary>
        /// The green pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Orange Cymbal")]
        public ButtonControl orangeCymbal { get; private set; }

        /// <summary>
        /// The yellow cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Green Pad", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenPad { get; private set; }

        /// <summary>
        /// The kick pedal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Kick")]
        public ButtonControl kick { get; private set; }

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
            yellowCymbal = GetChildControl<ButtonControl>(nameof(yellowCymbal));
            bluePad = GetChildControl<ButtonControl>(nameof(bluePad));
            orangeCymbal = GetChildControl<ButtonControl>(nameof(orangeCymbal));
            greenPad = GetChildControl<ButtonControl>(nameof(greenPad));

            kick = GetChildControl<ButtonControl>(nameof(kick));
        }

        /// <summary>
        /// Sets this device as the current <see cref="FiveLaneDrumkit"/>.
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
