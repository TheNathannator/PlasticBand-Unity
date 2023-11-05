using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class ProKeyboardTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            TestHelpers.AssertDeviceCreation<ProKeyboard>();

            TestHelpers.AssertDeviceCreation<XInputProKeyboard>();

            TestHelpers.AssertDeviceCreation<PS3ProKeyboard>();
            TestHelpers.AssertDeviceCreation<PS3ProKeyboard_ReportId>();
            TestHelpers.AssertDeviceCreation<WiiProKeyboard>();
            TestHelpers.AssertDeviceCreation<WiiProKeyboard_ReportId>();
        }
    }
}