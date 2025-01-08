using PlasticBand.Devices;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    internal abstract class SantrollerFiveLaneDrumkitHaptics : SantrollerHaptics, ISantrollerFiveLaneDrumkitHaptics
    {
        public class XInput : SantrollerFiveLaneDrumkitHaptics
        {
            public XInput(InputDevice device) : base(device)
            { }

            protected override void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_XInput(device, commandId, parameter);
        }

        public class Hid : SantrollerFiveLaneDrumkitHaptics
        {
            public Hid(InputDevice device) : base(device)
            { }

            protected override unsafe void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
                => SendCommand_Hid(device, commandId, parameter);
        }

        private enum SantrollerFiveLaneCommandId : byte
        {
            KickPedal = 0x90,
            RedPad = 0x91,
            YellowCymbal = 0x92,
            BluePad = 0x93,
            OrangeCymbal = 0x96,
            GreenPad = 0x95,
        }

        private FiveLanePad m_ActivePads;

        public SantrollerFiveLaneDrumkitHaptics(InputDevice device) : base(device)
        { }

        protected override void OnHapticsResumed()
        {
            base.OnHapticsResumed();
            SendNoteLights(m_ActivePads, true);
        }

        protected override void OnHapticsReset()
        {
            base.OnHapticsReset();
            m_ActivePads = FiveLanePad.None;
        }

        public void SetNoteLights(FiveLanePad pads, bool enabled)
        {
            if (enabled)
                m_ActivePads |= pads;
            else
                m_ActivePads &= ~pads;

            if (m_HapticsEnabled)
                SendNoteLights(pads, enabled);
        }

        private void SendNoteLights(FiveLanePad pads, bool enabled)
        {
            if ((pads & FiveLanePad.Kick) != 0 == enabled)
                SendCommand(SantrollerFiveLaneCommandId.KickPedal, (byte)(enabled ? 1 : 0));
            if ((pads & FiveLanePad.Red) != 0 == enabled)
                SendCommand(SantrollerFiveLaneCommandId.RedPad, (byte)(enabled ? 1 : 0));
            if ((pads & FiveLanePad.Yellow) != 0 == enabled)
                SendCommand(SantrollerFiveLaneCommandId.YellowCymbal, (byte)(enabled ? 1 : 0));
            if ((pads & FiveLanePad.Blue) != 0 == enabled)
                SendCommand(SantrollerFiveLaneCommandId.BluePad, (byte)(enabled ? 1 : 0));
            if ((pads & FiveLanePad.Orange) != 0 == enabled)
                SendCommand(SantrollerFiveLaneCommandId.OrangeCymbal, (byte)(enabled ? 1 : 0));
            if ((pads & FiveLanePad.Green) != 0 == enabled)
                SendCommand(SantrollerFiveLaneCommandId.GreenPad, (byte)(enabled ? 1 : 0));
        }

        private void SendCommand(SantrollerFiveLaneCommandId commandId, byte parameter = 0)
            => SendCommand((byte)commandId, parameter);
    }
}