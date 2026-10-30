using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class GuitarPraiseGuitarTests_NoReportId
        : FiveFretGuitarTests<GuitarPraiseGuitar_NoReportId, GuitarPraiseGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Button;

        protected override GuitarPraiseGuitarState_NoReportId CreateState()
            => new GuitarPraiseGuitarState_NoReportId()
        {
            buttons = (ushort)GuitarPraiseButton.None,
        };

        protected override void SetDpad(ref GuitarPraiseGuitarState_NoReportId state, DpadDirection dpad)
        {
            var buttons = (GuitarPraiseButton)state.buttons;
            buttons.SetDpad(dpad.ToHidDpad());
            state.buttons = (ushort)buttons;
        }

        protected override void SetMenuButtons(ref GuitarPraiseGuitarState_NoReportId state, MenuButton menuButtons)
        {
            state.buttons.SetBit((ushort)GuitarPraiseButton.Start, (menuButtons & MenuButton.Start) != 0);
            state.buttons.SetBit((ushort)GuitarPraiseButton.Select, (menuButtons & MenuButton.Select) != 0);
        }

        protected override void SetFrets(ref GuitarPraiseGuitarState_NoReportId state, FiveFret frets)
        {
            state.buttons.SetBit((ushort)GuitarPraiseButton.Green, (frets & FiveFret.Green) != 0);
            state.buttons.SetBit((ushort)GuitarPraiseButton.Red, (frets & FiveFret.Red) != 0);
            state.buttons.SetBit((ushort)GuitarPraiseButton.Yellow, (frets & FiveFret.Yellow) != 0);
            state.buttons.SetBit((ushort)GuitarPraiseButton.Blue, (frets & FiveFret.Blue) != 0);
            state.buttons.SetBit((ushort)GuitarPraiseButton.Orange, (frets & FiveFret.Orange) != 0);
        }

        protected override void SetTilt(ref GuitarPraiseGuitarState_NoReportId state, float value)
        {
            state.buttons.SetBit((ushort)GuitarPraiseButton.Tilt, value >= 0.5f);
        }

        protected override void SetWhammy(ref GuitarPraiseGuitarState_NoReportId state, float value)
        {
            state.whammy = (byte)(0xFF - DeviceHandling.DenormalizeByteUnsigned(value));
        }
    }

    internal class GuitarPraiseGuitarTests_ReportId
        : FiveFretGuitarTests<GuitarPraiseGuitar, GuitarPraiseGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Button;

        protected override GuitarPraiseGuitarState_ReportId CreateState()
            => new GuitarPraiseGuitarState_ReportId()
        {
            state = new GuitarPraiseGuitarState_NoReportId()
            {
                buttons = (ushort)GuitarPraiseButton.None,
            }
        };

        protected override void SetDpad(ref GuitarPraiseGuitarState_ReportId state, DpadDirection dpad)
        {
            var buttons = (GuitarPraiseButton)state.state.buttons;
            buttons.SetDpad(dpad.ToHidDpad());
            state.state.buttons = (ushort)buttons;
        }

        protected override void SetMenuButtons(ref GuitarPraiseGuitarState_ReportId state, MenuButton menuButtons)
        {
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Start, (menuButtons & MenuButton.Start) != 0);
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Select, (menuButtons & MenuButton.Select) != 0);
        }

        protected override void SetFrets(ref GuitarPraiseGuitarState_ReportId state, FiveFret frets)
        {
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Green, (frets & FiveFret.Green) != 0);
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Red, (frets & FiveFret.Red) != 0);
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Yellow, (frets & FiveFret.Yellow) != 0);
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Blue, (frets & FiveFret.Blue) != 0);
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Orange, (frets & FiveFret.Orange) != 0);
        }

        protected override void SetTilt(ref GuitarPraiseGuitarState_ReportId state, float value)
        {
            state.state.buttons.SetBit((ushort)GuitarPraiseButton.Tilt, value >= 0.5f);
        }

        protected override void SetWhammy(ref GuitarPraiseGuitarState_ReportId state, float value)
        {
            state.state.whammy = (byte)(0xFF - DeviceHandling.DenormalizeByteUnsigned(value));
        }
    }
}