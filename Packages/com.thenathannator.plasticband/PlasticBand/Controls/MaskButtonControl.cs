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
    /// A button that is considered pressed when all bits of a specified button mask are set.
    /// </summary>
    internal class MaskButtonControl : ButtonControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<MaskButtonControl>("MaskButton");
        }

        /// <summary>
        /// The mask to use for determining if this button is set.
        /// </summary>
        public int mask;

        public MaskButtonControl() : base()
        {
            m_StateBlock.format = InputStateBlock.FormatByte;
        }

#if PLASTICBAND_DEBUG_CONTROLS
        int previousValue;
#endif

        protected override void FinishSetup()
        {
            base.FinishSetup();

            if (mask == 0)
                throw new NotSupportedException($"MaskButtonControl '{this}' must have its '{nameof(mask)}' parameter set.");

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for MaskButtonControl '{this}'");
        }

        /// <summary>
        /// Reads the value of this control from a given state pointer.
        /// </summary>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int rawValue = stateBlock.ReadInt(statePtr);

            float value = 0f;
            if ((rawValue & mask) == mask)
                value = 1f;

#if PLASTICBAND_DEBUG_CONTROLS
            if (rawValue != previousValue)
            {
                Debug.Log($"[MaskButton] {nameof(rawValue)}: {rawValue:X}  {nameof(mask)}: {mask:X}  {nameof(value)}: {value}");
                previousValue = rawValue;
            }
#endif

            return Preprocess(value);
        }
    }
}
