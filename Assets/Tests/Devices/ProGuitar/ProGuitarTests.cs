using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class ProGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<ProGuitar>();

            AssertDeviceCreation<XInputProGuitar>();

            AssertDeviceCreation<PS3ProGuitar>();
            AssertDeviceCreation<PS3ProGuitar_ReportId>();
            AssertDeviceCreation<WiiProGuitar>();
            AssertDeviceCreation<WiiProGuitar_ReportId>();
        }
    }
}