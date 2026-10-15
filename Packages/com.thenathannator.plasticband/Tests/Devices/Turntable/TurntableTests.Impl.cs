using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputTurntableTests
        : TurntableTests<XInputTurntable, XInputTurntableState>
    {
        protected override XInputTurntableState CreateState()
            => new XInputTurntableState();

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane DJ Hero Turntable for Xbox 360
            AssertXInputMatch(XInputDeviceSubType.Turntable);
        }
    }

    internal class SantrollerXInputTurntableTests
        : TurntableTests<SantrollerXInputTurntable, XInputTurntableState>
    {
        protected override XInputTurntableState CreateState()
            => new XInputTurntableState();

        protected override void CheckExpectedDescriptions()
        {
            AssertXInputMatch(
                XInputDeviceSubType.Turntable,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0800
            );
        }
    }

    internal class PS3TurntableTests_NoReportId
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane DJ Hero Turntable for PS3
            AssertHidMatch(0x12BA, 0x0140, reportId: false);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane DJ Hero Turntable for PS3
            AssertHidMatch(0x12BA, 0x0140, reportId: true);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x1209, 0x2882, revision: 0x0800, reportId: true);
        }
    }

    internal class SantrollerV2HIDTurntableTests
        : TurntableTests<SantrollerV2HIDTurntable, SantrollerV2HIDTurntableState>
    {
        protected override SantrollerV2HIDTurntableState CreateState()
            => new SantrollerV2HIDTurntableState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,

            // The raw value for these properties is not 0, so we must set them explicitly
            leftVelocity = 0,
            rightVelocity = 0,
            crossfader = 0,
        };
    }
}