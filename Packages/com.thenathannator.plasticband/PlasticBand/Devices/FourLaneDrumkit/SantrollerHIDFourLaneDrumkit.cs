using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
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
    internal unsafe struct SantrollerFourLaneDrumkitState : IFourLaneDrumkitState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        public ushort buttons;
        public HidDpad dpad;

        public byte greenPadVelocity;
        public byte redPadVelocity;
        public byte yellowPadVelocity;
        public byte bluePadVelocity;
        public byte greenCymbalVelocity;
        public byte yellowCymbalVelocity;
        public byte blueCymbalVelocity;

        public bool south => (buttons & 0x0001) != 0;
        public bool east => (buttons & 0x0002) != 0;
        public bool west => (buttons & 0x0004) != 0;
        public bool north => (buttons & 0x0008) != 0;

        public bool red => east;
        public bool yellow => north;
        public bool blue => west;
        public bool green => south;

        public bool pad => (buttons & 0x0010) != 0;
        public bool cymbal => (buttons & 0x0020) != 0;

        public bool kick1 => (buttons & 0x0040) != 0;
        public bool kick2 => (buttons & 0x0080) != 0;

        public bool dpadUp => dpad.IsUp();
        public bool dpadRight => dpad.IsRight();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();

        public bool select => (buttons & 0x0100) != 0;
        public bool start => (buttons & 0x0200) != 0;
        public bool system => (buttons & 0x0400) != 0;

        byte IFourLaneDrumkitState_Flags.redPadVelocity => redPadVelocity;
        byte IFourLaneDrumkitState_Flags.yellowPadVelocity => yellowPadVelocity;
        byte IFourLaneDrumkitState_Flags.bluePadVelocity => bluePadVelocity;
        byte IFourLaneDrumkitState_Flags.greenPadVelocity => greenPadVelocity;
        byte IFourLaneDrumkitState_Flags.yellowCymbalVelocity => yellowCymbalVelocity;
        byte IFourLaneDrumkitState_Flags.blueCymbalVelocity => blueCymbalVelocity;
        byte IFourLaneDrumkitState_Flags.greenCymbalVelocity => greenCymbalVelocity;
    }

    /// <summary>
    /// A Santroller HID Rock Band Drum Kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller HID Rock Band Drumkit")]
    internal class SantrollerHIDFourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<SantrollerFourLaneDrumkitState>,
        ISantrollerFourLaneDrumkitHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDFourLaneDrumkit>(SantrollerDeviceType.GuitarHeroDrums);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new SantrollerFourLaneDrumkitHaptics.Hid(this);
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
