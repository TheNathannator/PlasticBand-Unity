using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public class StageKitTests : PlasticBandTestFixture<StageKit>
    {
        // Nothing specific to stage kits that needs to be tested currently,
        // only the base tests are needed
    }

    public abstract class StageKitTests<TStageKit, TState> : GamepadDeviceTestFixture<TStageKit, TState>
        where TStageKit : StageKit
        where TState : unmanaged, IInputStateTypeInfo
    {
        // Nothing specific to stage kits that needs to be tested currently,
        // only the base tests are needed
    }
}