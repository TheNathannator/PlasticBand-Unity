using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(SantrollerV2HIDGamepadState), displayName = "Santroller V2 HID Stage Kit", hideInUI = true)]
    internal class SantrollerV2HIDStageKit : StageKit, ISantrollerHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerV2HIDStageKit>(SantrollerHIDProtocol.V2, SantrollerDeviceType.StageKit);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerHaptics m_Haptics;

        public override void PauseHaptics() => m_Haptics.PauseHaptics();
        public override void ResumeHaptics() => m_Haptics.ResumeHaptics();
        public override void ResetHaptics() => m_Haptics.ResetHaptics();
        public override void SetFogMachine(bool enabled) => m_Haptics.SetFogMachine(enabled);
        public override void SetStrobeSpeed(StageKitStrobeSpeed speed) => m_Haptics.SetStrobeSpeed(speed);
        public override void SetLeds(StageKitLedColor color, StageKitLed leds) => m_Haptics.SetLeds(color, leds);
        public override void SetRedLeds(StageKitLed leds) => m_Haptics.SetRedLeds(leds);
        public override void SetYellowLeds(StageKitLed leds) => m_Haptics.SetYellowLeds(leds);
        public override void SetBlueLeds(StageKitLed leds) => m_Haptics.SetBlueLeds(leds);
        public override void SetGreenLeds(StageKitLed leds) => m_Haptics.SetGreenLeds(leds);
        public void SetStarPowerFill(float fill) => m_Haptics.SetStarPowerFill(fill);
        public void SetStarPowerActive(bool enabled) => m_Haptics.SetStarPowerActive(enabled);
        public void SetMultiplier(byte multiplier) => m_Haptics.SetMultiplier(multiplier);
        public void SetSoloActive(bool enabled) => m_Haptics.SetSoloActive(enabled);
        public void SetNoteMiss(bool enabled) => m_Haptics.SetNoteMiss(enabled);
    }
}
