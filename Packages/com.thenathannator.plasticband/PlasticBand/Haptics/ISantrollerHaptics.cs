using PlasticBand.Devices;

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

    /// <summary>
    /// Interface for Santroller 5-fret guitar haptics.
    /// </summary>
    public interface ISantrollerFiveFretGuitarHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Toggles the light for the specified frets.
        /// </summary>
        void SetNoteLights(FiveFret frets, bool enabled);

        /// <summary>
        /// Toggles the light for strumming without holding a fret.
        /// </summary>
        void SetOpenNoteLight(bool enabled);
    }

    /// <summary>
    /// Interface for Santroller 6-fret guitar haptics.
    /// </summary>
    public interface ISantrollerSixFretGuitarHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Toggles the light for the specified frets.
        /// </summary>
        void SetNoteLights(SixFret frets, bool enabled);

        /// <summary>
        /// Toggles the light for strumming without holding a fret.
        /// </summary>
        void SetOpenNoteLight(bool enabled);
    }

    /// <summary>
    /// Interface for Santroller 4-lane drums haptics.
    /// </summary>
    public interface ISantrollerFourLaneDrumkitHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Toggles the light for the specified drum/cymbals.
        /// </summary>
        void SetNoteLights(FourLanePad pads, bool enabled);
    }

    /// <summary>
    /// Interface for Santroller 5-lane drums haptics.
    /// </summary>
    public interface ISantrollerFiveLaneDrumkitHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Toggles the light for the specified drum/cymbals.
        /// </summary>
        void SetNoteLights(FiveLanePad pads, bool enabled);
    }

    /// <summary>
    /// Interface for Santroller turntable haptics.
    /// </summary>
    public interface ISantrollerTurntableHaptics : ISantrollerHaptics
    {
        /// <summary>
        /// Toggles the light for the specified buttons on the left and right turntables.
        /// </summary>
        void SetNoteLights(TurntableButton left, TurntableButton right, bool enabled);

        /// <summary>
        /// Toggles the light for scratching on the left and right turntables.
        /// </summary>
        void SetScratchLights(bool left, bool right);

        /// <summary>
        /// Sets a specific brightness for the Euphoria button light.
        /// </summary>
        void SetEuphoriaBrightness(float brightness);
    }
}