using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputFiveLaneDrumkitTests
        : FiveLaneDrumkitTests<XInputFiveLaneDrumkit, XInputFiveLaneDrumkitState>
    {
        protected override XInputFiveLaneDrumkitState CreateState()
            => new XInputFiveLaneDrumkitState();
    }

    internal class SantrollerXInputFiveLaneDrumkitTests
        : FiveLaneDrumkitTests<SantrollerXInputFiveLaneDrumkit, XInputFiveLaneDrumkitState>
    {
        protected override XInputFiveLaneDrumkitState CreateState()
            => new XInputFiveLaneDrumkitState();
    }

    internal class PS3FiveLaneDrumkitTests_NoReportId
        : FiveLaneDrumkitTests<PS3FiveLaneDrumkit, PS3FiveLaneDrumkitState_NoReportId>
    {
        protected override PS3FiveLaneDrumkitState_NoReportId CreateState()
            => new PS3FiveLaneDrumkitState_NoReportId()
        {
            dpad = HidDpad.Neutral,
        };
    }

    internal class PS3FiveLaneDrumkitTests_ReportId
        : FiveLaneDrumkitTests<PS3FiveLaneDrumkit_ReportId, PS3FiveLaneDrumkitState_ReportId>
    {
        protected override PS3FiveLaneDrumkitState_ReportId CreateState()
            => new PS3FiveLaneDrumkitState_ReportId()
        {
            state = new PS3FiveLaneDrumkitState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };
    }

    internal class SantrollerHIDFiveLaneDrumkitTests
        : FiveLaneDrumkitTests<SantrollerHIDFiveLaneDrumkit, SantrollerFiveLaneDrumkitState>
    {
        protected override SantrollerFiveLaneDrumkitState CreateState()
            => new SantrollerFiveLaneDrumkitState()
        {
            dpad = HidDpad.Neutral,
        };
    }
}