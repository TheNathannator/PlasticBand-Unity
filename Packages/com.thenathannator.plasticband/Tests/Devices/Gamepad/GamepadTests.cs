using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    internal abstract class GamepadTests<TGamepad, TState> : FaceButtonDeviceTestFixture<TGamepad, TState>
        where TGamepad : Gamepad
        where TState : unmanaged, IGamepadState
    {
        protected delegate void SetHorizontalSideAction(ref TState state, HorizontalSide buttons);

        protected virtual bool hasShareButton => false;
        protected virtual bool hasTriggerPress => false;

        protected sealed override ButtonControl GetFaceButton(TGamepad gamepad, FaceButton button)
        {
            switch (button)
            {
                case FaceButton.South: return gamepad.buttonSouth;
                case FaceButton.East: return gamepad.buttonEast;
                case FaceButton.West: return gamepad.buttonWest;
                case FaceButton.North: return gamepad.buttonNorth;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected sealed override ButtonControl GetMenuButton(TGamepad gamepad, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return gamepad.startButton;
                case MenuButton.Select: return gamepad.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected sealed override DpadControl GetDpad(TGamepad gamepad) => gamepad.dpad;

        protected sealed override void SetDpad(ref TState state, DpadDirection dpad)
        {
            state.dpadUp = dpad.IsUp();
            state.dpadDown = dpad.IsDown();
            state.dpadLeft = dpad.IsLeft();
            state.dpadRight = dpad.IsRight();
        }

        protected sealed override void SetFaceButtons(ref TState state, FaceButton buttons)
        {
            state.south = (buttons & FaceButton.South) != 0;
            state.east = (buttons & FaceButton.East) != 0;
            state.west = (buttons & FaceButton.West) != 0;
            state.north = (buttons & FaceButton.North) != 0;
        }

        protected sealed override void SetMenuButtons(ref TState state, MenuButton buttons)
        {
            state.start = (buttons & MenuButton.Start) != 0;
            state.select = (buttons & MenuButton.Select) != 0;
        }

        private void SetShareButton(ref TState state, bool pressed)
        {
            state.share = pressed;
        }

        private void SetShoulderButtons(ref TState state, HorizontalSide buttons)
        {
            state.leftShoulder = (buttons & HorizontalSide.Left) != 0;
            state.rightShoulder = (buttons & HorizontalSide.Right) != 0;
        }

        private void SetTriggerButtons(ref TState state, HorizontalSide buttons)
        {
            state.leftTriggerPress = (buttons & HorizontalSide.Left) != 0;
            state.rightTriggerPress = (buttons & HorizontalSide.Right) != 0;
        }

        private void SetStickButtons(ref TState state, HorizontalSide buttons)
        {
            state.leftStickPress = (buttons & HorizontalSide.Left) != 0;
            state.rightStickPress = (buttons & HorizontalSide.Right) != 0;
        }

        private void SetLeftTrigger(ref TState state, float value)
        {
            state.leftTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        private void SetRightTrigger(ref TState state, float value)
        {
            state.rightTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        private void SetLeftStick(ref TState state, float x, float y)
        {
            state.leftStickX = DeviceHandling.DenormalizeSByte(x);
            state.leftStickY = DeviceHandling.DenormalizeSByte(y);
        }

        private void SetRightStick(ref TState state, float x, float y)
        {
            state.rightStickX = DeviceHandling.DenormalizeSByte(x);
            state.rightStickY = DeviceHandling.DenormalizeSByte(y);
        }

        [Test]
        public void RecognizesShareButton() => CreateAndRun((gamepad) =>
        {
            var state = CreateState();

            var shareButton = gamepad.TryGetChildControl<ButtonControl>("shareButton");
            if (shareButton == null) shareButton = gamepad.TryGetChildControl<ButtonControl>("share");
            if (shareButton == null) shareButton = gamepad.TryGetChildControl<ButtonControl>("captureButton");
            if (shareButton == null) shareButton = gamepad.TryGetChildControl<ButtonControl>("capture");
            if (shareButton == null || !hasShareButton)
            {
                Assert.Pass("Device has no share button to test.");
                return;
            }

            AssertButtonPress(gamepad, state);

            SetShareButton(ref state, true);
            AssertButtonPress(gamepad, state, shareButton);

            SetShareButton(ref state, false);
            AssertButtonPress(gamepad, state);
        });

        [Test]
        public void RecognizesShoulderButtons() => CreateAndRun((gamepad) =>
        {
            var state = CreateState();

            var buttonList = new List<ButtonControl>(2);
            for (var buttons = HorizontalSide.None; buttons <= HorizontalSide.All; buttons++)
            {
                SetShoulderButtons(ref state, buttons);

                if ((buttons & HorizontalSide.Left) != 0) buttonList.Add(gamepad.leftShoulder);
                if ((buttons & HorizontalSide.Right) != 0) buttonList.Add(gamepad.rightShoulder);

                AssertButtonPress(gamepad, state, buttonList.ToArray());
                buttonList.Clear();
            }
        });

        [Test]
        public void RecognizesTriggerButtons() => CreateAndRun((gamepad) =>
        {
            var state = CreateState();

            var leftTriggerButton = gamepad.TryGetChildControl<ButtonControl>("leftTriggerButton");
            var rightTriggerButton = gamepad.TryGetChildControl<ButtonControl>("rightTriggerButton");
            if (leftTriggerButton == null) leftTriggerButton = gamepad.TryGetChildControl<ButtonControl>("leftTriggerPress");
            if (rightTriggerButton == null) rightTriggerButton = gamepad.TryGetChildControl<ButtonControl>("rightTriggerPress");
            if (leftTriggerButton == null || rightTriggerButton == null || !hasTriggerPress)
            {
                Assert.Pass("Device has no trigger buttons to test.");
                return;
            }

            var buttonList = new List<ButtonControl>(2);
            for (var buttons = HorizontalSide.None; buttons <= HorizontalSide.All; buttons++)
            {
                SetTriggerButtons(ref state, buttons);

                if ((buttons & HorizontalSide.Left) != 0) buttonList.Add(leftTriggerButton);
                if ((buttons & HorizontalSide.Right) != 0) buttonList.Add(rightTriggerButton);

                AssertButtonPress(gamepad, state, buttonList.ToArray());
                buttonList.Clear();
            }
        });

        [Test]
        public void RecognizesStickButtons() => CreateAndRun((gamepad) =>
        {
            var state = CreateState();

            var buttonList = new List<ButtonControl>(2);
            for (var buttons = HorizontalSide.None; buttons <= HorizontalSide.All; buttons++)
            {
                SetStickButtons(ref state, buttons);

                if ((buttons & HorizontalSide.Left) != 0) buttonList.Add(gamepad.leftStickButton);
                if ((buttons & HorizontalSide.Right) != 0) buttonList.Add(gamepad.rightStickButton);

                AssertButtonPress(gamepad, state, buttonList.ToArray());
                buttonList.Clear();
            }
        });

        [Test]
        public void RecognizesTriggers() => CreateAndRun((gamepad) =>
        {
            RecognizesUnsignedAxis(gamepad, CreateState(), gamepad.leftTrigger, SetLeftTrigger);
            RecognizesUnsignedAxis(gamepad, CreateState(), gamepad.rightTrigger, SetRightTrigger);
        });

        [Test]
        public void RecognizesSticks() => CreateAndRun((gamepad) =>
        {
            RecognizesStick(gamepad, CreateState(), gamepad.leftStick, SetLeftStick);
            RecognizesStick(gamepad, CreateState(), gamepad.rightStick, SetRightStick);
        });
    }
}
