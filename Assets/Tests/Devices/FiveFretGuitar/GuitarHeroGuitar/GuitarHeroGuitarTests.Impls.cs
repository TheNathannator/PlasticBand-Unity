using System;
using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;

namespace PlasticBand.Tests.Devices
{
    using SantrollerHIDButton = SantrollerHIDGuitarHeroGuitarState.Button;

    internal class XInputGuitarHeroGuitarTests
        : GuitarHeroGuitarTests_Accelerometer<XInputGuitarHeroGuitar, XInputGuitarHeroGuitarState>
    {
        protected override XInputGuitarHeroGuitarState CreateState()
            => new XInputGuitarHeroGuitarState()
        {
            whammy = short.MinValue,
            accelY = 0x80,
            accelZ = 0x80,
        };

        protected override void SetDpad(ref XInputGuitarHeroGuitarState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetMenuButtons(ref XInputGuitarHeroGuitarState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetFrets(ref XInputGuitarHeroGuitarState state, FiveFret frets)
            => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

        protected override void SetTilt(ref XInputGuitarHeroGuitarState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeInt16(value);
        }

        protected override void SetWhammy(ref XInputGuitarHeroGuitarState state, float value)
        {
            state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
        }

        protected override void SetSliderValue(ref XInputGuitarHeroGuitarState state, byte value)
        {
            // The Xbox 360 slider value is strange: the value is assigned to both the upper and lower byte
            // of the short, but values of 0x80 or greater result in an upper byte of one less than the lower byte
            state.slider = (short)-((sbyte)value * -0x0101);
        }

        protected override void SetAccelerometerX(ref XInputGuitarHeroGuitarState state, float value)
            => SetTilt(ref state, value);

        protected override void SetAccelerometerY(ref XInputGuitarHeroGuitarState state, float value)
        {
            state.accelY = DeviceHandling.DenormalizeByteSigned(value);
        }

        protected override void SetAccelerometerZ(ref XInputGuitarHeroGuitarState state, float value)
        {
            state.accelZ = DeviceHandling.DenormalizeByteSigned(value);
        }
    }

    internal class SantrollerXInputGuitarHeroGuitarTests
        : GuitarHeroGuitarTests<SantrollerXInputGuitarHeroGuitar, SantrollerXInputGuitarHeroGuitarState>
    {
        protected override SantrollerXInputGuitarHeroGuitarState CreateState()
            => new SantrollerXInputGuitarHeroGuitarState()
        {
            whammy = short.MinValue,
            accelY = 0x80,
            accelZ = 0x80,
        };

        protected override void SetDpad(ref SantrollerXInputGuitarHeroGuitarState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetMenuButtons(ref SantrollerXInputGuitarHeroGuitarState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetFrets(ref SantrollerXInputGuitarHeroGuitarState state, FiveFret frets)
            => XInputFiveFretGuitarHandling.SetFrets(ref state.buttons, frets);

        protected override void SetTilt(ref SantrollerXInputGuitarHeroGuitarState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeInt16(value);
        }

        protected override void SetWhammy(ref SantrollerXInputGuitarHeroGuitarState state, float value)
        {
            state.whammy = XInputFiveFretGuitarHandling.GetWhammy(value);
        }

        protected override void SetSliderValue(ref SantrollerXInputGuitarHeroGuitarState state, byte value)
        {
            state.slider = (short)-((sbyte)value * -0x0101);
        }
    }

    internal class PS3GuitarHeroGuitarTests_NoReportId
        : GuitarHeroGuitarTests_Accelerometer<PS3GuitarHeroGuitar, PS3GuitarHeroGuitarState_NoReportId>
    {
        protected override PS3GuitarHeroGuitarState_NoReportId CreateState()
            => new PS3GuitarHeroGuitarState_NoReportId()
        {
            dpad = 8,
            tilt = 0x200,
            accelY = 0x200,
            accelZ = 0x200,
        };

        protected override void SetDpad(ref PS3GuitarHeroGuitarState_NoReportId state, DpadDirection dpad)
        {
            state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref PS3GuitarHeroGuitarState_NoReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetFrets(ref PS3GuitarHeroGuitarState_NoReportId state, FiveFret frets)
            => PS3GuitarHeroGuitarHandling.SetFrets(ref state.buttons, frets);

        protected override void SetTilt(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
        {
            state.tilt = PS3DeviceHandling.DenormalizeAccelerometer(value);
        }

        protected override void SetWhammy(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
        {
            state.whammy = PS3GuitarHeroGuitarHandling.GetWhammy(value);
        }

        protected override void SetSliderValue(ref PS3GuitarHeroGuitarState_NoReportId state, byte value)
        {
            state.slider = value;
        }

        protected override void SetAccelerometerX(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
            => SetTilt(ref state, value);

        protected override void SetAccelerometerY(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
        {
            state.accelY = PS3DeviceHandling.DenormalizeAccelerometer(value);
        }

        protected override void SetAccelerometerZ(ref PS3GuitarHeroGuitarState_NoReportId state, float value)
        {
            state.accelZ = PS3DeviceHandling.DenormalizeAccelerometer(value);
        }
    }

    internal class PS3GuitarHeroGuitarTests_ReportId
        : GuitarHeroGuitarTests_Accelerometer<PS3GuitarHeroGuitar_ReportId, PS3GuitarHeroGuitarState_ReportId>
    {
        protected override PS3GuitarHeroGuitarState_ReportId CreateState()
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

        protected override void SetDpad(ref PS3GuitarHeroGuitarState_ReportId state, DpadDirection dpad)
        {
            state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref PS3GuitarHeroGuitarState_ReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.buttons, buttons);

        protected override void SetFrets(ref PS3GuitarHeroGuitarState_ReportId state, FiveFret frets)
            => PS3GuitarHeroGuitarHandling.SetFrets(ref state.state.buttons, frets);

        protected override void SetTilt(ref PS3GuitarHeroGuitarState_ReportId state, float value)
        {
            state.state.tilt = PS3DeviceHandling.DenormalizeAccelerometer(value);
        }

        protected override void SetWhammy(ref PS3GuitarHeroGuitarState_ReportId state, float value)
        {
            state.state.whammy = PS3GuitarHeroGuitarHandling.GetWhammy(value);
        }

        protected override void SetSliderValue(ref PS3GuitarHeroGuitarState_ReportId state, byte value)
        {
            state.state.slider = value;
        }

        protected override void SetAccelerometerX(ref PS3GuitarHeroGuitarState_ReportId state, float value)
            => SetTilt(ref state, value);

        protected override void SetAccelerometerY(ref PS3GuitarHeroGuitarState_ReportId state, float value)
        {
            state.state.accelY = PS3DeviceHandling.DenormalizeAccelerometer(value);
        }

        protected override void SetAccelerometerZ(ref PS3GuitarHeroGuitarState_ReportId state, float value)
        {
            state.state.accelZ = PS3DeviceHandling.DenormalizeAccelerometer(value);
        }
    }

    internal class SantrollerHIDGuitarHeroGuitarTests
        : GuitarHeroGuitarTests<SantrollerHIDGuitarHeroGuitar, SantrollerHIDGuitarHeroGuitarState>
    {
        protected override SantrollerHIDGuitarHeroGuitarState CreateState()
            => new SantrollerHIDGuitarHeroGuitarState()
        {
            reportId = 1,
            dpad = 8,
            tilt = 0x80,
        };

        protected override void SetDpad(ref SantrollerHIDGuitarHeroGuitarState state, DpadDirection dpad)
        {
            state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref SantrollerHIDGuitarHeroGuitarState state, MenuButton buttons)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.Start, (buttons & MenuButton.Start) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Select, (buttons & MenuButton.Select) != 0);
        }

        protected override void SetFrets(ref SantrollerHIDGuitarHeroGuitarState state, FiveFret frets)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.Green, (frets & FiveFret.Green) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Red, (frets & FiveFret.Red) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Yellow, (frets & FiveFret.Yellow) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Blue, (frets & FiveFret.Blue) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Orange, (frets & FiveFret.Orange) != 0);
        }

        protected override void SetTilt(ref SantrollerHIDGuitarHeroGuitarState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeByteSigned(value);
        }

        protected override void SetWhammy(ref SantrollerHIDGuitarHeroGuitarState state, float value)
        {
            state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetSliderValue(ref SantrollerHIDGuitarHeroGuitarState state, byte value)
        {
            state.slider = value;
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
            return (byte)IntegerAxisControl.Denormalize(value, PS3DeviceState.StickCenter, byte.MaxValue, PS3DeviceState.StickCenter);
        }
    }
}