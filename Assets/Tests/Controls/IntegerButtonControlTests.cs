using NUnit.Framework;
using PlasticBand.Controls;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Controls
{
    public class IntegerButtonControlTests : PlasticBandTestFixture
    {
        private struct IntegerButtonState : IInputStateTypeInfo
        {
            public FourCC format => new FourCC('I', 'B', 'T', 'N');

            // For this test to work, the min and max value must be
            // equal (in terms of absolute value) and of opposite signs
            public const int MinValue = -MaxValue;
            public const int MaxValue = 100;
            public const int PressPoint = 0;
            public const int NullValue = MaxValue / 10;

            [InputControl(layout = "IntButton", parameters = "minValue=-100,maxValue=100,intPressPoint=0,hasNullValue,nullValue=10")]
            public int intButton;
        }

        [InputControlLayout(stateType = typeof(IntegerButtonState), hideInUI = true)]
        private class IntegerButtonDevice : InputDevice
        {
            public IntegerButtonControl intButton { get; private set; }

            protected override void FinishSetup()
            {
                base.FinishSetup();
                intButton = GetChildControl<IntegerButtonControl>(nameof(intButton));
            }
        }

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
        public void CanCreate() => AssertDeviceCreation<IntegerButtonDevice>((device) =>
        {
            var intButton = device.intButton;

            Assert.That(intButton.minValue, Is.EqualTo(IntegerButtonState.MinValue));
            Assert.That(intButton.maxValue, Is.EqualTo(IntegerButtonState.MaxValue));
            Assert.That(intButton.intPressPoint, Is.EqualTo(IntegerButtonState.PressPoint));
            Assert.That(intButton.pressPoint, Is.GreaterThan(-1f));

            Assert.That(intButton.hasNullValue, Is.True);
            Assert.That(intButton.nullValue, Is.EqualTo(IntegerButtonState.NullValue));
        });

        [Test]
        public void HandlesState() => CreateAndRun<IntegerButtonDevice>((device) =>
        {
            // IntegerButton outputs an analog value, so we test this in the same way as IntegerAxis
            TestButton(device, IntegerButtonState.MinValue, 0f);

            for (int i = IntegerButtonState.MinValue; i <= IntegerButtonState.MaxValue; i++)
            {
                // Ignore null value
                if (i == IntegerButtonState.NullValue)
                    continue;

                float expectedValue = (float)(i - IntegerButtonState.MinValue) / (IntegerButtonState.MaxValue * 2);
                TestButton(device, i, expectedValue);

                // Test that null value does not affect state
                TestButton(device, IntegerButtonState.NullValue, expectedValue);
            }
        });

        private void TestButton(IntegerButtonDevice device, int rawValue, float normalValue)
        {
            const float epsilon = 1f / IntegerButtonState.MaxValue;

            var state = new IntegerButtonState() { intButton = rawValue };
            AssertAxisValue(device, state, normalValue, epsilon, device.intButton);

            // There is no negative range, the value should always be 0 or greater
            float actualValue = device.intButton.value;
            Assert.That(actualValue, Is.Not.Negative);

            if (state.intButton != IntegerButtonState.NullValue)
            {
                // Should only be pressed when above the press point
                bool pressed = device.intButton.isPressed;
                Assert.That(pressed, Is.EqualTo(state.intButton >= IntegerButtonState.PressPoint), $"Value {actualValue} ({state.intButton}) should be considered pressed but isn't!");
            }
        }
    }
}