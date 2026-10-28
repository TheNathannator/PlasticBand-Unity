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
    internal struct SantrollerV2HIDTurntableState : ITurntableState
    {
        [Flags]
        public enum PlatterButton : byte
        {
            None = 0,

            Green = 0x01,
            Red = 0x02,
            Blue = 0x04,
        }

        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public byte reserved;
        public SantrollerV2Button buttons;
        public PlatterButton leftTable;
        public PlatterButton rightTable;
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

        public bool north_euphoria
        {
            get => (buttons & SantrollerV2Button.North) != 0;
            set => buttons.SetBit(SantrollerV2Button.North, value);
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

        public bool leftGreen
        {
            get => (leftTable & PlatterButton.Green) != 0;
            set => leftTable.SetBit(PlatterButton.Green, value);
        }

        public bool leftRed
        {
            get => (leftTable & PlatterButton.Red) != 0;
            set => leftTable.SetBit(PlatterButton.Red, value);
        }

        public bool leftBlue
        {
            get => (leftTable & PlatterButton.Blue) != 0;
            set => leftTable.SetBit(PlatterButton.Blue, value);
        }

        public bool rightGreen
        {
            get => (rightTable & PlatterButton.Green) != 0;
            set => rightTable.SetBit(PlatterButton.Green, value);
        }

        public bool rightRed
        {
            get => (rightTable & PlatterButton.Red) != 0;
            set => rightTable.SetBit(PlatterButton.Red, value);
        }

        public bool rightBlue
        {
            get => (rightTable & PlatterButton.Blue) != 0;
            set => rightTable.SetBit(PlatterButton.Blue, value);
        }

        public sbyte leftVelocity
        {
            get => (sbyte)(leftStickX >> 8);
            set => leftStickX = (short)(value << 8);
        }

        public sbyte rightVelocity
        {
            get => (sbyte)(leftStickY >> 8);
            set => leftStickY = (short)(value << 8);
        }

        public sbyte crossfader
        {
            get => (sbyte)(rightStickX >> 8);
            set => rightStickX = (short)(value << 8);
        }

        public ushort effectsDial
        {
            get => (ushort)(rightStickY + 0x8000);
            set => rightStickY = (short)(value - 0x8000);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller V2 HID Turntable", hideInUI = true)]
    internal class SantrollerV2HIDTurntable : TranslatingTurntable<SantrollerV2HIDTurntableState>,
        ISantrollerTurntableHaptics
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerV2HIDTurntable>();
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerTurntableHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerTurntableHaptics m_Haptics;

        public override void PauseHaptics() => m_Haptics.PauseHaptics();
        public override void ResumeHaptics() => m_Haptics.ResumeHaptics();
        public override void ResetHaptics() => m_Haptics.ResetHaptics();
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
        public void SetHitNotes(TurntableHitNote notes) => m_Haptics.SetHitNotes(notes);
        public void SetEuphoriaBrightness(float brightness) => m_Haptics.SetEuphoriaBrightness(brightness);
    }
}
