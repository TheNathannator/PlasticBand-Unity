using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Devices
{
    public sealed class FiveFretGuitarTests : PlasticBandTestFixture<FiveFretGuitar>
    {
        public const FiveFret AllFrets = FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange;

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
        public static void _StrumAndDpadAreEquivalent(FiveFretGuitar guitar)
        {
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));
        }

        public static void _GetFretReturnsCorrectFrets(FiveFretGuitar guitar)
        {
            _GetFretReturnsCorrectFrets(guitar.GetFret, guitar.GetFret,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        }

        public static void _GetFretThrowsCorrectly(FiveFretGuitar guitar)
        {
            _GetFretThrowsCorrectly(guitar.GetFret, guitar.GetFret);
        }

        // These methods are re-used for different sets of frets, including solo frets and Pro Guitar emulated frets,
        // so they must take delegates and parameters for each fret control
        public static void _GetFretReturnsCorrectFrets(
            Func<int, ButtonControl> getByIndex, Func<FiveFret, ButtonControl> getByEnum,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
        {
            Assert.That(getByIndex(0), Is.EqualTo(green));
            Assert.That(getByIndex(1), Is.EqualTo(red));
            Assert.That(getByIndex(2), Is.EqualTo(yellow));
            Assert.That(getByIndex(3), Is.EqualTo(blue));
            Assert.That(getByIndex(4), Is.EqualTo(orange));

            Assert.That(getByEnum(FiveFret.Green), Is.EqualTo(green));
            Assert.That(getByEnum(FiveFret.Red), Is.EqualTo(red));
            Assert.That(getByEnum(FiveFret.Yellow), Is.EqualTo(yellow));
            Assert.That(getByEnum(FiveFret.Blue), Is.EqualTo(blue));
            Assert.That(getByEnum(FiveFret.Orange), Is.EqualTo(orange));
        }

        public static void _GetFretThrowsCorrectly(
            Func<int, ButtonControl> getByIndex, Func<FiveFret, ButtonControl> getByEnum)
        {
            for (int i = -5; i < FiveFretGuitar.FretCount + 5; i++)
            {
                if (i < 0 || i >= FiveFretGuitar.FretCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => getByIndex(i));
                else
                    Assert.DoesNotThrow(() => getByIndex(i));
            }

            for (var frets = FiveFret.None; frets <= AllFrets; frets++)
            {
                int fretCount = 0;
                if ((frets & FiveFret.Green) != 0) fretCount++;
                if ((frets & FiveFret.Red) != 0) fretCount++;
                if ((frets & FiveFret.Yellow) != 0) fretCount++;
                if ((frets & FiveFret.Blue) != 0) fretCount++;
                if ((frets & FiveFret.Orange) != 0) fretCount++;

                if (fretCount != 1)
                    Assert.Throws<ArgumentException>(() => getByEnum(frets));
                else
                    Assert.DoesNotThrow(() => getByEnum(frets));
            }
        }
    }

    public abstract class FiveFretGuitarTests<TGuitar, TState> : DeviceTestFixture<TGuitar, TState>
        where TGuitar : FiveFretGuitar
        where TState : unmanaged, IInputStateTypeInfo
    {
        public delegate void SetFiveFretAction(ref TState state, FiveFret fret);

        protected abstract AxisMode tiltMode { get; }

        protected abstract void SetFrets(ref TState state, FiveFret frets);
        protected abstract void SetWhammy(ref TState state, float value);
        protected abstract void SetTilt(ref TState state, float value);

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

        [Test]
        public void StrumAndDpadAreEquivalent()
            => CreateAndRun(FiveFretGuitarTests._StrumAndDpadAreEquivalent);

        [Test]
        public void GetFretReturnsCorrectFrets()
            => CreateAndRun(FiveFretGuitarTests._GetFretReturnsCorrectFrets);

        [Test]
        public void GetFretThrowsCorrectly()
            => CreateAndRun(FiveFretGuitarTests._GetFretThrowsCorrectly);

        [Test]
        public void RecognizesFrets() => CreateAndRun((guitar) =>
        {
            _RecognizesFrets(guitar, CreateState(), SetFrets,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        });

        [Test]
        public void GetFretMaskReturnsCorrectFrets() => CreateAndRun((guitar) =>
        {
            _GetFretMaskReturnsCorrectFrets(guitar, CreateState(), SetFrets, guitar.GetFretMask, guitar.GetFretMask,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        });

        [Test]
        public void RecognizesWhammy() => CreateAndRun((guitar) =>
        {
            RecognizesUnsignedAxis(guitar, CreateState(), guitar.whammy, SetWhammy);
        });

        [Test]
        public void RecognizesTilt() => CreateAndRun((guitar) =>
        {
            RecognizesAxis(guitar, CreateState(), guitar.tilt, tiltMode, SetTilt);
        });

        // These methods are re-used for different sets of frets, including solo frets and Pro Guitar emulated frets,
        // so it must take delegates and parameters for each fret control
        // They must also be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _RecognizesFrets(InputDevice device, TState state, SetFiveFretAction setFret,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
        {
            var fretList = new List<ButtonControl>(5);
            for (var frets = FiveFret.None; frets <= FiveFretGuitarTests.AllFrets; frets++)
            {
                setFret(ref state, frets);

                if ((frets & FiveFret.Green) != 0) fretList.Add(green);
                if ((frets & FiveFret.Red) != 0) fretList.Add(red);
                if ((frets & FiveFret.Yellow) != 0) fretList.Add(yellow);
                if ((frets & FiveFret.Blue) != 0) fretList.Add(blue);
                if ((frets & FiveFret.Orange) != 0) fretList.Add(orange);

                AssertButtonPress(device, state, fretList.ToArray());
                fretList.Clear();
            }
        }

        public static void _GetFretMaskReturnsCorrectFrets(InputDevice device, TState state,
            SetFiveFretAction setFret, Func<FiveFret> getMask, Func<InputEventPtr, FiveFret> getMaskFromEvent,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
        {
            for (var frets = FiveFret.None; frets <= FiveFretGuitarTests.AllFrets; frets++)
            {
                setFret(ref state, frets);
                InputSystem.QueueStateEvent(device, state);
                InputSystem.Update();

                var mask = getMask();
                Assert.That(mask, Is.EqualTo(frets), "Fret mask is not correct!");

                Assert.That((mask & FiveFret.Green) != 0, Is.EqualTo(green.isPressed), "Green fret state is not correct!");
                Assert.That((mask & FiveFret.Red) != 0, Is.EqualTo(red.isPressed), "Red fret state is not correct!");
                Assert.That((mask & FiveFret.Yellow) != 0, Is.EqualTo(yellow.isPressed), "Yellow fret state is not correct!");
                Assert.That((mask & FiveFret.Blue) != 0, Is.EqualTo(blue.isPressed), "Blue fret state is not correct!");
                Assert.That((mask & FiveFret.Orange) != 0, Is.EqualTo(orange.isPressed), "Orange fret state is not correct!");

                // Not using InputSystem.onEvent since it doesn't handle NUnit assert exceptions correctly
                using (_ = StateEvent.From(device, out var eventPtr))
                {
                    mask = getMaskFromEvent(eventPtr);
                    Assert.That(mask, Is.EqualTo(frets), "Fret mask is not correct!");

                    Assert.That((mask & FiveFret.Green) != 0, Is.EqualTo(green.IsPressedInEvent(eventPtr)), "Green fret state is not correct!");
                    Assert.That((mask & FiveFret.Red) != 0, Is.EqualTo(red.IsPressedInEvent(eventPtr)), "Red fret state is not correct!");
                    Assert.That((mask & FiveFret.Yellow) != 0, Is.EqualTo(yellow.IsPressedInEvent(eventPtr)), "Yellow fret state is not correct!");
                    Assert.That((mask & FiveFret.Blue) != 0, Is.EqualTo(blue.IsPressedInEvent(eventPtr)), "Blue fret state is not correct!");
                    Assert.That((mask & FiveFret.Orange) != 0, Is.EqualTo(orange.IsPressedInEvent(eventPtr)), "Orange fret state is not correct!");
                }
            }
        }
    }

    public static class XInputFiveFretGuitarHandling
    {
        public static void SetFrets(ref ushort buttonsField, FiveFret frets)
        {
            buttonsField.SetBit((ushort)XInputButton.A, (frets & FiveFret.Green) != 0);
            buttonsField.SetBit((ushort)XInputButton.B, (frets & FiveFret.Red) != 0);
            buttonsField.SetBit((ushort)XInputButton.Y, (frets & FiveFret.Yellow) != 0);
            buttonsField.SetBit((ushort)XInputButton.X, (frets & FiveFret.Blue) != 0);
            buttonsField.SetBit((ushort)XInputButton.LeftShoulder, (frets & FiveFret.Orange) != 0);
        }

        public static short GetWhammy(float value)
        {
            return (short)IntegerAxisControl.Denormalize(value, short.MinValue, short.MaxValue, short.MinValue);
        }
    }
}