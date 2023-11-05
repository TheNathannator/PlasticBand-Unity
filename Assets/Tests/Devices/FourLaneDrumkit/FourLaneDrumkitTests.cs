using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class FourLaneDrumkitTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<FourLaneDrumkit>();

            TestHelpers.AssertDeviceCreation<XInputFourLaneDrumkit>();
            TestHelpers.AssertDeviceCreation<SantrollerXInputFourLaneDrumkit>();

            TestHelpers.AssertDeviceCreation<PS3FourLaneDrumkit>();
            TestHelpers.AssertDeviceCreation<PS3FourLaneDrumkit_ReportId>();
            TestHelpers.AssertDeviceCreation<WiiFourLaneDrumkit>();
            TestHelpers.AssertDeviceCreation<WiiFourLaneDrumkit_ReportId>();
            TestHelpers.AssertDeviceCreation<PS4FourLaneDrumkit>();
            TestHelpers.AssertDeviceCreation<PS4FourLaneDrumkit_NoReportId>();
            TestHelpers.AssertDeviceCreation<SantrollerHIDFourLaneDrumkit>();
        }
    }
}