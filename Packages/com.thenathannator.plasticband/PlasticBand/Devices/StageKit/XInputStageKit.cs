using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Stage%20Kit/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputStageKitState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "start", layout = "Button", format = "BIT", bit = 4)]
        [InputControl(name = "select", layout = "Button", format = "BIT", bit = 5)]
        [InputControl(name = "leftStickPress", layout = "Button", format = "BIT", bit = 6)]
        [InputControl(name = "rightStickPress", layout = "Button", format = "BIT", bit = 7)]

        [InputControl(name = "leftShoulder", layout = "Button", format = "BIT", bit = 8)]
        [InputControl(name = "rightShoulder", layout = "Button", format = "BIT", bit = 9)]

        [InputControl(name = "buttonSouth", layout = "Button", format = "BIT", bit = 12)]
        [InputControl(name = "buttonEast", layout = "Button", format = "BIT", bit = 13)]
        [InputControl(name = "buttonWest", layout = "Button", format = "BIT", bit = 14)]
        [InputControl(name = "buttonNorth", layout = "Button", format = "BIT", bit = 15)]
        public ushort buttons;

        [InputControl(name = "leftTrigger", layout = "Button", format = "BYTE")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "Button", format = "BYTE")]
        public byte rightTrigger;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2S")]
        [InputControl(name = "leftStick/x", format = "SHRT", offset = 0, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/left", format = "SHRT", offset = 0)]
        [InputControl(name = "leftStick/right", format = "SHRT", offset = 0)]

        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "leftStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "leftStick/down", format = "SHRT", offset = 2)]
        public short leftStickX;
        public short leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2S")]
        [InputControl(name = "rightStick/x", format = "SHRT", offset = 0, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/left", format = "SHRT", offset = 0)]
        [InputControl(name = "rightStick/right", format = "SHRT", offset = 0)]

        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", format = "SHRT", offset = 2, parameters = "clamp=false,invert=false,normalize=false")]
        [InputControl(name = "rightStick/up", format = "SHRT", offset = 2)]
        [InputControl(name = "rightStick/down", format = "SHRT", offset = 2)]
        public short rightStickX;
        public short rightStickY;
    }

    [InputControlLayout(stateType = typeof(XInputStageKitState), displayName = "XInput Rock Band Stage Kit")]
    internal class XInputStageKit : StageKit
    {
        internal static new void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputStageKit>(XInputNonStandardSubType.StageKit);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = StageKitHaptics.Create(this, StageKitProtocol.XInput);
        }

        private StageKitHaptics m_Haptics;

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
    }
}