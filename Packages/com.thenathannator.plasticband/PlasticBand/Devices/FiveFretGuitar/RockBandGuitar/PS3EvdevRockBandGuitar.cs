#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Rock Band Guitar for PS3", hideInUI = true)]
    internal class PS3EvdevRockBandGuitar : EvdevRockBandGuitar<PS3EvdevRockBandGuitar>
    {
        private byte m_LastWhammy;
        private int m_LastPickupSwitch;

        internal new static void Initialize()
        {
            m_GreenFret = input_key_code_alt.BTN_A;
            m_RedFret = input_key_code_alt.BTN_B;
            m_YellowFret = input_key_code_alt.BTN_X;
            m_BlueFret = input_key_code_alt.BTN_Y;
            m_OrangeFret = input_key_code.BTN_TL;
            m_Solo = input_key_code.BTN_TL_2;

            m_Whammy = input_abs_code.ABS_Z;
            m_PickupSwitch = input_abs_code.ABS_RZ;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code_alt.BTN_A, "greenFret")
                .WithKey(input_key_code_alt.BTN_B, "redFret")
                .WithKey(input_key_code_alt.BTN_X, "yellowFret")
                .WithKey(input_key_code_alt.BTN_Y, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "tilt")
                .WithKey(input_key_code.BTN_TL_2, "solo")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Z, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RZ, "pickupSwitch", EvdevAxisOption.Unsigned);

            // Harmonix Rock Band guitar for PS3
            EvdevLayoutFinder.RegisterStaticLayout<PS3EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x12BA, 0x0200)
            );
        }

        // Whammy and pickup switch have a null value (0x80), which reads as the last non-null value
        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
        {
            byte whammy = (byte)m_StateHandler.ReadAbs(stateEvent, m_Whammy);
            if (whammy != RockBandGuitarState.kNullValue)
                m_LastWhammy = whammy;
            return m_LastWhammy;
        }

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TR) ? sbyte.MaxValue : (sbyte)0;

        protected override unsafe int ReadPickupSwitch(StateEvent* stateEvent)
        {
            int pickupSwitch = RockBandGuitarState.GetPickupSwitchNotch_NullState((byte)m_StateHandler.ReadAbs(stateEvent, m_PickupSwitch));
            if (pickupSwitch >= 0)
                m_LastPickupSwitch = pickupSwitch;
            return m_LastPickupSwitch;
        }
    }
}
#endif
