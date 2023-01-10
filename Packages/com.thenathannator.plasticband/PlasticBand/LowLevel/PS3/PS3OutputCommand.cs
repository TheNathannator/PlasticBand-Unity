using System;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// An output command for PS3 devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = Size)]
    internal unsafe struct PS3OutputCommand : IInputDeviceCommandInfo
    {
        internal const int Size = InputDeviceCommand.BaseCommandSize + sizeof(byte) * 2 +  DataSize;
        internal const int DataSize = sizeof(byte) * 7;

        public static FourCC Type => new FourCC('H', 'I', 'D', 'O');
        public FourCC typeStatic => Type;

        public InputDeviceCommand baseCommand;
        public byte reportId;
        private byte outputType;
        private fixed byte data[DataSize];

        public PS3OutputCommand(byte type, ReadOnlySpan<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != DataSize)
                throw new ArgumentException($"Data size is wrong! Expected {DataSize}, got {data.Length}", nameof(data));

            baseCommand = new InputDeviceCommand(Type, Size);

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
