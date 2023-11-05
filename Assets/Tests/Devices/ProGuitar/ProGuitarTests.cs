using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class ProGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<ProGuitar>();

            TestHelpers.AssertDeviceCreation<XInputProGuitar>();

            TestHelpers.AssertDeviceCreation<PS3ProGuitar>();
            TestHelpers.AssertDeviceCreation<PS3ProGuitar_ReportId>();
            TestHelpers.AssertDeviceCreation<WiiProGuitar>();
            TestHelpers.AssertDeviceCreation<WiiProGuitar_ReportId>();
        }
    }
}