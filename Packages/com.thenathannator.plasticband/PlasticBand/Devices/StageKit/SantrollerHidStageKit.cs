using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Stage%20Kit/Santroller.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHidStageKitState : IGamepadState
    {
        [Flags]
        public enum Button : ushort
        {
            None = 0,

            South = 0x0001,
            East = 0x0002,
            West = 0x0004,
            North = 0x0008,

            LeftShoulder = 0x0010,
            RightShoulder = 0x0020,
            LeftTriggerPress = 0x0040,
            RightTriggerPress = 0x0080,

            Select = 0x0100,
            Start = 0x0200,
            LeftStickClick = 0x0400,
            RightStickClick = 0x0800,

            System = 0x1000,
            Capture = 0x2000,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "buttonSouth", layout = "Button", bit = 0)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 1)]
        [InputControl(name = "buttonWest", layout = "Button", bit = 2)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3)]

        [InputControl(name = "leftShoulder", layout = "Button", bit = 4)]
        [InputControl(name = "rightShoulder", layout = "Button", bit = 5)]
        [InputControl(name = "leftTriggerButton", layout = "Button", bit = 6)]
        [InputControl(name = "rightTriggerButton", layout = "Button", bit = 7)]

        [InputControl(name = "select", layout = "Button", bit = 8)]
        [InputControl(name = "start", layout = "Button", bit = 9)]
        [InputControl(name = "leftStickPress", layout = "Button", bit = 10)]
        [InputControl(name = "rightStickPress", layout = "Button", bit = 11)]

        [InputControl(name = "systemButton", layout = "Button", bit = 12)]
        [InputControl(name = "shareButton", layout = "Button", bit = 13)]
        public Button buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public HidDpad dpad;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", offset = 0, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        [InputControl(name = "leftStick/left", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "leftStick/right", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", offset = 1, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0x80")]
        [InputControl(name = "leftStick/up", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "leftStick/down", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        public byte leftStickX;
        public byte leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", offset = 0, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        [InputControl(name = "rightStick/left", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "rightStick/right", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", offset = 1, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0x80")]
        [InputControl(name = "rightStick/up", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "rightStick/down", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        public byte rightStickX;
        public byte rightStickY;

        [InputControl(name = "leftTrigger", layout = "IntButton", format = "BYTE", parameters = "minValue=0x00,maxValue=0xFF")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "IntButton", format = "BYTE", parameters = "minValue=0x00,maxValue=0xFF")]
        public byte rightTrigger;

        public bool start
        {
            get => (buttons & Button.Start) != 0;
            set => buttons.SetBit(Button.Start, value);
        }

        public bool select
        {
            get => (buttons & Button.Select) != 0;
            set => buttons.SetBit(Button.Select, value);
        }

        public bool system
        {
            get => (buttons & Button.System) != 0;
            set => buttons.SetBit(Button.System, value);
        }

        public bool share
        {
            get => (buttons & Button.Capture) != 0;
            set => buttons.SetBit(Button.Capture, value);
        }

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

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => dpad.SetUp(value);
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

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => dpad.SetRight(value);
        }

        public bool leftShoulder
        {
            get => (buttons & Button.LeftShoulder) != 0;
            set => buttons.SetBit(Button.LeftShoulder, value);
        }

        public bool rightShoulder
        {
            get => (buttons & Button.RightShoulder) != 0;
            set => buttons.SetBit(Button.RightShoulder, value);
        }

        public bool leftTriggerPress
        {
            get => (buttons & Button.LeftTriggerPress) != 0;
            set => buttons.SetBit(Button.LeftTriggerPress, value);
        }

        public bool rightTriggerPress
        {
            get => (buttons & Button.RightTriggerPress) != 0;
            set => buttons.SetBit(Button.RightTriggerPress, value);
        }

        public bool leftStickPress
        {
            get => (buttons & Button.LeftStickClick) != 0;
            set => buttons.SetBit(Button.LeftStickClick, value);
        }

        public bool rightStickPress
        {
            get => (buttons & Button.RightStickClick) != 0;
            set => buttons.SetBit(Button.RightStickClick, value);
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
            get => HidDefinitions.GetByteX(leftStickX);
            set => leftStickX = HidDefinitions.SetByteX(value);
        }

        sbyte IGamepadState.leftStickY
        {
            get => HidDefinitions.GetInvertedByteY(leftStickY);
            set => leftStickY = HidDefinitions.SetInvertedByteY(value);
        }

        sbyte IGamepadState.rightStickX
        {
            get => HidDefinitions.GetByteX(rightStickX);
            set => rightStickX = HidDefinitions.SetByteX(value);
        }

        sbyte IGamepadState.rightStickY
        {
            get => HidDefinitions.GetInvertedByteY(rightStickY);
            set => rightStickY = HidDefinitions.SetInvertedByteY(value);
        }
    }

    [InputControlLayout(stateType = typeof(SantrollerHidStageKitState), displayName = "Santroller HID Stage Kit")]
    internal class SantrollerHidStageKit : StageKit, ISantrollerHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHidStageKit>(SantrollerDeviceType.StageKit);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerHaptics m_Haptics;

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public override void PauseHaptics() => m_Haptics.PauseHaptics();

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public override void ResumeHaptics() => m_Haptics.ResumeHaptics();

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public override void ResetHaptics() => m_Haptics.ResetHaptics();

        /// <inheritdoc cref="IStageKitHaptics.SetFogMachine(bool)"/>
        public override void SetFogMachine(bool enabled) => m_Haptics.SetFogMachine(enabled);

        /// <inheritdoc cref="IStageKitHaptics.SetStrobeSpeed(StageKitStrobeSpeed)"/>
        public override void SetStrobeSpeed(StageKitStrobeSpeed speed) => m_Haptics.SetStrobeSpeed(speed);

        /// <inheritdoc cref="IStageKitHaptics.SetLeds(StageKitLedColor, StageKitLed)"/>
        public override void SetLeds(StageKitLedColor color, StageKitLed leds) => m_Haptics.SetLeds(color, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetRedLeds(StageKitLed)"/>
        public override void SetRedLeds(StageKitLed leds) => m_Haptics.SetRedLeds(leds);

        /// <inheritdoc cref="IStageKitHaptics.SetYellowLeds(StageKitLed)"/>
        public override void SetYellowLeds(StageKitLed leds) => m_Haptics.SetYellowLeds(leds);

        /// <inheritdoc cref="IStageKitHaptics.SetBlueLeds(StageKitLed)"/>
        public override void SetBlueLeds(StageKitLed leds) => m_Haptics.SetBlueLeds(leds);

        /// <inheritdoc cref="IStageKitHaptics.SetGreenLeds(StageKitLed)"/>
        public override void SetGreenLeds(StageKitLed leds) => m_Haptics.SetGreenLeds(leds);

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
    }
}
