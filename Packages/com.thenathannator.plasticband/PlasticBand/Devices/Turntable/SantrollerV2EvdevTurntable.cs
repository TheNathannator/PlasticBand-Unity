#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Santroller V2 DJ Hero Turntable", hideInUI = true)]
    internal class SantrollerV2EvdevTurntable : EvdevTurntable<SantrollerV2EvdevTurntable>
    {
        internal new static void Initialize()
        {
            m_SouthButton = input_key_code_alt.BTN_A;
            m_EastButton = input_key_code_alt.BTN_B;
            m_WestButton = input_key_code_alt.BTN_X;
            m_NorthButton = input_key_code_alt.BTN_Y;

            m_LeftVelocity = input_abs_code.ABS_X;
            m_RightVelocity = input_abs_code.ABS_Y;
            m_Crossfader = input_abs_code.ABS_RY;
            m_EffectsDial = input_abs_code.ABS_RX;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code_alt.BTN_A, "buttonSouth")
                .WithKey(input_key_code_alt.BTN_B, "buttonEast")
                .WithKey(input_key_code_alt.BTN_X, "buttonWest")
                .WithKey(input_key_code_alt.BTN_Y, "buttonNorth")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, "rightTableVelocity")
                .WithAbs(input_abs_code.ABS_RX, "effectsDial")
                .WithAbs(input_abs_code.ABS_RY, "crossfader")
                .WithAbs(input_abs_code.ABS_Z, "leftTableGreen")
                .WithAbs(input_abs_code.ABS_RZ, "rightTableGreen");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerV2EvdevTurntable>(
                mappings,
                SantrollerLayoutFinder.GetEvdevMatcher(SantrollerDeviceType.DjHeroTurntable)
            );
        }

        protected override unsafe TurntableButton ReadLeftTableButtons(StateEvent* stateEvent)
            => ReadPlatterBitmask(stateEvent, input_abs_code.ABS_Z);

        protected override unsafe TurntableButton ReadRightTableButtons(StateEvent* stateEvent)
            => ReadPlatterBitmask(stateEvent, input_abs_code.ABS_RZ);

        protected override unsafe sbyte ReadLeftVelocity(StateEvent* stateEvent)
            => (sbyte)((short)m_StateHandler.ReadAbs(stateEvent, m_LeftVelocity) >> 8);

        protected override unsafe sbyte ReadRightVelocity(StateEvent* stateEvent)
            => (sbyte)((short)m_StateHandler.ReadAbs(stateEvent, m_RightVelocity) >> 8);

        protected override unsafe sbyte ReadCrossfader(StateEvent* stateEvent)
            => (sbyte)((short)m_StateHandler.ReadAbs(stateEvent, m_Crossfader) >> 8);

        protected override unsafe ushort ReadEffectsDial(StateEvent* stateEvent)
            => (ushort)m_StateHandler.ReadAbs(stateEvent, m_EffectsDial);
    }
}
#endif
