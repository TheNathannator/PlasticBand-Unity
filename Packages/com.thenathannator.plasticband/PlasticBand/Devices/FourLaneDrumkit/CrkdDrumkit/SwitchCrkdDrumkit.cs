using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    // TODO: Implement actual device layout
    internal static class SwitchCrkdDrumkit
    {
        internal static void Initialize()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_WSA
            // Stage Tour Drumkit for Switch 2
            // TODO: Proper layout
            InputSystem.RegisterLayoutMatcher<UnityEngine.InputSystem.Switch.SwitchProControllerHID>(new InputDeviceMatcher()
                .WithInterface("HID")
                .WithProduct("Pro Controller")
                .WithCapability("vendorId", 0x3958)
                .WithCapability("productId", 0xD900)
            );
#endif
        }
    }
}
