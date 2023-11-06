using PlasticBand.LowLevel;

namespace PlasticBand.Tests
{
    public static class Extensions
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
    }
}