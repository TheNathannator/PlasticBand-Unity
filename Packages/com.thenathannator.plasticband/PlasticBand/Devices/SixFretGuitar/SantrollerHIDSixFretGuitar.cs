using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/6-Fret%20Guitar/Santroller.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDSixFretGuitarState : ISixFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public ushort buttons;
        public HidDpad dpad;

        private readonly byte m_Whammy;
        private readonly byte m_Tilt;

        public bool dpadUp => dpad.IsUp();
        public bool dpadRight => dpad.IsRight();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();

        public bool black1 => (buttons & 0x0001) != 0;
        public bool black2 => (buttons & 0x0002) != 0;
        public bool black3 => (buttons & 0x0004) != 0;
        public bool white1 => (buttons & 0x0008) != 0;
        public bool white2 => (buttons & 0x0010) != 0;
        public bool white3 => (buttons & 0x0020) != 0;

        public bool select => (buttons & 0x0040) != 0;
        public bool start => (buttons & 0x0080) != 0;
        public bool ghtv => (buttons & 0x0100) != 0;
        public bool system => (buttons & 0x0200) != 0;

        public bool strumUp => dpad.IsUp();
        public bool strumDown => dpad.IsDown();

        public byte whammy => m_Whammy;
        public sbyte tilt => (sbyte)(m_Tilt - 0x80);
    }

    [InputControlLayout(stateType = typeof(TranslatedProGuitarState), displayName = "Santroller HID Guitar Hero Live Guitar")]
    internal class SantrollerHIDSixFretGuitar : TranslatingSixFretGuitar<SantrollerHIDSixFretGuitarState>,
        ISantrollerSixFretGuitarHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDSixFretGuitar>(SantrollerDeviceType.LiveGuitar);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new SantrollerSixFretGuitarHaptics(this);
        }

        private SantrollerSixFretGuitarHaptics m_Haptics;

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

        /// <inheritdoc cref="ISantrollerSixFretGuitarHaptics.SetNoteLights(SixFret, bool)"/>
        public void SetNoteLights(SixFret frets, bool enabled) => m_Haptics.SetNoteLights(frets, enabled);

        /// <inheritdoc cref="ISantrollerSixFretGuitarHaptics.SetOpenNoteLight(bool)"/>
        public void SetOpenNoteLight(bool enabled) => m_Haptics.SetOpenNoteLight(enabled);
    }
}