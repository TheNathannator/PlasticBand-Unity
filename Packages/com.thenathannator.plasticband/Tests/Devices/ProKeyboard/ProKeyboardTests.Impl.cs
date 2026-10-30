using System;
using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputProKeyboardTests
        : ProKeyboardTests<XInputProKeyboard, XInputProKeyboardState>
    {
        protected override XInputProKeyboardState CreateState()
            => new XInputProKeyboardState()
        {
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Pro Keyboard for Xbox 360
            // MadCatz Rock Band 3 MIDI Pro Adapter for Xbox 360 (Keys mode)
            AssertXInputMatch(XInputDeviceSubType.ProKeyboard);
        }
    }

    internal class PS3ProKeyboardTests_NoReportId
        : ProKeyboardTests<PS3ProKeyboard, PS3ProKeyboardState_NoReportId>
    {
        protected override PS3ProKeyboardState_NoReportId CreateState()
            => new PS3ProKeyboardState_NoReportId()
        {
            dpad = HidDpad.Neutral,
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Pro Keyboard for PS3
            AssertHidMatch(0x12BA, 0x2330, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Keys mode)
            AssertHidMatch(0x12BA, 0x2338, reportId: false);
        }
    }

    internal class PS3ProKeyboardTests_ReportId
        : ProKeyboardTests<PS3ProKeyboard_ReportId, PS3ProKeyboardState_ReportId>
    {
        protected override PS3ProKeyboardState_ReportId CreateState()
            => new PS3ProKeyboardState_ReportId()
        {
            state = new PS3ProKeyboardState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Pro Keyboard for PS3
            AssertHidMatch(0x12BA, 0x2330, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Keys mode)
            AssertHidMatch(0x12BA, 0x2338, reportId: true);
        }
    }

    internal class WiiProKeyboardTests_NoReportId
        : ProKeyboardTests<WiiProKeyboard, WiiProKeyboardState_NoReportId>
    {
        protected override WiiProKeyboardState_NoReportId CreateState()
            => new WiiProKeyboardState_NoReportId()
        {
            state = new PS3ProKeyboardState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Pro Keyboard for Wii
            AssertHidMatch(0x1BAD, 0x3330, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Keys mode)
            AssertHidMatch(0x1BAD, 0x3338, reportId: false);
        }
    }

    internal class WiiProKeyboardTests_ReportId
        : ProKeyboardTests<WiiProKeyboard_ReportId, WiiProKeyboardState_ReportId>
    {
        protected override WiiProKeyboardState_ReportId CreateState()
            => new WiiProKeyboardState_ReportId()
        {
            // lol, triple-layered state
            state = new WiiProKeyboardState_NoReportId()
            {
                state = new PS3ProKeyboardState_NoReportId()
                {
                    dpad = HidDpad.Neutral,
                }
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Pro Keyboard for Wii
            AssertHidMatch(0x1BAD, 0x3330, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Keys mode)
            AssertHidMatch(0x1BAD, 0x3338, reportId: true);
        }
    }
}