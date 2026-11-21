#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Mappings follow hid-sony's instrument mapping.
    // Not supported (requires state translation): cymbals, velocities.
    // Colored inputs are mapped straight to the pads, so cymbal hits register as pad hits.
    [InputControlLayout(displayName = "PS3/Wii Evdev Rock Band Drumkit", hideInUI = true)]
    internal class PS3EvdevFourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonyCross, "greenPad")
                .WithKey(SonyCircle, "redPad")
                .WithKey(SonySquare, "bluePad")
                .WithKey(SonyTriangle, "yellowPad")
                .WithKey(SonyButton5, "kick1")
                .WithKey(SonyButton6, "kick2");

            // Harmonix Rock Band drumkit for PS3
            RegisterLayout<PS3EvdevFourLaneDrumkit>(mapping, 0x12BA, 0x0210);
            // MadCatz Rock Band 3 MIDI Pro Adapter for PS3 (drums mode)
            RegisterLayout<PS3EvdevFourLaneDrumkit>(mapping, 0x12BA, 0x0218);
            // Harmonix Rock Band 1/2 drumkits for Wii
            RegisterLayout<PS3EvdevFourLaneDrumkit>(mapping, 0x1BAD, 0x0005);
            RegisterLayout<PS3EvdevFourLaneDrumkit>(mapping, 0x1BAD, 0x3110);
            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (drums mode)
            RegisterLayout<PS3EvdevFourLaneDrumkit>(mapping, 0x1BAD, 0x3138);
        }
    }
}
#endif
