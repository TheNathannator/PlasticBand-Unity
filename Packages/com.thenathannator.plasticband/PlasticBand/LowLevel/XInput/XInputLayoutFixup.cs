using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.LowLevel
{
    using static XInputController;

    /// <summary>
    /// Performs layout fixups for XInput devices that require state information to determine the true type.
    /// </summary>
    internal static class XInputLayoutFixup
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        /// <summary>
        /// An XInput state packet.
        /// </summary>
        private struct XInputState
        {
            public uint packetCount;
            public XInputGamepad gamepad;
        }

        /// <summary>
        /// Gets the current XInput state for the given user index.
        /// </summary>
        // 9_1_0 is used for best compatibility across all versions of Windows
        // Nothing important in the new versions that requires their use
        [DllImport("xinput9_1_0.dll")]
        private static extern int XInputGetState(
            int UserIndex, // DWORD
            out XInputState State // XINPUT_STATE*
        );

        /// <summary>
        /// Resolves the layout of an XInput device.
        /// </summary>
        internal delegate string XInputLayoutResolver(XInputCapabilities capabilities, XInputGamepad state);

        /// <summary>
        /// Registered layout resolvers for a given subtype.
        /// </summary>
        private static readonly Dictionary<DeviceSubType, XInputLayoutResolver> s_SubTypeLayoutOverrideMap = new Dictionary<DeviceSubType, XInputLayoutResolver>();

        /// <summary>
        /// Initializes the layout resolver.
        /// </summary>
        internal static void Initialize()
        {
            // Replace XInputControllerWindows layout matcher with one that only matches gamepads
            InputSystem.RemoveLayout(typeof(XInputControllerWindows).Name);
            InputSystem.RegisterLayout<XInputControllerWindows>(matches: new InputDeviceMatcher()
                .WithInterface(XInputOther.kInterfaceName)
                .WithCapability("subType", (int)DeviceSubType.Gamepad)
            );
            InputSystem.onFindLayoutForDevice += FindXInputDeviceLayout;
        }

        /// <summary>
        /// Registers a layout resolver for a subtype.
        /// </summary>
        internal static void RegisterLayoutResolver(DeviceSubType subType, XInputLayoutResolver resolveLayout)
        {
            // TODO: May be something better to do than just do nothing in this case
            if (s_SubTypeLayoutOverrideMap.ContainsKey(subType))
                return;

            s_SubTypeLayoutOverrideMap.Add(subType, resolveLayout);
        }

        /// <summary>
        /// Determines the layout to use for the given device description.
        /// </summary>
        internal static string FindXInputDeviceLayout(ref InputDeviceDescription description, string matchedLayout, InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore non-XInput devices
            if (description.interfaceName != XInputOther.kInterfaceName)
                return null;

            // Parse capabilities
            if (!Utilities.TryParseJson<XInputCapabilities>(description.capabilities, out var capabilities))
            {
                // Default to regular controller if no layout was matched already
                if (string.IsNullOrEmpty(matchedLayout))
                    return typeof(XInputControllerWindows).Name;

                return null;
            }

            // Check if the subtype has an override registered
            if (s_SubTypeLayoutOverrideMap.TryGetValue(capabilities.subType, out var resolveLayout))
            {
                int result = XInputGetState(capabilities.userIndex, out var state);
                if (result != 0)
                    return null;

                string layoutName = resolveLayout(capabilities, state.gamepad);
                if (layoutName != null)
                    return layoutName;
            }

            // Don't change the existing layout if no override was specified but it's not already set to a standard controller
            if (!string.IsNullOrEmpty(matchedLayout) && matchedLayout != typeof(XInputControllerWindows).Name)
                return null;

            // Set all other subtypes to be regular controllers, per XInput specs
            return typeof(XInputControllerWindows).Name;
        }
#endif
    }
}