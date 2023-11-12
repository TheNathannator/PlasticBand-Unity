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

        public delegate void SetSliderAction<TState>(ref TState state, byte value)
            where TState : unmanaged, IInputStateTypeInfo;

        private static void SetSliderValue(ref GuitarHeroSliderState state, byte value)
            => state.slider = value;

        [Test]
        public void HandlesWorldTourSlider()
            => CreateAndRun((device) => _HandlesWorldTourSlider(device, new GuitarHeroSliderState(), SetSliderValue,
                device.touchGreen, device.touchRed, device.touchYellow, device.touchBlue, device.touchOrange));

        [Test]
        public void HandlesGH5Slider()
            => CreateAndRun((device) => _HandlesGH5Slider(device, new GuitarHeroSliderState(), SetSliderValue,
                device.touchGreen, device.touchRed, device.touchYellow, device.touchBlue, device.touchOrange));

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _HandlesWorldTourSlider<TState>(InputDevice device, TState state, SetSliderAction<TState> setSlider,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
            where TState : unmanaged, IInputStateTypeInfo
            => HandlesSlider(device, state, GuitarHeroSliderControl.s_WTSliderLookup, setSlider,
                green, red, yellow, blue, orange);

        public static void _HandlesGH5Slider<TState>(InputDevice device, TState state, SetSliderAction<TState> setSlider,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
            where TState : unmanaged, IInputStateTypeInfo
            => HandlesSlider(device, state, GuitarHeroSliderControl.s_GH5SliderLookup, setSlider,
                green, red, yellow, blue, orange);

        // TODO: Make usable with GuitarHeroGuitar
        private static void HandlesSlider<TState>(InputDevice device, TState state, 
            Dictionary<byte, FiveFret> sliderLookup, SetSliderAction<TState> setSlider,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
            where TState : unmanaged, IInputStateTypeInfo
        {
            // Set initial state; no buttons should be pressed at this point
            byte sliderDefault = sliderLookup.First((pair) => pair.Value == FiveFret.None).Key;
            setSlider(ref state, sliderDefault);
            AssertButtonPress(device, state);

            var fretMap = new List<(FiveFret fret, ButtonControl control)>()
            {
                (FiveFret.Green, green),
                (FiveFret.Red, red),
                (FiveFret.Yellow, yellow),
                (FiveFret.Blue, blue),
                (FiveFret.Orange, orange),
            };

            // Run through each value in the lookup
            var fretList = new List<ButtonControl>(fretMap.Count);
            foreach (var pair in sliderLookup)
            {
                var (value, frets) = (pair.Key, pair.Value);
                setSlider(ref state, value);

                foreach (var (fret, control) in fretMap)
                {
                    if ((frets & fret) != 0)
                        fretList.Add(control);
                }

                AssertButtonPress(device, state, fretList.ToArray());
                fretList.Clear();
            }

            // Reset to default; no buttons should be pressed at this point
            setSlider(ref state, sliderDefault);
            AssertButtonPress(device, state);
        }
    }
}