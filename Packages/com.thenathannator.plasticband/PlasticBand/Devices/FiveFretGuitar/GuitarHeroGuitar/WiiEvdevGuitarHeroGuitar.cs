#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Backend.Linux;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Wii Guitar Hero guitars, which are Wii Remote extensions: the kernel's hid-wiimote driver
    // Not supported:
    // - tilt, which comes from the Wii Remote's accelerometer, we would need to group the wii devices together for that
    // - touch/slider bar (ABS_HAT0X), which requires state translation
    // - the guitar's analog stick (ABS_X/ABS_Y), which has no equivalent control
    [InputControlLayout(displayName = "Wii Evdev Guitar Hero Guitar", hideInUI = true)]
    internal class WiiEvdevGuitarHeroGuitar : GuitarHeroGuitar
    {
        private const string kProductName = "Nintendo Wii Remote Guitar";

        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_1, "greenFret")
                .WithKey(input_key_code.BTN_2, "redFret")
                .WithKey(input_key_code.BTN_3, "yellowFret")
                .WithKey(input_key_code.BTN_4, "blueFret")
                .WithKey(input_key_code.BTN_5, "orangeFret")
                .WithAbs(input_abs_code.ABS_HAT1X, Unsigned("whammy"));

            // Nintendo Wii Remote, original and Wii MotionPlus Inside models
            foreach (int productId in new[] { 0x0306, 0x0330 })
            {
                EvdevLayoutFinder.RegisterLayout<WiiEvdevGuitarHeroGuitar>(mapping, new InputDeviceMatcher()
                    .WithInterface(EvdevDefinitions.InterfaceName)
                    .WithProduct(kProductName)
                    .WithCapability("vendorId", 0x057E)
                    .WithCapability("productId", productId));
            }
        }
    }
}
#endif
