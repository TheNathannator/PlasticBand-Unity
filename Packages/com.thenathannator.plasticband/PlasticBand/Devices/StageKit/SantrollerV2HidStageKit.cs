using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SantrollerV2HidStageKitState : IGamepadState
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

        [InputControl(name = "leftShoulder", layout = "Button", bit = 8)]
        [InputControl(name = "rightShoulder", layout = "Button", bit = 9)]
        [InputControl(name = "systemButton", layout = "Button", bit = 10)]
        [InputControl(name = "shareButton", layout = "Button", bit = 11)]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13)]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15)]
        public SantrollerV2Button buttons;

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

        public bool start
        {
            get => (buttons & SantrollerV2Button.Start) != 0;
            set => buttons.SetBit(SantrollerV2Button.Start, value);
        }

        public bool select
        {
            get => (buttons & SantrollerV2Button.Select) != 0;
            set => buttons.SetBit(SantrollerV2Button.Select, value);
        }

        public bool system
        {
            get => (buttons & SantrollerV2Button.System) != 0;
            set => buttons.SetBit(SantrollerV2Button.System, value);
        }

        public bool share
        {
            get => (buttons & SantrollerV2Button.Share) != 0;
            set => buttons.SetBit(SantrollerV2Button.Share, value);
        }

        public bool south
        {
            get => (buttons & SantrollerV2Button.South) != 0;
            set => buttons.SetBit(SantrollerV2Button.South, value);
        }

        public bool east
        {
            get => (buttons & SantrollerV2Button.East) != 0;
            set => buttons.SetBit(SantrollerV2Button.East, value);
        }

        public bool west
        {
            get => (buttons & SantrollerV2Button.West) != 0;
            set => buttons.SetBit(SantrollerV2Button.West, value);
        }

        public bool north
        {
            get => (buttons & SantrollerV2Button.North) != 0;
            set => buttons.SetBit(SantrollerV2Button.North, value);
        }

        public bool dpadUp
        {
            get => buttons.GetDpad().IsUp();
            set => buttons.SetDpadUp(value);
        }

        public bool dpadDown
        {
            get => buttons.GetDpad().IsDown();
            set => buttons.SetDpadDown(value);
        }

        public bool dpadLeft
        {
            get => buttons.GetDpad().IsLeft();
            set => buttons.SetDpadLeft(value);
        }

        public bool dpadRight
        {
            get => buttons.GetDpad().IsRight();
            set => buttons.SetDpadRight(value);
        }

        public bool leftShoulder
        {
            get => (buttons & SantrollerV2Button.LeftShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftShoulder, value);
        }

        public bool rightShoulder
        {
            get => (buttons & SantrollerV2Button.RightShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightShoulder, value);
        }

        public bool leftTriggerPress
        {
            get => false;
            set { }
        }

        public bool rightTriggerPress
        {
            get => false;
            set { }
        }

        public bool leftStickPress
        {
            get => (buttons & SantrollerV2Button.LeftStickPress) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftStickPress, value);
        }

        public bool rightStickPress
        {
            get => (buttons & SantrollerV2Button.RightStickPress) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightStickPress, value);
        }

        byte IGamepadState.leftTrigger
        {
            get => leftTrigger;
            set => leftTrigger = value;
        }

        byte IGamepadState.rightTrigger
        {
            get => rightTrigger;
            set => rightTrigger = value;
        }

        sbyte IGamepadState.leftStickX
        {
            get => XInputDefinitions.GetAxisX(leftStickX);
            set => leftStickX = XInputDefinitions.SetAxisX(value);
        }

        sbyte IGamepadState.leftStickY
        {
            get => XInputDefinitions.GetAxisY(leftStickY);
            set => leftStickY = XInputDefinitions.SetAxisY(value);
        }

        sbyte IGamepadState.rightStickX
        {
            get => XInputDefinitions.GetAxisX(rightStickX);
            set => rightStickX = XInputDefinitions.SetAxisX(value);
        }

        sbyte IGamepadState.rightStickY
        {
            get => XInputDefinitions.GetAxisY(rightStickY);
            set => rightStickY = XInputDefinitions.SetAxisY(value);
        }
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
