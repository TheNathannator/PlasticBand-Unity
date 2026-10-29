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

        [InputControl(layout = "IntButton", defaultState = 0, parameters = "minValue=0,maxValue=255,intPressPoint=192")]
        public byte byteButton;

        [InputControl(layout = "IntButton", defaultState = -128, parameters = "minValue=-128,maxValue=127,intPressPoint=64")]
        public sbyte sbyteButton;

        [InputControl(layout = "IntButton", defaultState = 255, parameters = "minValue=255,maxValue=0,intPressPoint=63")]
        public byte invertedByteButton;

        [InputControl(layout = "IntButton", defaultState = 127, parameters = "minValue=127,maxValue=-128,intPressPoint=-65")]
        public sbyte invertedSbyteButton;

        [InputControl(layout = "IntButton", defaultState = 0, parameters = "minValue=0,maxValue=255,intPressPoint=192,hasNullValue,nullValue=160")]
        public byte nullByteButton;

        [InputControl(layout = "IntButton", defaultState = -128, parameters = "minValue=-128,maxValue=127,intPressPoint=64,hasNullValue,nullValue=32")]
        public sbyte nullSbyteButton;

        public static IntegerButtonState Default()
        {
            return new IntegerButtonState()
            {
                byteButton = 0,
                sbyteButton = -128,
                invertedByteButton = 255,
                invertedSbyteButton = 127,
                nullByteButton = 0,
                nullSbyteButton = -128,
            };
        }
    }

    [InputControlLayout(stateType = typeof(IntegerButtonState), hideInUI = true)]
    internal class IntegerButtonDevice : InputDevice
    {
        public IntegerButtonControl byteButton { get; private set; }
        public IntegerButtonControl sbyteButton { get; private set; }
        public IntegerButtonControl invertedByteButton { get; private set; }
        public IntegerButtonControl invertedSbyteButton { get; private set; }
        public IntegerButtonControl nullByteButton { get; private set; }
        public IntegerButtonControl nullSbyteButton { get; private set; }

        public IntegerButtonControl[] allButtons { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            byteButton = GetChildControl<IntegerButtonControl>(nameof(byteButton));
            sbyteButton = GetChildControl<IntegerButtonControl>(nameof(sbyteButton));
            invertedByteButton = GetChildControl<IntegerButtonControl>(nameof(invertedByteButton));
            invertedSbyteButton = GetChildControl<IntegerButtonControl>(nameof(invertedSbyteButton));
            nullByteButton = GetChildControl<IntegerButtonControl>(nameof(nullByteButton));
            nullSbyteButton = GetChildControl<IntegerButtonControl>(nameof(nullSbyteButton));

            allButtons = new IntegerButtonControl[]
            {
                byteButton,
                sbyteButton,
                invertedByteButton,
                invertedSbyteButton,
                nullByteButton,
                nullSbyteButton,
            };
        }
    }

    internal class IntegerButtonControlTests : PlasticBandTestFixture<IntegerButtonDevice>
    {
        private const float kEpsilon = 1f / 255f;

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
            Assert.That(device.byteButton.minValue,          Is.EqualTo(0), "byteButton has an incorrect minimum value!");
            Assert.That(device.sbyteButton.minValue,         Is.EqualTo(-128), "sbyteButton has an incorrect minimum value!");
            Assert.That(device.invertedByteButton.minValue,  Is.EqualTo(255), "invertedByteButton has an incorrect minimum value!");
            Assert.That(device.invertedSbyteButton.minValue, Is.EqualTo(127), "invertedSbyteButton has an incorrect minimum value!");
            Assert.That(device.nullByteButton.minValue,      Is.EqualTo(0), "nullByteButton has an incorrect minimum value!");
            Assert.That(device.nullSbyteButton.minValue,     Is.EqualTo(-128), "nullSbyteButton has an incorrect minimum value!");
        });

        [Test]
        public void SetsMaxValue() => CreateAndRun((device) =>
        {
            Assert.That(device.byteButton.maxValue,          Is.EqualTo(255), "byteButton has an incorrect maximum value!");
            Assert.That(device.sbyteButton.maxValue,         Is.EqualTo(127), "sbyteButton has an incorrect maximum value!");
            Assert.That(device.invertedByteButton.maxValue,  Is.EqualTo(0), "invertedByteButton has an incorrect maximum value!");
            Assert.That(device.invertedSbyteButton.maxValue, Is.EqualTo(-128), "invertedSbyteButton has an incorrect maximum value!");
            Assert.That(device.nullByteButton.maxValue,      Is.EqualTo(255), "nullByteButton has an incorrect maximum value!");
            Assert.That(device.nullSbyteButton.maxValue,     Is.EqualTo(127), "nullSbyteButton has an incorrect maximum value!");
        });

        [Test]
        public void SetsIntPressPoint() => CreateAndRun((device) =>
        {
            Assert.That(device.byteButton.intPressPoint,          Is.EqualTo(192), "byteButton has an incorrect press point!");
            Assert.That(device.sbyteButton.intPressPoint,         Is.EqualTo(64), "sbyteButton has an incorrect press point!");
            Assert.That(device.invertedByteButton.intPressPoint,  Is.EqualTo(63), "invertedByteButton has an incorrect press point!");
            Assert.That(device.invertedSbyteButton.intPressPoint, Is.EqualTo(-65), "invertedSbyteButton has an incorrect press point!");
            Assert.That(device.nullByteButton.intPressPoint,      Is.EqualTo(192), "nullByteButton has an incorrect press point!");
            Assert.That(device.nullSbyteButton.intPressPoint,     Is.EqualTo(64), "nullSbyteButton has an incorrect press point!");
        });

        [Test]
        public void SetsBasePressPoint() => CreateAndRun((device) =>
        {
            Assert.That(device.byteButton.pressPoint,          Is.InRange(0.75f - kEpsilon, 0.75f + kEpsilon), "byteButton did not transfer its press point correctly!");
            Assert.That(device.sbyteButton.pressPoint,         Is.InRange(0.75f - kEpsilon, 0.75f + kEpsilon), "sbyteButton did not transfer its press point correctly!");
            Assert.That(device.invertedByteButton.pressPoint,  Is.InRange(0.75f - kEpsilon, 0.75f + kEpsilon), "invertedByteButton did not transfer its press point correctly!");
            Assert.That(device.invertedSbyteButton.pressPoint, Is.InRange(0.75f - kEpsilon, 0.75f + kEpsilon), "invertedSbyteButton did not transfer its press point correctly!");
            Assert.That(device.nullByteButton.pressPoint,      Is.InRange(0.75f - kEpsilon, 0.75f + kEpsilon), "nullByteButton did not transfer its press point correctly!");
            Assert.That(device.nullSbyteButton.pressPoint,     Is.InRange(0.75f - kEpsilon, 0.75f + kEpsilon), "nullSbyteButton did not transfer its press point correctly!");
        });

        [Test]
        public void SetsHasNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.byteButton.hasNullValue,          Is.EqualTo(false), "byteButton reported the wrong null value presence!");
            Assert.That(device.sbyteButton.hasNullValue,         Is.EqualTo(false), "sbyteButton reported the wrong null value presence!");
            Assert.That(device.invertedByteButton.hasNullValue,  Is.EqualTo(false), "invertedByteButton reported the wrong null value presence!");
            Assert.That(device.invertedSbyteButton.hasNullValue, Is.EqualTo(false), "invertedSbyteButton reported the wrong null value presence!");
            Assert.That(device.nullByteButton.hasNullValue,      Is.EqualTo(true), "nullByteButton reported the wrong null value presence!");
            Assert.That(device.nullSbyteButton.hasNullValue,     Is.EqualTo(true), "nullSbyteButton reported the wrong null value presence!");
        });

        [Test]
        public void SetsNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.byteButton.nullValue,          Is.EqualTo(0), "byteButton has an incorrect null value!");
            Assert.That(device.sbyteButton.nullValue,         Is.EqualTo(0), "sbyteButton has an incorrect null value!");
            Assert.That(device.invertedByteButton.nullValue,  Is.EqualTo(0), "invertedByteButton has an incorrect null value!");
            Assert.That(device.invertedSbyteButton.nullValue, Is.EqualTo(0), "invertedSbyteButton has an incorrect null value!");
            Assert.That(device.nullByteButton.nullValue,      Is.EqualTo(160), "nullByteButton has an incorrect null value!");
            Assert.That(device.nullSbyteButton.nullValue,     Is.EqualTo(32), "nullSbyteButton has an incorrect null value!");
        });

        [Test]
        public void HandlesMinValue() => CreateAndRun((device) =>
        {
            var state = new IntegerButtonState()
            {
                byteButton = 0,
                sbyteButton = -128,
                invertedByteButton = 255,
                invertedSbyteButton = 127,
                nullByteButton = 0,
                nullSbyteButton = -128,
            };
            AssertAxisValue(device, state, 0f, 0f, device.allButtons);
            AssertButtonPress(device, state);
        });

        [Test]
        public void HandlesMaxValue() => CreateAndRun((device) =>
        {
            var state = new IntegerButtonState()
            {
                byteButton = 255,
                sbyteButton = 127,
                invertedByteButton = 0,
                invertedSbyteButton = -128,
                nullByteButton = 255,
                nullSbyteButton = 127,
            };
            AssertAxisValue(device, state, 1f, 0f, device.allButtons);
            AssertButtonPress(device, state, device.allButtons);
        });

        [Test]
        public void HandlesPressPoint() => CreateAndRun((device) =>
        {
            // Axis value is not tested here, for simplicity

            // No press
            var state = new IntegerButtonState()
            {
                byteButton = 0,
                sbyteButton = -128,
                invertedByteButton = 255,
                invertedSbyteButton = 127,
                nullByteButton = 0,
                nullSbyteButton = -128,
            };
            AssertButtonPress(device, state);

            // Press point - 1
            state = new IntegerButtonState()
            {
                byteButton = 0 + 191,
                sbyteButton = -128 + 191,
                invertedByteButton = 255 - 191,
                invertedSbyteButton = 191 - 191,
                nullByteButton = 0 + 191,
                nullSbyteButton = -128 + 191,
            };
            AssertButtonPress(device, state);

            // Press point
            state = new IntegerButtonState()
            {
                byteButton = 0 + 192,
                sbyteButton = -128 + 192,
                invertedByteButton = 255 - 192,
                invertedSbyteButton = 127 - 192,
                nullByteButton = 0 + 192,
                nullSbyteButton = -128 + 192,
            };
            AssertButtonPress(device, state, device.allButtons);

            // Press point + 1
            state = new IntegerButtonState()
            {
                byteButton = 0 + 193,
                sbyteButton = -128 + 193,
                invertedByteButton = 255 - 193,
                invertedSbyteButton = 127 - 193,
                nullByteButton = 0 + 193,
                nullSbyteButton = -128 + 193,
            };
            AssertButtonPress(device, state, device.allButtons);
        });

        [Test]
        public void HandlesNullValue() => CreateAndRun((device) =>
        {
            var state = IntegerButtonState.Default();
            AssertAxisValue(device, state, 0f, 0f, device.allButtons);

            for (int i = 0; i <= 255; i++)
            {
                if (i == 160)
                {
                    continue;
                }

                float expected = i / 255f;
                state.nullByteButton = (byte)i;
                state.nullSbyteButton = (sbyte)(i - 128);
                AssertAxisValue(device, state, expected, kEpsilon, device.nullByteButton, device.nullSbyteButton);

                // No change in value should occur at the null state
                state.nullByteButton = 160;
                state.nullSbyteButton = 32;
                AssertAxisValue(device, state, expected, kEpsilon, device.nullByteButton, device.nullSbyteButton);
            }
        });

        [Test]
        public void HandlesRange() => CreateAndRun((device) =>
        {
            var state = IntegerButtonState.Default();
            AssertAxisValue(device, state, 0f, 0f, device.allButtons);
            AssertButtonPress(device, state);

            // byteButton
            for (int i = 0; i <= 255; i++)
            {
                float expected = i / 255f;
                state.byteButton = (byte)i;

                AssertValueAndPress(device, ref state, i, expected, device.byteButton);
            }

            // sbyteButton
            state = IntegerButtonState.Default();
            for (int i = 0; i <= 255; i++)
            {
                float expected = i / 255f;
                state.sbyteButton = (sbyte)(i - 128);

                AssertValueAndPress(device, ref state, i, expected, device.sbyteButton);
            }

            // invertedByteButton
            state = IntegerButtonState.Default();
            for (int i = 0; i <= 255; i++)
            {
                float expected = i / 255f;
                state.invertedByteButton = (byte)(255 - i);

                AssertValueAndPress(device, ref state, i, expected, device.invertedByteButton);
            }

            // invertedSbyteButton
            state = IntegerButtonState.Default();
            for (int i = 0; i <= 255; i++)
            {
                float expected = i / 255f;
                state.invertedSbyteButton = (sbyte)(127 - i);

                AssertValueAndPress(device, ref state, i, expected, device.invertedSbyteButton);
            }

            // nullByteButton
            state = IntegerButtonState.Default();
            float lastExpected = float.NegativeInfinity;
            for (int i = 0; i <= 255; i++)
            {
                state.nullByteButton = (byte)i;
                float expected = i == 32 // Null value
                    ? lastExpected
                    : i / 255f;

                AssertValueAndPress(device, ref state, i, expected, device.nullByteButton);

                lastExpected = expected;
            }

            // nullSbyteButton
            state = IntegerButtonState.Default();
            lastExpected = float.NegativeInfinity;
            for (int i = 0; i <= 255; i++)
            {
                state.nullSbyteButton = (sbyte)(i - 128);
                float expected = state.nullSbyteButton == 32 // Null value
                    ? lastExpected
                    : i / 255f;

                AssertValueAndPress(device, ref state, i, expected, device.nullSbyteButton);

                lastExpected = expected;
            }
        });

        private void AssertValueAndPress(
            IntegerButtonDevice device,
            ref IntegerButtonState state,
            int i,
            float expected,
            IntegerButtonControl button
        )
        {
            AssertAxisValue(device, state, expected, kEpsilon, button);
            if (i >= 192)
            {
                AssertButtonPress(device, state, button);
            }
            else
            {
                AssertButtonPress(device, state);
            }
        }
    }
}
