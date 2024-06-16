using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.LowLevel
{
    internal static class GameInputLayoutFinder
    {
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterLayout<TDevice>(ushort vendorId, ushort productId)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetMatcher(vendorId, productId));
        }

        internal static InputDeviceMatcher GetMatcher(int vendorId, int productId)
        {
            return new InputDeviceMatcher()
                .WithInterface(GameInputDefinitions.InterfaceName)
                .WithCapability("vendorId", vendorId)
                .WithCapability("productId", productId);
        }
    }
}