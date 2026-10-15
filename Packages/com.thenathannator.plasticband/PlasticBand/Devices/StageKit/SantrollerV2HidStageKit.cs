using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SantrollerV2HidStageKitState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public byte reserved;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5,maxValue=7")]
        [InputControl(name = "start", layout = "Button", bit = 4)]
        [InputControl(name = "select", layout = "Button", bit = 5)]
        [InputControl(name = "leftStickPress", layout = "Button", bit = 6)]
        [InputControl(name = "rightStickPress", layout = "Button", bit = 7)]
        public byte buttons1;

        [InputControl(name = "leftShoulder", layout = "Button", bit = 0)]
        [InputControl(name = "rightShoulder", layout = "Button", bit = 1)]
        [InputControl(name = "systemButton", layout = "Button", bit = 2)]
        [InputControl(name = "captureButton", layout = "Button", bit = 3)]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 4)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 5)]
        [InputControl(name = "buttonWest", layout = "Button", bit = 6)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 7)]
        public byte buttons2;

        [InputControl(name = "leftTrigger", layout = "Button", format = "BYTE")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "Button", format = "BYTE")]
        public byte rightTrigger;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2S")]
        [InputControl(name = "leftStick/x", format = "SHRT", offset = 0, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/left", format = "SHRT", offset = 0)]
        [InputControl(name = "leftStick/right", format = "SHRT", offset = 0)]
        [InputControl(name = "leftStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "leftStick/down", format = "SHRT", offset = 2)]
        public short leftStickX;
        public short leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2S")]
        [InputControl(name = "rightStick/x", format = "SHRT", offset = 0, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/left", format = "SHRT", offset = 0)]
        [InputControl(name = "rightStick/right", format = "SHRT", offset = 0)]
        [InputControl(name = "rightStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "rightStick/down", format = "SHRT", offset = 2)]
        public short rightStickX;
        public short rightStickY;
    }

    [InputControlLayout(stateType = typeof(SantrollerV2HidStageKitState), displayName = "Santroller V2 HID Stage Kit", hideInUI = true)]
    internal class SantrollerV2HidStageKit : StageKit, ISantrollerHaptics
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerV2HidStageKit>();
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
