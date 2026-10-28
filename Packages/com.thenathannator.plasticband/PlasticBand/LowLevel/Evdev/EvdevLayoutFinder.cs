using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.LowLevel
{
    [Serializable]
    internal class EvdevCapabilities
    {
        public int vendorId;
        public int productId;
        public int subType;
        public int santrollerProtocol;
        public SantrollerCapabilities santrollerFlags;
        public string driver;
        public string keys;
        public string axes;

        public int SonyButtonBase
        {
            get
            {
                byte[] bits = Convert.FromBase64String(keys);
                // Older kernels expose sequential HID buttons; newer hid-sony versions
                // remap instruments to the standard gamepad key codes.
                if (HasBit(bits, 0x132))
                    return 0x130;
                if (HasBit(bits, 0x120))
                    return 0x120;
                if (HasBit(bits, 0x130) && HasBit(bits, 0x134) && HasBit(bits, 0x136))
                    return 0;
                throw new NotSupportedException("Evdev instrument has no supported HID button collection.");
            }
        }

        internal static bool HasBit(byte[] bits, int code)
            => code / 8 < bits.Length && (bits[code / 8] & (1 << (code & 7))) != 0;

        public void RequireAxes(params int[] codes)
        {
            byte[] bits = Convert.FromBase64String(axes);
            foreach (int code in codes)
            {
                if (!HasBit(bits, code))
                    throw new NotSupportedException($"Evdev instrument is missing required ABS code {code}.");
            }
        }

        public void RequireKeys(params int[] codes)
        {
            byte[] bits = Convert.FromBase64String(keys);
            foreach (int code in codes)
            {
                if (!HasBit(bits, code))
                    throw new NotSupportedException($"Evdev instrument is missing required KEY code {code}.");
            }
        }
    }

    internal static class EvdevLayoutFinder
    {
        internal static InputDeviceMatcher Matcher(string driver)
            => new InputDeviceMatcher().WithInterface("Evdev").WithCapability("driver", driver);

        internal static void Register<TDevice>(string driver, int vendorId, int productId)
            where TDevice : InputDevice
            => InputSystem.RegisterLayout<TDevice>(matches: Matcher(driver)
                .WithCapability("vendorId", vendorId).WithCapability("productId", productId));

        internal static void Register<TDevice>(string product)
            where TDevice : InputDevice
            => InputSystem.RegisterLayout<TDevice>(matches: Matcher("xpad").WithProduct("^" + product + "$"));

        internal static void RegisterXInput<TDevice>(int subType)
            where TDevice : InputDevice
            => InputSystem.RegisterLayout<TDevice>(matches: Matcher("xpad").WithCapability("subType", subType));

        internal static void RegisterXInput<TDevice>(int subType, string productName)
            where TDevice : InputDevice
            => InputSystem.RegisterLayout<TDevice>(matches: Matcher("xpad")
                .WithCapability("subType", subType).WithProduct("^" + productName + "$"));

        internal static void RegisterXInput<TDevice>(int subType, int vendorId, int productId)
            where TDevice : InputDevice
            => InputSystem.RegisterLayout<TDevice>(matches: Matcher("xpad")
                .WithCapability("subType", subType).WithCapability("vendorId", vendorId)
                .WithCapability("productId", productId));

        internal static void RegisterSantroller<TDevice>(SantrollerDeviceType deviceType, int protocol)
            where TDevice : InputDevice
        {
            int maxProduct = protocol == 1 ? SantrollerLayoutFinder.ProductID : SantrollerLayoutFinder.ProductIDV2;
            for (int product = SantrollerLayoutFinder.ProductID; product <= maxProduct; product++)
                InputSystem.RegisterLayout<TDevice>(matches: Matcher("hid-generic")
                    .WithCapability("vendorId", SantrollerLayoutFinder.VendorID)
                    .WithCapability("productId", product)
                    .WithCapability("santrollerProtocol", protocol)
                    .WithCapability("subType", (int)deviceType));
        }

        internal static void RegisterSantroller<TDevice>(SantrollerDeviceType deviceType, bool legacy = true)
            where TDevice : InputDevice
        {
            for (int protocol = legacy ? 1 : 2; protocol <= 2; protocol++)
                RegisterSantroller<TDevice>(deviceType, protocol);
        }

        internal static EvdevCapabilities GetCapabilities(InputDevice device)
        {
            if (!Utilities.TryParseJson<EvdevCapabilities>(device.description.capabilities, out var capabilities) ||
                (capabilities.driver != "xpad" && capabilities.driver != "sony" &&
                    capabilities.driver != "hid-generic"))
                throw new NotSupportedException("Missing or unsupported evdev driver profile.");
            return capabilities;
        }

        internal static void RequireXInputAxes(InputDevice device)
            => GetCapabilities(device).RequireAxes(0, 1, 2, 3, 4, 5);
    }
}
