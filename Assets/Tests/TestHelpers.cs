using System;
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