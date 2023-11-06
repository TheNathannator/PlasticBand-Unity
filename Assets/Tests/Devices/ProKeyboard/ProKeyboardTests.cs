using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class ProKeyboardTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<ProKeyboard>();

            AssertDeviceCreation<XInputProKeyboard>();

            AssertDeviceCreation<PS3ProKeyboard>();
            AssertDeviceCreation<PS3ProKeyboard_ReportId>();
            AssertDeviceCreation<WiiProKeyboard>();
            AssertDeviceCreation<WiiProKeyboard_ReportId>();
        }
    }
}