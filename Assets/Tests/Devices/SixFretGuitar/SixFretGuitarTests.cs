using System;
using NUnit.Framework;
using PlasticBand.Devices;

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
}