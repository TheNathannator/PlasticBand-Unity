using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 27)]
    internal struct PS3WiiFourLaneDrumkitState_NoReportId : IFourLaneDrumkitState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public ushort buttons;
        public byte dpad;

        private unsafe fixed byte unused2[8];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        public byte yellowVelocity;
        public byte redVelocity;
        public byte greenVelocity;
        public byte blueVelocity;

        public bool west => (buttons & 0x0001) != 0;
        public bool south => (buttons & 0x0002) != 0;
        public bool east => (buttons & 0x0004) != 0;
        public bool north => (buttons & 0x0008) != 0;

        public bool red => east;
        public bool yellow => north;
        public bool blue => west;
        public bool green => south;

        public bool kick1 => (buttons & 0x0010) != 0;
        public bool kick2 => (buttons & 0x0020) != 0;

        public bool select => (buttons & 0x0100) != 0;
        public bool start => (buttons & 0x0200) != 0;
        public bool system => (buttons & 0x1000) != 0;

        public bool pad => (buttons & 0x0400) != 0;
        public bool cymbal => (buttons & 0x0800) != 0;

        public bool dpadUp => dpad == 7 || dpad <= 1;
        public bool dpadRight => dpad >= 1 && dpad <= 3;
        public bool dpadDown => dpad >= 3 && dpad <= 5;
        public bool dpadLeft => dpad >= 5 && dpad <= 7;

        public byte redPadVelocity => redVelocity;
        public byte yellowPadVelocity => yellowVelocity;
        public byte bluePadVelocity => blueVelocity;
        public byte greenPadVelocity => greenVelocity;
        public byte yellowCymbalVelocity => yellowVelocity;
        public byte blueCymbalVelocity => blueVelocity;
        public byte greenCymbalVelocity => greenVelocity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3WiiFourLaneDrumkitState_ReportId : IFourLaneDrumkitState_Flags
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiFourLaneDrumkitState_NoReportId state;

        public bool west => state.west;
        public bool south => state.south;
        public bool east => state.east;
        public bool north => state.north;

        public bool red => state.red;
        public bool yellow => state.yellow;
        public bool blue => state.blue;
        public bool green => state.green;

        public bool kick1 => state.kick1;
        public bool kick2 => state.kick2;

        public bool select => state.select;
        public bool start => state.start;

        public bool pad => state.pad;
        public bool cymbal => state.cymbal;

        public bool system => state.system;

        public bool dpadUp => state.dpadUp;
        public bool dpadDown => state.dpadDown;
        public bool dpadLeft => state.dpadLeft;
        public bool dpadRight => state.dpadRight;

        public byte redPadVelocity => state.redPadVelocity;
        public byte yellowPadVelocity => state.yellowPadVelocity;
        public byte bluePadVelocity => state.bluePadVelocity;
        public byte greenPadVelocity => state.greenPadVelocity;
        public byte yellowCymbalVelocity => state.yellowCymbalVelocity;
        public byte blueCymbalVelocity => state.blueCymbalVelocity;
        public byte greenCymbalVelocity => state.greenCymbalVelocity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PSFourLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFourLaneState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedFourLaneButton.South, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedFourLaneButton.East, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedFourLaneButton.West, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedFourLaneButton.North, displayName = "Triangle")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFourLaneButton.System, displayName = "PlayStation")]
        public TranslatedFourLaneState state;
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 3 Rock Band Drumkit")]
    internal class PS3FourLaneDrumkit : TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_NoReportId>
    {
        internal new static void Initialize()
        {
            // Drumkit
            HidLayoutFinder.RegisterLayout<PS3FourLaneDrumkit_ReportId, PS3FourLaneDrumkit>(0x12BA, 0x0210);

            // MIDI Pro Adapter
            HidLayoutFinder.RegisterLayout<PS3FourLaneDrumkit_ReportId, PS3FourLaneDrumkit>(0x12BA, 0x0218);
        }
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), hideInUI = true)]
    internal class PS3FourLaneDrumkit_ReportId : TranslatingFourLaneDrumkit_Flags<PS3WiiFourLaneDrumkitState_ReportId> { }
}
