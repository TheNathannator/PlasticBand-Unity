using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class GuitarHeroGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<GuitarHeroGuitar>(VerifyDevice);

            AssertDeviceCreation<XInputGuitarHeroGuitar>(VerifyDevice);
            AssertDeviceCreation<SantrollerXInputGuitarHeroGuitar>(VerifyDevice);

            AssertDeviceCreation<PS3GuitarHeroGuitar>(VerifyDevice);
            AssertDeviceCreation<PS3GuitarHeroGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<SantrollerHIDGuitarHeroGuitar>(VerifyDevice);
        }

        private static void VerifyDevice<TGuitar>(TGuitar guitar)
            where TGuitar : GuitarHeroGuitar
        {
            FiveFretGuitarTests.VerifyDevice(guitar);

            // Ensure accelerometer X and tilt are equivalent
            Assert.That(guitar.accelX, Is.EqualTo(guitar.tilt));

            // Ensure GetTouchFret works correctly
            FiveFretGuitarTests.VerifyFrets(guitar.GetTouchFret, guitar.GetTouchFret,
                guitar.touchGreen, guitar.touchRed, guitar.touchYellow, guitar.touchBlue, guitar.touchOrange);
        }
    }
}