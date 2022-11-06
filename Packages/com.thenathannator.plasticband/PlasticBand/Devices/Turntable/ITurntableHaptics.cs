using UnityEngine.InputSystem.Haptics;

namespace PlasticBand.Haptics
{
    public interface ITurntableHaptics : IHaptics
    {
        /// <summary>
        /// Sets the brightness of the Euphoria button's light.
        /// </summary>
        /// <param name="brightness">
        /// The brightness to set the light to.
        /// </param>
        void SetEuphoriaBrightness(float brightness);
    }
}