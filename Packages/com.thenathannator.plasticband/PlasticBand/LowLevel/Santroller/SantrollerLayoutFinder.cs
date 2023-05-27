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
        public const int VendorID = 0x1209;
        public const int ProductID = 0x2882;

        // Binary-coded decimal does not use 0xA-0xF
        private const int kBcdDigitMin = 0;
        private const int kBcdDigitMax = 9;

        // Default device/rhythm types for XInput subtypes
        private static readonly Dictionary<int, (SantrollerDeviceType type, SantrollerRhythmType rhythm)> s_XInputSubtypeToDeviceType
            = new Dictionary<int, (SantrollerDeviceType, SantrollerRhythmType)>()
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

        internal static SantrollerDeviceType GetDeviceType(ushort version)
            => (SantrollerDeviceType)(version >> 8);

        internal static SantrollerRhythmType GetRhythmType(ushort version)
            => (SantrollerRhythmType)((version >> 4) & 0x0F);

        internal static int GetRevisionValue(SantrollerDeviceType deviceType, SantrollerRhythmType rhythmType, int consoleType)
            => (((int)deviceType & 0xFF) << 8) | (((int)rhythmType & 0x0F) << 4) | (consoleType & 0x0F);

        internal static void RegisterHIDLayout<TDevice>(SantrollerDeviceType deviceType,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None)
            where TDevice : InputDevice
        {
            // Register one matcher for every console type
            for (int i = kBcdDigitMin; i <= kBcdDigitMax; i++)
                InputSystem.RegisterLayout<TDevice>(matches: GetHidMatcher(deviceType, rhythmType, i));
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

        internal static InputDeviceMatcher GetHidMatcher(SantrollerDeviceType deviceType,
            SantrollerRhythmType rhythmType = SantrollerRhythmType.None,
            int consoleType = 0)
        {
            return HidLayoutFinder.GetMatcher(VendorID, ProductID)
                .WithVersion(GetRevisionValue(deviceType, rhythmType, consoleType).ToString());
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