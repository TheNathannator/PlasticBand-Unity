using System;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

#if PLASTICBAND_DEBUG_CONTROLS
using System.Text;
using UnityEngine;
#endif

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/General%20Notes.md#deciphering-pads-and-cymbals

// TODO: Velocity

namespace PlasticBand.Controls
{
    /// <summary>
    /// One of the pads and cymbals on a <see cref="FourLaneDrumkit"/>.
    /// </summary>
    public class FourLanePadsControl : ButtonControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FourLanePadsControl>("FourLanePads");
        }

        /// <summary>
        /// The button bit to use as the red flag.
        /// </summary>
        /// <remarks>
        /// Before finishing setup and in control attributes, this is an index.
        /// After finishing setup, this is a mask of the bit.
        /// </remarks>
        public int redBit;

        /// <summary>
        /// The button bit to use as the yellow flag.
        /// </summary>
        /// <remarks>
        /// Before finishing setup and in control attributes, this is an index.
        /// After finishing setup, this is a mask of the bit.
        /// </remarks>
        public int yellowBit;

        /// <summary>
        /// The button bit to use as the blue flag.
        /// </summary>
        /// <remarks>
        /// Before finishing setup and in control attributes, this is an index.
        /// After finishing setup, this is a mask of the bit.
        /// </remarks>
        public int blueBit;

        /// <summary>
        /// The button bit to use as the green flag.
        /// </summary>
        /// <remarks>
        /// Before finishing setup and in control attributes, this is an index.
        /// After finishing setup, this is a mask of the bit.
        /// </remarks>
        public int greenBit;

        /// <summary>
        /// The button bit to use as the pad flag.
        /// </summary>
        /// <remarks>
        /// Before finishing setup and in control attributes, this is an index.
        /// After finishing setup, this is a mask of the bit.
        /// </remarks>
        public int padBit;

        /// <summary>
        /// The button bit to use as the cymbal flag.
        /// </summary>
        /// <remarks>
        /// Before finishing setup and in control attributes, this is an index.
        /// After finishing setup, this is a mask of the bit.
        /// </remarks>
        public int cymbalBit;

        private DpadControl m_Dpad;
        private FourLanePad m_PadToTest;
        private bool m_HasFlags;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            // Retrieve d-pad from parent device
            // No checks done; if this fails it's a misconfiguration
            m_Dpad = parent.GetChildControl<DpadControl>("dpad");

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for FourLanePads '{this}'");

            if (stateBlock.sizeInBits < 8)
                throw new NotSupportedException($"FourLanePads '{this}' must be at least 8 bits in size.");

            if (redBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter '{nameof(redBit)}' ({redBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (yellowBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter '{nameof(yellowBit)}' ({yellowBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (blueBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter '{nameof(blueBit)}' ({blueBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (greenBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter '{nameof(greenBit)}' ({greenBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (padBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter '{nameof(padBit)}' ({padBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (cymbalBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter '{nameof(cymbalBit)}' ({cymbalBit}) must be less than state block size ({stateBlock.sizeInBits}).");

            switch (name)
            {
                case "redPad": m_PadToTest = FourLanePad.RedPad; break;
                case "yellowPad": m_PadToTest = FourLanePad.YellowPad; break;
                case "bluePad": m_PadToTest = FourLanePad.BluePad; break;
                case "greenPad": m_PadToTest = FourLanePad.GreenPad; break;
                case "yellowCymbal": m_PadToTest = FourLanePad.YellowCymbal; break;
                case "blueCymbal": m_PadToTest = FourLanePad.BlueCymbal; break;
                case "greenCymbal": m_PadToTest = FourLanePad.GreenCymbal; break;
                default: throw new NotSupportedException($"Could not determine pad to test from name: {name}");
            };

            // Turn bits into masks that can be used more efficiently
            redBit = 1 << redBit;
            yellowBit = 1 << yellowBit;
            blueBit = 1 << blueBit;
            greenBit = 1 << greenBit;
            padBit = 1 << padBit;
            cymbalBit = 1 << cymbalBit;
        }

#if PLASTICBAND_DEBUG_CONTROLS
        int m_PreviousButtons;
        FourLanePad m_PreviousPads;
        StringBuilder m_MessageBuilder = new StringBuilder();
#endif

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            // Read button bits
            int buttons = stateBlock.ReadInt(statePtr);
#if !PLASTICBAND_DEBUG_CONTROLS // Don't skip processing when debugging, keeps the logic in one place
                                // This is just a fast-path anyways
            if (buttons == 0)
            {
                return 0f;
            }
#endif

            // Bitmask of individual pads/cymbals
            FourLanePad pads = FourLanePad.None;

            // Pre-calculate input flags
            bool red = (buttons & redBit) != 0;
            bool yellow = (buttons & yellowBit) != 0;
            bool blue = (buttons & blueBit) != 0;
            bool green = (buttons & greenBit) != 0;
            bool pad = (buttons & padBit) != 0;
            bool cymbal = (buttons & cymbalBit) != 0;
            bool dpadUp = m_Dpad.up.ReadUnprocessedValueFromState(statePtr) >= m_Dpad.up.pressPointOrDefault;
            bool dpadDown = m_Dpad.down.ReadUnprocessedValueFromState(statePtr) >= m_Dpad.down.pressPointOrDefault;

#if PLASTICBAND_DEBUG_CONTROLS
            if (buttons != m_PreviousButtons)
            {
                m_MessageBuilder.Clear();
                m_MessageBuilder.Append("[FourLanePads] Before: ");
                if (red) m_MessageBuilder.Append("R ");
                if (yellow) m_MessageBuilder.Append("Y ");
                if (blue) m_MessageBuilder.Append("B ");
                if (green) m_MessageBuilder.Append("G ");
                if (pad) m_MessageBuilder.Append("P ");
                if (cymbal) m_MessageBuilder.Append("C ");
                if (dpadUp) m_MessageBuilder.Append("U ");
                if (dpadDown) m_MessageBuilder.Append("D ");
            }
#endif

            if (pad || cymbal)
                m_HasFlags = true;

            // Pad + cymbal hits can be ambiguous, we need to resolve this
            if (pad && cymbal)
            {
                // There's only ambiguity between pad + cymbal hits of different colors, same-color pad + cymbal can be used directly
                int colorCount = 0;
                colorCount += red ? 1 : 0;
                colorCount += (yellow || dpadUp) ? 1 : 0;
                colorCount += (blue || dpadDown) ? 1 : 0;
                colorCount += (green || !(dpadUp || dpadDown)) ? 1 : 0;

                if (colorCount > 1)
                {
                    // The d-pad inputs let us resolve the ambiguity of a pad+cymbal hit
                    // Only d-pad is checked here since it is the only unique identifier

                    // Yellow
                    if (dpadUp)
                    {
                        pads |= FourLanePad.YellowCymbal;
                        yellow = false;
                        cymbal = false;
                    }

                    // Blue
                    if (dpadDown)
                    {
                        pads |= FourLanePad.BlueCymbal;
                        blue = false;
                        cymbal = false;
                    }

                    // Green
                    if (!(dpadUp || dpadDown))
                    {
                        pads |= FourLanePad.GreenCymbal;
                        green = false;
                        cymbal = false;
                    }
                }
            }

            // Now that disambiguation has been applied, we can process things normally

            // Check for pad hits
            // Rock Band 1 kits don't send the pad or cymbal flags, so we also check if
            // flags have not been detected and if the cymbal flag is not active
            if (pad || (!cymbal && !m_HasFlags))
            {
                if (red) pads |= FourLanePad.RedPad;
                if (yellow) pads |= FourLanePad.YellowPad;
                if (blue) pads |= FourLanePad.BluePad;
                if (green) pads |= FourLanePad.GreenPad;
            }

            // Check for cymbal hits
            if (cymbal)
            {
                if (yellow) pads |= FourLanePad.YellowCymbal;
                if (blue) pads |= FourLanePad.BlueCymbal;
                if (green) pads |= FourLanePad.GreenCymbal;
            }

#if PLASTICBAND_DEBUG_CONTROLS
            if (buttons != m_PreviousButtons)
            {
                m_MessageBuilder.Append(" After: ");
                if (red) m_MessageBuilder.Append("R ");
                if (yellow) m_MessageBuilder.Append("Y ");
                if (blue) m_MessageBuilder.Append("B ");
                if (green) m_MessageBuilder.Append("G ");
                if (pad) m_MessageBuilder.Append("P ");
                if (cymbal) m_MessageBuilder.Append("C ");
                if (dpadUp) m_MessageBuilder.Append("U ");
                if (dpadDown) m_MessageBuilder.Append("D ");

                m_MessageBuilder.AppendLine();
                m_MessageBuilder.Append(pads);
                var changedPads = pads ^ m_PreviousPads;
                var newPads = changedPads & pads;
                var removedPads = changedPads & ~pads;
                m_MessageBuilder.Append($"  New: {newPads}  Removed: {removedPads}  Changed: {changedPads}");
                Debug.Log(m_MessageBuilder);
            }

            m_PreviousButtons = buttons;
            m_PreviousPads = pads;
#endif

            return (pads & m_PadToTest) != 0 ? 1f : 0f;
        }
    }
}
