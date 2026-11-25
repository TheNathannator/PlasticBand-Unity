#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;

namespace PlasticBand.LowLevel
{
    internal static class EvdevExtensions
    {
        public static EvdevDeviceMatcher WithXusbSubtype(
            this EvdevDeviceMatcher matcher,
            XInputDeviceSubType xusbSubtype
        )
        {
            return matcher.WithXusbSubtype((byte)xusbSubtype);
        }

        public static EvdevDeviceMatcher WithSantrollerSubtype(
            this EvdevDeviceMatcher matcher,
            SantrollerDeviceType santrollerSubtype
        )
        {
            return matcher.WithSantrollerSubtype((byte)santrollerSubtype);
        }

        public static EvdevDeviceMatcher WithSantrollerCapabilities(
            this EvdevDeviceMatcher matcher,
            SantrollerCapabilities santrollerCapabilities
        )
        {
            return matcher.WithSantrollerCapabilities((byte)santrollerCapabilities);
        }
    }
}
#endif
