using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using System.Runtime.InteropServices;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Santroller.md
namespace PlasticBand.Devices
{
    /// <summary>
    /// The state format for Santroller HID Rock Band Drum Kits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerFourLaneDrumkitState : IInputStateTypeInfo
    {
        public byte reportId;
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0)]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3)]

        [InputControl(name = "kick1", layout = "Button", bit = 4)]
        [InputControl(name = "kick2", layout = "Button", bit = 5)]

        [InputControl(name = "redPad", layout = "Button", bit = 6)]
        [InputControl(name = "yellowPad", layout = "Button", bit = 7)]
        [InputControl(name = "bluePad", layout = "Button", bit = 8)]
        [InputControl(name = "greenPad", layout = "Button", bit = 9)]
        [InputControl(name = "yellowCymbal", layout = "Button", bit = 10)]
        [InputControl(name = "blueCymbal", layout = "Button", bit = 11)]
        [InputControl(name = "greenCymbal", layout = "Button", bit = 12)]

        [InputControl(name = "selectButton", layout = "Button", bit = 13)]
        [InputControl(name = "startButton", layout = "Button", bit = 14)]
        [InputControl(name = "systemButton", layout = "Button", bit = 15)]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", displayName = "Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", displayName = "Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Pad Velocity")]
        public byte greenVelocity;

        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Pad Velocity")]
        public byte redVelocity;

        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Pad Velocity")]
        public byte yellowVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Pad Velocity")]
        public byte blueVelocity;

        [InputControl(name = "greenCymbalVelocity", layout = "Axis", displayName = "Green Cymbal Velocity")]
        public byte greenCymbalVelocity;
        
        [InputControl(name = "yellowCymbalVelocity", layout = "Axis", displayName = "Yellow Cymbal Velocity")]
        public byte yellowCymbalVelocity;

        [InputControl(name = "blueCymbalVelocity", layout = "Axis", displayName = "Blue Cymbal Velocity")]
        public byte blueCymbalVelocity;
    }

    /// <summary>
    /// A Santroller HID Rock Band Drum Kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(SantrollerFourLaneDrumkitState), displayName = "Santroller HID Rock Band Drumkit")]
    internal class SantrollerHIDFourLaneDrumkit : FourLaneDrumkit, ISantrollerFourLaneDrumkitHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDFourLaneDrumkit>(
                SantrollerDeviceType.GuitarHeroDrums);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new SantrollerFourLaneDrumkitHaptics(this);
        }

        private SantrollerFourLaneDrumkitHaptics m_Haptics;

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public void PauseHaptics() => m_Haptics.PauseHaptics();

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public void ResumeHaptics() => m_Haptics.ResumeHaptics();

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public void ResetHaptics() => m_Haptics.ResetHaptics();

        /// <inheritdoc cref="IStageKitHaptics.SetFogMachine(bool)"/>
        public void SetFogMachine(bool enabled) => m_Haptics.SetFogMachine(enabled);

        /// <inheritdoc cref="IStageKitHaptics.SetStrobeSpeed(StageKitStrobeSpeed)"/>
        public void SetStrobeSpeed(StageKitStrobeSpeed speed) => m_Haptics.SetStrobeSpeed(speed);

        /// <inheritdoc cref="IStageKitHaptics.SetLeds(StageKitLedColor, StageKitLed)"/>
        public void SetLeds(StageKitLedColor color, StageKitLed leds) => m_Haptics.SetLeds(color, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetRedLeds(StageKitLed)"/>
        public void SetRedLeds(StageKitLed leds) => SetLeds(StageKitLedColor.Red, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetYellowLeds(StageKitLed)"/>
        public void SetYellowLeds(StageKitLed leds) => SetLeds(StageKitLedColor.Yellow, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetBlueLeds(StageKitLed)"/>
        public void SetBlueLeds(StageKitLed leds) => SetLeds(StageKitLedColor.Blue, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetGreenLeds(StageKitLed)"/>
        public void SetGreenLeds(StageKitLed leds) => SetLeds(StageKitLedColor.Green, leds);

        /// <inheritdoc cref="ISantrollerHaptics.SetStarPowerFill(float)"/>
        public void SetStarPowerFill(float fill) => m_Haptics.SetStarPowerFill(fill);

        /// <inheritdoc cref="ISantrollerHaptics.SetStarPowerActive(bool)"/>
        public void SetStarPowerActive(bool enabled) => m_Haptics.SetStarPowerActive(enabled);

        /// <inheritdoc cref="ISantrollerHaptics.SetMultiplier(uint)"/>
        public void SetMultiplier(uint multiplier) => m_Haptics.SetMultiplier(multiplier);

        /// <inheritdoc cref="ISantrollerHaptics.SetSolo(bool)"/>
        public void SetSolo(bool enabled) => m_Haptics.SetSolo(enabled);

        /// <inheritdoc cref="ISantrollerFourLaneDrumkitHaptics.SetNoteLights(FourLanePad, bool)"/>
        public void SetNoteLights(FourLanePad pads, bool enabled) => m_Haptics.SetNoteLights(pads, enabled);
    }
}
