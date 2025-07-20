using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal struct PS4FourLaneDrumkitState_NoReportId : IFourLaneDrumkitState_DistinctVelocities
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(4)]
        public PS4Button1 buttons1;

        [FieldOffset(6)]
        public PS4Button2 buttons2;

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

        public bool south
        {
            get => (buttons1 & PS4Button1.Cross) != 0;
            set => buttons1.SetBit(PS4Button1.Cross, value);
        }

        public bool east
        {
            get => (buttons1 & PS4Button1.Circle) != 0;
            set => buttons1.SetBit(PS4Button1.Circle, value);
        }

        public bool west
        {
            get => (buttons1 & PS4Button1.Square) != 0;
            set => buttons1.SetBit(PS4Button1.Square, value);
        }

        public bool north
        {
            get => (buttons1 & PS4Button1.Triangle) != 0;
            set => buttons1.SetBit(PS4Button1.Triangle, value);
        }

        public bool kick1
        {
            get => (buttons1 & PS4Button1.L2) != 0;
            set => buttons1.SetBit(PS4Button1.L2, value);
        }

        public bool kick2
        {
            get => (buttons1 & PS4Button1.R2) != 0;
            set => buttons1.SetBit(PS4Button1.R2, value);
        }

        public bool select
        {
            get => (buttons1 & PS4Button1.Select) != 0;
            set => buttons1.SetBit(PS4Button1.Select, value);
        }

        public bool start
        {
            get => (buttons1 & PS4Button1.Start) != 0;
            set => buttons1.SetBit(PS4Button1.Start, value);
        }

        public bool system
        {
            get => (buttons2 & PS4Button2.PlayStation) != 0;
            set => buttons2.SetBit(PS4Button2.PlayStation, value);
        }

        public bool dpadUp
        {
            get => buttons1.GetDpad().IsUp();
            set => buttons1.SetDpadUp(value);
        }

        public bool dpadRight
        {
            get => buttons1.GetDpad().IsRight();
            set => buttons1.SetDpadRight(value);
        }

        public bool dpadDown
        {
            get => buttons1.GetDpad().IsDown();
            set => buttons1.SetDpadDown(value);
        }

        public bool dpadLeft
        {
            get => buttons1.GetDpad().IsLeft();
            set => buttons1.SetDpadLeft(value);
        }

        byte IFourLaneDrumkitState_DistinctVelocities.redPadVelocity
        {
            get => redPad;
            set => redPad = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.yellowPadVelocity
        {
            get => yellowPad;
            set => yellowPad = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.bluePadVelocity
        {
            get => bluePad;
            set => bluePad = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.greenPadVelocity
        {
            get => greenPad;
            set => greenPad = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.yellowCymbalVelocity
        {
            get => yellowCymbal;
            set => yellowCymbal = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.blueCymbalVelocity
        {
            get => blueCymbal;
            set => blueCymbal = value;
        }

        byte IFourLaneDrumkitState_DistinctVelocities.greenCymbalVelocity
        {
            get => greenCymbal;
            set => greenCymbal = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4FourLaneDrumkitState_ReportId : IFourLaneDrumkitState_DistinctVelocities
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4FourLaneDrumkitState_NoReportId state;

        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool west { get => state.west; set => state.west = value; }
        public bool north { get => state.north; set => state.north = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public byte redPadVelocity { get => state.redPad; set => state.redPad = value; }
        public byte yellowPadVelocity { get => state.yellowPad; set => state.yellowPad = value; }
        public byte bluePadVelocity { get => state.bluePad; set => state.bluePad = value; }
        public byte greenPadVelocity { get => state.greenPad; set => state.greenPad = value; }
        public byte yellowCymbalVelocity { get => state.yellowCymbal; set => state.yellowCymbal = value; }
        public byte blueCymbalVelocity { get => state.blueCymbal; set => state.blueCymbal = value; }
        public byte greenCymbalVelocity { get => state.greenCymbal; set => state.greenCymbal = value; }

        public bool kick1 { get => state.kick1; set => state.kick1 = value; }
        public bool kick2 { get => state.kick2; set => state.kick2 = value; }
    }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 4 Rock Band Drumkit", hideInUI = true)]
    internal class PS4FourLaneDrumkit_NoReportId : TranslatingFourLaneDrumkit_Distinct<PS4FourLaneDrumkitState_NoReportId> { }

    [InputControlLayout(stateType = typeof(PSFourLaneDrumkitLayout), displayName = "PlayStation 4 Rock Band Drumkit")]
    internal class PS4FourLaneDrumkit : TranslatingFourLaneDrumkit_Distinct<PS4FourLaneDrumkitState_ReportId>
    {
        internal new static void Initialize()
        {
            // MadCatz
            HidLayoutFinder.RegisterLayout<PS4FourLaneDrumkit, PS4FourLaneDrumkit_NoReportId>(0x0738, 0x8262, reportIdDefault: true);

            // PDP
            HidLayoutFinder.RegisterLayout<PS4FourLaneDrumkit, PS4FourLaneDrumkit_NoReportId>(0x0E6F, 0x0174, reportIdDefault: true);
        }
    }
}
