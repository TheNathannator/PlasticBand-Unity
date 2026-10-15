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
    internal struct SantrollerV2HIDTurntableState : ITurntableState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public SantrollerV2HIDReport report;

        public bool south
        {
            get => report.south;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x10 : report.buttons2 & ~0x10);
        }

        public bool east
        {
            get => report.east;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x20 : report.buttons2 & ~0x20);
        }

        public bool west
        {
            get => report.west;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x40 : report.buttons2 & ~0x40);
        }

        public bool north_euphoria
        {
            get => report.north;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x80 : report.buttons2 & ~0x80);
        }

        public bool dpadUp
        {
            get => report.dpadUp;
            set => report.buttons1 = (byte)((report.buttons1 & 0xF0) | (byte)(value ? HidDpad.Up : HidDpad.Neutral));
        }

        public bool dpadRight
        {
            get => report.dpadRight;
            set => report.buttons1 = (byte)((report.buttons1 & 0xF0) | (byte)(value ? HidDpad.Right : HidDpad.Neutral));
        }

        public bool dpadDown
        {
            get => report.dpadDown;
            set => report.buttons1 = (byte)((report.buttons1 & 0xF0) | (byte)(value ? HidDpad.Down : HidDpad.Neutral));
        }

        public bool dpadLeft
        {
            get => report.dpadLeft;
            set => report.buttons1 = (byte)((report.buttons1 & 0xF0) | (byte)(value ? HidDpad.Left : HidDpad.Neutral));
        }

        public bool start
        {
            get => report.start;
            set => report.buttons1 = (byte)(value ? report.buttons1 | 0x10 : report.buttons1 & ~0x10);
        }

        public bool select
        {
            get => report.back;
            set => report.buttons1 = (byte)(value ? report.buttons1 | 0x20 : report.buttons1 & ~0x20);
        }

        public bool system
        {
            get => report.guide;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x04 : report.buttons2 & ~0x04);
        }

        public bool leftGreen
        {
            get => (report.leftTrigger & 0x01) != 0;
            set => report.leftTrigger = (byte)(value ? report.leftTrigger | 0x01 : report.leftTrigger & ~0x01);
        }

        public bool leftRed
        {
            get => (report.leftTrigger & 0x02) != 0;
            set => report.leftTrigger = (byte)(value ? report.leftTrigger | 0x02 : report.leftTrigger & ~0x02);
        }

        public bool leftBlue
        {
            get => (report.leftTrigger & 0x04) != 0;
            set => report.leftTrigger = (byte)(value ? report.leftTrigger | 0x04 : report.leftTrigger & ~0x04);
        }

        public bool rightGreen
        {
            get => (report.rightTrigger & 0x01) != 0;
            set => report.rightTrigger = (byte)(value ? report.rightTrigger | 0x01 : report.rightTrigger & ~0x01);
        }

        public bool rightRed
        {
            get => (report.rightTrigger & 0x02) != 0;
            set => report.rightTrigger = (byte)(value ? report.rightTrigger | 0x02 : report.rightTrigger & ~0x02);
        }

        public bool rightBlue
        {
            get => (report.rightTrigger & 0x04) != 0;
            set => report.rightTrigger = (byte)(value ? report.rightTrigger | 0x04 : report.rightTrigger & ~0x04);
        }

        public sbyte leftVelocity
        {
            get => (sbyte)(report.leftStickX >> 8);
            set => report.leftStickX = (short)(value << 8);
        }

        public sbyte rightVelocity
        {
            get => (sbyte)(report.leftStickY >> 8);
            set => report.leftStickY = (short)(value << 8);
        }

        public sbyte crossfader
        {
            get => (sbyte)(report.rightStickX >> 8);
            set => report.rightStickX = (short)(value << 8);
        }

        public ushort effectsDial
        {
            get => (ushort)(report.rightStickY + 0x8000);
            set => report.rightStickY = (short)(value - 0x8000);
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
