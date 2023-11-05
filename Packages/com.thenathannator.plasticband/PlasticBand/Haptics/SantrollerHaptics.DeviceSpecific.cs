using PlasticBand.Devices;
using UnityEngine;
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