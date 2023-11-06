using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class TurntableTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<Turntable>();

            AssertDeviceCreation<XInputTurntable>();
            AssertDeviceCreation<SantrollerXInputTurntable>();

            AssertDeviceCreation<PS3Turntable>();
            AssertDeviceCreation<PS3Turntable_ReportId>();
            AssertDeviceCreation<SantrollerHIDTurntable>();
        }
    }
}