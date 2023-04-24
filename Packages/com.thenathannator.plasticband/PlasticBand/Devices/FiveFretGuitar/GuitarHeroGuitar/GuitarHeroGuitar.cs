using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

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
        /// </summary>l
        public new static IReadOnlyList<GuitarHeroGuitar> all => s_AllDevices;
        private static readonly List<GuitarHeroGuitar> s_AllDevices = new List<GuitarHeroGuitar>();

        /// <summary>
        /// Registers <see cref="GuitarHeroGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<GuitarHeroGuitar>();
        }

        /// <summary>
        /// The Star Power pedal port on the bottom of the guitar.
        /// </summary>
        [InputControl(name = "spPedal", displayName = "Star Power Pedal")]
        public ButtonControl spPedal { get; private set; }

        /// <summary>
        /// The green segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(name = "touchGreen", displayName = "Touch/Slider Bar Green")]
        public ButtonControl touchGreen { get; private set; }

        /// <summary>
        /// The red segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(name = "touchRed", displayName = "Touch/Slider Bar Red")]
        public ButtonControl touchRed { get; private set; }

        /// <summary>
        /// The yellow segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(name = "touchYellow", displayName = "Touch/Slider Bar Yellow")]
        public ButtonControl touchYellow { get; private set; }

        /// <summary>
        /// The blue segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(name = "touchBlue", displayName = "Touch/Slider Bar Blue")]
        public ButtonControl touchBlue { get; private set; }

        /// <summary>
        /// The orange segment of the guitar's touch/slider bar.
        /// </summary>
        [InputControl(name = "touchOrange", displayName = "Touch/Slider Bar Orange")]
        public ButtonControl touchOrange { get; private set; }

        /// <summary>
        /// The X-axis of the guitar's accelerometer.
        /// </summary>
        /// <remarks>
        /// Positive is when the guitar is pointed up, negative is when the guitar is pointed down.
        /// </remarks>
        [InputControl(name = "accelX", noisy = true, displayName = "Accelerometer X")]
        public AxisControl accelX { get; private set; }

        /// <summary>
        /// The Y-axis of the guitar's accelerometer.
        /// </summary>
        /// <remarks>
        /// Positive is when the guitar is face down, negative is when the guitar is face up.
        /// </remarks>
        [InputControl(name = "accelY", noisy = true, displayName = "Accelerometer Y")]
        public AxisControl accelY { get; private set; }

        /// <summary>
        /// The Z-axis of the guitar's accelerometer.
        /// </summary>
        /// <remarks>
        /// Positive is when the guitar is pointed right, negative is when the guitar is pointed left.
        /// </remarks>
        [InputControl(name = "accelZ", noisy = true, displayName = "Accelerometer Z")]
        public AxisControl accelZ { get; private set; }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
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

        /// <summary>
        /// Sets this device as the current <see cref="GuitarHeroGuitar"/>.
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
