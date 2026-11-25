#if HIDROGEN_0_6_0_OR_LATER
using System;
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "Wii Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class WiiEvdevGuitarHeroGuitar : EvdevGuitarHeroGuitar<WiiEvdevGuitarHeroGuitar>
    {

        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_1;
            m_RedFret = input_key_code.BTN_2;
            m_YellowFret = input_key_code.BTN_3;
            m_BlueFret = input_key_code.BTN_4;
            m_OrangeFret = input_key_code.BTN_5;
            m_SpPedal = default; // No star power pedal

            m_Whammy = input_abs_code.ABS_HAT1X;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_DPAD_UP, "dpad/up")
                .WithKey(input_key_code.BTN_DPAD_DOWN, "dpad/down")
                .WithKey(input_key_code.BTN_1, "greenFret")
                .WithKey(input_key_code.BTN_2, "redFret")
                .WithKey(input_key_code.BTN_3, "yellowFret")
                .WithKey(input_key_code.BTN_4, "blueFret")
                .WithKey(input_key_code.BTN_5, "orangeFret")
                .WithAbs(input_abs_code.ABS_HAT0X, "touchBar")
                .WithAbs(input_abs_code.ABS_HAT1X, "whammy");

            // All Wii Remote extensions share the Wii Remote's IDs, so they're told apart by hid-wiimote's device name

            // Nintendo Wii Remote
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevGuitarHeroGuitar>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x057E, 0x0306)
                    .WithProduct("Nintendo Wii Remote Guitar")
            );

            // Nintendo Wii Remote (Wii MotionPlus Inside)
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevGuitarHeroGuitar>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x057E, 0x0330)
                    .WithProduct("Nintendo Wii Remote Guitar")
            );
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
        {
            int whammy = m_StateHandler.ReadAbs(stateEvent, m_Whammy);
            // wiimote driver sends raw value, so we have to scale it up
            return (byte)(Math.Min(Math.Max(whammy, 0), 0x0F) * byte.MaxValue / 0x0F);
        }

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent) => 0;

        protected override unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent)
        {
            int touchBar = m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_HAT0X);
            // wiimote driver sends raw value, so we have to map
            byte rawTouchBar = (byte)((touchBar & 0x1F) << 3);
            return TranslatingGuitarHeroGuitar.s_WtTouchFrets[(rawTouchBar & 0xF0) >> 4];
        }
    }
}
#endif
