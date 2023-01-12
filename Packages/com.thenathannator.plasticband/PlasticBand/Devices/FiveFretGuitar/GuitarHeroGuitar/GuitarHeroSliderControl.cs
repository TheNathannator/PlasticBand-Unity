using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

#if PLASTICBAND_DEBUG_CONTROLS
using UnityEngine;
#endif

namespace PlasticBand.Controls
{
    public class GuitarHeroSliderControl : InputControl<GuitarHeroSliderControl.SliderFret>
    {
        [InputControlLayout(hideInUI = true)]
        public class GuitarHeroSliderSegmentControl : ButtonControl
        {
            internal static void Initialize()
            {
                InputSystem.RegisterLayout<GuitarHeroSliderSegmentControl>();
            }

            private SliderFret m_FretToTest;
            private GuitarHeroSliderControl m_Slider;

            protected override void FinishSetup()
            {
                base.FinishSetup();
                m_FretToTest = name switch
                {
                    "touchGreen" => SliderFret.Green,
                    "touchRed" => SliderFret.Red,
                    "touchYellow" => SliderFret.Yellow,
                    "touchBlue" => SliderFret.Blue,
                    "touchOrange" => SliderFret.Orange,
                    _ => throw new NotSupportedException($"Could not determine fret to test from name: {name}")
                };

                m_StateBlock = parent.stateBlock;
                m_Slider = (GuitarHeroSliderControl)parent;
            }

            public override unsafe float ReadUnprocessedValueFromState(void* statePtr)
            {
                var value = m_Slider.ReadUnprocessedValueFromState(statePtr);
                return (value & m_FretToTest) != 0 ? 1f : 0f;
            }
        }

        internal static void Initialize()
        {
            GuitarHeroSliderSegmentControl.Initialize();
            InputSystem.RegisterLayout<GuitarHeroSliderControl>("GuitarHeroSlider");
        }

        [Flags]
        public enum SliderFret : byte
        {
            None = 0x00,
            Green = 0x01,
            Red = 0x02,
            Yellow = 0x04,
            Blue = 0x08,
            Orange = 0x10
        }

        // Lookup for possible values for the slider bar, ordered by frets
        // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/gh5_neck.html
        private static readonly Dictionary<byte, SliderFret> s_SliderLookup = new Dictionary<byte, SliderFret>()
        {
            // TODO: This might not support World Tour guitars yet.
            // If the values the Wii World Tour guitar reports (https://wiibrew.org/wiki/Wiimote/Extension_Controllers/Guitar_Hero_(Wii)_Guitars)
            // are the same as on Xbox 360 and PS3, there'll need to be additional logic to determine which guitars are World Tour and which are GH5.

            // GH5 guitars
            { 0x00, SliderFret.None },
            { 0x95, SliderFret.Green },
            { 0xB0, SliderFret.Green | SliderFret.Red },
            { 0xCD,                    SliderFret.Red },
            { 0xE5, SliderFret.Green | SliderFret.Red | SliderFret.Yellow },
            { 0xE6,                    SliderFret.Red | SliderFret.Yellow },
            { 0x19, SliderFret.Green |                  SliderFret.Yellow },
            { 0x1A,                                     SliderFret.Yellow },
            { 0x2C, SliderFret.Green | SliderFret.Red | SliderFret.Yellow | SliderFret.Blue },
            { 0x2D, SliderFret.Green |                  SliderFret.Yellow | SliderFret.Blue },
            { 0x2E,                    SliderFret.Red | SliderFret.Yellow | SliderFret.Blue },
            { 0x2F,                                     SliderFret.Yellow | SliderFret.Blue },
            { 0x46, SliderFret.Green | SliderFret.Red |                     SliderFret.Blue },
            { 0x47, SliderFret.Green |                                      SliderFret.Blue },
            { 0x48,                    SliderFret.Red |                     SliderFret.Blue },
            { 0x49,                                                         SliderFret.Blue },
            { 0x5F, SliderFret.Green | SliderFret.Red | SliderFret.Yellow | SliderFret.Blue | SliderFret.Orange },
            { 0x60, SliderFret.Green | SliderFret.Red |                     SliderFret.Blue | SliderFret.Orange },
            { 0x61, SliderFret.Green |                  SliderFret.Yellow | SliderFret.Blue | SliderFret.Orange },
            { 0x62, SliderFret.Green |                                      SliderFret.Blue | SliderFret.Orange },
            { 0x63,                    SliderFret.Red | SliderFret.Yellow | SliderFret.Blue | SliderFret.Orange },
            { 0x64,                    SliderFret.Red |                     SliderFret.Blue | SliderFret.Orange },
            { 0x65,                                     SliderFret.Yellow | SliderFret.Blue | SliderFret.Orange },
            { 0x66,                                                         SliderFret.Blue | SliderFret.Orange },
            { 0x78, SliderFret.Green | SliderFret.Red | SliderFret.Yellow |                   SliderFret.Orange },
            { 0x79, SliderFret.Green | SliderFret.Red |                                       SliderFret.Orange },
            { 0x7A, SliderFret.Green |                  SliderFret.Yellow |                   SliderFret.Orange },
            { 0x7B, SliderFret.Green |                                                        SliderFret.Orange },
            { 0x7C,                    SliderFret.Red | SliderFret.Yellow |                   SliderFret.Orange },
            { 0x7D,                    SliderFret.Red |                                       SliderFret.Orange },
            { 0x7E,                                     SliderFret.Yellow |                   SliderFret.Orange },
            { 0x7F,                                                                           SliderFret.Orange }
        };

        /// <summary>
        /// The green segment of the touch/slider bar.
        /// </summary>
        [InputControl(name = "touchGreen", format = "BYTE", offset = 0, displayName = "Green")]
        public GuitarHeroSliderSegmentControl green { get; private set; }

        /// <summary>
        /// The red segment of the touch/slider bar.
        /// </summary>
        [InputControl(name = "touchRed", format = "BYTE", offset = 0, displayName = "Red")]
        public GuitarHeroSliderSegmentControl red { get; private set; }

        /// <summary>
        /// The yellow segment of the touch/slider bar.
        /// </summary>
        [InputControl(name = "touchYellow", format = "BYTE", offset = 0, displayName = "Yellow")]
        public GuitarHeroSliderSegmentControl yellow { get; private set; }

        /// <summary>
        /// The blue segment of the touch/slider bar.
        /// </summary>
        [InputControl(name = "touchBlue", format = "BYTE", offset = 0, displayName = "Blue")]
        public GuitarHeroSliderSegmentControl blue { get; private set; }

        /// <summary>
        /// The orange segment of the touch/slider bar.
        /// </summary>
        [InputControl(name = "touchOrange", format = "BYTE", offset = 0, displayName = "Orange")]
        public GuitarHeroSliderSegmentControl orange { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            green = GetChildControl<GuitarHeroSliderSegmentControl>("touchGreen");
            red = GetChildControl<GuitarHeroSliderSegmentControl>("touchRed");
            yellow = GetChildControl<GuitarHeroSliderSegmentControl>("touchYellow");
            blue = GetChildControl<GuitarHeroSliderSegmentControl>("touchBlue");
            orange = GetChildControl<GuitarHeroSliderSegmentControl>("touchOrange");

            if (!stateBlock.format.IsIntegerFormat())
                throw new NotSupportedException($"Non-integer format '{stateBlock.format}' is not supported for GuitarHeroSlider '{this}'");

            if (stateBlock.sizeInBits < 8)
                throw new NotSupportedException($"GuitarHeroSlider '{this}' must be at least 8 bits in size.");
        }

#if PLASTICBAND_DEBUG_CONTROLS
        byte previousValue;
#endif

        public override unsafe SliderFret ReadUnprocessedValueFromState(void* statePtr)
        {
            // Read only the bottom byte
            // In the only case where the value is larger than a byte, the bottom byte is duplicated to the top
            // (with some states this only applies when viewed as negative in hexadecimal,
            // but the lookup uses the non-signed version since that works fine)
            byte rawValue = unchecked((byte)stateBlock.ReadInt(statePtr));
            if (!s_SliderLookup.TryGetValue(rawValue, out var flags))
            {
#if PLASTICBAND_DEBUG_CONTROLS
                if (rawValue != previousValue)
                {
                    Debug.LogWarning($"[GuitarHeroSlider] rawValue {rawValue:X} is not defined in lookup!");
                    previousValue = rawValue;
                }
#endif

                return SliderFret.None;
            }

#if PLASTICBAND_DEBUG_CONTROLS
            if (rawValue != previousValue)
            {
                Debug.Log($"[GuitarHeroSlider] rawValue: {rawValue:X}  flags: {flags}");
                previousValue = rawValue;
            }
#endif

            return flags;
        }
    }
}