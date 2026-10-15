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
    internal struct SantrollerV2HIDRockBandGuitarState : IRockBandGuitarState_Flags
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

        public bool green
        {
            get => (buttons & SantrollerV2Button.A) != 0;
            set => buttons.SetBit(SantrollerV2Button.A, value);
        }

        public bool red
        {
            get => (buttons & SantrollerV2Button.B) != 0;
            set => buttons.SetBit(SantrollerV2Button.B, value);
        }

        public bool yellow
        {
            get => (buttons & SantrollerV2Button.Y) != 0;
            set => buttons.SetBit(SantrollerV2Button.Y, value);
        }

        public bool blue
        {
            get => (buttons & SantrollerV2Button.X) != 0;
            set => buttons.SetBit(SantrollerV2Button.X, value);
        }

        public bool orange
        {
            get => (buttons & SantrollerV2Button.LeftShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftShoulder, value);
        }

        public bool solo
        {
            get => (buttons & SantrollerV2Button.LeftThumb) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftThumb, value);
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

        public byte whammy
        {
            get => (byte)((rightStickX >> 8) + 0x80);
            set => rightStickX = (short)(((value - 0x80) << 8) | value);
        }

        public sbyte tilt
        {
            get => (sbyte)(rightStickY >> 8);
            set => rightStickY = (short)((value << 8) | (byte)value);
        }

        public int pickupSwitch
        {
            get => RockBandGuitarState.GetPickupSwitchNotch(leftTrigger);
            set => leftTrigger = RockBandGuitarState.SetPickupSwitchNotch(value);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Santroller V2 HID Rock Band Guitar", hideInUI = true)]
    internal class SantrollerV2HIDRockBandGuitar : TranslatingRockBandGuitar_Flags<SantrollerV2HIDRockBandGuitarState>,
        ISantrollerFiveFretGuitarHaptics
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerV2HIDRockBandGuitar>();
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFiveFretGuitarHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerFiveFretGuitarHaptics m_Haptics;

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
        public void SetHitNotes(FiveFretGuitarHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}

