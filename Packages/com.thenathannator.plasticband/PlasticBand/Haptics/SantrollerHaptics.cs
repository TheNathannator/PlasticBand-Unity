using UnityEngine;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Interface for Santroller device haptics.
    /// </summary>
    public interface ISantrollerHaptics : IStageKitHaptics
    {
        // bool batchOutputs { get; set; }

        /// <summary>
        /// Sends the fill amount of the Star Power gauge (0-1).
        /// </summary>
        void SetStarPowerFill(float fill);

        /// <summary>
        /// Sends whether or not Star Power is active.
        /// </summary>
        void SetStarPowerActive(bool enabled);

        /// <summary>
        /// Sends the current multiplier number.
        /// </summary>
        void SetMultiplier(byte multiplier);

        /// <summary>
        /// Sends whether or not a solo is active.
        /// </summary>
        void SetSoloActive(bool enabled);

        /// <summary>
        /// Sends whether or not a note was missed.
        /// </summary>
        void SetNoteMiss(bool enabled);
    }

    /// <summary>
    /// Handles haptics for Santroller devices.
    /// </summary>
    internal struct SantrollerHaptics : ISantrollerHaptics
    {
        internal enum CommandId : byte
        {
            StarPowerFill = 0x08,
            StarPowerActive = 0x09,
            Multiplier = 0x0A,
            SoloActive = 0x0B,
            NoteMiss = 0x0C,
        }

        private StageKitHaptics m_Base;

        private byte m_StarPowerFill;
        private bool m_StarPowerActive;
        private byte m_Multiplier;
        private bool m_SoloActive;
        private bool m_MissActive;

        public bool hapticsEnabled => m_Base.hapticsEnabled;

        public static SantrollerHaptics Create(InputDevice device, StageKitProtocol protocol)
            => new SantrollerHaptics()
        {
            m_Base = StageKitHaptics.Create(device, protocol),
            m_StarPowerFill = 0,
            m_StarPowerActive = false,
            m_Multiplier = 1,
            m_SoloActive = false,
            m_MissActive = false,
        };

        public void PauseHaptics()
        {
            if (!m_Base.hapticsEnabled)
                return;

            m_Base.PauseHaptics();
        }

        public void ResumeHaptics()
        {
            if (m_Base.hapticsEnabled)
                return;

            m_Base.ResumeHaptics();

            SendCommand(CommandId.StarPowerFill, m_StarPowerFill);
            SendToggle(CommandId.StarPowerActive, m_StarPowerActive);
            SendCommand(CommandId.Multiplier, m_Multiplier);
            SendToggle(CommandId.SoloActive, m_SoloActive);
            SendToggle(CommandId.NoteMiss, m_MissActive);
        }

        public void ResetHaptics()
        {
            m_Base.ResetHaptics();

            m_StarPowerFill = 0;
            m_StarPowerActive = false;
            m_Multiplier = 1;
            m_SoloActive = false;
            m_MissActive = false;
        }

        public void SetFogMachine(bool enabled) => m_Base.SetFogMachine(enabled);
        public void SetStrobeSpeed(StageKitStrobeSpeed speed) => m_Base.SetStrobeSpeed(speed);
        public void SetLeds(StageKitLedColor color, StageKitLed leds) => m_Base.SetLeds(color, leds);
        public void SetRedLeds(StageKitLed leds) => m_Base.SetRedLeds(leds);
        public void SetYellowLeds(StageKitLed leds) => m_Base.SetYellowLeds(leds);
        public void SetBlueLeds(StageKitLed leds) => m_Base.SetBlueLeds(leds);
        public void SetGreenLeds(StageKitLed leds) => m_Base.SetGreenLeds(leds);

        public void SetStarPowerFill(float fill)
        {
            byte value = (byte)(Mathf.Clamp(fill, 0f, 1f) * byte.MaxValue);
            if (value != m_StarPowerFill)
            {
                m_StarPowerFill = value;
                SendCommand(CommandId.StarPowerFill, value);
            }
        }

        public void SetStarPowerActive(bool enabled)
        {
            if (enabled != m_StarPowerActive)
            {
                m_StarPowerActive = enabled;
                SendToggle(CommandId.StarPowerActive, enabled);
            }
        }

        public void SetMultiplier(byte multiplier)
        {
            if (multiplier < 1)
            {
                multiplier = 1;
            }

            if (multiplier != m_Multiplier)
            {
                m_Multiplier = multiplier;
                SendCommand(CommandId.Multiplier, multiplier);
            }
        }

        public void SetSoloActive(bool enabled)
        {
            if (enabled != m_SoloActive)
            {
                m_SoloActive = enabled;
                SendToggle(CommandId.SoloActive, enabled);
            }
        }

        public void SetNoteMiss(bool enabled)
        {
            if (enabled != m_MissActive)
            {
                m_MissActive = enabled;
                SendToggle(CommandId.NoteMiss, enabled);
            }
        }

        private void SendCommand(CommandId commandId, byte parameter)
        {
            SendCommand((byte)commandId, parameter);
        }

        private void SendToggle(CommandId commandId, bool parameter)
        {
            SendCommand((byte)commandId, (byte)(parameter ? 1 : 0));
        }

        public void SendCommand(byte commandId, byte parameter)
        {
            m_Base.SendCommand(commandId, parameter);
        }
    }
}