using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class StageKitTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<StageKit>();

            AssertDeviceCreation<XInputStageKit>();
            AssertDeviceCreation<SantrollerXInputStageKit>();

            AssertDeviceCreation<SantrollerHidStageKit>();
        }
    }
}