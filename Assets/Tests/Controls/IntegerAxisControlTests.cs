using NUnit.Framework;
using PlasticBand.Controls;
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

            // For this test to work, the min and max value must be
            // equal (in terms of absolute value) and of opposite signs
            public const int MinValue = -MaxValue;
            public const int MaxValue = 100;
            public const int ZeroPoint = 0;
            public const int NullValue = MaxValue / 10;

            [InputControl(layout = "IntAxis", parameters = "minValue=-100,maxValue=100,zeroPoint=0,hasNullValue,nullValue=10")]
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
        public void CanCreate() => AssertDeviceCreation<IntegerAxisDevice>((device) =>
        {
            var intAxis = device.intAxis;

            Assert.That(intAxis.minValue, Is.EqualTo(IntegerAxisState.MinValue));
            Assert.That(intAxis.maxValue, Is.EqualTo(IntegerAxisState.MaxValue));
            Assert.That(intAxis.zeroPoint, Is.EqualTo(IntegerAxisState.ZeroPoint));

            Assert.That(intAxis.hasNullValue, Is.True);
            Assert.That(intAxis.nullValue, Is.EqualTo(IntegerAxisState.NullValue));
        });

        [Test]
        public void HandlesState() => CreateAndRun<IntegerAxisDevice>((device) =>
        {
            const float epsilon = 1f / IntegerAxisState.MaxValue;

            // Zero point
            var state = new IntegerAxisState() { intAxis = IntegerAxisState.ZeroPoint };
            AssertAxisValue(device, state, 0f, epsilon, device.intAxis);

            for (int i = 0; i <= IntegerAxisState.MaxValue; i++)
            {
                // Ignore null value
                if (i == IntegerAxisState.NullValue)
                    continue;

                float expectedValue = (float)i / IntegerAxisState.MaxValue;

                // Positive
                state.intAxis = i;
                AssertAxisValue(device, state, expectedValue, epsilon, device.intAxis);

                // Test that null value does not affect state
                state.intAxis = IntegerAxisState.NullValue;
                AssertAxisValue(device, state, expectedValue, epsilon, device.intAxis);

                // Negative
                state.intAxis = -i;
                AssertAxisValue(device, state, -expectedValue, epsilon, device.intAxis);

                // Test that null value does not affect state
                state.intAxis = IntegerAxisState.NullValue;
                AssertAxisValue(device, state, -expectedValue, epsilon, device.intAxis);
            }
        });
    }
}