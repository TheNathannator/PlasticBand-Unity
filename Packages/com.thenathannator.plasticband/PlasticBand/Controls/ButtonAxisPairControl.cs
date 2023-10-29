using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

#if PLASTICBAND_DEBUG_CONTROLS
using UnityEngine;
#endif

namespace PlasticBand.Controls
{
    /// <summary>
    /// A button that is considered pressed when either a given button bit or a given axis are active.
    /// </summary>
    internal class ButtonAxisPairControl : ButtonControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<ButtonAxisPairControl>("ButtonAxisPair");
        }

        [InputControl(displayName = "Axis")]
        public AxisControl axis { get; set; }

        [InputControl(displayName = "Button")]
        public ButtonControl button { get; set; }

#if PLASTICBAND_DEBUG_CONTROLS
        float previousAxisValue;
        float previousButtonValue;
#endif

        protected override void FinishSetup()
        {
            base.FinishSetup();

            axis = GetChildControl<AxisControl>(nameof(axis));
            button = GetChildControl<ButtonControl>(nameof(button));
        }

        /// <summary>
        /// Reads the value of this control from a given state pointer.
        /// </summary>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            float axisValue = axis.ReadUnprocessedValueFromState(statePtr);
            float buttonValue = button.ReadUnprocessedValueFromState(statePtr);
            float finalValue = Math.Max(axisValue, buttonValue);

#if PLASTICBAND_DEBUG_CONTROLS
            if (axisValue != previousAxisValue || buttonValue != previousButtonValue)
            {
                Debug.Log($"[ButtonAxisPairControl] {nameof(axisValue)}: {axisValue}  {nameof(buttonValue)}: {buttonValue}  {nameof(finalValue)}: {finalValue}");
                previousAxisValue = axisValue;
                previousButtonValue = buttonValue;
            }
#endif

            return Preprocess(finalValue);
        }
    }
}
