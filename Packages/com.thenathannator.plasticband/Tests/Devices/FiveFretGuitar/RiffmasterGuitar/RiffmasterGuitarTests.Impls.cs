using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    internal class XboxOneRiffmasterGuitarTests_ReportId
        : RiffmasterGuitarTests<XboxOneRiffmasterGuitar, XboxOneRiffmasterGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override XboxOneRiffmasterGuitarState CreateState()
            => new XboxOneRiffmasterGuitarState()
        {
            reportId = 0x20,
        };
    }

    internal class PS4RiffmasterGuitarTests_ReportId
        : RiffmasterGuitarTests<PS4RiffmasterGuitar, PS4RiffmasterGuitarState_ReportId>
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

        // P1 is only present on PS4/5 Riffmasters
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }

    internal class PS4RiffmasterGuitarTests_NoReportId
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

        // P1 is only present on PS4/5 Riffmasters
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }

    internal class PS5RiffmasterGuitarTests_ReportId
        : RiffmasterGuitarTests<PS5RiffmasterGuitar, PS5RiffmasterGuitarState_ReportId>
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

        // P1 is only present on PS4/5 Riffmasters
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }

    internal class PS5RiffmasterGuitarTests_NoReportId
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

        // P1 is only present on PS4/5 Riffmasters
        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), (ButtonControl)guitar["p1Button"], SetP1);
        });
    }
}