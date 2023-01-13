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
        [InputControl(name = "greenFret", displayName = "Green Fret", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenFret { get; private set; }

        /// <summary>
        /// The red fret on the guitar.
        /// </summary>
        [InputControl(name = "redFret", displayName = "Red Fret", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redFret { get; private set; }

        /// <summary>
        /// The yellow fret on the guitar.
        /// </summary>
        [InputControl(name = "yellowFret", displayName = "Yellow Fret")]
        public ButtonControl yellowFret { get; private set; }

        /// <summary>
        /// The blue fret on the guitar.
        /// </summary>
        [InputControl(name = "blueFret", displayName = "Blue Fret")]
        public ButtonControl blueFret { get; private set; }

        /// <summary>
        /// The orange fret on the guitar.
        /// </summary>
        [InputControl(name = "orangeFret", displayName = "Orange Fret")]
        public ButtonControl orangeFret { get; private set; }

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
        [InputControl(name = "selectButton", displayName = "Select")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
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
