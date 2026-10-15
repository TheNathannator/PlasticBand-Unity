using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputStageKitTests
        : StageKitTests<XInputStageKit, XInputGamepad>
    {
        protected override XInputGamepad CreateState()
            => new XInputGamepad();

        protected override void CheckExpectedDescriptions()
        {
            // PDP Rock Band Stage Kit for Xbox 360
            AssertXInputMatch(XInputDeviceSubType.StageKit);
        }
    }

    internal class SantrollerXInputStageKitTests
        : StageKitTests<SantrollerXInputStageKit, XInputGamepad>
    {
        protected override XInputGamepad CreateState()
            => new XInputGamepad();

        protected override void CheckExpectedDescriptions()
        {
            AssertXInputMatch(
                XInputDeviceSubType.StageKit,
                vendorId: 0x1209,
                productId: 0x2882,
                revision: 0x0900
            );
        }
    }

    internal class SantrollerHidStageKitTests
        : StageKitTests<SantrollerHidStageKit, SantrollerHidStageKitState>
    {
        protected override SantrollerHidStageKitState CreateState()
            => new SantrollerHidStageKitState()
        {
            dpad = HidDpad.Neutral,
            leftStickX = 0x80,
            leftStickY = 0x80,
            rightStickX = 0x80,
            rightStickY = 0x80,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x1209, 0x2882, revision: 0x0900, reportId: true);
        }
    }

    internal class SantrollerV2HidStageKitTests
        : StageKitTests<SantrollerV2HidStageKit, SantrollerV2HidStageKitState>
    {
        protected override SantrollerV2HidStageKitState CreateState()
            => new SantrollerV2HidStageKitState()
        {
            reportId = 1,
            buttons1 = 8,
        };

        protected override void CheckExpectedDescriptions()
        {
        }
    }
}