using System;
#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Backend.Linux;
#endif
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

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
        DisneyInfinity = 10,
        Skylanders = 11,
        LegoDimensions = 12,
        ProjectDiva = 13,
        GuitarFreaks = 14,
        Wheel = 15,
        ProKeys = 16,
        ProGuitarMustang = 17,
        ProGuitarSquire = 18,
        KeyboardMouse = 19,
        FightStick = 20,
        FlightStick = 21,
        PopNMusic = 22,
        DjMax = 23,
        Taiko = 24,
        PowerGigDrum = 25,
        PowerGigGuitar = 26,
        RockRevolutionGuitar = 27,
        Midi = 28,
        BeatMania = 29,
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

    /// <summary>
    /// Registers and resolves layouts for Santroller devices.
    /// </summary>
    internal static class SantrollerLayoutFinder
    {
        public const int VendorID = 0x1209;
        public const int ProductID = 0x2882;
        public const int ProductIDV2 = 0x2883;

        internal static short GetRevisionValue(SantrollerDeviceType deviceType)
            => (short)(((int)deviceType & 0xFF) << 8);

        internal static void RegisterXInputLayout<TDevice>(XInputDeviceSubType subType,
            SantrollerDeviceType deviceType)
            where TDevice : InputDevice
        {
            XInputLayoutFinder.RegisterLayout<TDevice>(subType, VendorID, ProductID, GetRevisionValue(deviceType));
            XInputLayoutFinder.RegisterLayout<TDevice>(subType, VendorID, ProductIDV2, GetRevisionValue(deviceType));
        }

#if HIDROGEN_0_6_0_OR_LATER
        /// <summary>
        /// Registers an evdev layout for Santroller V2 devices, which are exposed through hid-generic.
        /// </summary>
        internal static void RegisterEvdevLayout<TDevice>(
            EvdevDeviceMapping mapping,
            SantrollerDeviceType type
        )
            where TDevice : InputDevice
        {
            EvdevLayoutFinder.RegisterLayout<TDevice>(mapping, new InputDeviceMatcher()
                .WithInterface(EvdevDefinitions.InterfaceName)
                .WithCapability("vendorId", VendorID)
                .WithCapability("productId", ProductIDV2)
                .WithCapability("santrollerSubtype", (int)type));
        }

        /// <summary>
        /// Registers an evdev layout for Santroller V1 devices running in HID mode, which are exposed
        /// through hid-generic (or hid-santroller, which uses the same input mapping).
        /// </summary>
        /// <remarks>
        /// V1 devices share their vendor/product IDs and revision between HID and XInput mode,
        /// so a zero xpad subtype is required to avoid matching devices bound to xpad.
        /// </remarks>
        internal static void RegisterEvdevV1HidLayout<TDevice>(
            EvdevDeviceMapping mapping,
            SantrollerDeviceType type
        )
            where TDevice : InputDevice
        {
            EvdevLayoutFinder.RegisterLayout<TDevice>(mapping, new InputDeviceMatcher()
                .WithInterface(EvdevDefinitions.InterfaceName)
                .WithVersion(GetRevisionValue(type).ToString())
                .WithCapability("vendorId", VendorID)
                .WithCapability("productId", ProductID)
                .WithCapability("xpadSubtype", 0));
        }

        /// <summary>
        /// Registers an evdev layout for Santroller V1 devices running in XInput mode, which are exposed through xpad.
        /// </summary>
        internal static void RegisterEvdevXpadLayout<TDevice>(
            EvdevDeviceMapping mapping,
            XInputDeviceSubType subType,
            SantrollerDeviceType type
        )
            where TDevice : InputDevice
        {
            EvdevLayoutFinder.RegisterLayout<TDevice>(mapping, new InputDeviceMatcher()
                .WithInterface(EvdevDefinitions.InterfaceName)
                .WithVersion(GetRevisionValue(type).ToString())
                .WithCapability("vendorId", VendorID)
                .WithCapability("productId", ProductID)
                .WithCapability("xpadSubtype", (int)subType));
        }
#endif
    }
}