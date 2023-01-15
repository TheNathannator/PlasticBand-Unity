using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// An output command for PS3 devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal unsafe struct PS3OutputCommand : IInputDeviceCommandInfo
    {
        /// <summary>
        /// The size of this command.
        /// </summary>
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte) * 2 +  kDataSize;

        /// <summary>
        /// The size of <see cref="data"/>.
        /// </summary>
        internal const int kDataSize = sizeof(byte) * 7;

        /// <summary>
        /// The format of this command.
        /// </summary>
        public FourCC typeStatic => HidDefinitions.OutputFormat;

        /// <summary>
        /// The base command info.
        /// </summary>
        public InputDeviceCommand baseCommand;

        /// <summary>
        /// The report ID.
        /// </summary>
        public byte reportId;

        /// <summary>
        /// The output type.
        /// </summary>
        public byte outputType;

        /// <summary>
        /// The rest of the command's data.
        /// Varies depending on output type.
        /// </summary>
        public fixed byte data[kDataSize];

        /// <summary>
        /// Creates a new <see cref="PS3OutputCommand"/> with the specified output type and data.
        /// </summary>
        public PS3OutputCommand(byte type, byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != kDataSize)
                throw new ArgumentException($"Data size is wrong! Expected {kDataSize}, got {data.Length}", nameof(data));

            baseCommand = new InputDeviceCommand(HidDefinitions.OutputFormat, kSize);

            reportId = 0x01;
            outputType = type;

            for (int i = 0; i < data.Length; i++)
            {
                this.data[i] = data[i];
            }
        }

        /// <summary>
        /// Creates a new <see cref="PS3OutputCommand"/> with the specified report ID, output type, and data.
        /// </summary>
        public PS3OutputCommand(byte reportId, byte type, byte[] data) : this(type, data)
        {
            this.reportId = reportId;
        }
    }
}
