using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using System.Runtime.InteropServices;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/Santroller.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// The state format for Santroller HID DJ Hero Turntables.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDTurntableState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "buttonSouth", layout = "Button", bit = 0, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 1, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 2, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle / Euphoria")]

        [InputControl(name = "euphoria", layout = "Button", bit = 3, displayName = "Euphoria")]

        [InputControl(name = "selectButton", layout = "Button", bit = 4)]
        [InputControl(name = "startButton", layout = "Button", bit = 5)]
        [InputControl(name = "systemButton", layout = "Button", bit = 6)]

        [InputControl(name = "rightTableGreen", layout = "Button", bit = 7)]
        [InputControl(name = "rightTableRed", layout = "Button", bit = 8)]
        [InputControl(name = "rightTableBlue", layout = "Button", bit = 9)]

        [InputControl(name = "leftTableGreen", layout = "Button", bit = 10)]
        [InputControl(name = "leftTableRed", layout = "Button", bit = 11)]
        [InputControl(name = "leftTableBlue", layout = "Button", bit = 12)]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 15)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=15,wrapAtValue=7", displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", displayName = "Down/Strum Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        [InputControl(name = "leftTableVelocity", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte leftTableVelocity;

        [InputControl(name = "rightTableVelocity", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte rightTableVelocity;

        [InputControl(name = "effectsDial", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte effectsDial;

        [InputControl(name = "crossFader", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte crossFader;
    }

    /// <summary>
    /// A Santroller HID Turntable.
    /// </summary>
    [InputControlLayout(stateType = typeof(SantrollerHIDTurntableState), displayName = "Santroller HID Turntable")]
    internal class SantrollerHIDTurntable : PS3Turntable, ISantrollerTurntableHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDTurntable>(SantrollerDeviceType.DjHeroTurntable);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new SantrollerTurntableHaptics(this);
        }

        private new SantrollerTurntableHaptics m_Haptics;

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public new void PauseHaptics()
        {
            base.PauseHaptics();
            m_Haptics.PauseHaptics();
        }

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public new void ResumeHaptics()
        {
            base.ResumeHaptics();
            m_Haptics.ResumeHaptics();
        }

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public new void ResetHaptics()
        {
            base.ResetHaptics();
            m_Haptics.ResetHaptics();
        }

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

        /// <inheritdoc cref="ISantrollerTurntableHaptics.SetNoteLights(TurntableButton, TurntableButton, bool)"/>
        public void SetNoteLights(TurntableButton left, TurntableButton right, bool enabled) => m_Haptics.SetNoteLights(left, right, enabled);

        /// <inheritdoc cref="ISantrollerTurntableHaptics.SetScratchLights(bool, bool)"/>
        public void SetScratchLights(bool left, bool right) => m_Haptics.SetScratchLights(left, right);

        /// <inheritdoc cref="ISantrollerTurntableHaptics.SetEuphoriaBrightness(float)"/>
        public void SetEuphoriaBrightness(float brightness) => m_Haptics.SetEuphoriaBrightness(brightness);
    }
}
