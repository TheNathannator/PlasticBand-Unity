using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal interface IProGuitarState : IInputStateTypeInfo
    {
        bool south { get; set; }
        bool east { get; set; }
        bool west { get; set; }
        bool north { get; set; }

        bool dpadUp { get; set; }
        bool dpadDown { get; set; }
        bool dpadLeft { get; set; }
        bool dpadRight { get; set; }

        bool start { get; set; }
        bool select { get; set; }
        bool system { get; set; }

        bool green { get; set; }
        bool red { get; set; }
        bool yellow { get; set; }
        bool blue { get; set; }
        bool orange { get; set; }
        bool solo { get; set; }

        // Raw fret number values, for convenience in the state translation
        // since we copy the compression technique used
        ushort frets1 { get; }
        ushort frets2 { get; }

        // Separated fret number values, for convenience in the unit tests
        byte fret1 { get; set; }
        byte fret2 { get; set; }
        byte fret3 { get; set; }
        byte fret4 { get; set; }
        byte fret5 { get; set; }
        byte fret6 { get; set; }

        byte velocity1 { get; set; }
        byte velocity2 { get; set; }
        byte velocity3 { get; set; }
        byte velocity4 { get; set; }
        byte velocity5 { get; set; }
        byte velocity6 { get; set; }

        bool tilt { get; set; }

        bool digitalPedal { get; set; }
        // byte analogPedal { get; set; }
    }

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
        /// Whether or not the guitar's 1st string has been strummed.
        /// </summary>
        /// <remarks>
        /// Due to how this is calculated, the value does not persist beyond the end of the current frame.
        /// Do not depend on it being held for any period of time.
        /// </remarks>
        [InputControl(displayName = "String 1 Strum")]
        public ButtonControl strum1 { get; private set; }

        /// <summary>
        /// Whether or not the guitar's 2nd string has been strummed.
        /// </summary>
        /// <remarks>
        /// Due to how this is calculated, the value does not persist beyond the end of the current frame.
        /// Do not depend on it being held for any period of time.
        /// </remarks>
        [InputControl(displayName = "String 2 Strum")]
        public ButtonControl strum2 { get; private set; }

        /// <summary>
        /// Whether or not the guitar's 3rd string has been strummed.
        /// </summary>
        /// <remarks>
        /// Due to how this is calculated, the value does not persist beyond the end of the current frame.
        /// Do not depend on it being held for any period of time.
        /// </remarks>
        [InputControl(displayName = "String 3 Strum")]
        public ButtonControl strum3 { get; private set; }

        /// <summary>
        /// Whether or not the guitar's 4th string has been strummed.
        /// </summary>
        /// <remarks>
        /// Due to how this is calculated, the value does not persist beyond the end of the current frame.
        /// Do not depend on it being held for any period of time.
        /// </remarks>
        [InputControl(displayName = "String 4 Strum")]
        public ButtonControl strum4 { get; private set; }

        /// <summary>
        /// Whether or not the guitar's 5th string has been strummed.
        /// </summary>
        /// <remarks>
        /// Due to how this is calculated, the value does not persist beyond the end of the current frame.
        /// Do not depend on it being held for any period of time.
        /// </remarks>
        [InputControl(displayName = "String 5 Strum")]
        public ButtonControl strum5 { get; private set; }

        /// <summary>
        /// Whether or not the guitar's 6th string has been strummed.
        /// </summary>
        /// <remarks>
        /// Due to how this is calculated, the value does not persist beyond the end of the current frame.
        /// Do not depend on it being held for any period of time.
        /// </remarks>
        [InputControl(displayName = "String 6 Strum")]
        public ButtonControl strum6 { get; private set; }

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
        /// The emulated green solo fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Solo Green Fret")]
        public ButtonControl soloGreen { get; private set; }

        /// <summary>
        /// The emulated red solo fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Solo Red Fret")]
        public ButtonControl soloRed { get; private set; }

        /// <summary>
        /// The emulated yellow solo fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Solo Yellow Fret")]
        public ButtonControl soloYellow { get; private set; }

        /// <summary>
        /// The emulated blue solo fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Solo Blue Fret")]
        public ButtonControl soloBlue { get; private set; }

        /// <summary>
        /// The emulated orange solo fret input on the guitar.
        /// </summary>
        [InputControl(displayName = "Solo Orange Fret")]
        public ButtonControl soloOrange { get; private set; }

        /// <summary>
        /// The guitar's tilt orientation.
        /// </summary>
        /// <remarks>
        /// Not supported on Xbox 360 guitars unfortunately,
        /// as they do not report the necessary data through XInput alone.
        /// </remarks>
        [InputControl(displayName = "Tilt", noisy = true)]
        public AxisControl tilt { get; private set; }

        /// <summary>
        /// The digital pedal input on the guitar.
        /// </summary>
        /// <remarks>
        /// Not supported on Xbox 360 guitars unfortunately,
        /// as they do not report the necessary data through XInput alone.
        /// </remarks>
        [InputControl(displayName = "Digital Pedal")]
        public ButtonControl digitalPedal { get; private set; }

        /// <summary>
        /// The number of strings available on the guitar.
        /// </summary>
        public const int StringCount = 6;

        /// <summary>
        /// The number of emulated 5-fret frets available on the guitar.
        /// Provided for convenience, equivalent to <see cref="FiveFretGuitar.FretCount"/>.
        /// </summary>
        public const int EmulatedFretCount = FiveFretGuitar.FretCount;

        /// <summary>
        /// Retrieves a fret control by index.
        /// </summary>
        public IntegerControl GetStringFret(int index)
        {
            switch (index)
            {
                case 0: return fret1;
                case 1: return fret2;
                case 2: return fret3;
                case 3: return fret4;
                case 4: return fret5;
                case 5: return fret6;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(StringCount)} ({StringCount})!");
            }
        }

        /// <summary>
        /// Retrieves a strum control by index.
        /// </summary>
        public ButtonControl GetStringStrum(int index)
        {
            switch (index)
            {
                case 0: return strum1;
                case 1: return strum2;
                case 2: return strum3;
                case 3: return strum4;
                case 4: return strum5;
                case 5: return strum6;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(StringCount)} ({StringCount})!");
            }
        }

        /// <summary>
        /// Retrieves an emulated fret control by index.<br/>
        /// 0 = green, 4 = orange.
        /// </summary>
        public ButtonControl GetEmulatedFret(int index)
        {
            switch (index)
            {
                case 0: return greenFret;
                case 1: return redFret;
                case 2: return yellowFret;
                case 3: return blueFret;
                case 4: return orangeFret;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(EmulatedFretCount)} ({EmulatedFretCount})!");
            }
        }

        /// <summary>
        /// Retrieves an emulated fret control by enum value.
        /// </summary>
        public ButtonControl GetEmulatedFret(FiveFret fret)
        {
            switch (fret)
            {
                case FiveFret.Green: return greenFret;
                case FiveFret.Red: return redFret;
                case FiveFret.Yellow: return yellowFret;
                case FiveFret.Blue: return blueFret;
                case FiveFret.Orange: return orangeFret;
                default: throw new ArgumentException($"Invalid fret value {fret}!", nameof(fret));
            }
        }

        /// <summary>
        /// Retrieves an emulated solo fret control by index.<br/>
        /// 0 = green, 4 = orange.
        /// </summary>
        public ButtonControl GetEmulatedSoloFret(int index)
        {
            switch (index)
            {
                case 0: return soloGreen;
                case 1: return soloRed;
                case 2: return soloYellow;
                case 3: return soloBlue;
                case 4: return soloOrange;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(EmulatedFretCount)} ({EmulatedFretCount})!");
            }
        }

        /// <summary>
        /// Retrieves an emulated solo fret control by enum value.
        /// </summary>
        public ButtonControl GetEmulatedSoloFret(FiveFret fret)
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
        /// Retrives a bitmask of the current emulated fret states.
        /// </summary>
        public FiveFret GetEmulatedFretMask()
        {
            var mask = FiveFret.None;
            if (greenFret.isPressed) mask |= FiveFret.Green;
            if (redFret.isPressed) mask |= FiveFret.Red;
            if (yellowFret.isPressed) mask |= FiveFret.Yellow;
            if (blueFret.isPressed) mask |= FiveFret.Blue;
            if (orangeFret.isPressed) mask |= FiveFret.Orange;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the emulated fret states in the given state event.
        /// </summary>
        public FiveFret GetEmulatedFretMask(InputEventPtr eventPtr)
        {
            var mask = FiveFret.None;
            if (greenFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Green;
            if (redFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Red;
            if (yellowFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Yellow;
            if (blueFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Blue;
            if (orangeFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Orange;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the current emulated solo fret states.
        /// </summary>
        public FiveFret GetEmulatedSoloFretMask()
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
        /// Retrives a bitmask of the emulated solo fret states in the given state event.
        /// </summary>
        public FiveFret GetEmulatedSoloFretMask(InputEventPtr eventPtr)
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
        /// Retrives a bitmask of the current emulated fret states, excluding the solo frets.
        /// </summary>
        public FiveFret GetEmulatedFretMaskExcludingSolo()
        {
            var mask = FiveFret.None;
            if (greenFret.isPressed) mask |= FiveFret.Green;
            if (redFret.isPressed) mask |= FiveFret.Red;
            if (yellowFret.isPressed) mask |= FiveFret.Yellow;
            if (blueFret.isPressed) mask |= FiveFret.Blue;
            if (orangeFret.isPressed) mask |= FiveFret.Orange;
            return mask & ~GetEmulatedSoloFretMask();
        }

        /// <summary>
        /// Retrives a bitmask of the emulated fret states in the given state event, excluding the solo frets.
        /// </summary>
        public FiveFret GetEmulatedFretMaskExcludingSolo(InputEventPtr eventPtr)
        {
            var mask = FiveFret.None;
            if (greenFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Green;
            if (redFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Red;
            if (yellowFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Yellow;
            if (blueFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Blue;
            if (orangeFret.IsPressedInEvent(eventPtr)) mask |= FiveFret.Orange;
            return mask & ~GetEmulatedSoloFretMask(eventPtr);
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

            strum1 = GetChildControl<ButtonControl>(nameof(strum1));
            strum2 = GetChildControl<ButtonControl>(nameof(strum2));
            strum3 = GetChildControl<ButtonControl>(nameof(strum3));
            strum4 = GetChildControl<ButtonControl>(nameof(strum4));
            strum5 = GetChildControl<ButtonControl>(nameof(strum5));
            strum6 = GetChildControl<ButtonControl>(nameof(strum6));

            greenFret = GetChildControl<ButtonControl>(nameof(greenFret));
            redFret = GetChildControl<ButtonControl>(nameof(redFret));
            yellowFret = GetChildControl<ButtonControl>(nameof(yellowFret));
            blueFret = GetChildControl<ButtonControl>(nameof(blueFret));
            orangeFret = GetChildControl<ButtonControl>(nameof(orangeFret));

            soloGreen = GetChildControl<ButtonControl>(nameof(soloGreen));
            soloRed = GetChildControl<ButtonControl>(nameof(soloRed));
            soloYellow = GetChildControl<ButtonControl>(nameof(soloYellow));
            soloBlue = GetChildControl<ButtonControl>(nameof(soloBlue));
            soloOrange = GetChildControl<ButtonControl>(nameof(soloOrange));

            tilt = GetChildControl<AxisControl>(nameof(tilt));

            digitalPedal = GetChildControl<ButtonControl>(nameof(digitalPedal));
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
