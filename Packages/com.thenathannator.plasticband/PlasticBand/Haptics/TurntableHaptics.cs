using System;
using System.Diagnostics;
using PlasticBand.Devices.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Interface for <see cref="Turntable"/> haptics.
    /// </summary>
    public interface ITurntableHaptics : IHaptics
    {
        /// <summary>
        /// Enables or disables blinking of the Euphoria light on the turntable.
        /// </summary>
        void SetEuphoriaBlink(bool enable);
    }

    /// <summary>
    /// Handles haptics for DJ Hero turntables.
    /// </summary>
    internal abstract class TurntableHaptics : ITurntableHaptics
    {
        private const long kEuphoriaPeriod = 3000;
        private const long kEuphoriaPeriodHalf = kEuphoriaPeriod / 2;

        private const float kEuphoriaForceDisable = -1;

        private readonly InputDevice m_Device;

        private bool m_HapticsEnabled = true;

        private readonly Stopwatch m_EuphoriaTimer = new Stopwatch();
        private bool m_EuphoriaEnabled;

        public TurntableHaptics(InputDevice device)
        {
            m_Device = device ?? throw new ArgumentNullException(nameof(device));
        }

        public void OnUpdate()
        {
            if (!m_EuphoriaTimer.IsRunning)
                return;

            long elapsed = m_EuphoriaTimer.ElapsedMilliseconds;
            if (elapsed >= kEuphoriaPeriod)
            {
                elapsed = 0;
                m_EuphoriaTimer.Restart();
            }
            else if (elapsed >= kEuphoriaPeriodHalf)
            {
                elapsed = kEuphoriaPeriodHalf - elapsed;
            }

            float brightness = Mathf.Lerp(0f, 1f, (float)elapsed / kEuphoriaPeriodHalf);
            SendEuphoria(m_Device, brightness);
        }

        public void PauseHaptics()
        {
            if (!m_HapticsEnabled)
                return;

            m_HapticsEnabled = false;
            m_EuphoriaTimer.Stop();
            SendEuphoria(m_Device, kEuphoriaForceDisable);
        }

        public void ResumeHaptics()
        {
            if (m_HapticsEnabled)
                return;

            m_HapticsEnabled = true;
            if (m_EuphoriaEnabled)
                m_EuphoriaTimer.Start();
        }

        public void ResetHaptics()
        {
            m_HapticsEnabled = true;
            m_EuphoriaEnabled = false;
            m_EuphoriaTimer.Stop();
            SendEuphoria(m_Device, kEuphoriaForceDisable);
        }

        public void SetEuphoriaBlink(bool enable)
        {
            m_EuphoriaEnabled = enable;
            if (m_HapticsEnabled)
            {
                if (enable)
                    if (!m_EuphoriaTimer.IsRunning)
                        m_EuphoriaTimer.Start();
                else
                    m_EuphoriaTimer.Stop();
            }
        }

        protected abstract void SendEuphoria(InputDevice device, float brightness);
    }

    internal class XInputTurntableHaptics : TurntableHaptics
    {
        public XInputTurntableHaptics(InputDevice device) : base(device)
        { }

        protected override void SendEuphoria(InputDevice device, float brightness)
        {
            // Handle force-disable value
            if (brightness < 0)
                brightness = 0;

            var command = new XInputVibrationCommand(0, brightness);
            device.LoggedExecuteCommand(ref command);
        }
    }

    internal class PS3TurntableHaptics : TurntableHaptics
    {
        public PS3TurntableHaptics(InputDevice device) : base(device)
        { }

        private float m_PreviousBrightness;
        private bool m_PreviousState;

        protected override unsafe void SendEuphoria(InputDevice device, float brightness)
        {
            // Enable at the start of an increase, disable at the start of a decrease
            bool enable = brightness > m_PreviousBrightness;
            var command = new PS3OutputCommand(0x91);
            command.data[0] = 1;
            command.data[1] = (byte)(enable ? 1 : 0);

            // Force-disable
            if (brightness < 0f)
                m_PreviousState = !enable;

            m_PreviousBrightness = brightness;
            if (enable != m_PreviousState)
            {
                m_PreviousState = enable;
                device.LoggedExecuteCommand(ref command);
            }
        }
    }
}