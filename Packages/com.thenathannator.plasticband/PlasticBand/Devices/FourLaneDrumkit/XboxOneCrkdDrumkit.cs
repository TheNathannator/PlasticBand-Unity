using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XboxOneFourLaneDrumkitLayout), displayName = "Xbox One CRKD Drumkit")]
    internal class XboxOneCrkdDrumkit : XboxOneFourLaneDrumkit
    {
        internal new static void Initialize()
        {
            // Stage Tour Drumkit for Xbox
            // - Also used for PC1 mode on all models
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdDrumkit>(0x3958, 0xD300);
        }
    }
}
