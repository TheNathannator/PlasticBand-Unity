using PlasticBand.Devices;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    internal abstract class SantrollerFiveFretGuitarHaptics : SantrollerHaptics, ISantrollerFiveFretGuitarHaptics
    {
        public class XInput : SantrollerFiveFretGuitarHaptics
        {
            public XInput(InputDevice device) : base(device)
            { }

            protected override void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_XInput(device, commandId, parameter);
        }

        public class Hid : SantrollerFiveFretGuitarHaptics
        {
            public Hid(InputDevice device) : base(device)
            { }

            protected override unsafe void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_Hid(device, commandId, parameter);
        }

        private enum SantrollerFiveFretCommandId : byte
        {
            OpenNote = 0x90,
            GreenNote = 0x91,
            RedNote = 0x92,
            YellowNote = 0x93,
            BlueNote = 0x94,
            OrangeNote = 0x95,
        }

        private FiveFret m_ActiveFrets;
        private bool m_OpenNoteActive;

        public SantrollerFiveFretGuitarHaptics(InputDevice device) : base(device)
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
            m_ActiveFrets = FiveFret.None;
            m_OpenNoteActive = false;
        }

        public void SetNoteLights(FiveFret frets, bool enabled)
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

        private void SendNoteLights(FiveFret frets, bool enabled)
        {
            if ((frets & FiveFret.Green) != 0 == enabled)
                SendCommand(SantrollerFiveFretCommandId.GreenNote, (byte)(enabled ? 1 : 0));
            if ((frets & FiveFret.Red) != 0 == enabled)
                SendCommand(SantrollerFiveFretCommandId.RedNote, (byte)(enabled ? 1 : 0));
            if ((frets & FiveFret.Yellow) != 0 == enabled)
                SendCommand(SantrollerFiveFretCommandId.YellowNote, (byte)(enabled ? 1 : 0));
            if ((frets & FiveFret.Blue) != 0 == enabled)
                SendCommand(SantrollerFiveFretCommandId.BlueNote, (byte)(enabled ? 1 : 0));
            if ((frets & FiveFret.Orange) != 0 == enabled)
                SendCommand(SantrollerFiveFretCommandId.OrangeNote, (byte)(enabled ? 1 : 0));
        }

        private void SendOpenNoteLight(bool enabled)
        {
            SendCommand(SantrollerFiveFretCommandId.OpenNote, (byte)(enabled ? 1 : 0));
        }

        private void SendCommand(SantrollerFiveFretCommandId commandId, byte parameter = 0)
            => SendCommand((byte)commandId, parameter);
    }
}