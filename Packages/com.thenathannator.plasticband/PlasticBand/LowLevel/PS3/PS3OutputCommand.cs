using System;
using System.Runtime.InteropServices;
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
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte) * 2 +  kDataSize;
        internal const int kDataSize = sizeof(byte) * 7;

        public static FourCC type => new FourCC('H', 'I', 'D', 'O');
        public FourCC typeStatic => type;

        public InputDeviceCommand baseCommand;
        public byte reportId;
        public byte outputType;
        public fixed byte data[kDataSize];

        public PS3OutputCommand(byte type, ReadOnlySpan<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != kDataSize)
                throw new ArgumentException($"Data size is wrong! Expected {kDataSize}, got {data.Length}", nameof(data));

            baseCommand = new InputDeviceCommand(PS3OutputCommand.type, kSize);

            reportId = 0x01;
            outputType = type;

            for (int i = 0; i < data.Length; i++)
            {
                this.data[i] = data[i];
            }
        }

        public PS3OutputCommand(byte reportId, byte type, ReadOnlySpan<byte> data) : this(type, data)
        {
            this.reportId = reportId;
        }
    }
}
