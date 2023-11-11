using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

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

                // Ignore the `parent` property
                string name = property.Name;
                if (name == nameof(InputDevice.parent))
                    continue;

                // Ensure the returned control is not null
                var control = property.GetMethod.Invoke(device, null);
                Assert.That(control, Is.Not.Null, $"Control {name} on device {device} is not set!");
            }
        });

        public static void CreateAndRun(Action<TDevice> action)
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

        public static void AssertAxisValue<TState>(InputDevice device, TState state,
            float value, float epsilon, params AxisControl[] axes)
            where TState : unmanaged, IInputStateTypeInfo
        {
            InputSystem.QueueStateEvent(device, state);
            InputSystem.Update();
            AssertAxisValue(value, epsilon, axes);
        }

        public static void AssertAxisValue(float value, float epsilon, params AxisControl[] axes)
        {
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

        public delegate void AssertButtonsAction<TMask>(TMask result, TMask expected, Func<ButtonControl, bool> getPressed);

        /// <summary>
        /// Asserts the state of buttons across both input events and post-update properties.
        /// </summary>
        public static void AssertButtonsWithEventUpdate<TState, TMask>(InputDevice device, TState state,
            TMask targetMask, Func<TMask> getMask, Func<InputEventPtr, TMask> getMaskFromEvent,
            AssertButtonsAction<TMask> assertMask)
            where TState : unmanaged, IInputStateTypeInfo
        {
            void UpdateAssert() => assertMask(getMask(), targetMask, (button) => button.isPressed);
            void EventAssert(InputEventPtr eventPtr)
                => assertMask(getMaskFromEvent(eventPtr), targetMask, (button) => button.IsPressedInEvent(eventPtr));

            // Input event assert
            using (InputSystem.onEvent.Assert(EventAssert))
            {
                InputSystem.QueueStateEvent(device, state);
                InputSystem.Update();
            }

            // Post-update assert
            UpdateAssert();

            // Input event assert using post-update state
            // Not strictly necessary, but might as well to be thorough
            using (StateEvent.From(device, out var eventPtr))
            {
                EventAssert(eventPtr);
            }
        }
    }
}