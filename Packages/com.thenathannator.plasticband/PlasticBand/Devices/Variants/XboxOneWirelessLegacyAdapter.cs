using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
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

        // GameInput doesn't pass on reports while the game doesn't have focus, so device info and disconnects can be
        // missed. Like Rock Band 4, device info is requested again whenever focus comes back, and slots the adapter
        // doesn't confirm within this long are taken to have disconnected meanwhile.
        internal const double kConfirmTimeout = 0.5;

        private readonly bool[] m_UnsupportedSlots = new bool[kMaxDevices];
        private readonly bool[] m_UnconfirmedSlots = new bool[kMaxDevices];
        private double m_ConfirmDeadline = double.NaN;

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

                m_UnconfirmedSlots[state->userIndex] = false;
                var device = m_RealDevices[state->userIndex];
                if (device == null)
                {
                    // Slots with a device we have no layout for will keep sending input, don't ask about them again
                    if (!m_UnsupportedSlots[state->userIndex])
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
                if (info->userIndex >= m_RealDevices.Count)
                {
                    Logging.Error($"Unexpected wireless legacy adapter user index {info->userIndex}! Should not be >= {m_RealDevices.Count}");
                    return;
                }

                m_UnconfirmedSlots[info->userIndex] = false;

                // The report data following the wireless legacy header matches that of the instruments,
                // so we create the corresponding layouts for those instruments
                string layout;
                switch (info->deviceType)
                {
                    case XboxOneWirelessLegacyDeviceType.Guitar: layout = nameof(XboxOneRockBandGuitar); break;
                    case XboxOneWirelessLegacyDeviceType.Drums: layout = nameof(XboxOneFourLaneDrumkit); break;
                    default:
                        Logging.Error($"Unexpected wireless legacy adapter device type {info->deviceType}!");
                        m_UnsupportedSlots[info->userIndex] = true;
                        m_RealDevices[info->userIndex]?.Dispose();
                        m_RealDevices[info->userIndex] = null;
                        return;
                }

                m_UnsupportedSlots[info->userIndex] = false;
                // Device info is sent for every slot whenever it's requested, keep the devices that are unchanged
                var existing = m_RealDevices[info->userIndex];
                if (existing != null && existing.layout == layout)
                    return;

                existing?.Dispose();
                m_RealDevices[info->userIndex] = new VariantRealDevice(layout);
            }
            else if (StateReader.ReadReportIdState(stateEvent, out XboxOneWirelessLegacyDeviceDisconnect* disconnect))
            {
                if (disconnect->userIndex >= m_RealDevices.Count)
                {
                    Logging.Error($"Unexpected wireless legacy adapter user index {disconnect->userIndex}! Should not be >= {m_RealDevices.Count}");
                    return;
                }

                m_RealDevices[disconnect->userIndex]?.Dispose();
                m_RealDevices[disconnect->userIndex] = null;
                m_UnsupportedSlots[disconnect->userIndex] = false;
                m_UnconfirmedSlots[disconnect->userIndex] = false;
            }
        }

        private void RequestDeviceInfo()
        {
            var requestInfo = XboxOneWirelessLegacyRequestInfoCommand.Create();
            this.LoggedExecuteCommand(ref requestInfo);
        }

        internal void OnFocusChanged(bool focused)
        {
            if (!focused)
                return;

            for (int i = 0; i < kMaxDevices; i++)
                m_UnconfirmedSlots[i] = m_RealDevices[i] != null;
            m_ConfirmDeadline = InputState.currentTime + kConfirmTimeout;
            RequestDeviceInfo();
        }

        // Devices can't be removed from OnNextUpdate, the input system is going through its devices at the time
        private void OnAfterUpdate()
        {
            if (double.IsNaN(m_ConfirmDeadline) || InputState.currentTime < m_ConfirmDeadline)
                return;

            m_ConfirmDeadline = double.NaN;
            for (int i = 0; i < kMaxDevices; i++)
            {
                if (!m_UnconfirmedSlots[i])
                    continue;

                m_UnconfirmedSlots[i] = false;
                m_RealDevices[i]?.Dispose();
                m_RealDevices[i] = null;
            }
        }

        protected override void OnAdded()
        {
            base.OnAdded();

            for (int i = 0; i < kMaxDevices; i++)
                m_RealDevices.Add(null);
            m_RealDevices.TrimExcess();

            Application.focusChanged += OnFocusChanged;
            InputSystem.onAfterUpdate += OnAfterUpdate;
            RequestDeviceInfo();
        }

        protected override void OnRemoved()
        {
            Application.focusChanged -= OnFocusChanged;
            InputSystem.onAfterUpdate -= OnAfterUpdate;
            base.OnRemoved();
        }
    }
}