using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.LowLevel
{
    internal enum SantrollerDeviceType
    {
        Unknown = 0,
        Gamepad = 0x01,
        DancePad = 0x02,
        GuitarHeroGuitar = 0x03,
        RockBandGuitar = 0x04,
        GuitarHeroDrums = 0x05,
        RockBandDrums = 0x06,
        LiveGuitar = 0x07,
        DjHeroTurntable = 0x08,
        StageKit = 0x09
    }

    /// <summary>
    /// Registers and resolves layouts for Santroller devices.
    /// </summary>
    internal static class SantrollerLayoutFinder
    {
        public const int VendorID = 0x1209;
        public const int ProductID = 0x2882;

        internal static SantrollerDeviceType GetDeviceType(ushort version)
            => (SantrollerDeviceType)(version >> 8);

        internal static short GetRevisionValue(SantrollerDeviceType deviceType)
            => (short)(((int)deviceType & 0xFF) << 8);

        internal static void RegisterHIDLayout<TDevice>(SantrollerDeviceType deviceType)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetHidMatcher(deviceType));
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputController.DeviceSubType subType,
            SantrollerDeviceType deviceType)
            where TDevice : InputDevice
        {
            InputSystem.RegisterLayout<TDevice>(matches: GetXInputMatcher(subType, deviceType));
        }

        [Conditional("UNITY_STANDALONE_WIN"), Conditional("UNITY_EDITOR_WIN")]
        internal static void RegisterXInputLayout<TDevice>(XInputNonStandardSubType subType,
            SantrollerDeviceType deviceType)
            where TDevice : InputDevice
            => RegisterXInputLayout<TDevice>((XInputController.DeviceSubType)subType, deviceType);

        internal static InputDeviceMatcher GetHidMatcher(SantrollerDeviceType deviceType)
        {
            return HidLayoutFinder.GetMatcher(VendorID, ProductID)
                .WithVersion(GetRevisionValue(deviceType).ToString());
        }

        internal static InputDeviceMatcher GetXInputMatcher(XInputController.DeviceSubType subType,
            SantrollerDeviceType deviceType)
        {
            return XInputLayoutFinder.GetMatcher(subType, VendorID, ProductID, GetRevisionValue(deviceType));
        }
    }
}