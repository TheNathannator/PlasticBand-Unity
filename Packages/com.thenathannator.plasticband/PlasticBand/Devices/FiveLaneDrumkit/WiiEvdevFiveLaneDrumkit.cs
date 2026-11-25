#if HIDROGEN_0_6_0_OR_LATER
using System;
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [InputControlLayout(stateType = typeof(TranslatedFiveLaneState), displayName = "Guitar Hero Drumkit for Wii", hideInUI = true)]
    internal class WiiEvdevFiveLaneDrumkit : EvdevFiveLaneDrumkit<WiiEvdevFiveLaneDrumkit>
    {
        private const int kMaxHitStrength = 7;

        private const input_abs_code kRed = input_abs_code.ABS_HAT0X;
        private const input_abs_code kYellow = input_abs_code.ABS_HAT2X;
        private const input_abs_code kBlue = input_abs_code.ABS_HAT0Y;
        private const input_abs_code kOrange = input_abs_code.ABS_HAT2Y;
        private const input_abs_code kGreen = input_abs_code.ABS_HAT1X;
        private const input_abs_code kKick = input_abs_code.ABS_HAT3X;

        internal new static void Initialize()
        {
            var mappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithAbs(kRed, "redPad")
                .WithAbs(kYellow, "yellowCymbal")
                .WithAbs(kBlue, "bluePad")
                .WithAbs(kOrange, "orangeCymbal")
                .WithAbs(kGreen, "greenPad")
                .WithAbs(kKick, "kick");

            // All Wii Remote extensions share the Wii Remote's IDs, so they're told apart by hid-wiimote's device name

            // Nintendo Wii Remote
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevFiveLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x057E, 0x0306)
                    .WithProduct("Nintendo Wii Remote Drums")
            );

            // Nintendo Wii Remote (Wii MotionPlus Inside)
            EvdevLayoutFinder.RegisterStaticLayout<WiiEvdevFiveLaneDrumkit>(
                mappings,
                new EvdevDeviceMatcher()
                    .WithHardwareIds(0x057E, 0x0330)
                    .WithProduct("Nintendo Wii Remote Drums")
            );
        }

        // The d-pad comes from the Wii Remote, and the drums' ABS_HAT* axes are pads, so only start/select are read
        // The drums' hat axes are pads, not a d-pad
        protected override unsafe EvdevHatValue ReadDpad(StateEvent* stateEvent) => EvdevHatValue.None;

        protected override unsafe FiveLanePad ReadHits(StateEvent* stateEvent)
        {
            var hits = FiveLanePad.None;
            if (IsHit(m_StateHandler.ReadAbs(stateEvent, kRed))) hits |= FiveLanePad.Red;
            if (IsHit(m_StateHandler.ReadAbs(stateEvent, kYellow))) hits |= FiveLanePad.Yellow;
            if (IsHit(m_StateHandler.ReadAbs(stateEvent, kBlue))) hits |= FiveLanePad.Blue;
            if (IsHit(m_StateHandler.ReadAbs(stateEvent, kOrange))) hits |= FiveLanePad.Orange;
            if (IsHit(m_StateHandler.ReadAbs(stateEvent, kGreen))) hits |= FiveLanePad.Green;
            if (IsHit(m_StateHandler.ReadAbs(stateEvent, kKick))) hits |= FiveLanePad.Kick;
            return hits;
        }

        protected override unsafe void ReadVelocities(
            StateEvent* stateEvent,
            out byte red, out byte yellow, out byte blue,
            out byte orange, out byte green, out byte kick
        )
        {
            red = ToVelocity(m_StateHandler.ReadAbs(stateEvent, kRed));
            yellow = ToVelocity(m_StateHandler.ReadAbs(stateEvent, kYellow));
            blue = ToVelocity(m_StateHandler.ReadAbs(stateEvent, kBlue));
            orange = ToVelocity(m_StateHandler.ReadAbs(stateEvent, kOrange));
            green = ToVelocity(m_StateHandler.ReadAbs(stateEvent, kGreen));
            kick = ToVelocity(m_StateHandler.ReadAbs(stateEvent, kKick));
        }

        private static bool IsHit(int strength) => strength > 0;

        private static byte ToVelocity(int strength)
            => (byte)(Math.Min(Math.Max(strength, 0), kMaxHitStrength) * byte.MaxValue / kMaxHitStrength);
    }
}
#endif
