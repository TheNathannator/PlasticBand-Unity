using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Xbox%20360%20Rock%20Band%20Stage%20Kit.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput stagekit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputStageKitState), displayName = "Santroller device in XInput Stagekit mode")]
    internal class SantrollerXInputStageKit : XInputStageKit
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputStageKit>(XInputNonStandardSubType.StageKit);
        }
    }
}
