using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;

using Debug = UnityEngine.Debug;

namespace PlasticBand.LowLevel
{
    internal enum SantrollerDeviceType
    {
        Unknown = 0,

        Gamepad = 0x01,
        Wheel = 0x02,
        ArcadeStick = 0x03,
        FlightStick = 0x04,
        DancePad = 0x05,
        ArcadePad = 0x06,
        Guitar = 0x07,
        LiveGuitar = 0x08,
        Drums = 0x09,
        DjHeroTurntable = 0x10,
        StageKit = 0x11,
    }

    internal enum SantrollerRhythmType
    {
        None = 0,

        // Guitar/drumkit
        GuitarHero = 0,
        RockBand = 1,
    }

    /// <summary>
    /// Registers and resolves layouts for Santroller devices.
    /// </summary>
    internal static class SantrollerLayoutFinder
    {
        // Dummy device to ensure the HID layout finder doesn't get to Santroller devices
        // first and make the layout finding process ignore any layouts we might provide for them
        private class SantrollerHidDevice : InputDevice { }

        public const ushort VendorID = 0x1209;
        public const ushort ProductID = 0x2882;

        // Fall back to regular HID layout finder for devices without explicit layouts
        private static readonly InputDeviceFindControlLayoutDelegate s_HidLayoutFinder = (InputDeviceFindControlLayoutDelegate)typeof(HID)
            .GetMethod("OnFindLayoutForDevice", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, new Type[]
                { typeof(InputDeviceDescription).MakeByRefType(), typeof(string), typeof(InputDeviceExecuteCommandDelegate) }, null)
            .CreateDelegate(typeof(InputDeviceFindControlLayoutDelegate));

        // Default device/rhythm types for XInput subtypes
        private static readonly Dictionary<int, (SantrollerDeviceType type, SantrollerRhythmType rhythm)> s_XInputSubtypeToDeviceType
            = new Dictionary<int, (SantrollerDeviceType type, SantrollerRhythmType rhythm)>()
        {
            { (int)XInputController.DeviceSubType.Gamepad,         (SantrollerDeviceType.Gamepad, SantrollerRhythmType.None) },
            { (int)XInputController.DeviceSubType.Wheel,           (SantrollerDeviceType.Wheel, SantrollerRhythmType.None) },
            { (int)XInputController.DeviceSubType.ArcadeStick,     (SantrollerDeviceType.ArcadeStick, SantrollerRhythmType.None) },
            { (int)XInputController.DeviceSubType.FlightStick,     (SantrollerDeviceType.FlightStick, SantrollerRhythmType.None) },
            { (int)XInputController.DeviceSubType.DancePad,        (SantrollerDeviceType.DancePad, SantrollerRhythmType.None) },
            { (int)XInputController.DeviceSubType.ArcadePad,       (SantrollerDeviceType.ArcadePad, SantrollerRhythmType.None) },
            { (int)XInputController.DeviceSubType.Guitar,          (SantrollerDeviceType.Guitar, SantrollerRhythmType.RockBand) },
            { (int)XInputController.DeviceSubType.GuitarAlternate, (SantrollerDeviceType.Guitar, SantrollerRhythmType.GuitarHero) },
            { (int)XInputController.DeviceSubType.DrumKit,         (SantrollerDeviceType.Drums, SantrollerRhythmType.RockBand) },
            { (int)XInputNonStandardSubType.Turntable,             (SantrollerDeviceType.DjHeroTurntable, SantrollerRhythmType.None) },
            { (int)XInputNonStandardSubType.StageKit,              (SantrollerDeviceType.StageKit, SantrollerRhythmType.None) },
        };

        // Registered layouts
        private static readonly Dictionary<(SantrollerDeviceType, SantrollerRhythmType?), string> s_AvailableHidLayouts
            = new Dictionary<(SantrollerDeviceType, SantrollerRhythmType?), string>();

        internal static void Initialize()
        {
            // Register dummy device
            InputSystem.RegisterLayout<SantrollerHidDevice>(matches: GetHidMatcher());

            // Ensure no layouts have persisted across a domain reload
            s_AvailableHidLayouts.Clear();

            // Register layout finder
            InputSystem.onFindLayoutForDevice += FindSantrollerDeviceLayout;
        }

        internal static SantrollerDeviceType GetDeviceType(ushort version)
            => (SantrollerDeviceType)(version >> 8);

        internal static SantrollerRhythmType GetRhythmType(ushort version)
            => (SantrollerRhythmType)((version >> 4) & 0x0F);

        private static string FindSantrollerDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore non-HID devices
            if (description.interfaceName != HidDefinitions.InterfaceName)
                return null;

            // If another layout resolver got to the device first, there's nothing we can do
            if (!string.IsNullOrEmpty(matchedLayout) && matchedLayout != nameof(SantrollerHidDevice))
                return null;

            // Parse version
            if (!ushort.TryParse(description.version, out var version))
                return null;

            var deviceType = GetDeviceType(version);
            var rhythmType = GetRhythmType(version);

            // Check if the devicetype and rhythm type has an override registered
            if (s_AvailableHidLayouts.TryGetValue((deviceType, rhythmType), out var layout))
                return layout;

            // A lot of devices are rhythm type independent, so also match on just the device type
            if (s_AvailableHidLayouts.TryGetValue((deviceType, null), out layout))
                return layout;

            // We don't have a specific layout registered, fall back to the regular HID layout resolver
            return s_HidLayoutFinder(ref description, null, executeDeviceCommand);
        }

        internal static void RegisterHIDLayout<TDevice>(SantrollerDeviceType deviceType, SantrollerRhythmType? rhythmType = null)
            where TDevice : InputDevice
        {
            // Register to the input system
            InputSystem.RegisterLayout<TDevice>();

            // Ensure no resolver is registered yet
            if (s_AvailableHidLayouts.ContainsKey((deviceType, rhythmType)))
            {
                Debug.LogError($"Device type {deviceType}:{(rhythmType != null ? rhythmType.ToString() : "All")} is already registered!");
                return;
            }

            s_AvailableHidLayouts.Add((deviceType, rhythmType), typeof(TDevice).Name);
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputController.DeviceSubType subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((int)subType, deviceType, rhythmType);

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputNonStandardSubType subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((int)subType, deviceType, rhythmType);

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(int subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetXInputMatcher(subType, deviceType, rhythmType));
        }

        internal static InputDeviceMatcher GetHidMatcher()
        {
            return HidLayoutFinder.GetMatcher(VendorID, ProductID);
        }

        internal static InputDeviceMatcher GetXInputMatcher(int subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
        {
            if (deviceType == SantrollerDeviceType.Unknown)
                (deviceType, rhythmType) = s_XInputSubtypeToDeviceType[subType];
            int revision = (((int)deviceType) << 8) | (((int)rhythmType) << 4);

            return new InputDeviceMatcher()
                .WithInterface(XInputLayoutFinder.InterfaceName)
                .WithCapability("subType", subType)
                .WithCapability("gamepad/leftStickX", (int)VendorID)
                .WithCapability("gamepad/leftStickY", (int)ProductID)
                .WithCapability("gamepad/rightStickX", revision);
        }
    }
}