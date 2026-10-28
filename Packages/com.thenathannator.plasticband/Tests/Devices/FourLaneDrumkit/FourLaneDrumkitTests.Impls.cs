using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputFourLaneDrumkitTests
        : FourLaneDrumkitTests_Flags<XInputFourLaneDrumkit, XInputFourLaneDrumkitState>
    {
        protected override XInputFourLaneDrumkitState CreateState()
            => new XInputFourLaneDrumkitState();
    }

    internal class SantrollerXInputFourLaneDrumkitTests
        : FourLaneDrumkitTests_Flags<SantrollerXInputFourLaneDrumkit, XInputFourLaneDrumkitState>
    {
        protected override XInputFourLaneDrumkitState CreateState()
            => new XInputFourLaneDrumkitState();
    }

    internal class XboxOneFourLaneDrumkitTests
        : FourLaneDrumkitTests_Distinct<XboxOneFourLaneDrumkit, XboxOneFourLaneDrumkitState>
    {
        protected override int VelocityPrecision => 16;

        protected override XboxOneFourLaneDrumkitState CreateState()
            => new XboxOneFourLaneDrumkitState()
        {
            reportId = 0x20,
        };
    }

    internal class PS3FourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Flags<PS3FourLaneDrumkit, PS3WiiFourLaneDrumkitState_NoReportId>
    {
        protected override PS3WiiFourLaneDrumkitState_NoReportId CreateState()
            => new PS3WiiFourLaneDrumkitState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };
    }

    internal class PS3FourLaneDrumkitTests_ReportId
        : FourLaneDrumkitTests_Flags<PS3FourLaneDrumkit_ReportId, PS3WiiFourLaneDrumkitState_ReportId>
    {
        protected override PS3WiiFourLaneDrumkitState_ReportId CreateState()
            => new PS3WiiFourLaneDrumkitState_ReportId()
        {
            state = new PS3WiiFourLaneDrumkitState_NoReportId()
            {
                dpad = HidDpad.Neutral
            }
        };
    }

    internal class WiiFourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Flags<WiiFourLaneDrumkit, PS3WiiFourLaneDrumkitState_NoReportId>
    {
        protected override PS3WiiFourLaneDrumkitState_NoReportId CreateState()
            => new PS3WiiFourLaneDrumkitState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };
    }

    internal class WiiFourLaneDrumkitTests_ReportId
        : FourLaneDrumkitTests_Flags<WiiFourLaneDrumkit_ReportId, PS3WiiFourLaneDrumkitState_ReportId>
    {
        protected override PS3WiiFourLaneDrumkitState_ReportId CreateState()
            => new PS3WiiFourLaneDrumkitState_ReportId()
        {
            state = new PS3WiiFourLaneDrumkitState_NoReportId()
            {
                dpad = HidDpad.Neutral
            }
        };
    }

    internal class PS4FourLaneDrumkitTests_ReportId
        : FourLaneDrumkitTests_Distinct<PS4FourLaneDrumkit, PS4FourLaneDrumkitState_ReportId>
    {
        protected override PS4FourLaneDrumkitState_ReportId CreateState()
            => new PS4FourLaneDrumkitState_ReportId()
        {
            state = new PS4FourLaneDrumkitState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            }
        };
    }

    internal class PS4FourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Distinct<PS4FourLaneDrumkit_NoReportId, PS4FourLaneDrumkitState_NoReportId>
    {
        protected override PS4FourLaneDrumkitState_NoReportId CreateState()
            => new PS4FourLaneDrumkitState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };
    }

    internal class SantrollerHIDFourLaneDrumkitTests
        : FourLaneDrumkitTests_Hybrid<SantrollerHIDFourLaneDrumkit, SantrollerFourLaneDrumkitState>
    {
        protected override SantrollerFourLaneDrumkitState CreateState()
            => new SantrollerFourLaneDrumkitState()
        {
            dpad = HidDpad.Neutral
        };
    }
}