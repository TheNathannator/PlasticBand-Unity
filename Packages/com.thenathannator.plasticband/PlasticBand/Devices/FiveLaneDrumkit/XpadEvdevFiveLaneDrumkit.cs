#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Guitar Hero Drumkit for Xbox 360", hideInUI = true)]
    internal class XpadEvdevFiveLaneDrumkit : EvdevFiveLaneDrumkit<XpadEvdevFiveLaneDrumkit>
    {
        internal new static void Initialize()
        {
            m_RedPad = input_key_code_alt.BTN_B;
            m_YellowCymbal = input_key_code_alt.BTN_Y;
            m_BluePad = input_key_code_alt.BTN_X;
            m_OrangeCymbal = input_key_code.BTN_TR;
            m_GreenPad = input_key_code_alt.BTN_A;
            m_Kick = input_key_code.BTN_TL;

            var xpadMappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code_alt.BTN_B, "redPad")
                .WithKey(input_key_code_alt.BTN_Y, "yellowCymbal")
                .WithKey(input_key_code_alt.BTN_X, "bluePad")
                .WithKey(input_key_code.BTN_TR, "orangeCymbal")
                .WithKey(input_key_code_alt.BTN_A, "greenPad")
                .WithKey(input_key_code.BTN_TL, "kick")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Y, "velocity1")
                .WithAbs(input_abs_code.ABS_RX, "velocity2")
                .WithAbs(input_abs_code.ABS_RY, "velocity3");

            // RedOctane Guitar Hero 5, Band Hero, and Warriors of Rock Drumkit for Xbox 360
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevFiveLaneDrumkit>(
                xpadMappings,
                new EvdevDeviceMatcher()
                    .WithDriver(EvdevDefinitions.DriverXpad)
                    .WithXusbSubtype(XInputDeviceSubType.DrumKit)
                    .WithHardwareIds(0x1430, 0x0805)
            );

            // Santroller V1 devices in XInput mode
            EvdevLayoutFinder.RegisterStaticLayout<XpadEvdevFiveLaneDrumkit>(
                xpadMappings,
                SantrollerLayoutFinder.GetEvdevXpadMatcher(XInputDeviceSubType.DrumKit, SantrollerDeviceType.GuitarHeroDrums)
            );
        }

        protected override unsafe void ReadVelocities(
            StateEvent* stateEvent,
            out UInt7 red, out UInt7 yellow, out UInt7 blue,
            out UInt7 orange, out UInt7 green, out UInt7 kick
        )
        {
            // Velocities are packed two at a time into the stick axes
            int leftStickY = ~m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_Y);
            int rightStickX = m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RX);
            int rightStickY = ~m_StateHandler.ReadAbs(stateEvent, input_abs_code.ABS_RY);

            red = UInt7.Clamped((leftStickY >> 8) & 0xFF);
            yellow = UInt7.Clamped(rightStickX & 0xFF);
            blue = UInt7.Clamped((rightStickX >> 8) & 0xFF);
            orange = UInt7.Clamped(rightStickY & 0xFF);
            green = UInt7.Clamped(leftStickY & 0xFF);
            kick = UInt7.Clamped((rightStickY >> 8) & 0xFF);
        }
    }
}
#endif
