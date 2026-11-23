using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.LowLevel
{
    internal enum SantrollerDeviceType : byte
    {
        Unknown = 0,
        Gamepad = 1,
        DancePad = 2,
        GuitarHeroGuitar = 3,
        RockBandGuitar = 4,
        GuitarHeroDrums = 5,
        RockBandDrums = 6,
        LiveGuitar = 7,
        DjHeroTurntable = 8,
        StageKit = 9,
        ProKeys = 16,
        ProGuitarMustang = 17,
        ProGuitarSquier = 18,
        KeyboardMouse = 19,
        FightStick = 20,
        FlightStick = 21,
        PopNMusic = 22,
        DJMax = 23,
        Taiko = 24,
        PowerGigDrum = 25,
        Microphone = 32,
        DisneyInfinity = 33,
        Skylanders = 34,
        LegoDimensions = 35,
        ProjectDiva = 36,
        GuitarFreaks = 37,
        Wheel = 38,
        PowerGigGuitar = 39,
        RockRevolutionGuitar = 40,
        Midi = 41,
        BeatMania = 48,
        DrumMania = 49,
        KeyboardMania = 50,
    }

    [Flags]
    internal enum SantrollerCapabilities : byte
    {
        None = 0,
        StandardPlayerLeds = 0x01,
        JoystickRgb = 0x02,
        InstrumentLed = 0x04,
        Rumble = 0x08,
        DpadAsButtons = 0x10,
        YAxisInverted = 0x20,
    }

    internal enum SantrollerHIDProtocol
    {
        V1 = 1,
        V2 = 2,
    }

    [Serializable]
    internal struct SantrollerHIDCapabilities
    {
        public SantrollerHIDProtocol protocol;
        public SantrollerDeviceType subType;
        public SantrollerCapabilities capabilities;
    }

    /// <summary>
    /// Registers and resolves layouts for Santroller devices.
    /// </summary>
    internal static class SantrollerLayoutFinder
    {
        public const string InterfaceName = "SantrollerHID";
        public const int VendorID = 0x1209;
        public const int ProductID = 0x2882;
        public const int ProductIDV2 = 0x2883;

        /// <summary>
        /// Determines whether the given capabilities match the target layout.
        /// If it does, returns a value of 0 or greater which is added to the overall match score.
        /// If it doesn't, returns a value below 0 to skip the layout.
        /// </summary>
        internal delegate float SantrollerHIDLayoutMatchPredicate(SantrollerHIDCapabilities capabilities);

        private class SantrollerHIDLayoutMatcher
        {
            public SantrollerHIDLayoutMatchPredicate predicate;
            public InputDeviceMatcher matcher;
            public string layoutName;
        }

        private static readonly Dictionary<(SantrollerHIDProtocol, SantrollerDeviceType), List<SantrollerHIDLayoutMatcher>> s_HIDLayoutMatchers
            = new Dictionary<(SantrollerHIDProtocol, SantrollerDeviceType), List<SantrollerHIDLayoutMatcher>>();

        internal static void Initialize()
        {
            // Ensure no layouts have persisted across a domain reload
            s_HIDLayoutMatchers.Clear();

            InputSystem.onFindLayoutForDevice += FindHIDDeviceLayout;
        }

        internal static short GetRevisionValue(SantrollerDeviceType deviceType)
            => (short)(((int)deviceType & 0xFF) << 8);

        internal static void RegisterXInputLayout<TDevice>(XInputDeviceSubType subType,
            SantrollerDeviceType deviceType)
            where TDevice : InputDevice
        {
            XInputLayoutFinder.RegisterLayout<TDevice>(subType, VendorID, ProductID, GetRevisionValue(deviceType));
            XInputLayoutFinder.RegisterLayout<TDevice>(subType, VendorID, ProductIDV2, GetRevisionValue(deviceType));
        }

        private static string FindHIDDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore anything not created by the Santroller HID adapter
            if (description.interfaceName != InterfaceName)
            {
                return null;
            }

            // Get matchers
            if (!Utilities.TryParseJson<SantrollerHIDCapabilities>(description.capabilities, out var capabilities) ||
                !s_HIDLayoutMatchers.TryGetValue((capabilities.protocol, capabilities.subType), out var matchers))
            {
                return null;
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

            return foundMatch;
        }

        private static void RegisterHIDMatcher<TDevice>(SantrollerHIDProtocol protocol, SantrollerDeviceType deviceType,
            SantrollerHIDLayoutMatchPredicate predicate)
            where TDevice : InputDevice
        {
            var key = (protocol, deviceType);
            if (!s_HIDLayoutMatchers.TryGetValue(key, out var matchers))
            {
                matchers = new List<SantrollerHIDLayoutMatcher>();
                s_HIDLayoutMatchers.Add(key, matchers);
            }

            matchers.Add(new SantrollerHIDLayoutMatcher()
            {
                predicate = predicate,
                matcher = GetHIDMatcher(protocol, deviceType),
                layoutName = typeof(TDevice).Name
            });
        }

        internal static void RegisterHIDLayout<TDevice>(SantrollerHIDProtocol protocol, SantrollerDeviceType deviceType)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetHIDMatcher(protocol, deviceType));
            RegisterHIDMatcher<TDevice>(protocol, deviceType, (_) => 0);
        }

        internal static void RegisterHIDLayout<TDevice>(SantrollerHIDProtocol protocol, SantrollerDeviceType deviceType,
            SantrollerHIDLayoutMatchPredicate predicate)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>();
            RegisterHIDMatcher<TDevice>(protocol, deviceType, predicate);
        }

        private static InputDeviceMatcher GetHIDMatcher(SantrollerHIDProtocol protocol, SantrollerDeviceType deviceType)
        {
            return new InputDeviceMatcher()
                // `int` cast is required for the input system's JSON parser to match these values correctly
                .WithInterface(InterfaceName)
                .WithCapability("protocol", (int)protocol)
                .WithCapability("subType", (int)deviceType);
        }

        internal static InputDeviceDescription GetHIDDescription(InputDeviceDescription adapter,
            SantrollerHIDProtocol protocol, SantrollerDeviceType deviceType,
            SantrollerCapabilities capabilities = SantrollerCapabilities.None)
        {
            var description = adapter;
            description.interfaceName = InterfaceName;
            description.capabilities = JsonUtility.ToJson(new SantrollerHIDCapabilities()
            {
                protocol = protocol,
                subType = deviceType,
                capabilities = capabilities,
            });
            return description;
        }
    }
}