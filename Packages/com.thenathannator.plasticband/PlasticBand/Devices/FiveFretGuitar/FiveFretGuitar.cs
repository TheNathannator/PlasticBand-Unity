using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 5-fret guitar controller.
    /// </summary>
    [InputControlLayout(displayName = "5-Fret Guitar")]
    public class FiveFretGuitar : InputDevice
    {
        /// <summary>
        /// The current <see cref="FiveFretGuitar"/>.
        /// </summary>
        public static FiveFretGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="FiveFretGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<FiveFretGuitar> all => s_AllDevices;
        private static readonly List<FiveFretGuitar> s_AllDevices = new List<FiveFretGuitar>();

        /// <summary>
        /// Registers <see cref="FiveFretGuitar"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FiveFretGuitar>();
        }

        /// <summary>
        /// The green fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Green Fret", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenFret { get; private set; }

        /// <summary>
        /// The red fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Red Fret", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redFret { get; private set; }

        /// <summary>
        /// The yellow fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Yellow Fret")]
        public ButtonControl yellowFret { get; private set; }

        /// <summary>
        /// The blue fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Blue Fret")]
        public ButtonControl blueFret { get; private set; }

        /// <summary>
        /// The orange fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Orange Fret")]
        public ButtonControl orangeFret { get; private set; }

        /// <summary>
        /// The guitar's strum up input.
        /// On most models this is equivalent to the d-pad up input, but on some it may not be.
        /// </summary>
        [InputControl(displayName = "Strum Up")]
        public ButtonControl strumUp { get; private set; }

        /// <summary>
        /// The guitar's strum down input.
        /// On most models this is equivalent to the d-pad down input, but on some it may not be.
        /// </summary>
        [InputControl(displayName = "Strum Down")]
        public ButtonControl strumDown { get; private set; }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/down", displayName = "Down/Strum Down")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The guitar's tilt orientation.
        /// </summary>
        [InputControl(displayName = "Tilt", noisy = true)]
        public AxisControl tilt { get; private set; }

        /// <summary>
        /// The guitar's whammy bar.
        /// </summary>
        [InputControl(displayName = "Whammy")]
        public AxisControl whammy { get; private set; }

        /// <summary>
        /// The Start button on the guitar.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the guitar.
        /// </summary>
        [InputControl(displayName = "Select")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            greenFret = GetChildControl<ButtonControl>(nameof(greenFret));
            redFret = GetChildControl<ButtonControl>(nameof(redFret));
            yellowFret = GetChildControl<ButtonControl>(nameof(yellowFret));
            blueFret = GetChildControl<ButtonControl>(nameof(blueFret));
            orangeFret = GetChildControl<ButtonControl>(nameof(orangeFret));

            strumUp = GetChildControl<ButtonControl>(nameof(strumUp));
            strumDown = GetChildControl<ButtonControl>(nameof(strumDown));

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            tilt = GetChildControl<AxisControl>(nameof(tilt));
            whammy = GetChildControl<AxisControl>(nameof(whammy));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));
        }

        /// <summary>
        /// Sets this device as the current <see cref="FiveFretGuitar"/>.
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
