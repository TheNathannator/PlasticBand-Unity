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
        GuitarHero = 0,
        RockBand
    }

    /// <summary>
    /// Performs layout fixups for Santroller devices that require state information to determine the true type.
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

        // For reference
        // internal static SantrollerConsoleType GetConsoleType(ushort version)
        //     => (SantrollerConsoleType)(version & 0x0F);

        /// <summary>
        /// Determines the layout to use for the given device description.
        /// </summary>
        private static string FindSantrollerDeviceLayout(ref InputDeviceDescription description, string matchedLayout,
            InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore non-HID devices
            if (description.interfaceName != "HID")
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
        /// <see cref="XInputController.DeviceSubType"/>.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputController.DeviceSubType subType)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device using the specified
        /// <see cref="XInputNonStandardSubType"/>.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputNonStandardSubType subType)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device using the specified subtype.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(int subType)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetXInputMatcher(subType));
        }

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device using the specified device type and rhythm type.
        /// </summary>
        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(SantrollerDeviceType deviceType, SantrollerRhythmType rhythmType)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetXInputMatcher(deviceType, rhythmType));
        }

        /// <summary>
        /// Gets a matcher that matches XInput Santroller devices with the given subtype.
        /// </summary>
        internal static InputDeviceMatcher GetXInputMatcher(int subType)
        {
            return new InputDeviceMatcher()
                .WithInterface(XInputLayoutFinder.InterfaceName)
                .WithCapability("subType", subType)
                .WithCapability("gamepad/leftStickX", (int)VendorID)
                .WithCapability("gamepad/leftStickY", (int)ProductID);
        }
        
        /// <summary>
        /// Gets a matcher that matches XInput Santroller devices with the given device type and rhythm type.
        /// </summary>
        internal static InputDeviceMatcher GetXInputMatcher(SantrollerDeviceType deviceType, SantrollerRhythmType rhythmType)
        {
            int revision = (((int)deviceType) << 8) | (((int)rhythmType) << 4);
            return new InputDeviceMatcher()
                .WithInterface(XInputLayoutFinder.InterfaceName)
                .WithCapability("gamepad/leftStickX", (int)VendorID)
                .WithCapability("gamepad/leftStickY", (int)ProductID)
                .WithCapability("gamepad/rightStickX", revision);
        }
    }
}