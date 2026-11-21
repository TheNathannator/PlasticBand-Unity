#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Backend.Linux;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Wii Guitar Hero World Tour drums, which are Wii Remote extensions: the kernel's hid-wiimote
    // driver exposes each part of a Wii Remote as its own evdev device, the drums being "Nintendo Wii
    // Remote Drums". The pads' hit bits aren't reported as keys; hid-wiimote folds each one into the pad's
    // axis instead, which reads how hard it was hit (0 to 7, or 3 for hits without velocity data) while
    // the pad is held, and 0 otherwise. Light hits can read as low as 1, so pads count as pressed as soon
    // as they leave 0.
    // Not supported:
    // - the d-pad, which comes from the Wii Remote itself (a separate evdev device)
    // - the hi-hat pedal (ABS_HAT3Y), which has no equivalent control
    // - the analog stick (ABS_X/ABS_Y), which has no equivalent control
    [InputControlLayout(displayName = "Wii Evdev Guitar Hero Drumkit", hideInUI = true)]
    internal class WiiEvdevFiveLaneDrumkit : FiveLaneDrumkit
    {
        private const string kProductName = "Nintendo Wii Remote Drums";

        internal new static void Initialize()
        {
            var mapping = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithAbsButton(input_abs_code.ABS_HAT1X, PressOnAnyValue("greenPad"))
                .WithAbsButton(input_abs_code.ABS_HAT0X, PressOnAnyValue("redPad"))
                .WithAbsButton(input_abs_code.ABS_HAT2X, PressOnAnyValue("yellowCymbal"))
                .WithAbsButton(input_abs_code.ABS_HAT0Y, PressOnAnyValue("bluePad"))
                .WithAbsButton(input_abs_code.ABS_HAT2Y, PressOnAnyValue("orangeCymbal"))
                .WithAbsButton(input_abs_code.ABS_HAT3X, PressOnAnyValue("kick"));

            // Nintendo Wii Remote, original and Wii MotionPlus Inside models
            foreach (int productId in new[] { 0x0306, 0x0330 })
            {
                EvdevLayoutFinder.RegisterLayout<WiiEvdevFiveLaneDrumkit>(mapping, new InputDeviceMatcher()
                    .WithInterface(EvdevDefinitions.InterfaceName)
                    .WithProduct(kProductName)
                    .WithCapability("vendorId", 0x057E)
                    .WithCapability("productId", productId));
            }
        }
    }
}
#endif
