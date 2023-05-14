using System;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Handles haptics for Santroller devices.
    /// </summary>
    internal class SantrollerHaptics : StageKitHaptics, ISantrollerHaptics
    {
        protected enum SantrollerCommandId : byte
        {
            StarPowerFill = 0x08,
            StarPowerActive = 0x09,
            Multiplier = 0x0A,
            SoloActive = 0x0B
        }

        private readonly ISantrollerHapticsSender m_Sender;

        private float m_StarPowerFill;
        private bool m_StarPowerActive;
        private uint m_Multiplier = 1;
        private bool m_SoloActive;

        public SantrollerHaptics(InputDevice device) : base(device)
        {
            switch (device.description.interfaceName)
            {
                case HidDefinitions.InterfaceName: m_Sender = new HidSantrollerHapticsSender(); break;
                case XInputLayoutFinder.InterfaceName: m_Sender = new XInputSantrollerHapticsSender(); break;
                default: throw new ArgumentException($"Unsupported interface type '{device.description.interfaceName}'!", nameof(device));
            }
        }

        protected override void OnHapticsResumed()
        {
            base.OnHapticsResumed();
            SendStarPowerFill(m_StarPowerFill);
            SendStarPowerActive(m_StarPowerActive);
            SendMultiplier(m_Multiplier);
            SendSolo(m_SoloActive);
        }

        protected override void OnHapticsReset()
        {
            base.OnHapticsReset();
            m_StarPowerFill = 0f;
            m_StarPowerActive = false;
            m_Multiplier = 1;
            m_SoloActive = false;
        }

        public void SetStarPowerFill(float fill)
        {
            m_StarPowerFill = fill;
            if (m_HapticsEnabled)
                SendStarPowerFill(fill);
        }

        public void SetStarPowerActive(bool enabled)
        {
            m_StarPowerActive = enabled;
            if (m_HapticsEnabled)
                SendStarPowerActive(enabled);
        }

        public void SetMultiplier(uint multiplier)
        {
            m_Multiplier = multiplier;
            if (m_HapticsEnabled)
                SendMultiplier(multiplier);
        }

        public void SetSolo(bool enabled)
        {
            m_SoloActive = enabled;
            if (m_HapticsEnabled)
                SendSolo(enabled);
        }

        private void SendStarPowerFill(float fill)
        {
            fill = Mathf.Clamp(fill, 0f, 1f) * byte.MaxValue;
            SendCommand(SantrollerCommandId.StarPowerFill, (byte)fill);
        }

        private void SendStarPowerActive(bool enabled)
        {
            SendCommand(SantrollerCommandId.StarPowerActive, (byte)(enabled ? 1 : 0));
        }

        private void SendMultiplier(uint multiplier)
        {
            if (multiplier > 245)
                multiplier = 245;
            else if (multiplier < 1)
                multiplier = 1;

            SendCommand(SantrollerCommandId.StarPowerActive, (byte)(multiplier + 10));
        }

        private void SendSolo(bool enabled)
        {
            SendCommand(SantrollerCommandId.SoloActive, (byte)(enabled ? 1 : 0));
        }

        protected void SendCommand(byte commandId, byte parameter = 0)
            => SendCommand(m_Device, commandId, parameter);

        private void SendCommand(SantrollerCommandId commandId, byte parameter = 0)
            => SendCommand((byte)commandId, parameter);

        protected override void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
            => m_Sender.SendCommand(device, commandId, parameter);
    }
}