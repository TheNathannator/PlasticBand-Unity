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
        internal const int kDefaultReportId = 0x00;
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte) * 2 + kDataSize;
        internal const int kDataSize = sizeof(byte) * 7;

        public FourCC typeStatic => HidDefinitions.OutputFormat;

        public InputDeviceCommand baseCommand;
        public byte reportId;
        public byte outputType;
        public fixed byte data[kDataSize];

        public PS3OutputCommand(byte type)
        {
            baseCommand = new InputDeviceCommand(HidDefinitions.OutputFormat, kSize);

            reportId = kDefaultReportId;
            outputType = type;
        }

        public PS3OutputCommand(byte type, byte[] data) : this(type)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != kDataSize)
                throw new ArgumentException($"Data size is wrong! Expected {kDataSize}, got {data.Length}", nameof(data));

            for (int i = 0; i < data.Length; i++)
            {
                this.data[i] = data[i];
            }
        }

        public PS3OutputCommand(byte reportId, byte type) : this(type)
        {
            this.reportId = reportId;
        }

        public PS3OutputCommand(byte reportId, byte type, byte[] data) : this(type, data)
        {
            this.reportId = reportId;
        }
    }
}
