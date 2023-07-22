using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Santroller.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDRockBandGuitarState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;


        [InputControl(name = "buttonWest", layout = "Button", bit = 3)]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 0)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 1)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 2)]

        [InputControl(name = "leftShoulder", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5)]
        [InputControl(name = "startButton", layout = "Button", bit = 6)]
        [InputControl(name = "systemButton", layout = "Button", bit = 7)]

        [InputControl(name = "greenFret", layout = "Button", bit = 8)]
        [InputControl(name = "redFret", layout = "Button", bit = 9)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 10)]
        [InputControl(name = "blueFret", layout = "Button", bit = 11)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 12)]

        [InputControl(name = "soloGreen", layout = "Button", bit = 13)]
        [InputControl(name = "soloRed", layout = "Button", bit = 14)]
        [InputControl(name = "soloYellow", layout = "Button", bit = 15)]
        public ushort buttons;

        [InputControl(name = "soloBlue", layout = "Button", bit = 0)]
        [InputControl(name = "soloOrange", layout = "Button", bit = 1)]
        public byte buttons2;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=3,maxValue=5,nullValue=8")]
        public byte dpad;

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        [InputControl(name = "pickupSwitch", layout = "RockBandPickupSwitch")]
        public byte pickupSwitch;

        [InputControl(name = "tilt", layout = "Axis")]
        public byte tilt;
    }

    [InputControlLayout(stateType = typeof(SantrollerHIDRockBandGuitarState), displayName = "Santroller HID Rock Band Guitar")]
    internal class SantrollerHIDRockBandGuitar : RockBandGuitar, ISantrollerFiveFretGuitarHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDRockBandGuitar>(
                SantrollerDeviceType.RockBandGuitar);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new SantrollerFiveFretGuitarHaptics(this);
        }

        private SantrollerFiveFretGuitarHaptics m_Haptics;

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

        /// <inheritdoc cref="ISantrollerFiveFretGuitarHaptics.SetNoteLights(FiveFret, bool)"/>
        public void SetNoteLights(FiveFret frets, bool enabled) => m_Haptics.SetNoteLights(frets, enabled);

        /// <inheritdoc cref="ISantrollerFiveFretGuitarHaptics.SetOpenNoteLight(bool)"/>
        public void SetOpenNoteLight(bool enabled) => m_Haptics.SetOpenNoteLight(enabled);
    }
}
