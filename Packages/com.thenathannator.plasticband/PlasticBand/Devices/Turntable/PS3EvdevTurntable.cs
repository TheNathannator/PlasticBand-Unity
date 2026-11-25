#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    // This requires a fixed hid-sony release that hasn't been mainlined yet, just putting it here for reference
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "DJ Hero Turntable for PS3", hideInUI = true)]
    internal class PS3EvdevTurntable : EvdevTurntable<PS3EvdevTurntable>
    {
        internal new static void Initialize()
        {
            m_SouthButton = input_key_code.BTN_A;
            m_EastButton = input_key_code.BTN_B;
            m_WestButton = input_key_code.BTN_Y;
            m_NorthButton = input_key_code.BTN_X;

            m_LeftGreen = input_key_code.BTN_TRIGGER_HAPPY_1;
            m_LeftRed = input_key_code.BTN_TRIGGER_HAPPY_2;
            m_LeftBlue = input_key_code.BTN_TRIGGER_HAPPY_3;
            m_RightGreen = input_key_code.BTN_TRIGGER_HAPPY_4;
            m_RightRed = input_key_code.BTN_TRIGGER_HAPPY_5;
            m_RightBlue = input_key_code.BTN_TRIGGER_HAPPY_6;

            m_LeftVelocity = input_abs_code.ABS_X;
            m_RightVelocity = input_abs_code.ABS_Y;
            m_Crossfader = input_abs_code.ABS_RX;
            m_EffectsDial = input_abs_code.ABS_RY;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_Y, "buttonWest")
                .WithKey(input_key_code.BTN_X, "buttonNorth")
                .WithHat(0, "dpad")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_1, "leftTableGreen")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_2, "leftTableRed")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_3, "leftTableBlue")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_4, "rightTableGreen")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_5, "rightTableRed")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_6, "rightTableBlue")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, "rightTableVelocity")
                .WithAbs(input_abs_code.ABS_RX, "crossfader")
                .WithAbs(input_abs_code.ABS_RY, "effectsDial");

            // Activision DJ Hero turntable for PS3
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevTurntable>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x0140)
            );
        }

        // 8-bit, centered on 0x80
        protected override unsafe sbyte ReadLeftVelocity(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_LeftVelocity) - 0x80);

        protected override unsafe sbyte ReadRightVelocity(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_RightVelocity) - 0x80);

        // 10-bit
        protected override unsafe sbyte ReadCrossfader(StateEvent* stateEvent)
            => (sbyte)((m_StateHandler.ReadAbs(stateEvent, m_Crossfader) >> 2) - 0x80);

        protected override unsafe ushort ReadEffectsDial(StateEvent* stateEvent)
            => (ushort)(m_StateHandler.ReadAbs(stateEvent, m_EffectsDial) << 6);
    }
}
#endif
