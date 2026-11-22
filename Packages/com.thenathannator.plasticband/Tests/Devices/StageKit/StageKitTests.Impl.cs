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

    internal class SantrollerHIDStageKitTests
        : StageKitTests<SantrollerHIDStageKit, SantrollerHIDStageKitState>
    {
        protected override SantrollerHIDStageKitState CreateState()
            => new SantrollerHIDStageKitState()
        {
            dpad = HidDpad.Neutral,
            leftStickX = 0x80,
            leftStickY = 0x80,
            rightStickX = 0x80,
            rightStickY = 0x80,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertSantrollerHidMatch(SantrollerHIDProtocol.V1, SantrollerDeviceType.StageKit);
        }
    }

    internal class SantrollerV2HIDStageKitTests
        : StageKitTests<SantrollerV2HIDStageKit, SantrollerV2HIDGamepadState>
    {
        protected override SantrollerV2HIDGamepadState CreateState()
            => new SantrollerV2HIDGamepadState()
        {
            reportId = 1,
            buttons = SantrollerV2Button.Neutral,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertSantrollerHidMatch(SantrollerHIDProtocol.V2, SantrollerDeviceType.StageKit);
        }
    }
}