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

        public bool hasNullValue; 
        public int nullValue;

        public new bool invert;

        private float m_InvertBase;

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

            m_InvertBase = invert && minValue == zeroPoint ? 1f : 0f;
        }

        private float m_LastValue;

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int value = stateBlock.ReadInt(statePtr);
            if (hasNullValue && value == nullValue)
                return m_LastValue;

            float normalized = Normalize(value, minValue, maxValue, zeroPoint);
            m_LastValue = Preprocess(normalized);
            return !invert ? m_LastValue : m_InvertBase - m_LastValue;
        }

        public static float Normalize(int value, int minValue, int maxValue, int zeroPoint)
        {
            if (value >= maxValue)
                return 1f;
            else if (value <= minValue)
                return minValue != zeroPoint ? -1f : 0f;

            int max;
            int min;
            float @base;
            if (value >= zeroPoint)
            {
                max = maxValue;
                min = zeroPoint;
                @base = 0f;
            }
            else
            {
                max = zeroPoint;
                min = minValue;
                @base = -1f;
            }

            float percentage;
            if (max == min) // Prevent divide-by-0
            {
                percentage = value >= max ? 1f : 0f;
            }
            else
            {
                percentage = (float) (value - min) / (max - min);
            }

            float normalized = @base + percentage;
            return normalized;
        }

        public static int Denormalize(float value, int minValue, int maxValue, int zeroPoint)
        {
            if (value >= 1f)
                return maxValue;
            else if (value <= -1f)
                return minValue;

            int max;
            int min;
            float @base;
            if (value >= 0f)
            {
                max = maxValue;
                min = zeroPoint;
                @base = 0f;
            }
            else
            {
                max = zeroPoint;
                min = minValue;
                @base = 1f;
            }

            int denormalized = min + (int)((max - min) * (@base + value));
            return denormalized;
        }
    }
}