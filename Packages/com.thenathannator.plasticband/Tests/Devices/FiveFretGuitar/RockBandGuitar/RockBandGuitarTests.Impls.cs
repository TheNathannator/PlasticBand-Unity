using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    internal class XInputRockBandGuitarTests
        : RockBandGuitarTests_Flags<XInputRockBandGuitar, XInputRockBandGuitarState>
    {
        protected override XInputRockBandGuitarState CreateState()
            => new XInputRockBandGuitarState()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
        };
    }

    internal class XboxOneRockBandGuitarTests
        : RockBandGuitarTests_Distinct<XboxOneRockBandGuitar, XboxOneRockBandGuitarState>
    {
        protected override XboxOneRockBandGuitarState CreateState()
            => new XboxOneRockBandGuitarState()
        {
            reportId = 0x20,
        };
    }

    internal class SantrollerXInputRockBandGuitarTests
        : RockBandGuitarTests_Flags<SantrollerXInputRockBandGuitar, XInputRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override XInputRockBandGuitarState CreateState()
            => new XInputRockBandGuitarState()
        {
            // The default value for this property is not 0, so we must set it explicitly
            whammy = 0,
        };
    }

    internal class PS3RockBandGuitarTests_NoReportId
        : RockBandGuitarTests_Flags<PS3RockBandGuitar, PS3WiiRockBandGuitarState_NoReportId>
    {
        protected override PS3WiiRockBandGuitarState_NoReportId CreateState()
            => new PS3WiiRockBandGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
        };
    }

    internal class PS3RockBandGuitarTests_ReportId
        : RockBandGuitarTests_Flags<PS3RockBandGuitar_ReportId, PS3WiiRockBandGuitarState_ReportId>
    {
        protected override PS3WiiRockBandGuitarState_ReportId CreateState()
            => new PS3WiiRockBandGuitarState_ReportId()
        {
            state = new PS3WiiRockBandGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };
    }

    internal class WiiRockBandGuitarTests_NoReportId
        : RockBandGuitarTests_Flags<WiiRockBandGuitar, PS3WiiRockBandGuitarState_NoReportId>
    {
        protected override PS3WiiRockBandGuitarState_NoReportId CreateState()
            => new PS3WiiRockBandGuitarState_NoReportId()
        {
            dpad = HidDpad.Neutral,
        };
    }

    internal class WiiRockBandGuitarTests_ReportId
        : RockBandGuitarTests_Flags<WiiRockBandGuitar_ReportId, PS3WiiRockBandGuitarState_ReportId>
    {
        protected override PS3WiiRockBandGuitarState_ReportId CreateState()
            => new PS3WiiRockBandGuitarState_ReportId()
        {
            state = new PS3WiiRockBandGuitarState_NoReportId()
            {
                dpad = HidDpad.Neutral,
            }
        };
    }

    internal class PS4RockBandGuitarTests_ReportId
        : RockBandGuitarTests_Distinct<PS4RockBandGuitar, PS4RockBandGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RockBandGuitarState_ReportId CreateState()
            => new PS4RockBandGuitarState_ReportId()
        {
            state = new PS4RockBandGuitarState_NoReportId()
            {
                buttons1 = HidDpad.Neutral.AsPS4Buttons(),
            }
        };
    }

    internal class PS4RockBandGuitarTests_NoReportId
        : RockBandGuitarTests_Distinct<PS4RockBandGuitar_NoReportId, PS4RockBandGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RockBandGuitarState_NoReportId CreateState()
            => new PS4RockBandGuitarState_NoReportId()
        {
            buttons1 = HidDpad.Neutral.AsPS4Buttons(),
        };
    }

    internal class SantrollerHIDRockBandGuitarTests
        : RockBandGuitarTests_Distinct<SantrollerHIDRockBandGuitar, SantrollerHIDRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override SantrollerHIDRockBandGuitarState CreateState()
            => new SantrollerHIDRockBandGuitarState()
        {
            reportId = 1,
            dpad = HidDpad.Neutral,

            // The default value for this property is not 0, so we must set it explicitly
            tilt = 0,
        };
    }
}