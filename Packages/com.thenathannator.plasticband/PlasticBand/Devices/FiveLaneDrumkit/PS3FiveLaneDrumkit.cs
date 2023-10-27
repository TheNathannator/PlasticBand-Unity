using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/PS3.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 27)]
    internal unsafe struct PS3FiveLaneDrumkitState_NoReportId : IFiveLaneDrumkitState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public ushort buttons;
        public byte dpad;

        public fixed byte unused2[8];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        public byte yellowVelocity;
        public byte redVelocity;
        public byte greenVelocity;
        public byte blueVelocity;
        public byte kickVelocity;
        public byte orangeVelocity;

        public bool west => (buttons & 0x0001) != 0;
        public bool south => (buttons & 0x0002) != 0;
        public bool east => (buttons & 0x0004) != 0;
        public bool north => (buttons & 0x0008) != 0;

        public bool red => east;
        public bool yellow => north;
        public bool blue => west;
        public bool green => south;
        public bool kick => (buttons & 0x0010) != 0;
        public bool orange => (buttons & 0x0020) != 0;

        public bool select => (buttons & 0x0100) != 0;
        public bool start => (buttons & 0x0200) != 0;
        public bool system => (buttons & 0x1000) != 0;

        public bool dpadUp => dpad == 7 || dpad <= 1;
        public bool dpadRight => dpad >= 1 && dpad <= 3;
        public bool dpadDown => dpad >= 3 && dpad <= 5;
        public bool dpadLeft => dpad >= 5 && dpad <= 7;

        byte IFiveLaneDrumkitState.redVelocity => redVelocity;
        byte IFiveLaneDrumkitState.yellowVelocity => yellowVelocity;
        byte IFiveLaneDrumkitState.blueVelocity => blueVelocity;
        byte IFiveLaneDrumkitState.orangeVelocity => orangeVelocity;
        byte IFiveLaneDrumkitState.greenVelocity => greenVelocity;
        byte IFiveLaneDrumkitState.kickVelocity => kickVelocity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FiveLaneDrumkitState_ReportId : IFiveLaneDrumkitState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3FiveLaneDrumkitState_NoReportId state;

        public bool west => state.west;
        public bool south => state.south;
        public bool east => state.east;
        public bool north => state.north;

        public bool red => state.red;
        public bool yellow => state.yellow;
        public bool blue => state.blue;
        public bool green => state.green;
        public bool orange => state.orange;
        public bool kick => state.kick;

        public bool select => state.select;
        public bool start => state.start;
        public bool system => state.system;

        public bool dpadUp => state.dpadUp;
        public bool dpadDown => state.dpadDown;
        public bool dpadLeft => state.dpadLeft;
        public bool dpadRight => state.dpadRight;

        public byte redVelocity => state.redVelocity;
        public byte yellowVelocity => state.yellowVelocity;
        public byte blueVelocity => state.blueVelocity;
        public byte orangeVelocity => state.orangeVelocity;
        public byte greenVelocity => state.greenVelocity;
        public byte kickVelocity => state.kickVelocity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3FiveLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFiveLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFiveLaneButton.South, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFiveLaneButton.East, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFiveLaneButton.West, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFiveLaneButton.North, displayName = "Triangle")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFiveLaneButton.System, displayName = "PlayStation")]
        public TranslatedFiveLaneState state;
    }

    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitLayout), displayName = "PlayStation 3 Guitar Hero Drumkit")]
    internal class PS3FiveLaneDrumkit : TranslatingFiveLaneDrumkit<PS3FiveLaneDrumkitState_NoReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3FiveLaneDrumkit_ReportId, PS3FiveLaneDrumkit>(0x12BA, 0x0120);
        }
    }

    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitLayout), hideInUI = true)]
    internal class PS3FiveLaneDrumkit_ReportId : TranslatingFiveLaneDrumkit<PS3FiveLaneDrumkitState_ReportId> { }
}
