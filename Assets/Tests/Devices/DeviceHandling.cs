using PlasticBand.Controls;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public static class DeviceHandling
    {
        public static bool IsUp(this DpadDirection dpad)
            => dpad == DpadDirection.UpLeft || dpad <= DpadDirection.UpRight;
        public static bool IsRight(this DpadDirection dpad)
            => dpad >= DpadDirection.UpRight && dpad <= DpadDirection.DownRight;
        public static bool IsDown(this DpadDirection dpad)
            => dpad >= DpadDirection.DownRight && dpad <= DpadDirection.DownLeft;
        public static bool IsLeft(this DpadDirection dpad)
            => dpad >= DpadDirection.DownLeft && dpad <= DpadDirection.UpLeft;

        internal static HidDpad ToHidDpad(this DpadDirection dpad)
            // DpadDirection is equivalent to HidDpad
            => (HidDpad)dpad;

        public static void SetBit(ref this ushort value, ushort mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= (ushort)~mask;
        }

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
            var xButtons = (XInputButton)buttonsField;
            SetDpad(ref xButtons, dpad);
            buttonsField = (ushort)xButtons;
        }

        internal static void SetDpad(ref XInputButton buttonsField, DpadDirection dpad)
        {
            buttonsField.SetBit(XInputButton.DpadUp, dpad.IsUp());
            buttonsField.SetBit(XInputButton.DpadDown, dpad.IsDown());
            buttonsField.SetBit(XInputButton.DpadLeft, dpad.IsLeft());
            buttonsField.SetBit(XInputButton.DpadRight, dpad.IsRight());
        }

        public static void SetFaceButtons(ref ushort buttonsField, FaceButton buttons)
        {
            var xButtons = (XInputButton)buttonsField;
            SetFaceButtons(ref xButtons, buttons);
            buttonsField = (ushort)xButtons;
        }

        internal static void SetFaceButtons(ref XInputButton buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit(XInputButton.A, (buttons & FaceButton.South) != 0);
            buttonsField.SetBit(XInputButton.B, (buttons & FaceButton.East) != 0);
            buttonsField.SetBit(XInputButton.X, (buttons & FaceButton.West) != 0);
            buttonsField.SetBit(XInputButton.Y, (buttons & FaceButton.North) != 0);
        }

        public static void SetMenuButtons(ref ushort buttonsField, MenuButton buttons)
        {
            var xButtons = (XInputButton)buttonsField;
            SetMenuButtons(ref xButtons, buttons);
            buttonsField = (ushort)xButtons;
        }

        internal static void SetMenuButtons(ref XInputButton buttonsField, MenuButton buttons)
        {
            buttonsField.SetBit(XInputButton.Start, (buttons & MenuButton.Start) != 0);
            buttonsField.SetBit(XInputButton.Back, (buttons & MenuButton.Select) != 0);
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
            var psButtons = (PS3Button)buttonsField;
            SetFaceButtons(ref psButtons, buttons);
            buttonsField = (ushort)psButtons;
        }

        internal static void SetFaceButtons(ref PS3Button buttonsField, FaceButton buttons)
        {
            buttonsField.SetBit(PS3Button.Cross, (buttons & FaceButton.South) != 0);
            buttonsField.SetBit(PS3Button.Circle, (buttons & FaceButton.East) != 0);
            buttonsField.SetBit(PS3Button.Square, (buttons & FaceButton.West) != 0);
            buttonsField.SetBit(PS3Button.Triangle, (buttons & FaceButton.North) != 0);
        }

        public static void SetMenuButtons(ref ushort buttonsField, MenuButton buttons)
        {
            var psButtons = (PS3Button)buttonsField;
            SetMenuButtons(ref psButtons, buttons);
            buttonsField = (ushort)psButtons;
        }

        internal static void SetMenuButtons(ref PS3Button buttonsField, MenuButton buttons)
        {
            buttonsField.SetBit(PS3Button.Start, (buttons & MenuButton.Start) != 0);
            buttonsField.SetBit(PS3Button.Select, (buttons & MenuButton.Select) != 0);
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
        }

        public static void SetMenuButtons(ref ushort buttonsField, MenuButton buttons)
        {
            buttonsField.SetBit((ushort)PS4Button1.Start, (buttons & MenuButton.Start) != 0);
            buttonsField.SetBit((ushort)PS4Button1.Select, (buttons & MenuButton.Select) != 0);
        }
    }
}