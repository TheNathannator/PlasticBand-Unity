#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Wii Evdev DJ Hero Turntable", hideInUI = true)]
    internal class WiiEvdevTurntable : EvdevTurntable<WiiEvdevTurntable>
    {
        internal new static void Initialize()
        {
            m_SouthButton = default; // No face buttons
            m_EastButton = default;
            m_WestButton = default;
            m_NorthButton = input_key_code.BTN_7; // Euphoria

            m_LeftGreen = input_key_code.BTN_4;
            m_LeftRed = input_key_code.BTN_5;
            m_LeftBlue = input_key_code.BTN_6;
            m_RightGreen = input_key_code.BTN_1;
            m_RightRed = input_key_code.BTN_2;
            m_RightBlue = input_key_code.BTN_3;

            m_LeftVelocity = input_abs_code.ABS_HAT1X;
            m_RightVelocity = input_abs_code.ABS_HAT0X;
            m_Crossfader = input_abs_code.ABS_HAT2X;
            m_EffectsDial = input_abs_code.ABS_HAT3X;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_1, "rightTableGreen")
                .WithKey(input_key_code.BTN_2, "rightTableRed")
                .WithKey(input_key_code.BTN_3, "rightTableBlue")
                .WithKey(input_key_code.BTN_4, "leftTableGreen")
                .WithKey(input_key_code.BTN_5, "leftTableRed")
                .WithKey(input_key_code.BTN_6, "leftTableBlue")
                .WithKey(input_key_code.BTN_7, "buttonNorth") // Euphoria
                .WithAbs(input_abs_code.ABS_HAT0X, "rightTableVelocity")
                .WithAbs(input_abs_code.ABS_HAT1X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_HAT2X, "crossfader")
                .WithAbs(input_abs_code.ABS_HAT3X, "effectsDial");

            // All Wii Remote extensions share the Wii Remote's IDs, so they're told apart by hid-wiimote's device name

            // Nintendo Wii Remote
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevTurntable>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x057E, 0x0306)
                    .WithProduct("Nintendo Wii Remote Turntable")
            );

            // Nintendo Wii Remote (Wii MotionPlus Inside)
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevTurntable>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x057E, 0x0330)
                    .WithProduct("Nintendo Wii Remote Turntable")
            );
        }

        // The ABS_HAT axes are the turntable's own fields, not a d-pad
        protected override unsafe EvdevHatValue ReadDpad(StateEvent* stateEvent) => EvdevHatValue.None;

        // 6-bit two's complement, scaled up to 8 bits
        private static sbyte FromVelocityField(int raw) => (sbyte)(((raw << 26) >> 26) << 2);

        protected override unsafe sbyte ReadLeftVelocity(StateEvent* stateEvent)
            => FromVelocityField(m_StateHandler.ReadAbs(stateEvent, m_LeftVelocity));

        protected override unsafe sbyte ReadRightVelocity(StateEvent* stateEvent)
            => FromVelocityField(m_StateHandler.ReadAbs(stateEvent, m_RightVelocity));

        // The 4-bit field (reported doubled), scaled up to 8 bits and centred on 0
        protected override unsafe sbyte ReadCrossfader(StateEvent* stateEvent)
            => (sbyte)((((m_StateHandler.ReadAbs(stateEvent, m_Crossfader) >> 1) & 0x0F) << 4) - 0x80);

        // The low 3 bits of the dial, scaled up to 16 bits; wraps like the full dial does
        protected override unsafe ushort ReadEffectsDial(StateEvent* stateEvent)
            => (ushort)((m_StateHandler.ReadAbs(stateEvent, m_EffectsDial) & 0x07) << 13);
    }
}
#endif
