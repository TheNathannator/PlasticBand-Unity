using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Bitmask of possible notes that can be hit on a turntable.
    /// </summary>
    [Flags]
    public enum TurntableHitNote : byte
    {
        None = 0,

        LeftOpen = 0x01,
        LeftGreen = 0x02,
        LeftRed = 0x04,
        LeftBlue = 0x08,

        RightOpen = 0x10,
        RightGreen = 0x20,
        RightRed = 0x40,
        RightBlue = 0x80,

        All = LeftOpen | LeftGreen | LeftRed | LeftBlue | RightOpen | RightGreen | RightRed | RightBlue,
    }

    /// <summary>
    /// Interface for Santroller turntable haptics.
    /// </summary>
    public interface ISantrollerTurntableHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Enables the specified note hit LEDs on both turntables.
        /// </summary>
        /// <param name="left">
        /// Bitmask of note hit LEDs to enable on the left turntable. Anything not specified will be disabled.<br/>
        /// Use <see cref="TurntableHitNote.None"/> to disable all note hit LEDs.
        /// </param>
        /// <param name="right">
        /// Bitmask of note hit LEDs to enable on the right turntable. Anything not specified will be disabled.<br/>
        /// Use <see cref="TurntableHitNote.None"/> to disable all note hit LEDs.
        /// </param>
        void SetHitNotes(TurntableHitNote notes);

        /// <summary>
        /// Sets a specific brightness for the Euphoria button light.
        /// </summary>
        void SetEuphoriaBrightness(float brightness);
    }

    internal struct SantrollerTurntableHaptics : ISantrollerTurntableHaptics
    {
        private enum CommandId : byte
        {
            NoteHit = 0x90,

            EuphoriaBrightness = 0xA0,
        }

        private SantrollerHaptics m_Base;

        private TurntableHitNote m_ActiveNotes;
        private byte m_EuphoriaBrightness;

        public static SantrollerTurntableHaptics Create(InputDevice device, StageKitProtocol protocol)
            => new SantrollerTurntableHaptics()
        {
            m_Base = SantrollerHaptics.Create(device, protocol),
            m_ActiveNotes = TurntableHitNote.None,
            m_EuphoriaBrightness = 0,
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

            SendCommand(CommandId.NoteHit, (byte)m_ActiveNotes);
            SendCommand(CommandId.EuphoriaBrightness, m_EuphoriaBrightness);
        }

        public void ResetHaptics()
        {
            m_Base.ResetHaptics();

            m_ActiveNotes = TurntableHitNote.None;
            m_EuphoriaBrightness = 0;
        }

        // Stage kit commands
        public void SetFogMachine(bool enabled) => m_Base.SetFogMachine(enabled);
        public void SetStrobeSpeed(StageKitStrobeSpeed speed) => m_Base.SetStrobeSpeed(speed);
        public void SetLeds(StageKitLedColor color, StageKitLed leds) => m_Base.SetLeds(color, leds);
        public void SetRedLeds(StageKitLed leds) => m_Base.SetRedLeds(leds);
        public void SetYellowLeds(StageKitLed leds) => m_Base.SetYellowLeds(leds);
        public void SetBlueLeds(StageKitLed leds) => m_Base.SetBlueLeds(leds);
        public void SetGreenLeds(StageKitLed leds) => m_Base.SetGreenLeds(leds);

        // Santroller common commands
        public void SetStarPowerFill(float fill) => m_Base.SetStarPowerFill(fill);
        public void SetStarPowerActive(bool enabled) => m_Base.SetStarPowerActive(enabled);
        public void SetMultiplier(byte multiplier) => m_Base.SetMultiplier(multiplier);
        public void SetSoloActive(bool enabled) => m_Base.SetSoloActive(enabled);
        public void SetNoteMiss(bool enabled) => m_Base.SetNoteMiss(enabled);

        public void SetHitNotes(TurntableHitNote notes)
        {
            notes &= TurntableHitNote.All;
            if (notes != m_ActiveNotes)
            {
                m_ActiveNotes = notes;
                SendCommand(CommandId.NoteHit, (byte)notes);
            }
        }

        public void SetEuphoriaBrightness(float brightness)
        {
            byte value = (byte)(Mathf.Clamp(brightness, 0f, 1f) * byte.MaxValue);
            if (value != m_EuphoriaBrightness)
            {
                m_EuphoriaBrightness = value;
                SendCommand(CommandId.EuphoriaBrightness, value);
            }
        }

        private void SendCommand(CommandId commandId, byte parameter)
        {
            m_Base.SendCommand((byte)commandId, parameter);
        }
    }
}