using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class SixFretGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<SixFretGuitar>(VerifyDevice);

            TestHelpers.AssertDeviceCreation<XInputSixFretGuitar>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS3WiiUSixFretGuitar>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS3WiiUSixFretGuitar_ReportId>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS4SixFretGuitar>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS4SixFretGuitar_NoReportId>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<SantrollerHIDSixFretGuitar>(VerifyDevice);
            TestHelpers.AssertDeviceCreation<SantrollerXInputSixFretGuitar>(VerifyDevice);
        }

        public static void VerifyDevice(SixFretGuitar guitar)
        {
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));
        }
    }
}