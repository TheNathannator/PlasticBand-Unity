using System;
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

    public abstract class StageKitTests<TStageKit, TState> : FaceButtonDeviceTestFixture<TStageKit, TState>
        where TStageKit : StageKit
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected override ButtonControl GetFaceButton(TStageKit stageKit, FaceButton button)
        {
            switch (button)
            {
                case FaceButton.South: return stageKit.buttonSouth;
                case FaceButton.East: return stageKit.buttonEast;
                case FaceButton.West: return stageKit.buttonWest;
                case FaceButton.North: return stageKit.buttonNorth;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override ButtonControl GetMenuButton(TStageKit stageKit, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return stageKit.startButton;
                case MenuButton.Select: return stageKit.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override DpadControl GetDpad(TStageKit stageKit) => stageKit.dpad;
    }
}