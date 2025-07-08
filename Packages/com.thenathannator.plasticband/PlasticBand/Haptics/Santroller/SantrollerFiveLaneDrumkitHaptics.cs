using System;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Bitmask of possible notes that can be hit on a 5-lane drumkit.
    /// </summary>
    [Flags]
    public enum FiveLaneDrumkitHitNote : byte
    {
        None = 0,

        Kick = 0x01,
        RedPad = 0x02,
        YellowCymbal = 0x04,
        BluePad = 0x08,
        OrangeCymbal = 0x10,
        GreenPad = 0x20,

        All = Kick | RedPad | YellowCymbal | BluePad | OrangeCymbal | GreenPad,
    }

    /// <summary>
    /// Interface for Santroller 5-lane drums haptics.
    /// </summary>
    public interface ISantrollerFiveLaneDrumkitHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Enables the specified note hit LEDs.
        /// </summary>
        /// <param name="notes">
        /// Bitmask of note hit LEDs to enable. Anything not specified will be disabled.<br/>
        /// Use <see cref="FiveLaneDrumkitHitNote.None"/> to disable all note hit LEDs.
        /// </param>
        void SetHitNotes(FiveLaneDrumkitHitNote notes);
    }

    internal struct SantrollerFiveLaneDrumkitHaptics : ISantrollerFiveLaneDrumkitHaptics
    {
        private enum CommandId : byte
        {
            NoteHit = 0x90,
        }

        private SantrollerHaptics m_Base;

        private FiveLaneDrumkitHitNote m_ActiveNotes;

        public static SantrollerFiveLaneDrumkitHaptics Create(InputDevice device, StageKitProtocol protocol)
            => new SantrollerFiveLaneDrumkitHaptics()
        {
            m_Base = SantrollerHaptics.Create(device, protocol),
            m_ActiveNotes = FiveLaneDrumkitHitNote.None,
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

            m_ActiveNotes = FiveLaneDrumkitHitNote.None;
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

        public void SetHitNotes(FiveLaneDrumkitHitNote notes)
        {
            notes &= FiveLaneDrumkitHitNote.All;
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