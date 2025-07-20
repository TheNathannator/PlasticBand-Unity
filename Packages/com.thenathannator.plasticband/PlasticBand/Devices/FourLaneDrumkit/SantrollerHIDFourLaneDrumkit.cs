using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Santroller.md
namespace PlasticBand.Devices
{
    /// <summary>
    /// The state format for Santroller HID Rock Band Drum Kits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerFourLaneDrumkitState : IFourLaneDrumkitState_FlagButtons,
        IFourLaneDrumkitState_DistinctVelocities
    {
        [Flags]
        public enum Button : ushort
        {
            None = 0,

            South = 0x0001,
            East = 0x0002,
            West = 0x0004,
            North = 0x0008,

            Pad = 0x0010,
            Cymbal = 0x0020,

            Kick1 = 0x0040,
            Kick2 = 0x0080,

            Select = 0x0100,
            Start = 0x0200,
            System = 0x0400,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public Button buttons;
        public HidDpad dpad;

        public byte greenPadVelocity;
        public byte redPadVelocity;
        public byte yellowPadVelocity;
        public byte bluePadVelocity;
        public byte greenCymbalVelocity;
        public byte yellowCymbalVelocity;
        public byte blueCymbalVelocity;

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

        public bool north
        {
            get => (buttons & Button.North) != 0;
            set => buttons.SetBit(Button.North, value);
        }

        public bool pad
        {
            get => (buttons & Button.Pad) != 0;
            set => buttons.SetBit(Button.Pad, value);
        }

        public bool cymbal
        {
            get => (buttons & Button.Cymbal) != 0;
            set => buttons.SetBit(Button.Cymbal, value);
        }

        public bool kick1
        {
            get => (buttons & Button.Kick1) != 0;
            set => buttons.SetBit(Button.Kick1, value);
        }

        public bool kick2
        {
            get => (buttons & Button.Kick2) != 0;
            set => buttons.SetBit(Button.Kick2, value);
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

        byte IFourLaneDrumkitState_DistinctVelocities.redPadVelocity
        {
            get => redPadVelocity;
            set => redPadVelocity = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.yellowPadVelocity
        {
            get => yellowPadVelocity;
            set => yellowPadVelocity = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.bluePadVelocity
        {
            get => bluePadVelocity;
            set => bluePadVelocity = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.greenPadVelocity
        {
            get => greenPadVelocity;
            set => greenPadVelocity = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.yellowCymbalVelocity
        {
            get => yellowCymbalVelocity;
            set => yellowCymbalVelocity = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.blueCymbalVelocity
        {
            get => blueCymbalVelocity;
            set => blueCymbalVelocity = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.greenCymbalVelocity
        {
            get => greenCymbalVelocity;
            set => greenCymbalVelocity = value;
        }
    }

    /// <summary>
    /// A Santroller HID Rock Band Drum Kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller HID Rock Band Drumkit")]
    internal class SantrollerHIDFourLaneDrumkit : TranslatingFourLaneDrumkit_Hybrid<SantrollerFourLaneDrumkitState>,
        ISantrollerFourLaneDrumkitHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDFourLaneDrumkit>(SantrollerDeviceType.GuitarHeroDrums);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFourLaneDrumkitHaptics.Create(this, StageKitProtocol.SantrollerHID);
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

        /// <inheritdoc cref="ISantrollerFourLaneDrumkitHaptics.SetHitNotes(FourLaneDrumkitHitNote)"/>
        public void SetHitNotes(FourLaneDrumkitHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}
