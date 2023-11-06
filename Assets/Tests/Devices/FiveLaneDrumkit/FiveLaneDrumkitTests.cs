using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class FiveLaneDrumkitTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<FiveLaneDrumkit>();

            AssertDeviceCreation<XInputFiveLaneDrumkit>();
            AssertDeviceCreation<SantrollerXInputFiveLaneDrumkit>();

            AssertDeviceCreation<PS3FiveLaneDrumkit>();
            AssertDeviceCreation<PS3FiveLaneDrumkit_ReportId>();
            AssertDeviceCreation<SantrollerHIDFiveLaneDrumkit>();
        }
    }
}