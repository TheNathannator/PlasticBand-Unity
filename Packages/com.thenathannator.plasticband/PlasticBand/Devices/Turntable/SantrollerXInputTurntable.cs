using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/Xbox%20360.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller XInput Turntable")]
    internal class SantrollerXInputTurntable : XInputTurntable, ISantrollerTurntableHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputTurntable>(
                XInputNonStandardSubType.Turntable, SantrollerDeviceType.DjHeroTurntable);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerTurntableHaptics.Create(this, StageKitProtocol.XInput);
        }

        private SantrollerTurntableHaptics m_Haptics;
        private bool m_EuphoriaOverridden;

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public override void PauseHaptics()
        {
            m_Haptics.PauseHaptics();
            if (!m_EuphoriaOverridden)
            {
                base.PauseHaptics();
            }
        }

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public override void ResumeHaptics()
        {
            m_Haptics.ResumeHaptics();
            if (!m_EuphoriaOverridden)
            {
                base.ResumeHaptics();
            }
        }

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public override void ResetHaptics()
        {
            m_Haptics.ResetHaptics();
            if (!m_EuphoriaOverridden)
            {
                base.ResetHaptics();
            }
        }

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
        public void SetEuphoriaBrightness(float brightness)
        {
            if (!m_EuphoriaOverridden)
            {
                base.PauseHaptics();
                m_EuphoriaOverridden = true;
            }

            m_Haptics.SetEuphoriaBrightness(brightness);
        }
    }
}
