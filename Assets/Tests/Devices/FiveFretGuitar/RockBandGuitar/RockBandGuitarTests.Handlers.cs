using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    using SantrollerHIDButton = SantrollerHIDRockBandGuitarState.Button;

    public partial class RockBandGuitarTests
    {
        private class XInputRockBandGuitarHandler : RockBandGuitarHandler<XInputRockBandGuitarState>
        {
            public XInputRockBandGuitarHandler(XInputRockBandGuitar guitar)
                : base(guitar) { }

            public override XInputRockBandGuitarState CreateState()
                => new XInputRockBandGuitarState()
            {
                whammy = short.MinValue,
            };

            public override void SetDpad(ref XInputRockBandGuitarState state, DpadDirection dpad)
                => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

            public override void SetFaceButtons(ref XInputRockBandGuitarState state, FaceButton buttons)
                => XInputDeviceHandling.SetMenuButtonsOnly(ref state.buttons, buttons);

            public override void SetFrets(ref XInputRockBandGuitarState state, FiveFret frets)
                => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

            public override void SetSoloFrets(ref XInputRockBandGuitarState state, FiveFret frets)
                => XInputRockBandGuitarHandling.SetSoloFrets(ref state.buttons, frets);

            public override void SetWhammy(ref XInputRockBandGuitarState state, float value)
            {
                state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
            }

            public override void SetTilt(ref XInputRockBandGuitarState state, float value)
            {
                state.tilt = DeviceHandling.DenormalizeInt16(value);
            }

            public override void SetPickupSwitch(ref XInputRockBandGuitarState state, int value)
            {
                state.pickupSwitch = XInputRockBandGuitarHandling.GetPickupSwitch(value);
            }
        }

        private class PS3RockBandGuitarHandler : RockBandGuitarHandler<PS3RockBandGuitarState_NoReportId>
        {
            public PS3RockBandGuitarHandler(PS3RockBandGuitar guitar)
                : base(guitar) { }

            public override PS3RockBandGuitarState_NoReportId CreateState()
                => new PS3RockBandGuitarState_NoReportId()
            {
                dpad = 8,
            };

            public override void SetDpad(ref PS3RockBandGuitarState_NoReportId state, DpadDirection dpad)
            {
                state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref PS3RockBandGuitarState_NoReportId state, FaceButton buttons)
                => PS3DeviceHandling.SetMenuButtonsOnly(ref state.buttons, buttons);

            public override void SetFrets(ref PS3RockBandGuitarState_NoReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetFrets(ref state.buttons, frets);

            public override void SetSoloFrets(ref PS3RockBandGuitarState_NoReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetSoloFrets(ref state.buttons, frets);

            public override void SetTilt(ref PS3RockBandGuitarState_NoReportId state, float value)
            {
                state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
            }

            public override void SetWhammy(ref PS3RockBandGuitarState_NoReportId state, float value)
            {
                state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
            }

            public override void SetPickupSwitch(ref PS3RockBandGuitarState_NoReportId state, int value)
            {
                state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
            }
        }

        private class PS3RockBandGuitar_ReportIdHandler : RockBandGuitarHandler<PS3RockBandGuitarState_ReportId>
        {
            public PS3RockBandGuitar_ReportIdHandler(PS3RockBandGuitar_ReportId guitar)
                : base(guitar) { }

            public override PS3RockBandGuitarState_ReportId CreateState()
                => new PS3RockBandGuitarState_ReportId()
            {
                state = new PS3RockBandGuitarState_NoReportId()
                {
                    dpad = 8,
                }
            };

            public override void SetDpad(ref PS3RockBandGuitarState_ReportId state, DpadDirection dpad)
            {
                state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref PS3RockBandGuitarState_ReportId state, FaceButton buttons)
                => PS3DeviceHandling.SetMenuButtonsOnly(ref state.state.buttons, buttons);

            public override void SetFrets(ref PS3RockBandGuitarState_ReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetFrets(ref state.state.buttons, frets);

            public override void SetSoloFrets(ref PS3RockBandGuitarState_ReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetSoloFrets(ref state.state.buttons, frets);

            public override void SetTilt(ref PS3RockBandGuitarState_ReportId state, float value)
            {
                state.state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
            }

            public override void SetWhammy(ref PS3RockBandGuitarState_ReportId state, float value)
            {
                state.state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
            }

            public override void SetPickupSwitch(ref PS3RockBandGuitarState_ReportId state, int value)
            {
                state.state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
            }
        }

        private class WiiRockBandGuitarHandler : RockBandGuitarHandler<WiiRockBandGuitarState_NoReportId>
        {
            public WiiRockBandGuitarHandler(WiiRockBandGuitar guitar)
                : base(guitar) { }

            public override WiiRockBandGuitarState_NoReportId CreateState()
                => new WiiRockBandGuitarState_NoReportId()
            {
                state = new PS3RockBandGuitarState_NoReportId()
                {
                    dpad = 8,
                }
            };

            public override void SetDpad(ref WiiRockBandGuitarState_NoReportId state, DpadDirection dpad)
            {
                state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref WiiRockBandGuitarState_NoReportId state, FaceButton buttons)
                => PS3DeviceHandling.SetMenuButtonsOnly(ref state.state.buttons, buttons);

            public override void SetFrets(ref WiiRockBandGuitarState_NoReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetFrets(ref state.state.buttons, frets);

            public override void SetSoloFrets(ref WiiRockBandGuitarState_NoReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetSoloFrets(ref state.state.buttons, frets);

            public override void SetTilt(ref WiiRockBandGuitarState_NoReportId state, float value)
            {
                state.state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
            }

            public override void SetWhammy(ref WiiRockBandGuitarState_NoReportId state, float value)
            {
                state.state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
            }

            public override void SetPickupSwitch(ref WiiRockBandGuitarState_NoReportId state, int value)
            {
                state.state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
            }
        }

        private class WiiRockBandGuitar_ReportIdHandler : RockBandGuitarHandler<WiiRockBandGuitarState_ReportId>
        {
            public WiiRockBandGuitar_ReportIdHandler(WiiRockBandGuitar_ReportId guitar)
                : base(guitar) { }

            public override WiiRockBandGuitarState_ReportId CreateState()
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

            public override void SetDpad(ref WiiRockBandGuitarState_ReportId state, DpadDirection dpad)
            {
                state.state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref WiiRockBandGuitarState_ReportId state, FaceButton buttons)
                => PS3DeviceHandling.SetMenuButtonsOnly(ref state.state.state.buttons, buttons);

            public override void SetFrets(ref WiiRockBandGuitarState_ReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetFrets(ref state.state.state.buttons, frets);

            public override void SetSoloFrets(ref WiiRockBandGuitarState_ReportId state, FiveFret frets)
                => PS3RockBandGuitarHandling.SetSoloFrets(ref state.state.state.buttons, frets);

            public override void SetTilt(ref WiiRockBandGuitarState_ReportId state, float value)
            {
                state.state.state.buttons.SetBit((ushort)PS3Button.R2, value >= 0.5f);
            }

            public override void SetWhammy(ref WiiRockBandGuitarState_ReportId state, float value)
            {
                state.state.state.whammy = PS3RockBandGuitarHandling.GetWhammy(value);
            }

            public override void SetPickupSwitch(ref WiiRockBandGuitarState_ReportId state, int value)
            {
                state.state.state.pickupSwitch = PS3RockBandGuitarHandling.GetPickupSwitch(value);
            }
        }

        private class PS4RockBandGuitarHandler : RockBandGuitarHandler<PS4RockBandGuitarState_ReportId>
        {
            public PS4RockBandGuitarHandler(PS4RockBandGuitar guitar)
                : base(guitar, tiltMode: AxisMode.Unsigned) { }

            public override PS4RockBandGuitarState_ReportId CreateState()
                => new PS4RockBandGuitarState_ReportId()
            {
                state = new PS4RockBandGuitarState_NoReportId()
                {
                    buttons1 = (ushort)HidDpad.Neutral.AsPS4Buttons(),
                }
            };

            public override void SetDpad(ref PS4RockBandGuitarState_ReportId state, DpadDirection dpad)
            {
                PS4DeviceHandling.SetDpad(ref state.state.buttons1, dpad);
            }

            public override void SetFaceButtons(ref PS4RockBandGuitarState_ReportId state, FaceButton buttons)
                => PS4DeviceHandling.SetMenuButtonsOnly(ref state.state.buttons1, buttons);

            public override void SetFrets(ref PS4RockBandGuitarState_ReportId state, FiveFret frets)
                => PS4RockBandGuitarHandling.SetFrets(ref state.state.buttons1, frets);

            public override void SetSoloFrets(ref PS4RockBandGuitarState_ReportId state, FiveFret frets)
                => PS4RockBandGuitarHandling.SetSoloFrets(ref state.state.buttons1, frets);

            public override void SetTilt(ref PS4RockBandGuitarState_ReportId state, float value)
            {
                state.state.tilt = DeviceHandling.DenormalizeByteUnsigned(value);
            }

            public override void SetWhammy(ref PS4RockBandGuitarState_ReportId state, float value)
            {
                state.state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
            }

            public override void SetPickupSwitch(ref PS4RockBandGuitarState_ReportId state, int value)
            {
                state.state.pickupSwitch = (byte)value;
            }
        }

        private class PS4RockBandGuitar_NoReportIdHandler : RockBandGuitarHandler<PS4RockBandGuitarState_NoReportId>
        {
            public PS4RockBandGuitar_NoReportIdHandler(PS4RockBandGuitar_NoReportId guitar)
                : base(guitar, tiltMode: AxisMode.Unsigned) { }

            public override PS4RockBandGuitarState_NoReportId CreateState()
                => new PS4RockBandGuitarState_NoReportId()
            {
                buttons1 = (ushort)HidDpad.Neutral.AsPS4Buttons(),
            };

            public override void SetDpad(ref PS4RockBandGuitarState_NoReportId state, DpadDirection dpad)
            {
                PS4DeviceHandling.SetDpad(ref state.buttons1, dpad);
            }

            public override void SetFaceButtons(ref PS4RockBandGuitarState_NoReportId state, FaceButton buttons)
                => PS4DeviceHandling.SetMenuButtonsOnly(ref state.buttons1, buttons);

            public override void SetFrets(ref PS4RockBandGuitarState_NoReportId state, FiveFret frets)
                => PS4RockBandGuitarHandling.SetFrets(ref state.buttons1, frets);

            public override void SetSoloFrets(ref PS4RockBandGuitarState_NoReportId state, FiveFret frets)
                => PS4RockBandGuitarHandling.SetSoloFrets(ref state.buttons1, frets);

            public override void SetTilt(ref PS4RockBandGuitarState_NoReportId state, float value)
            {
                state.tilt = DeviceHandling.DenormalizeByteUnsigned(value);
            }

            public override void SetWhammy(ref PS4RockBandGuitarState_NoReportId state, float value)
            {
                state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
            }

            public override void SetPickupSwitch(ref PS4RockBandGuitarState_NoReportId state, int value)
            {
                state.pickupSwitch = (byte)value;
            }
        }

        private class SantrollerHIDRockBandGuitarHandler : RockBandGuitarHandler<SantrollerHIDRockBandGuitarState>
        {
            public SantrollerHIDRockBandGuitarHandler(SantrollerHIDRockBandGuitar guitar)
                : base(guitar, tiltMode: AxisMode.Signed) { }

            public override SantrollerHIDRockBandGuitarState CreateState()
                => new SantrollerHIDRockBandGuitarState()
            {
                reportId = 1,
                dpad = 8,
                tilt = 0x80,
            };

            public override void SetDpad(ref SantrollerHIDRockBandGuitarState state, DpadDirection dpad)
            {
                state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref SantrollerHIDRockBandGuitarState state, FaceButton buttons)
            {
                state.buttons.SetBit((ushort)SantrollerHIDButton.Start, (buttons & FaceButton.Start) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Select, (buttons & FaceButton.Select) != 0);
            }

            public override void SetFrets(ref SantrollerHIDRockBandGuitarState state, FiveFret frets)
            {
                state.buttons.SetBit((ushort)SantrollerHIDButton.Green, (frets & FiveFret.Green) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Red, (frets & FiveFret.Red) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Yellow, (frets & FiveFret.Yellow) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Blue, (frets & FiveFret.Blue) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Orange, (frets & FiveFret.Orange) != 0);
            }

            public override void SetSoloFrets(ref SantrollerHIDRockBandGuitarState state, FiveFret frets)
            {
                state.buttons.SetBit((ushort)SantrollerHIDButton.SoloGreen, (frets & FiveFret.Green) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.SoloRed, (frets & FiveFret.Red) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.SoloYellow, (frets & FiveFret.Yellow) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.SoloBlue, (frets & FiveFret.Blue) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.SoloOrange, (frets & FiveFret.Orange) != 0);
            }

            public override void SetTilt(ref SantrollerHIDRockBandGuitarState state, float value)
            {
                state.tilt = DeviceHandling.DenormalizeByteSigned(value);
            }

            public override void SetWhammy(ref SantrollerHIDRockBandGuitarState state, float value)
            {
                state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
            }

            public override void SetPickupSwitch(ref SantrollerHIDRockBandGuitarState state, int value)
            {
                // Santroller guitars don't do the neutral state deal that PS3/Wii Rock Band guitars do,
                // so the XInput conversion is used here instead
                state.pickupSwitch = XInputRockBandGuitarHandling.GetPickupSwitch(value);
            }
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
        public static void SetFrets(ref ushort buttonField, FiveFret frets)
        {
            buttonField.SetBit((ushort)PS4Button1.Cross, (frets & FiveFret.Green) != 0);
            buttonField.SetBit((ushort)PS4Button1.Circle, (frets & FiveFret.Red) != 0);
            buttonField.SetBit((ushort)PS4Button1.Triangle, (frets & FiveFret.Yellow) != 0);
            buttonField.SetBit((ushort)PS4Button1.Square, (frets & FiveFret.Blue) != 0);
            buttonField.SetBit((ushort)PS4Button1.L2, (frets & FiveFret.Orange) != 0);
        }

        public static void SetSoloFrets(ref ushort buttonField, FiveFret frets)
        {
            buttonField.SetBit((ushort)PS4Button1.L3, frets != FiveFret.None);
            SetFrets(ref buttonField, frets);
        }
    }
}