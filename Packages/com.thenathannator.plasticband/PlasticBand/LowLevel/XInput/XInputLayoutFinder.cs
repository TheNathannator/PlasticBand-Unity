using System.Collections.Generic;
using System.Linq;
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
        /// Determines whether or not a device matches an override entry.
        /// </summary>
        internal delegate bool XInputOverrideDetermineMatch(XInputCapabilities capabilities, XInputGamepad state);

        /// <summary>
        /// Data used for overriding the layout of an XInput device.
        /// </summary>
        private class XInputLayoutOverride
        {
            public int subType;
            public XInputOverrideDetermineMatch resolve;
            public InputDeviceMatcher matcher;
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
            // Ignore non-XInput devices
            if (description.interfaceName != InterfaceName)
                return null;

            // Parse capabilities
            if (!Utilities.TryParseJson<XInputCapabilities>(description.capabilities, out var capabilities))
                return DefaultLayoutIfNull(matchedLayout);

            // Check if the subtype has an override registered
            var overrides = s_LayoutOverrides.Where((entry) => entry.subType == (int)capabilities.subType);
            if (!overrides.Any())
                return DefaultLayoutIfNull(matchedLayout);

            // Get device state
            XInputGamepad state = default;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (XInputGetState(capabilities.userIndex, out var packet) != 0)
                return DefaultLayoutIfNull(matchedLayout);

            state = packet.gamepad;
#endif

            // Go through device matchers
            XInputLayoutOverride matchedEntry = null;
            float greatestMatch = 0f;
            foreach (var entry in overrides)
            {
                // Ignore invalid overrides and non-matching resolvers
                if (string.IsNullOrEmpty(entry.layoutName) && !entry.resolve(capabilities, state))
                    continue;

                // Keep track of the best match
                float match = entry.matcher.MatchPercentage(description);
                if (match > greatestMatch)
                {
                    greatestMatch = match;
                    matchedEntry = entry;
                }
            }

            // Use matched entry if available
            if (matchedEntry != null && !string.IsNullOrEmpty(matchedEntry.layoutName))
                return matchedEntry.layoutName;

            // Use existing or default layout otherwise
            return DefaultLayoutIfNull(matchedLayout);
        }

        private static string DefaultLayoutIfNull(string matchedLayout)
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // XInputControllerWindows is only available when building for Windows
            => string.IsNullOrEmpty(matchedLayout) ? nameof(XInputControllerWindows) : null;
#else
            => null;
#endif

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified
        /// <see cref="DeviceSubType"/>, with a layout resolver used to identify it.
        /// </summary>
        internal static void RegisterLayout<TDevice>(DeviceSubType subType, XInputOverrideDetermineMatch resolveLayout,
            InputDeviceMatcher matcher = default)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType, resolveLayout, matcher);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified
        /// <see cref="XInputNonStandardSubType"/>, with a layout resolver used to identify it.
        /// </summary>
        internal static void RegisterLayout<TDevice>(XInputNonStandardSubType subType, XInputOverrideDetermineMatch resolveLayout,
            InputDeviceMatcher matcher = default)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType, resolveLayout, matcher);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput device using the specified
        /// subtype, with a layout resolver used to identify it.
        /// </summary>
        internal static void RegisterLayout<TDevice>(int subType, XInputOverrideDetermineMatch resolveLayout,
            InputDeviceMatcher matcher = default)
            where TDevice : InputDevice
        {
            // Register to the input system
            InputSystem.RegisterLayout<TDevice>();

            // Add to override list
            s_LayoutOverrides.Add(new XInputLayoutOverride()
            {
                subType = subType,
                resolve = resolveLayout,
                matcher = matcher.empty ? GetMatcher(subType) : matcher,
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