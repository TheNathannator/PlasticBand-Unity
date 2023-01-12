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
        internal const int kSize = InputDeviceCommand.BaseCommandSize + (sizeof(float) * 2);

        public static FourCC type => new FourCC('R', 'M', 'B', 'L');
        public FourCC typeStatic => type;

        public InputDeviceCommand baseCommand;
        public float leftMotorSpeed;
        public float rightMotorSpeed;

        public XInputVibrationCommand(float leftMotor, float rightMotor)
        {
            baseCommand = new InputDeviceCommand(type, kSize);
            leftMotorSpeed = Mathf.Clamp(leftMotor, 0.0f, 1.0f);
            rightMotorSpeed = Mathf.Clamp(rightMotor, 0.0f, 1.0f);
        }

        public XInputVibrationCommand(ushort leftMotor, ushort rightMotor)
        {
            baseCommand = new InputDeviceCommand(type, kSize);
            leftMotorSpeed = (float)leftMotor / ushort.MaxValue;
            rightMotorSpeed = (float)rightMotor / ushort.MaxValue;
        }
    }
}
