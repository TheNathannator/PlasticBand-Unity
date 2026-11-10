using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 4 CRKD Drumkit", hideInUI = true)]
    internal class PS4CrkdDrumkit_NoReportId : PS4FourLaneDrumkit_NoReportId { }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 4 CRKD Drumkit")]
    internal class PS4CrkdDrumkit : PS4FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            // Stage Tour Drumkit for PlayStation (PS4 mode)
            HidLayoutFinder.RegisterLayout<PS4CrkdDrumkit, PS4CrkdDrumkit_NoReportId>(0x3958, 0xD500, reportIdDefault: true);
        }
    }
}
