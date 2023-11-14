using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    public class TurntableTests : PlasticBandTestFixture<Turntable>
    {
        public const TurntableButton AllButtons = TurntableButton.Green | TurntableButton.Red | TurntableButton.Blue;

        [Test]
        public void NorthAndEuphoriaAreEquivalent()
            => CreateAndRun(_NorthAndEuphoriaAreEquivalent);

        [Test]
        public void GetLeftButtonReturnsCorrectButtons()
            => CreateAndRun(_GetLeftButtonReturnsCorrectButtons);

        [Test]
        public void GetLeftButtonThrowsCorrectly()
            => CreateAndRun(_GetLeftButtonThrowsCorrectly);

        [Test]
        public void GetRightButtonReturnsCorrectButtons()
            => CreateAndRun(_GetRightButtonReturnsCorrectButtons);

        [Test]
        public void GetRightButtonThrowsCorrectly()
            => CreateAndRun(_GetRightButtonThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _NorthAndEuphoriaAreEquivalent(Turntable turntable)
        {
            Assert.That(turntable.euphoria, Is.EqualTo(turntable.buttonNorth));
        }

        public static void _GetLeftButtonReturnsCorrectButtons(Turntable turntable)
        {
            Assert.That(turntable.GetLeftButton(0), Is.EqualTo(turntable.leftTableGreen));
            Assert.That(turntable.GetLeftButton(1), Is.EqualTo(turntable.leftTableRed));
            Assert.That(turntable.GetLeftButton(2), Is.EqualTo(turntable.leftTableBlue));

            Assert.That(turntable.GetLeftButton(TurntableButton.Green), Is.EqualTo(turntable.leftTableGreen));
            Assert.That(turntable.GetLeftButton(TurntableButton.Red), Is.EqualTo(turntable.leftTableRed));
            Assert.That(turntable.GetLeftButton(TurntableButton.Blue), Is.EqualTo(turntable.leftTableBlue));
        }

        public static void _GetLeftButtonThrowsCorrectly(Turntable turntable)
        {
            for (int i = -5; i < Turntable.ButtonCount + 5; i++)
            {
                if (i < 0 || i >= Turntable.ButtonCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => turntable.GetLeftButton(i));
                else
                    Assert.DoesNotThrow(() => turntable.GetLeftButton(i));
            }

            for (var buttons = TurntableButton.None; buttons <= AllButtons; buttons++)
            {
                int buttonCount = 0;
                if ((buttons & TurntableButton.Green) != 0) buttonCount++;
                if ((buttons & TurntableButton.Red) != 0) buttonCount++;
                if ((buttons & TurntableButton.Blue) != 0) buttonCount++;

                if (buttonCount != 1)
                    Assert.Throws<ArgumentException>(() => turntable.GetLeftButton(buttons));
                else
                    Assert.DoesNotThrow(() => turntable.GetLeftButton(buttons));
            }
        }

        public static void _GetRightButtonReturnsCorrectButtons(Turntable turntable)
        {
            Assert.That(turntable.GetRightButton(0), Is.EqualTo(turntable.rightTableGreen));
            Assert.That(turntable.GetRightButton(1), Is.EqualTo(turntable.rightTableRed));
            Assert.That(turntable.GetRightButton(2), Is.EqualTo(turntable.rightTableBlue));

            Assert.That(turntable.GetRightButton(TurntableButton.Green), Is.EqualTo(turntable.rightTableGreen));
            Assert.That(turntable.GetRightButton(TurntableButton.Red), Is.EqualTo(turntable.rightTableRed));
            Assert.That(turntable.GetRightButton(TurntableButton.Blue), Is.EqualTo(turntable.rightTableBlue));
        }

        public static void _GetRightButtonThrowsCorrectly(Turntable turntable)
        {
            for (int i = -5; i < Turntable.ButtonCount + 5; i++)
            {
                if (i < 0 || i >= Turntable.ButtonCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => turntable.GetRightButton(i));
                else
                    Assert.DoesNotThrow(() => turntable.GetRightButton(i));
            }

            for (var buttons = TurntableButton.None; buttons <= AllButtons; buttons++)
            {
                int buttonCount = 0;
                if ((buttons & TurntableButton.Green) != 0) buttonCount++;
                if ((buttons & TurntableButton.Red) != 0) buttonCount++;
                if ((buttons & TurntableButton.Blue) != 0) buttonCount++;

                if (buttonCount != 1)
                    Assert.Throws<ArgumentException>(() => turntable.GetRightButton(buttons));
                else
                    Assert.DoesNotThrow(() => turntable.GetRightButton(buttons));
            }
        }
    }

    internal abstract class TurntableTests<TTurntable, TState> : FaceButtonDeviceTestFixture<TTurntable, TState>
        where TTurntable : Turntable
        where TState : unmanaged, ITurntableState
    {
        public delegate void SetTurntableButtonAction(ref TState state, TurntableButton buttons);

        protected override ButtonControl GetFaceButton(TTurntable turntable, FaceButton button)
        {
            switch (button)
            {
                case FaceButton.South: return turntable.buttonSouth;
                case FaceButton.East: return turntable.buttonEast;
                case FaceButton.West: return turntable.buttonWest;
                case FaceButton.North: return turntable.buttonNorth;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override ButtonControl GetMenuButton(TTurntable turntable, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return turntable.startButton;
                case MenuButton.Select: return turntable.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override DpadControl GetDpad(TTurntable turntable) => turntable.dpad;

        protected override void SetDpad(ref TState state, DpadDirection dpad)
        {
            state.dpadUp = dpad.IsUp();
            state.dpadDown = dpad.IsDown();
            state.dpadLeft = dpad.IsLeft();
            state.dpadRight = dpad.IsRight();
        }

        protected override void SetFaceButtons(ref TState state, FaceButton buttons)
        {
            state.south = (buttons & FaceButton.South) != 0;
            state.east = (buttons & FaceButton.East) != 0;
            state.west = (buttons & FaceButton.West) != 0;
            state.north_euphoria = (buttons & FaceButton.North) != 0;
        }

        protected override void SetMenuButtons(ref TState state, MenuButton buttons)
        {
            state.start = (buttons & MenuButton.Start) != 0;
            state.select = (buttons & MenuButton.Select) != 0;
        }

        protected void SetLeftTableButtons(ref TState state, TurntableButton buttons)
        {
            state.leftGreen = (buttons & TurntableButton.Green) != 0;
            state.leftRed = (buttons & TurntableButton.Red) != 0;
            state.leftBlue = (buttons & TurntableButton.Blue) != 0;
        }

        protected void SetRightTableButtons(ref TState state, TurntableButton buttons)
        {
            state.rightGreen = (buttons & TurntableButton.Green) != 0;
            state.rightRed = (buttons & TurntableButton.Red) != 0;
            state.rightBlue = (buttons & TurntableButton.Blue) != 0;
        }

        protected void SetLeftTableVelocity(ref TState state, float velocity)
        {
            state.leftVelocity = DeviceHandling.DenormalizeSByte(velocity);
        }

        protected void SetRightTableVelocity(ref TState state, float velocity)
        {
            state.rightVelocity = DeviceHandling.DenormalizeSByte(velocity);
        }

        protected void SetEffectsDial(ref TState state, float rotation)
        {
            state.effectsDial = DeviceHandling.DenormalizeUInt16(rotation);
        }

        protected void SetCrossfader(ref TState state, float position)
        {
            state.crossfader = DeviceHandling.DenormalizeSByte(position);
        }

        [Test]
        public void NorthAndEuphoriaAreEquivalent()
            => CreateAndRun(TurntableTests._NorthAndEuphoriaAreEquivalent);

        [Test]
        public void GetLeftButtonReturnsCorrectButtons()
            => CreateAndRun(TurntableTests._GetLeftButtonReturnsCorrectButtons);

        [Test]
        public void GetLeftButtonThrowsCorrectly()
            => CreateAndRun(TurntableTests._GetLeftButtonThrowsCorrectly);

        [Test]
        public void GetRightButtonReturnsCorrectButtons()
            => CreateAndRun(TurntableTests._GetRightButtonReturnsCorrectButtons);

        [Test]
        public void GetRightButtonThrowsCorrectly()
            => CreateAndRun(TurntableTests._GetRightButtonThrowsCorrectly);

        [Test]
        public void RecognizesLeftTableButtons() => CreateAndRun((turntable) =>
        {
            RecognizesTableButtons(turntable, SetLeftTableButtons,
                turntable.leftTableGreen, turntable.leftTableRed, turntable.leftTableBlue);
        });

        [Test]
        public void RecognizesRightTableButtons() => CreateAndRun((turntable) =>
        {
            RecognizesTableButtons(turntable, SetRightTableButtons,
                turntable.rightTableGreen, turntable.rightTableRed, turntable.rightTableBlue);
        });

        protected void RecognizesTableButtons(TTurntable turntable, SetTurntableButtonAction setButtons,
            ButtonControl green, ButtonControl red, ButtonControl blue)
        {
            var state = CreateState();
            var buttonList = new List<ButtonControl>(3);
            for (var buttons = TurntableButton.None; buttons <= TurntableTests.AllButtons; buttons++)
            {
                setButtons(ref state, buttons);

                if ((buttons & TurntableButton.Green) != 0) buttonList.Add(green);
                if ((buttons & TurntableButton.Red) != 0) buttonList.Add(red);
                if ((buttons & TurntableButton.Blue) != 0) buttonList.Add(blue);

                AssertButtonPress(turntable, state, buttonList.ToArray());
                buttonList.Clear();
            }
        }

        [Test]
        public void RecognizesLeftTableVelocity() => CreateAndRun((turntable) =>
        {
            RecognizesSignedAxis(turntable, CreateState(), turntable.leftTableVelocity, SetLeftTableVelocity);
        });

        [Test]
        public void RecognizesRightTableVelocity() => CreateAndRun((turntable) =>
        {
            RecognizesSignedAxis(turntable, CreateState(), turntable.rightTableVelocity, SetRightTableVelocity);
        });

        [Test]
        public void RecognizesEffectsDial() => CreateAndRun((turntable) =>
        {
            RecognizesUnsignedAxis(turntable, CreateState(), turntable.effectsDial, SetEffectsDial);
        });

        [Test]
        public void RecognizesCrossfader() => CreateAndRun((turntable) =>
        {
            RecognizesSignedAxis(turntable, CreateState(), turntable.crossfader, SetCrossfader);
        });
    }
}