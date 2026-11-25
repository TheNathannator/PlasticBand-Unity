#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Rock Band Drumkit for PS3", hideInUI = true)]
    internal class PS3EvdevFourLaneDrumkit : EvdevFourLaneDrumkit<PS3EvdevFourLaneDrumkit>
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
        }
    }
}
#endif
