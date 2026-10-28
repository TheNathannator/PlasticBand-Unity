using PlasticBand.LowLevel;

namespace PlasticBand.Devices.LowLevel
{
    internal static class SantrollerExtensions
    {
        internal static void SetBit(ref this SantrollerHIDGuitarHeroGuitarState.Button value,
            SantrollerHIDGuitarHeroGuitarState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this SantrollerHIDRockBandGuitarState.Button value,
            SantrollerHIDRockBandGuitarState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this SantrollerHIDSixFretGuitarState.Button value,
            SantrollerHIDSixFretGuitarState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this SantrollerFiveLaneDrumkitState.Button value,
            SantrollerFiveLaneDrumkitState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this SantrollerFourLaneDrumkitState.Button value,
            SantrollerFourLaneDrumkitState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this SantrollerHIDTurntableState.Button value,
            SantrollerHIDTurntableState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        #region Santroller V2

        internal static void SetBit(ref this SantrollerV2Button value, SantrollerV2Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this SantrollerV2HIDTurntableState.PlatterButton value,
            SantrollerV2HIDTurntableState.PlatterButton mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static HidDpad GetDpad(this SantrollerV2Button buttons)
            => (HidDpad)((ushort)buttons & (ushort)SantrollerV2Button.DpadMask);

        internal static void SetDpad(ref this SantrollerV2Button buttons, HidDpad dpad)
            => buttons = (buttons & ~SantrollerV2Button.DpadMask) | ((SantrollerV2Button)dpad & SantrollerV2Button.DpadMask);

        internal static void SetDpadUp(ref this SantrollerV2Button buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetUp(pressed);
            buttons.SetDpad(dpad);
        }

        internal static void SetDpadRight(ref this SantrollerV2Button buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetRight(pressed);
            buttons.SetDpad(dpad);
        }

        internal static void SetDpadDown(ref this SantrollerV2Button buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetDown(pressed);
            buttons.SetDpad(dpad);
        }

        internal static void SetDpadLeft(ref this SantrollerV2Button buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetLeft(pressed);
            buttons.SetDpad(dpad);
        }

        #endregion
    }
}