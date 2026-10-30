using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputProGuitarTests
        : ProGuitarTests<XInputProGuitar, XInputProGuitarState>
    {
        protected override XInputProGuitarState CreateState()
            => new XInputProGuitarState();
    }

    internal class PS3ProGuitarTests_NoReportId
        : ProGuitarTests_TiltPedal<PS3ProGuitar, PS3WiiProGuitarState_NoReportId>
    {
        protected override PS3WiiProGuitarState_NoReportId CreateState()
            => new PS3WiiProGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };
    }

    internal class PS3ProGuitarTests_ReportId
        : ProGuitarTests_TiltPedal<PS3ProGuitar_ReportId, PS3WiiProGuitarState_ReportId>
    {
        protected override PS3WiiProGuitarState_ReportId CreateState()
            => new PS3WiiProGuitarState_ReportId()
        {
            state = new PS3WiiProGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral
            }
        };
    }

    internal class WiiProGuitarTests_NoReportId
        : ProGuitarTests_TiltPedal<WiiProGuitar, PS3WiiProGuitarState_NoReportId>
    {
        protected override PS3WiiProGuitarState_NoReportId CreateState()
            => new PS3WiiProGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };
    }

    internal class WiiProGuitarTests_ReportId
        : ProGuitarTests_TiltPedal<WiiProGuitar_ReportId, PS3WiiProGuitarState_ReportId>
    {
        protected override PS3WiiProGuitarState_ReportId CreateState()
            => new PS3WiiProGuitarState_ReportId()
        {
            state = new PS3WiiProGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral
            }
        };
    }
}