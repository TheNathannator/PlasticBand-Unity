#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "PS3/Wii Evdev Rock Band Drumkit", hideInUI = true)]
    internal class PS3EvdevFourLaneDrumkit : EvdevFourLaneDrumkit<PS3EvdevFourLaneDrumkit>
    {
        internal new static void Initialize()
        {
            m_Green = input_key_code.BTN_A;
            m_Red = input_key_code.BTN_B;
            m_Yellow = input_key_code.BTN_X;
            m_Blue = input_key_code.BTN_Y;
            m_Kick1 = input_key_code.BTN_TL;
            m_Kick2 = input_key_code.BTN_TR;
            m_PadFlag = input_key_code.BTN_THUMB_L;
            m_CymbalFlag = input_key_code.BTN_THUMB_R;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad")
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonNorth")
                .WithKey(input_key_code.BTN_Y, "buttonWest")
                .WithKey(input_key_code.BTN_TL, "kick1")
                .WithKey(input_key_code.BTN_TR, "kick2")
                .WithKey(input_key_code.BTN_THUMB_L, "padFlag")
                .WithKey(input_key_code.BTN_THUMB_R, "cymbalFlag");

            // Harmonix Rock Band drumkit for PS3
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x0210)
            );

            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (drums mode)
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x0218)
            );

            // Harmonix Rock Band 1/2 drumkits for Wii
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x0005)
            );
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x3110)
            );

            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (drums mode)
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x3138)
            );
        }
    }
}
#endif
