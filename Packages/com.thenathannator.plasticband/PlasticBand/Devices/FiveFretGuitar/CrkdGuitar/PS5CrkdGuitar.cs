using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "PlayStation 5 CRKD Guitar", hideInUI = true)]
    internal class PS5CrkdGuitar_NoReportId : PS5RiffmasterGuitar_NoReportId { }

    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "PlayStation 5 CRKD Guitar")]
    internal class PS5CrkdGuitar : PS5RiffmasterGuitar
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS5CrkdGuitar, PS5CrkdGuitar_NoReportId>(0x3651, 0x1600, reportIdDefault: true);
            HidLayoutFinder.RegisterLayout<PS5CrkdGuitar, PS5CrkdGuitar_NoReportId>(0x3651, 0x5600, reportIdDefault: true);
            HidLayoutFinder.RegisterLayout<PS5CrkdGuitar, PS5CrkdGuitar_NoReportId>(0x3958, 0x5600, reportIdDefault: true);
        }
    }
}
