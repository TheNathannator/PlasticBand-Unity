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
    internal unsafe struct SantrollerHIDTurntableState : ITurntableState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "selectButton", layout = "Button", bit = 4)]
        [InputControl(name = "startButton", layout = "Button", bit = 5)]
        [InputControl(name = "systemButton", layout = "Button", bit = 6, displayName = "System")]

        [InputControl(name = "rightTableGreen", layout = "Button", bit = 7)]
        [InputControl(name = "rightTableRed", layout = "Button", bit = 8)]
        [InputControl(name = "rightTableBlue", layout = "Button", bit = 9)]

        [InputControl(name = "leftTableGreen", layout = "Button", bit = 10)]
        [InputControl(name = "leftTableRed", layout = "Button", bit = 11)]
        [InputControl(name = "leftTableBlue", layout = "Button", bit = 12)]
        public ushort buttons;

        public HidDpad dpad;

        private readonly byte m_LeftTableVelocity;
        private readonly byte m_RightTableVelocity;
        private readonly byte m_EffectsDial;
        private readonly byte m_Crossfader;

        public bool south => (buttons & 0x0001) != 0;
        public bool east => (buttons & 0x0002) != 0;
        public bool west => (buttons & 0x0004) != 0;
        public bool north_euphoria => (buttons & 0x0008) != 0;

        public bool select => (buttons & 0x0010) != 0;
        public bool start => (buttons & 0x0020) != 0;
        public bool system => (buttons & 0x0040) != 0;

        public bool dpadUp => dpad.IsUp();
        public bool dpadRight => dpad.IsRight();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();

        public bool rightGreen => (buttons & 0x0080) != 0;
        public bool rightRed => (buttons & 0x0100) != 0;
        public bool rightBlue => (buttons & 0x0200) != 0;

        public bool leftGreen => (buttons & 0x0400) != 0;
        public bool leftRed => (buttons & 0x0800) != 0;
        public bool leftBlue => (buttons & 0x1000) != 0;

        public sbyte leftVelocity => (sbyte)(m_LeftTableVelocity - 0x80);
        public sbyte rightVelocity => (sbyte)(m_RightTableVelocity - 0x80);

        public ushort effectsDial => (ushort)(m_EffectsDial << 8);
        public sbyte crossfader => (sbyte)(m_Crossfader - 0x80);
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
