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

            // CRKD Gibson Les Paul Guitar for Xbox (Oxblood model)
            // CRKD Gibson Xplorer Guitar (PC1 mode)
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3651, 0x2200);

            // CRKD Gibson SG Guitar for PlayStation (PC mode, wired)
            // - Midnight Blue model
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3651, 0x8200);

            // CRKD Gibson SG Guitar for PlayStation (PC mode, wireless)
            // - Midnight Blue model
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3651, 0x8210);

            // Stage Tour Gibson Kramer Guitar for Xbox
            // - Also used for PC1 mode on all models
            GameInputLayoutFinder.RegisterLayout<XboxOneCrkdGuitar>(0x3958, 0x4161);
        }
    }
}
