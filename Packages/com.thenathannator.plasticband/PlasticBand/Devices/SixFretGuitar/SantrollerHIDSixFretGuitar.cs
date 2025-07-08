using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
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
        [Flags]
        public enum Button : ushort
        {
            None = 0,

            Black1 = 0x0001,
            Black2 = 0x0002,
            Black3 = 0x0004,
            White1 = 0x0008,
            White2 = 0x0010,
            White3 = 0x0020,

            Select = 0x0040,
            Start = 0x0080,
            GHTV = 0x0100,
            System = 0x0200,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public Button buttons;
        public HidDpad dpad;

        private byte m_Whammy;
        private byte m_Tilt;

        public bool black1
        {
            get => (buttons & Button.Black1) != 0;
            set => buttons.SetBit(Button.Black1, value);
        }

        public bool black2
        {
            get => (buttons & Button.Black2) != 0;
            set => buttons.SetBit(Button.Black2, value);
        }

        public bool black3
        {
            get => (buttons & Button.Black3) != 0;
            set => buttons.SetBit(Button.Black3, value);
        }

        public bool white1
        {
            get => (buttons & Button.White1) != 0;
            set => buttons.SetBit(Button.White1, value);
        }

        public bool white2
        {
            get => (buttons & Button.White2) != 0;
            set => buttons.SetBit(Button.White2, value);
        }

        public bool white3
        {
            get => (buttons & Button.White3) != 0;
            set => buttons.SetBit(Button.White3, value);
        }

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => dpad.SetUp(value);
        }

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => dpad.SetRight(value);
        }

        public bool dpadDown
        {
            get => dpad.IsDown();
            set => dpad.SetDown(value);
        }

        public bool dpadLeft
        {
            get => dpad.IsLeft();
            set => dpad.SetLeft(value);
        }

        public bool select
        {
            get => (buttons & Button.Select) != 0;
            set => buttons.SetBit(Button.Select, value);
        }

        public bool start
        {
            get => (buttons & Button.Start) != 0;
            set => buttons.SetBit(Button.Start, value);
        }

        public bool ghtv
        {
            get => (buttons & Button.GHTV) != 0;
            set => buttons.SetBit(Button.GHTV, value);
        }

        public bool system
        {
            get => (buttons & Button.System) != 0;
            set => buttons.SetBit(Button.System, value);
        }

        public bool strumUp
        {
            get => dpadUp;
            set => dpadUp = value;
        }

        public bool strumDown
        {
            get => dpadDown;
            set => dpadDown = value;
        }

        public byte whammy
        {
            get => m_Whammy;
            set => m_Whammy = value;
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt - 0x80);
            set => m_Tilt = (byte)(value + 0x80);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "Santroller HID Guitar Hero Live Guitar")]
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
            m_Haptics = SantrollerSixFretGuitarHaptics.Create(this, StageKitProtocol.SantrollerHID);
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

        /// <inheritdoc cref="ISantrollerSixFretGuitarHaptics.SetHitNotes(SixFretGuitarHitNote)"/>
        public void SetHitNotes(SixFretGuitarHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}