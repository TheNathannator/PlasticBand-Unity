using System.Collections.Generic;
using System.Diagnostics;
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
        public const string InterfaceName = "XInput";

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct XInputState
        {
            public uint packetCount;
            public XInputGamepad gamepad;
        }

        // 9_1_0 is used for best compatibility across all versions of Windows
        // Nothing important in the new versions that requires their use
        [DllImport("xinput9_1_0.dll")]
        private static extern int XInputGetState(
            int UserIndex, // DWORD
            out XInputState State // XINPUT_STATE*
        );
#endif

        // Layout resolution info
        internal delegate bool XInputOverrideDetermineMatch(XInputCapabilities capabilities, XInputGamepad state);
        private class XInputLayoutOverride
        {
            public int subType;
            public XInputOverrideDetermineMatch resolve;
            public InputDeviceMatcher matcher;
            public string layoutName;
        }

        // Registered layout resolvers
        private static readonly List<XInputLayoutOverride> s_LayoutOverrides = new List<XInputLayoutOverride>();

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void Initialize()
        {
            // Ensure no layouts have persisted across a domain reload
            s_LayoutOverrides.Clear();

            // Register layout finder
            InputSystem.onFindLayoutForDevice += FindXInputDeviceLayout;
        }

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

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterLayout<TDevice>(DeviceSubType subType, XInputOverrideDetermineMatch resolveLayout,
            InputDeviceMatcher matcher = default)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType, resolveLayout, matcher);

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterLayout<TDevice>(XInputNonStandardSubType subType, XInputOverrideDetermineMatch resolveLayout,
            InputDeviceMatcher matcher = default)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType, resolveLayout, matcher);

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
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

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterLayout<TDevice>(DeviceSubType subType)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType);

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterLayout<TDevice>(XInputNonStandardSubType subType)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((int)subType);

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterLayout<TDevice>(int subType)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetMatcher(subType));
        }

        internal static InputDeviceMatcher GetMatcher(int subType)
        {
            return new InputDeviceMatcher()
                .WithInterface(InterfaceName)
                .WithCapability("subType", subType);
        }
    }
}