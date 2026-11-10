using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    internal static class SwitchProControllerPatches
    {
        internal static void Initialize()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_WSA
            // Some Switch Pro Controllers have their hardware IDs set to 0 and are only identifiable by name
            // Specific examples include:
            // - "Lic Pro Controller"
            // - "Nintendo Wireless Gamepad"
            // To be as broad as possible, we match against anything ending in "Pro Controller" or "Wireless Gamepad"
            InputSystem.RegisterLayoutMatcher<UnityEngine.InputSystem.Switch.SwitchProControllerHID>(new InputDeviceMatcher()
                .WithInterface("HID")
                .WithProduct(".*Pro Controller")
                .WithCapability("vendorId", 0x0000)
                .WithCapability("productId", 0x0000)
            );
            InputSystem.RegisterLayoutMatcher<UnityEngine.InputSystem.Switch.SwitchProControllerHID>(new InputDeviceMatcher()
                .WithInterface("HID")
                .WithProduct(".*Wireless Gamepad")
                .WithCapability("vendorId", 0x0000)
                .WithCapability("productId", 0x0000)
            );
#endif
        }
    }
}