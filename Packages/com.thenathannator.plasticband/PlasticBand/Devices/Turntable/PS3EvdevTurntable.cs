#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using static PlasticBand.LowLevel.EvdevMappings;

namespace PlasticBand.Devices
{
    // NOTE: This layout is never directly matched; instead a derived layout is
    // always built on-demand via EvdevLayoutFinder.
    // Mappings follow hid-sony's turntable mapping:
    // - the platter velocities (left stick X/Y in the report) are remapped to ABS_X/ABS_Y
    // - the crossfader and effects knob (vendor fields in place of the accelerometer) are ABS_RX/ABS_RY
    // - the platter buttons (a bitmask in another vendor field) are reported as BTN_TRIGGER_HAPPY1-6
    // The platter buttons also press the matching face button, as on the real turntable.
    // Requires a hid-sony with the platter button keys; without them the layout won't match.
    [InputControlLayout(displayName = "PS3 Evdev DJ Hero Turntable", hideInUI = true)]
    internal class PS3EvdevTurntable : Turntable
    {
        internal new static void Initialize()
        {
            var mapping = CreateBase()
                .WithKey(SonyCross, "buttonSouth")
                .WithKey(SonyCircle, "buttonEast")
                .WithKey(SonySquare, "buttonWest")
                .WithKey(SonyTriangle, "buttonNorth")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY1, "leftTableGreen")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY2, "leftTableRed")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY3, "leftTableBlue")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY4, "rightTableGreen")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY5, "rightTableRed")
                .WithKey(input_key_code.BTN_TRIGGER_HAPPY6, "rightTableBlue")
                .WithAbs(input_abs_code.ABS_X, "leftTableVelocity")
                .WithAbs(input_abs_code.ABS_Y, Inverted("rightTableVelocity"))
                .WithAbs(input_abs_code.ABS_RX, "crossfader")
                .WithAbs(input_abs_code.ABS_RY, Inverted("effectsDial"));

            // Activision DJ Hero turntable for PS3
            RegisterLayout<PS3EvdevTurntable>(mapping, 0x12BA, 0x0140);
        }
    }
}
#endif
