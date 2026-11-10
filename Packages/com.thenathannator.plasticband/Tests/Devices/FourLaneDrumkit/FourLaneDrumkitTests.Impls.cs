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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Drumkit for Xbox 360 (all models)
            // MadCatz Rock Band 3 MIDI Pro Adapter for Xbox 360 (Drums mode)
            AssertXInputMatch(XInputDeviceSubType.DrumKit, XInputDeviceFlags.ForceFeedbackSupported);
            // MadCatz Rock Band Drumkit 1 for Xbox 360
            AssertXInputMatch(XInputDeviceSubType.DrumKit, vendorId: 0, productId: 0, revision: 0);
            // ION Drum Rocker for Xbox 360
            AssertXInputMatch(XInputDeviceSubType.DrumKit, vendorId: 0x15E4, productId: 0x0130);
        }
    }

    internal class SantrollerXInputFourLaneDrumkitTests
        : FourLaneDrumkitTests_Flags<SantrollerXInputFourLaneDrumkit, XInputFourLaneDrumkitState>
    {
        protected override XInputFourLaneDrumkitState CreateState()
            => new XInputFourLaneDrumkitState();

        protected override void CheckExpectedDescriptions()
        {
            AssertXInputMatch(
                XInputDeviceSubType.DrumKit,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0600
            );
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 4 Drumkit for Xbox One
            AssertGameInputMatch(0x0738, 0x4262);
            // PDP Rock Band 4 Drumkit for Xbox One
            AssertGameInputMatch(0x0E6F, 0x0171);
        }
    }

    internal class XboxCrkdDrumkitTests
        : FourLaneDrumkitTests_Distinct<XboxCrkdDrumkit, XboxOneFourLaneDrumkitState>
    {
        protected override int VelocityPrecision => 16;

        protected override XboxOneFourLaneDrumkitState CreateState()
            => new XboxOneFourLaneDrumkitState()
        {
            reportId = 0x20,
        };

        protected override void CheckExpectedDescriptions()
        {
            // Stage Tour Drumkit for Xbox
            AssertGameInputMatch(0x3958, 0xD300);
        }
    }

    internal class PS3FourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Flags<PS3FourLaneDrumkit, PS3WiiFourLaneDrumkitState_NoReportId>
    {
        protected override PS3WiiFourLaneDrumkitState_NoReportId CreateState()
            => new PS3WiiFourLaneDrumkitState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Drumkit for PS3 (all models)
            AssertHidMatch(0x12BA, 0x0210, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Drums mode)
            AssertHidMatch(0x12BA, 0x0218, reportId: false);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Drumkit for PS3 (all models)
            AssertHidMatch(0x12BA, 0x0210, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Drums mode)
            AssertHidMatch(0x12BA, 0x0218, reportId: true);
        }
    }

    internal class WiiFourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Flags<WiiFourLaneDrumkit, PS3WiiFourLaneDrumkitState_NoReportId>
    {
        protected override PS3WiiFourLaneDrumkitState_NoReportId CreateState()
            => new PS3WiiFourLaneDrumkitState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Drumkit for Wii (all models made for RB1)
            AssertHidMatch(0x1BAD, 0x0005, reportId: false);
            // MadCatz Rock Band Drumkit for Wii (all models made for RB2 and later)
            AssertHidMatch(0x1BAD, 0x3110, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Drums mode)
            AssertHidMatch(0x1BAD, 0x3138, reportId: false);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band Drumkit for Wii (all models made for RB1)
            AssertHidMatch(0x1BAD, 0x0005, reportId: true);
            // MadCatz Rock Band Drumkit for Wii (all models made for RB2 and later)
            AssertHidMatch(0x1BAD, 0x3110, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Drums mode)
            AssertHidMatch(0x1BAD, 0x3138, reportId: true);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 4 Drumkit for PS4
            AssertHidMatch(0x0738, 0x8262, reportId: true);
            // PDP Rock Band 4 Drumkit for PS4
            AssertHidMatch(0x0E6F, 0x0174, reportId: true);
        }
    }

    internal class PS4FourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Distinct<PS4FourLaneDrumkit_NoReportId, PS4FourLaneDrumkitState_NoReportId>
    {
        protected override PS4FourLaneDrumkitState_NoReportId CreateState()
            => new PS4FourLaneDrumkitState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 4 Drumkit for PS4
            AssertHidMatch(0x0738, 0x8262, reportId: false);
            // PDP Rock Band 4 Drumkit for PS4
            AssertHidMatch(0x0E6F, 0x0174, reportId: false);
        }
    }

    internal class PS5FourLaneDrumkitTests_ReportId
        : FourLaneDrumkitTests_Distinct<PS5FourLaneDrumkit, PS5FourLaneDrumkitState_ReportId>
    {
        protected override PS5FourLaneDrumkitState_ReportId CreateState()
            => new PS5FourLaneDrumkitState_ReportId()
        {
            state = new PS5FourLaneDrumkitState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // None currently
        }
    }

    internal class PS5FourLaneDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Distinct<PS5FourLaneDrumkit_NoReportId, PS5FourLaneDrumkitState_NoReportId>
    {
        protected override PS5FourLaneDrumkitState_NoReportId CreateState()
            => new PS5FourLaneDrumkitState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };

        protected override void CheckExpectedDescriptions()
        {
            // None currently
        }
    }

    internal class PS4CrkdDrumkitTests_ReportId
        : FourLaneDrumkitTests_Distinct<PS4CrkdDrumkit, PS4FourLaneDrumkitState_ReportId>
    {
        protected override PS4FourLaneDrumkitState_ReportId CreateState()
            => new PS4FourLaneDrumkitState_ReportId()
        {
            state = new PS4FourLaneDrumkitState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // Stage Tour Drumkit for PlayStation (PS4 mode)
            AssertHidMatch(0x3958, 0xD500, reportId: true);
        }
    }

    internal class PS4CrkdDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Distinct<PS4CrkdDrumkit_NoReportId, PS4FourLaneDrumkitState_NoReportId>
    {
        protected override PS4FourLaneDrumkitState_NoReportId CreateState()
            => new PS4FourLaneDrumkitState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };

        protected override void CheckExpectedDescriptions()
        {
            // Stage Tour Drumkit for PlayStation (PS4 mode)
            AssertHidMatch(0x3958, 0xD500, reportId: false);
        }
    }

    internal class PS5CrkdDrumkitTests_ReportId
        : FourLaneDrumkitTests_Distinct<PS5CrkdDrumkit, PS5FourLaneDrumkitState_ReportId>
    {
        protected override PS5FourLaneDrumkitState_ReportId CreateState()
            => new PS5FourLaneDrumkitState_ReportId()
        {
            state = new PS5FourLaneDrumkitState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // Stage Tour Drumkit for PlayStation (PS5 mode)
            AssertHidMatch(0x3958, 0xD600, reportId: true);
        }
    }

    internal class PS5CrkdDrumkitTests_NoReportId
        : FourLaneDrumkitTests_Distinct<PS5CrkdDrumkit_NoReportId, PS5FourLaneDrumkitState_NoReportId>
    {
        protected override PS5FourLaneDrumkitState_NoReportId CreateState()
            => new PS5FourLaneDrumkitState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };

        protected override void CheckExpectedDescriptions()
        {
            // Stage Tour Drumkit for PlayStation (PS5 mode)
            AssertHidMatch(0x3958, 0xD600, reportId: false);
        }
    }

    internal class SantrollerHIDFourLaneDrumkitTests
        : FourLaneDrumkitTests_Hybrid<SantrollerHIDFourLaneDrumkit, SantrollerFourLaneDrumkitState>
    {
        protected override SantrollerFourLaneDrumkitState CreateState()
            => new SantrollerFourLaneDrumkitState()
        {
            dpad = HidDpad.Neutral
        };

        protected override void CheckExpectedDescriptions()
        {
        }
    }

    internal class SantrollerV2HIDFourLaneDrumkitTests
        : FourLaneDrumkitTests_Flags<SantrollerV2HIDFourLaneDrumkit, SantrollerV2HIDFourLaneDrumkitState>
    {
        protected override SantrollerV2HIDFourLaneDrumkitState CreateState()
            => new SantrollerV2HIDFourLaneDrumkitState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,
        };

        protected override void CheckExpectedDescriptions()
        {
        }
    }
}