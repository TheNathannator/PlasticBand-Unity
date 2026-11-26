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
    internal struct SantrollerV2HIDFiveLaneDrumkitState : IFiveLaneDrumkitState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public byte reserved;
        public SantrollerV2Button buttons;
        private unsafe fixed byte unused[4];

        public byte m_GreenVelocity;
        public byte m_RedVelocity;
        public byte m_YellowVelocity;
        public byte m_BlueVelocity;
        public byte m_OrangeVelocity;
        public byte m_KickVelocity;

        public HidDpad dpad
        {
            get => buttons.GetDpad();
            set => buttons.SetDpad(value);
        }

        public bool red_east
        {
            get => (buttons & SantrollerV2Button.East) != 0;
            set => buttons.SetBit(SantrollerV2Button.East, value);
        }

        public bool yellow_north
        {
            get => (buttons & SantrollerV2Button.North) != 0;
            set => buttons.SetBit(SantrollerV2Button.North, value);
        }

        public bool blue_west
        {
            get => (buttons & SantrollerV2Button.West) != 0;
            set => buttons.SetBit(SantrollerV2Button.West, value);
        }

        public bool green_south
        {
            get => (buttons & SantrollerV2Button.South) != 0;
            set => buttons.SetBit(SantrollerV2Button.South, value);
        }

        public bool orange
        {
            get => (buttons & SantrollerV2Button.RightShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightShoulder, value);
        }

        public bool kick
        {
            get => (buttons & SantrollerV2Button.LeftShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftShoulder, value);
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
            get => (buttons & SantrollerV2Button.Select) != 0;
            set => buttons.SetBit(SantrollerV2Button.Select, value);
        }

        public bool system
        {
            get => (buttons & SantrollerV2Button.System) != 0;
            set => buttons.SetBit(SantrollerV2Button.System, value);
        }

        public UInt7 greenVelocity
        {
            get => UInt7.Clamped(m_GreenVelocity);
            set => m_GreenVelocity = (byte)value;
        }

        public UInt7 redVelocity
        {
            get => UInt7.Clamped(m_RedVelocity);
            set => m_RedVelocity = (byte)value;
        }

        public UInt7 yellowVelocity
        {
            get => UInt7.Clamped(m_YellowVelocity);
            set => m_YellowVelocity = (byte)value;
        }

        public UInt7 blueVelocity
        {
            get => UInt7.Clamped(m_BlueVelocity);
            set => m_BlueVelocity = (byte)value;
        }

        public UInt7 orangeVelocity
        {
            get => UInt7.Clamped(m_OrangeVelocity);
            set => m_OrangeVelocity = (byte)value;
        }

        public UInt7 kickVelocity
        {
            get => UInt7.Clamped(m_KickVelocity);
            set => m_KickVelocity = (byte)value;
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Santroller V2 HID Guitar Hero Drumkit", hideInUI = true)]
    internal class SantrollerV2HIDFiveLaneDrumkit : TranslatingFiveLaneDrumkit<SantrollerV2HIDFiveLaneDrumkitState>,
        ISantrollerFiveLaneDrumkitHaptics
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerV2HIDFiveLaneDrumkit>(SantrollerHIDProtocol.V2, SantrollerDeviceType.GuitarHeroDrums);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = SantrollerFiveLaneDrumkitHaptics.Create(this, StageKitProtocol.SantrollerHID);
        }

        private SantrollerFiveLaneDrumkitHaptics m_Haptics;

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
        public void SetHitNotes(FiveLaneDrumkitHitNote notes) => m_Haptics.SetHitNotes(notes);
    }
}

