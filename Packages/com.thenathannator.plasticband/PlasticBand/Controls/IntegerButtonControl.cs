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

        public int intPressPoint = 0;
        public int minValue = 0;
        public int maxValue = 0;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported on {nameof(IntegerButtonControl)} '{this}'");

            if (pressPoint < 0 && intPressPoint > 0)
                pressPoint = IntegerAxisControl.NormalizeUnchecked(intPressPoint, minValue, maxValue, minValue);

            if (maxValue == 0 && minValue == 0)
                throw new NotSupportedException($"Range parameters have not been set on {nameof(IntegerButtonControl)} '{this}'! Please set {nameof(minValue)} and {nameof(maxValue)}.");

            if (maxValue <= minValue)
                throw new NotSupportedException($"Maximum value ({maxValue}) must be greater than minimum value ({minValue}) on {nameof(IntegerButtonControl)} '{this}'!");
        }

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int value = stateBlock.ReadInt(statePtr);
            float normalized = IntegerAxisControl.NormalizeUnchecked(value, minValue, maxValue, minValue);
            return Preprocess(normalized);
        }
    }
}