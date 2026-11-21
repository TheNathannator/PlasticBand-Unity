#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Backend.Linux;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.LowLevel
{
    /// <summary>
    /// Helpers for building evdev mappings and registering them for the two evdev sources we care about:
    /// <list type="bullet">
    /// <item>Santroller V2 devices, exposed through hid-generic. The HID report mirrors XInput, so Y axes
    /// are positive-up and must be inverted relative to evdev's positive-down convention.</item>
    /// <item>XInput devices, exposed through xpad. xpad already inverts Y axes to evdev convention.</item>
    /// </list>
    /// Both sources use the same key codes (BTN_A/B/X/Y, BTN_TL/TR, BTN_SELECT/START/MODE, BTN_THUMBL/R),
    /// so only axis mappings differ between them.
    /// </summary>
    /// <remarks>
    /// Only controls that map directly onto a single evdev input are handled; anything requiring
    /// state translation (GH5 touch bar, RB pickup switch and solo frets, drum velocities and
    /// pad/cymbal flags, turntable platter buttons) is left unmapped.
    /// </remarks>
    internal static class EvdevMappings
    {
        /// <summary>
        /// Creates a mapping pre-populated with the menu buttons and d-pad.
        /// </summary>
        public static EvdevDeviceMapping CreateBase()
        {
            return new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithHat(0, "dpad");
        }

        /// <summary>
        /// An axis that rests at its minimum and reads 0 to 1 (e.g. whammy).
        /// </summary>
        public static EvdevControlMapping Unsigned(string path)
            => new EvdevControlMapping() { path = path, options = EvdevControlOptions.Unsigned };

        /// <summary>
        /// An axis whose direction should be flipped relative to the default handling.
        /// Used to keep the raw report's direction for Y axes, which are otherwise inverted
        /// to convert from evdev's positive-down convention (e.g. Santroller V2's XInput-style
        /// Y axes, or PS3 accelerometer tilt).
        /// </summary>
        public static EvdevControlMapping Inverted(string path)
            => new EvdevControlMapping() { path = path, options = EvdevControlOptions.Inverted };

        /// <summary>
        /// An axis used as a button that counts as pressed as soon as it leaves its resting value
        /// (e.g. drum pads that report how hard they were hit, where light hits stay low).
        /// </summary>
        public static EvdevControlMapping PressOnAnyValue(string path)
            => new EvdevControlMapping() { path = path, options = EvdevControlOptions.PressOnAnyValue };

        // hid-sony instrument key mapping (instrument_keymap), by HID button usage.
        // PS3 instruments name these in PS3Button bit order; note that PS3Button's L2/R2/L1/R1
        // correspond to usages 5/6/7/8, while on PS4/PS5 usages 5-8 are L1/R1/L2/R2.
        public const input_key_code SonySquare = input_key_code.BTN_Y;    // usage 1, BTN_WEST
        public const input_key_code SonyCross = input_key_code.BTN_A;     // usage 2, BTN_SOUTH
        public const input_key_code SonyCircle = input_key_code.BTN_B;    // usage 3, BTN_EAST
        public const input_key_code SonyTriangle = input_key_code.BTN_X;  // usage 4, BTN_NORTH
        public const input_key_code SonyButton5 = input_key_code.BTN_TL;  // PS3Button.L2 / PS4 L1
        public const input_key_code SonyButton6 = input_key_code.BTN_TR;  // PS3Button.R2 / PS4 R1
        public const input_key_code SonyButton7 = input_key_code.BTN_TL2; // PS3Button.L1 / PS4 L2
        public const input_key_code SonyButton8 = input_key_code.BTN_TR2; // PS3Button.R1 / PS4 R2

        /// <summary>
        /// Registers a layout for a specific device by vendor and product ID (e.g. instruments handled by hid-sony).
        /// </summary>
        public static void RegisterLayout<TDevice>(
            EvdevDeviceMapping mapping,
            int vendorId,
            int productId
        )
            where TDevice : InputDevice
        {
            EvdevLayoutFinder.RegisterLayout<TDevice>(mapping, (ushort)vendorId, (ushort)productId);
        }

        public static void RegisterXpadLayout<TDevice>(
            EvdevDeviceMapping mapping,
            XInputDeviceSubType subType
        )
            where TDevice : InputDevice
        {
            EvdevLayoutFinder.RegisterLayout<TDevice>(mapping, GetXpadMatcher(subType));
        }

        public static void RegisterXpadLayout<TDevice>(
            EvdevDeviceMapping mapping,
            XInputDeviceSubType subType,
            int vendorId,
            int productId
        )
            where TDevice : InputDevice
        {
            EvdevLayoutFinder.RegisterLayout<TDevice>(mapping, GetXpadMatcher(subType)
                .WithCapability("vendorId", vendorId)
                .WithCapability("productId", productId));
        }

        private static InputDeviceMatcher GetXpadMatcher(XInputDeviceSubType subType)
        {
            return new InputDeviceMatcher()
                .WithInterface(EvdevDefinitions.InterfaceName)
                .WithCapability("xpadSubtype", (int)subType);
        }
    }
}
#endif
