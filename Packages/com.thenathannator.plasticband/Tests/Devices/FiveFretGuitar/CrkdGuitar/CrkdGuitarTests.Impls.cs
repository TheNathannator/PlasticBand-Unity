using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XboxOneCrkdGuitarTests
        : RockBandGuitarTests_Distinct<XboxOneCrkdGuitar, XboxOneRockBandGuitarState>
    {
        protected override XboxOneRockBandGuitarState CreateState()
            => new XboxOneRockBandGuitarState()
        {
            reportId = 0x20,
        };

        protected override void CheckExpectedDescriptions()
        {
            // CRKD Gibson Les Paul Guitar (Blueberry/Tribal/Cherry/Olive)
            AssertGameInputMatch(0x3651, 0x4161);
            // Stage Tour Gibson Kramer Guitar for Xbox
            AssertGameInputMatch(0x3958, 0x4161);
        }
    }

    internal class PS4CrkdGuitarTests_ReportId
        : RiffmasterGuitarTests<PS4CrkdGuitar, PS4RiffmasterGuitarState_ReportId>
    {
        protected override PS4RiffmasterGuitarState_ReportId CreateState()
            => new PS4RiffmasterGuitarState_ReportId()
        {
            state = new PS4RiffmasterGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),

                // The default value for these properties is not 0, so we must set them explicitly
                leftStickX = 0,
                leftStickY = 0,
                rightStickX = 0,
                rightStickY = 0,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // CRKD Gibson SG Guitar (Cherry/Black) (PS4 mode, wired)
            AssertHidMatch(0x3651, 0x1500, reportId: true);
            // CRKD Gibson SG Guitar (Cherry/Black) (PS4 mode, wireless)
            AssertHidMatch(0x3651, 0x5500, reportId: true);
            // Stage Tour Gibson Kramer Guitar (PS4 mode)
            AssertHidMatch(0x3958, 0x5500, reportId: true);
        }
    }

    internal class PS4CrkdGuitarTests_NoReportId
        : RiffmasterGuitarTests<PS4CrkdGuitar_NoReportId, PS4RiffmasterGuitarState_NoReportId>
    {
        protected override PS4RiffmasterGuitarState_NoReportId CreateState()
            => new PS4RiffmasterGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),

            // The default value for these properties is not 0, so we must set them explicitly
            leftStickX = 0,
            leftStickY = 0,
            rightStickX = 0,
            rightStickY = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            // CRKD Gibson SG Guitar (Cherry/Black) (PS4 mode, wired)
            AssertHidMatch(0x3651, 0x1500, reportId: false);
            // CRKD Gibson SG Guitar (Cherry/Black) (PS4 mode, wireless)
            AssertHidMatch(0x3651, 0x5500, reportId: false);
            // Stage Tour Gibson Kramer Guitar (PS4 mode)
            AssertHidMatch(0x3958, 0x5500, reportId: false);
        }
    }

    internal class PS5CrkdGuitarTests_ReportId
        : RiffmasterGuitarTests<PS5CrkdGuitar, PS5RiffmasterGuitarState_ReportId>
    {
        protected override PS5RiffmasterGuitarState_ReportId CreateState()
            => new PS5RiffmasterGuitarState_ReportId()
        {
            state = new PS5RiffmasterGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),

                // The default value for these properties is not 0, so we must set them explicitly
                leftStickX = 0,
                leftStickY = 0,
                rightStickX = 0,
                rightStickY = 0,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // CRKD Gibson SG Guitar (Cherry/Black) (PS5 mode, wired)
            AssertHidMatch(0x3651, 0x1600, reportId: true);
            // CRKD Gibson SG Guitar (Cherry/Black) (PS5 mode, wireless)
            AssertHidMatch(0x3651, 0x5600, reportId: true);
            // Stage Tour Gibson Kramer Guitar (PS5 mode)
            AssertHidMatch(0x3958, 0x5600, reportId: true);
        }
    }

    internal class PS5CrkdGuitarTests_NoReportId
        : RiffmasterGuitarTests<PS5CrkdGuitar_NoReportId, PS5RiffmasterGuitarState_NoReportId>
    {
        protected override PS5RiffmasterGuitarState_NoReportId CreateState()
            => new PS5RiffmasterGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),

            // The default value for these properties is not 0, so we must set them explicitly
            leftStickX = 0,
            leftStickY = 0,
            rightStickX = 0,
            rightStickY = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            // CRKD Gibson SG Guitar (Cherry/Black) (PS5 mode, wired)
            AssertHidMatch(0x3651, 0x1600, reportId: false);
            // CRKD Gibson SG Guitar (Cherry/Black) (PS5 mode, wireless)
            AssertHidMatch(0x3651, 0x5600, reportId: false);
            // Stage Tour Gibson Kramer Guitar (PS5 mode)
            AssertHidMatch(0x3958, 0x5600, reportId: false);
        }
    }
}
