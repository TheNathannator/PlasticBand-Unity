using PlasticBand.Devices;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    internal abstract class SantrollerFourLaneDrumkitHaptics : SantrollerHaptics, ISantrollerFourLaneDrumkitHaptics
    {
        public class XInput : SantrollerFourLaneDrumkitHaptics
        {
            public XInput(InputDevice device) : base(device)
            { }

            protected override void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_XInput(device, commandId, parameter);
        }

        public class Hid : SantrollerFourLaneDrumkitHaptics
        {
            public Hid(InputDevice device) : base(device)
            { }

            protected override unsafe void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_Hid(device, commandId, parameter);
        }

        private enum SantrollerFourLaneCommandId : byte
        {
            KickPedal = 0x90,
            RedPad = 0x91,
            YellowPad = 0x92,
            BluePad = 0x93,
            GreenPad = 0x94,
            YellowCymbal = 0x95,
            BlueCymbal = 0x96,
            GreenCymbal = 0x97,
        }

        private FourLanePad m_ActivePads;

        public SantrollerFourLaneDrumkitHaptics(InputDevice device) : base(device)
        { }

        protected override void OnHapticsResumed()
        {
            base.OnHapticsResumed();
            SendNoteLights(m_ActivePads, true);
        }

        protected override void OnHapticsReset()
        {
            base.OnHapticsReset();
            m_ActivePads = FourLanePad.None;
        }

        public void SetNoteLights(FourLanePad pads, bool enabled)
        {
            if (enabled)
                m_ActivePads |= pads;
            else
                m_ActivePads &= ~pads;

            if (m_HapticsEnabled)
                SendNoteLights(pads, enabled);
        }

        private void SendNoteLights(FourLanePad pads, bool enabled)
        {
            if ((pads & FourLanePad.Kick1) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.KickPedal, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.Kick2) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.KickPedal, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.RedPad) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.RedPad, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.YellowPad) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.YellowPad, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.BluePad) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.BluePad, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.GreenPad) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.GreenPad, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.YellowCymbal) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.YellowCymbal, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.BlueCymbal) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.BlueCymbal, (byte)(enabled ? 1 : 0));
            if ((pads & FourLanePad.GreenCymbal) != 0 == enabled)
                SendCommand(SantrollerFourLaneCommandId.GreenCymbal, (byte)(enabled ? 1 : 0));
        }

        private void SendCommand(SantrollerFourLaneCommandId commandId, byte parameter = 0)
            => SendCommand((byte)commandId, parameter);
    }
}