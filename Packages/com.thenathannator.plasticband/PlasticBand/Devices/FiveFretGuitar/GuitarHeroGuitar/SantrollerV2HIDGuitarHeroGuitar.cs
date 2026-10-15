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
    internal struct SantrollerV2HIDGuitarHeroGuitarState : IGuitarHeroGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public SantrollerV2HIDReport report;

        public bool green
        {
            get => report.south;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x10 : report.buttons2 & ~0x10);
        }

        public bool red
        {
            get => report.east;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x20 : report.buttons2 & ~0x20);
        }

        public bool yellow
        {
            get => report.north;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x80 : report.buttons2 & ~0x80);
        }

        public bool blue
        {
            get => report.west;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x40 : report.buttons2 & ~0x40);
        }

        public bool orange
        {
            get => report.leftShoulder;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x01 : report.buttons2 & ~0x01);
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

        public byte whammy
        {
            get => (byte)((report.rightStickX >> 8) + 0x80);
            set => report.rightStickX = (short)(((value - 0x80) << 8) | value);
        }

        public sbyte tilt
        {
            get => (sbyte)(report.rightStickY >> 8);
            set => report.rightStickY = (short)((value << 8) | (byte)value);
        }

        public bool spPedal
        {
            get => report.rightShoulder;
            set => report.buttons2 = (byte)(value ? report.buttons2 | 0x02 : report.buttons2 & ~0x02);
        }

        public byte rawTouchBar
        {
            get => (byte)((report.leftStickX >> 8) ^ 0x80);
            set => report.leftStickX = (short)((byte)(value ^ 0x80) << 8);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Santroller V2 HID Guitar Hero Guitar", hideInUI = true)]
    internal class SantrollerV2HIDGuitarHeroGuitar : TranslatingGuitarHeroGuitar_Discrete<SantrollerV2HIDGuitarHeroGuitarState>,
        ISantrollerFiveFretGuitarHaptics
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerV2HIDGuitarHeroGuitar>();
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
