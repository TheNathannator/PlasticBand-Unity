using PlasticBand.Devices;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    internal abstract class SantrollerSixFretGuitarHaptics : SantrollerHaptics, ISantrollerSixFretGuitarHaptics
    {
        public class XInput : SantrollerSixFretGuitarHaptics
        {
            public XInput(InputDevice device) : base(device)
            { }

            protected override void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_XInput(device, commandId, parameter);
        }

        public class Hid : SantrollerSixFretGuitarHaptics
        {
            public Hid(InputDevice device) : base(device)
            { }

            protected override unsafe void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_Hid(device, commandId, parameter);
        }

        private enum SantrollerSixFretCommandId : byte
        {
            OpenNote = 0x90,
            Black1 = 0x91,
            Black2 = 0x92,
            Black3 = 0x93,
            White1 = 0x94,
            White2 = 0x95,
            White3 = 0x96,
        }

        private SixFret m_ActiveFrets;
        private bool m_OpenNoteActive;

        public SantrollerSixFretGuitarHaptics(InputDevice device) : base(device)
        { }

        protected override void OnHapticsResumed()
        {
            base.OnHapticsResumed();
            SendNoteLights(m_ActiveFrets, true);
            SendOpenNoteLight(m_OpenNoteActive);
        }

        protected override void OnHapticsReset()
        {
            base.OnHapticsReset();
            m_ActiveFrets = SixFret.None;
            m_OpenNoteActive = false;
        }

        public void SetNoteLights(SixFret frets, bool enabled)
        {
            if (enabled)
                m_ActiveFrets |= frets;
            else
                m_ActiveFrets &= ~frets;

            if (m_HapticsEnabled)
                SendNoteLights(frets, enabled);
        }

        public void SetOpenNoteLight(bool enabled)
        {
            m_OpenNoteActive = enabled;
            if (m_HapticsEnabled)
                SendOpenNoteLight(enabled);
        }

        private void SendNoteLights(SixFret frets, bool enabled)
        {
            if ((frets & SixFret.Black1) != 0 == enabled)
                SendCommand(SantrollerSixFretCommandId.Black1, (byte)(enabled ? 1 : 0));
            if ((frets & SixFret.Black2) != 0 == enabled)
                SendCommand(SantrollerSixFretCommandId.Black2, (byte)(enabled ? 1 : 0));
            if ((frets & SixFret.Black3) != 0 == enabled)
                SendCommand(SantrollerSixFretCommandId.Black3, (byte)(enabled ? 1 : 0));
            if ((frets & SixFret.White1) != 0 == enabled)
                SendCommand(SantrollerSixFretCommandId.White1, (byte)(enabled ? 1 : 0));
            if ((frets & SixFret.White2) != 0 == enabled)
                SendCommand(SantrollerSixFretCommandId.White2, (byte)(enabled ? 1 : 0));
            if ((frets & SixFret.White3) != 0 == enabled)
                SendCommand(SantrollerSixFretCommandId.White3, (byte)(enabled ? 1 : 0));
        }

        private void SendOpenNoteLight(bool enabled)
        {
            SendCommand(SantrollerSixFretCommandId.OpenNote, (byte)(enabled ? 1 : 0));
        }

        private void SendCommand(SantrollerSixFretCommandId commandId, byte parameter = 0)
            => SendCommand((byte)commandId, parameter);
    }
}