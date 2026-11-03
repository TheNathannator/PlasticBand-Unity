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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero Guitar for Xbox 360 (all models)
            AssertXInputMatch(XInputDeviceSubType.GuitarAlternate);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero 5 and Band Hero Guitar for Xbox 360
            AssertXInputMatch(
                XInputDeviceSubType.GuitarAlternate,
                vendorId: 0x1430,
                productId: 0x0705
            );
        }
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

        protected override void CheckExpectedDescriptions()
        {
            AssertXInputMatch(
                XInputDeviceSubType.GuitarAlternate,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0300
            );
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero Guitar for PS3 (all models)
            AssertHidMatch(0x12BA, 0x0100, reportId: false);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero Guitar for PS3 (all models)
            AssertHidMatch(0x12BA, 0x0100, reportId: true);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero 5 Guitar for PS3
            AssertHidMatch(
                0x12BA,
                0x0100,
                productName: "Guitar Hero5 for PlayStation (R) 3",
                reportId: false
            );
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero 5 Guitar for PS3
            AssertHidMatch(
                0x12BA,
                0x0100,
                productName: "Guitar Hero5 for PlayStation (R) 3",
                reportId: true
            );
        }
    }

    internal class PCGuitarHeroGuitarTests_NoReportId
        : GuitarHeroGuitarTests<PCGuitarHeroGuitar, PS3GuitarHeroGuitarState_NoReportId>
    {
        protected override PS3GuitarHeroGuitarState_NoReportId CreateState()
            => new PS3GuitarHeroGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
            // The default value for these properties is not 0, so we must set them explicitly
            tilt = 0,
            rawTouchBar = 0x80,
        };

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero World Tour Guitar for PC
            AssertHidMatch(0x1430, 0x474C, reportId: false);
        }
    }

    internal class PCGuitarHeroGuitarTests_ReportId
        : GuitarHeroGuitarTests<PCGuitarHeroGuitar_ReportId, PS3GuitarHeroGuitarState_ReportId>
    {
        protected override PS3GuitarHeroGuitarState_ReportId CreateState()
            => new PS3GuitarHeroGuitarState_ReportId()
        {
            state = new PS3GuitarHeroGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
                // The default value for these properties is not 0, so we must set them explicitly
                tilt = 0,
                rawTouchBar = 0x80,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // RedOctane Guitar Hero World Tour Guitar for PC
            AssertHidMatch(0x1430, 0x474C, reportId: true);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x1209, 0x2882, revision: 0x0300, reportId: true);
        }
    }

    internal class SantrollerV2HIDGuitarHeroGuitarTests
        : GuitarHeroGuitarTests<SantrollerV2HIDGuitarHeroGuitar, SantrollerV2HIDGuitarHeroGuitarState>
    {
        protected override SantrollerV2HIDGuitarHeroGuitarState CreateState()
            => new SantrollerV2HIDGuitarHeroGuitarState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,
            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
            rawTouchBar = 0x80,
        };

        protected override void CheckExpectedDescriptions()
        {
        }
    }
}