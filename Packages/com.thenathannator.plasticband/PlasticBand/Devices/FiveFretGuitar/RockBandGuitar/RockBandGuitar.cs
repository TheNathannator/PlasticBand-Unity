using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band 5-fret guitar.
    /// Has some additional features that aren't available on all 5-fret guitars.
    /// </summary>
    [InputControlLayout(displayName = "Rock Band 5-Fret Guitar")]
    public class RockBandGuitar : FiveFretGuitar
    {
        /// <summary>
        /// The current <see cref="RockBandGuitar"/>.
        /// </summary>
        public static new RockBandGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="RockBandGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<RockBandGuitar> all => s_AllDevices;
        private static List<RockBandGuitar> s_AllDevices = new List<RockBandGuitar>();

        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<RockBandGuitar>();
        }

        /// <summary>
        /// The green solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(name = "soloGreen", displayName = "Solo Green Fret")]
        public ButtonControl soloGreen { get; private set; }

        /// <summary>
        /// The red solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(name = "soloRed", displayName = "Solo Red Fret")]
        public ButtonControl soloRed { get; private set; }

        /// <summary>
        /// The yellow solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(name = "soloYellow", displayName = "Solo Yellow Fret")]
        public ButtonControl soloYellow { get; private set; }

        /// <summary>
        /// The blue solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(name = "soloBlue", displayName = "Solo Blue Fret")]
        public ButtonControl soloBlue { get; private set; }

        /// <summary>
        /// The orange solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(name = "soloOrange", displayName = "Solo Orange Fret")]
        public ButtonControl soloOrange { get; private set; }

        /// <summary>
        /// The pickup switch on the guitar.
        /// </summary>
        [InputControl(name = "pickupSwitch", displayName = "Pickup Switch")]
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

        /// <summary>
        /// Sets this device as the current <see cref="RockBandGuitar"/>.
        /// </summary>
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
