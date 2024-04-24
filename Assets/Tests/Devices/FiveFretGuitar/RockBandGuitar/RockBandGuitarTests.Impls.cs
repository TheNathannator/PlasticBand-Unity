using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    using SantrollerHIDButton = SantrollerHIDRockBandGuitarState.Button;

    internal class XInputRockBandGuitarTests
        : RockBandGuitarTests_SoloFlag<XInputRockBandGuitar, XInputRockBandGuitarState>
    {
        protected override XInputRockBandGuitarState CreateState()
            => new XInputRockBandGuitarState()
        {
            whammy = short.MinValue,
        };

        protected override void SetDpad(ref XInputRockBandGuitarState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetMenuButtons(ref XInputRockBandGuitarState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetFrets(ref XInputRockBandGuitarState state, FiveFret frets)
            => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

        protected override void SetSoloFrets(ref XInputRockBandGuitarState state, FiveFret frets)
            => XInputRockBandGuitarHandling.SetSoloFrets(ref state.buttons, frets);

        protected override void SetWhammy(ref XInputRockBandGuitarState state, float value)
        {
            state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
        }

        protected override void SetTilt(ref XInputRockBandGuitarState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeInt16(value);
        }

        protected override void SetPickupSwitch(ref XInputRockBandGuitarState state, int value)
        {
            state.pickupSwitch = XInputRockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    internal class SantrollerXInputRockBandGuitarTests
        : RockBandGuitarTests_SoloFlag<SantrollerXInputRockBandGuitar, XInputRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override XInputRockBandGuitarState CreateState()
            => new XInputRockBandGuitarState()
        {
            whammy = short.MinValue,
        };

        protected override void SetDpad(ref XInputRockBandGuitarState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetMenuButtons(ref XInputRockBandGuitarState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetFrets(ref XInputRockBandGuitarState state, FiveFret frets)
            => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

        protected override void SetSoloFrets(ref XInputRockBandGuitarState state, FiveFret frets)
            => XInputRockBandGuitarHandling.SetSoloFrets(ref state.buttons, frets);

        protected override void SetWhammy(ref XInputRockBandGuitarState state, float value)
        {
            state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
        }

        protected override void SetTilt(ref XInputRockBandGuitarState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeInt16(value);
        }

        protected override void SetPickupSwitch(ref XInputRockBandGuitarState state, int value)
        {
            state.pickupSwitch = XInputRockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    internal class PS3RockBandGuitarTests_NoReportId
        : RockBandGuitarTests_SoloFlag<PS3RockBandGuitar, PS3RockBandGuitarState_NoReportId>
    {
        protected override PS3RockBandGuitarState_NoReportId CreateState()
            => new PS3RockBandGuitarState_NoReportId()
        {
            dpad = 8,
        };

        protected override void SetDpad(ref PS3RockBandGuitarState_NoReportId state, DpadDirection dpad)
        {
            state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref PS3RockBandGuitarState_NoReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetFrets(ref PS3RockBandGuitarState_NoReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetFrets(ref state.buttons, frets);

        protected override void SetSoloFrets(ref PS3RockBandGuitarState_NoReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetSoloFrets(ref state.buttons, frets);

        protected override void SetTilt(ref PS3RockBandGuitarState_NoReportId state, float value)
        {
            state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
        }

        protected override void SetWhammy(ref PS3RockBandGuitarState_NoReportId state, float value)
        {
            state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
        }

        protected override void SetPickupSwitch(ref PS3RockBandGuitarState_NoReportId state, int value)
        {
            state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    internal class PS3RockBandGuitarTests_ReportId
        : RockBandGuitarTests_SoloFlag<PS3RockBandGuitar_ReportId, PS3RockBandGuitarState_ReportId>
    {
        protected override PS3RockBandGuitarState_ReportId CreateState()
            => new PS3RockBandGuitarState_ReportId()
        {
            state = new PS3RockBandGuitarState_NoReportId()
            {
                dpad = 8,
            }
        };

        protected override void SetDpad(ref PS3RockBandGuitarState_ReportId state, DpadDirection dpad)
        {
            state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref PS3RockBandGuitarState_ReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.buttons, buttons);

        protected override void SetFrets(ref PS3RockBandGuitarState_ReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetFrets(ref state.state.buttons, frets);

        protected override void SetSoloFrets(ref PS3RockBandGuitarState_ReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetSoloFrets(ref state.state.buttons, frets);

        protected override void SetTilt(ref PS3RockBandGuitarState_ReportId state, float value)
        {
            state.state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
        }

        protected override void SetWhammy(ref PS3RockBandGuitarState_ReportId state, float value)
        {
            state.state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
        }

        protected override void SetPickupSwitch(ref PS3RockBandGuitarState_ReportId state, int value)
        {
            state.state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    internal class WiiRockBandGuitarTests_NoReportId
        : RockBandGuitarTests_SoloFlag<WiiRockBandGuitar, WiiRockBandGuitarState_NoReportId>
    {
        protected override WiiRockBandGuitarState_NoReportId CreateState()
            => new WiiRockBandGuitarState_NoReportId()
        {
            state = new PS3RockBandGuitarState_NoReportId()
            {
                dpad = 8,
            }
        };

        protected override void SetDpad(ref WiiRockBandGuitarState_NoReportId state, DpadDirection dpad)
        {
            state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref WiiRockBandGuitarState_NoReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.buttons, buttons);

        protected override void SetFrets(ref WiiRockBandGuitarState_NoReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetFrets(ref state.state.buttons, frets);

        protected override void SetSoloFrets(ref WiiRockBandGuitarState_NoReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetSoloFrets(ref state.state.buttons, frets);

        protected override void SetTilt(ref WiiRockBandGuitarState_NoReportId state, float value)
        {
            state.state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
        }

        protected override void SetWhammy(ref WiiRockBandGuitarState_NoReportId state, float value)
        {
            state.state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
        }

        protected override void SetPickupSwitch(ref WiiRockBandGuitarState_NoReportId state, int value)
        {
            state.state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    internal class WiiRockBandGuitarTests_ReportId
        : RockBandGuitarTests_SoloFlag<WiiRockBandGuitar_ReportId, WiiRockBandGuitarState_ReportId>
    {
        protected override WiiRockBandGuitarState_ReportId CreateState()
            => new WiiRockBandGuitarState_ReportId()
        {
            // lol, triple-layered state
            state = new WiiRockBandGuitarState_NoReportId()
            {
                state = new PS3RockBandGuitarState_NoReportId()
                {
                    dpad = 8,
                }
            }
        };

        protected override void SetDpad(ref WiiRockBandGuitarState_ReportId state, DpadDirection dpad)
        {
            state.state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref WiiRockBandGuitarState_ReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.state.buttons, buttons);

        protected override void SetFrets(ref WiiRockBandGuitarState_ReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetFrets(ref state.state.state.buttons, frets);

        protected override void SetSoloFrets(ref WiiRockBandGuitarState_ReportId state, FiveFret frets)
            => PS3RockBandGuitarHandling.SetSoloFrets(ref state.state.state.buttons, frets);

        protected override void SetTilt(ref WiiRockBandGuitarState_ReportId state, float value)
        {
            state.state.state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
        }

        protected override void SetWhammy(ref WiiRockBandGuitarState_ReportId state, float value)
        {
            state.state.state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
        }

        protected override void SetPickupSwitch(ref WiiRockBandGuitarState_ReportId state, int value)
        {
            state.state.state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    internal class PS4RockBandGuitarTests_ReportId
        : RockBandGuitarTests_SoloDistinct<PS4RockBandGuitar, PS4RockBandGuitarState_ReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RockBandGuitarState_ReportId CreateState()
            => new PS4RockBandGuitarState_ReportId()
        {
            state = new PS4RockBandGuitarState_NoReportId()
            {
                buttons1 = (ushort)HidDpad.Neutral.AsPS4Buttons(),
            }
        };

        protected override void SetDpad(ref PS4RockBandGuitarState_ReportId state, DpadDirection dpad)
        {
            PS4DeviceHandling.SetDpad(ref state.state.buttons1, dpad);
        }

        protected override void SetMenuButtons(ref PS4RockBandGuitarState_ReportId state, MenuButton buttons)
            => PS4DeviceHandling.SetMenuButtons(ref state.state.buttons1, buttons);

        protected override void SetFrets(ref PS4RockBandGuitarState_ReportId state, FiveFret frets)
            => PS4RockBandGuitarHandling.SetFrets(ref state.state.frets, frets);

        protected override void SetSoloFrets(ref PS4RockBandGuitarState_ReportId state, FiveFret frets)
            => PS4RockBandGuitarHandling.SetFrets(ref state.state.soloFrets, frets);

        protected override void SetTilt(ref PS4RockBandGuitarState_ReportId state, float value)
        {
            state.state.tilt = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetWhammy(ref PS4RockBandGuitarState_ReportId state, float value)
        {
            state.state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetPickupSwitch(ref PS4RockBandGuitarState_ReportId state, int value)
        {
            state.state.pickupSwitch = (byte)value;
        }
    }

    internal class PS4RockBandGuitarTests_NoReportId
        : RockBandGuitarTests_SoloDistinct<PS4RockBandGuitar_NoReportId, PS4RockBandGuitarState_NoReportId>
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected override PS4RockBandGuitarState_NoReportId CreateState()
            => new PS4RockBandGuitarState_NoReportId()
        {
            buttons1 = (ushort)HidDpad.Neutral.AsPS4Buttons(),
        };

        protected override void SetDpad(ref PS4RockBandGuitarState_NoReportId state, DpadDirection dpad)
        {
            PS4DeviceHandling.SetDpad(ref state.buttons1, dpad);
        }

        protected override void SetMenuButtons(ref PS4RockBandGuitarState_NoReportId state, MenuButton buttons)
            => PS4DeviceHandling.SetMenuButtons(ref state.buttons1, buttons);

        protected override void SetFrets(ref PS4RockBandGuitarState_NoReportId state, FiveFret frets)
            => PS4RockBandGuitarHandling.SetFrets(ref state.frets, frets);

        protected override void SetSoloFrets(ref PS4RockBandGuitarState_NoReportId state, FiveFret frets)
            => PS4RockBandGuitarHandling.SetFrets(ref state.soloFrets, frets);

        protected override void SetTilt(ref PS4RockBandGuitarState_NoReportId state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetWhammy(ref PS4RockBandGuitarState_NoReportId state, float value)
        {
            state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetPickupSwitch(ref PS4RockBandGuitarState_NoReportId state, int value)
        {
            state.pickupSwitch = (byte)value;
        }
    }

    internal class SantrollerHIDRockBandGuitarTests
        : RockBandGuitarTests_SoloDistinct<SantrollerHIDRockBandGuitar, SantrollerHIDRockBandGuitarState>
    {
        protected override AxisMode tiltMode => AxisMode.Signed;

        protected override SantrollerHIDRockBandGuitarState CreateState()
            => new SantrollerHIDRockBandGuitarState()
        {
            reportId = 1,
            dpad = 8,
            tilt = 0x80,
        };

        protected override void SetDpad(ref SantrollerHIDRockBandGuitarState state, DpadDirection dpad)
        {
            state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref SantrollerHIDRockBandGuitarState state, MenuButton buttons)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.Start, (buttons & MenuButton.Start) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Select, (buttons & MenuButton.Select) != 0);
        }

        protected override void SetFrets(ref SantrollerHIDRockBandGuitarState state, FiveFret frets)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.Green, (frets & FiveFret.Green) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Red, (frets & FiveFret.Red) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Yellow, (frets & FiveFret.Yellow) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Blue, (frets & FiveFret.Blue) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Orange, (frets & FiveFret.Orange) != 0);
        }

        protected override void SetSoloFrets(ref SantrollerHIDRockBandGuitarState state, FiveFret frets)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.SoloGreen, (frets & FiveFret.Green) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.SoloRed, (frets & FiveFret.Red) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.SoloYellow, (frets & FiveFret.Yellow) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.SoloBlue, (frets & FiveFret.Blue) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.SoloOrange, (frets & FiveFret.Orange) != 0);
        }

        protected override void SetTilt(ref SantrollerHIDRockBandGuitarState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeByteSigned(value);
        }

        protected override void SetWhammy(ref SantrollerHIDRockBandGuitarState state, float value)
        {
            state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetPickupSwitch(ref SantrollerHIDRockBandGuitarState state, int value)
        {
            // Santroller guitars don't do the neutral state deal that PS3/Wii Rock Band guitars do,
            // so the XInput conversion is used here instead
            state.pickupSwitch = XInputRockBandGuitarHandling.GetPickupSwitch(value);
        }
    }

    public static class XInputRockBandGuitarHandling
    {
        public static void SetSoloFrets(ref ushort buttonField, FiveFret frets)
        {
            buttonField.SetBit((ushort)XInputButton.LeftThumb, frets != FiveFret.None);
            XInputFiveFretGuitarHandling.SetFrets(ref buttonField, frets);
        }

        public static byte GetPickupSwitch(int value)
        {
            return DeviceHandling.DenormalizeByteUnsigned((value + 0.5f) / 5f);
        }
    }

    public static class PS3RockBandGuitarHandling
    {
        public static void SetFrets(ref ushort buttonField, FiveFret frets)
        {
            buttonField.SetBit((ushort)PS3Button.Cross, (frets & FiveFret.Green) != 0);
            buttonField.SetBit((ushort)PS3Button.Circle, (frets & FiveFret.Red) != 0);
            buttonField.SetBit((ushort)PS3Button.Triangle, (frets & FiveFret.Yellow) != 0);
            buttonField.SetBit((ushort)PS3Button.Square, (frets & FiveFret.Blue) != 0);
            buttonField.SetBit((ushort)PS3Button.L2, (frets & FiveFret.Orange) != 0);
        }

        public static void SetSoloFrets(ref ushort buttonField, FiveFret frets)
        {
            buttonField.SetBit((ushort)PS3Button.L1, frets != FiveFret.None);
            SetFrets(ref buttonField, frets);
        }

        public static byte GetWhammy(float value)
        {
            byte whammy = DeviceHandling.DenormalizeByteUnsigned(value);

            // PS3/Wii Rock Band guitars reset the whammy to a neutral value
            // after some time of no movement, need to avoid setting that value
            if (whammy == PS3DeviceState.StickCenter)
                whammy--;

            return whammy;
        }

        public static byte GetPickupSwitch(int value)
        {
            byte pickup = DeviceHandling.DenormalizeByteUnsigned((value + 0.5f) / 5f);

            // PS3/Wii Rock Band guitars reset the pickup switch to a neutral value
            // after some time of no movement, need to avoid setting that value
            if (pickup == RockBandPickupSwitchControl.kNullValue)
                pickup--;

            return pickup;
        }
    }

    public static class PS4RockBandGuitarHandling
    {
        public static void SetFrets(ref byte buttonField, FiveFret frets)
        {
            buttonField.SetBit(0x01, (frets & FiveFret.Green) != 0);
            buttonField.SetBit(0x02, (frets & FiveFret.Red) != 0);
            buttonField.SetBit(0x04, (frets & FiveFret.Yellow) != 0);
            buttonField.SetBit(0x08, (frets & FiveFret.Blue) != 0);
            buttonField.SetBit(0x10, (frets & FiveFret.Orange) != 0);
        }
    }
}