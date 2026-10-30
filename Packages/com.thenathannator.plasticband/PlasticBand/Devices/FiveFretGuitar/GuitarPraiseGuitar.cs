using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Praise%20Guitar.md

namespace PlasticBand.Devices
{
    [Flags]
    internal enum GuitarPraiseButton : ushort
    {
        None = 0x0008, // D-pad neutral state is 8

        DpadMask = 0x000F,

        Green = 0x0010,
        Red = 0x0020,
        Yellow = 0x0040,
        Blue = 0x0080,
        Orange = 0x0100,

        Tilt = 0x0200,

        Start = 0x0400,
        Select = 0x0800,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct GuitarPraiseGuitarState_NoReportId : IFiveFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        private fixed byte unused1[3];

        [InputControl(name = "whammy", layout = "IntAxis", parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0xFF")]
        private byte m_Whammy;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "greenFret", layout = "Button", bit = 4)]
        [InputControl(name = "redFret", layout = "Button", bit = 5)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 6)]
        [InputControl(name = "blueFret", layout = "Button", bit = 7)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]
        [InputControl(name = "tilt", layout = "Button", bit = 9)]
        [InputControl(name = "start", layout = "Button", bit = 10)]
        [InputControl(name = "select", layout = "Button", bit = 11)]
        public GuitarPraiseButton buttons;

        private byte unused2;

        public bool green
        {
            get => (buttons & GuitarPraiseButton.Green) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Green, value);
        }

        public bool red
        {
            get => (buttons & GuitarPraiseButton.Red) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Red, value);
        }

        public bool yellow
        {
            get => (buttons & GuitarPraiseButton.Yellow) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Yellow, value);
        }

        public bool blue
        {
            get => (buttons & GuitarPraiseButton.Blue) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Blue, value);
        }

        public bool orange
        {
            get => (buttons & GuitarPraiseButton.Orange) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Orange, value);
        }

        public bool dpadUp
        {
            get => GetDpad(buttons).IsUp();
            set => SetDpad(ref buttons, GetDpad(buttons).ChangeUp(value));
        }

        public bool dpadDown
        {
            get => GetDpad(buttons).IsDown();
            set => SetDpad(ref buttons, GetDpad(buttons).ChangeDown(value));
        }

        public bool dpadLeft
        {
            get => GetDpad(buttons).IsLeft();
            set => SetDpad(ref buttons, GetDpad(buttons).ChangeLeft(value));
        }

        public bool dpadRight
        {
            get => GetDpad(buttons).IsRight();
            set => SetDpad(ref buttons, GetDpad(buttons).ChangeRight(value));
        }

        public bool start
        {
            get => (buttons & GuitarPraiseButton.Start) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Start, value);
        }

        public bool select
        {
            get => (buttons & GuitarPraiseButton.Select) != 0;
            set => SetBit(ref buttons, GuitarPraiseButton.Select, value);
        }

        public bool system
        {
            get => false;
            set { }
        }

        public byte whammy
        {
            get => (byte)(byte.MaxValue - m_Whammy);
            set => m_Whammy = (byte)(byte.MaxValue - value);
        }

        public sbyte tilt
        {
            get => (buttons & GuitarPraiseButton.Tilt) != 0 ? sbyte.MaxValue : (sbyte)0;
            set => SetBit(ref buttons, GuitarPraiseButton.Tilt, value >= 64);
        }

        private static HidDpad GetDpad(GuitarPraiseButton buttons)
        {
            return (HidDpad)(buttons & GuitarPraiseButton.DpadMask);
        }

        private static void SetDpad(ref GuitarPraiseButton buttons, HidDpad dpad)
        {
            buttons = (buttons & ~GuitarPraiseButton.DpadMask) | ((GuitarPraiseButton)dpad & GuitarPraiseButton.DpadMask);
        }

        private static void SetBit(ref GuitarPraiseButton buttons, GuitarPraiseButton mask, bool set)
        {
            if (set)
                buttons |= mask;
            else
                buttons &= ~mask;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct GuitarPraiseGuitarState_ReportId : IFiveFretGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public GuitarPraiseGuitarState_NoReportId state;

        public bool green { get => state.green; set => state.green = value; }
        public bool red { get => state.red; set => state.red = value; }
        public bool yellow { get => state.yellow; set => state.yellow = value; }
        public bool blue { get => state.blue; set => state.blue = value; }
        public bool orange { get => state.orange; set => state.orange = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public byte whammy { get => state.whammy; set => state.whammy = value; }
        public sbyte tilt { get => state.tilt; set => state.tilt = value; }
    }

    [InputControlLayout(stateType = typeof(GuitarPraiseGuitarState_ReportId), displayName = "Guitar Praise Guitar")]
    internal class GuitarPraiseGuitar : FiveFretGuitar
    {
        internal new static void Initialize()
        {
            // Guitar Praise Guitar for PC
            HidLayoutFinder.RegisterLayout<GuitarPraiseGuitar, GuitarPraiseGuitar_NoReportId>(0x0314, 0x0830);
        }
    }

    [InputControlLayout(stateType = typeof(GuitarPraiseGuitarState_NoReportId), displayName = "Guitar Praise Guitar", hideInUI = true)]
    internal class GuitarPraiseGuitar_NoReportId : GuitarPraiseGuitar { }
}
