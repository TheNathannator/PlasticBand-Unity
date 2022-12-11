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
        [InputControl(name = "accelX", noisy = true, displayName = "Accelerometer X")]
        public AxisControl accelX { get; private set; }

        /// <summary>
        /// The Y-axis of the guitar's accelerometer.
        /// </summary>
        [InputControl(name = "accelY", noisy = true, displayName = "Accelerometer Y")]
        public AxisControl accelY { get; private set; }

        /// <summary>
        /// The Z-axis of the guitar's accelerometer.
        /// </summary>
        [InputControl(name = "accelZ", noisy = true, displayName = "Accelerometer Z")]
        public AxisControl accelZ { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            spPedal = GetChildControl<ButtonControl>("spPedal");

            touchGreen = GetChildControl<ButtonControl>("touchGreen");
            touchRed = GetChildControl<ButtonControl>("touchRed");
            touchYellow = GetChildControl<ButtonControl>("touchYellow");
            touchBlue = GetChildControl<ButtonControl>("touchBlue");
            touchOrange = GetChildControl<ButtonControl>("touchOrange");

            accelX = GetChildControl<AxisControl>("accelX");
            accelY = GetChildControl<AxisControl>("accelY");
            accelZ = GetChildControl<AxisControl>("accelZ");
        }
    }
}
