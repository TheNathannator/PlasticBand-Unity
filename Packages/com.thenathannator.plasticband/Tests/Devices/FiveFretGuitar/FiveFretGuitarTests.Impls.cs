using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    internal class GuitarPraiseGuitarTests_ReportId
        : FiveFretGuitarTests<GuitarPraiseGuitar, GuitarPraiseGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Button;

        protected override GuitarPraiseGuitarState_ReportId CreateState()
            => new GuitarPraiseGuitarState_ReportId()
        {
            state = new GuitarPraiseGuitarState_NoReportId()
            {
                buttons = GuitarPraiseButton.None,
            }
        };

        protected override void CheckExpectedDescriptions()
        {
            // Guitar Praise Guitar for PC
            AssertHidMatch(0x0314, 0x0830, reportId: true);
        }
    }

    internal class GuitarPraiseGuitarTests_NoReportId
        : FiveFretGuitarTests<GuitarPraiseGuitar_NoReportId, GuitarPraiseGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Button;

        protected override GuitarPraiseGuitarState_NoReportId CreateState()
            => new GuitarPraiseGuitarState_NoReportId()
        {
            buttons = GuitarPraiseButton.None,
        };

        protected override void CheckExpectedDescriptions()
        {
            AssertHidMatch(0x0314, 0x0830, reportId: false);
        }
    }
}