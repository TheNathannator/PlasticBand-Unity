using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Re-definition of <c>XInputControllerWindows</c> that is available on all platforms,
    /// for custom backends that re-implement the XInput interface on non-Windows platforms.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGamepad), hideInUI = true)]
    internal class XInputControllerUniversal : XInputController
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<XInputControllerUniversal>();

#if !(UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_WSA)
            // We only register matches on non-Windows platforms. Originally wanted to replace XInputControllerWindows
            // even on Windows, but InputSystem.RemoveLayout does *not* remove layout matchers for some reason...
            InputSystem.RegisterLayoutMatcher<XInputControllerUniversal>(
                new InputDeviceMatcher().WithInterface(XInputLayoutFinder.InterfaceName)
            );
            XInputLayoutFinder.RegisterLayout<XInputControllerUniversal>(XInputDeviceSubType.Gamepad);
#endif
        }
    }
}
