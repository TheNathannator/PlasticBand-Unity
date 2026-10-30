using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Tests.Devices
{
    internal class XInputControllerUniversalTests
        : GamepadDeviceTestFixture<XInputControllerUniversal, XInputGamepad>
    {
        protected override XInputGamepad CreateState()
            => new XInputGamepad();

        protected override void CheckExpectedDescriptions()
        {
            // This should be the fallback for any XInput devices not matched by other layouts
            AssertDescriptionMatches(new InputDeviceDescription()
            {
                interfaceName = "XInput",
            });

            AssertXInputMatch(XInputDeviceSubType.Unknown);
            AssertXInputMatch(XInputDeviceSubType.Gamepad);
        }
    }
}
