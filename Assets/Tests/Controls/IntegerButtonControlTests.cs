using NUnit.Framework;
using PlasticBand.Controls;
using UnityEngine;
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

            public const int MinValue = -100;
            public const int MaxValue = 100;
            public const int PressPoint = 0;

            [InputControl(layout = "IntButton", parameters = "minValue=-100,maxValue=100,intPressPoint=0")]
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
        public void CanCreate() => TestHelpers.AssertDeviceCreation<IntegerButtonDevice>((device) =>
        {
            var intButton = device.intButton;
            Assert.That(intButton, Is.Not.Null);
            Assert.That(intButton.minValue, Is.EqualTo(IntegerButtonState.MinValue));
            Assert.That(intButton.maxValue, Is.EqualTo(IntegerButtonState.MaxValue));
            Assert.That(intButton.intPressPoint, Is.EqualTo(IntegerButtonState.PressPoint));
            Assert.That(intButton.pressPoint, Is.GreaterThan(-1f));
        });

        [Test]
        public void HandlesState() => TestHelpers.CreateAndRun<IntegerButtonDevice>((device) =>
        {
            // IntegerButton outputs an analog value, so we test this in the same way as IntegerAxis
            // There is no negative range, the value should always be 0 or greater
            TestButton(device, IntegerButtonState.MinValue, 0f);

            for (int i = 1; i <= 4; i++)
            {
                float factor = 0.5f * i;
                float value = Mathf.LerpUnclamped(IntegerButtonState.MinValue, IntegerButtonState.MaxValue, factor);
                TestButton(device, Mathf.RoundToInt(value), factor);
            }
        });

        private void TestButton(IntegerButtonDevice device, int rawValue, float normalValue)
        {
            const float epsilon = 1f / byte.MaxValue;

            var state = new IntegerButtonState() { intButton = rawValue };
            TestHelpers.AssertAxisValue(device, state, normalValue, epsilon, device.intButton);
            Assert.That(device.intButton.value, Is.Not.Negative);
            Assert.That(device.intButton.isPressed, Is.EqualTo(state.intButton >= IntegerButtonState.PressPoint));
        }
    }
}