using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XboxOneRockBandGuitarLayout), displayName = "Xbox One CRKD Guitar")]
    internal class XboxOneCrkdGuitar : XboxOneRockBandGuitar
    {
        internal new static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3651, 0x4161);
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3958, 0x4161);
        }
    }
}
