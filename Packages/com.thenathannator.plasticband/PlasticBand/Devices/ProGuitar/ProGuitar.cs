using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

// TODO: Solo GRYBO frets
// Not implemented right now because:
// 1. it's not known where the solo fret flag is for Xbox 360 Pro guitars
// 2. it's gonna be a pain to put them together

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band 3 Pro Guitar controller.
    /// </summary>
    [InputControlLayout(displayName = "Rock Band Pro Guitar")]
    public class ProGuitar : InputDevice
    {
        /// <summary>
        /// The current <see cref="ProGuitar"/>.
        /// </summary>
        public static ProGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="ProGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<ProGuitar> all => s_AllDevices;
        private static readonly List<ProGuitar> s_AllDevices = new List<ProGuitar>();

        internal static void Initialize()
        {
            InputSystem.RegisterLayout<ProGuitar>();
        }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The bottom face button on the guitar.
        /// </summary>
        [InputControl(displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button on the guitar.
        /// </summary>
        [InputControl(displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button on the guitar.
        /// </summary>
        [InputControl(displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button on the guitar.
        /// </summary>
        [InputControl(displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

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
        /// The held fret of the guitar's 1st string.
        /// </summary>
        [InputControl(displayName = "String 1 Fret")]
        public IntegerControl fret1 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 2nd string.
        /// </summary>
        [InputControl(displayName = "String 2 Fret")]
        public IntegerControl fret2 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 3rd string.
        /// </summary>
        [InputControl(displayName = "String 3 Fret")]
        public IntegerControl fret3 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 4th string.
        /// </summary>
        [InputControl(displayName = "String 4 Fret")]
        public IntegerControl fret4 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 5th string.
        /// </summary>
        [InputControl(displayName = "String 5 Fret")]
        public IntegerControl fret5 { get; private set; }

        /// <summary>
        /// The held fret of the guitar's 6th string.
        /// </summary>
        [InputControl(displayName = "String 6 Fret")]
        public IntegerControl fret6 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 1st string.
        /// </summary>
        [InputControl(displayName = "String 1 Velocity")]
        public AxisControl velocity1 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 2nd string.
        /// </summary>
        [InputControl(displayName = "String 2 Velocity")]
        public AxisControl velocity2 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 3rd string.
        /// </summary>
        [InputControl(displayName = "String 3 Velocity")]
        public AxisControl velocity3 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 4th string.
        /// </summary>
        [InputControl(displayName = "String 4 Velocity")]
        public AxisControl velocity4 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 5th string.
        /// </summary>
        [InputControl(displayName = "String 5 Velocity")]
        public AxisControl velocity5 { get; private set; }

        /// <summary>
        /// The velocity of the guitar's 6th string.
        /// </summary>
        [InputControl(displayName = "String 6 Velocity")]
        public AxisControl velocity6 { get; private set; }

        /// <summary>
        /// The emulated green fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Green Fret", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenFret { get; private set; }

        /// <summary>
        /// The emulated red fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Red Fret", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redFret { get; private set; }

        /// <summary>
        /// The emulated yellow fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Yellow Fret")]
        public ButtonControl yellowFret { get; private set; }

        /// <summary>
        /// The emulated blue fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Blue Fret")]
        public ButtonControl blueFret { get; private set; }

        /// <summary>
        /// The emulated orange fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Orange Fret")]
        public ButtonControl orangeFret { get; private set; }

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
        /// The number of strings available on the guitar.
        /// </summary>
        public const int StringCount = 6;

        /// <summary>
        /// Retrieves a fret control by index.
        /// </summary>
        public IntegerControl GetFret(int index)
        {
            switch (index)
            {
                case 0: return fret1;
                case 1: return fret2;
                case 2: return fret3;
                case 3: return fret4;
                case 4: return fret5;
                case 5: return fret6;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Retrieves a velocity control by index.
        /// </summary>
        public AxisControl GetVelocity(int index)
        {
            switch (index)
            {
                case 0: return velocity1;
                case 1: return velocity2;
                case 2: return velocity3;
                case 3: return velocity4;
                case 4: return velocity5;
                case 5: return velocity6;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            buttonSouth = GetChildControl<ButtonControl>(nameof(buttonSouth));
            buttonEast = GetChildControl<ButtonControl>(nameof(buttonEast));
            buttonWest = GetChildControl<ButtonControl>(nameof(buttonWest));
            buttonNorth = GetChildControl<ButtonControl>(nameof(buttonNorth));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));

            fret1 = GetChildControl<IntegerControl>(nameof(fret1));
            fret2 = GetChildControl<IntegerControl>(nameof(fret2));
            fret3 = GetChildControl<IntegerControl>(nameof(fret3));
            fret4 = GetChildControl<IntegerControl>(nameof(fret4));
            fret5 = GetChildControl<IntegerControl>(nameof(fret5));
            fret6 = GetChildControl<IntegerControl>(nameof(fret6));

            velocity1 = GetChildControl<AxisControl>(nameof(velocity1));
            velocity2 = GetChildControl<AxisControl>(nameof(velocity2));
            velocity3 = GetChildControl<AxisControl>(nameof(velocity3));
            velocity4 = GetChildControl<AxisControl>(nameof(velocity4));
            velocity5 = GetChildControl<AxisControl>(nameof(velocity5));
            velocity6 = GetChildControl<AxisControl>(nameof(velocity6));

            greenFret = GetChildControl<ButtonControl>(nameof(greenFret));
            redFret = GetChildControl<ButtonControl>(nameof(redFret));
            yellowFret = GetChildControl<ButtonControl>(nameof(yellowFret));
            blueFret = GetChildControl<ButtonControl>(nameof(blueFret));
            orangeFret = GetChildControl<ButtonControl>(nameof(orangeFret));

            tilt = GetChildControl<AxisControl>(nameof(tilt));
            whammy = GetChildControl<AxisControl>(nameof(whammy));
        }

        /// <inheritdoc/>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }
    }
}
