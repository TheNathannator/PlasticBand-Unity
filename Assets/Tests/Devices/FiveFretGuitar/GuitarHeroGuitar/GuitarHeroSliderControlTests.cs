using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlasticBand.Controls;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Devices
{
    internal struct GuitarHeroSliderState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('G', 'H', 'S', 'L');

        [InputControl(name = "touchGreen", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchRed", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchYellow", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchBlue", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchOrange", layout = "GuitarHeroSlider", format = "BYTE")]
        public byte slider;
    }

    [InputControlLayout(stateType = typeof(GuitarHeroSliderState), hideInUI = true)]
    internal class GuitarHeroSliderDevice : InputDevice
    {
        public ButtonControl touchGreen { get; private set; }
        public ButtonControl touchRed { get; private set; }
        public ButtonControl touchYellow { get; private set; }
        public ButtonControl touchBlue { get; private set; }
        public ButtonControl touchOrange { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            touchGreen = GetChildControl<GuitarHeroSliderControl>(nameof(touchGreen));
            touchRed = GetChildControl<GuitarHeroSliderControl>(nameof(touchRed));
            touchYellow = GetChildControl<GuitarHeroSliderControl>(nameof(touchYellow));
            touchBlue = GetChildControl<GuitarHeroSliderControl>(nameof(touchBlue));
            touchOrange = GetChildControl<GuitarHeroSliderControl>(nameof(touchOrange));
        }
    }

    internal class GuitarHeroSliderControlTests : PlasticBandTestFixture<GuitarHeroSliderDevice>
    {
        public override void Setup()
        {
            base.Setup();
            InputSystem.RegisterLayout<GuitarHeroSliderDevice>(nameof(GuitarHeroSliderDevice));
        }

        public override void TearDown()
        {
            InputSystem.RemoveLayout(nameof(GuitarHeroSliderDevice));
            base.TearDown();
        }

        [Test]
        public void HandlesWorldTourSlider()
            => CreateAndRun((device) => HandlesSlider(device, GuitarHeroSliderControl.s_WTSliderLookup));

        [Test]
        public void HandlesGH5Slider()
            => CreateAndRun((device) => HandlesSlider(device, GuitarHeroSliderControl.s_GH5SliderLookup));

        private static void HandlesSlider(GuitarHeroSliderDevice device, Dictionary<byte, FiveFret> sliderLookup)
        {
            // Set initial state; no buttons should be pressed at this point
            byte sliderDefault = sliderLookup.First((pair) => pair.Value == FiveFret.None).Key;
            var state = new GuitarHeroSliderState()
            {
                slider = sliderDefault
            };
            AssertButtonPress(device, state);

            var fretMap = new List<(FiveFret fret, ButtonControl control)>()
            {
                (FiveFret.Green, device.touchGreen),
                (FiveFret.Red, device.touchRed),
                (FiveFret.Yellow, device.touchYellow),
                (FiveFret.Blue, device.touchBlue),
                (FiveFret.Orange, device.touchOrange),
            };

            // Run through each value in the lookup
            var fretList = new List<ButtonControl>(fretMap.Count);
            foreach (var pair in sliderLookup)
            {
                var (value, frets) = (pair.Key, pair.Value);
                state.slider = value;

                foreach (var (fret, control) in fretMap)
                {
                    if ((frets & fret) != 0)
                        fretList.Add(control);
                }

                AssertButtonPress(device, state, fretList.ToArray());
                fretList.Clear();
            }

            // Reset to default; no buttons should be pressed at this point
            state.slider = sliderDefault;
            AssertButtonPress(device, state);
        }
    }
}