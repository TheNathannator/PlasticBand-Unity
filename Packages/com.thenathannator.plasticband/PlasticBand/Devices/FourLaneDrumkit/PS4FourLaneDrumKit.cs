using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 77)]
    internal struct PS4FourLaneDrumkitState_NoReportId : IFourLaneDrumkitState_Distinct
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(4)]
        public ushort buttons1;

        [FieldOffset(6)]
        public byte buttons2;

        [FieldOffset(42)]
        public byte redPad;

        [FieldOffset(43)]
        public byte bluePad;

        [FieldOffset(44)]
        public byte yellowPad;

        [FieldOffset(45)]
        public byte greenPad;

        [FieldOffset(46)]
        public byte yellowCymbal;

        [FieldOffset(47)]
        public byte blueCymbal;

        [FieldOffset(48)]
        public byte greenCymbal;

        public bool west => (buttons1 & 0x0010) != 0;
        public bool south => (buttons1 & 0x0020) != 0;
        public bool east => (buttons1 & 0x0040) != 0;
        public bool north => (buttons1 & 0x0080) != 0;

        public bool kick1 => (buttons1 & 0x0100) != 0;
        public bool kick2 => (buttons1 & 0x0200) != 0;

        public bool select => (buttons1 & 0x1000) != 0;
        public bool start => (buttons1 & 0x2000) != 0;
        public bool system => (buttons2 & 0x01) != 0;

        public bool dpadUp => ((HidDpad)(buttons1 & 0x0F)).IsUp();
        public bool dpadRight => ((HidDpad)(buttons1 & 0x0F)).IsRight();
        public bool dpadDown => ((HidDpad)(buttons1 & 0x0F)).IsDown();
        public bool dpadLeft => ((HidDpad)(buttons1 & 0x0F)).IsLeft();

        byte IFourLaneDrumkitState_Distinct.redPad => redPad;
        byte IFourLaneDrumkitState_Distinct.yellowPad => yellowPad;
        byte IFourLaneDrumkitState_Distinct.bluePad => bluePad;
        byte IFourLaneDrumkitState_Distinct.greenPad => greenPad;
        byte IFourLaneDrumkitState_Distinct.yellowCymbal => yellowCymbal;
        byte IFourLaneDrumkitState_Distinct.blueCymbal => blueCymbal;
        byte IFourLaneDrumkitState_Distinct.greenCymbal => greenCymbal;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4FourLaneDrumkitState_ReportId : IFourLaneDrumkitState_Distinct
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4FourLaneDrumkitState_NoReportId state;

        public bool south => state.south;
        public bool east => state.east;
        public bool west => state.west;
        public bool north => state.north;

        public bool dpadUp => state.dpadUp;
        public bool dpadDown => state.dpadDown;
        public bool dpadLeft => state.dpadLeft;
        public bool dpadRight => state.dpadRight;

        public bool start => state.start;
        public bool select => state.select;
        public bool system => state.system;

        public byte redPad => state.redPad;
        public byte yellowPad => state.yellowPad;
        public byte bluePad => state.bluePad;
        public byte greenPad => state.greenPad;
        public byte yellowCymbal => state.yellowCymbal;
        public byte blueCymbal => state.blueCymbal;
        public byte greenCymbal => state.greenCymbal;

        public bool kick1 => state.kick1;
        public bool kick2 => state.kick2;
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 4 Rock Band Drumkit")]
    internal class PS4FourLaneDrumkit : TranslatingFourLaneDrumkit_Distinct<PS4FourLaneDrumkitState_NoReportId>
    {
        internal new static void Initialize()
        {
            // MadCatz
            HidLayoutFinder.RegisterLayout<PS4FourLaneDrumkit, PS4FourLaneDrumkit_NoReportId>(0x0738, 0x8262, reportIdDefault: true);

            // PDP
            HidLayoutFinder.RegisterLayout<PS4FourLaneDrumkit, PS4FourLaneDrumkit_NoReportId>(0x0E6F, 0x0174, reportIdDefault: true);
        }
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), hideInUI = true)]
    internal class PS4FourLaneDrumkit_NoReportId : TranslatingFourLaneDrumkit_Distinct<PS4FourLaneDrumkitState_ReportId> { }
}
