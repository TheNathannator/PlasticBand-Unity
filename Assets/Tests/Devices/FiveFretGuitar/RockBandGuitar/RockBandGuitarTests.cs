using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class RockBandGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<RockBandGuitar>(FiveFretGuitarTests.VerifyDevice);

            AssertDeviceCreation<XInputRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<SantrollerXInputRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);

            AssertDeviceCreation<PS3RockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<PS3RockBandGuitar_ReportId>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<WiiRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<WiiRockBandGuitar_ReportId>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<PS4RockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<PS4RockBandGuitar_NoReportId>(FiveFretGuitarTests.VerifyDevice);
            AssertDeviceCreation<SantrollerHIDRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
        }
    }
}