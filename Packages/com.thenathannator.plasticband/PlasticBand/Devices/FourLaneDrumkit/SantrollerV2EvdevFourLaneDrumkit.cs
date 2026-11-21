#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Covers Santroller V2 devices, exposed through hid-generic/hid-santroller.
    // The HID report mirrors XInput, so Y axes are positive-up and must be inverted
    // relative to evdev's convention. Uses the same key codes as xpad.
    // Not supported (requires state translation): cymbals, velocities.
    // Colored inputs are mapped straight to the pads, so cymbal hits register as pad hits.
    [InputControlLayout(displayName = "Santroller V2 Evdev Rock Band Drumkit", hideInUI = true)]
    internal class SantrollerV2EvdevFourLaneDrumkit : FourLaneDrumkit
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
            SantrollerLayoutFinder.RegisterEvdevLayout<SantrollerV2EvdevFourLaneDrumkit>(mapping, SantrollerDeviceType.RockBandDrums);
        }
    }
}
#endif
