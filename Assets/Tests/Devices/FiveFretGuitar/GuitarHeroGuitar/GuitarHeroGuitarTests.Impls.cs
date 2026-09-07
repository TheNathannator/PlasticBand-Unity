using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputGuitarHeroGuitarTests
        : GuitarHeroGuitarTests<XInputGuitarHeroGuitar, XInputGuitarHeroGuitarState>
    {
        protected override XInputGuitarHeroGuitarState CreateState()
            => new XInputGuitarHeroGuitarState()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
            rawTouchBar = 0x80,
        };
    }

    internal class XInputGuitarHeroGuitarTests_GH5
        : GuitarHeroGuitarTests<XInputGuitarHeroGuitar_GH5, XInputGuitarHeroGuitarState_GH5>
    {
        protected override XInputGuitarHeroGuitarState_GH5 CreateState()
            => new XInputGuitarHeroGuitarState_GH5()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
            rawTouchBar = 0x80,
        };
    }

    internal class SantrollerXInputGuitarHeroGuitarTests
        : GuitarHeroGuitarTests<SantrollerXInputGuitarHeroGuitar, XInputGuitarHeroGuitarState>
    {
        protected override XInputGuitarHeroGuitarState CreateState()
            => new XInputGuitarHeroGuitarState()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
            rawTouchBar = 0x80,
        };
    }

    internal class PS3GuitarHeroGuitarTests_NoReportId
        : GuitarHeroGuitarTests<PS3GuitarHeroGuitar, PS3GuitarHeroGuitarState_NoReportId>
    {
        protected override PS3GuitarHeroGuitarState_NoReportId CreateState()
            => new PS3GuitarHeroGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
            rawTouchBar = 0x80,
        };
    }

    internal class PS3GuitarHeroGuitarTests_ReportId
        : GuitarHeroGuitarTests<PS3GuitarHeroGuitar_ReportId, PS3GuitarHeroGuitarState_ReportId>
    {
        protected override PS3GuitarHeroGuitarState_ReportId CreateState()
            => new PS3GuitarHeroGuitarState_ReportId()
        {
            state = new PS3GuitarHeroGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
                // The default value for this property is not 0, so we must set it explicitly
                tilt = 0,
                rawTouchBar = 0x80,
            }
        };
    }

    internal class PS3GuitarHeroGuitarTests_GH5_NoReportId
        : GuitarHeroGuitarTests<PS3GuitarHeroGuitar_GH5, PS3GuitarHeroGuitarState_NoReportId>
    {
        protected override PS3GuitarHeroGuitarState_NoReportId CreateState()
            => new PS3GuitarHeroGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
            rawTouchBar = 0x80,
        };
    }

    internal class PS3GuitarHeroGuitarTests_GH5_ReportId
        : GuitarHeroGuitarTests<PS3GuitarHeroGuitar_GH5_ReportId, PS3GuitarHeroGuitarState_ReportId>
    {
        protected override PS3GuitarHeroGuitarState_ReportId CreateState()
            => new PS3GuitarHeroGuitarState_ReportId()
        {
            state = new PS3GuitarHeroGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
                // The default value for this property is not 0, so we must set it explicitly
                tilt = 0,
                rawTouchBar = 0x80,
            }
        };
    }

    internal class SantrollerHIDGuitarHeroGuitarTests
        : GuitarHeroGuitarTests<SantrollerHIDGuitarHeroGuitar, SantrollerHIDGuitarHeroGuitarState>
    {
        protected override SantrollerHIDGuitarHeroGuitarState CreateState()
            => new SantrollerHIDGuitarHeroGuitarState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,
            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
            rawTouchBar = 0x80,
        };
    }
}