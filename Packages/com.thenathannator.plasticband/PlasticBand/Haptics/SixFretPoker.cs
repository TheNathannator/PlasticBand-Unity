using System;
using System.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Handles sending pokes to Guitar Hero Live guitars that need it.
    /// </summary>
    internal class SixFretPoker<TPoke>
        where TPoke : struct, IInputDeviceCommandInfo
    {
        // For PS3/Wii U, the poke data must be sent at least every 10 seconds, but for PS4,
        // the poke data must be sent at least every 8 seconds, so we use that for simplicity.
        private const long kPokeInterval = 8000;

        private readonly InputDevice m_Device;
        private readonly Stopwatch m_PokeTimer = new Stopwatch();
        private readonly TPoke m_PokeData;

        public SixFretPoker(InputDevice device, TPoke pokeData)
        {
            m_Device = device ?? throw new ArgumentNullException(nameof(device));
            m_PokeData = pokeData;
            m_PokeTimer.Start();
        }

        public void OnUpdate()
        {
            if (m_PokeTimer.ElapsedMilliseconds >= kPokeInterval)
            {
                m_PokeTimer.Restart();
                // Avoid modifications to the command data
                var command = m_PokeData;
                m_Device.LoggedExecuteCommand(ref command);
            }
        }
    }
}