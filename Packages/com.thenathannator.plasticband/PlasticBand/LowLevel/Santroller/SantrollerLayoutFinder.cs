using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;

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
        /// <summary>
        /// Vendor ID for Santroller devices.
        /// </summary>
        public const ushort VendorID = 0x1209;

        /// <summary>
        /// Product ID for Santroller devices.
        /// </summary>
        public const ushort ProductID = 0x2882;

        /// <summary>
        /// Lookup for the default device/rhythm type of an XInput subtype.
        /// </summary>
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

        /// <summary>
        /// Registered layout resolvers for a given subtype.
        /// </summary>
        private static readonly Dictionary<(SantrollerDeviceType, SantrollerRhythmType?), string> s_LayoutOverrides
            = new Dictionary<(SantrollerDeviceType, SantrollerRhythmType?), string>();

        /// <summary>
        /// Initializes the layout resolver.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.onFindLayoutForDevice += FindSantrollerDeviceLayout;
        }

        internal static SantrollerDeviceType GetDeviceType(ushort version)
            => (SantrollerDeviceType)(version >> 8);

        internal static SantrollerRhythmType GetRhythmType(ushort version)
            => (SantrollerRhythmType)((version >> 4) & 0x0F);

        /// <summary>
        /// Determines the layout to use for the given device description.
        /// </summary>
        private static string FindSantrollerDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore non-HID devices
            if (description.interfaceName != HidDefinitions.InterfaceName)
                return null;

            // Parse HID descriptor
            HID.HIDDeviceDescriptor descriptor = HID.HIDDeviceDescriptor.FromJson(description.capabilities);
            if (descriptor.vendorId != VendorID && descriptor.productId != ProductID)
                return null;

            // Parse version
            if (!ushort.TryParse(description.version, out var version))
                return null;

            var deviceType = GetDeviceType(version);
            var rhythmType = GetRhythmType(version);

            // Check if the devicetype and rhythm type has an override registered
            if (s_LayoutOverrides.TryGetValue((deviceType, rhythmType), out var layout))
                return layout;

            // A lot of devices are rhythm type independent, so also match on just the device type
            if (s_LayoutOverrides.TryGetValue((deviceType, null), out layout))
                return layout;

            return null;
        }

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an HID Santroller device using the specified
        /// <see cref="SantrollerDeviceType"/> and <see cref="SantrollerRhythmType"/>.
        /// </summary>
        internal static void RegisterHIDLayout<TDevice>(SantrollerDeviceType deviceType, SantrollerRhythmType? rhythmType = null)
            where TDevice : InputDevice
        {
            // Register to the input system
            InputSystem.RegisterLayout<TDevice>();

            // Ensure no resolver is registered yet
            if (s_LayoutOverrides.ContainsKey((deviceType, rhythmType)))
                throw new ArgumentException($"Device type {deviceType}:{(rhythmType != null ? rhythmType.ToString() : "All")} is already registered!");

            s_LayoutOverrides.Add((deviceType, rhythmType), typeof(TDevice).Name);
        }

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device using the specified
        /// <see cref="XInputController.DeviceSubType"/> and Santroller device/rhythm type.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputController.DeviceSubType subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((int)subType, deviceType, rhythmType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device using the specified
        /// <see cref="XInputNonStandardSubType"/> and Santroller device/rhythm type.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputNonStandardSubType subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((int)subType, deviceType, rhythmType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device using the specified
        /// XInput subtype and Santroller device/rhythm type.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(int subType,
            SantrollerDeviceType deviceType = SantrollerDeviceType.Unknown,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetXInputMatcher(subType, deviceType, rhythmType));
        }

        /// <summary>
        /// Gets a matcher that matches XInput Santroller devices with the given device type and rhythm type.
        /// </summary>
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