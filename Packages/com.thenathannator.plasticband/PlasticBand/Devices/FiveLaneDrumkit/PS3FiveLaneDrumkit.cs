using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/PS3.md

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FiveLaneDrumkitState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "bluePad", layout = "Button", bit = 0)]
        [InputControl(name = "greenPad", layout = "Button", bit = 1)]
        [InputControl(name = "redPad", layout = "Button", bit = 2)]
        [InputControl(name = "yellowCymbal", layout = "Button", bit = 3)]

        [InputControl(name = "kick", layout = "Button", bit = 4)]
        [InputControl(name = "orangeCymbal", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", displayName = "Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", displayName = "Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        public fixed byte unused2[8];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        // - Try and pair velocity with pads directly
        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Velocity")]
        public byte yellowVelocity;

        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Velocity")]
        public byte redVelocity;

        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Velocity")]
        public byte greenVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Velocity")]
        public byte blueVelocity;

        [InputControl(name = "kickVelocity", layout = "Axis", displayName = "Kick Velocity")]
        public byte kickVelocity;

        [InputControl(name = "orangeVelocity", layout = "Axis", displayName = "Orange Velocity")]
        public byte orangeVelocity;

        public fixed byte unused3[10];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3FiveLaneDrumkitState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3FiveLaneDrumkitState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitState_NoReportId), hideInUI = true)]
    internal class PS3FiveLaneDrumkit_NoReportId : PS3FiveLaneDrumkit { }

    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitState_ReportId), hideInUI = true)]
    internal class PS3FiveLaneDrumkit_ReportId : PS3FiveLaneDrumkit { }
}

namespace PlasticBand.Devices
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && HIDROGEN_FORCE_REPORT_IDS)
    using DefaultState = PS3FiveLaneDrumkitState_ReportId;
#else
    using DefaultState = PS3FiveLaneDrumkitState_NoReportId;
#endif

    [InputControlLayout(stateType = typeof(DefaultState), displayName = "PlayStation 3 Guitar Hero Drumkit")]
    internal class PS3FiveLaneDrumkit : FiveLaneDrumkit
    {
        internal new static void Initialize()
        {
            HidReportIdLayoutFinder.RegisterLayout<PS3FiveLaneDrumkit,
                PS3FiveLaneDrumkit_ReportId, PS3FiveLaneDrumkit_NoReportId>(0x12BA, 0x0120);
        }
    }
}
