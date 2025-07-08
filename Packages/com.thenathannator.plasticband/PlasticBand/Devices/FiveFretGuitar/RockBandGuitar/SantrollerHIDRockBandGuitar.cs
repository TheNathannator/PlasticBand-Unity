using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Santroller.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDRockBandGuitarState : IRockBandGuitarState_Distinct
    {
        [Flags]
        public enum Button : ushort
        {
            None = 0,

            Green = 0x0001,
            Red = 0x0002,
            Yellow = 0x0004,
            Blue = 0x0008,
            Orange = 0x0010,

            SoloGreen = 0x0020,
            SoloRed = 0x0040,
            SoloYellow = 0x0080,
            SoloBlue = 0x0100,
            SoloOrange = 0x0200,

            Select = 0x0400,
            Start = 0x0800,
            System = 0x1000,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public Button buttons;
        public HidDpad dpad;

        public byte whammy;
        public byte pickupSwitch;

        [InputControl(name = "tilt", layout = "IntAxis", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        private byte m_Tilt;

        public bool green
        {
            get => (buttons & Button.Green) != 0;
            set => buttons.SetBit(Button.Green, value);
        }

        public bool red
        {
            get => (buttons & Button.Red) != 0;
            set => buttons.SetBit(Button.Red, value);
        }

        public bool yellow
        {
            get => (buttons & Button.Yellow) != 0;
            set => buttons.SetBit(Button.Yellow, value);
        }

        public bool blue
        {
            get => (buttons & Button.Blue) != 0;
            set => buttons.SetBit(Button.Blue, value);
        }

        public bool orange
        {
            get => (buttons & Button.Orange) != 0;
            set => buttons.SetBit(Button.Orange, value);
        }

        public bool soloGreen
        {
            get => (buttons & Button.SoloGreen) != 0;
            set => buttons.SetBit(Button.SoloGreen, value);
        }

        public bool soloRed
        {
            get => (buttons & Button.SoloRed) != 0;
            set => buttons.SetBit(Button.SoloRed, value);
        }

        public bool soloYellow
        {
            get => (buttons & Button.SoloYellow) != 0;
            set => buttons.SetBit(Button.SoloYellow, value);
        }

        public bool soloBlue
        {
            get => (buttons & Button.SoloBlue) != 0;
            set => buttons.SetBit(Button.SoloBlue, value);
        }

        public bool soloOrange
        {
            get => (buttons & Button.SoloOrange) != 0;
            set => buttons.SetBit(Button.SoloOrange, value);
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

        public bool system
        {
            get => (buttons & Button.System) != 0;
            set => buttons.SetBit(Button.System, value);
        }

        byte IFiveFretGuitarState.whammy
        {
            get => whammy;
            set => whammy = value;
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt ^ 0x80);
            set => m_Tilt = (byte)(value ^ 0x80);
        }

        int IRockBandGuitarState_Base.pickupSwitch
        {
            get => pickupSwitch;
            set => pickupSwitch = (byte)value;
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Santroller HID Rock Band Guitar")]
    internal class SantrollerHIDRockBandGuitar : TranslatingRockBandGuitar_Distinct<SantrollerHIDRockBandGuitarState>, ISantrollerFiveFretGuitarHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDRockBandGuitar>(
                SantrollerDeviceType.RockBandGuitar);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFiveFretGuitarHaptics.Create(this, StageKitProtocol.SantrollerHID);
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
