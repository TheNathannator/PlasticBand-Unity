using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class GuitarHeroGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<GuitarHeroGuitar>(VerifyDevice);

            TestHelpers.AssertDeviceCreation<XInputGuitarHeroGuitar>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<SantrollerXInputGuitarHeroGuitar>(VerifyDevice);

            TestHelpers.AssertDeviceCreation<PS3GuitarHeroGuitar>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS3GuitarHeroGuitar_ReportId>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<SantrollerHIDGuitarHeroGuitar>(VerifyDevice);
        }

        private static void VerifyDevice<TGuitar>(TGuitar guitar)
            where TGuitar : GuitarHeroGuitar
        {
            FiveFretGuitarTests.VerifyDevice(guitar);
            Assert.That(guitar.accelX, Is.EqualTo(guitar.tilt));
        }
    }
}