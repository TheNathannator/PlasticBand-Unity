using System;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

#if PLASTICBAND_DEBUG_CONTROLS
using UnityEngine;
#endif

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/General%20Notes.md#pickup-switch-xbox-360-ps3-wii

namespace PlasticBand.Controls
{
    /// <summary>
    /// The pickup switch on an Xbox 360/PS3/Wii <see cref="RockBandGuitar"/>.
    /// </summary>
    internal class RockBandPickupSwitchControl : IntegerControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<RockBandPickupSwitchControl>("RockBandPickupSwitch");
        }

        internal const int kNullValue = PS3DeviceState.StickCenter;
        internal const float kNotchSize = (byte.MaxValue + 1) / 5f;

        public bool hasNullValue;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            if (stateBlock.format != InputStateBlock.FormatByte)
                throw new NotSupportedException($"Format '{stateBlock.format}' is not supported for RockBandPickupSwitch '{this}', must be '{InputStateBlock.FormatByte}'");

            if (stateBlock.sizeInBits < 8)
                throw new NotSupportedException($"RockBandPickupSwitch '{this}' must be at least 8 bits in size.");
        }

        // As much as I don't like this, it's unfortunately necessary
        private int m_PreviousNotch = 0;

        /// <inheritdoc/>
        public override unsafe int ReadUnprocessedValueFromState(void* statePtr)
        {
            int rawValue = stateBlock.ReadInt(statePtr);
            // PS3/Wii RB guitars will report back 0x7F after the switch has been resting for a moment,
            // so we need to ignore that
            if (hasNullValue && rawValue == kNullValue)
                return m_PreviousNotch;

            // Determine which notch is currently selected
            // See the reference doc linked above for more details
            int notch = (int)(rawValue / kNotchSize);
            m_PreviousNotch = notch;

#if PLASTICBAND_DEBUG_CONTROLS
            if (rawValue != m_PreviousNotch)
            {
                Debug.Log($"[RockBandPickupSwitch] {nameof(rawValue)}: {rawValue:X}, {nameof(notch)}: {notch}");
                m_PreviousNotch = rawValue;
            }
#endif

            return notch;
        }
    }
}