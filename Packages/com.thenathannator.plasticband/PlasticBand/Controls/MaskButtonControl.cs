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

        /// <summary>
        /// A mask of bits that should cause this button to not be set.
        /// </summary>
        public int excludeMask;

        /// <summary>
        /// Whether or not all bits must be set for this button to be set. Defaults to true.
        /// </summary>
        public bool matchAll = true;

        /// <summary>
        /// Whether or not all exclusion bits must be set for this button to not be set.
        /// </summary>
        public bool excludeMatchAll = true;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            if (mask == 0)
                throw new NotSupportedException($"MaskButtonControl '{this}' must have a mask set.");

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for MaskButtonControl '{this}'");
        }

        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            int rawValue = stateBlock.ReadInt(statePtr);

            float value = 0f;
            if (CheckMask(rawValue, mask, matchAll))
            {
                if (!CheckMask(rawValue, excludeMask, excludeMatchAll))
                {
                    value = 1f;
                }
            }

            return Preprocess(value);
        }

        private bool CheckMask(int rawValue, int mask, bool matchAll)
        {
            if (mask == 0)
                return false;

            if (matchAll)
            {
                if ((rawValue & mask) == mask)
                    return true;
            }
            else if ((rawValue & mask) != 0)
            {
                return true;
            }

            return false;
        }
    }
}
