using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputTurntableTests
        : TurntableTests<XInputTurntable, XInputTurntableState>
    {
        protected override XInputTurntableState CreateState()
            => new XInputTurntableState();
    }

    internal class SantrollerXInputTurntableTests
        : TurntableTests<SantrollerXInputTurntable, XInputTurntableState>
    {
        protected override XInputTurntableState CreateState()
            => new XInputTurntableState();
    }

    internal class PS3TurntableTests
        : TurntableTests<PS3Turntable, PS3TurntableState_NoReportId>
    {
        protected override PS3TurntableState_NoReportId CreateState()
            => new PS3TurntableState_NoReportId()
        {
            dpad = HidDpad.Neutral,

            // The raw value for these properties is not 0, so we must set them explicitly
            leftVelocity = 0,
            rightVelocity = 0,
            crossfader = 0,
        };
    }

    internal class PS3TurntableTests_ReportId
        : TurntableTests<PS3Turntable_ReportId, PS3TurntableState_ReportId>
    {
        protected override PS3TurntableState_ReportId CreateState()
            => new PS3TurntableState_ReportId()
        {
            state = new PS3TurntableState_NoReportId()
            {
                dpad = HidDpad.Neutral,

                // The raw value for these properties is not 0, so we must set them explicitly
                leftVelocity = 0,
                rightVelocity = 0,
                crossfader = 0,
            }
        };
    }

    internal class SantrollerHIDTurntableTests_ReportId
        : TurntableTests<SantrollerHIDTurntable, SantrollerHIDTurntableState>
    {
        protected override SantrollerHIDTurntableState CreateState()
            => new SantrollerHIDTurntableState()
        {
            dpad = HidDpad.Neutral,

            // The raw value for these properties is not 0, so we must set them explicitly
            leftVelocity = 0,
            rightVelocity = 0,
            crossfader = 0,
        };
    }
}