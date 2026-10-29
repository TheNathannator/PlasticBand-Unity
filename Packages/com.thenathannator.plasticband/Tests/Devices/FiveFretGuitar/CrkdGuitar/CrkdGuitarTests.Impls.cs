using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Controls;

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
    }

    internal class PS4CrkdGuitarTests_ReportId
        : RiffmasterGuitarTests<PS4CrkdGuitar, PS4RiffmasterGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RiffmasterGuitarState_ReportId CreateState()
            => new PS4RiffmasterGuitarState_ReportId()
        {
            state = new PS4RiffmasterGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),

                // The default value for these properties is not 0, so we must set them explicitly
                joystickX = 0,
                joystickY = 0,
            }
        };

        // P1 is only present on PS4/5 guitars
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }

    internal class PS4CrkdGuitarTests_NoReportId
        : RiffmasterGuitarTests<PS4RiffmasterGuitar_NoReportId, PS4RiffmasterGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RiffmasterGuitarState_NoReportId CreateState()
            => new PS4RiffmasterGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),

            // The default value for these properties is not 0, so we must set them explicitly
            joystickX = 0,
            joystickY = 0,
        };

        // P1 is only present on PS4/5 guitars
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }

    internal class PS5CrkdGuitarTests_ReportId
        : RiffmasterGuitarTests<PS5CrkdGuitar, PS5RiffmasterGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS5RiffmasterGuitarState_ReportId CreateState()
            => new PS5RiffmasterGuitarState_ReportId()
        {
            state = new PS5RiffmasterGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),

                // The default value for these properties is not 0, so we must set them explicitly
                joystickX = 0,
                joystickY = 0,
            }
        };

        // P1 is only present on PS4/5 guitars
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }

    internal class PS5CrkdGuitarTests_NoReportId
        : RiffmasterGuitarTests<PS5RiffmasterGuitar_NoReportId, PS5RiffmasterGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS5RiffmasterGuitarState_NoReportId CreateState()
            => new PS5RiffmasterGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),

            // The default value for these properties is not 0, so we must set them explicitly
            joystickX = 0,
            joystickY = 0,
        };

        // P1 is only present on PS4/5 guitars
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }
}
