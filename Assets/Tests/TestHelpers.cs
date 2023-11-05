using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests
{
    public static class TestHelpers
    {
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
    }
}