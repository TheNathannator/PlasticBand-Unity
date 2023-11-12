using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Bitmask of possible fret values on a 6-fret guitar.
    /// </summary>
    [Flags]
    public enum SixFret
    {
        None = 0,
        Black1 = 0x01,
        Black2 = 0x02,
        Black3 = 0x04,
        White1 = 0x08,
        White2 = 0x10,
        White3 = 0x20,
    }

    internal interface ISixFretGuitarState : IInputStateTypeInfo
    {
        bool dpadUp { get; set; }
        bool dpadDown { get; set; }
        bool dpadLeft { get; set; }
        bool dpadRight { get; set; }

        bool start { get; set; }
        bool select { get; set; }
        bool ghtv { get; set; }
        bool system { get; set; }

        bool black1 { get; set; }
        bool black2 { get; set; }
        bool black3 { get; set; }
        bool white1 { get; set; }
        bool white2 { get; set; }
        bool white3 { get; set; }

        bool strumUp { get; set; }
        bool strumDown { get; set; }

        byte whammy { get; set; }
        sbyte tilt { get; set; }
    }

    /// <summary>
    /// A 6-fret guitar controller.
    /// </summary>
    [InputControlLayout(displayName = "Guitar Hero Live (6-Fret) Guitar")]
    public class SixFretGuitar : InputDevice
    {
        /// <summary>
        /// The current <see cref="SixFretGuitar"/>.
        /// </summary>
        public static SixFretGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SixFretGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SixFretGuitar> all => s_AllDevices;
        private static readonly List<SixFretGuitar> s_AllDevices = new List<SixFretGuitar>();

        internal static void Initialize()
        {
            InputSystem.RegisterLayout<SixFretGuitar>();
        }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Black 1", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl black1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Black 2", usages = new[] { "Back", "Cancel" })]
        public ButtonControl black2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Black 3")]
        public ButtonControl black3 { get; private set; }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "White 1")]
        public ButtonControl white1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "White 2")]
        public ButtonControl white2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "White 3")]
        public ButtonControl white3 { get; private set; }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        /// <remarks>
        /// D-pad up and down are also used for strum up/down.
        /// </remarks>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        [InputControl(name = "dpad/up", displayName = "Up/Strum Up", alias = "strumUp")]
        [InputControl(name = "dpad/down", displayName = "Down/Strum Down", alias = "strumDown")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The guitar's strum up input.
        /// Provided for convenience; this is equivalent to <c>dpad.up</c>.
        /// </summary>
        public ButtonControl strumUp => dpad.up;

        /// <summary>
        /// The guitar's strum down input.
        /// Provided for convenience; this is equivalent to <c>dpad.down</c>.
        /// </summary>
        public ButtonControl strumDown => dpad.down;

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
        [InputControl(displayName = "Hero Power")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The GHTV button on the guitar.
        /// </summary>
        [InputControl(displayName = "GHTV Button")]
        public ButtonControl ghtvButton { get; private set; }

        /// <summary>
        /// The number of frets available on the guitar.
        /// </summary>
        public const int FretCount = 6;

        /// <summary>
        /// Retrieves a fret control by index.<br/>
        /// 0 = black 1, 5 = white 3.
        /// </summary>
        public ButtonControl GetFret(int index)
        {
            switch (index)
            {
                case 0: return black1;
                case 1: return black2;
                case 2: return black3;
                case 3: return white1;
                case 4: return white2;
                case 5: return white3;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(FretCount)} ({FretCount})!");
            }
        }

        /// <summary>
        /// Retrieves a fret control by enum value.
        /// </summary>
        public ButtonControl GetFret(SixFret fret)
        {
            switch (fret)
            {
                case SixFret.Black1: return black1;
                case SixFret.Black2: return black2;
                case SixFret.Black3: return black3;
                case SixFret.White1: return white1;
                case SixFret.White2: return white2;
                case SixFret.White3: return white3;
                default: throw new ArgumentException($"Invalid fret value {fret}!", nameof(fret));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current fret states.
        /// </summary>
        public SixFret GetFretMask()
        {
            var mask = SixFret.None;
            if (black1.isPressed) mask |= SixFret.Black1;
            if (black2.isPressed) mask |= SixFret.Black2;
            if (black3.isPressed) mask |= SixFret.Black3;
            if (white1.isPressed) mask |= SixFret.White1;
            if (white2.isPressed) mask |= SixFret.White2;
            if (white3.isPressed) mask |= SixFret.White3;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the current fret states.
        /// </summary>
        public SixFret GetFretMask(InputEventPtr eventPtr)
        {
            var mask = SixFret.None;
            if (black1.IsPressedInEvent(eventPtr)) mask |= SixFret.Black1;
            if (black2.IsPressedInEvent(eventPtr)) mask |= SixFret.Black2;
            if (black3.IsPressedInEvent(eventPtr)) mask |= SixFret.Black3;
            if (white1.IsPressedInEvent(eventPtr)) mask |= SixFret.White1;
            if (white2.IsPressedInEvent(eventPtr)) mask |= SixFret.White2;
            if (white3.IsPressedInEvent(eventPtr)) mask |= SixFret.White3;
            return mask;
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            black1 = GetChildControl<ButtonControl>(nameof(black1));
            black2 = GetChildControl<ButtonControl>(nameof(black2));
            black3 = GetChildControl<ButtonControl>(nameof(black3));
            white1 = GetChildControl<ButtonControl>(nameof(white1));
            white2 = GetChildControl<ButtonControl>(nameof(white2));
            white3 = GetChildControl<ButtonControl>(nameof(white3));

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            tilt = GetChildControl<AxisControl>(nameof(tilt));
            whammy = GetChildControl<AxisControl>(nameof(whammy));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));
            ghtvButton = GetChildControl<ButtonControl>(nameof(ghtvButton));
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
