using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Xbox%20360%20Rock%20Band%20Stage%20Kit.md

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(XInputStageKitState), displayName = "Santroller XInput Stage Kit")]
    internal class SantrollerXInputStageKit : XInputStageKit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputStageKit>(XInputNonStandardSubType.StageKit);
        }
    }
}
