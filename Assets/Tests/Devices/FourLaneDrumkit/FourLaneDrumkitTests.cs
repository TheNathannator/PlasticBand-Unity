using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class FourLaneDrumkitTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<FourLaneDrumkit>();

            AssertDeviceCreation<XInputFourLaneDrumkit>();
            AssertDeviceCreation<SantrollerXInputFourLaneDrumkit>();

            AssertDeviceCreation<PS3FourLaneDrumkit>();
            AssertDeviceCreation<PS3FourLaneDrumkit_ReportId>();
            AssertDeviceCreation<WiiFourLaneDrumkit>();
            AssertDeviceCreation<WiiFourLaneDrumkit_ReportId>();
            AssertDeviceCreation<PS4FourLaneDrumkit>();
            AssertDeviceCreation<PS4FourLaneDrumkit_NoReportId>();
            AssertDeviceCreation<SantrollerHIDFourLaneDrumkit>();
        }
    }
}