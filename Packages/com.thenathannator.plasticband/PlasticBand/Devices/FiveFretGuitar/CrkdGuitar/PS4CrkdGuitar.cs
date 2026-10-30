using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "PlayStation 4 CRKD Guitar", hideInUI = true)]
    internal class PS4CrkdGuitar_NoReportId : PS4RiffmasterGuitar_NoReportId { }

    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "PlayStation 4 CRKD Guitar")]
    internal class PS4CrkdGuitar : PS4RiffmasterGuitar
    {
        internal new static void Initialize()
        {
            // CRKD Gibson SG Guitar for PlayStation (PS4 mode, wired)
            // - Cherry model
            // - Black model
            HidLayoutFinder.RegisterLayout<PS4CrkdGuitar, PS4CrkdGuitar_NoReportId>(0x3651, 0x1500, reportIdDefault: true);

            // CRKD Gibson SG Guitar for PlayStation (PS4 mode, wireless)
            // - Cherry model
            // - Black model
            HidLayoutFinder.RegisterLayout<PS4CrkdGuitar, PS4CrkdGuitar_NoReportId>(0x3651, 0x5500, reportIdDefault: true);

            // Stage Tour Gibson Kramer Guitar for PlayStation (PS4 mode)
            HidLayoutFinder.RegisterLayout<PS4CrkdGuitar, PS4CrkdGuitar_NoReportId>(0x3958, 0x5500, reportIdDefault: true);
        }
    }
}
