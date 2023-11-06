using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Guitar Hero 5-fret guitar.
    /// Has some additional features that aren't available on all 5-fret guitars.
    /// </summary>
    [InputControlLayout(displayName = "Guitar Hero 5-Fret Guitar")]
    public class GuitarHeroGuitar : FiveFretGuitar
    {
        /// <summary>
        /// The current <see cref="GuitarHeroGuitar"/>.
        /// </summary>
        public static new GuitarHeroGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="GuitarHeroGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<GuitarHeroGuitar> all => s_AllDevices;
        private static readonly List<GuitarHeroGuitar> s_AllDevices = new List<GuitarHeroGuitar>();

        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<GuitarHeroGuitar>();
        }

        /// <summary>
        /// The Star Power pedal port on the bottom of the guitar.
        /// </summary>
        [InputControl(displayName = "Star Power Pedal")]
        public ButtonControl spPedal { get; private set; }

        /// <summary>
        /// The green segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(displayName = "Touch/Slider Bar Green")]
        public ButtonControl touchGreen { get; private set; }

        /// <summary>
        /// The red segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(displayName = "Touch/Slider Bar Red")]
        public ButtonControl touchRed { get; private set; }

        /// <summary>
        /// The yellow segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(displayName = "Touch/Slider Bar Yellow")]
        public ButtonControl touchYellow { get; private set; }

        /// <summary>
        /// The blue segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(displayName = "Touch/Slider Bar Blue")]
        public ButtonControl touchBlue { get; private set; }

        /// <summary>
        /// The orange segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(displayName = "Touch/Slider Bar Orange")]
        public ButtonControl touchOrange { get; private set; }

        /// <summary>
        /// The X-axis of the guitar's accelerometer; equivalent to regular tilt.
        /// </summary>
        /// <remarks>
        /// Positive is when the guitar is pointed up, negative is when the guitar is pointed down.
        /// </remarks>
        [InputControl(name = nameof(tilt), displayName = "Tilt/Accelerometer X", noisy = true, alias = "accelX")]
        public AxisControl accelX { get; private set; }

        /// <summary>
        /// The Y-axis of the guitar's accelerometer.
        /// </summary>
        /// <remarks>
        /// Positive is when the guitar is face down, negative is when the guitar is face up.
        /// </remarks>
        [InputControl(displayName = "Accelerometer Y", noisy = true)]
        public AxisControl accelY { get; private set; }

        /// <summary>
        /// The Z-axis of the guitar's accelerometer.
        /// </summary>
        /// <remarks>
        /// Positive is when the guitar is pointed right, negative is when the guitar is pointed left.
        /// </remarks>
        [InputControl(displayName = "Accelerometer Z", noisy = true)]
        public AxisControl accelZ { get; private set; }

        /// <summary>
        /// Retrieves a touch fret control by index.<br/>
        /// 0 = green, 4 = orange.
        /// </summary>
        public ButtonControl GetTouchFret(int index)
        {
            switch (index)
            {
                case 0: return touchGreen;
                case 1: return touchRed;
                case 2: return touchYellow;
                case 3: return touchBlue;
                case 4: return touchOrange;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(FretCount)} ({FretCount})!");
            }
        }

        /// <summary>
        /// Retrieves a touch fret control by enum value.
        /// </summary>
        public ButtonControl GetTouchFret(FiveFret fret)
        {
            switch (fret)
            {
                case FiveFret.Green: return touchGreen;
                case FiveFret.Red: return touchRed;
                case FiveFret.Yellow: return touchYellow;
                case FiveFret.Blue: return touchBlue;
                case FiveFret.Orange: return touchOrange;
                default: throw new ArgumentException($"Invalid fret value {fret}!", nameof(fret));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current touch fret states.
        /// </summary>
        public FiveFret GetTouchFretMask()
        {
            var mask = FiveFret.None;
            if (touchGreen.isPressed) mask |= FiveFret.Green;
            if (touchRed.isPressed) mask |= FiveFret.Red;
            if (touchYellow.isPressed) mask |= FiveFret.Yellow;
            if (touchBlue.isPressed) mask |= FiveFret.Blue;
            if (touchOrange.isPressed) mask |= FiveFret.Orange;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the touch fret states in the given state event.
        /// </summary>
        public FiveFret GetTouchFretMask(InputEventPtr eventPtr)
        {
            var mask = FiveFret.None;
            if (touchGreen.IsPressedInEvent(eventPtr)) mask |= FiveFret.Green;
            if (touchRed.IsPressedInEvent(eventPtr)) mask |= FiveFret.Red;
            if (touchYellow.IsPressedInEvent(eventPtr)) mask |= FiveFret.Yellow;
            if (touchBlue.IsPressedInEvent(eventPtr)) mask |= FiveFret.Blue;
            if (touchOrange.IsPressedInEvent(eventPtr)) mask |= FiveFret.Orange;
            return mask;
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            spPedal = GetChildControl<ButtonControl>(nameof(spPedal));

            touchGreen = GetChildControl<ButtonControl>(nameof(touchGreen));
            touchRed = GetChildControl<ButtonControl>(nameof(touchRed));
            touchYellow = GetChildControl<ButtonControl>(nameof(touchYellow));
            touchBlue = GetChildControl<ButtonControl>(nameof(touchBlue));
            touchOrange = GetChildControl<ButtonControl>(nameof(touchOrange));

            accelX = GetChildControl<AxisControl>(nameof(accelX));
            accelY = GetChildControl<AxisControl>(nameof(accelY));
            accelZ = GetChildControl<AxisControl>(nameof(accelZ));
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
