using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

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
        private static readonly List<RockBandGuitar> s_AllDevices = new List<RockBandGuitar>();

        /// <summary>
        /// Registers <see cref="RockBandGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<RockBandGuitar>();
        }

        /// <summary>
        /// The green solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(displayName = "Solo Green Fret")]
        public ButtonControl soloGreen { get; private set; }

        /// <summary>
        /// The red solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(displayName = "Solo Red Fret")]
        public ButtonControl soloRed { get; private set; }

        /// <summary>
        /// The yellow solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(displayName = "Solo Yellow Fret")]
        public ButtonControl soloYellow { get; private set; }

        /// <summary>
        /// The blue solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(displayName = "Solo Blue Fret")]
        public ButtonControl soloBlue { get; private set; }

        /// <summary>
        /// The orange solo fret near the bottom of the guitar's neck.
        /// </summary>
        [InputControl(displayName = "Solo Orange Fret")]
        public ButtonControl soloOrange { get; private set; }

        /// <summary>
        /// The pickup switch on the guitar.
        /// </summary>
        [InputControl(displayName = "Pickup Switch")]
        public AxisControl pickupSwitch { get; private set; }

        /// <summary>
        /// The number of frets available on the guitar.
        /// </summary>
        public const int SoloFretCount = 5;

        /// <summary>
        /// Retrieves a solo fret control by index.<br/>
        /// 0 = green, 4 = orange.
        /// </summary>
        public ButtonControl GetSoloFret(int index)
        {
            switch (index)
            {
                case 0: return soloGreen;
                case 1: return soloRed;
                case 2: return soloYellow;
                case 3: return soloBlue;
                case 4: return soloOrange;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Retrieves a solo fret control by enum value.
        /// </summary>
        public ButtonControl GetSoloFret(FiveFret fret)
        {
            switch (fret)
            {
                case FiveFret.Green: return soloGreen;
                case FiveFret.Red: return soloRed;
                case FiveFret.Yellow: return soloYellow;
                case FiveFret.Blue: return soloBlue;
                case FiveFret.Orange: return soloOrange;
                default: throw new ArgumentException($"Could not determine the solo fret to retrieve! Value: '{fret}'", nameof(fret));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current solo fret states.
        /// </summary>
        public FiveFret GetSoloFretMask()
        {
            var mask = FiveFret.None;
            if (soloGreen.isPressed) mask |= FiveFret.Green;
            if (soloRed.isPressed) mask |= FiveFret.Red;
            if (soloYellow.isPressed) mask |= FiveFret.Yellow;
            if (soloBlue.isPressed) mask |= FiveFret.Blue;
            if (soloOrange.isPressed) mask |= FiveFret.Orange;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the solo fret states in the given state event.
        /// </summary>
        public FiveFret GetSoloFretMask(InputEventPtr eventPtr)
        {
            var mask = FiveFret.None;
            if (soloGreen.IsPressedInEvent(eventPtr)) mask |= FiveFret.Green;
            if (soloRed.IsPressedInEvent(eventPtr)) mask |= FiveFret.Red;
            if (soloYellow.IsPressedInEvent(eventPtr)) mask |= FiveFret.Yellow;
            if (soloBlue.IsPressedInEvent(eventPtr)) mask |= FiveFret.Blue;
            if (soloOrange.IsPressedInEvent(eventPtr)) mask |= FiveFret.Orange;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the current fret states, excluding the solo frets.
        /// </summary>
        public FiveFret GetFretMaskExcludingSolo()
        {
            var mask = FiveFret.None;
            if (greenFret.isPressed) mask |= FiveFret.Green;
            if (redFret.isPressed) mask |= FiveFret.Red;
            if (yellowFret.isPressed) mask |= FiveFret.Yellow;
            if (blueFret.isPressed) mask |= FiveFret.Blue;
            if (orangeFret.isPressed) mask |= FiveFret.Orange;
            return mask & ~GetSoloFretMask();
        }

        /// <summary>
        /// Retrives a bitmask of the fret states in the given state event, excluding the solo frets.
        /// </summary>
        public FiveFret GetFretMaskExcludingSolo(InputEventPtr eventPtr)
        {
            var mask = FiveFret.None;
            if (greenFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Green;
            if (redFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Red;
            if (yellowFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Yellow;
            if (blueFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Blue;
            if (orangeFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Orange;
            return mask & ~GetSoloFretMask(eventPtr);
        }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            soloGreen = GetChildControl<ButtonControl>(nameof(soloGreen));
            soloRed = GetChildControl<ButtonControl>(nameof(soloRed));
            soloYellow = GetChildControl<ButtonControl>(nameof(soloYellow));
            soloBlue = GetChildControl<ButtonControl>(nameof(soloBlue));
            soloOrange = GetChildControl<ButtonControl>(nameof(soloOrange));

            pickupSwitch = GetChildControl<AxisControl>(nameof(pickupSwitch));
        }

        /// <summary>
        /// Sets this device as the current <see cref="RockBandGuitar"/>.
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
