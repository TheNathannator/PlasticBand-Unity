#if HIDROGEN_0_6_0_OR_LATER
using System;
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedSixFretState), displayName = "PS3/Wii U Evdev Guitar Hero Live Guitar", hideInUI = true)]
    internal class PS3EvdevSixFretGuitar : EvdevSixFretGuitar<PS3EvdevSixFretGuitar>
    {
        internal new static void Initialize()
        {
            m_Black1 = input_key_code.BTN_A;
            m_Black2 = input_key_code.BTN_B;
            m_Black3 = input_key_code.BTN_X;
            m_White1 = input_key_code.BTN_Y;
            m_White2 = input_key_code.BTN_TL;
            m_White3 = input_key_code.BTN_TR;
            m_Ghtv = input_key_code.BTN_THUMB_L;
            m_Select = input_key_code.BTN_SELECT;

            m_Whammy = input_abs_code.ABS_RZ;
            m_Tilt = input_abs_code.ABS_RY;

            // These mappings are only required for device validation, but may as well fill them out properly
            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "black1")
                .WithKey(input_key_code.BTN_B, "black2")
                .WithKey(input_key_code.BTN_X, "black3")
                .WithKey(input_key_code.BTN_Y, "white1")
                .WithKey(input_key_code.BTN_TL, "white2")
                .WithKey(input_key_code.BTN_TR, "white3")
                .WithKey(input_key_code.BTN_THUMB_L, "ghtvButton")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Y, "strumBar")
                .WithAbs(input_abs_code.ABS_RZ, "whammy")
                .WithAbs(input_abs_code.ABS_RY, "tilt");

            // Activision Guitar Hero Live dongle for PS3/Wii U
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevSixFretGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x074B)
            );
        }

        protected override unsafe void ReadStrum(StateEvent* stateEvent, out bool strumUp, out bool strumDown)
        {
            int strumBar = m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y);
            strumUp = strumBar < 0x80;
            strumDown = strumBar > 0x80;
        }

        // Rests at the center of its axis, with only the upper half being used;
        // this matches the HID layout's (whammy - 0x80) * 2
        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => (byte)((m_StateHandler.ReadAbs(stateEvent, m_Whammy) - 0x80) * 2);

        // In the 10-bit accelerometer field, but an 8-bit value centered at 0x80, as in the HID layout
        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => (sbyte)(Math.Min(m_StateHandler.ReadAbs(stateEvent, m_Tilt) & 0x3FF, 0xFF) - 0x80);
    }
}
#endif
