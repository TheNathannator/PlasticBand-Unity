using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputSixFretGuitarTests
        : SixFretGuitarTests<XInputSixFretGuitar, XInputSixFretGuitarState>
    {
        protected override XInputSixFretGuitarState CreateState()
            => new XInputSixFretGuitarState();
    }

    internal class SantrollerXInputSixFretGuitarTests
        : SixFretGuitarTests<SantrollerXInputSixFretGuitar, XInputSixFretGuitarState>
    {
        protected override XInputSixFretGuitarState CreateState()
            => new XInputSixFretGuitarState();
    }

    internal class PS3WiiUSixFretGuitarTests_NoReportId
        : SixFretGuitarTests<PS3WiiUSixFretGuitar, PS3WiiUSixFretGuitarState_NoReportId>
    {
        protected override PS3WiiUSixFretGuitarState_NoReportId CreateState()
            => new PS3WiiUSixFretGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
            strumBar = 0x80,
        };
    }

    internal class PS3WiiUSixFretGuitarTests_ReportId
        : SixFretGuitarTests<PS3WiiUSixFretGuitar_ReportId, PS3WiiUSixFretGuitarState_ReportId>
    {
        protected override PS3WiiUSixFretGuitarState_ReportId CreateState()
            => new PS3WiiUSixFretGuitarState_ReportId()
        {
            state = new PS3WiiUSixFretGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
                strumBar = 0x80,
            }
        };
    }

    internal class PS4SixFretGuitarTests_ReportId
        : SixFretGuitarTests<PS4SixFretGuitar, PS4SixFretGuitarState_ReportId>
    {
        protected override PS4SixFretGuitarState_ReportId CreateState()
            => new PS4SixFretGuitarState_ReportId()
        {
            state = new PS4SixFretGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
                strumBar = 0x80,
            }
        };
    }

    internal class PS4SixFretGuitarTests_NoReportId
        : SixFretGuitarTests<PS4SixFretGuitar_NoReportId, PS4SixFretGuitarState_NoReportId>
    {
        protected override PS4SixFretGuitarState_NoReportId CreateState()
            => new PS4SixFretGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            strumBar = 0x80,
        };
    }

    internal class SantrollerHIDSixFretGuitarTests
        : SixFretGuitarTests<SantrollerHIDSixFretGuitar, SantrollerHIDSixFretGuitarState>
    {
        protected override SantrollerHIDSixFretGuitarState CreateState()
            => new SantrollerHIDSixFretGuitarState()
        {
            dpad = HidDpad.Neutral,
        };
    }
}