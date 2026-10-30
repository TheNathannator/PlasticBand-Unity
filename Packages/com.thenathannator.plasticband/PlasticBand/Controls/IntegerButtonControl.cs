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
    /// A button which reads from an integer and normalizes the value using the given minimum and maximum parameters,
    /// such that minimum = 0 and maximum = 1.
    /// </summary>
    internal class IntegerButtonControl : ButtonControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<IntegerButtonControl>("IntButton");
        }

        public int minValue = 0;
        public int maxValue = 0;
        public int intPressPoint = int.MinValue;

        public bool hasNullValue; 
        public int nullValue;

        private float m_LastValue;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            // For whatever reason, 'sbyte' is not checked for in IsIntegerFormat
            if (!stateBlock.format.IsIntegerFormat() && stateBlock.format != InputStateBlock.FormatSByte)
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported on '{this}'");

            if (maxValue == 0 && minValue == 0)
                throw new NotSupportedException($"Range parameters have not been set on '{this}'! Please set {nameof(minValue)} and {nameof(maxValue)}.");

            int realMin = Math.Min(minValue, maxValue);
            int realMax = Math.Max(minValue, maxValue);

            if (pressPoint < 0 && intPressPoint > realMin && intPressPoint < realMax)
                pressPoint = IntegerAxisControl.Normalize(intPressPoint, minValue, maxValue, minValue);
        }

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int value = stateBlock.ReadInt(statePtr);
            if (hasNullValue && value == nullValue)
                return m_LastValue;

            float normalized = IntegerAxisControl.Normalize(value, minValue, maxValue, minValue);
            m_LastValue = Preprocess(normalized);
            return m_LastValue;
        }
    }
}