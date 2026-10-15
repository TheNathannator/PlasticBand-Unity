using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputRockBandGuitarTests
        : RockBandGuitarTests_Flags<XInputRockBandGuitar, XInputRockBandGuitarState>
    {
        protected override XInputRockBandGuitarState CreateState()
            => new XInputRockBandGuitarState()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Guitar for Xbox 360 (all models)
            AssertXInputMatch(XInputDeviceSubType.Guitar);
            // MadCatz Fender Precision Bass Guitar for Xbox 360
            AssertXInputMatch(XInputDeviceSubType.GuitarBass);
        }
    }

    internal class XboxOneRockBandGuitarTests
        : RockBandGuitarTests_Distinct<XboxOneRockBandGuitar, XboxOneRockBandGuitarState>
    {
        protected override XboxOneRockBandGuitarState CreateState()
            => new XboxOneRockBandGuitarState()
        {
            reportId = 0x20,
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 4 Fender Stratocaster Guitar for Xbox One
            AssertGameInputMatch(0x0738, 0x4161);
            // PDP Rock Band 4 Fender Jaguar Guitar for Xbox One
            AssertGameInputMatch(0x0E6F, 0x0170);
        }
    }

    internal class SantrollerXInputRockBandGuitarTests
        : RockBandGuitarTests_Flags<SantrollerXInputRockBandGuitar, XInputRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override XInputRockBandGuitarState CreateState()
            => new XInputRockBandGuitarState()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertXInputMatch(
                XInputDeviceSubType.Guitar,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0400
            );
            AssertXInputMatch(
                XInputDeviceSubType.GuitarBass,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0400
            );
        }
    }

    internal class PS3RockBandGuitarTests_NoReportId
        : RockBandGuitarTests_Flags<PS3RockBandGuitar, PS3WiiRockBandGuitarState_NoReportId>
    {
        protected override PS3WiiRockBandGuitarState_NoReportId CreateState()
            => new PS3WiiRockBandGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Guitar for PS3 (all models)
            AssertHidMatch(0x12BA, 0x0200, reportId: false);
        }
    }

    internal class PS3RockBandGuitarTests_ReportId
        : RockBandGuitarTests_Flags<PS3RockBandGuitar_ReportId, PS3WiiRockBandGuitarState_ReportId>
    {
        protected override PS3WiiRockBandGuitarState_ReportId CreateState()
            => new PS3WiiRockBandGuitarState_ReportId()
        {
            state = new PS3WiiRockBandGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Guitar for PS3 (all models)
            AssertHidMatch(0x12BA, 0x0200, reportId: true);
        }
    }

    internal class WiiRockBandGuitarTests_NoReportId
        : RockBandGuitarTests_Flags<WiiRockBandGuitar, PS3WiiRockBandGuitarState_NoReportId>
    {
        protected override PS3WiiRockBandGuitarState_NoReportId CreateState()
            => new PS3WiiRockBandGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Guitar for Wii (all models made for RB1)
            AssertHidMatch(0x1BAD, 0x0004, reportId: false);
            // MadCatz Rock Band Guitar for Wii (all models made for RB2 and later)
            AssertHidMatch(0x1BAD, 0x3010, reportId: false);
        }
    }

    internal class WiiRockBandGuitarTests_ReportId
        : RockBandGuitarTests_Flags<WiiRockBandGuitar_ReportId, PS3WiiRockBandGuitarState_ReportId>
    {
        protected override PS3WiiRockBandGuitarState_ReportId CreateState()
            => new PS3WiiRockBandGuitarState_ReportId()
        {
            state = new PS3WiiRockBandGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Guitar for Wii (all models made for RB1)
            AssertHidMatch(0x1BAD, 0x0004, reportId: true);
            // MadCatz Rock Band Guitar for Wii (all models made for RB2 and later)
            AssertHidMatch(0x1BAD, 0x3010, reportId: true);
        }
    }

    internal class PS4RockBandGuitarTests_ReportId
        : RockBandGuitarTests_Distinct<PS4RockBandGuitar, PS4RockBandGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RockBandGuitarState_ReportId CreateState()
            => new PS4RockBandGuitarState_ReportId()
        {
            state = new PS4RockBandGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 4 Fender Stratocaster Guitar for PS4
            AssertHidMatch(0x0738, 0x8261, reportId: true);
            // PDP Rock Band 4 Fender Jaguar Guitar for PS4
            AssertHidMatch(0x0E6F, 0x0173, reportId: true);
        }
    }

    internal class PS4RockBandGuitarTests_NoReportId
        : RockBandGuitarTests_Distinct<PS4RockBandGuitar_NoReportId, PS4RockBandGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RockBandGuitarState_NoReportId CreateState()
            => new PS4RockBandGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 4 Fender Stratocaster Guitar for PS4
            AssertHidMatch(0x0738, 0x8261, reportId: false);
            // PDP Rock Band 4 Fender Jaguar Guitar for PS4
            AssertHidMatch(0x0E6F, 0x0173, reportId: false);
        }
    }

    internal class SantrollerHIDRockBandGuitarTests
        : RockBandGuitarTests_Distinct<SantrollerHIDRockBandGuitar, SantrollerHIDRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override SantrollerHIDRockBandGuitarState CreateState()
            => new SantrollerHIDRockBandGuitarState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,

            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x1209, 0x2882, revision: 0x0400, reportId: true);
        }
    }

    internal class SantrollerV2HIDRockBandGuitarTests
        : RockBandGuitarTests_Flags<SantrollerV2HIDRockBandGuitar, SantrollerV2HIDRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override SantrollerV2HIDRockBandGuitarState CreateState()
            => new SantrollerV2HIDRockBandGuitarState()
        {
            report = new SantrollerV2HIDReport()
            {
                reportId = 1,
                buttons1 = (byte)HidDpad.Neutral,
            },

            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
        };
    }
}