using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    public class RockBandGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<RockBandGuitar>(VerifyDevice);

            AssertDeviceCreation<XInputRockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<SantrollerXInputRockBandGuitar>(VerifyDevice);

            AssertDeviceCreation<PS3RockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<PS3RockBandGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<WiiRockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<WiiRockBandGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<PS4RockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<PS4RockBandGuitar_NoReportId>(VerifyDevice);
            AssertDeviceCreation<SantrollerHIDRockBandGuitar>(VerifyDevice);
        }

        private static void VerifyDevice(RockBandGuitar guitar)
        {
            // Ensure GetSoloFret works correctly
            FiveFretGuitarTests.VerifyFrets(guitar.GetSoloFret, guitar.GetSoloFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        }
    }
}