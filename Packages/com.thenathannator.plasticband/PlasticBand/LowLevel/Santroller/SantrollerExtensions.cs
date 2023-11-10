namespace PlasticBand.Devices.LowLevel
{
    internal static class SantrollerExtensions
    {
        internal static void SetBit(ref this SantrollerFiveLaneDrumkitState.Button value,
            SantrollerFiveLaneDrumkitState.Button mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }
}