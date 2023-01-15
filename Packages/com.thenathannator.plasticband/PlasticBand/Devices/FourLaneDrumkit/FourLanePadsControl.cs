using System;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

#if PLASTICBAND_DEBUG_CONTROLS
using System.Text;
using UnityEngine;
#endif

// TODO: Velocity
// REVIEW/TODO: would using a stateful approach that tracks the previous buttons and
// only acts on what's different work better with the hardware issues?
// Issue with that is if someone wants to read from the previous state pointer...

namespace PlasticBand.Controls
{
    /// <summary>
    /// The pads and cymbals on a <see cref="FourLaneDrumkit"/>.
    /// </summary>
    public class FourLanePadsControl : InputControl<FourLanePadsControl.FourLanePad>
    {
        /// <summary>
        /// One of the pads or cymbals on a <see cref="FourLaneDrumkit"/>.
        /// </summary>
        [InputControlLayout(hideInUI = true)]
        public class FourLanePadControl : ButtonControl
        {
            /// <summary>
            /// Registers <see cref="FourLanePadControl"/> to the input system.
            /// </summary>
            internal static void Initialize()
            {
                InputSystem.RegisterLayout<FourLanePadControl>();
            }

            private FourLanePad m_PadToTest;
            private FourLanePadsControl m_Pads;

            /// <summary>
            /// Finishes setup of the control.
            /// </summary>
            protected override void FinishSetup()
            {
                base.FinishSetup();
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

                m_StateBlock = parent.stateBlock;
                m_Pads = (FourLanePadsControl)parent;
            }

            /// <summary>
            /// Reads the value of this control from a given state pointer.
            /// </summary>
            public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
            {
                var value = m_Pads.ReadUnprocessedValueFromState(statePtr);
                return (value & m_PadToTest) != 0 ? 1f : 0f;
            }
        }

        /// <summary>
        /// Registers <see cref="FourLanePadsControl"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            FourLanePadControl.Initialize();
            InputSystem.RegisterLayout<FourLanePadsControl>("FourLanePads");
        }

        /// <summary>
        /// Flags of active pads/cymbals.
        /// </summary>
        [Flags]
        public enum FourLanePad
        {
            None = 0,

            RedPad = 0x01,
            YellowPad = 0x02,
            BluePad = 0x04,
            GreenPad = 0x08,

            YellowCymbal = 0x20,
            BlueCymbal = 0x40,
            GreenCymbal = 0x80
        }

        /// <summary>
        /// The red pad of the drumkit.
        /// </summary>
        [InputControl(name = "redPad", format = "USHT", offset = 0, displayName = "Red Pad")]
        public FourLanePadControl redPad { get; private set; }

        /// <summary>
        /// The yellow pad of the drumkit.
        /// </summary>
        [InputControl(name = "yellowPad", format = "USHT", offset = 0, displayName = "Yellow Pad")]
        public FourLanePadControl yellowPad { get; private set; }

        /// <summary>
        /// The blue pad of the drumkit.
        /// </summary>
        [InputControl(name = "bluePad", format = "USHT", offset = 0, displayName = "Blue Pad")]
        public FourLanePadControl bluePad { get; private set; }

        /// <summary>
        /// The green pad of the drumkit.
        /// </summary>
        [InputControl(name = "greenPad", format = "USHT", offset = 0, displayName = "Green Pad")]
        public FourLanePadControl greenPad { get; private set; }

        /// <summary>
        /// The yellow cymbal of the drumkit.
        /// </summary>
        [InputControl(name = "yellowCymbal", format = "USHT", offset = 0, displayName = "Yellow Cymbal")]
        public FourLanePadControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue cymbal of the drumkit.
        /// </summary>
        [InputControl(name = "blueCymbal", format = "USHT", offset = 0, displayName = "Blue Cymbal")]
        public FourLanePadControl blueCymbal { get; private set; }

        /// <summary>
        /// The green cymbal of the drumkit.
        /// </summary>
        [InputControl(name = "greenCymbal", format = "USHT", offset = 0, displayName = "Green Cymbal")]
        public FourLanePadControl greenCymbal { get; private set; }

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

        /// <summary>
        /// The d-pad control of the parent device.
        /// </summary>
        private DpadControl m_Dpad;

        /// <summary>
        /// Finishes setup of the control.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            redPad = GetChildControl<FourLanePadControl>("redPad");
            yellowPad = GetChildControl<FourLanePadControl>("yellowPad");
            bluePad = GetChildControl<FourLanePadControl>("bluePad");
            greenPad = GetChildControl<FourLanePadControl>("greenPad");
            yellowCymbal = GetChildControl<FourLanePadControl>("yellowCymbal");
            blueCymbal = GetChildControl<FourLanePadControl>("blueCymbal");
            greenCymbal = GetChildControl<FourLanePadControl>("greenCymbal");

            // Retrieve d-pad from parent device
            // No checks done; if this fails it's a misconfiguration
            m_Dpad = parent.GetChildControl<DpadControl>("dpad");

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for FourLanePads '{this}'");

            if (stateBlock.sizeInBits < 8)
                throw new NotSupportedException($"FourLanePads '{this}' must be at least 8 bits in size.");

            if (redBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter 'redBit' ({redBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (yellowBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter 'yellowBit' ({yellowBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (blueBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter 'blueBit' ({blueBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (greenBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter 'greenBit' ({greenBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (padBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter 'padBit' ({padBit}) must be less than state block size ({stateBlock.sizeInBits}).");
            if (cymbalBit >= stateBlock.sizeInBits) throw new NotSupportedException($"FourLanePads parameter 'cymbalBit' ({cymbalBit}) must be less than state block size ({stateBlock.sizeInBits}).");

            // Turn bits into masks that can be used more efficiently
            redBit = 1 << redBit;
            yellowBit = 1 << yellowBit;
            blueBit = 1 << blueBit;
            greenBit = 1 << greenBit;
            padBit = 1 << padBit;
            cymbalBit = 1 << cymbalBit;
        }

#if PLASTICBAND_DEBUG_CONTROLS
        int previousButtons;
        FourLanePad previousPads;
        StringBuilder sb = new StringBuilder();
#endif

        /// <summary>
        /// Reads the value of this control from a given state pointer.
        /// </summary>
        public override unsafe FourLanePad ReadUnprocessedValueFromState(void* statePtr)
        {
            // A version of this with more detailed comments may be found here:
            // https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/General%20Notes.md

            // Read button bits
            int buttons = stateBlock.ReadInt(statePtr);
#if !PLASTICBAND_DEBUG_CONTROLS // Don't skip processing when debugging, keeps the logic in one place
                                // This is just a fast-path anyways
            if (buttons == 0)
            {
                return FourLanePad.None;
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
            if (buttons != previousButtons)
            {
                sb.Clear();
                sb.Append("[FourLanePads] Before: ");
                if (red) sb.Append("R ");
                if (yellow) sb.Append("Y ");
                if (blue) sb.Append("B ");
                if (green) sb.Append("G ");
                if (pad) sb.Append("P ");
                if (cymbal) sb.Append("C ");
                if (dpadUp) sb.Append("U ");
                if (dpadDown) sb.Append("D ");
            }
#endif

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
            // Rock Band 1 kits don't send the pad or cymbal flags, so we also check if cymbal is not set for compatibility with those
            if (pad || !cymbal)
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
            if (buttons != previousButtons)
            {
                sb.Append(" After: ");
                if (red) sb.Append("R ");
                if (yellow) sb.Append("Y ");
                if (blue) sb.Append("B ");
                if (green) sb.Append("G ");
                if (pad) sb.Append("P ");
                if (cymbal) sb.Append("C ");
                if (dpadUp) sb.Append("U ");
                if (dpadDown) sb.Append("D ");

                sb.AppendLine();
                sb.Append(pads);
                var changedPads = pads ^ previousPads;
                var newPads = changedPads & pads;
                var removedPads = changedPads & ~pads;
                sb.Append($"  New: {newPads}  Removed: {removedPads}  Changed: {changedPads}");
                Debug.Log(sb);
            }

            previousButtons = buttons;
            previousPads = pads;
#endif

            return pads;
        }
    }
}
