using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XboxOneFourLaneDrumkitLayout), displayName = "Xbox CRKD Drumkit")]
    internal class XboxCrkdDrumkit : XboxOneFourLaneDrumkit
    {
        internal new static void Initialize()
        {
            // Stage Tour Drumkit for Xbox
            // - Also used for PC1 mode on all models
            GameInputLayoutFinder.RegisterLayout<XboxCrkdDrumkit>(0x3958, 0xD300);
        }
    }
}
