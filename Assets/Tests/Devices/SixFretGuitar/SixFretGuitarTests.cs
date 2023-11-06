using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class SixFretGuitarTests : PlasticBandTestFixture
    {
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
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));
        }
    }
}