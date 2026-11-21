#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Backend.Linux;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Wii DJ Hero turntables, which are Wii Remote extensions: the kernel's hid-wiimote driver exposes
    // each part of a Wii Remote as its own evdev device, the turntable being "Nintendo Wii Remote Turntable".
    // hid-wiimote's turntable mapping:
    // - platter buttons are BTN_1-3 (right table green/red/blue) and BTN_4-6 (left table), euphoria is BTN_7
    // - table velocities are ABS_HAT0X (right) and ABS_HAT1X (left). They're the raw 6-bit signed fields,
    //   which the driver passes through unsigned (63 is -1) despite declaring a -8 to 8 range, so they're
    //   sign-extended here.
    // - the crossfader is ABS_HAT2X (0 to 30, in steps of 2)
    // - the effects dial is ABS_HAT3X, but the driver only reports the low 3 of its 5 bits, so it wraps
    //   every eighth of the dial's real range
    // Not supported:
    // - the d-pad, which comes from the Wii Remote itself (a separate evdev device)
    // - the face buttons: the turntable has none, as on the real Wii turntable the platter buttons stand in
    //   for them, and an evdev key can only map to one control
    // - the analog stick (ABS_X/ABS_Y), which has no equivalent control
    [InputControlLayout(displayName = "Wii Evdev DJ Hero Turntable", hideInUI = true)]
    internal class WiiEvdevTurntable : Turntable
    {
        private const string kProductName = "Nintendo Wii Remote Turntable";

        internal new static void Initialize()
        {
            var mapping = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_1, "rightTableGreen")
                .WithKey(input_key_code.BTN_2, "rightTableRed")
                .WithKey(input_key_code.BTN_3, "rightTableBlue")
                .WithKey(input_key_code.BTN_4, "leftTableGreen")
                .WithKey(input_key_code.BTN_5, "leftTableRed")
                .WithKey(input_key_code.BTN_6, "leftTableBlue")
                .WithKey(input_key_code.BTN_7, "buttonNorth")
                .WithAbs(input_abs_code.ABS_HAT0X, SignedBits("rightTableVelocity", 6))
                .WithAbs(input_abs_code.ABS_HAT1X, SignedBits("leftTableVelocity", 6))
                .WithAbs(input_abs_code.ABS_HAT2X, "crossfader")
                .WithAbs(input_abs_code.ABS_HAT3X, Unsigned("effectsDial"));

            // Nintendo Wii Remote, original and Wii MotionPlus Inside models
            foreach (int productId in new[] { 0x0306, 0x0330 })
            {
                EvdevLayoutFinder.RegisterLayout<WiiEvdevTurntable>(mapping, new InputDeviceMatcher()
                    .WithInterface(EvdevDefinitions.InterfaceName)
                    .WithProduct(kProductName)
                    .WithCapability("vendorId", 0x057E)
                    .WithCapability("productId", productId));
            }
        }
    }
}
#endif
