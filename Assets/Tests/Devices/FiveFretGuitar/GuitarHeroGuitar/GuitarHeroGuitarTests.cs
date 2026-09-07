using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    internal sealed class GuitarHeroGuitarTests : PlasticBandTestFixture<GuitarHeroGuitar>
    {
        [Test]
        public void GetTouchFretReturnsCorrectFrets()
            => CreateAndRun(_GetTouchFretReturnsCorrectFrets);

        [Test]
        public void GetTouchFretThrowsCorrectly()
            => CreateAndRun(_GetTouchFretThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetTouchFretReturnsCorrectFrets(GuitarHeroGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetTouchFret, guitar.GetTouchFret,
                guitar.touchGreen, guitar.touchRed, guitar.touchYellow, guitar.touchBlue, guitar.touchOrange);
        }

        public static void _GetTouchFretThrowsCorrectly(GuitarHeroGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetTouchFret, guitar.GetTouchFret);
        }

        public static void HandlesTouchBar<TState>(GuitarHeroGuitar device, TState state,
            List<(byte value, FiveFret frets)> touchValues)
            where TState : unmanaged, IGuitarHeroGuitarState
        {
            const byte sliderDefault = 0x80;

            // Set initial state; no buttons should be pressed at this point
            state.rawTouchBar = sliderDefault;
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
            foreach (var pair in touchValues)
            {
                var (value, frets) = (pair.value, pair.frets);
                state.rawTouchBar = value;

                foreach (var (fret, control) in fretMap)
                {
                    if ((frets & fret) != 0)
                        fretList.Add(control);
                }

                AssertButtonPress(device, state, fretList.ToArray());
                fretList.Clear();
            }

            // Reset to default; no buttons should be pressed at this point
            state.rawTouchBar = sliderDefault;
            AssertButtonPress(device, state);
        }
    }

    internal abstract class GuitarHeroGuitarTests<TGuitar, TState> : FiveFretGuitarTests<TGuitar, TState>
        where TGuitar : GuitarHeroGuitar
        where TState : unmanaged, IGuitarHeroGuitarState
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override void SetDpad(ref TState state, DpadDirection dpad)
        {
            state.dpadUp = dpad.IsUp();
            state.dpadDown = dpad.IsDown();
            state.dpadLeft = dpad.IsLeft();
            state.dpadRight = dpad.IsRight();
        }

        protected override void SetMenuButtons(ref TState state, MenuButton buttons)
        {
            state.start = (buttons & MenuButton.Start) != 0;
            state.select = (buttons & MenuButton.Select) != 0;
        }

        protected override DpadControl GetDpad(TGuitar guitar) => guitar.dpad;

        protected override ButtonControl GetMenuButton(TGuitar guitar, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return guitar.startButton;
                case MenuButton.Select: return guitar.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override void SetFrets(ref TState state, FiveFret frets)
        {
            state.green = (frets & FiveFret.Green) != 0;
            state.red = (frets & FiveFret.Red) != 0;
            state.yellow = (frets & FiveFret.Yellow) != 0;
            state.blue = (frets & FiveFret.Blue) != 0;
            state.orange = (frets & FiveFret.Orange) != 0;
        }

        protected override void SetTilt(ref TState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeSByte(value);
        }

        protected override void SetWhammy(ref TState state, float value)
        {
            state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        [Test]
        public void GetTouchFretReturnsCorrectFrets()
            => CreateAndRun(GuitarHeroGuitarTests._GetTouchFretReturnsCorrectFrets);

        [Test]
        public void GetTouchFretThrowsCorrectly()
            => CreateAndRun(GuitarHeroGuitarTests._GetTouchFretThrowsCorrectly);
    }

    internal abstract class GuitarHeroGuitarTests_WT<TGuitar, TState> : FiveFretGuitarTests<TGuitar, TState>
        where TGuitar : GuitarHeroGuitar
        where TState : unmanaged, IGuitarHeroGuitarState
    {
        [Test]
        public void HandlesTouchBar() => CreateAndRun((guitar) =>
        {
            var touchValues = new List<(byte value, FiveFret frets)>();
            for (int index = 0; index < TranslatingGuitarHeroGuitar.s_WtTouchFrets.Length; index++)
            {
                var frets = TranslatingGuitarHeroGuitar.s_WtTouchFrets[index];
                for (int n = 0; n < 0x0F; n++)
                {
                    touchValues.Add(((byte)((index << 4) + n), frets));
                }
            }

            GuitarHeroGuitarTests.HandlesTouchBar(guitar, CreateState(), touchValues);
        });
    }

    internal abstract class GuitarHeroGuitarTests_GH5<TGuitar, TState> : FiveFretGuitarTests<TGuitar, TState>
        where TGuitar : GuitarHeroGuitar
        where TState : unmanaged, IGuitarHeroGuitarState
    {
        [Test]
        public void HandlesTouchBar() => CreateAndRun((guitar) =>
        {
            var touchValues = new List<(byte value, FiveFret frets)>();
            for (int index = 0; index < TranslatingGuitarHeroGuitar.s_Gh5TouchFrets.Length; index++)
            {
                touchValues.Add(((byte)index, TranslatingGuitarHeroGuitar.s_WtTouchFrets[index]));
            }

            GuitarHeroGuitarTests.HandlesTouchBar(guitar, CreateState(), touchValues);
        });
    }
}