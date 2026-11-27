#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Rock Band Guitar for PS4", hideInUI = true)]
    internal class PS4EvdevRockBandGuitar : EvdevRockBandGuitar<PS4EvdevRockBandGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code_alt.BTN_A;
            m_RedFret = input_key_code_alt.BTN_B;
            m_YellowFret = input_key_code_alt.BTN_X;
            m_BlueFret = input_key_code_alt.BTN_Y;
            m_OrangeFret = input_key_code.BTN_TL;
            m_Solo = input_key_code.BTN_THUMB_L;

            m_Whammy = input_abs_code.ABS_Z;
            m_Tilt = input_abs_code.ABS_RZ;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code_alt.BTN_A, "greenFret")
                .WithKey(input_key_code_alt.BTN_B, "redFret")
                .WithKey(input_key_code_alt.BTN_X, "yellowFret")
                .WithKey(input_key_code_alt.BTN_Y, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_THUMB_L, "solo")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Z, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RZ, "tilt", EvdevAxisOption.Unsigned);

            // PDP Riffmaster for PS4
            EvdevLayoutFinder.RegisterStaticLayout<PS4EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x0E6F, 0x024A)
            );

            // PDP Jaguar for PS4 (Bluetooth)
            EvdevLayoutFinder.RegisterStaticLayout<PS4EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x0E6F, 0x0173)
            );

            // MadCatz Stratocaster for PS4 (Bluetooth)
            EvdevLayoutFinder.RegisterStaticLayout<PS4EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x0738, 0x8261)
            );

            // CRKD Gibson SG for PS4 (wired and dongle)
            EvdevLayoutFinder.RegisterStaticLayout<PS4EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x3651, 0x1500)
            );
            EvdevLayoutFinder.RegisterStaticLayout<PS4EvdevRockBandGuitar>(
                mappings,
                new EvdevDeviceMatcher().WithHardwareIds(0x3651, 0x5500)
            );
        }

        // hid-sony doesn't expose most of the vendor bytes, so we don't get seperate regular/solo
        // frets or the pickup. solo is done flag-style for now until the driver exposes those inputs

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => (byte)m_StateHandler.ReadAbs(stateEvent, m_Whammy);

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => (sbyte)((byte)m_StateHandler.ReadAbs(stateEvent, m_Tilt) >> 1);

        protected override unsafe int ReadPickupSwitch(StateEvent* stateEvent) => 0;
    }
}
#endif
