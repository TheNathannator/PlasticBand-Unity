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

        protected override void CheckExpectedDescriptions()
        {
            // Activision Guitar Hero Live Guitar for Xbox 360
            AssertXInputMatch(XInputDeviceSubType.GuitarAlternate, XInputDeviceFlags.NoNavigation);
        }
    }

    internal class XboxOneSixFretGuitarTests
        : SixFretGuitarTests<XboxOneSixFretGuitar, XboxOneSixFretGuitarState>
    {
        protected override XboxOneSixFretGuitarState CreateState()
            => new XboxOneSixFretGuitarState()
        {
            reportId = 0x21,
            baseState = new PS3WiiUSixFretGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
                strumBar = 0x80,

                // The default value for these properties is not 0, so we must set them explicitly
                tilt = 0,
            },
        };

        protected override void CheckExpectedDescriptions()
        {
            // Activision Guitar Hero Live Guitar for Xbox One
            AssertGameInputMatch(0x1430, 0x079B);
        }
    }

    internal class SantrollerXInputSixFretGuitarTests
        : SixFretGuitarTests<SantrollerXInputSixFretGuitar, XInputSixFretGuitarState>
    {
        protected override XInputSixFretGuitarState CreateState()
            => new XInputSixFretGuitarState();

        protected override void CheckExpectedDescriptions()
        {
            AssertXInputMatch(
                XInputDeviceSubType.GuitarAlternate,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0700
            );
        }
    }

    internal class PS3WiiUSixFretGuitarTests_NoReportId
        : SixFretGuitarTests<PS3WiiUSixFretGuitar, PS3WiiUSixFretGuitarState_NoReportId>
    {
        protected override PS3WiiUSixFretGuitarState_NoReportId CreateState()
            => new PS3WiiUSixFretGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
            strumBar = 0x80,

            // The default value for these properties is not 0, so we must set them explicitly
            tilt = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            // Activision Guitar Hero Live Guitar for PS3 and Wii U
            AssertHidMatch(0x12BA, 0x074B, reportId: false);
        }
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

                // The default value for these properties is not 0, so we must set them explicitly
                tilt = 0,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // Activision Guitar Hero Live Guitar for PS3 and Wii U
            AssertHidMatch(0x12BA, 0x074B, reportId: true);
        }
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

                // The default value for these properties is not 0, so we must set them explicitly
                tilt = 0,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // Activision Guitar Hero Live Guitar for PS4
            AssertHidMatch(0x1430, 0x07BB, reportId: true);
        }
    }

    internal class PS4SixFretGuitarTests_NoReportId
        : SixFretGuitarTests<PS4SixFretGuitar_NoReportId, PS4SixFretGuitarState_NoReportId>
    {
        protected override PS4SixFretGuitarState_NoReportId CreateState()
            => new PS4SixFretGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            strumBar = 0x80,

            // The default value for these properties is not 0, so we must set them explicitly
            tilt = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            // Activision Guitar Hero Live Guitar for PS4
            AssertHidMatch(0x1430, 0x07BB, reportId: false);
        }
    }

    internal class SantrollerHIDSixFretGuitarTests
        : SixFretGuitarTests<SantrollerHIDSixFretGuitar, SantrollerHIDSixFretGuitarState>
    {
        protected override SantrollerHIDSixFretGuitarState CreateState()
            => new SantrollerHIDSixFretGuitarState()
        {
            dpad = HidDpad.Neutral,

            // The default value for these properties is not 0, so we must set them explicitly
            tilt = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x1209, 0x2882, revision: 0x0700, reportId: true);
        }
    }

    internal class SantrollerV2HIDSixFretGuitarTests
        : SixFretGuitarTests<SantrollerV2HIDSixFretGuitar, SantrollerV2HIDSixFretGuitarState>
    {
        protected override SantrollerV2HIDSixFretGuitarState CreateState()
            => new SantrollerV2HIDSixFretGuitarState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,

            // The default value for these properties is not 0, so we must set them explicitly
            tilt = 0,
        };
    }
}