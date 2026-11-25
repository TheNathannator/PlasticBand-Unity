#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller DJ Hero Turntable", hideInUI = true)]
    internal class SantrollerEvdevTurntable : EvdevTurntable<SantrollerEvdevTurntable>
    {
        internal new static void Initialize()
        {
            m_SouthButton = input_key_code.BTN_A;
            m_EastButton = input_key_code.BTN_B;
            m_WestButton = input_key_code.BTN_X;
            m_NorthButton = input_key_code.BTN_Y;

            m_LeftGreen = input_key_code.BTN_TR;
            m_LeftRed = input_key_code.BTN_TL_2;
            m_LeftBlue = input_key_code.BTN_TR_2;
            m_RightGreen = input_key_code.BTN_C;
            m_RightRed = input_key_code.BTN_Z;
            m_RightBlue = input_key_code.BTN_TL;

            m_LeftVelocity = input_abs_code.ABS_X;
            m_RightVelocity = input_abs_code.ABS_Y;
            m_Crossfader = input_abs_code.ABS_RY;
            m_EffectsDial = input_abs_code.ABS_RX;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithHat(0, "dpad")
                .WithKey(input_key_code.BTN_TR, "leftTableGreen")
                .WithKey(input_key_code.BTN_TL_2, "leftTableRed")
                .WithKey(input_key_code.BTN_TR_2, "leftTableBlue")
                .WithKey(input_key_code.BTN_C, "rightTableGreen")
                .WithKey(input_key_code.BTN_Z, "rightTableRed")
                .WithKey(input_key_code.BTN_TL, "rightTableBlue")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, "rightTableVelocity")
                .WithAbs(input_abs_code.ABS_RX, "effectsDial")
                .WithAbs(input_abs_code.ABS_RY, "crossfader");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerEvdevTurntable>(
                mappings,
                SantrollerLayoutFinder.GetEvdevV1HidMatcher(SantrollerDeviceType.DjHeroTurntable)
            );
        }

        protected override unsafe sbyte ReadLeftVelocity(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_LeftVelocity) - 0x80);

        protected override unsafe sbyte ReadRightVelocity(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_RightVelocity) - 0x80);

        protected override unsafe sbyte ReadCrossfader(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_Crossfader) - 0x80);

        protected override unsafe ushort ReadEffectsDial(StateEvent* stateEvent)
            => (ushort)((m_StateHandler.ReadAbs(stateEvent, m_EffectsDial) & 0xFF) << 8);
    }
}
#endif
