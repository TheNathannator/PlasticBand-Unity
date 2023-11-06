using System;
using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class SixFretGuitarTests : PlasticBandTestFixture
    {
        public const SixFret AllFrets = SixFret.Black1 | SixFret.Black2 | SixFret.Black3 |
            SixFret.White1 | SixFret.White2 | SixFret.White3;

        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<SixFretGuitar>(VerifyDevice);

            AssertDeviceCreation<XInputSixFretGuitar>(VerifyDevice);
            AssertDeviceCreation<SantrollerXInputSixFretGuitar>(VerifyDevice);

            AssertDeviceCreation<PS3WiiUSixFretGuitar>(VerifyDevice);
            AssertDeviceCreation<PS3WiiUSixFretGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<PS4SixFretGuitar>(VerifyDevice);
            AssertDeviceCreation<PS4SixFretGuitar_NoReportId>(VerifyDevice);
            AssertDeviceCreation<SantrollerHIDSixFretGuitar>(VerifyDevice);
        }

        public static void VerifyDevice(SixFretGuitar guitar)
        {
            // Ensure strum and d-pad up/down are equivalent
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));

            // Ensure GetFret returns the correct controls
            Assert.That(guitar.GetFret(SixFret.Black1), Is.EqualTo(guitar.black1));
            Assert.That(guitar.GetFret(SixFret.Black2), Is.EqualTo(guitar.black2));
            Assert.That(guitar.GetFret(SixFret.Black3), Is.EqualTo(guitar.black3));
            Assert.That(guitar.GetFret(SixFret.White1), Is.EqualTo(guitar.white1));
            Assert.That(guitar.GetFret(SixFret.White2), Is.EqualTo(guitar.white2));
            Assert.That(guitar.GetFret(SixFret.White3), Is.EqualTo(guitar.white3));

            // Ensure correct GetFret throw behavior
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