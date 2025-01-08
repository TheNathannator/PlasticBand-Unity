using PlasticBand.Devices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    internal abstract class SantrollerTurntableHaptics : SantrollerHaptics, ISantrollerTurntableHaptics
    {
        public class XInput : SantrollerTurntableHaptics
        {
            public XInput(InputDevice device) : base(device)
            { }

            protected override void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_XInput(device, commandId, parameter);
        }

        public class Hid : SantrollerTurntableHaptics
        {
            public Hid(InputDevice device) : base(device)
            { }

            protected override unsafe void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_Hid(device, commandId, parameter);
        }

        private enum SantrollerTurntableCommandId : byte
        {
            LeftScratch = 0x90,
            LeftGreen = 0x91,
            LeftRed = 0x92,
            LeftBlue = 0x93,

            RightGreen = 0x98,
            RightRed = 0x99,
            RightBlue = 0x9A,
            RightScratch = 0x9B,

            EuphoriaBrightness = 0xA0,
        }

        private TurntableButton m_ActiveLeftButtons;
        private TurntableButton m_ActiveRightButtons;
        private bool m_LeftScratchActive;
        private bool m_RightScratchActive;
        private float m_EuphoriaBrightness;

        public SantrollerTurntableHaptics(InputDevice device) : base(device)
        { }

        protected override void OnHapticsResumed()
        {
            base.OnHapticsResumed();
            SendNoteLights(m_ActiveLeftButtons, m_ActiveRightButtons, true);
            SendScratchLights(m_LeftScratchActive, m_RightScratchActive);
            SendEuphoriaBrightness(m_EuphoriaBrightness);
        }

        protected override void OnHapticsReset()
        {
            base.OnHapticsReset();
            m_ActiveLeftButtons = TurntableButton.None;
            m_ActiveRightButtons = TurntableButton.None;
            m_LeftScratchActive = false;
            m_RightScratchActive = false;
            m_EuphoriaBrightness = 0f;
        }

        public void SetNoteLights(TurntableButton left, TurntableButton right, bool enabled)
        {
            if (enabled)
            {
                m_ActiveLeftButtons |= left;
                m_ActiveRightButtons |= right;
            }
            else
            {
                m_ActiveLeftButtons &= ~left;
                m_ActiveRightButtons &= ~right;
            }

            if (m_HapticsEnabled)
                SendNoteLights(left, right, enabled);
        }

        public void SetScratchLights(bool left, bool right)
        {
            m_LeftScratchActive = left;
            m_RightScratchActive = right;

            if (m_HapticsEnabled)
                SendScratchLights(left, right);
        }

        public void SetEuphoriaBrightness(float brightness)
        {
            m_EuphoriaBrightness = brightness;
            if (m_HapticsEnabled)
                SendEuphoriaBrightness(brightness);
        }

        private void SendNoteLights(TurntableButton left, TurntableButton right, bool enabled)
        {
            if ((left & TurntableButton.Green) != 0 == enabled)
                SendCommand(SantrollerTurntableCommandId.LeftGreen, (byte)(enabled ? 1 : 0));
            if ((left & TurntableButton.Red) != 0 == enabled)
                SendCommand(SantrollerTurntableCommandId.LeftRed, (byte)(enabled ? 1 : 0));
            if ((left & TurntableButton.Blue) != 0 == enabled)
                SendCommand(SantrollerTurntableCommandId.LeftBlue, (byte)(enabled ? 1 : 0));

            if ((right & TurntableButton.Green) != 0 == enabled)
                SendCommand(SantrollerTurntableCommandId.RightGreen, (byte)(enabled ? 1 : 0));
            if ((right & TurntableButton.Red) != 0 == enabled)
                SendCommand(SantrollerTurntableCommandId.RightRed, (byte)(enabled ? 1 : 0));
            if ((right & TurntableButton.Blue) != 0 == enabled)
                SendCommand(SantrollerTurntableCommandId.RightBlue, (byte)(enabled ? 1 : 0));
        }

        private void SendScratchLights(bool left, bool right)
        {
            SendCommand(SantrollerTurntableCommandId.LeftScratch, (byte)(left ? 1 : 0));
            SendCommand(SantrollerTurntableCommandId.RightScratch, (byte)(right ? 1 : 0));
        }

        public void SendEuphoriaBrightness(float brightness)
        {
            brightness = Mathf.Clamp(brightness, 0f, 1f) * byte.MaxValue;
            SendCommand(SantrollerTurntableCommandId.EuphoriaBrightness, (byte)brightness);
        }

        private void SendCommand(SantrollerTurntableCommandId commandId, byte parameter = 0)
            => SendCommand((byte)commandId, parameter);
    }
}