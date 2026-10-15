using System.Runtime.InteropServices;
using PlasticBand.Devices;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{

    /// <summary>
    /// Output report to request features/capabilities from a V2 Santroller device (Report ID 0x10).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = kSize)]
    internal struct SantrollerRequestFeaturesCommand : IInputDeviceCommandInfo
    {
        internal const int kSize = InputDeviceCommand.BaseCommandSize + sizeof(byte) * 2;
        internal const byte kReportId = 0x10;

        public FourCC typeStatic => HidDefinitions.OutputFormat;

        public InputDeviceCommand baseCommand;
        public byte reportId;
        public byte dummy;

        public static SantrollerRequestFeaturesCommand Create() => new SantrollerRequestFeaturesCommand()
        {
            baseCommand = new InputDeviceCommand(HidDefinitions.OutputFormat, kSize),
            reportId = kReportId,
            dummy = 0x00,
        };
    }

    /// <summary>
    /// Input report response containing device subtype and capabilities (Report ID 0x10, 3 bytes).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SantrollerFeaturesResponse : IReportIdStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x10;

        public byte reportId;
        public SantrollerDeviceType subType;
        public SantrollerCapabilities capabilities;
    }

    /// <summary>
    /// Unified V2 Santroller input report (Report ID 0x01, 14 bytes).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SantrollerV2HIDReport : IReportIdStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x01;

        public byte reportId;
        public byte reserved;
        public byte buttons1;
        public byte buttons2;
        public byte leftTrigger;
        public byte rightTrigger;
        public short leftStickX;
        public short leftStickY;
        public short rightStickX;
        public short rightStickY;

        public HidDpad dpad => (HidDpad)(buttons1 & 0x0F);
        public bool dpadUp => dpad.IsUp();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();
        public bool dpadRight => dpad.IsRight();

        public bool start => (buttons1 & 0x10) != 0;
        public bool select => (buttons1 & 0x20) != 0;
        public bool back => select;
        public bool leftStickClick => (buttons1 & 0x40) != 0;
        public bool rightStickClick => (buttons1 & 0x80) != 0;

        public bool leftShoulder => (buttons2 & 0x01) != 0;
        public bool rightShoulder => (buttons2 & 0x02) != 0;
        public bool guide => (buttons2 & 0x04) != 0;
        public bool misc => (buttons2 & 0x08) != 0;
        public bool south => (buttons2 & 0x10) != 0;
        public bool east => (buttons2 & 0x20) != 0;
        public bool west => (buttons2 & 0x40) != 0;
        public bool north => (buttons2 & 0x80) != 0;
    }
}
