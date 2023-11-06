using System;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    public class FiveFretGuitarTests : PlasticBandTestFixture
    {
        public const FiveFret AllFrets = FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange;

        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<FiveFretGuitar>(VerifyDevice);
        }

        public static void VerifyDevice(FiveFretGuitar guitar)
        {
            // Ensure strum and d-pad up/down are equivalent
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));

            // Ensure GetFret works correctly
            VerifyFrets(guitar.GetFret, guitar.GetFret,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        }

        public static void VerifyFrets(Func<int, ButtonControl> getByIndex, Func<FiveFret, ButtonControl> getByEnum,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
        {
            // Ensure the correct controls are returned
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

            // Ensure correct throw behavior
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
}