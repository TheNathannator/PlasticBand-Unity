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
    /// Button flags for Santroller V2 devices.
    /// Bits 0-3 contain the 4-bit HidDpad hat switch.
    /// Bits 4-15 contain standard button flags matching XInput/Santroller V2 button layout.
    /// </summary>
    [System.Flags]
    internal enum SantrollerV2Button : ushort
    {
        None = 0,

        DpadMask = 0x000F,

        Start = 0x0010,
        Select = 0x0020,
        Back = 0x0020,
        LeftStick = 0x0040,
        LeftThumb = 0x0040,
        RightStick = 0x0080,
        RightThumb = 0x0080,

        LeftShoulder = 0x0100,
        RightShoulder = 0x0200,
        Guide = 0x0400,
        System = 0x0400,
        Misc = 0x0800,
        Capture = 0x0800,

        South = 0x1000,
        A = 0x1000,
        East = 0x2000,
        B = 0x2000,
        West = 0x4000,
        X = 0x4000,
        North = 0x8000,
        Y = 0x8000,
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
        public SantrollerV2Button buttons;
        public byte leftTrigger;
        public byte rightTrigger;
        public short leftStickX;
        public short leftStickY;
        public short rightStickX;
        public short rightStickY;

        public HidDpad dpad => (HidDpad)((ushort)buttons & (ushort)SantrollerV2Button.DpadMask);
        public bool dpadUp => dpad.IsUp();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();
        public bool dpadRight => dpad.IsRight();

        public bool start => (buttons & SantrollerV2Button.Start) != 0;
        public bool select => (buttons & SantrollerV2Button.Back) != 0;
        public bool back => select;
        public bool leftStickClick => (buttons & SantrollerV2Button.LeftThumb) != 0;
        public bool rightStickClick => (buttons & SantrollerV2Button.RightThumb) != 0;

        public bool leftShoulder => (buttons & SantrollerV2Button.LeftShoulder) != 0;
        public bool rightShoulder => (buttons & SantrollerV2Button.RightShoulder) != 0;
        public bool guide => (buttons & SantrollerV2Button.Guide) != 0;
        public bool misc => (buttons & SantrollerV2Button.Misc) != 0;
        public bool south => (buttons & SantrollerV2Button.South) != 0;
        public bool east => (buttons & SantrollerV2Button.East) != 0;
        public bool west => (buttons & SantrollerV2Button.West) != 0;
        public bool north => (buttons & SantrollerV2Button.North) != 0;
    }
}
