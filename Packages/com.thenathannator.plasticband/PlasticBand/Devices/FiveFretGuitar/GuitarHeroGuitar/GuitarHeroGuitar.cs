using PlasticBand.Controls;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// Dummy struct used to give GuitarHeroGuitars some controls in their layout.
    /// </summary>
    internal unsafe struct GuitarHeroGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('G', 'H', 'G', 'T');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]
        [InputControl(name = "strumUp", layout = "Button", bit = 0)]
        [InputControl(name = "strumDown", layout = "Button", bit = 1)]

        [InputControl(name = "greenFret", layout = "Button", bit = 4)]
        [InputControl(name = "redFret", layout = "Button", bit = 5)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 6)]
        [InputControl(name = "blueFret", layout = "Button", bit = 7)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]

        [InputControl(name = "startButton", layout = "Button", bit = 12)]
        [InputControl(name = "selectButton", layout = "Button", bit = 13)]
        [InputControl(name = "spPedal", layout = "Button", bit = 14, displayName = "Star Power Pedal")]
        public ushort buttons;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;

        [InputControl(name = "whammy", layout = "Axis")]
        public short whammy;

        [InputControl(name = "sliderBar", layout = "GuitarHeroSlider", displayName = "Touch/Slider Bar")]
        public byte slider;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Guitar Hero 5-fret guitar.
    /// Has some additional features that aren't available on all 5-fret guitars.
    /// </summary>
    [InputControlLayout(stateType = typeof(GuitarHeroGuitarState), displayName = "Guitar Hero 5-Fret Guitar")]
    public class GuitarHeroGuitar : FiveFretGuitar
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<GuitarHeroGuitar>();
        }

        /// <summary>
        /// The Star Power pedal port on the bottom of the guitar.
        /// </summary>
        public ButtonControl spPedal { get; private set; }

        /// <summary>
        /// The guitar's touch/slider bar.
        /// </summary>
        public GuitarHeroSliderControl sliderBar { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            spPedal = GetChildControl<ButtonControl>("spPedal");
            sliderBar = GetChildControl<GuitarHeroSliderControl>("sliderBar");
        }
    }
}
