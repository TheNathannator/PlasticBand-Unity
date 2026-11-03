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
}
