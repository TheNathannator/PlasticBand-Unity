using System;
using UnityEngine.InputSystem;

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
        }
    }
}