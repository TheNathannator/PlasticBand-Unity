#if HIDROGEN_0_6_0_OR_LATER
using System;
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Guitar Hero Guitar for PS3", hideInUI = true)]
    internal class PS3EvdevGuitarHeroGuitar : EvdevGuitarHeroGuitar<PS3EvdevGuitarHeroGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_A;
            m_RedFret = input_key_code.BTN_B;
            m_YellowFret = input_key_code.BTN_Y;
            m_BlueFret = input_key_code.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;
            m_SpPedal = input_key_code.BTN_TR;

            m_Whammy = input_abs_code.ABS_Z;
            m_Tilt = input_abs_code.ABS_RY;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "spPedal")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Z, "whammy")
                .WithAbs(input_abs_code.ABS_RY, "tilt")
                .WithAbs(input_abs_code.ABS_RZ, "touchFrets");

            // RedOctane Guitar Hero Guitar for PS3 (all models)
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevGuitarHeroGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x0100)
            );

            // RedOctane Guitar Hero 5 Guitar for PS3
            // Needs special handling for its slider bar
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevGuitarHeroGuitar_GH5>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x12BA, 0x0100)
                    .WithProduct("Guitar Hero5 for PlayStation (R) 3", supportRegex: false)
            );
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
        {
            int whammy = m_StateHandler.ReadAbs(stateEvent, m_Whammy);
            return (byte)Math.Min(Math.Max((whammy - 0x80) * 2, 0), byte.MaxValue);
        }

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
        {
            // The accelerometer reads below 0x200 when the guitar is pointed up, so the range is reversed
            int tilt = m_StateHandler.ReadAbs(stateEvent, m_Tilt);
            return (sbyte)IntegerAxisControl.Convert(tilt, 0x280, 0x180, 0x200, sbyte.MinValue, sbyte.MaxValue, 0);
        }

        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
        {
            byte touchBar = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RZ);
            return TranslatingGuitarHeroGuitar.s_WtTouchFrets[(touchBar & 0xF0) >> 4];
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "PS3 Evdev Guitar Hero 5 Guitar", hideInUI = true)]
    internal class PS3EvdevGuitarHeroGuitar_GH5 : PS3EvdevGuitarHeroGuitar
    {
        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
        {
            byte touchBar = (byte)m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RZ);
            return TranslatingGuitarHeroGuitar.s_Gh5TouchFrets[touchBar];
        }
    }
}
#endif
