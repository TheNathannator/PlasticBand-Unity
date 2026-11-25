#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFourLaneState), displayName = "Rock Band Drumkit for Xbox One", hideInUI = true)]
    internal class XoneEvdevFourLaneDrumkit : EvdevFourLaneDrumkit<XoneEvdevFourLaneDrumkit>
    {
        internal new static void Initialize()
        {
            // xone swaps X and Y compared to the gamepad
            m_Red = input_key_code.BTN_B;
            m_Yellow = input_key_code.BTN_X;
            m_Blue = input_key_code.BTN_Y;
            m_Green = input_key_code.BTN_A;
            m_Kick1 = input_key_code.BTN_TRIGGER_HAPPY_1;
            m_Kick2 = input_key_code.BTN_TRIGGER_HAPPY_2;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad")
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonNorth")
                .WithKey(input_key_code.BTN_Y, "buttonWest")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_1, "kick1")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_2, "kick2")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_3, "redPad")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_4, "yellowPad")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_5, "bluePad")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_6, "greenPad")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_7, "yellowCymbal")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_8, "blueCymbal")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_9, "greenCymbal");

            // MadCatz Rock Band 4 drumkit for Xbox One
            EvdevLayoutFinder.RegisterStaticLayout<XoneEvdevFourLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher().WithDriver(EvdevDefinitions.DriverXoneMadCatzDrumkit)
            );
        }

        // Pads and cymbals are reported directly, but xone only passes on whether each was hit,
        // so every hit reads at full velocity
        protected override unsafe FourLanePad ReadPads(StateEvent* stateEvent,
            bool red, bool yellow, bool blue, bool green, bool dpadUp, bool dpadDown)
        {
            var pads = FourLanePad.None;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_3)) pads |= FourLanePad.RedPad;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_4)) pads |= FourLanePad.YellowPad;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_5)) pads |= FourLanePad.BluePad;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_6)) pads |= FourLanePad.GreenPad;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_7)) pads |= FourLanePad.YellowCymbal;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_8)) pads |= FourLanePad.BlueCymbal;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_9)) pads |= FourLanePad.GreenCymbal;
            return pads;
        }
    }
}
#endif
