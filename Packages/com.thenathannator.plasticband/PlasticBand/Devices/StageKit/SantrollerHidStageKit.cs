using System;
using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Stage%20Kit/Santroller.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHidStageKitState : IInputStateTypeInfo
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
        // May conflict with the regular trigger axes
        // [InputControl(name = "leftTriggerPress", layout = "Button", bit = 6)]
        // [InputControl(name = "rightTriggerPress", layout = "Button", bit = 7)]

        [InputControl(name = "select", layout = "Button", bit = 8)]
        [InputControl(name = "start", layout = "Button", bit = 9)]
        [InputControl(name = "leftStickPress", layout = "Button", bit = 10)]
        [InputControl(name = "rightStickPress", layout = "Button", bit = 11)]

        [InputControl(name = "systemButton", layout = "Button", bit = 12)]
        [InputControl(name = "captureButton", layout = "Button", bit = 13)]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", format = "BYTE", offset = 0, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "leftStick/left", format = "BYTE", offset = 0, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/right", format = "BYTE", offset = 0, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]

        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", format = "BYTE", offset = 1, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
        [InputControl(name = "leftStick/up", format = "BYTE", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "leftStick/down", format = "BYTE", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
        public byte leftStickX;
        public byte leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", format = "BYTE", offset = 0, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        [InputControl(name = "rightStick/left", format = "BYTE", offset = 0, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/right", format = "BYTE", offset = 0, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1")]

        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", format = "BYTE", offset = 1, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
        [InputControl(name = "rightStick/up", format = "BYTE", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "rightStick/down", format = "BYTE", offset = 1, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=1,clampMin=0.5,clampMax=1,invert=false")]
        public byte rightStickX;
        public byte rightStickY;

        [InputControl(name = "leftTrigger", layout = "Button", format = "BYTE")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "Button", format = "BYTE")]
        public byte rightTrigger;
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
