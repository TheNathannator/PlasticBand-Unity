using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

#if PLASTICBAND_DEBUG_CONTROLS
using UnityEngine;
#endif

namespace PlasticBand.Controls
{
    /// <summary>
    /// An axis which reads from an integer and normalizes the value using the given minimum, maximum, and zero-point
    /// parameters, such that minimum = -1, maximum = 1, and zero-point = 0.
    /// </summary>
    internal class IntegerAxisControl : AxisControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<IntegerAxisControl>("IntAxis");
        }

        public int minValue = 0;
        public int maxValue = 0;
        public int zeroPoint = 0;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            var format = stateBlock.format;
            // For whatever reason, 'sbyte' is not checked for in IsIntegerFormat
            if (!format.IsIntegerFormat() && format != InputStateBlock.FormatSByte)
                throw new NotSupportedException($"Non-integer format '{format}' is not supported on {nameof(IntegerAxisControl)} '{this}'");

            if (maxValue == 0 && minValue == 0 && zeroPoint == 0)
                throw new NotSupportedException($"Range parameters have not been set on {nameof(IntegerAxisControl)} '{this}'! Please set {nameof(minValue)}, {nameof(maxValue)}, and {nameof(zeroPoint)}.");

            if (minValue > zeroPoint)
                throw new NotSupportedException($"Zero point ({zeroPoint}) is less than minimum value ({minValue}) on {nameof(IntegerAxisControl)} '{this}'!");

            if (maxValue < zeroPoint)
                throw new NotSupportedException($"Max value ({maxValue}) is less than zero point ({zeroPoint}) on {nameof(IntegerAxisControl)} '{this}'!");
        }

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int value = stateBlock.ReadInt(statePtr);
            float normalized = NormalizeUnchecked(value, minValue, maxValue, zeroPoint);
            return Preprocess(normalized);
        }

        internal static float NormalizeUnchecked(int value, int minValue, int maxValue, int zeroPoint)
        {
            float max;
            float min;
            float @base;
            if (value > zeroPoint)
            {
                max = maxValue;
                min = zeroPoint;
                @base = 0;
            }
            else if (value < zeroPoint)
            {
                max = zeroPoint;
                min = minValue;
                @base = -1;
            }
            else // intValue == zeroPoint
            {
                return 0f;
            }

            float percentage = (value - min) / (max - min);
            float normalized = @base + percentage;
            if (float.IsNaN(normalized))
                normalized = 0;

            return normalized;
        }
    }
}