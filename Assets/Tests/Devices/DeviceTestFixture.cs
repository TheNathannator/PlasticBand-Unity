using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public enum DpadDirection : uint
    {
        Up = 0,
        UpRight = 1,
        Right = 2,
        DownRight = 3,
        Down = 4,
        DownLeft = 5,
        Left = 6,
        UpLeft = 7,
        Neutral = 8,

        Min = Up,
        Max = Neutral
    }

    [Flags]
    public enum MenuButton
    {
        None = 0,

        Start = 0x01,
        Select = 0x02,

        All = Start | Select
    }

    [Flags]
    public enum FaceButton
    {
        None = 0,

        South = 0x01,
        East = 0x02,
        West = 0x04,
        North = 0x08,

        All = South | East | West | North
    }

    public abstract class DeviceTestFixture<TDevice, TState> : PlasticBandTestFixture<TDevice>
        where TDevice : InputDevice
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected abstract TState CreateState();

        // All PlasticBand devices have start/select and a d-pad
        protected abstract ButtonControl GetMenuButton(TDevice device, MenuButton button);
        protected abstract void SetMenuButtons(ref TState state, MenuButton buttons);

        protected abstract DpadControl GetDpad(TDevice device);
        protected abstract void SetDpad(ref TState state, DpadDirection dpad);

        [Test]
        public void RecognizesMenuButtons() => CreateAndRun((device) =>
        {
            var state = CreateState();

            var startButton = GetMenuButton(device, MenuButton.Start);
            var selectButton = GetMenuButton(device, MenuButton.Select);

            var buttonList = new List<ButtonControl>(2);
            for (var buttons = MenuButton.None; buttons <= MenuButton.All; buttons++)
            {
                SetMenuButtons(ref state, buttons);

                if ((buttons & MenuButton.Start) != 0) buttonList.Add(startButton);
                if ((buttons & MenuButton.Select) != 0) buttonList.Add(selectButton);

                AssertButtonPress(device, state, buttonList.ToArray());
                buttonList.Clear();
            }
        });

        [Test]
        public void RecognizesDpad() => CreateAndRun((device) =>
        {
            var state = CreateState();
            var dpad = GetDpad(device);

            var directionList = new List<ButtonControl>(4);
            for (var dpadDir = DpadDirection.Min; dpadDir <= DpadDirection.Max; dpadDir++)
            {
                SetDpad(ref state, dpadDir);

                if (dpadDir.IsUp()) directionList.Add(dpad.up);
                if (dpadDir.IsDown()) directionList.Add(dpad.down);
                if (dpadDir.IsLeft()) directionList.Add(dpad.left);
                if (dpadDir.IsRight()) directionList.Add(dpad.right);
    
                AssertButtonPress(device, state, directionList.ToArray());
                directionList.Clear();
            }
        });
    }

    // D-pad and start/select are present on all PlasticBand devices, but the face buttons are not
    public abstract class FaceButtonDeviceTestFixture<TDevice, TState> : DeviceTestFixture<TDevice, TState>
        where TDevice : InputDevice
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected abstract ButtonControl GetFaceButton(TDevice device, FaceButton button);
        protected abstract void SetFaceButtons(ref TState state, FaceButton buttons);

        [Test]
        public void RecognizesFaceButtons() => CreateAndRun((device) =>
        {
            var state = CreateState();

            var southButton = GetFaceButton(device, FaceButton.South);
            var eastButton = GetFaceButton(device, FaceButton.East);
            var westButton = GetFaceButton(device, FaceButton.West);
            var northButton = GetFaceButton(device, FaceButton.North);

            var buttonList = new List<ButtonControl>(6);
            for (var buttons = FaceButton.None; buttons <= FaceButton.All; buttons++)
            {
                SetFaceButtons(ref state, buttons);

                if ((buttons & FaceButton.South) != 0) buttonList.Add(southButton);
                if ((buttons & FaceButton.East) != 0) buttonList.Add(eastButton);
                if ((buttons & FaceButton.West) != 0) buttonList.Add(westButton);
                if ((buttons & FaceButton.North) != 0) buttonList.Add(northButton);

                AssertButtonPress(device, state, buttonList.ToArray());
                buttonList.Clear();
            }
        });
    }
}