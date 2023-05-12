namespace PlasticBand.Haptics
{
    /// <summary>
    /// Interface for Santroller device haptics.
    /// </summary>
    public interface ISantrollerHaptics : IStageKitHaptics
    {
        /// <summary>
        /// Sends the fill amount of the Star Power gauge (0-1).
        /// </summary>
        void SetStarPowerFill(float fill);

        /// <summary>
        /// Sends whether or not Star Power is active.
        /// </summary>
        void SetStarPowerActive(bool enabled);

        /// <summary>
        /// Sends the current multiplier number (1-245).
        /// </summary>
        void SetMultiplier(uint multiplier);

        /// <summary>
        /// Sends whether or not a solo is active.
        /// </summary>
        void SetSolo(bool enabled);
    }
}