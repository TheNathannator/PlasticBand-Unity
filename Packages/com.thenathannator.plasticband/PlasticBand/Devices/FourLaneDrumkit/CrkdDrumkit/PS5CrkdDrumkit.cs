using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 5 CRKD Drumkit", hideInUI = true)]
    internal class PS5CrkdDrumkit_NoReportId : PS5FourLaneDrumkit_NoReportId { }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 5 CRKD Drumkit")]
    internal class PS5CrkdDrumkit : PS5FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            // Stage Tour Drumkit for PlayStation (PS5 mode)
            HidLayoutFinder.RegisterLayout<PS5CrkdDrumkit, PS5CrkdDrumkit_NoReportId>(0x3958, 0xD600, reportIdDefault: true);
        }
    }
}
