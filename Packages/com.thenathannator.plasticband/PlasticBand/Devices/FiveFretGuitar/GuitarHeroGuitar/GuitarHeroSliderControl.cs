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
    public class GuitarHeroSliderControl : ButtonControl
    {
        /// <summary>
        /// Registers <see cref="GuitarHeroSliderControl"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<GuitarHeroSliderControl>("GuitarHeroSlider");
        }

        /// <summary>
        /// Lookup for possible values for the slider bar.
        /// </summary>
        private static readonly Dictionary<byte, FiveFret> s_SliderLookup = new Dictionary<byte, FiveFret>()
        {
            // TODO: This might not support World Tour guitars yet.
            // If the values the Wii World Tour guitar reports (https://wiibrew.org/wiki/Wiimote/Extension_Controllers/Guitar_Hero_(Wii)_Guitars)
            // are the same as on Xbox 360 and PS3, there'll need to be additional logic to determine which guitars are World Tour and which are GH5.

            // GH5 guitars
            { 0x00, FiveFret.None },
            { 0x95, FiveFret.Green },
            { 0xB0, FiveFret.Green | FiveFret.Red },
            { 0xCD,                  FiveFret.Red },
            { 0xE5, FiveFret.Green | FiveFret.Red | FiveFret.Yellow },
            { 0xE6,                  FiveFret.Red | FiveFret.Yellow },
            { 0x19, FiveFret.Green |                FiveFret.Yellow },
            { 0x1A,                                 FiveFret.Yellow },
            { 0x2C, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue },
            { 0x2D, FiveFret.Green |                FiveFret.Yellow | FiveFret.Blue },
            { 0x2E,                  FiveFret.Red | FiveFret.Yellow | FiveFret.Blue },
            { 0x2F,                                 FiveFret.Yellow | FiveFret.Blue },
            { 0x46, FiveFret.Green | FiveFret.Red |                   FiveFret.Blue },
            { 0x47, FiveFret.Green |                                  FiveFret.Blue },
            { 0x48,                  FiveFret.Red |                   FiveFret.Blue },
            { 0x49,                                                   FiveFret.Blue },
            { 0x5F, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange },
            { 0x60, FiveFret.Green | FiveFret.Red |                   FiveFret.Blue | FiveFret.Orange },
            { 0x61, FiveFret.Green |                FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange },
            { 0x62, FiveFret.Green |                                  FiveFret.Blue | FiveFret.Orange },
            { 0x63,                  FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange },
            { 0x64,                  FiveFret.Red |                   FiveFret.Blue | FiveFret.Orange },
            { 0x65,                                 FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange },
            { 0x66,                                                   FiveFret.Blue | FiveFret.Orange },
            { 0x78, FiveFret.Green | FiveFret.Red | FiveFret.Yellow |                 FiveFret.Orange },
            { 0x79, FiveFret.Green | FiveFret.Red |                                   FiveFret.Orange },
            { 0x7A, FiveFret.Green |                FiveFret.Yellow |                 FiveFret.Orange },
            { 0x7B, FiveFret.Green |                                                  FiveFret.Orange },
            { 0x7C,                  FiveFret.Red | FiveFret.Yellow |                 FiveFret.Orange },
            { 0x7D,                  FiveFret.Red |                                   FiveFret.Orange },
            { 0x7E,                                 FiveFret.Yellow |                 FiveFret.Orange },
            { 0x7F,                                                                   FiveFret.Orange }
        };

        /// <summary>
        /// The pad flag to test to determine state.
        /// </summary>
        private FiveFret m_FretToTest;

        /// <summary>
        /// Finishes setup of the control.
        /// </summary>
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

        /// <summary>
        /// Reads the value of this control from a given state pointer.
        /// </summary>
        public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
        {
            // Read only the bottom byte
            // In the only case where the value is larger than a byte, the bottom byte is duplicated to the top
            // (with some states this only applies when viewed as negative in hexadecimal,
            // but the lookup uses the non-signed version since that works fine)
            byte rawValue = unchecked((byte)stateBlock.ReadInt(statePtr));
            if (!s_SliderLookup.TryGetValue(rawValue, out var flags))
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