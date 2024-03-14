using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    [Flags]
    public enum SideButton
    {
        None = 0,

        Left = 0x01,
        Right = 0x02,

        All = Left | Right
    }

    public class StageKitTests : PlasticBandTestFixture<StageKit>
    {
        // Nothing specific to stage kits that needs to be tested currently,
        // only the base tests are needed
    }

    public abstract class StageKitTests<TStageKit, TState> : FaceButtonDeviceTestFixture<TStageKit, TState>
        where TStageKit : StageKit
        where TState : unmanaged, IInputStateTypeInfo
    {
        public delegate void SetSideButtonAction(ref TState state, SideButton buttons);

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

        protected abstract void SetShoulderButtons(ref TState state, SideButton buttons);
        protected abstract void SetStickButtons(ref TState state, SideButton buttons);

        protected abstract void SetLeftTrigger(ref TState state, float value);
        protected abstract void SetRightTrigger(ref TState state, float value);
        protected abstract void SetLeftStick(ref TState state, float x, float y);
        protected abstract void SetRightStick(ref TState state, float x, float y);

        [Test]
        public void RecognizesShoulderButtons() => CreateAndRun((stageKit) =>
        {
            RecognizesSideButtons(stageKit, SetShoulderButtons, stageKit.leftShoulder, stageKit.rightShoulder);
        });

        [Test]
        public void RecognizesStickButtons() => CreateAndRun((stageKit) =>
        {
            RecognizesSideButtons(stageKit, SetStickButtons, stageKit.leftStickButton, stageKit.rightStickButton);
        });

        [Test]
        public void RecognizesTriggers() => CreateAndRun((stageKit) =>
        {
            RecognizesUnsignedAxis(stageKit, CreateState(), stageKit.leftTrigger, SetLeftTrigger);
            RecognizesUnsignedAxis(stageKit, CreateState(), stageKit.rightTrigger, SetRightTrigger);
        });

        [Test]
        [Ignore("Currently takes 4-5 minutes to execute for some forsaken reason")]
        public void RecognizesSticks() => CreateAndRun((stageKit) =>
        {
            RecognizesStick(stageKit, CreateState(), stageKit.leftStick, SetLeftStick);
            RecognizesStick(stageKit, CreateState(), stageKit.rightStick, SetRightStick);
        });

        protected void RecognizesSideButtons(TStageKit stageKit, SetSideButtonAction setButtons,
            ButtonControl left, ButtonControl right)
        {
            var state = CreateState();
            var buttonList = new List<ButtonControl>(2);
            for (var buttons = SideButton.None; buttons <= SideButton.All; buttons++)
            {
                setButtons(ref state, buttons);

                if ((buttons & SideButton.Left) != 0) buttonList.Add(left);
                if ((buttons & SideButton.Right) != 0) buttonList.Add(right);

                AssertButtonPress(stageKit, state, buttonList.ToArray());
                buttonList.Clear();
            }
        }
    }
}