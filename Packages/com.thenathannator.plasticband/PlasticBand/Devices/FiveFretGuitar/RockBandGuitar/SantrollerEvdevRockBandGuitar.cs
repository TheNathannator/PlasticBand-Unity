#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState), displayName = "Santroller Rock Band Guitar", hideInUI = true)]
    internal class SantrollerEvdevRockBandGuitar : EvdevRockBandGuitar<SantrollerEvdevRockBandGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_A;
            m_RedFret = input_key_code.BTN_B;
            m_YellowFret = input_key_code.BTN_Y;
            m_BlueFret = input_key_code.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;

            m_Whammy = input_abs_code.ABS_X;
            m_Tilt = input_abs_code.ABS_RX;
            m_PickupSwitch = input_abs_code.ABS_Y;

            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithKey(input_key_code.BTN_TR, "soloGreen")
                .WithKey(input_key_code.BTN_TL_2, "soloRed")
                .WithKey(input_key_code.BTN_TR_2, "soloYellow")
                .WithKey(input_key_code.BTN_C, "soloBlue")
                .WithKey(input_key_code.BTN_Z, "soloOrange")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_X, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_Y, "pickupSwitch", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RX, "tilt");

            EvdevLayoutFinder.RegisterStaticLayout<SantrollerEvdevRockBandGuitar>(
                mappings,
                SantrollerLayoutFinder.GetEvdevV1HidMatcher(SantrollerDeviceType.RockBandGuitar)
            );
        }

        // The solo frets are their own buttons rather than a solo flag
        protected override unsafe FiveFret ReadSoloFrets(StateEvent* stateEvent)
        {
            var soloFrets = FiveFret.None;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TR)) soloFrets |= FiveFret.Green;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TL_2)) soloFrets |= FiveFret.Red;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_TR_2)) soloFrets |= FiveFret.Yellow;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_C)) soloFrets |= FiveFret.Blue;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_Z)) soloFrets |= FiveFret.Orange;
            return soloFrets;
        }

        protected override unsafe byte ReadWhammy(StateEvent* stateEvent)
            => (byte)m_StateHandler.ReadAbs(stateEvent, m_Whammy);

        protected override unsafe sbyte ReadTilt(StateEvent* stateEvent)
            => (sbyte)(m_StateHandler.ReadAbs(stateEvent, m_Tilt) ^ 0x80);
    }
}
#endif
