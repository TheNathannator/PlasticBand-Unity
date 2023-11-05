using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class TurntableTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<Turntable>();

            TestHelpers.AssertDeviceCreation<XInputTurntable>();
            TestHelpers.AssertDeviceCreation<SantrollerXInputTurntable>();

            TestHelpers.AssertDeviceCreation<PS3Turntable>();
            TestHelpers.AssertDeviceCreation<PS3Turntable_ReportId>();
            TestHelpers.AssertDeviceCreation<SantrollerHIDTurntable>();
        }
    }
}