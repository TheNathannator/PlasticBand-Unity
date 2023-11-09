using NUnit.Framework;
using PlasticBand.Controls;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Controls
{
    internal struct IntegerAxisState : IInputStateTypeInfo
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
    internal class IntegerAxisDevice : InputDevice
    {
        public IntegerAxisControl intAxis { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            intAxis = GetChildControl<IntegerAxisControl>(nameof(intAxis));
        }
    }

    internal class IntegerAxisControlTests : PlasticBandTestFixture<IntegerAxisDevice>
    {
        private const float kEpsilon = 1f / IntegerAxisState.MaxValue;

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
        public void SetsMinValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intAxis.minValue, Is.EqualTo(IntegerAxisState.MinValue));
        });

        [Test]
        public void SetsMaxValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intAxis.maxValue, Is.EqualTo(IntegerAxisState.MaxValue));
        });

        [Test]
        public void SetsZeroPoint() => CreateAndRun((device) =>
        {
            Assert.That(device.intAxis.zeroPoint, Is.EqualTo(IntegerAxisState.ZeroPoint));
        });

        [Test]
        public void SetsHasNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intAxis.hasNullValue, Is.True);
        });

        [Test]
        public void SetsNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intAxis.nullValue, Is.EqualTo(IntegerAxisState.NullValue));
        });

        [Test]
        public void HandlesMaxValue() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState() { intAxis = IntegerAxisState.MaxValue };
            AssertAxisValue(device, state, 1f, kEpsilon, device.intAxis);
        });

        [Test]
        public void HandlesMinValue() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState() { intAxis = IntegerAxisState.MinValue };
            AssertAxisValue(device, state, -1f, kEpsilon, device.intAxis);
        });

        [Test]
        public void HandlesZeroPoint() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState() { intAxis = IntegerAxisState.ZeroPoint };
            AssertAxisValue(device, state, 0f, kEpsilon, device.intAxis);
        });

        [Test]
        public void HandlesNullValue() => CreateAndRun((device) =>
        {
            AssertValueNullChecked(device, IntegerAxisState.MaxValue, 1f);
            AssertValueNullChecked(device, IntegerAxisState.MinValue, -1f);
            AssertValueNullChecked(device, IntegerAxisState.ZeroPoint, 0f);
        });

        [Test]
        public void HandlesRange() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState() { intAxis = IntegerAxisState.ZeroPoint };
            for (int i = 0; i <= IntegerAxisState.MaxValue; i++)
            {
                // Ignore null value
                if (i == IntegerAxisState.NullValue)
                    continue;

                float expectedValue = (float)i / IntegerAxisState.MaxValue;

                // Positive
                AssertValueNullChecked(device, i, expectedValue);

                // Negative
                AssertValueNullChecked(device, -i, -expectedValue);
            }
        });

        private static void AssertValueNullChecked(IntegerAxisDevice device, int rawValue, float normalValue)
        {
            var state = new IntegerAxisState() { intAxis = rawValue };
            AssertAxisValue(device, state, normalValue, kEpsilon, device.intAxis);

            // Test that null value does not affect state
            state.intAxis = IntegerAxisState.NullValue;
            AssertAxisValue(device, state, normalValue, kEpsilon, device.intAxis);
        }
    }
}