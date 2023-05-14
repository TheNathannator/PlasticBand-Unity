using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;

namespace PlasticBand.Haptics
{
    internal interface ISantrollerHapticsSender
    {
        void SendCommand(InputDevice device, byte commandId, byte parameter);
    }

    internal class XInputSantrollerHapticsSender : ISantrollerHapticsSender
    {
        public void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
        {
            var command = new XInputVibrationCommand(parameter, commandId);
            device.ExecuteCommand(ref command);
        }
    }

    internal class HidSantrollerHapticsSender : ISantrollerHapticsSender
    {
        public unsafe void SendCommand(InputDevice device, byte commandId, byte parameter = 0)
        {
            var command = new PS3OutputCommand(0x5A);
            command.data[0] = parameter;
            command.data[1] = commandId;
            device.ExecuteCommand(ref command);
        }
    }
}