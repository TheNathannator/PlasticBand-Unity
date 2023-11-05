using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class StageKitTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<StageKit>();

            TestHelpers.AssertDeviceCreation<XInputStageKit>();
            TestHelpers.AssertDeviceCreation<SantrollerXInputStageKit>();

            TestHelpers.AssertDeviceCreation<SantrollerHidStageKit>();
        }
    }
}