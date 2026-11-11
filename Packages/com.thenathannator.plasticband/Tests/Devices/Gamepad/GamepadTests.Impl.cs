using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputControllerUniversalTests
        : GamepadTests<XInputControllerUniversal, XInputGamepad>
    {
        protected override XInputGamepad CreateState()
            => new XInputGamepad();

        protected override void CheckExpectedDescriptions()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_WSA // See comment in XInputControllerUniversal.Initialize()
            NUnit.Framework.Assert.Pass("XInputControllerUniversal is not used for matching on Windows");
#else
            // This should be the fallback for any XInput devices not matched by other layouts
            AssertDescriptionMatches(new UnityEngine.InputSystem.Layouts.InputDeviceDescription()
            {
                interfaceName = "XInput",
            });

            AssertXInputMatch(XInputDeviceSubType.Unknown);
            AssertXInputMatch(XInputDeviceSubType.Gamepad);
#endif
        }
    }

    internal class AndroidGamepadTests_NoReportId
        : GamepadTests<AndroidGamepad_NoReportId, AndroidGamepadState_NoReportId>
    {
        protected override AndroidGamepadState_NoReportId CreateState()
            => new AndroidGamepadState_NoReportId()
        {
            dpad = HidDpad.Neutral,
            leftStickX = 0,
            leftStickY = 0,
            rightStickX = 0,
            rightStickY = 0,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x20BC, 0x5500, reportId: false);
        }
    }

    internal class AndroidGamepadTests_ReportId
        : GamepadTests<AndroidGamepad, AndroidGamepadState_ReportId>
    {
        protected override AndroidGamepadState_ReportId CreateState()
            => new AndroidGamepadState_ReportId()
        {
            state = new AndroidGamepadState_NoReportId()
            {
                dpad = HidDpad.Neutral,
                leftStickX = 0,
                leftStickY = 0,
                rightStickX = 0,
                rightStickY = 0,
            },
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x20BC, 0x5500, reportId: true);
        }
    }
}
