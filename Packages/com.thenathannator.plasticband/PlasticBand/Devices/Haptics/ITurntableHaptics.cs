using PlasticBand.Devices;
using UnityEngine.InputSystem.Haptics;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Interface for <see cref="Turntable"/> haptics.
    /// </summary>
    public interface ITurntableHaptics : IHaptics
    {
        /// <summary>
        /// Enables or disables blinking of the Euphoria light on the turntable.
        /// </summary>
        void SetEuphoriaBlink(bool enable);
    }
}