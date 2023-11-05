using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class RockBandGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<RockBandGuitar>(FiveFretGuitarTests.VerifyDevice);

            TestHelpers.AssertDeviceCreation<XInputRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<SantrollerXInputRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);

            TestHelpers.AssertDeviceCreation<PS3RockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS3RockBandGuitar_ReportId>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<WiiRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<WiiRockBandGuitar_ReportId>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS4RockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<PS4RockBandGuitar_NoReportId>(FiveFretGuitarTests.VerifyDevice);
            TestHelpers.AssertDeviceCreation<SantrollerHIDRockBandGuitar>(FiveFretGuitarTests.VerifyDevice);
        }
    }
}