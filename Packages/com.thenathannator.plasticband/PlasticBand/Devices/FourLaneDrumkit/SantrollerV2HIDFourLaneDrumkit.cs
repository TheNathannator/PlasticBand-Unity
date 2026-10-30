using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
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
    internal struct SantrollerV2HIDFourLaneDrumkitState : IFourLaneDrumkitState_FlagButtons,
        IFourLaneDrumkitState_SharedVelocities
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public byte reserved;
        public SantrollerV2Button buttons;
        public byte leftTrigger;
        public byte rightTrigger;
        public short leftStickX;
        public short leftStickY;
        public short rightStickX;
        public short rightStickY;

        public HidDpad dpad
        {
            get => buttons.GetDpad();
            set => buttons.SetDpad(value);
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

        public bool kick1
        {
            get => (buttons & SantrollerV2Button.LeftShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftShoulder, value);
        }

        public bool kick2
        {
            get => (buttons & SantrollerV2Button.LeftStick) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftStick, value);
        }

        public bool pad
        {
            get => (buttons & SantrollerV2Button.RightStick) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightStick, value);
        }

        public bool cymbal
        {
            get => (buttons & SantrollerV2Button.RightShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightShoulder, value);
        }

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => buttons.SetDpadUp(value);
        }

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => buttons.SetDpadRight(value);
        }

        public bool dpadDown
        {
            get => dpad.IsDown();
            set => buttons.SetDpadDown(value);
        }

        public bool dpadLeft
        {
            get => dpad.IsLeft();
            set => buttons.SetDpadLeft(value);
        }

        public bool start
        {
            get => (buttons & SantrollerV2Button.Start) != 0;
            set => buttons.SetBit(SantrollerV2Button.Start, value);
        }

        public bool select
        {
            get => (buttons & SantrollerV2Button.Back) != 0;
            set => buttons.SetBit(SantrollerV2Button.Back, value);
        }

        public bool system
        {
            get => (buttons & SantrollerV2Button.Guide) != 0;
            set => buttons.SetBit(SantrollerV2Button.Guide, value);
        }

        public byte redVelocity
        {
            get => (byte)(leftStickX >> 8);
            set => leftStickX = (short)(value << 8);
        }

        public byte yellowVelocity
        {
            get => (byte)(leftStickY >> 8);
            set => leftStickY = (short)(value << 8);
        }

        public byte blueVelocity
        {
            get => (byte)(rightStickX >> 8);
            set => rightStickX = (short)(value << 8);
        }

        public byte greenVelocity
        {
            get => (byte)(rightStickY >> 8);
            set => rightStickY = (short)(value << 8);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Santroller V2 HID Rock Band Drumkit", hideInUI = true)]
    internal class SantrollerV2HIDFourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<SantrollerV2HIDFourLaneDrumkitState>,
        ISantrollerFourLaneDrumkitHaptics
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerV2HIDFourLaneDrumkit>();
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFourLaneDrumkitHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerFourLaneDrumkitHaptics m_Haptics;

        public void PauseHaptics() => m_Haptics.PauseHaptics();
        public void ResumeHaptics() => m_Haptics.ResumeHaptics();
        public void ResetHaptics() => m_Haptics.ResetHaptics();
        public void SetFogMachine(bool enabled) => m_Haptics.SetFogMachine(enabled);
        public void SetStrobeSpeed(StageKitStrobeSpeed speed) => m_Haptics.SetStrobeSpeed(speed);
        public void SetLeds(StageKitLedColor color, StageKitLed leds) => m_Haptics.SetLeds(color, leds);
        public void SetRedLeds(StageKitLed leds) => m_Haptics.SetRedLeds(leds);
        public void SetYellowLeds(StageKitLed leds) => m_Haptics.SetYellowLeds(leds);
        public void SetBlueLeds(StageKitLed leds) => m_Haptics.SetBlueLeds(leds);
        public void SetGreenLeds(StageKitLed leds) => m_Haptics.SetGreenLeds(leds);
        public void SetStarPowerFill(float fill) => m_Haptics.SetStarPowerFill(fill);
        public void SetStarPowerActive(bool enabled) => m_Haptics.SetStarPowerActive(enabled);
        public void SetMultiplier(byte multiplier) => m_Haptics.SetMultiplier(multiplier);
        public void SetSoloActive(bool enabled) => m_Haptics.SetSoloActive(enabled);
        public void SetNoteMiss(bool enabled) => m_Haptics.SetNoteMiss(enabled);
        public void SetHitNotes(FourLaneDrumkitHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}

