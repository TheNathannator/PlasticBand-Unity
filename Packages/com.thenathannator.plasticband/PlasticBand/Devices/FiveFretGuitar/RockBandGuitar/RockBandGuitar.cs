using System;
using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal interface IRockBandGuitarState_Base : IFiveFretGuitarState
    {
        int pickupSwitch { get; set; }
    }

    internal interface IRockBandGuitarState_Flags : IRockBandGuitarState_Base
    {
        bool solo { get; set; }
    }

    internal interface IRockBandGuitarState_Distinct : IRockBandGuitarState_Base
    {
        bool soloGreen { get; set; }
        bool soloRed { get; set; }
        bool soloYellow { get; set; }
        bool soloBlue { get; set; }
        bool soloOrange { get; set; }
    }

    internal static class RockBandGuitarState
    {
        internal const int kNullValue = PS3DeviceState.StickCenter;

        // Normally this value would be byte.MaxValue / 5f, the + 1 is to
        // avoid having to call Math.Clamp at the expense of a small amount of range
        internal const float kNotchSize = (byte.MaxValue + 1) / 5f;

        public static byte EnsureNotNull(byte value)
        {
            return value == kNullValue ? (byte)(kNullValue + 1) : value;
        }

        /// <summary>
        /// Calculates the notch for the given pickup switch value, ranging from 0 to 4 inclusively.
        /// </summary>
        public static int GetPickupSwitchNotch(byte value)
        {
            return (int)(value / kNotchSize);
        }

        /// <summary>
        /// Calculates the notch for the given pickup switch value, ranging from 0 to 4 inclusively.
        /// This variant handles the null value on PS3/Wii RB guitars and returns -1 if it's passed in.
        /// </summary>
        public static int GetPickupSwitchNotch_NullState(byte value)
        {
            if (value == kNullValue)
                return -1;

            return GetPickupSwitchNotch(value);
        }

        /// <summary>
        /// Calculates the value to set for the given pickup switch notch.
        /// This excludes the null value on PS3/Wii RB guitars.
        /// </summary>
        public static byte SetPickupSwitchNotch(int notch)
        {
            // Multiply by size to get the notch range's edge, then add half to get the range center
            byte value = (byte)((notch * kNotchSize) + (kNotchSize / 2));

            // PS3/Wii Rock Band guitars reset the pickup switch to a neutral value
            // after some time of no movement, need to avoid setting that value
            if (value == kNullValue)
                value--;

            return value;
        }

        /// <summary>
        /// Calculates the value to set for the given pickup switch notch.
        /// This variant handles the null notch state and returns the null value for PS3/Wii RB guitars if it's passed in.
        /// </summary>
        public static byte SetPickupSwitchNotch_NullState(int notch)
        {
            if (notch < 0)
                return kNullValue;

            return SetPickupSwitchNotch(notch);
        }
    }

    /// <summary>
    /// A Rock Band 5-fret guitar.
    /// Has some additional features that aren't available on all 5-fret guitars,
    /// such as a second set of "solo" frets and a pickup switch.
    /// </summary>
    /// <remarks>
    /// To better facilitate their original use, and to prevent duplicate control issues,
    /// the state of the solo frets are *not* mirrored onto the normal frets.
    /// If the normal and solo frets should have the same function, they must be read together.
    /// <br/>
    /// For guitars that report the solo frets fully independently, this behavior allows the frets to be used as such.
    /// For guitars that use a flag strategy to report solo frets (i.e. five color flags and a sixth "solo fret pressed" flag),
    /// the solo flag takes precedence, which means pressing a solo fret will make all normal frets also behave as solo frets.
    /// </remarks>
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
        public IntegerControl pickupSwitch { get; private set; }

        /// <summary>
        /// The number of notches on the pickup switch.
        /// </summary>
        public const int PickupNotchCount = 5;

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
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(FretCount)} ({FretCount})!");
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
                default: throw new ArgumentException($"Invalid fret value {fret}!", nameof(fret));
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

        protected override void FinishSetup()
        {
            base.FinishSetup();

            soloGreen = GetChildControl<ButtonControl>(nameof(soloGreen));
            soloRed = GetChildControl<ButtonControl>(nameof(soloRed));
            soloYellow = GetChildControl<ButtonControl>(nameof(soloYellow));
            soloBlue = GetChildControl<ButtonControl>(nameof(soloBlue));
            soloOrange = GetChildControl<ButtonControl>(nameof(soloOrange));

            pickupSwitch = GetChildControl<IntegerControl>(nameof(pickupSwitch));
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
