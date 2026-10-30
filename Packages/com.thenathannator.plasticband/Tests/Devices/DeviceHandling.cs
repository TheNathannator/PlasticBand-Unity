using PlasticBand.Controls;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public static class DeviceHandling
    {
        public static bool IsUp(this DpadDirection dpad)
            => dpad == DpadDirection.UpLeft || dpad == DpadDirection.Up || dpad == DpadDirection.UpRight;
        public static bool IsRight(this DpadDirection dpad)
            => dpad == DpadDirection.UpRight || dpad == DpadDirection.Right || dpad == DpadDirection.DownRight;
        public static bool IsDown(this DpadDirection dpad)
            => dpad == DpadDirection.DownLeft || dpad == DpadDirection.Down || dpad == DpadDirection.DownRight;
        public static bool IsLeft(this DpadDirection dpad)
            => dpad == DpadDirection.UpLeft || dpad == DpadDirection.Left || dpad == DpadDirection.DownLeft;

        internal static HidDpad ToHidDpad(this DpadDirection dpad)
            // DpadDirection is equivalent to HidDpad
            => (HidDpad)dpad;

        public static byte DenormalizeByteSigned(float value)
        {
            return (byte)IntegerAxisControl.Denormalize(value, byte.MinValue, byte.MaxValue, 0x80);
        }

        public static byte DenormalizeByteUnsigned(float value)
        {
            return (byte)IntegerAxisControl.Denormalize(value, byte.MinValue, byte.MaxValue, byte.MinValue);
        }

        public static sbyte DenormalizeSByte(float value)
        {
            return (sbyte)IntegerAxisControl.Denormalize(value, sbyte.MinValue, sbyte.MaxValue, 0);
        }

        public static short DenormalizeInt16(float value)
        {
            return (short)IntegerAxisControl.Denormalize(value, short.MinValue, short.MaxValue, 0);
        }

        public static ushort DenormalizeUInt16(float value)
        {
            return (ushort)IntegerAxisControl.Denormalize(value, ushort.MinValue, ushort.MaxValue, ushort.MinValue);
        }

        public static int CreateMask(int start, int end)
        {
            int mask = 0;
            for (int i = start; i < end; i++)
                mask |= 1 << i;
            return mask;
        }
    }
}