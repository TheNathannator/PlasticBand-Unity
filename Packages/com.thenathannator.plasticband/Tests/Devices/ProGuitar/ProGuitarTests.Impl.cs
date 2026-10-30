using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputProGuitarTests
        : ProGuitarTests<XInputProGuitar, XInputProGuitarState>
    {
        protected override XInputProGuitarState CreateState()
            => new XInputProGuitarState();

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Fender Mustang Guitar for Xbox 360 (17-fret)
            // MadCatz Rock Band 3 Fender Squire Guitar for Xbox 360 (22-fret)
            // MadCatz Rock Band 3 MIDI Pro Adapter for Xbox 360 (Guitar mode; 17-fret)
            // MadCatz Rock Band 3 MIDI Pro Adapter for Xbox 360 (Guitar mode; 22-fret)
            AssertXInputMatch(XInputDeviceSubType.ProGuitar);
        }
    }

    internal class PS3ProGuitarTests_NoReportId
        : ProGuitarTests_TiltPedal<PS3ProGuitar, PS3WiiProGuitarState_NoReportId>
    {
        protected override PS3WiiProGuitarState_NoReportId CreateState()
            => new PS3WiiProGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Fender Mustang Guitar for PS3 (17-fret)
            AssertHidMatch(0x12BA, 0x2430, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Guitar mode; 17-fret)
            AssertHidMatch(0x12BA, 0x2438, reportId: false);
            // MadCatz Rock Band 3 Fender Squire Guitar for PS3 (22-fret)
            AssertHidMatch(0x12BA, 0x2530, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Guitar mode; 22-fret)
            AssertHidMatch(0x12BA, 0x2538, reportId: false);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Fender Mustang Guitar for PS3 (17-fret)
            AssertHidMatch(0x12BA, 0x2430, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Guitar mode; 17-fret)
            AssertHidMatch(0x12BA, 0x2438, reportId: true);
            // MadCatz Rock Band 3 Fender Squire Guitar for PS3 (22-fret)
            AssertHidMatch(0x12BA, 0x2530, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (Guitar mode; 22-fret)
            AssertHidMatch(0x12BA, 0x2538, reportId: true);
        }
    }

    internal class WiiProGuitarTests_NoReportId
        : ProGuitarTests_TiltPedal<WiiProGuitar, PS3WiiProGuitarState_NoReportId>
    {
        protected override PS3WiiProGuitarState_NoReportId CreateState()
            => new PS3WiiProGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral
        };

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Fender Mustang Guitar for Wii (17-fret)
            AssertHidMatch(0x1BAD, 0x3430, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Guitar mode; 17-fret)
            AssertHidMatch(0x1BAD, 0x3438, reportId: false);
            // MadCatz Rock Band 3 Fender Squire Guitar for Wii (22-fret)
            AssertHidMatch(0x1BAD, 0x3530, reportId: false);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Guitar mode; 22-fret)
            AssertHidMatch(0x1BAD, 0x3538, reportId: false);
        }
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

        protected override void CheckExpectedDescriptions()
        {
            // MadCatz Rock Band 3 Fender Mustang Guitar for Wii (17-fret)
            AssertHidMatch(0x1BAD, 0x3430, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Guitar mode; 17-fret)
            AssertHidMatch(0x1BAD, 0x3438, reportId: true);
            // MadCatz Rock Band 3 Fender Squire Guitar for Wii (22-fret)
            AssertHidMatch(0x1BAD, 0x3530, reportId: true);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Guitar mode; 22-fret)
            AssertHidMatch(0x1BAD, 0x3538, reportId: true);
        }
    }
}