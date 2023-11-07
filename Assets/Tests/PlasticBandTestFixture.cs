using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests
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
    public enum FaceButton
    {
        None = 0,

        South = 0x01,
        East = 0x02,
        West = 0x04,
        North = 0x08,

        Start = 0x10,
        Select = 0x20,

        All = South | East | West | North | Start | Select
    }

    public enum AxisMode
    {
        Signed,
        Unsigned,
        Button
    }

    // Interface to simplify the amount of parameters that need to be passed around everywhere
    public interface IDeviceHandler<TState>
        where TState : unmanaged, IInputStateTypeInfo
    {
        // Opaque to simplify the generics required to use the interface
        InputDevice device { get; }

        DpadControl dpad { get; }

        // Note: may be null if not present on the device
        // D-pad and start/select are present on all PlasticBand devices,
        // but these face buttons are not
        ButtonControl southButton { get; }
        ButtonControl eastButton { get; }
        ButtonControl westButton { get; }
        ButtonControl northButton { get; }
        // end note

        ButtonControl startButton { get; }
        ButtonControl selectButton { get; }

        TState CreateState();

        void SetDpad(ref TState state, DpadDirection dpad);
        void SetFaceButtons(ref TState state, FaceButton buttons);
    }

    public class PlasticBandTestFixture : InputTestFixture
    {
        public delegate void SetButtonAction<TState>(ref TState state, bool pressed)
            where TState : unmanaged, IInputStateTypeInfo;

        public delegate void SetAxisAction<TState>(ref TState state, float value)
            where TState : unmanaged, IInputStateTypeInfo;

        public override void Setup()
        {
            base.Setup();

            // The input test fixture resets the input system, so we must re-initialize everything here
            Initialization.Initialize();
        }

        public static void AssertDeviceCreation<TDevice>(Action<TDevice> validateAction = null)
            where TDevice : InputDevice
        {
            TDevice device = null;
            try
            {
                Assert.DoesNotThrow(() => device = InputSystem.AddDevice<TDevice>());
                Assert.That(InputSystem.devices, Has.Exactly(1).TypeOf<TDevice>());
                Assert.That(InputSystem.devices, Contains.Item(device));
                AssertControlPropertiesSet(device);
                validateAction?.Invoke(device);
            }
            finally
            {
                if (device != null)
                    InputSystem.RemoveDevice(device);
            }
        }

        public static void CreateAndRun<TDevice>(Action<TDevice> action)
            where TDevice : InputDevice
        {
            TDevice device = InputSystem.AddDevice<TDevice>();
            try
            {
                action(device);
            }
            finally
            {
                InputSystem.RemoveDevice(device);
            }
        }

        public static void AssertControlPropertiesSet(InputDevice device)
        {
            foreach (var property in device.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                // Search for InputControl-returning properties
                if (!property.PropertyType.IsSubclassOf(typeof(InputControl)))
                    continue;

                // Ignore the `parent` property
                string name = property.Name;
                if (name == nameof(InputDevice.parent))
                    continue;

                // Ensure the returned control is not null
                var control = property.GetMethod.Invoke(device, null);
                Assert.That(control, Is.Not.Null, $"Control {name} on device {device} is not set!");
            }
        }

        public static void AssertAxisValue<TState>(InputDevice device, TState state,
            float value, float epsilon, params AxisControl[] axes)
            where TState : unmanaged, IInputStateTypeInfo
        {
            InputSystem.QueueStateEvent(device, state);
            InputSystem.Update();
            foreach (var axis in axes)
            {
                float axisValue = axis.value;
                Assert.That(axisValue, Is.InRange(value - epsilon, value + epsilon), $"Value for axis '{axis}' is not in range!");
            }
        }

        public static void AssertIntegerValue<TState>(InputDevice device, TState state,
            int value, params IntegerControl[] integers)
            where TState : unmanaged, IInputStateTypeInfo
        {
            InputSystem.QueueStateEvent(device, state);
            InputSystem.Update();
            foreach (var integer in integers)
            {
                float integerValue = integer.value;
                Assert.That(integerValue, Is.EqualTo(value), $"Value for integer '{integer}' is not correct!");
            }
        }

        public static void RecognizesCommonControls<TState>(IDeviceHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            RecognizesFaceButtons(handler);
            RecognizesDpad(handler);
        }

        public static void RecognizesFaceButtons<TState>(IDeviceHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var device = handler.device;
            var state = handler.CreateState();

            var southButton = handler.southButton;
            var eastButton = handler.eastButton;
            var westButton = handler.westButton;
            var northButton = handler.northButton;

            var startButton = handler.startButton;
            var selectButton = handler.selectButton;

            var buttonList = new List<ButtonControl>(6);
            for (var buttons = FaceButton.None; buttons <= FaceButton.All; buttons++)
            {
                handler.SetFaceButtons(ref state, buttons);

                // Not all devices have the 4 main face buttons
                if (southButton != null)
                {
                    if ((buttons & FaceButton.South) != 0) buttonList.Add(southButton);
                    if ((buttons & FaceButton.East) != 0) buttonList.Add(eastButton);
                    if ((buttons & FaceButton.West) != 0) buttonList.Add(westButton);
                    if ((buttons & FaceButton.North) != 0) buttonList.Add(northButton);
                }

                if ((buttons & FaceButton.Start) != 0) buttonList.Add(startButton);
                if ((buttons & FaceButton.Select) != 0) buttonList.Add(selectButton);

                AssertButtonPress(device, state, buttonList.ToArray());
                buttonList.Clear();
            }
        }

        public static void RecognizesDpad<TState>(IDeviceHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var device = handler.device;
            var state = handler.CreateState();

            var dpad = handler.dpad;

            var directionList = new List<ButtonControl>(4);
            for (var dpadDir = DpadDirection.Min; dpadDir <= DpadDirection.Max; dpadDir++)
            {
                handler.SetDpad(ref state, dpadDir);

                if (dpadDir.IsUp()) directionList.Add(dpad.up);
                if (dpadDir.IsDown()) directionList.Add(dpad.down);
                if (dpadDir.IsLeft()) directionList.Add(dpad.left);
                if (dpadDir.IsRight()) directionList.Add(dpad.right);
    
                AssertButtonPress(device, state, directionList.ToArray());
                directionList.Clear();
            }
        }

        public static void RecognizesButton<TState>(IDeviceHandler<TState> handler, ButtonControl button,
            SetButtonAction<TState> setButton)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var device = handler.device;
            var state = handler.CreateState();

            setButton(ref state, true);
            AssertButtonPress(device, state, button);

            setButton(ref state, false);
            AssertButtonPress(device, state);
        }

        public static void RecognizesAxis<TState>(IDeviceHandler<TState> handler, AxisControl axis, AxisMode mode,
            SetAxisAction<TState> setAxis)
            where TState : unmanaged, IInputStateTypeInfo
        {
            switch (mode)
            {
                case AxisMode.Signed: RecognizesSignedAxis(handler, axis, setAxis); break;
                case AxisMode.Unsigned: RecognizesUnsignedAxis(handler, axis, setAxis); break;
                case AxisMode.Button: RecognizesButtonAxis(handler, axis, setAxis); break;
                default: throw new NotImplementedException($"Unhandled axis mode {mode}!");
            }
        }

        public static void RecognizesUnsignedAxis<TState>(IDeviceHandler<TState> handler, AxisControl axis,
            SetAxisAction<TState> setAxis)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var device = handler.device;
            var state = handler.CreateState();

            for (int i = 0; i <= 100; i++)
            {
                float value = i / 100f;
                setAxis(ref state, value);
                AssertAxisValue(device, state, value, 1 / 100f, axis);
            }
        }

        public static void RecognizesSignedAxis<TState>(IDeviceHandler<TState> handler, AxisControl axis,
            SetAxisAction<TState> setAxis)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var device = handler.device;

            var state = handler.CreateState();
            for (int i = -100; i <= 100; i++)
            {
                float value = i / 100f;
                setAxis(ref state, value);
                AssertAxisValue(device, state, value, 1 / 100f, axis);
            }
        }

        public static void RecognizesButtonAxis<TState>(IDeviceHandler<TState> handler, AxisControl button,
            SetAxisAction<TState> setButton)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var device = handler.device;
            var state = handler.CreateState();

            setButton(ref state, 1f);
            AssertAxisValue(device, state, 1f, 0.001f, button);

            setButton(ref state, 0f);
            AssertAxisValue(device, state, 0f, 0.001f, button);
        }
    }
}