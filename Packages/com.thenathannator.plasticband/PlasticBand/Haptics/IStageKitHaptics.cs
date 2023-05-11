using System;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Haptics;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Speeds for the stage kit strobe light.
    /// </summary>
    public enum StageKitStrobeSpeed : byte
    {
        Off,
        Slow,
        Medium,
        Fast,
        Fastest
    }

    /// <summary>
    /// Available LED colors on the stage kit.
    /// </summary>
    [Flags]
    public enum StageKitLedColor : byte
    {
        None = 0x00,

        Red = 0x01,
        Yellow = 0x02,
        Blue = 0x04,
        Green = 0x08,

        All = Red | Yellow | Blue | Green
    }

    /// <summary>
    /// Bitmask for the stage kit LEDs.
    /// </summary>
    [Flags]
    public enum StageKitLed : byte
    {
        None = 0x00,

        Led1 = 0x01,
        Led2 = 0x02,
        Led3 = 0x04,
        Led4 = 0x08,
        Led5 = 0x10,
        Led6 = 0x20,
        Led7 = 0x40,
        Led8 = 0x80,

        All = Led1 | Led2 | Led3 | Led4 | Led5 | Led6 | Led7 | Led8
    }

    /// <summary>
    /// Interface for <see cref="StageKit"/> haptics.
    /// </summary>
    public interface IStageKitHaptics : IHaptics
    {
        /// <summary>
        /// Enables or disables the fog machine.
        /// </summary>
        void SetFogMachine(bool enabled);

        /// <summary>
        /// Sets the speed of the strobe light.
        /// </summary>
        void SetStrobeSpeed(StageKitStrobeSpeed speed);

        /// <summary>
        /// Sets the state of the given LED color(s).
        /// </summary>
        void SetLeds(StageKitLedColor color, StageKitLed leds);

        /// <summary>
        /// Sets the state of the red LEDs.
        /// </summary>
        void SetRedLeds(StageKitLed leds);

        /// <summary>
        /// Sets the state of the yellow LEDs.
        /// </summary>
        void SetYellowLeds(StageKitLed leds);

        /// <summary>
        /// Sets the state of the blue LEDs.
        /// </summary>
        void SetBlueLeds(StageKitLed leds);

        /// <summary>
        /// Sets the state of the green LEDs.
        /// </summary>
        void SetGreenLeds(StageKitLed leds);
    }
}