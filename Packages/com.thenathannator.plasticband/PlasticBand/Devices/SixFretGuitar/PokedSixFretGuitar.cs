using System;
using System.Threading;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// A 6-fret guitar "poke"/"keep-alive" message.
    /// </summary>
    internal unsafe struct SixFretHidPokeCommand : IInputDeviceCommandInfo
    {
        internal const int Size = InputDeviceCommand.BaseCommandSize + DataSize;
        internal const int DataSize = sizeof(byte) * 9;

        public static FourCC Type => new FourCC('H', 'I', 'D', 'O');
        public FourCC typeStatic => Type;

        public InputDeviceCommand baseCommand;
        private fixed byte data[DataSize];

        /// <summary>
        /// Creates a SixFretHidPokeCommand.
        /// </summary>
        public static SixFretHidPokeCommand Create(ReadOnlySpan<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != DataSize)
                throw new ArgumentException($"Data size is wrong! Expected {DataSize}, got {data.Length}", nameof(data));

            var command = new SixFretHidPokeCommand()
            {
                baseCommand = new InputDeviceCommand(Type, Size)
            };

            // Copy data
            fixed (byte* ptr = data)
            {
                for (int i = 0; i < data.Length && i < DataSize; i++)
                {
                    command.data[i] = ptr[i];
                }
            }

            return command;
        }
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 6-fret guitar controller that requires a "poke" or "keep-alive" message to be sent on occasion to receive full input data.
    /// </summary>
    public abstract class PokedSixFretGuitar : SixFretGuitar
    {
        /// <summary>
        /// The poke interval time, in milliseconds.
        /// </summary>
        /// <remarks>
        /// For PS3/Wii U, the poke data must be sent every 10 seconds at minimum, but for PS4,
        /// the poke data must be sent every 8 seconds, so we use that for simplicity.
        /// </remarks>
        private const int kPokeInterval = 8000;

        /// <summary>
        /// A timer used for sending the poke data.
        /// </summary>
        private Timer pokeTimer;

        /// <summary>
        /// Handles when this device should be poked.
        /// </summary>
        private static void PokeTimerProc(object obj)
        {
            var device = (PokedSixFretGuitar)obj;
            device.OnPoke();
        }

        /// <summary>
        /// Handles when this device should be poked.
        /// </summary>
        protected abstract void OnPoke();

        protected override void FinishSetup()
        {
            base.FinishSetup();
            pokeTimer = new Timer(PokeTimerProc, this, 0, kPokeInterval);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            pokeTimer.Dispose();
        }
    }
}