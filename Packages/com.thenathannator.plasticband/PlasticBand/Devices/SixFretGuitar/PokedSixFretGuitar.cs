using System.Diagnostics;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 6-fret guitar controller that requires a "poke" or "keep-alive" message to be sent on occasion to receive full input data.
    /// </summary>
    public abstract class PokedSixFretGuitar : SixFretGuitar, IInputUpdateCallbackReceiver
    {
        /// <summary>
        /// The poke interval time, in milliseconds.
        /// </summary>
        /// <remarks>
        /// For PS3/Wii U, the poke data must be sent every 10 seconds at minimum, but for PS4,
        /// the poke data must be sent every 8 seconds, so we use that for simplicity.
        /// </remarks>
        private const long kPokeInterval = 8000;

        /// <summary>
        /// A timer used for sending the poke data.
        /// </summary>
        private readonly Stopwatch m_PokeTimer = new Stopwatch();

        /// <summary>
        /// Handles when this device should be poked.
        /// </summary>
        protected abstract void OnPoke();

        void IInputUpdateCallbackReceiver.OnUpdate()
        {
            if (m_PokeTimer.ElapsedMilliseconds >= kPokeInterval)
            {
                m_PokeTimer.Restart();
                OnPoke();
            }
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_PokeTimer.Start();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            m_PokeTimer.Stop();
        }
    }
}