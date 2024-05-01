namespace PlasticBand.Devices.LowLevel
{
    internal static class SantrollerExtensions
    {
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
    }
}