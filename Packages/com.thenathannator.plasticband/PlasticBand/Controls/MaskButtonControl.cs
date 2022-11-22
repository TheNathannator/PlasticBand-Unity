using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Controls
{
    /// <summary>
    /// A button that is considered pressed when all bits of a specified button mask are set.
    /// </summary>
    public class MaskButtonControl : ButtonControl
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

        protected override void FinishSetup()
        {
            base.FinishSetup();

            if (mask == 0)
                throw new NotSupportedException($"MaskButtonControl '{this}' must have its 'mask' parameter set.");

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for MaskButtonControl '{this}'");
        }

        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int rawValue = stateBlock.ReadInt(statePtr);

            float value = 0f;
            if ((rawValue & mask) == mask)
                value = 1f;

            return Preprocess(value);
        }

        public override unsafe void WriteValueIntoState(float value, void* statePtr)
        {
            // TODO
            throw new NotSupportedException($"Control '{this}' does not support writing");
        }
    }
}
