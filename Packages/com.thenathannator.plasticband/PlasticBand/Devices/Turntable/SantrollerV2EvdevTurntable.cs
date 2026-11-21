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
    // Not supported (requires state translation): platter buttons (packed into the trigger axes).
    // The firmware also presses the matching face button with each platter button, so those still register.
    [InputControlLayout(displayName = "Santroller V2 Evdev DJ Hero Turntable", hideInUI = true)]
    internal class SantrollerV2EvdevTurntable : Turntable
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(input_key_code.BTN_A, "buttonSouth")
                .WithKey(input_key_code.BTN_B, "buttonEast")
                .WithKey(input_key_code.BTN_X, "buttonWest")
                .WithKey(input_key_code.BTN_Y, "buttonNorth")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, Inverted("rightTableVelocity"))
                // Same placement as XInput turntables: effects dial on right stick X, crossfader on
                // right stick Y. Verified against firmware output (Tools/EvdevTests).
                .WithAbs(input_abs_code.ABS_RX, "effectsDial")
                .WithAbs(input_abs_code.ABS_RY, Inverted("crossfader"));
            SantrollerLayoutFinder.RegisterEvdevLayout<SantrollerV2EvdevTurntable>(mapping, SantrollerDeviceType.DjHeroTurntable);
        }
    }
}
#endif
