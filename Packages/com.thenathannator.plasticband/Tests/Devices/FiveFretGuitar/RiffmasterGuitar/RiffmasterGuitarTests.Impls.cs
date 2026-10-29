using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XboxOneRiffmasterGuitarTests
        : RiffmasterGuitarTests<XboxOneRiffmasterGuitar, XboxOneRiffmasterGuitarState>
    {
        protected override bool hasRightStick => false;
        protected override bool hasP1 => false;

        protected override XboxOneRiffmasterGuitarState CreateState()
            => new XboxOneRiffmasterGuitarState()
        {
            reportId = 0x20,
        };
    }

    internal class PS4RiffmasterGuitarTests_ReportId
        : RiffmasterGuitarTests<PS4RiffmasterGuitar, PS4RiffmasterGuitarState_ReportId>
    {
        protected override bool hasRightStick => false;

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
    }

    internal class PS4RiffmasterGuitarTests_NoReportId
        : RiffmasterGuitarTests<PS4RiffmasterGuitar_NoReportId, PS4RiffmasterGuitarState_NoReportId>
    {
        protected override bool hasRightStick => false;

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
    }

    internal class PS5RiffmasterGuitarTests_ReportId
        : RiffmasterGuitarTests<PS5RiffmasterGuitar, PS5RiffmasterGuitarState_ReportId>
    {
        protected override bool hasRightStick => false;

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
    }

    internal class PS5RiffmasterGuitarTests_NoReportId
        : RiffmasterGuitarTests<PS5RiffmasterGuitar_NoReportId, PS5RiffmasterGuitarState_NoReportId>
    {
        protected override bool hasRightStick => false;

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
    }
}