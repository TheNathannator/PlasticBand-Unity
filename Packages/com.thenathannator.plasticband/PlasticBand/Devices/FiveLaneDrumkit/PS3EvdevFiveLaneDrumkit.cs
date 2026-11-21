#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Mappings follow hid-sony's instrument mapping.
    // Not supported (requires state translation): velocities
    [InputControlLayout(displayName = "PS3 Evdev Guitar Hero Drumkit", hideInUI = true)]
    internal class PS3EvdevFiveLaneDrumkit : FiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonyCross, "greenPad")
                .WithKey(SonyCircle, "redPad")
                .WithKey(SonySquare, "bluePad")
                .WithKey(SonyTriangle, "yellowCymbal")
                .WithKey(SonyButton6, "orangeCymbal")
                .WithKey(SonyButton5, "kick");

            // RedOctane Guitar Hero drumkit dongle for PS3
            RegisterLayout<PS3EvdevFiveLaneDrumkit>(mapping, 0x12BA, 0x0120);
        }
    }
}
#endif
