using System;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Bitmask of possible notes that can be hit on a 6-fret guitar.
    /// </summary>
    [Flags]
    public enum SixFretGuitarHitNote : byte
    {
        None = 0,

        Open = 0x01,
        Black1 = 0x02,
        Black2 = 0x04,
        Black3 = 0x08,
        White1 = 0x10,
        White2 = 0x20,
        White3 = 0x40,

        All = Open | Black1 | Black2 | Black3 | White1 | White2 | White3,
    }

    /// <summary>
    /// Interface for Santroller 6-fret guitar haptics.
    /// </summary>
    public interface ISantrollerSixFretGuitarHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Enables the specified note hit LEDs.
        /// </summary>
        /// <param name="notes">
        /// Bitmask of note hit LEDs to enable. Anything not specified will be disabled.<br/>
        /// Use <see cref="SixFretGuitarHitNote.None"/> to disable all note hit LEDs.
        /// </param>
        void SetHitNotes(SixFretGuitarHitNote notes);
    }

    internal struct SantrollerSixFretGuitarHaptics : ISantrollerSixFretGuitarHaptics
    {
        private enum CommandId : byte
        {
            NoteHit = 0x90,
        }

        private SantrollerHaptics m_Base;

        private SixFretGuitarHitNote m_ActiveNotes;

        public static SantrollerSixFretGuitarHaptics Create(InputDevice device, StageKitProtocol protocol)
            => new SantrollerSixFretGuitarHaptics()
        {
            m_Base = SantrollerHaptics.Create(device, protocol),
            m_ActiveNotes = SixFretGuitarHitNote.None,
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
        }

        public void ResetHaptics()
        {
            m_Base.ResetHaptics();

            m_ActiveNotes = SixFretGuitarHitNote.None;
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

        public void SetHitNotes(SixFretGuitarHitNote notes)
        {
            notes &= SixFretGuitarHitNote.All;
            if (notes != m_ActiveNotes)
            {
                m_ActiveNotes = notes;
                SendCommand(CommandId.NoteHit, (byte)notes);
            }
        }

        private void SendCommand(CommandId commandId, byte parameter)
        {
            m_Base.SendCommand((byte)commandId, parameter);
        }
    }
}