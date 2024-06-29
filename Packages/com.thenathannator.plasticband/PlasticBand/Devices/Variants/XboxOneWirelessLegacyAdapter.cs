using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Rock%20Band%20Adapters/Xbox%20One%20Wireless%20Legacy%20Adapter.md

namespace PlasticBand.Devices
{
    internal enum XboxOneWirelessLegacyDeviceType : byte
    {
        Guitar = 1,
        Drums = 2,
    }

    #region Input reports
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneWirelessLegacyState : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x20;

        public byte reportId;
        public GameInputButton buttons;

        public byte userIndex;
        public XboxOneWirelessLegacyDeviceType deviceType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneWirelessLegacyDeviceInfo : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x22;

        public byte reportId;
        public byte userIndex;
        public XboxOneWirelessLegacyDeviceType deviceType;
        private ushort m_VendorId; // Big-endian
        private byte unknown;
        private byte m_XInputSubType;
        // Too complicated to handle when we don't need it
        // private unsafe fixed char name[124];

        public ushort vendorId => (ushort)((m_VendorId << 8) | (m_VendorId >> 8));
        public ushort xinputSubType => (byte)(m_XInputSubType & 0x7F);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneWirelessLegacyDeviceDisconnect : IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x23;

        public byte reportId;
        public byte userIndex;
    }
    #endregion

    #region Output reports
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal unsafe struct XboxOneWirelessLegacySetState : IInputDeviceCommandInfo
    {
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte);
        internal const byte kReportId = 0x21;

        public FourCC typeStatic => GameInputDefinitions.OutputFormat;

        public InputDeviceCommand baseCommand;
        public byte reportId;
        public byte userIndex;
        public byte unknown;

        public static XboxOneWirelessLegacySetState Create() => new XboxOneWirelessLegacySetState()
        {
            baseCommand = new InputDeviceCommand(GameInputDefinitions.OutputFormat, kSize),
            reportId = kReportId,
        };
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal struct XboxOneWirelessLegacyRequestInfoCommand : IInputDeviceCommandInfo
    {
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte);
        internal const byte kReportId = 0x24;

        public FourCC typeStatic => GameInputDefinitions.OutputFormat;

        public InputDeviceCommand baseCommand;
        public byte reportId;

        public static XboxOneWirelessLegacyRequestInfoCommand Create() => new XboxOneWirelessLegacyRequestInfoCommand()
        {
            baseCommand = new InputDeviceCommand(GameInputDefinitions.OutputFormat, kSize),
            reportId = kReportId,
        };
    }
    #endregion

    [InputControlLayout(stateType = typeof(VariantDeviceDummyState), hideInUI = true)]
    internal class XboxOneWirelessLegacyAdapter : VariantMultiDevice
    {
        private const int kMaxDevices = 4;

        internal static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneWirelessLegacyAdapter>(0x0738, 0x4164);
        }

        protected override unsafe void OnStateEvent(InputEventPtr eventPtr)
        {
            if (eventPtr.type != StateEvent.Type)
                return;

            var stateEvent = StateEvent.From(eventPtr);
            if (StateReader.ReadReportIdState(stateEvent, out XboxOneWirelessLegacyState* state))
            {
                if (state->userIndex >= m_RealDevices.Count)
                {
                    Logging.Error($"Unexpected wireless legacy adapter user index {state->userIndex}! Should not be >= {m_RealDevices.Count}");
                    return;
                }

                var device = m_RealDevices[state->userIndex];
                if (device == null)
                {
                    RequestDeviceInfo();
                    return;
                }

                // The report data following the wireless legacy header matches that of the instruments,
                // so we take just that and shove the report ID one byte before it
                int realStateSize = (int)stateEvent->stateSizeInBytes - sizeof(XboxOneWirelessLegacyState) + 1;
                byte* realState = (byte*)stateEvent->state + sizeof(XboxOneWirelessLegacyState) - 1;
                *realState = state->reportId;

                device.OnStateEvent(stateEvent->stateFormat, realState, realStateSize);
            }
            else if (StateReader.ReadReportIdState(stateEvent, out XboxOneWirelessLegacyDeviceInfo* info))
            {
                if (state->userIndex >= m_RealDevices.Count)
                {
                    Logging.Error($"Unexpected wireless legacy adapter user index {state->userIndex}! Should not be >= {m_RealDevices.Count}");
                    return;
                }

                m_RealDevices[state->userIndex]?.Dispose();

                // The report data following the wireless legacy header matches that of the instruments,
                // so we create the corresponding layouts for those instruments
                string layout;
                switch (info->deviceType)
                {
                    case XboxOneWirelessLegacyDeviceType.Guitar: layout = nameof(XboxOneRockBandGuitar); break;
                    case XboxOneWirelessLegacyDeviceType.Drums: layout = nameof(XboxOneFourLaneDrumkit); break;
                    default:
                        Logging.Error($"Unexpected wireless legacy adapter device type {info->deviceType}!");
                        return;
                }

                m_RealDevices[state->userIndex] = new VariantRealDevice(layout);
            }
            else if (StateReader.ReadReportIdState(stateEvent, out XboxOneWirelessLegacyDeviceDisconnect* disconnect))
            {
                if (state->userIndex >= m_RealDevices.Count)
                {
                    Logging.Error($"Unexpected wireless legacy adapter user index {state->userIndex}! Should not be >= {m_RealDevices.Count}");
                    return;
                }

                m_RealDevices[state->userIndex]?.Dispose();
                m_RealDevices[state->userIndex] = null;
            }
        }

        private void RequestDeviceInfo()
        {
            var requestInfo = XboxOneWirelessLegacyRequestInfoCommand.Create();
            this.LoggedExecuteCommand(ref requestInfo);
        }

        protected override void OnAdded()
        {
            base.OnAdded();

            for (int i = 0; i < kMaxDevices; i++)
                m_RealDevices.Add(null);
            m_RealDevices.TrimExcess();

            RequestDeviceInfo();
        }
    }
}