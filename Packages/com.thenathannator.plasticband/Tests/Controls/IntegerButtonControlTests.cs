using NUnit.Framework;
using PlasticBand.Controls;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Controls
{
    internal struct IntegerButtonState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('I', 'B', 'T', 'N');

        // For this test to work, the min and max value must be
        // equal (in terms of absolute value) and of opposite signs
        public const int MinValue = -MaxValue;
        public const int MaxValue = 100;
        public const int IntPressPoint = 0;
        public const float PressPoint = ((float)IntPressPoint - MinValue) / (MaxValue - MinValue);
        public const int NullValue = MaxValue / 10;

        [InputControl(layout = "IntButton", parameters = "minValue=-100,maxValue=100,intPressPoint=0,hasNullValue,nullValue=10")]
        public int intButton;
    }

    [InputControlLayout(stateType = typeof(IntegerButtonState), hideInUI = true)]
    internal class IntegerButtonDevice : InputDevice
    {
        public IntegerButtonControl intButton { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            intButton = GetChildControl<IntegerButtonControl>(nameof(intButton));
        }
    }

    internal class IntegerButtonControlTests : PlasticBandTestFixture<IntegerButtonDevice>
    {
        private const float kEpsilon = 1f / (IntegerButtonState.MaxValue - IntegerButtonState.MinValue);

        public override void Setup()
        {
            base.Setup();
            InputSystem.RegisterLayout<IntegerButtonDevice>(nameof(IntegerButtonDevice));
        }

        public override void TearDown()
        {
            InputSystem.RemoveLayout(nameof(IntegerButtonDevice));
            base.TearDown();
        }

        [Test]
        public void SetsMinValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intButton.minValue, Is.EqualTo(IntegerButtonState.MinValue));
        });

        [Test]
        public void SetsMaxValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intButton.maxValue, Is.EqualTo(IntegerButtonState.MaxValue));
        });

        [Test]
        public void SetsIntPressPoint() => CreateAndRun((device) =>
        {
            Assert.That(device.intButton.intPressPoint, Is.EqualTo(IntegerButtonState.IntPressPoint));
        });

        [Test]
        public void SetsPressPoint() => CreateAndRun((device) =>
        {
            Assert.That(device.intButton.pressPoint,
                Is.InRange(IntegerButtonState.PressPoint - kEpsilon, IntegerButtonState.PressPoint + kEpsilon));
        });

        [Test]
        public void SetsHasNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intButton.hasNullValue, Is.True);
        });

        [Test]
        public void SetsNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.intButton.nullValue, Is.EqualTo(IntegerButtonState.NullValue));
        });

        [Test]
        public void HandlesMaxValue() => CreateAndRun((device) =>
        {
            var state = new IntegerButtonState() { intButton = IntegerButtonState.MaxValue };
            AssertAxisValue(device, state, 1f, kEpsilon, device.intButton);
        });

        [Test]
        public void HandlesMinValue() => CreateAndRun((device) =>
        {
            var state = new IntegerButtonState() { intButton = IntegerButtonState.MinValue };
            AssertAxisValue(device, state, 0f, kEpsilon, device.intButton);
        });

        [Test]
        public void HandlesPressPoint() => CreateAndRun((device) =>
        {
            // Should be pressed when directly at the press point
            var state = new IntegerButtonState() { intButton = IntegerButtonState.IntPressPoint };
            AssertButtonPress(device, state, device.intButton);

            // Should be pressed when above the press point
            state.intButton = IntegerButtonState.MaxValue;
            AssertButtonPress(device, state, device.intButton);

            state.intButton = IntegerButtonState.IntPressPoint + 10;
            AssertButtonPress(device, state, device.intButton);

            state.intButton = IntegerButtonState.IntPressPoint + 1;
            AssertButtonPress(device, state, device.intButton);

            // Should *not* be pressed when below the press point
            state.intButton = IntegerButtonState.IntPressPoint - 1;
            AssertButtonPress(device, state);

            state.intButton = IntegerButtonState.IntPressPoint - 10;
            AssertButtonPress(device, state);

            state.intButton = IntegerButtonState.MinValue;
            AssertButtonPress(device, state);
        });

        [Test]
        public void HandlesNullValue() => CreateAndRun((device) =>
        {
            AssertValueNullChecked(device, IntegerButtonState.MaxValue, 1f);
            AssertValueNullChecked(device, IntegerButtonState.MinValue, 0f);
        });

        [Test]
        public void HandlesRange() => CreateAndRun((device) =>
        {
            // IntegerButton outputs an analog value, so we test this similarly to IntegerAxis
            for (int i = IntegerButtonState.MinValue; i <= IntegerButtonState.MaxValue; i++)
            {
                // Ignore null value
                if (i == IntegerButtonState.NullValue)
                    continue;

                float expectedValue = (float)(i - IntegerButtonState.MinValue) / (IntegerButtonState.MaxValue * 2);
                AssertValueNullChecked(device, i, expectedValue);

                // There is no negative range, the value should always be 0 or greater
                float actualValue = device.intButton.value;
                Assert.That(actualValue, Is.Not.Negative);

                // Should only be pressed when above the press point
                bool pressed = device.intButton.isPressed;
                Assert.That(pressed, Is.EqualTo(i >= IntegerButtonState.IntPressPoint), $"Value {actualValue} (raw: {i}) should be considered pressed but isn't!");
            }
        });

        private static void AssertValueNullChecked(IntegerButtonDevice device, int rawValue, float normalValue)
        {
            var state = new IntegerButtonState() { intButton = rawValue };
            AssertAxisValue(device, state, normalValue, kEpsilon, device.intButton);

            // Test that null value does not affect state
            state.intButton = IntegerButtonState.NullValue;
            AssertAxisValue(device, state, normalValue, kEpsilon, device.intButton);
        }
    }
}