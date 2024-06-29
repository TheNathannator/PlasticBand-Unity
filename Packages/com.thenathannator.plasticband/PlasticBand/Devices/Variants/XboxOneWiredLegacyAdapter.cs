using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Rock%20Band%20Adapters/Xbox%20One%20Wired%20Legacy%20Adapter.md

namespace PlasticBand.Devices
{
    #region Input reports
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneWiredLegacyState : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x20;

        public byte reportId;
        public GameInputButton buttons;

        private unsafe fixed byte padding[3];

        public XInputGamepad state;

        private unsafe fixed byte padding2[6];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneWiredLegacyDeviceInfo : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x22;

        public byte reportId;
        public byte subType;
        private ushort m_VendorId; // Big-endian
        private ushort m_ProductId; // Big-endian

        public ushort vendorId => (ushort)((m_VendorId << 8) | (m_VendorId >> 8));
        public ushort productId => (ushort)((m_ProductId << 8) | (m_ProductId >> 8));
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneWiredLegacyDeviceDisconnect : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x23;

        public byte reportId;
        public byte subType;
    }
    #endregion

    #region Output reports
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal unsafe struct XboxOneWiredLegacySetState : IInputDeviceCommandInfo
    {
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte);
        internal const byte kReportId = 0x21;

        public FourCC typeStatic => GameInputDefinitions.OutputFormat;

        public InputDeviceCommand baseCommand;
        public byte reportId;
        public fixed byte data[23];

        public static XboxOneWiredLegacySetState Create() => new XboxOneWiredLegacySetState()
        {
            baseCommand = new InputDeviceCommand(GameInputDefinitions.OutputFormat, kSize),
            reportId = kReportId,
        };
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal struct XboxOneWiredLegacyRequestInfoCommand : IInputDeviceCommandInfo
    {
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte);
        internal const byte kReportId = 0x24;

        public FourCC typeStatic => GameInputDefinitions.OutputFormat;

        public InputDeviceCommand baseCommand;
        public byte reportId;

        public static XboxOneWiredLegacyRequestInfoCommand Create() => new XboxOneWiredLegacyRequestInfoCommand()
        {
            baseCommand = new InputDeviceCommand(GameInputDefinitions.OutputFormat, kSize),
            reportId = kReportId,
        };
    }
    #endregion

    [InputControlLayout(stateType = typeof(VariantDeviceDummyState), hideInUI = true)]
    internal class XboxOneWiredLegacyAdapter : VariantSingleDevice
    {
        internal static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneWiredLegacyAdapter>(0x0E6F, 0x0175);
        }

        protected override unsafe void OnStateEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
                return;

            var stateEvent = StateEvent.From(eventPtr);
            if (StateReader.ReadReportIdState(stateEvent, out XboxOneWiredLegacyState* state))
            {
                if (m_RealDevice == null)
                {
                    RequestDeviceInfo();
                    return;
                }

                m_RealDevice.OnStateEvent(ref state->state);
            }
            else if (StateReader.ReadReportIdState(stateEvent, out XboxOneWiredLegacyDeviceInfo* info))
            {
                m_RealDevice?.Dispose();

                // Report the device as an XInput one, since that's pretty much what it is lol
                var description = new InputDeviceDescription()
                {
                    interfaceName = XInputLayoutFinder.InterfaceName,
                    capabilities = JsonUtility.ToJson(new XInputCapabilities()
                    {
                        type = 1,
                        subType = (XInputController.DeviceSubType)info->subType,
                        gamepad = new XInputGamepad()
                        {
                            leftStickX = (short)info->vendorId,
                            leftStickY = (short)info->productId,
                        }
                    })
                };
                m_RealDevice = new VariantRealDevice(description);
            }
            else if (StateReader.ReadReportIdState(stateEvent, out XboxOneWiredLegacyDeviceDisconnect* disconnect))
            {
                m_RealDevice?.Dispose();
                m_RealDevice = null;
            }
        }

        private void RequestDeviceInfo()
        {
            var requestInfo = XboxOneWiredLegacyRequestInfoCommand.Create();
            this.LoggedExecuteCommand(ref requestInfo);
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            RequestDeviceInfo();
        }
    }
}