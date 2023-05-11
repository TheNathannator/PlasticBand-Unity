using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// Command to set the vibration state of an XInput device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal struct XInputVibrationCommand : IInputDeviceCommandInfo
    {
        /// <summary>
        /// The size of this command.
        /// </summary>
        internal const int kSize = InputDeviceCommand.BaseCommandSize + (sizeof(float) * 2);

        /// <summary>
        /// The format of this command.
        /// </summary>
        public static readonly FourCC Type = new FourCC('R', 'M', 'B', 'L');

        /// <summary>
        /// The format of this command.
        /// </summary>
        public FourCC typeStatic => Type;

        /// <summary>
        /// The base command info.
        /// </summary>
        public InputDeviceCommand baseCommand;

        /// <summary>
        /// The left motor speed.
        /// </summary>
        public float leftMotorSpeed;

        /// <summary>
        /// The right motor speed.
        /// </summary>
        public float rightMotorSpeed;

        /// <summary>
        /// Creates a new <see cref="XInputVibrationCommand"/> with the given left and right motor speeds, as floats.
        /// </summary>
        public XInputVibrationCommand(float leftMotor, float rightMotor)
        {
            baseCommand = new InputDeviceCommand(Type, kSize);
            leftMotorSpeed = Mathf.Clamp(leftMotor, 0.0f, 1.0f);
            rightMotorSpeed = Mathf.Clamp(rightMotor, 0.0f, 1.0f);
        }

        /// <summary>
        /// Creates a new <see cref="XInputVibrationCommand"/> with the given left and right motor speeds, as ushorts.
        /// </summary>
        public XInputVibrationCommand(ushort leftMotor, ushort rightMotor)
        {
            baseCommand = new InputDeviceCommand(Type, kSize);
            leftMotorSpeed = (float)leftMotor / ushort.MaxValue;
            rightMotorSpeed = (float)rightMotor / ushort.MaxValue;
        }

        /// <summary>
        /// Creates a new <see cref="XInputVibrationCommand"/> with the given left and right motor speeds, as bytes.
        /// </summary>
        public XInputVibrationCommand(byte leftMotor, byte rightMotor)
        {
            baseCommand = new InputDeviceCommand(Type, kSize);
            leftMotorSpeed = (float)leftMotor / byte.MaxValue;
            rightMotorSpeed = (float)rightMotor / byte.MaxValue;
        }
    }
}
