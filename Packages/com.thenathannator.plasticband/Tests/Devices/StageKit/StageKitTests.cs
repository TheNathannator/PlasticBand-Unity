using PlasticBand.Devices;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public class StageKitTests : PlasticBandTestFixture<StageKit>
    {
        // Nothing specific to stage kits that needs to be tested currently,
        // only the base tests are needed

        protected override void CheckExpectedDescriptions() { }
    }

    internal abstract class StageKitTests<TStageKit, TState> : GamepadTests<TStageKit, TState>
        where TStageKit : StageKit
        where TState : unmanaged, IGamepadState
    {
        // Nothing specific to stage kits that needs to be tested currently,
        // only the base tests are needed
    }
}