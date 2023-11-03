using NUnit.Framework;
using PlasticBand.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Controls
{
    public class IntegerAxisControlTests : PlasticBandTestFixture
    {
        private struct IntegerAxisState : IInputStateTypeInfo
        {
            public FourCC format => new FourCC('I', 'A', 'X', 'S');

            public const int MinValue = -100;
            public const int MaxValue = 100;
            public const int ZeroPoint = 0;

            [InputControl(layout = "IntAxis", parameters = "minValue=-100,maxValue=100,zeroPoint=0")]
            public int intAxis;
        }

        [InputControlLayout(stateType = typeof(IntegerAxisState), hideInUI = true)]
        private class IntegerAxisDevice : InputDevice
        {
            public IntegerAxisControl intAxis { get; private set; }

            protected override void FinishSetup()
            {
                base.FinishSetup();
                intAxis = GetChildControl<IntegerAxisControl>(nameof(intAxis));
            }
        }

        public override void Setup()
        {
            base.Setup();
            InputSystem.RegisterLayout<IntegerAxisDevice>(nameof(IntegerAxisDevice));
        }

        public override void TearDown()
        {
            InputSystem.RemoveLayout(nameof(IntegerAxisDevice));
            base.TearDown();
        }

        [Test]
        public void CanCreate() => TestHelpers.AssertDeviceCreation<IntegerAxisDevice>((device) =>
        {
            var intAxis = device.intAxis;
            Assert.That(intAxis, Is.Not.Null);
            Assert.That(intAxis.minValue, Is.EqualTo(IntegerAxisState.MinValue));
            Assert.That(intAxis.maxValue, Is.EqualTo(IntegerAxisState.MaxValue));
            Assert.That(intAxis.zeroPoint, Is.EqualTo(IntegerAxisState.ZeroPoint));
        });

        [Test]
        public void HandlesState() => TestHelpers.CreateAndRun<IntegerAxisDevice>((device) =>
        {
            const float epsilon = 1f / byte.MaxValue;

            // Zero point
            var state = new IntegerAxisState() { intAxis = IntegerAxisState.ZeroPoint };
            TestHelpers.AssertAxisValue(device, state, 0f, epsilon, device.intAxis);

            for (int i = 1; i <= 4; i++)
            {
                float factor = 0.5f * i;

                // Positive
                float value = Mathf.LerpUnclamped(IntegerAxisState.ZeroPoint, IntegerAxisState.MaxValue, factor);
                state.intAxis = Mathf.RoundToInt(value);
                TestHelpers.AssertAxisValue(device, state, factor, epsilon, device.intAxis);

                // Negative
                value = Mathf.LerpUnclamped(IntegerAxisState.ZeroPoint, IntegerAxisState.MinValue, factor);
                state.intAxis = Mathf.RoundToInt(value);
                TestHelpers.AssertAxisValue(device, state, -factor, epsilon, device.intAxis);
            }
        });
    }
}