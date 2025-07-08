using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/Santroller.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// The state format for Santroller HID DJ Hero Turntables.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDTurntableState : ITurntableState
    {
        [Flags]
        public enum Button : ushort
        {
            None = 0,

            South = 0x0001,
            East = 0x0002,
            West = 0x0004,
            North_Euphoria = 0x0008,

            Select = 0x0010,
            Start = 0x0020,
            System = 0x0040,

            RightGreen = 0x0080,
            RightRed = 0x0100,
            RightBlue = 0x0200,

            LeftGreen = 0x0400,
            LeftRed = 0x0800,
            LeftBlue = 0x1000,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public Button buttons;

        public HidDpad dpad;

        private byte m_LeftTableVelocity;
        private byte m_RightTableVelocity;
        private byte m_EffectsDial;
        private byte m_Crossfader;

        public bool south
        {
            get => (buttons & Button.South) != 0;
            set => buttons.SetBit(Button.South, value);
        }

        public bool east
        {
            get => (buttons & Button.East) != 0;
            set => buttons.SetBit(Button.East, value);
        }

        public bool west
        {
            get => (buttons & Button.West) != 0;
            set => buttons.SetBit(Button.West, value);
        }

        public bool north_euphoria
        {
            get => (buttons & Button.North_Euphoria) != 0;
            set => buttons.SetBit(Button.North_Euphoria, value);
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

        public bool leftGreen
        {
            get => (buttons & Button.LeftGreen) != 0;
            set => buttons.SetBit(Button.LeftGreen, value);
        }

        public bool leftRed
        {
            get => (buttons & Button.LeftRed) != 0;
            set => buttons.SetBit(Button.LeftRed, value);
        }

        public bool leftBlue
        {
            get => (buttons & Button.LeftBlue) != 0;
            set => buttons.SetBit(Button.LeftBlue, value);
        }

        public bool rightGreen
        {
            get => (buttons & Button.RightGreen) != 0;
            set => buttons.SetBit(Button.RightGreen, value);
        }

        public bool rightRed
        {
            get => (buttons & Button.RightRed) != 0;
            set => buttons.SetBit(Button.RightRed, value);
        }

        public bool rightBlue
        {
            get => (buttons & Button.RightBlue) != 0;
            set => buttons.SetBit(Button.RightBlue, value);
        }

        public sbyte leftVelocity
        {
            get => (sbyte)(m_LeftTableVelocity - 0x80);
            set => m_LeftTableVelocity = (byte)(value + 0x80);
        }

        public sbyte rightVelocity
        {
            get => (sbyte)(m_RightTableVelocity - 0x80);
            set => m_RightTableVelocity = (byte)(value + 0x80);
        }

        public ushort effectsDial
        {
            get => (ushort)(m_EffectsDial << 8);
            set => m_EffectsDial = (byte)(value >> 8);
        }

        public sbyte crossfader
        {
            get => (sbyte)(m_Crossfader - 0x80);
            set => m_Crossfader = (byte)(value + 0x80);
        }
    }

    /// <summary>
    /// A Santroller HID Turntable.
    /// </summary>
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller HID Turntable")]
    internal class SantrollerHIDTurntable : TranslatingTurntable<SantrollerHIDTurntableState>, ISantrollerTurntableHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDTurntable>(SantrollerDeviceType.DjHeroTurntable);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerTurntableHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerTurntableHaptics m_Haptics;

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public override void PauseHaptics() => m_Haptics.PauseHaptics();

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public override void ResumeHaptics() => m_Haptics.ResumeHaptics();

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public override void ResetHaptics() => m_Haptics.ResetHaptics();

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

        /// <inheritdoc cref="ISantrollerTurntableHaptics.SetHitNotes(TurntableHitNote)"/>
        public void SetHitNotes(TurntableHitNote notes) => m_Haptics.SetHitNotes(notes);

        /// <inheritdoc cref="ISantrollerTurntableHaptics.SetEuphoriaBrightness(float)"/>
        public void SetEuphoriaBrightness(float brightness) => m_Haptics.SetEuphoriaBrightness(brightness);
    }
}
