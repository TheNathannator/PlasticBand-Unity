#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedTurntableState), displayName = "Xpad Evdev DJ Hero Turntable", hideInUI = true)]
    internal class XpadEvdevTurntable : EvdevTurntable<XpadEvdevTurntable>
    {
        internal new static void Initialize()
        {
            m_SouthButton = input_key_code.BTN_A;
            m_EastButton = input_key_code.BTN_B;
            m_WestButton = input_key_code.BTN_X;
            m_NorthButton = input_key_code.BTN_Y;

            m_LeftVelocity = input_abs_code.ABS_X;
            m_RightVelocity = input_abs_code.ABS_Y;
            m_Crossfader = input_abs_code.ABS_RY;
            m_EffectsDial = input_abs_code.ABS_RX;

            var xpadMappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, "rightTableVelocity")
                .WithAbs(input_abs_code.ABS_RX, "effectsDial")
                .WithAbs(input_abs_code.ABS_RY, "crossfader")
                .WithAbs(input_abs_code.ABS_Z, "leftTableGreen")
                .WithAbs(input_abs_code.ABS_RZ, "rightTableGreen");

            // Activision DJ Hero turntable for Xbox 360
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevTurntable>(
                xpadMappings,
                new EvdevDeviceMatcher()
                    .WithDriver(EvdevDefinitions.DriverXpad)
                    .WithXusbSubtype(XInputDeviceSubType.Turntable)
            );

            // Santroller V1 devices in XInput mode
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevTurntable>(
                xpadMappings,
                SantrollerLayoutFinder.GetEvdevXpadMatcher(XInputDeviceSubType.Turntable, SantrollerDeviceType.DjHeroTurntable)
            );
        }

        protected override unsafe TurntableButton ReadLeftTableButtons(StateEvent* stateEvent)
            => ReadPlatterBitmask(stateEvent, input_abs_code.ABS_Z);

        protected override unsafe TurntableButton ReadRightTableButtons(StateEvent* stateEvent)
            => ReadPlatterBitmask(stateEvent, input_abs_code.ABS_RZ);

        protected override unsafe sbyte ReadLeftVelocity(StateEvent* stateEvent)
            => (sbyte)m_StateHandler.ReadAbs(stateEvent, m_LeftVelocity);

        protected override unsafe sbyte ReadRightVelocity(StateEvent* stateEvent)
            => (sbyte)~m_StateHandler.ReadAbs(stateEvent, m_RightVelocity);

        protected override unsafe sbyte ReadCrossfader(StateEvent* stateEvent)
            => (sbyte)((short)~m_StateHandler.ReadAbs(stateEvent, m_Crossfader) >> 8);

        // The raw signed 16-bit value; the dial wraps, so it's reinterpreted as unsigned like XInputTurntableState does
        protected override unsafe ushort ReadEffectsDial(StateEvent* stateEvent)
            => (ushort)m_StateHandler.ReadAbs(stateEvent, m_EffectsDial);
    }
}
#endif
