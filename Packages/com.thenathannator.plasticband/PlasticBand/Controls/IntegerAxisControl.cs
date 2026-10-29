using System;
using UnityEngine;
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

        private float m_LastValue;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            // For whatever reason, 'sbyte' is not checked for in IsIntegerFormat
            if (!stateBlock.format.IsIntegerFormat() && stateBlock.format != InputStateBlock.FormatSByte)
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported on '{this}'");

            if (maxValue == 0 && minValue == 0 && zeroPoint == 0)
                throw new NotSupportedException($"Range parameters have not been set on '{this}'! Please set {nameof(minValue)}, {nameof(maxValue)}, and {nameof(zeroPoint)}.");

            int realMin = Math.Min(minValue, maxValue);
            int realMax = Math.Max(minValue, maxValue);

            if (zeroPoint < realMin || zeroPoint > realMax)
                throw new NotSupportedException($"Zero point ({zeroPoint}) is out of range on '{this}'! Must be between min and max values [{realMin} - {realMax}]!");
        }

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int value = stateBlock.ReadInt(statePtr);
            if (hasNullValue && value == nullValue)
            {
                return m_LastValue;
            }

            float normalized = Normalize(value, minValue, maxValue, zeroPoint);
            m_LastValue = Preprocess(normalized);
            return m_LastValue;
        }

        public static float Normalize(int value, int minValue, int maxValue, int zeroPoint)
        {
            if (maxValue >= minValue)
            {
                return DoNormalize(value, minValue, maxValue, zeroPoint);
            }
            else
            {
                return -DoNormalize(value, maxValue, minValue, zeroPoint);
            }
        }

        private static float DoNormalize(int value, int minValue, int maxValue, int zeroPoint)
        {
            // Reject invalid bounds
            if (zeroPoint < minValue || zeroPoint > maxValue)
            {
                return 0f;
            }

            // Clamp to min/max
            if (value >= maxValue)
            {
                return maxValue == zeroPoint ? 0f : 1f;
            }
            else if (value <= minValue)
            {
                return minValue == zeroPoint ? 0f : -1f;
            }

            // Split the actual range into positive and negative portions,
            // which are interpolated separately
            if (value >= zeroPoint)
            {
                return Mathf.InverseLerp(zeroPoint, maxValue, value);
            }
            else
            {
                return -1f + Mathf.InverseLerp(minValue, zeroPoint, value);
            }
        }

        public static int Denormalize(float value, int minValue, int maxValue, int zeroPoint)
        {
            if (maxValue >= minValue)
            {
                return DoDenormalize(value, minValue, maxValue, zeroPoint);
            }
            else
            {
                return -DoDenormalize(value, maxValue, minValue, zeroPoint);
            }
        }

        private static int DoDenormalize(float value, int minValue, int maxValue, int zeroPoint)
        {
            // Reject invalid bounds
            if (zeroPoint < minValue || zeroPoint > maxValue || minValue == maxValue)
            {
                return 0;
            }

            // Clamp to min/max
            if (value >= 1f)
            {
                return maxValue;
            }
            else if (value <= -1f)
            {
                return minValue;
            }

            // Split the actual range into positive and negative portions,
            // which are interpolated separately
            if (value >= 0f)
            {
                return IntLerp(zeroPoint, maxValue, value);
            }
            else
            {
                return IntLerp(minValue, zeroPoint, 1 + value);
            }
        }

        private static int IntLerp(int min, int max, float percentage)
        {
            return (int)(min + (max - min) * percentage);
        }

        public static int Convert(
            int value,
            int fromMin, int fromMax, int fromZero,
            int toMin, int toMax, int toZero
        )
        {
            float normalized = Normalize(value, fromMin, fromMax, fromZero);
            int converted = Denormalize(normalized, toMin, toMax, toZero);
            return converted;
        }
    }
}