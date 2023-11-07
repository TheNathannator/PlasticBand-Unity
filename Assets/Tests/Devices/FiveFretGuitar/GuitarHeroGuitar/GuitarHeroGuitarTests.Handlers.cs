using System;
using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;

namespace PlasticBand.Tests.Devices
{
    using SantrollerHIDButton = SantrollerHIDGuitarHeroGuitarState.Button;

    public partial class GuitarHeroGuitarTests
    {
        private class XInputGuitarHeroGuitarHandler : GuitarHeroGuitarHandler<XInputGuitarHeroGuitarState>
        {
            public XInputGuitarHeroGuitarHandler(XInputGuitarHeroGuitar guitar) 
                : base(guitar) { }

            public override XInputGuitarHeroGuitarState CreateState()
                => new XInputGuitarHeroGuitarState()
            {
                whammy = short.MinValue,
                accelY = 0x80,
                accelZ = 0x80,
            };

            public override void SetDpad(ref XInputGuitarHeroGuitarState state, DpadDirection dpad)
                => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

            public override void SetFaceButtons(ref XInputGuitarHeroGuitarState state, FaceButton buttons)
                => XInputDeviceHandling.SetMenuButtonsOnly(ref state.buttons, buttons);

            public override void SetFrets(ref XInputGuitarHeroGuitarState state, FiveFret frets)
                => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

            public override void SetTilt(ref XInputGuitarHeroGuitarState state, float value)
            {
                state.tilt = DeviceHandling.DenormalizeInt16(value);
            }

            public override void SetWhammy(ref XInputGuitarHeroGuitarState state, float value)
            {
                state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
            }

            public override void SetAccelerometerX(ref XInputGuitarHeroGuitarState state, float value)
                => SetTilt(ref state, value);

            public override void SetAccelerometerY(ref XInputGuitarHeroGuitarState state, float value)
            {
                state.accelY = DeviceHandling.DenormalizeByteSigned(value);
            }

            public override void SetAccelerometerZ(ref XInputGuitarHeroGuitarState state, float value)
            {
                state.accelZ = DeviceHandling.DenormalizeByteSigned(value);
            }
        }

        private class SantrollerXInputGuitarHeroGuitarHandler : GuitarHeroGuitarHandler<SantrollerXInputGuitarHeroGuitarState>
        {
            public SantrollerXInputGuitarHeroGuitarHandler(SantrollerXInputGuitarHeroGuitar guitar)
                : base(guitar, accelerometers: false) { }

            public override SantrollerXInputGuitarHeroGuitarState CreateState()
                => new SantrollerXInputGuitarHeroGuitarState()
            {
                whammy = short.MinValue,
                accelY = 0x80,
                accelZ = 0x80,
            };

            public override void SetDpad(ref SantrollerXInputGuitarHeroGuitarState state, DpadDirection dpad)
                => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

            public override void SetFaceButtons(ref SantrollerXInputGuitarHeroGuitarState state, FaceButton buttons)
                => XInputDeviceHandling.SetMenuButtonsOnly(ref state.buttons, buttons);

            public override void SetFrets(ref SantrollerXInputGuitarHeroGuitarState state, FiveFret frets)
                => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

            public override void SetTilt(ref SantrollerXInputGuitarHeroGuitarState state, float value)
            {
                state.tilt = DeviceHandling.DenormalizeInt16(value);
            }

            public override void SetWhammy(ref SantrollerXInputGuitarHeroGuitarState state, float value)
            {
                state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
            }

            public override void SetAccelerometerX(ref SantrollerXInputGuitarHeroGuitarState state, float value)
                => throw new NotSupportedException($"{nameof(SantrollerXInputGuitarHeroGuitar)} does not support the accelerometer axes!");

            public override void SetAccelerometerY(ref SantrollerXInputGuitarHeroGuitarState state, float value)
                => throw new NotSupportedException($"{nameof(SantrollerXInputGuitarHeroGuitar)} does not support the accelerometer axes!");

            public override void SetAccelerometerZ(ref SantrollerXInputGuitarHeroGuitarState state, float value)
                => throw new NotSupportedException($"{nameof(SantrollerXInputGuitarHeroGuitar)} does not support the accelerometer axes!");
        }

        private class PS3GuitarHeroGuitarHandler : GuitarHeroGuitarHandler<PS3GuitarHeroGuitarState_NoReportId>
        {
            public PS3GuitarHeroGuitarHandler(PS3GuitarHeroGuitar guitar) 
                : base(guitar) { }

            public override PS3GuitarHeroGuitarState_NoReportId CreateState()
                => new PS3GuitarHeroGuitarState_NoReportId()
            {
                dpad = 8,
                tilt = 0x200,
                accelY = 0x200,
                accelZ = 0x200,
            };

            public override void SetDpad(ref PS3GuitarHeroGuitarState_NoReportId state, DpadDirection dpad)
            {
                state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref PS3GuitarHeroGuitarState_NoReportId state, FaceButton buttons)
                => PS3DeviceHandling.SetMenuButtonsOnly(ref state.buttons, buttons);

            public override void SetFrets(ref PS3GuitarHeroGuitarState_NoReportId state, FiveFret frets)
                => PS3GuitarHeroGuitarHandling.SetFrets(ref state.buttons, frets);

            public override void SetTilt(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
            {
                state.tilt = PS3DeviceHandling.DenormalizeAccelerometer(value);
            }

            public override void SetWhammy(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
            {
                state.whammy = PS3GuitarHeroGuitarHandling.GetWhammy(value);
            }

            public override void SetAccelerometerX(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
                => SetTilt(ref state, value);

            public override void SetAccelerometerY(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
            {
                state.accelY = PS3DeviceHandling.DenormalizeAccelerometer(value);
            }

            public override void SetAccelerometerZ(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
            {
                state.accelZ = PS3DeviceHandling.DenormalizeAccelerometer(value);
            }
        }

        private class PS3GuitarHeroGuitar_ReportIdHandler : GuitarHeroGuitarHandler<PS3GuitarHeroGuitarState_ReportId>
        {
            public PS3GuitarHeroGuitar_ReportIdHandler(PS3GuitarHeroGuitar_ReportId guitar) 
                : base(guitar) { }

            public override PS3GuitarHeroGuitarState_ReportId CreateState()
                => new PS3GuitarHeroGuitarState_ReportId()
            {
                state = new PS3GuitarHeroGuitarState_NoReportId()
                {
                    dpad = 8,
                    tilt = 0x200,
                    accelY = 0x200,
                    accelZ = 0x200,
                }
            };

            public override void SetDpad(ref PS3GuitarHeroGuitarState_ReportId state, DpadDirection dpad)
            {
                state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref PS3GuitarHeroGuitarState_ReportId state, FaceButton buttons)
                => PS3DeviceHandling.SetMenuButtonsOnly(ref state.state.buttons, buttons);

            public override void SetFrets(ref PS3GuitarHeroGuitarState_ReportId state, FiveFret frets)
                => PS3GuitarHeroGuitarHandling.SetFrets(ref state.state.buttons, frets);

            public override void SetTilt(ref PS3GuitarHeroGuitarState_ReportId state, float value)
            {
                state.state.tilt = PS3DeviceHandling.DenormalizeAccelerometer(value);
            }

            public override void SetWhammy(ref PS3GuitarHeroGuitarState_ReportId state, float value)
            {
                state.state.whammy = PS3GuitarHeroGuitarHandling.GetWhammy(value);
            }

            public override void SetAccelerometerX(ref PS3GuitarHeroGuitarState_ReportId state, float value)
                => SetTilt(ref state, value);

            public override void SetAccelerometerY(ref PS3GuitarHeroGuitarState_ReportId state, float value)
            {
                state.state.accelY = PS3DeviceHandling.DenormalizeAccelerometer(value);
            }

            public override void SetAccelerometerZ(ref PS3GuitarHeroGuitarState_ReportId state, float value)
            {
                state.state.accelZ = PS3DeviceHandling.DenormalizeAccelerometer(value);
            }
        }

        private class SantrollerHIDGuitarHeroGuitarHandler : GuitarHeroGuitarHandler<SantrollerHIDGuitarHeroGuitarState>
        {
            public SantrollerHIDGuitarHeroGuitarHandler(SantrollerHIDGuitarHeroGuitar guitar) 
                : base(guitar, accelerometers: false) { }

            public override SantrollerHIDGuitarHeroGuitarState CreateState()
                => new SantrollerHIDGuitarHeroGuitarState()
            {
                reportId = 1,
                dpad = 8,
                tilt = 0x80,
            };

            public override void SetDpad(ref SantrollerHIDGuitarHeroGuitarState state, DpadDirection dpad)
            {
                state.dpad = HidDeviceHandling.GetDpadByte(dpad);
            }

            public override void SetFaceButtons(ref SantrollerHIDGuitarHeroGuitarState state, FaceButton buttons)
            {
                state.buttons.SetBit((ushort)SantrollerHIDButton.Start, (buttons & FaceButton.Start) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Select, (buttons & FaceButton.Select) != 0);
            }

            public override void SetFrets(ref SantrollerHIDGuitarHeroGuitarState state, FiveFret frets)
            {
                state.buttons.SetBit((ushort)SantrollerHIDButton.Green, (frets & FiveFret.Green) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Red, (frets & FiveFret.Red) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Yellow, (frets & FiveFret.Yellow) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Blue, (frets & FiveFret.Blue) != 0);
                state.buttons.SetBit((ushort)SantrollerHIDButton.Orange, (frets & FiveFret.Orange) != 0);
            }

            public override void SetTilt(ref SantrollerHIDGuitarHeroGuitarState state, float value)
            {
                state.tilt = DeviceHandling.DenormalizeByteSigned(value);
            }

            public override void SetWhammy(ref SantrollerHIDGuitarHeroGuitarState state, float value)
            {
                state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
            }

            public override void SetAccelerometerX(ref SantrollerHIDGuitarHeroGuitarState state, float value)
                => throw new NotSupportedException($"{nameof(SantrollerHIDGuitarHeroGuitar)} does not support the accelerometer axes!");

            public override void SetAccelerometerY(ref SantrollerHIDGuitarHeroGuitarState state, float value)
                => throw new NotSupportedException($"{nameof(SantrollerHIDGuitarHeroGuitar)} does not support the accelerometer axes!");

            public override void SetAccelerometerZ(ref SantrollerHIDGuitarHeroGuitarState state, float value)
                => throw new NotSupportedException($"{nameof(SantrollerHIDGuitarHeroGuitar)} does not support the accelerometer axes!");
        }
    }

    public static class PS3GuitarHeroGuitarHandling
    {
        public static void SetFrets(ref ushort buttonsField, FiveFret frets)
        {
            buttonsField.SetBit((ushort)PS3Button.Cross, (frets & FiveFret.Green) != 0);
            buttonsField.SetBit((ushort)PS3Button.Circle, (frets & FiveFret.Red) != 0);
            buttonsField.SetBit((ushort)PS3Button.Square, (frets & FiveFret.Yellow) != 0);
            buttonsField.SetBit((ushort)PS3Button.Triangle, (frets & FiveFret.Blue) != 0);
            buttonsField.SetBit((ushort)PS3Button.L2, (frets & FiveFret.Orange) != 0);
        }

        public static byte GetWhammy(float value)
        {
            return (byte)IntegerAxisControl.DenormalizeUnchecked(value, PS3DeviceState.StickCenter, byte.MaxValue, PS3DeviceState.StickCenter);
        }
    }
}