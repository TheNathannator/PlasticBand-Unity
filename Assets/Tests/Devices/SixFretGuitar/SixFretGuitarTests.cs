using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    public sealed class SixFretGuitarTests : PlasticBandTestFixture<SixFretGuitar>
    {
        public const SixFret AllFrets = SixFret.Black1 | SixFret.Black2 | SixFret.Black3 |
            SixFret.White1 | SixFret.White2 | SixFret.White3;

        [Test]
        public void StrumAndDpadAreEquivalent()
            => CreateAndRun(_StrumAndDpadAreEquivalent);

        [Test]
        public void GetFretReturnsCorrectFrets()
            => CreateAndRun(_GetFretReturnsCorrectFrets);

        [Test]
        public void GetFretThrowsCorrectly()
            => CreateAndRun(_GetFretThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _StrumAndDpadAreEquivalent(SixFretGuitar guitar)
        {
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));
        }

        public static void _GetFretReturnsCorrectFrets(SixFretGuitar guitar)
        {
            Assert.That(guitar.GetFret(0), Is.EqualTo(guitar.black1));
            Assert.That(guitar.GetFret(1), Is.EqualTo(guitar.black2));
            Assert.That(guitar.GetFret(2), Is.EqualTo(guitar.black3));
            Assert.That(guitar.GetFret(3), Is.EqualTo(guitar.white1));
            Assert.That(guitar.GetFret(4), Is.EqualTo(guitar.white2));
            Assert.That(guitar.GetFret(5), Is.EqualTo(guitar.white3));

            Assert.That(guitar.GetFret(SixFret.Black1), Is.EqualTo(guitar.black1));
            Assert.That(guitar.GetFret(SixFret.Black2), Is.EqualTo(guitar.black2));
            Assert.That(guitar.GetFret(SixFret.Black3), Is.EqualTo(guitar.black3));
            Assert.That(guitar.GetFret(SixFret.White1), Is.EqualTo(guitar.white1));
            Assert.That(guitar.GetFret(SixFret.White2), Is.EqualTo(guitar.white2));
            Assert.That(guitar.GetFret(SixFret.White3), Is.EqualTo(guitar.white3));
        }

        public static void _GetFretThrowsCorrectly(SixFretGuitar guitar)
        {
            for (int i = -5; i < SixFretGuitar.FretCount + 5; i++)
            {
                if (i < 0 || i >= SixFretGuitar.FretCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => guitar.GetFret(i));
                else
                    Assert.DoesNotThrow(() => guitar.GetFret(i));
            }

            for (var frets = SixFret.None; frets <= AllFrets; frets++)
            {
                int fretCount = 0;
                if ((frets & SixFret.Black1) != 0) fretCount++;
                if ((frets & SixFret.Black2) != 0) fretCount++;
                if ((frets & SixFret.Black3) != 0) fretCount++;
                if ((frets & SixFret.White1) != 0) fretCount++;
                if ((frets & SixFret.White2) != 0) fretCount++;
                if ((frets & SixFret.White3) != 0) fretCount++;

                if (fretCount != 1)
                    Assert.Throws<ArgumentException>(() => guitar.GetFret(frets));
                else
                    Assert.DoesNotThrow(() => guitar.GetFret(frets));
            }
        }
    }

    internal abstract class SixFretGuitarTests<TGuitar, TState> : DeviceTestFixture<TGuitar, TState>
        where TGuitar : SixFretGuitar
        where TState : unmanaged, ISixFretGuitarState
    {
        protected override ButtonControl GetMenuButton(TGuitar guitar, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return guitar.startButton;
                case MenuButton.Select: return guitar.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override DpadControl GetDpad(TGuitar guitar) => guitar.dpad;

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

        protected void SetFrets(ref TState state, SixFret frets)
        {
            state.black1 = (frets & SixFret.Black1) != 0;
            state.black2 = (frets & SixFret.Black2) != 0;
            state.black3 = (frets & SixFret.Black3) != 0;
            state.white1 = (frets & SixFret.White1) != 0;
            state.white2 = (frets & SixFret.White2) != 0;
            state.white3 = (frets & SixFret.White3) != 0;
        }

        protected void SetStrumUp(ref TState state, bool pressed)
        {
            state.strumUp = pressed;
        }

        protected void SetStrumDown(ref TState state, bool pressed)
        {
            state.strumDown = pressed;
        }

        protected void SetWhammy(ref TState state, float value)
        {
            state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected void SetTilt(ref TState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeSByte(value);
        }

        protected void SetGHTVButton(ref TState state, bool pressed)
        {
            state.ghtv = pressed;
        }

        [Test]
        public void StrumAndDpadAreEquivalent()
            => CreateAndRun(SixFretGuitarTests._StrumAndDpadAreEquivalent);

        [Test]
        public void GetFretReturnsCorrectFrets()
            => CreateAndRun(SixFretGuitarTests._GetFretReturnsCorrectFrets);

        [Test]
        public void GetFretThrowsCorrectly()
            => CreateAndRun(SixFretGuitarTests._GetFretThrowsCorrectly);

        [Test]
        public void RecognizesFrets() => CreateAndRun((guitar) =>
        {
            var state = CreateState();
            var fretList = new List<ButtonControl>(6);
            for (var frets = SixFret.None; frets <= SixFretGuitarTests.AllFrets; frets++)
            {
                SetFrets(ref state, frets);

                if ((frets & SixFret.Black1) != 0) fretList.Add(guitar.black1);
                if ((frets & SixFret.Black2) != 0) fretList.Add(guitar.black2);
                if ((frets & SixFret.Black3) != 0) fretList.Add(guitar.black3);
                if ((frets & SixFret.White1) != 0) fretList.Add(guitar.white1);
                if ((frets & SixFret.White2) != 0) fretList.Add(guitar.white2);
                if ((frets & SixFret.White3) != 0) fretList.Add(guitar.white3);

                AssertButtonPress(guitar, state, fretList.ToArray());
                fretList.Clear();
            }
        });

        // Most 6-fret guitars report the strumbar independently from the d-pad,
        // so we need to check this explicitly
        [Test]
        public void RecognizesStrumAsDpad() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), guitar.dpad.up, SetStrumUp);
            RecognizesButton(guitar, CreateState(), guitar.dpad.down, SetStrumDown);
        });

        [Test]
        public void GetFretMaskReturnsCorrectFrets() => CreateAndRun((guitar) =>
        {
            var state = CreateState();
            for (var frets = SixFret.None; frets <= SixFretGuitarTests.AllFrets; frets++)
            {
                SetFrets(ref state, frets);
                AssertButtonsWithEventUpdate(guitar, state, frets, guitar.GetFretMask, guitar.GetFretMask, AssertMask);
            }

            void AssertMask(SixFret mask, SixFret targetMask, Func<ButtonControl, bool> buttonPressed)
            {
                Assert.That(mask, Is.EqualTo(targetMask), "Fret mask is not correct!");

                Assert.That((mask & SixFret.Black1) != 0, Is.EqualTo(buttonPressed(guitar.black1)), "Black 1 fret state is not correct!");
                Assert.That((mask & SixFret.Black2) != 0, Is.EqualTo(buttonPressed(guitar.black2)), "Black 2 fret state is not correct!");
                Assert.That((mask & SixFret.Black3) != 0, Is.EqualTo(buttonPressed(guitar.black3)), "Black 3 fret state is not correct!");
                Assert.That((mask & SixFret.White1) != 0, Is.EqualTo(buttonPressed(guitar.white1)), "White 1 fret state is not correct!");
                Assert.That((mask & SixFret.White2) != 0, Is.EqualTo(buttonPressed(guitar.white2)), "White 2 fret state is not correct!");
                Assert.That((mask & SixFret.White3) != 0, Is.EqualTo(buttonPressed(guitar.white3)), "White 3 fret state is not correct!");
            }
        });

        [Test]
        public void RecognizesWhammy() => CreateAndRun((guitar) =>
        {
            RecognizesUnsignedAxis(guitar, CreateState(), guitar.whammy, SetWhammy);
        });

        [Test]
        public void RecognizesTilt() => CreateAndRun((guitar) =>
        {
            RecognizesSignedAxis(guitar, CreateState(), guitar.tilt, SetTilt);
        });

        [Test]
        public void RecognizesGHTVButton() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), guitar.ghtvButton, SetGHTVButton);
        });
    }
}