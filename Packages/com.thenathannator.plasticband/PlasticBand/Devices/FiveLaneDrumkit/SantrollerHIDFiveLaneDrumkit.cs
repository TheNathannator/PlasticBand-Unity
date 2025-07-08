using System;
using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;


// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/Santroller.md
namespace PlasticBand.Devices
{
    /// <summary>
    /// The state format for Santroller HID Guitar Hero Drum Kits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerFiveLaneDrumkitState : IFiveLaneDrumkitState
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
            Kick = 0x0020,

            Select = 0x0040,
            Start = 0x0080,
            System = 0x0100,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public Button buttons;
        public HidDpad dpad;

        public byte greenVelocity;
        public byte redVelocity;
        public byte yellowVelocity;
        public byte blueVelocity;
        public byte orangeVelocity;
        public byte kickVelocity;

        public bool red_east
        {
            get => (buttons & Button.Red) != 0;
            set => buttons.SetBit(Button.Red, value);
        }

        public bool yellow_north
        {
            get => (buttons & Button.Yellow) != 0;
            set => buttons.SetBit(Button.Yellow, value);
        }

        public bool blue_west
        {
            get => (buttons & Button.Blue) != 0;
            set => buttons.SetBit(Button.Blue, value);
        }

        public bool green_south
        {
            get => (buttons & Button.Green) != 0;
            set => buttons.SetBit(Button.Green, value);
        }

        public bool orange
        {
            get => (buttons & Button.Orange) != 0;
            set => buttons.SetBit(Button.Orange, value);
        }

        public bool kick
        {
            get => (buttons & Button.Kick) != 0;
            set => buttons.SetBit(Button.Kick, value);
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

        byte IFiveLaneDrumkitState.redVelocity
        {
            get => redVelocity;
            set => redVelocity = value;
        }

        byte IFiveLaneDrumkitState.yellowVelocity
        {
            get => yellowVelocity;
            set => yellowVelocity = value;
        }

        byte IFiveLaneDrumkitState.blueVelocity
        {
            get => blueVelocity;
            set => blueVelocity = value;
        }

        byte IFiveLaneDrumkitState.orangeVelocity
        {
            get => orangeVelocity;
            set => orangeVelocity = value;
        }

        byte IFiveLaneDrumkitState.greenVelocity
        {
            get => greenVelocity;
            set => greenVelocity = value;
        }

        byte IFiveLaneDrumkitState.kickVelocity
        {
            get => kickVelocity;
            set => kickVelocity = value;
        }
    }

    /// <summary>
    /// A Santroller HID Guitar Hero Drum Kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Santroller HID Guitar Hero Drumkit")]
    internal class SantrollerHIDFiveLaneDrumkit : TranslatingFiveLaneDrumkit<SantrollerFiveLaneDrumkitState>,
        ISantrollerFiveLaneDrumkitHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDFiveLaneDrumkit>(SantrollerDeviceType.GuitarHeroDrums);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFiveLaneDrumkitHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerFiveLaneDrumkitHaptics m_Haptics;

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

        /// <inheritdoc cref="ISantrollerFiveLaneDrumkitHaptics.SetHitNotes(FiveLaneDrumkitHitNote)"/>
        public void SetHitNotes(FiveLaneDrumkitHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}
