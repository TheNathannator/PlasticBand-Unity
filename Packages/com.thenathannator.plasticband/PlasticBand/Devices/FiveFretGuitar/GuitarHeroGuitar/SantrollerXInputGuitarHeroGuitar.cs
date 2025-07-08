using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerXInputGuitarHeroGuitarState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]
        [InputControl(name = "spPedal", layout = "Button", bit = 9)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]
        public ushort buttons;

        // Accelerometer values are not provided on Santroller guitars
        // [InputControl(name = "accelY", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize=true,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte accelY;

        // [InputControl(name = "accelZ", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize=true,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte accelZ;

        [InputControl(name = "touchGreen", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchRed", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchYellow", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchBlue", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchOrange", layout = "GuitarHeroSlider", format = "SHRT")]
        public short slider;

        public short unused;

        [InputControl(name = "whammy", layout = "Axis", defaultState = short.MinValue, parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true, alias = null)]
        public short tilt;
    }

    [InputControlLayout(stateType = typeof(SantrollerXInputGuitarHeroGuitarState), displayName = "Santroller XInput Guitar Hero Guitar")]
    internal class SantrollerXInputGuitarHeroGuitar : XInputGuitarHeroGuitar, ISantrollerFiveFretGuitarHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputGuitarHeroGuitar>(
                XInputController.DeviceSubType.GuitarAlternate, SantrollerDeviceType.GuitarHeroGuitar);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFiveFretGuitarHaptics.Create(this, StageKitProtocol.XInput);
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
        public void SetRedLeds(StageKitLed leds) => m_Haptics.SetRedLeds(leds);

        /// <inheritdoc cref="IStageKitHaptics.SetYellowLeds(StageKitLed)"/>
        public void SetYellowLeds(StageKitLed leds) => m_Haptics.SetYellowLeds(leds);

        /// <inheritdoc cref="IStageKitHaptics.SetBlueLeds(StageKitLed)"/>
        public void SetBlueLeds(StageKitLed leds) => m_Haptics.SetBlueLeds(leds);

        /// <inheritdoc cref="IStageKitHaptics.SetGreenLeds(StageKitLed)"/>
        public void SetGreenLeds(StageKitLed leds) => m_Haptics.SetGreenLeds(leds);

        /// <inheritdoc cref="ISantrollerHaptics.SetStarPowerFill(float)"/>
        public void SetStarPowerFill(float fill) => m_Haptics.SetStarPowerFill(fill);

        /// <inheritdoc cref="ISantrollerHaptics.SetStarPowerActive(bool)"/>
        public void SetStarPowerActive(bool enabled) => m_Haptics.SetStarPowerActive(enabled);

        /// <inheritdoc cref="ISantrollerHaptics.SetMultiplier(byte)"/>
        public void SetMultiplier(byte multiplier) => m_Haptics.SetMultiplier(multiplier);

        /// <inheritdoc cref="ISantrollerHaptics.SetSoloActive(bool)"/>
        public void SetSoloActive(bool enabled) => m_Haptics.SetSoloActive(enabled);

        /// <inheritdoc cref="ISantrollerHaptics.SetNoteMiss(bool)"/>
        public void SetNoteMiss(bool enabled) => m_Haptics.SetNoteMiss(enabled);

        /// <inheritdoc cref="ISantrollerFiveFretGuitarHaptics.SetHitNotes(FiveFretGuitarHitNote)"/>
        public void SetHitNotes(FiveFretGuitarHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}
