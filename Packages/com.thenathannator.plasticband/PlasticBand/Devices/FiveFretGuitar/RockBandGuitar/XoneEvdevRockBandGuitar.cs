#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Rock Band Guitar for Xbox One", hideInUI = true)]
    internal class XoneEvdevRockBandGuitar : EvdevRockBandGuitar<XoneEvdevRockBandGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_TRIGGER_HAPPY_1;
            m_RedFret = input_key_code.BTN_TRIGGER_HAPPY_2;
            m_YellowFret = input_key_code.BTN_TRIGGER_HAPPY_3;
            m_BlueFret = input_key_code.BTN_TRIGGER_HAPPY_4;
            m_OrangeFret = input_key_code.BTN_TRIGGER_HAPPY_5;

            m_Whammy = input_abs_code.ABS_Y;
            m_Tilt = input_abs_code.ABS_Z;
            m_PickupSwitch = input_abs_code.ABS_X;

            var jaguarMappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_1, "greenFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_2, "redFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_3, "yellowFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_4, "blueFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_5, "orangeFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_6, "soloGreen")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_7, "soloRed")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_8, "soloYellow")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_9, "soloBlue")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_10, "soloOrange")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Y, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_Z, "tilt", EvdevAxisOption.Unsigned);

            // The Stratocaster additionally has its pickup switch exposed
            var stratocasterMappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_1, "greenFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_2, "redFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_3, "yellowFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_4, "blueFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_5, "orangeFret")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_6, "soloGreen")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_7, "soloRed")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_8, "soloYellow")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_9, "soloBlue")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY_10, "soloOrange")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_Y, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_Z, "tilt", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_X, "pickupSwitch", EvdevAxisOption.Unsigned);

            // MadCatz Stratocaster for Xbox One
            EvdevLayoutFinder.RegisterStaticLayout<XoneEvdevRockBandGuitar>(
                stratocasterMappings,
                new EvdevDeviceMatcher().WithDriver(EvdevDefinitions.DriverXoneMadCatzStratocaster)
            );

            // PDP Jaguar for Xbox One
            EvdevLayoutFinder.RegisterStaticLayout<XoneEvdevRockBandGuitar>(
                jaguarMappings,
                new EvdevDeviceMatcher().WithDriver(EvdevDefinitions.DriverXonePdpJaguar)
            );
        }

        // The solo frets are their own buttons rather than a solo flag
        protected override unsafe FiveFret ReadSoloFrets(StateEvent* stateEvent)
        {
            var soloFrets = FiveFret.None;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_6)) soloFrets |= FiveFret.Green;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_7)) soloFrets |= FiveFret.Red;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_8)) soloFrets |= FiveFret.Yellow;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_9)) soloFrets |= FiveFret.Blue;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TRIGGER_HAPPY_10)) soloFrets |= FiveFret.Orange;
            return soloFrets;
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => (byte)m_StateHandler.ReadAbs(stateEvent, m_Whammy);

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => (sbyte)((byte)m_StateHandler.ReadAbs(stateEvent, m_Tilt) >> 1);

        // The pickup switch notch is in the top nibble (0x00, 0x10, 0x20, 0x30, or 0x40); the Jaguar has none,
        // and reads as 0
        protected override unsafe int ReadPickupSwitch(StateEvent* stateEvent)
            => (byte)m_StateHandler.ReadAbs(stateEvent, m_PickupSwitch) >> 4;
    }
}
#endif
