using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XboxOneRockBandGuitarLayout), displayName = "Xbox One CRKD Guitar")]
    internal class XboxOneCrkdGuitar : XboxOneRockBandGuitar
    {
        internal new static void Initialize()
        {
            // CRKD Gibson Les Paul Guitar for Xbox
            // - Blueberry Burst model
            // - Tribal model
            // - Cherry Sunburst model
            // - Olive Drab model
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3651, 0x4161);

            // Stage Tour Gibson Kramer Guitar for Xbox
            // - Also used for PC1 mode on all models
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3958, 0x4161);
        }
    }
}
