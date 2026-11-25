#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "PS3 Evdev Guitar Hero Drumkit", hideInUI = true)]
    internal class PS3EvdevFiveLaneDrumkit : EvdevFiveLaneDrumkit<PS3EvdevFiveLaneDrumkit>
    {
        internal new static void Initialize()
        {
            m_GreenPad = input_key_code.BTN_A;
            m_RedPad = input_key_code.BTN_B;
            m_YellowCymbal = input_key_code.BTN_X;
            m_BluePad = input_key_code.BTN_Y;
            m_OrangeCymbal = input_key_code.BTN_TR;
            m_Kick = input_key_code.BTN_TL;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenPad")
                .WithKey(input_key_code.BTN_B, "redPad")
                .WithKey(input_key_code.BTN_X, "yellowCymbal")
                .WithKey(input_key_code.BTN_Y, "bluePad")
                .WithKey(input_key_code.BTN_TR, "orangeCymbal")
                .WithKey(input_key_code.BTN_TL, "kick")
                .WithHat(0, "dpad");

            // RedOctane Guitar Hero drumkit dongle for PS3
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevFiveLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x0120)
            );
        }

        // hid-sony doesn't report velocity data, so velocities will always be max
    }
}
#endif
