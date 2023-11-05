using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class FiveLaneDrumkitTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<FiveLaneDrumkit>();

            TestHelpers.AssertDeviceCreation<XInputFiveLaneDrumkit>();
            TestHelpers.AssertDeviceCreation<SantrollerXInputFiveLaneDrumkit>();

            TestHelpers.AssertDeviceCreation<PS3FiveLaneDrumkit>();
            TestHelpers.AssertDeviceCreation<PS3FiveLaneDrumkit_ReportId>();
            TestHelpers.AssertDeviceCreation<SantrollerHIDFiveLaneDrumkit>();
        }
    }
}