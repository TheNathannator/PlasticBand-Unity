using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Determines whether the given capabilities match the target layout.
        /// If it does, returns a value of 0 or greater which is added to the overall match score.
        /// If it doesn't, returns a value below 0 to skip the layout.
        /// </summary>
        internal delegate float XInputLayoutMatchPredicate(XInputCapabilities capabilities);

        private class XInputLayoutMatcher
        {
            public XInputLayoutMatchPredicate predicate;
            public InputDeviceMatcher matcher;
            public string layoutName;
        }

        private static readonly Dictionary<DeviceSubType, List<XInputLayoutMatcher>> s_LayoutMatchers
            = new Dictionary<DeviceSubType, List<XInputLayoutMatcher>>();

        private static string s_DefaultLayout
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // XInputControllerWindows is only available when building for Windows
            => nameof(XInputControllerWindows);
#else
            => null;
#endif

        internal static void Initialize()
        {
            // Ensure no layouts have persisted across a domain reload
            s_LayoutMatchers.Clear();

            InputSystem.onFindLayoutForDevice += FindXInputDeviceLayout;
        }

        private static string FindXInputDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore non-XInput devices
            if (description.interfaceName != InterfaceName)
            {
                return null;
            }

            // Get matchers
            if (!Utilities.TryParseJson<XInputCapabilities>(description.capabilities, out var capabilities) ||
                !s_LayoutMatchers.TryGetValue(capabilities.subType, out var matchers))
            {
                return DefaultLayoutIfNull(matchedLayout);
            }

            // Find the greatest match out of all of them
            string foundMatch = null;
            float greatestMatch = 0f;
            foreach (var matcher in matchers)
            {
                float predicateMatch = matcher.predicate(capabilities);
                if (predicateMatch < 0)
                {
                    continue;
                }

                // Keep track of the best match
                float match = matcher.matcher.MatchPercentage(description);
                match += predicateMatch;
                if (match > greatestMatch)
                {
                    greatestMatch = match;
                    foundMatch = matcher.layoutName;
                }
            }

            if (foundMatch != null)
            {
                return foundMatch;
            }

            return DefaultLayoutIfNull(matchedLayout);
        }

        private static string DefaultLayoutIfNull(string matchedLayout)
            => string.IsNullOrEmpty(matchedLayout) ? s_DefaultLayout : null;

        private static void RegisterMatcher<TDevice>(
            DeviceSubType subType,
            XInputLayoutMatchPredicate predicate,
            InputDeviceMatcher matcher
        )
            where TDevice : InputDevice
        {
            if (!s_LayoutMatchers.TryGetValue(subType, out var matchers))
            {
                matchers = new List<XInputLayoutMatcher>();
                s_LayoutMatchers.Add(subType, matchers);
            }

            string layoutName = typeof(TDevice).Name;
            if (!matcher.empty && matchers.Any((entry) => entry.matcher == matcher))
            {
                Logging.Error($"[XInputLayoutFinder] Matcher {matcher} is already registered for subtype {subType}!");
                return;
            }

            matchers.Add(new XInputLayoutMatcher()
            {
                predicate = predicate,
                matcher = matcher.empty ? GetMatcher(subType) : matcher,
                layoutName = layoutName
            });
        }

        /// <summary>
        /// Registers a layout for the given subtype, using the given callback to 
        /// </summary>
        internal static void RegisterLayout<TDevice>(
            DeviceSubType subType,
            XInputLayoutMatchPredicate predicate,
            InputDeviceMatcher matcher = default
        )
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>();
            RegisterMatcher<TDevice>(subType, predicate, matcher);
        }

        private static void RegisterLayout<TDevice>(DeviceSubType subType, InputDeviceMatcher matcher)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: matcher);
            RegisterMatcher<TDevice>(subType, (_) => 0, matcher);
        }

        internal static void RegisterLayout<TDevice>(DeviceSubType subType)
            where TDevice : InputDevice
        {
            RegisterLayout<TDevice>(subType, GetMatcher(subType));
        }

        internal static void RegisterLayout<TDevice>(DeviceSubType subType, short vendorId, short productId)
            where TDevice : InputDevice
        {
            RegisterLayout<TDevice>(subType, GetMatcher(subType, vendorId, productId));
        }

        internal static void RegisterLayout<TDevice>(DeviceSubType subType, short vendorId, short productId, short revision)
            where TDevice : InputDevice
        {
            RegisterLayout<TDevice>(subType, GetMatcher(subType, vendorId, productId, revision));
        }

        internal static void RegisterLayout<TDevice>(
            XInputNonStandardSubType subType,
            XInputLayoutMatchPredicate predicate,
            InputDeviceMatcher matcher = default
        )
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((DeviceSubType)subType, predicate, matcher);

        internal static void RegisterLayout<TDevice>(XInputNonStandardSubType subType)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((DeviceSubType)subType);

        internal static void RegisterLayout<TDevice>(XInputNonStandardSubType subType, short vendorId, short productId)
            where TDevice : InputDevice
            => RegisterLayout<TDevice>((DeviceSubType)subType, vendorId, productId);

        private static InputDeviceMatcher GetMatcher(DeviceSubType subType)
        {
            return new InputDeviceMatcher()
                .WithInterface(InterfaceName)
                .WithCapability("subType", (int)subType);
        }

        private static InputDeviceMatcher GetMatcher(DeviceSubType subType, short vendorId, short productId)
        {
            return GetMatcher(subType)
                // `int` cast is required for the input system's JSON parser to match these values correctly
                // The `short` parameters already ensure proper ranges, so no big deal
                .WithCapability("gamepad/leftStickX", (int)vendorId)
                .WithCapability("gamepad/leftStickY", (int)productId);
        }

        private static InputDeviceMatcher GetMatcher(DeviceSubType subType, short vendorId, short productId, short revision)
        {
            return GetMatcher(subType, vendorId, productId)
                .WithCapability("gamepad/rightStickX", (int)revision);
        }
    }
}