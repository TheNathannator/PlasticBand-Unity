using System;
using System.Collections.Generic;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

#if PLASTICBAND_DEBUG_CONTROLS
using UnityEngine;
#endif

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/General%20Notes.md#touchslider-bar

namespace PlasticBand.Controls
{
    /// <summary>
    /// One of the segments on a <see cref="GuitarHeroGuitar"/>'s slider bar.
    /// </summary>
    internal class GuitarHeroSliderControl : ButtonControl
    {
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<GuitarHeroSliderControl>("GuitarHeroSlider");
        }

        // For conciseness
        private const FiveFret G = FiveFret.Green;
        private const FiveFret R = FiveFret.Red;
        private const FiveFret Y = FiveFret.Yellow;
        private const FiveFret B = FiveFret.Blue;
        private const FiveFret O = FiveFret.Orange;

        // Possible values for the World Tour slider bar
        internal static readonly Dictionary<byte, FiveFret> s_WTSliderLookup = new Dictionary<byte, FiveFret>()
        {
            { 0x7B, FiveFret.None },
            { 0x7C, FiveFret.None },
            { 0x7D, FiveFret.None },
            { 0x20, G },
            { 0x21, G },
            { 0x22, G },
            { 0x39, G | R },
            { 0x3A, G | R },
            { 0x3B, G | R },
            { 0x51,     R },
            { 0x52,     R },
            { 0x53,     R },
            { 0x67,     R | Y },
            { 0x68,     R | Y },
            { 0x69,     R | Y },
            { 0x6A,     R | Y },
            { 0x95,         Y },
            { 0x96,         Y },
            { 0x97,         Y },
            { 0xA7,         Y | B },
            { 0xA8,         Y | B },
            { 0xBD,             B },
            { 0xBE,             B },
            { 0xBF,             B },
            { 0xD4,             B | O },
            { 0xD5,             B | O },
            { 0xD6,             B | O },
            { 0xFB,                 O },
            { 0xFC,                 O }
        };

        // Possible values for the GH5 slider bar
        internal static readonly Dictionary<byte, FiveFret> s_GH5SliderLookup = new Dictionary<byte, FiveFret>()
        {
            // GH5 guitars
            { 0x00, FiveFret.None },
            { 0x95, G },
            { 0xB0, G | R },
            { 0xCD,     R },
            { 0xE5, G | R | Y },
            { 0xE6,     R | Y },
            { 0x19, G |     Y },
            { 0x1A,         Y },
            { 0x2C, G | R | Y | B },
            { 0x2D, G |     Y | B },
            { 0x2E,     R | Y | B },
            { 0x2F,         Y | B },
            { 0x46, G | R |     B },
            { 0x47, G |         B },
            { 0x48,     R |     B },
            { 0x49,             B },
            { 0x5F, G | R | Y | B | O },
            { 0x60, G | R |     B | O },
            { 0x61, G |     Y | B | O },
            { 0x62, G |         B | O },
            { 0x63,     R | Y | B | O },
            { 0x64,     R |     B | O },
            { 0x65,         Y | B | O },
            { 0x66,             B | O },
            { 0x78, G | R | Y |     O },
            { 0x79, G | R |         O },
            { 0x7A, G |     Y |     O },
            { 0x7B, G |             O },
            { 0x7C,     R | Y |     O },
            { 0x7D,     R |         O },
            { 0x7E,         Y |     O },
            { 0x7F,                 O }
        };

        private Dictionary<byte, FiveFret> m_SliderLookup;
        private FiveFret m_FretToTest;

        protected override void FinishSetup()
        {
            base.FinishSetup();

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for GuitarHeroSlider '{this}'");

            if (stateBlock.sizeInBits < 8)
                throw new NotSupportedException($"GuitarHeroSlider '{this}' must be at least 8 bits in size.");

            switch (name)
            {
                case "touchGreen": m_FretToTest = FiveFret.Green; break;
                case "touchRed": m_FretToTest = FiveFret.Red; break;
                case "touchYellow": m_FretToTest = FiveFret.Yellow; break;
                case "touchBlue": m_FretToTest = FiveFret.Blue; break;
                case "touchOrange": m_FretToTest = FiveFret.Orange; break;
                default: throw new NotSupportedException($"Could not determine fret to test from name: {name}");
            };
        }

#if PLASTICBAND_DEBUG_CONTROLS
        byte m_previousValue;
#endif

        /// <inheritdoc/>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            // Read only the bottom byte
            // In the only case where the value is larger than a byte, the bottom byte is duplicated to the top
            // (with some states this only applies when viewed as negative in hexadecimal,
            // but the lookup uses the non-signed version since that works fine)
            byte rawValue = unchecked((byte)stateBlock.ReadInt(statePtr));

            // Determine slider type if needed
            if (m_SliderLookup == null)
            {
                // Find the lookup where the current value is neutral
                // May be inaccurate in the case someone's noodling on the slider while waiting around during startup
                // or while connecting their guitar, but the cases where the WT is neutral and the GH5 is not are
                // pretty difficult to get on accident lol
                if (s_WTSliderLookup.TryGetValue(rawValue, out var flags2) && flags2 == FiveFret.None)
                    m_SliderLookup = s_WTSliderLookup;
                else if (s_GH5SliderLookup.TryGetValue(rawValue, out flags2) && flags2 == FiveFret.None)
                    m_SliderLookup = s_GH5SliderLookup;
                else
                    // No lookup is neutral, wait for the slider to be in a neutral state
                    return 0f;
            }

            if (!m_SliderLookup.TryGetValue(rawValue, out var flags))
            {
#if PLASTICBAND_DEBUG_CONTROLS
                if (rawValue != m_previousValue)
                {
                    Debug.LogWarning($"[GuitarHeroSlider] {nameof(rawValue)} {rawValue:X} is not defined in lookup!");
                    m_previousValue = rawValue;
                }
#endif

                return 0f;
            }

#if PLASTICBAND_DEBUG_CONTROLS
            if (rawValue != m_previousValue)
            {
                Debug.Log($"[GuitarHeroSlider] {nameof(rawValue)}: {rawValue:X}  flags: {flags}");
                m_previousValue = rawValue;
            }
#endif

            return (flags & m_FretToTest) != 0 ? 1f : 0f;
        }
    }
}