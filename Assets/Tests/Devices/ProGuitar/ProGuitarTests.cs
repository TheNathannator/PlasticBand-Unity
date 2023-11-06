using System;
using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class ProGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<ProGuitar>(VerifyDevice);

            AssertDeviceCreation<XInputProGuitar>(VerifyDevice);

            AssertDeviceCreation<PS3ProGuitar>(VerifyDevice);
            AssertDeviceCreation<PS3ProGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<WiiProGuitar>(VerifyDevice);
            AssertDeviceCreation<WiiProGuitar_ReportId>(VerifyDevice);
        }

        private static void VerifyDevice(ProGuitar guitar)
        {
            // Ensure GetString* returns the correct controls
            Assert.That(guitar.GetStringFret(0), Is.EqualTo(guitar.fret1));
            Assert.That(guitar.GetStringFret(1), Is.EqualTo(guitar.fret2));
            Assert.That(guitar.GetStringFret(2), Is.EqualTo(guitar.fret3));
            Assert.That(guitar.GetStringFret(3), Is.EqualTo(guitar.fret4));
            Assert.That(guitar.GetStringFret(4), Is.EqualTo(guitar.fret5));
            Assert.That(guitar.GetStringFret(5), Is.EqualTo(guitar.fret6));

            Assert.That(guitar.GetStringStrum(0), Is.EqualTo(guitar.strum1));
            Assert.That(guitar.GetStringStrum(1), Is.EqualTo(guitar.strum2));
            Assert.That(guitar.GetStringStrum(2), Is.EqualTo(guitar.strum3));
            Assert.That(guitar.GetStringStrum(3), Is.EqualTo(guitar.strum4));
            Assert.That(guitar.GetStringStrum(4), Is.EqualTo(guitar.strum5));
            Assert.That(guitar.GetStringStrum(5), Is.EqualTo(guitar.strum6));

            // Ensure correct GetString* throw behavior
            for (int i = -5; i < ProGuitar.StringCount + 5; i++)
            {
                if (i < 0 || i >= ProGuitar.StringCount)
                {
                    Assert.Throws<ArgumentOutOfRangeException>(() => guitar.GetStringFret(i));
                    Assert.Throws<ArgumentOutOfRangeException>(() => guitar.GetStringStrum(i));
                }
                else
                {
                    Assert.DoesNotThrow(() => guitar.GetStringFret(i));
                    Assert.DoesNotThrow(() => guitar.GetStringStrum(i));
                }
            }

            // Ensure GetEmulatedFret works correctly
            FiveFretGuitarTests.VerifyFrets(guitar.GetEmulatedFret, guitar.GetEmulatedFret,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);

            // Ensure GetEmulatedSoloFret works correctly
            FiveFretGuitarTests.VerifyFrets(guitar.GetEmulatedSoloFret, guitar.GetEmulatedSoloFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        }
    }
}