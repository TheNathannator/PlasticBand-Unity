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
    /// Registers layouts for XInput devices, and performs fixups for devices that require state information to determine the true type.
    /// </summary>
    internal static class XInputLayoutFinder
    {
        /// <summary>
        /// The interface name used for XInput devices.
        /// </summary>
        public const string InterfaceName = "XInput";

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        /// <summary>
        /// An XInput state packet.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
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
#endif

        /// <summary>
        /// Determines whether or not an override should apply to a device.
        /// </summary>
        internal delegate bool XInputOverrideMatch(XInputCapabilities capabilities, XInputGamepad state);

        /// <summary>
        /// Resolves the layout of an XInput device.
        /// </summary>
        private struct XInputLayoutOverride
        {
            public DeviceSubType subType;
            public XInputOverrideMatch resolve;
            public string layoutName;
        }

        /// <summary>
        /// Registered layout resolvers for a given subtype.
        /// </summary>
        private static readonly List<XInputLayoutOverride> s_LayoutOverrides = new List<XInputLayoutOverride>();

        /// <summary>
        /// Initializes the layout resolver.
        /// </summary>
        internal static void Initialize()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.onFindLayoutForDevice += FindXInputDeviceLayout;
#endif
        }

        /// <summary>
        /// Determines the layout to use for the given device description.
        /// </summary>
        private static string FindXInputDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // Ignore non-XInput devices
            if (description.interfaceName != InterfaceName)
                return null;

            // Parse capabilities
            if (!Utilities.TryParseJson<XInputCapabilities>(description.capabilities, out var capabilities))
            {
                // Default to regular controller if no layout was matched already
                if (string.IsNullOrEmpty(matchedLayout))
                    return nameof(XInputControllerWindows);

                return null;
            }

            // Check if the subtype has an override registered
            int index = s_LayoutOverrides.FindIndex((entry) => entry.subType == capabilities.subType);
            if (index >= 0)
            {
                var entry = s_LayoutOverrides[index];

                int result = XInputGetState(capabilities.userIndex, out var state);
                if (result != 0)
                    return null;

                if (entry.resolve(capabilities, state.gamepad) && !string.IsNullOrEmpty(entry.layoutName))
                    return entry.layoutName;
            }

            // Don't change the existing layout if no override was specified
            if (!string.IsNullOrEmpty(matchedLayout))
                return null;

            // Set all other subtypes to be regular controllers, per XInput specs
            return nameof(XInputControllerWindows);
#else
            return null;
#endif
        }

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified
        /// <see cref="DeviceSubType"/>, with a layout resolver used to identify it.
        /// </summary>
        internal static void RegisterLayout<TDevice>(DeviceSubType subType, XInputOverrideMatch resolveLayout)
            where TDevice : InputDevice
        {
            // Register to the input system
            InputSystem.RegisterLayout<TDevice>();

            // Add to override list
            s_LayoutOverrides.Add(new XInputLayoutOverride()
            {
                subType = subType,
                resolve = resolveLayout,
                layoutName = typeof(TDevice).Name
            });
        }

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified
        /// <see cref="DeviceSubType"/>.
        /// </summary>
        internal static void RegisterLayout<TDevice>(DeviceSubType subType)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified
        /// <see cref="XInputNonStandardSubType"/>.
        /// </summary>
        internal static void RegisterLayout<TDevice>(XInputNonStandardSubType subType)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified subtype.
        /// </summary>
        internal static void RegisterLayout<TDevice>(int subType)
            where TDevice : InputDevice
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<TDevice>(matches: GetMatcher(subType));
#endif
        }

        /// <summary>
        /// Gets a matcher that matches XInput Santroller devices with the given subtype.
        /// </summary>
        internal static InputDeviceMatcher GetMatcher(int subType)
        {
            return new InputDeviceMatcher()
                .WithInterface(InterfaceName)
                .WithCapability("subType", subType);
        }
    }
}