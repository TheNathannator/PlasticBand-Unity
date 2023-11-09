using System;
using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class TurntableTests : PlasticBandTestFixture<Turntable>
    {
        public const TurntableButton AllButtons = TurntableButton.Green | TurntableButton.Red | TurntableButton.Blue;

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
}