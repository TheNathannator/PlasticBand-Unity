using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests
{
    public enum AxisMode
    {
        Signed,
        Unsigned,
        Button
    }

    public class PlasticBandTestFixture<TDevice> : InputTestFixture
        where TDevice : InputDevice
    {
        public delegate void SetButtonAction<TState>(ref TState state, bool pressed)
            where TState : unmanaged, IInputStateTypeInfo;

        public delegate void SetAxisAction<TState>(ref TState state, float value)
            where TState : unmanaged, IInputStateTypeInfo;

        public delegate void SetStickAction<TState>(ref TState state, float x, float y)
            where TState : unmanaged, IInputStateTypeInfo;

        public delegate void AssertAxisValueAction(AxisControl axis, Func<AxisControl, float> getValue,
            float value, float epsilon);

        public override void Setup()
        {
            base.Setup();

            // The input test fixture resets the input system, so we must re-initialize everything here
            Initialization.Initialize();
        }

        [Test]
        public void CanCreate() => Assert.DoesNotThrow(() => CreateAndRun((device) => {}));

        [Test]
        public void AllControlPropertiesSet() => CreateAndRun((device) =>
        {
            foreach (var property in device.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                // Search for InputControl-returning properties
                if (!property.PropertyType.IsSubclassOf(typeof(InputControl)))
                    continue;

                // Ignore the `parent` property and any indexers
                string name = property.Name;
                if (name == nameof(InputDevice.parent) || name == "Item")
                    continue;

                // Ensure the returned control is not null
                var control = property.GetMethod.Invoke(device, null);
                Assert.That(control, Is.Not.Null, $"Control {name} on device {device} is not set!");
            }
        });

        public void CreateAndRun(Action<TDevice> action)
        {
            var device = InputSystem.AddDevice(typeof(TDevice).Name);
            try
            {
                if (!(device is TDevice tDevice))
                {
                    Assert.Fail($"Expected device of type {typeof(TDevice)}, but got {device.GetType()} instead!");
                    return;
                }

                InitializeDevice(tDevice);
                action(tDevice);
            }
            finally
            {
                InputSystem.RemoveDevice(device);
            }
        }

        protected virtual void InitializeDevice(TDevice device) { }

        public static void AssertEventUpdate<TState>(InputDevice device, TState state,
            Action<InputEventPtr> onInputEvent, Action onAfterUpdate = null)
            where TState : unmanaged, IInputStateTypeInfo
        {
            // Input event assert
            using (InputSystem.onEvent
                .ForDevice(device)
                .Where((eventPtr) => eventPtr.IsA<StateEvent>() || eventPtr.IsA<DeltaStateEvent>())
                .Assert(onInputEvent))
            {
                InputSystem.QueueStateEvent(device, state);
                InputSystem.Update();
            }

            // Post-update assert
            onAfterUpdate?.Invoke();
        }

        public static new void AssertButtonPress<TState>(InputDevice device, TState state, params ButtonControl[] buttons)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => AssertButton((button) => button.isPressed);
            void EventAssert(InputEventPtr eventPtr)
                => AssertButton((button) => button.IsPressedInEvent(eventPtr));

            AssertEventUpdate(device, state, EventAssert, UpdateAssert);

            void AssertButton(Func<ButtonControl, bool> getPressed)
            {
                foreach (var control in device.allControls)
                {
                    if (!(control is ButtonControl button))
                        continue;

                    if (buttons.Contains(button))
                        Assert.That(getPressed(button), Is.True, $"Expected button {button} to be pressed");
                    else
                        Assert.That(getPressed(button), Is.False, $"Expected button {button} to NOT be pressed");
                }
            }
        }

        public static void AssertButtonValue<TState>(InputDevice device, TState state,
            float value, float epsilon, params ButtonControl[] buttons)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => AssertButton((button) => button.isPressed, (button) => button.value);
            void EventAssert(InputEventPtr eventPtr)
                => AssertButton((button) => button.IsPressedInEvent(eventPtr), (button) => button.ReadValueFromEvent(eventPtr));

            AssertEventUpdate(device, state, EventAssert, UpdateAssert);

            void AssertButton(Func<ButtonControl, bool> getPressed, Func<ButtonControl, float> getValue)
            {
                foreach (var control in device.allControls)
                {
                    if (!(control is ButtonControl button))
                        continue;

                    if (buttons.Contains(button))
                    {
                        Assert.That(getValue(button), Is.InRange(value - epsilon, value + epsilon), $"Value for button '{button}' is not in range!");
                        if (value >= button.pressPointOrDefault)
                            Assert.That(getPressed(button), Is.True, $"Expected button {button} to be pressed");
                        else
                            Assert.That(getPressed(button), Is.False, $"Expected button {button} to NOT be pressed");
                    }
                    else
                    {
                        Assert.That(getPressed(button), Is.False, $"Expected button {button} to NOT be pressed");
                        float defaultValue = button.ReadDefaultValue();
                        Assert.That(getValue(button), Is.InRange(defaultValue - epsilon, defaultValue + epsilon), $"Expected default value for button '{button}'!");
                    }
                }
            }
        }

        public delegate void AssertMaskAction<TMask>(TMask result, TMask expected, Func<ButtonControl, bool> getPressed);

        /// <summary>
        /// Asserts the state of buttons across both input events and post-update properties.
        /// </summary>
        public static void AssertButtonMask<TState, TMask>(InputDevice device, TState state,
            TMask targetMask, Func<TMask> getMask, Func<InputEventPtr, TMask> getMaskFromEvent,
            AssertMaskAction<TMask> assertMask)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => assertMask(getMask(), targetMask, (button) => button.isPressed);
            void EventAssert(InputEventPtr eventPtr)
                => assertMask(getMaskFromEvent(eventPtr), targetMask, (button) => button.IsPressedInEvent(eventPtr));

            AssertEventUpdate(device, state, EventAssert, UpdateAssert);
        }

        public static void AssertAxisValue<TState>(InputDevice device, TState state,
            float value, float epsilon, params AxisControl[] axes)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => AssertButton((axis) => axis.value);
            void EventAssert(InputEventPtr eventPtr)
                => AssertButton((axis) => axis.ReadValueFromEvent(eventPtr));

            AssertEventUpdate(device, state, EventAssert, UpdateAssert);

            void AssertButton(Func<AxisControl, float> getValue)
            {
                foreach (var control in device.allControls)
                {
                    if (!(control is AxisControl axis))
                        continue;

                    if (axes.Contains(axis))
                    {
                        AssertAxisValue(axis, getValue, value, epsilon);
                    }
                    else
                    {
                        AssertAxisIsDefault(axis, getValue, epsilon);
                    }
                }
            }
        }

        public static void AssertAxisValues<TState>(InputDevice device, TState state,
            float epsilon, List<(AxisControl axis, float value)> axes)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => AssertButton((axis) => axis.value);
            void EventAssert(InputEventPtr eventPtr)
                => AssertButton((axis) => axis.ReadValueFromEvent(eventPtr));

            AssertEventUpdate(device, state, EventAssert, UpdateAssert);

            void AssertButton(Func<AxisControl, float> getValue)
            {
                foreach (var control in device.allControls)
                {
                    if (!(control is AxisControl axis))
                        continue;

                    var (found, value) = axes.FirstOrDefault((v) => v.axis == axis);
                    if (found != null)
                    {
                        AssertAxisValue(axis, getValue, value, epsilon);
                    }
                    else
                    {
                        AssertAxisIsDefault(axis, getValue, epsilon);
                    }
                }
            }
        }

        public static void AssertAxisValue(AxisControl axis, Func<AxisControl, float> getValue,
            float value, float epsilon)
        {
            Assert.That(getValue(axis), Is.InRange(value - epsilon, value + epsilon), $"Value for axis '{axis}' is not in range!");
        }

        public static void AssertAxisIsDefault(AxisControl axis, Func<AxisControl, float> getValue, float epsilon)
        {
            float defaultValue = axis.ReadDefaultValue();
            Assert.That(getValue(axis), Is.InRange(defaultValue - epsilon, defaultValue + epsilon), $"Expected default value for axis '{axis}'!");
        }

        public static void AssertIntegerValue<TState>(InputDevice device, TState state,
            int value, params IntegerControl[] integers)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => AssertInteger((axis) => axis.value);
            void EventAssert(InputEventPtr eventPtr)
                => AssertInteger((axis) => axis.ReadValueFromEvent(eventPtr));

            AssertEventUpdate(device, state, EventAssert, UpdateAssert);

            void AssertInteger(Func<IntegerControl, int> getValue)
            {
                foreach (var control in device.allControls)
                {
                    if (!(control is IntegerControl integer))
                        continue;

                    if (integers.Contains(integer))
                    {
                        Assert.That(getValue(integer), Is.EqualTo(value), $"Value for integer '{integer}' is not correct!");
                    }
                    else
                    {
                        float defaultValue = integer.ReadDefaultValue();
                        Assert.That(getValue(integer), Is.EqualTo(defaultValue), $"Expected default value for integer '{integer}'!");
                    }
                }
            }
        }

        public static void RecognizesButton<TState>(InputDevice device, TState state, ButtonControl button,
            SetButtonAction<TState> setButton)
            where TState : unmanaged, IInputStateTypeInfo
        {
            setButton(ref state, true);
            AssertButtonPress(device, state, button);

            setButton(ref state, false);
            AssertButtonPress(device, state);
        }

        public static void RecognizesAxis<TState>(InputDevice device, TState state, AxisControl axis, AxisMode mode,
            SetAxisAction<TState> setAxis)
            where TState : unmanaged, IInputStateTypeInfo
        {
            switch (mode)
            {
                case AxisMode.Signed: RecognizesSignedAxis(device, state, axis, setAxis); break;
                case AxisMode.Unsigned: RecognizesUnsignedAxis(device, state, axis, setAxis); break;
                case AxisMode.Button: RecognizesButtonAxis(device, state, axis, setAxis); break;
                default: throw new NotImplementedException($"Unhandled axis mode {mode}!");
            }
        }

        public static void RecognizesUnsignedAxis<TState>(InputDevice device, TState state, AxisControl axis,
            SetAxisAction<TState> setAxis)
            where TState : unmanaged, IInputStateTypeInfo
        {
            for (int i = 0; i <= 100; i++)
            {
                float value = i / 100f;
                setAxis(ref state, value);
                AssertAxisValue(device, state, value, 1 / 100f, axis);
            }
        }

        public static void RecognizesSignedAxis<TState>(InputDevice device, TState state, AxisControl axis,
            SetAxisAction<TState> setAxis)
            where TState : unmanaged, IInputStateTypeInfo
        {
            for (int i = -100; i <= 100; i++)
            {
                float value = i / 100f;
                setAxis(ref state, value);
                AssertAxisValue(device, state, value, 1 / 100f, axis);
            }
        }

        public static void RecognizesButtonAxis<TState>(InputDevice device, TState state, AxisControl button,
            SetAxisAction<TState> setButton)
            where TState : unmanaged, IInputStateTypeInfo
        {
            setButton(ref state, 1f);
            AssertAxisValue(device, state, 1f, 0.001f, button);

            setButton(ref state, 0f);
            AssertAxisValue(device, state, 0f, 0.001f, button);
        }

        [Obsolete("Drastically slows down tests for whatever reason, mark tests that use this as [Ignored]")]
        public static void RecognizesStick<TState>(InputDevice device, TState state, StickControl stick,
            SetStickAction<TState> setStick)
            where TState : unmanaged, IInputStateTypeInfo
        {
            // Sticks automatically have deadzones applied to them,
            // so we must account for it
            var axisDeadzone = new AxisDeadzoneProcessor()
            {
                min = InputSystem.settings.defaultDeadzoneMin,
                max = InputSystem.settings.defaultDeadzoneMax,
            };

            void AddAxes(List<(AxisControl, float)> axisList, float value,
                AxisControl axis, AxisControl pos, AxisControl neg)
            {
                axisList.Add((axis, value));

                if (value > 0f) axisList.Add((pos, value));
                else if (value < 0f) axisList.Add((neg, -value));
            }

            void AddX(List<(AxisControl, float)> axisList, float value)
                => AddAxes(axisList, value, stick.x, stick.right, stick.left);

            void AddY(List<(AxisControl, float)> axisList, float value)
                => AddAxes(axisList, value, stick.y, stick.up, stick.down);

            const int range = 100;
            const float division = range;
            const float epsilon = 1 / division;

            var axes = new List<(AxisControl axis, float value)>();
            for (int i = -range; i <= range; i++)
            {
                float value = i / division;
                // TODO: why in the world does this add 5 whole minutes to the execution time???
                float deadzoned = axisDeadzone.Process(value);

                // X axis
                setStick(ref state, value, 0f);
                AddX(axes, deadzoned);
                AssertAxisValues(device, state, epsilon, axes);
                axes.Clear();

                // Y axis
                setStick(ref state, 0f, value);
                AddY(axes, deadzoned);
                AssertAxisValues(device, state, epsilon, axes);
                axes.Clear();

                // Both axes
                setStick(ref state, value, value);
                AddX(axes, deadzoned);
                AddY(axes, deadzoned);
                AssertAxisValues(device, state, epsilon, axes);
                axes.Clear();
            }
        }
    }
}