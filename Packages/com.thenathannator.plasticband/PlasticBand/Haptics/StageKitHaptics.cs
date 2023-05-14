using System;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Handles haptics for Rock Band stage kits.
    /// </summary>
    internal abstract class StageKitHaptics : IStageKitHaptics
    {
        protected enum StageKitCommandId : byte
        {
            FogOn = 0x01,
            FogOff = 0x02,

            StrobeSlow = 0x03,
            StrobeMedium = 0x04,
            StrobeFast = 0x05,
            StrobeFastest = 0x06,
            StrobeOff = 0x07,

            BlueLeds = 0x20,
            GreenLeds = 0x40,
            YellowLeds = 0x60,
            RedLeds = 0x80,

            Reset = 0xFF
        }

        protected readonly InputDevice m_Device;

        protected bool m_HapticsEnabled = true;
        private bool m_FogEnabled;
        private StageKitStrobeSpeed m_StrobeSpeed;
        private StageKitLed m_RedLeds;
        private StageKitLed m_YellowLeds;
        private StageKitLed m_BlueLeds;
        private StageKitLed m_GreenLeds;

        public StageKitHaptics(InputDevice device)
        {
            m_Device = device ?? throw new ArgumentNullException(nameof(device));
        }

        public void PauseHaptics()
        {
            if (!m_HapticsEnabled)
                return;

            m_HapticsEnabled = false;
            OnHapticsPaused();
        }

        public void ResumeHaptics()
        {
            if (m_HapticsEnabled)
                return;

            m_HapticsEnabled = true;
            OnHapticsResumed();
        }

        public void ResetHaptics()
        {
            m_HapticsEnabled = true;
            OnHapticsReset();
        }

        protected virtual void OnHapticsPaused()
        {
            SendReset();
        }

        protected virtual void OnHapticsResumed()
        {
            SendFogMachine(m_FogEnabled);
            SendStrobeSpeed(m_StrobeSpeed);
            SendLeds(StageKitLedColor.Red, m_RedLeds);
            SendLeds(StageKitLedColor.Yellow, m_YellowLeds);
            SendLeds(StageKitLedColor.Blue, m_BlueLeds);
            SendLeds(StageKitLedColor.Green, m_GreenLeds);
        }

        protected virtual void OnHapticsReset()
        {
            m_FogEnabled = false;
            m_StrobeSpeed = StageKitStrobeSpeed.Off;
            m_RedLeds = StageKitLed.None;
            m_YellowLeds = StageKitLed.None;
            m_BlueLeds = StageKitLed.None;
            m_GreenLeds = StageKitLed.None;
            SendReset();
        }

        public void SetFogMachine(bool enabled)
        {
            m_FogEnabled = enabled;
            if (m_HapticsEnabled)
                SendFogMachine(enabled);
        }

        public void SetStrobeSpeed(StageKitStrobeSpeed speed)
        {
            m_StrobeSpeed = speed;
            if (m_HapticsEnabled)
                SendStrobeSpeed(speed);
        }

        public void SetLeds(StageKitLedColor color, StageKitLed leds)
        {
            if ((color & StageKitLedColor.Red) != 0)
                m_RedLeds = leds;
            if ((color & StageKitLedColor.Yellow) != 0)
                m_YellowLeds = leds;
            if ((color & StageKitLedColor.Blue) != 0)
                m_BlueLeds = leds;
            if ((color & StageKitLedColor.Green) != 0)
                m_GreenLeds = leds;

            if (m_HapticsEnabled)
                SendLeds(color, leds);
        }

        public void SetRedLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Red, leds);

        public void SetYellowLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Yellow, leds);

        public void SetBlueLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Blue, leds);

        public void SetGreenLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Green, leds);

        private void SendFogMachine(bool enabled)
        {
            SendCommand(enabled ? StageKitCommandId.FogOn : StageKitCommandId.FogOff);
        }

        private void SendStrobeSpeed(StageKitStrobeSpeed speed)
        {
            StageKitCommandId id;
            switch (speed)
            {
                default: id = StageKitCommandId.StrobeOff; break;
                case StageKitStrobeSpeed.Slow: id = StageKitCommandId.StrobeSlow; break;
                case StageKitStrobeSpeed.Medium: id = StageKitCommandId.StrobeMedium; break;
                case StageKitStrobeSpeed.Fast: id = StageKitCommandId.StrobeFast; break;
                case StageKitStrobeSpeed.Fastest: id = StageKitCommandId.StrobeFastest; break;
            }

            SendCommand(id);
        }

        private void SendLeds(StageKitLedColor color, StageKitLed leds)
        {
            // StageKitLedColor is a bitmask, so multiple colors can be set at once
            if ((color & StageKitLedColor.Red) != 0)
                SendCommand(StageKitCommandId.RedLeds, (byte)leds);

            if ((color & StageKitLedColor.Yellow) != 0)
                SendCommand(StageKitCommandId.YellowLeds, (byte)leds);

            if ((color & StageKitLedColor.Blue) != 0)
                SendCommand(StageKitCommandId.BlueLeds, (byte)leds);

            if ((color & StageKitLedColor.Green) != 0)
                SendCommand(StageKitCommandId.GreenLeds, (byte)leds);

        }

        private void SendReset()
        {
            SendCommand(StageKitCommandId.Reset);
        }

        protected void SendCommand(StageKitCommandId commandId, byte parameter = 0)
            => SendCommand(m_Device, (byte)commandId, parameter);

        protected abstract void SendCommand(InputDevice device, byte commandId, byte parameter);
    }

    internal class XInputStageKitHaptics : StageKitHaptics
    {
        public XInputStageKitHaptics(InputDevice device) : base(device)
        { }

        protected override void SendCommand(InputDevice device, byte commandId, byte parameter)
        {
            var command = new XInputVibrationCommand(parameter, commandId);
            device.ExecuteCommand(ref command);
        }
    }
}