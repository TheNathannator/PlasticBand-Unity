using System;
using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public sealed class ProGuitarTests : PlasticBandTestFixture<ProGuitar>
    {
        [Test]
        public void GetStringFretReturnsCorrectFrets()
            => CreateAndRun(_GetStringFretReturnsCorrectFrets);

        [Test]
        public void GetStringFretThrowsCorrectly()
            => CreateAndRun(_GetStringFretThrowsCorrectly);

        [Test]
        public void GetStringStrumReturnsCorrectStrums()
            => CreateAndRun(_GetStringStrumReturnsCorrectStrums);

        [Test]
        public void GetStringStrumThrowsCorrectly()
            => CreateAndRun(_GetStringStrumThrowsCorrectly);

        [Test]
        public void GetEmulatedFretReturnsCorrectFrets()
            => CreateAndRun(_GetEmulatedFretReturnsCorrectFrets);

        [Test]
        public void GetEmulatedFretThrowsCorrectly()
            => CreateAndRun(_GetEmulatedFretThrowsCorrectly);

        [Test]
        public void GetEmulatedSoloFretReturnsCorrectFrets()
            => CreateAndRun(_GetEmulatedSoloFretReturnsCorrectFrets);

        [Test]
        public void GetEmulatedSoloFretThrowsCorrectly()
            => CreateAndRun(_GetEmulatedSoloFretThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetStringFretReturnsCorrectFrets(ProGuitar guitar)
        {
            Assert.That(guitar.GetStringFret(0), Is.EqualTo(guitar.fret1));
            Assert.That(guitar.GetStringFret(1), Is.EqualTo(guitar.fret2));
            Assert.That(guitar.GetStringFret(2), Is.EqualTo(guitar.fret3));
            Assert.That(guitar.GetStringFret(3), Is.EqualTo(guitar.fret4));
            Assert.That(guitar.GetStringFret(4), Is.EqualTo(guitar.fret5));
            Assert.That(guitar.GetStringFret(5), Is.EqualTo(guitar.fret6));
        }

        public static void _GetStringFretThrowsCorrectly(ProGuitar guitar)
        {
            for (int i = -5; i < ProGuitar.StringCount + 5; i++)
            {
                if (i < 0 || i >= ProGuitar.StringCount)
                    Assert.Throws<ArgumentOutOfRangeException>(()
                        => guitar.GetStringFret(i));
                else
                    Assert.DoesNotThrow(()
                        => guitar.GetStringFret(i));
            }
        }

        public static void _GetStringStrumReturnsCorrectStrums(ProGuitar guitar)
        {
            Assert.That(guitar.GetStringStrum(0), Is.EqualTo(guitar.strum1));
            Assert.That(guitar.GetStringStrum(1), Is.EqualTo(guitar.strum2));
            Assert.That(guitar.GetStringStrum(2), Is.EqualTo(guitar.strum3));
            Assert.That(guitar.GetStringStrum(3), Is.EqualTo(guitar.strum4));
            Assert.That(guitar.GetStringStrum(4), Is.EqualTo(guitar.strum5));
            Assert.That(guitar.GetStringStrum(5), Is.EqualTo(guitar.strum6));
        }

        public static void _GetStringStrumThrowsCorrectly(ProGuitar guitar)
        {
            for (int i = -5; i < ProGuitar.StringCount + 5; i++)
            {
                if (i < 0 || i >= ProGuitar.StringCount)
                    Assert.Throws<ArgumentOutOfRangeException>(()
                        => guitar.GetStringStrum(i));
                else
                    Assert.DoesNotThrow(()
                        => guitar.GetStringStrum(i));
            }
        }

        public static void _GetEmulatedFretReturnsCorrectFrets(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetEmulatedFret, guitar.GetEmulatedFret,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        }

        public static void _GetEmulatedFretThrowsCorrectly(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetEmulatedFret, guitar.GetEmulatedFret);
        }

        public static void _GetEmulatedSoloFretReturnsCorrectFrets(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetEmulatedSoloFret, guitar.GetEmulatedSoloFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        }

        public static void _GetEmulatedSoloFretThrowsCorrectly(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetEmulatedSoloFret, guitar.GetEmulatedSoloFret);
        }
    }
}