#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Rock Band Drumkit for Wii", hideInUI = true)]
    internal class WiiEvdevFourLaneDrumkit : PS3EvdevFourLaneDrumkit
    {
        internal new static void Initialize()
        {
            m_Red = input_key_code.BTN_B;
            m_Yellow = input_key_code.BTN_X;
            m_Blue = input_key_code.BTN_Y;
            m_Green = input_key_code.BTN_A;
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
                .WithKey(input_key_code.BTN_THUMB_L, "padFlag")
                .WithKey(input_key_code.BTN_THUMB_R, "cymbalFlag")
                .WithKey(input_key_code.BTN_TL, "kick1")
                .WithKey(input_key_code.BTN_TR, "kick2");

            // Harmonix Rock Band 1/2 drumkits for Wii
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x0005)
            );
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x3110)
            );

            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (drums mode)
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x1BAD, 0x3138)
            );
        }
    }
}
#endif
