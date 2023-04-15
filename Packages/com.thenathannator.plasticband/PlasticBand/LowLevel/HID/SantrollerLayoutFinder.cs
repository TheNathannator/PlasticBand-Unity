using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.LowLevel
{
    internal enum SantrollerDeviceType {
        Gamepad=1,
        Wheel,
        ArcadeStick,
        FlightStick,
        DancePad,
        ArcadePad,
        Guitar,
        LiveGuitar,
        Drums,
        DjHeroTurntable
    }

    internal enum SantrollerRhythmType {
        GuitarHero,
        RockBand
    }

    /// <summary>
    /// Performs layout fixups for Santroller devices that require state information to determine the true type.
    /// </summary>
    internal static class SantrollerLayoutFinder
    {
              


        /// <summary>
        /// Registered layout resolvers for a given subtype.
        /// </summary>
        private static readonly Dictionary<(SantrollerDeviceType, SantrollerRhythmType?), string> s_DeviceTypeLayoutOverrideMap = new Dictionary<(SantrollerDeviceType, SantrollerRhythmType?), string>();

        /// <summary>
        /// Initializes the layout resolver.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.onFindLayoutForDevice += FindSantrollerDeviceLayout;
        }

        /// <summary>
        /// Registers a layout resolver for a subtype.
        /// </summary>
        internal static void RegisterLayout(SantrollerDeviceType deviceType, SantrollerRhythmType? rhythmType, string layout)
        {
            // TODO: May be something better to do than just do nothing in this case
            if (s_DeviceTypeLayoutOverrideMap.ContainsKey((deviceType, rhythmType)))
                return;

            s_DeviceTypeLayoutOverrideMap.Add((deviceType, rhythmType), layout);
        }

        /// <summary>
        /// Determines the layout to use for the given device description.
        /// </summary>
        internal static string FindSantrollerDeviceLayout(ref InputDeviceDescription description, string matchedLayout, InputDeviceExecuteCommandDelegate executeDeviceCommand)
        {
            // Ignore non-HID devices
            if (description.interfaceName != "HID")
                return null;

            // Parse HID descriptor
            HID.HIDDeviceDescriptor descriptor = HID.HIDDeviceDescriptor.FromJson(description.capabilities);

            if (descriptor.vendorId != 0x1209 && descriptor.productId != 0x2882)
                return null;

            // Parse version
            if (!short.TryParse(description.version, out var version)) {
                return null;
            }
            var major = version >> 8;
            var minor = version >> 4 & 0x0f;
            var revision = version & 0x0f;
            var deviceType = (SantrollerDeviceType)major;
            var rhythmType = (SantrollerRhythmType)minor;
            // Check if the devicetype and rhythm type has an override registered
            if (s_DeviceTypeLayoutOverrideMap.TryGetValue((deviceType, rhythmType), out var layout))
            {
                return layout;
            }

            // A lot of devices are rhythm type independant, so also match on just the device type
            if (s_DeviceTypeLayoutOverrideMap.TryGetValue((deviceType, null), out layout))
            {
                return layout;
            }

            return null;
        }

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device of the specified <see cref="XInputController.DeviceSubType"/>.
        /// </summary>
        public static void Register<TDevice>(XInputController.DeviceSubType subType)
            where TDevice : InputDevice
            => Register<TDevice>((int)subType);

        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device of the specified <see cref="XInputNonStandardSubType"/>.
        /// </summary>
        public static void Register<TDevice>(XInputNonStandardSubType subType)
            where TDevice : InputDevice
            => Register<TDevice>((int)subType);


        /// <summary>
        /// Registers <typeparamref name="TDevice"/> to the input system as an XInput Santroller device of the specified subtype.
        /// </summary>
        public static void Register<TDevice>(int subType)
            where TDevice : InputDevice
        {
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<TDevice>(matches: new InputDeviceMatcher()
                .WithInterface(XInputOther.kInterfaceName)
                .WithCapability("subType", subType)
                .WithCapability("leftStickX", 0x1209)
                .WithCapability("leftStickY", 0x2882)
            );
    #endif
        }
    }
}