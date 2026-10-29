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

        [InputControl(layout = "IntAxis", defaultState = 128, parameters = "minValue=0,maxValue=255,zeroPoint=128")]
        public byte byteAxis;

        [InputControl(layout = "IntAxis", defaultState = 0, parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
        public sbyte sbyteAxis;

        [InputControl(layout = "IntAxis", defaultState = 128, parameters = "minValue=255,maxValue=0,zeroPoint=128")]
        public byte invertedByteAxis;

        [InputControl(layout = "IntAxis", defaultState = 0, parameters = "minValue=127,maxValue=-128,zeroPoint=0")]
        public sbyte invertedSbyteAxis;

        [InputControl(layout = "IntAxis", defaultState = 128, parameters = "minValue=0,maxValue=255,zeroPoint=128,hasNullValue,nullValue=192")]
        public byte nullByteAxis;

        [InputControl(layout = "IntAxis", defaultState = 0, parameters = "minValue=-128,maxValue=127,zeroPoint=0,hasNullValue,nullValue=64")]
        public sbyte nullSbyteAxis;

        public static IntegerAxisState Default()
        {
            return new IntegerAxisState()
            {
                byteAxis = 128,
                sbyteAxis = 0,
                invertedByteAxis = 128,
                invertedSbyteAxis = 0,
                nullByteAxis = 128,
                nullSbyteAxis = 0,
            };
        }
    }

    [InputControlLayout(stateType = typeof(IntegerAxisState), hideInUI = true)]
    internal class IntegerAxisDevice : InputDevice
    {
        public IntegerAxisControl byteAxis { get; private set; }
        public IntegerAxisControl sbyteAxis { get; private set; }
        public IntegerAxisControl invertedByteAxis { get; private set; }
        public IntegerAxisControl invertedSbyteAxis { get; private set; }
        public IntegerAxisControl nullByteAxis { get; private set; }
        public IntegerAxisControl nullSbyteAxis { get; private set; }

        public IntegerAxisControl[] allAxes { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            byteAxis = GetChildControl<IntegerAxisControl>(nameof(byteAxis));
            sbyteAxis = GetChildControl<IntegerAxisControl>(nameof(sbyteAxis));
            invertedByteAxis = GetChildControl<IntegerAxisControl>(nameof(invertedByteAxis));
            invertedSbyteAxis = GetChildControl<IntegerAxisControl>(nameof(invertedSbyteAxis));
            nullByteAxis = GetChildControl<IntegerAxisControl>(nameof(nullByteAxis));
            nullSbyteAxis = GetChildControl<IntegerAxisControl>(nameof(nullSbyteAxis));

            allAxes = new IntegerAxisControl[]
            {
                byteAxis,
                sbyteAxis,
                invertedByteAxis,
                invertedSbyteAxis,
                nullByteAxis,
                nullSbyteAxis,
            };
        }
    }

    internal class IntegerAxisControlTests : PlasticBandTestFixture<IntegerAxisDevice>
    {
        private const float kEpsilon = 1f / 255f;

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
            Assert.That(device.byteAxis.minValue,          Is.EqualTo(0), "byteAxis has an incorrect minimum value!");
            Assert.That(device.sbyteAxis.minValue,         Is.EqualTo(-128), "sbyteAxis has an incorrect minimum value!");
            Assert.That(device.invertedByteAxis.minValue,  Is.EqualTo(255), "invertedByteAxis has an incorrect minimum value!");
            Assert.That(device.invertedSbyteAxis.minValue, Is.EqualTo(127), "invertedSbyteAxis has an incorrect minimum value!");
            Assert.That(device.nullByteAxis.minValue,      Is.EqualTo(0), "nullByteAxis has an incorrect minimum value!");
            Assert.That(device.nullSbyteAxis.minValue,     Is.EqualTo(-128), "nullSbyteAxis has an incorrect minimum value!");
        });

        [Test]
        public void SetsMaxValue() => CreateAndRun((device) =>
        {
            Assert.That(device.byteAxis.maxValue,          Is.EqualTo(255), "byteAxis has an incorrect maximum value!");
            Assert.That(device.sbyteAxis.maxValue,         Is.EqualTo(127), "sbyteAxis has an incorrect maximum value!");
            Assert.That(device.invertedByteAxis.maxValue,  Is.EqualTo(0), "invertedByteAxis has an incorrect maximum value!");
            Assert.That(device.invertedSbyteAxis.maxValue, Is.EqualTo(-128), "invertedSbyteAxis has an incorrect maximum value!");
            Assert.That(device.nullByteAxis.maxValue,      Is.EqualTo(255), "nullByteAxis has an incorrect maximum value!");
            Assert.That(device.nullSbyteAxis.maxValue,     Is.EqualTo(127), "nullSbyteAxis has an incorrect maximum value!");
        });

        [Test]
        public void SetsZeroPoint() => CreateAndRun((device) =>
        {
            Assert.That(device.byteAxis.zeroPoint,          Is.EqualTo(128), "byteAxis has an incorrect zero point!");
            Assert.That(device.sbyteAxis.zeroPoint,         Is.EqualTo(0), "sbyteAxis has an incorrect zero point!");
            Assert.That(device.invertedByteAxis.zeroPoint,  Is.EqualTo(128), "invertedByteAxis has an incorrect zero point!");
            Assert.That(device.invertedSbyteAxis.zeroPoint, Is.EqualTo(0), "invertedSbyteAxis has an incorrect zero point!");
            Assert.That(device.nullByteAxis.zeroPoint,      Is.EqualTo(128), "nullByteAxis has an incorrect zero point!");
            Assert.That(device.nullSbyteAxis.zeroPoint,     Is.EqualTo(0), "nullSbyteAxis has an incorrect zero point!");
        });

        [Test]
        public void SetsHasNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.byteAxis.hasNullValue,          Is.EqualTo(false), "byteAxis reported the wrong null value presence!");
            Assert.That(device.sbyteAxis.hasNullValue,         Is.EqualTo(false), "sbyteAxis reported the wrong null value presence!");
            Assert.That(device.invertedByteAxis.hasNullValue,  Is.EqualTo(false), "invertedByteAxis reported the wrong null value presence!");
            Assert.That(device.invertedSbyteAxis.hasNullValue, Is.EqualTo(false), "invertedSbyteAxis reported the wrong null value presence!");
            Assert.That(device.nullByteAxis.hasNullValue,      Is.EqualTo(true), "nullByteAxis reported the wrong null value presence!");
            Assert.That(device.nullSbyteAxis.hasNullValue,     Is.EqualTo(true), "nullSbyteAxis reported the wrong null value presence!");
        });

        [Test]
        public void SetsNullValue() => CreateAndRun((device) =>
        {
            Assert.That(device.byteAxis.nullValue,          Is.EqualTo(0), "byteAxis has an incorrect null value!");
            Assert.That(device.sbyteAxis.nullValue,         Is.EqualTo(0), "sbyteAxis has an incorrect null value!");
            Assert.That(device.invertedByteAxis.nullValue,  Is.EqualTo(0), "invertedByteAxis has an incorrect null value!");
            Assert.That(device.invertedSbyteAxis.nullValue, Is.EqualTo(0), "invertedSbyteAxis has an incorrect null value!");
            Assert.That(device.nullByteAxis.nullValue,      Is.EqualTo(192), "nullByteAxis has an incorrect null value!");
            Assert.That(device.nullSbyteAxis.nullValue,     Is.EqualTo(64), "nullSbyteAxis has an incorrect null value!");
        });

        [Test]
        public void HandlesMinValue() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState()
            {
                byteAxis = 0,
                sbyteAxis = -128,
                invertedByteAxis = 255,
                invertedSbyteAxis = 127,
                nullByteAxis = 0,
                nullSbyteAxis = -128,
            };
            AssertAxisValue(device, state, -1f, 0f, device.allAxes);
        });

        [Test]
        public void HandlesMaxValue() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState()
            {
                byteAxis = 255,
                sbyteAxis = 127,
                invertedByteAxis = 0,
                invertedSbyteAxis = -128,
                nullByteAxis = 255,
                nullSbyteAxis = 127,
            };
            AssertAxisValue(device, state, 1f, 0f, device.allAxes);
        });

        [Test]
        public void HandlesZeroPoint() => CreateAndRun((device) =>
        {
            var state = new IntegerAxisState()
            {
                byteAxis = 128,
                sbyteAxis = 0,
                invertedByteAxis = 128,
                invertedSbyteAxis = 0,
                nullByteAxis = 128,
                nullSbyteAxis = 0,
            };
            AssertAxisValue(device, state, 0f, 0f, device.allAxes);
        });

        [Test]
        public void HandlesNullValue() => CreateAndRun((device) =>
        {
            var state = IntegerAxisState.Default();
            AssertAxisValue(device, state, 0f, 0f, device.allAxes);

            for (int i = -128; i <= 127; i++)
            {
                if (i == 64)
                {
                    continue;
                }

                float expected = i / (i < 0 ? 128f : 127f);
                state.nullByteAxis = (byte)(128 + i);
                state.nullSbyteAxis = (sbyte)i;
                AssertAxisValue(device, state, expected, kEpsilon, device.nullByteAxis, device.nullSbyteAxis);

                // No change in value should occur at the null state
                state.nullByteAxis = 192;
                state.nullSbyteAxis = 64;
                AssertAxisValue(device, state, expected, kEpsilon, device.nullByteAxis, device.nullSbyteAxis);
            }
        });

        [Test]
        public void HandlesRange() => CreateAndRun((device) =>
        {
            var state = IntegerAxisState.Default();
            AssertAxisValue(device, state, 0f, 0f, device.allAxes);

            // byteAxis
            for (int i = -128; i <= 127; i++)
            {
                float expected = i / (i < 0 ? 128f : 127f);
                state.byteAxis = (byte)(128 + i);

                AssertAxisValue(device, state, expected, kEpsilon, device.byteAxis);
            }

            // sbyteAxis
            state = IntegerAxisState.Default();
            for (int i = -128; i <= 127; i++)
            {
                float expected = i / (i < 0 ? 128f : 127f);
                state.sbyteAxis = (sbyte)i;

                AssertAxisValue(device, state, expected, kEpsilon, device.sbyteAxis);
            }

            // invertedByteAxis
            state = IntegerAxisState.Default();
            for (int i = -127; i <= 128; i++)
            {
                float expected = i / (i < 0 ? 127f : 128f);
                state.invertedByteAxis = (byte)(128 - i);

                AssertAxisValue(device, state, expected, kEpsilon, device.invertedByteAxis);
            }

            // invertedSbyteAxis
            state = IntegerAxisState.Default();
            for (int i = -127; i <= 128; i++)
            {
                float expected = i / (i < 0 ? 127f : 128f);
                state.invertedSbyteAxis = (sbyte)-i;

                AssertAxisValue(device, state, expected, kEpsilon, device.invertedSbyteAxis);
            }

            // nullByteAxis
            state = IntegerAxisState.Default();
            float lastExpected = float.NegativeInfinity;
            for (int i = -128; i <= 127; i++)
            {
                float expected = i == 64 // Null value
                    ? lastExpected
                    : i / (i < 0 ? 128f : 127f);
                state.nullByteAxis = (byte)(128 + i);

                AssertAxisValue(device, state, expected, kEpsilon, device.nullByteAxis);

                lastExpected = expected;
            }

            // nullSbyteAxis
            state = IntegerAxisState.Default();
            lastExpected = float.NegativeInfinity;
            for (int i = -128; i <= 127; i++)
            {
                float expected = i == 64 // Null value
                    ? lastExpected
                    : i / (i < 0 ? 128f : 127f);
                state.nullSbyteAxis = (sbyte)i;

                AssertAxisValue(device, state, expected, kEpsilon, device.nullSbyteAxis);

                lastExpected = expected;
            }
        });

        [Test]
        public void RoundTrips()
        {
            const int stepSize = 50;
            const float epsilon = 1f / stepSize;

            void TestAxisByte(byte min, byte max, byte zero)
            {
                for (int i = -stepSize; i < stepSize; i++)
                {
                    float normalized = i / (float) stepSize;

                    byte denormalized = (byte) IntegerAxisControl.Denormalize(normalized, min, max, zero);
                    float renormalized = IntegerAxisControl.Normalize(denormalized, min, max, zero);

                    Assert.That(renormalized, Is.InRange(normalized - epsilon, normalized + epsilon));
                }
            }

            void TestAxisSbyte(sbyte min, sbyte max, sbyte zero)
            {
                for (int i = -stepSize; i < stepSize; i++)
                {
                    float normalized = i / (float) stepSize;

                    sbyte denormalized = (sbyte) IntegerAxisControl.Denormalize(normalized, min, max, zero);
                    float renormalized = IntegerAxisControl.Normalize(denormalized, min, max, zero);

                    Assert.That(renormalized, Is.InRange(normalized - epsilon, normalized + epsilon));
                }
            }

            TestAxisByte(0, 255, 128); // byteAxis
            TestAxisSbyte(-128, 127, 0); // sbyteAxis
            TestAxisByte(255, 0, 128); // invertedByteAxis
            TestAxisSbyte(127, -128, 0); // invertedSbyteAxis
            TestAxisByte(0, 255, 128); // nullByteAxis
            TestAxisSbyte(-128, 127, 0); // nullSbyteAxis
        }
    }
}
