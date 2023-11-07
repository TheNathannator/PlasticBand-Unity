using PlasticBand.Controls;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public static class DeviceHandling
    {
        public static byte DenormalizeByteSigned(float value)
        {
            return (byte)IntegerAxisControl.Denormalize(value, byte.MinValue, byte.MaxValue, 0x80);
        }

        public static byte DenormalizeByteUnsigned(float value)
        {
            return (byte)IntegerAxisControl.Denormalize(value, byte.MinValue, byte.MaxValue, byte.MinValue);
        }

        public static short DenormalizeInt16(float value)
        {
            return (short)IntegerAxisControl.Denormalize(value, short.MinValue, short.MaxValue, 0);
        }
    }

    public static class XInputDeviceHandling
    {
        public static void SetDpad(ref ushort buttonsField, DpadDirection dpad)
        {
            buttonsField.SetBit((ushort)XInputButton.DpadUp, dpad.IsUp());
            buttonsField.SetBit((ushort)XInputButton.DpadDown, dpad.IsDown());
            buttonsField.SetBit((ushort)XInputButton.DpadLeft, dpad.IsLeft());
            buttonsField.SetBit((ushort)XInputButton.DpadRight, dpad.IsRight());
        }

        public static void SetFaceButtons(ref ushort buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit((ushort)XInputButton.A, (buttons & FaceButton.South) != 0);
            buttonsField.SetBit((ushort)XInputButton.B, (buttons & FaceButton.East) != 0);
            buttonsField.SetBit((ushort)XInputButton.X, (buttons & FaceButton.West) != 0);
            buttonsField.SetBit((ushort)XInputButton.Y, (buttons & FaceButton.North) != 0);

            SetMenuButtonsOnly(ref buttonsField, buttons);
        }

        public static void SetMenuButtonsOnly(ref ushort buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit((ushort)XInputButton.Start, (buttons & FaceButton.Start) != 0);
            buttonsField.SetBit((ushort)XInputButton.Back, (buttons & FaceButton.Select) != 0);
        }
    }

    public static class HidDeviceHandling
    {
        public static byte GetDpadByte(DpadDirection dpad) => (byte)dpad;
    }

    public static class PS3DeviceHandling
    {
        public static void SetFaceButtons(ref ushort buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit((ushort)PS3Button.Cross, (buttons & FaceButton.South) != 0);
            buttonsField.SetBit((ushort)PS3Button.Circle, (buttons & FaceButton.East) != 0);
            buttonsField.SetBit((ushort)PS3Button.Square, (buttons & FaceButton.West) != 0);
            buttonsField.SetBit((ushort)PS3Button.Triangle, (buttons & FaceButton.North) != 0);

            SetMenuButtonsOnly(ref buttonsField, buttons);
        }

        public static void SetMenuButtonsOnly(ref ushort buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit((ushort)PS3Button.Start, (buttons & FaceButton.Start) != 0);
            buttonsField.SetBit((ushort)PS3Button.Select, (buttons & FaceButton.Select) != 0);
        }

        public static short DenormalizeAccelerometer(float value)
        {
            return (short)IntegerAxisControl.Denormalize(value, 0, 0x3FF, 0x200);
        }
    }

    public static class PS4DeviceHandling
    {
        public static void SetDpad(ref ushort buttonsField, DpadDirection dpad)
        {
            var buttons = (PS4Button1)buttonsField;
            buttons.SetDpad(dpad.ToHidDpad());
            buttonsField = (ushort)buttons;
        }

        public static void SetFaceButtons(ref ushort buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit((ushort)PS4Button1.Cross, (buttons & FaceButton.South) != 0);
            buttonsField.SetBit((ushort)PS4Button1.Circle, (buttons & FaceButton.East) != 0);
            buttonsField.SetBit((ushort)PS4Button1.Square, (buttons & FaceButton.West) != 0);
            buttonsField.SetBit((ushort)PS4Button1.Triangle, (buttons & FaceButton.North) != 0);

            SetMenuButtonsOnly(ref buttonsField, buttons);
        }

        public static void SetMenuButtonsOnly(ref ushort buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit((ushort)PS4Button1.Start, (buttons & FaceButton.Start) != 0);
            buttonsField.SetBit((ushort)PS4Button1.Select, (buttons & FaceButton.Select) != 0);
        }
    }
}