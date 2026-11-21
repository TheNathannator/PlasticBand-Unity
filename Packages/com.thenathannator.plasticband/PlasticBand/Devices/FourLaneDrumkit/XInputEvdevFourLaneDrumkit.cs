#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Not supported (requires state translation): cymbals, velocities.
    // Colored inputs are mapped straight to the pads, so cymbal hits register as pad hits.
    // Covers XInput devices exposed through xpad, including Santroller V1 devices in XInput mode.
    // xpad already converts Y axes to evdev's positive-down convention.
    // GH drumkits share the DrumKit subtype, and xpad doesn't expose the capability flags needed to tell
    // them apart, so this is used as the default; known GH drumkits are matched by XInputEvdevFiveLaneDrumkit.
    [InputControlLayout(displayName = "XInput Evdev Rock Band Drumkit", hideInUI = true)]
    internal class XInputEvdevFourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "greenPad")
                .WithKey(input_key_code.BTN_B, "redPad")
                .WithKey(input_key_code.BTN_X, "bluePad")
                .WithKey(input_key_code.BTN_Y, "yellowPad")
                .WithKey(input_key_code.BTN_TL, "kick1")
                .WithKey(input_key_code.BTN_THUMB_L, "kick2");

            RegisterXpadLayout<XInputEvdevFourLaneDrumkit>(mapping, XInputDeviceSubType.DrumKit);
            SantrollerLayoutFinder.RegisterEvdevXpadLayout<XInputEvdevFourLaneDrumkit>(mapping,
                XInputDeviceSubType.DrumKit, SantrollerDeviceType.RockBandDrums);
        }
    }
}
#endif
