using System;
using System.Runtime.InteropServices;
using PlasticBand.Controls;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/PS3.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS3GuitarHeroGuitarState_NoReportId : IGuitarHeroGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        public PS3Button buttons;

        [FieldOffset(2)]
        public HidDpad dpad;

        [FieldOffset(5)]
        public byte m_Whammy;

        [FieldOffset(6)]
        public byte m_TouchBar;

        [FieldOffset(19)]
        public short m_Tilt;

        public bool green
        {
            get => (buttons & PS3Button.Cross) != 0;
            set => buttons.SetBit(PS3Button.Cross, value);
        }

        public bool red
        {
            get => (buttons & PS3Button.Circle) != 0;
            set => buttons.SetBit(PS3Button.Circle, value);
        }

        public bool yellow
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

        public bool blue
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool orange
        {
            get => (buttons & PS3Button.L2) != 0;
            set => buttons.SetBit(PS3Button.L2, value);
        }

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => dpad.SetUp(value);
        }

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => dpad.SetRight(value);
        }

        public bool dpadDown
        {
            get => dpad.IsDown();
            set => dpad.SetDown(value);
        }

        public bool dpadLeft
        {
            get => dpad.IsLeft();
            set => dpad.SetLeft(value);
        }

        public bool select
        {
            get => (buttons & PS3Button.Select) != 0;
            set => buttons.SetBit(PS3Button.Select, value);
        }

        public bool start
        {
            get => (buttons & PS3Button.Start) != 0;
            set => buttons.SetBit(PS3Button.Start, value);
        }

        public bool system
        {
            get => (buttons & PS3Button.PlayStation) != 0;
            set => buttons.SetBit(PS3Button.PlayStation, value);
        }

        public byte whammy
        {
            get => (byte) Math.Max((m_Whammy - 0x80) * 2, 0);
            set => m_Whammy = (byte)((value / 2) + 0x80);
        }

        public sbyte tilt
        {
            get => (sbyte) IntegerAxisControl.Convert(m_Tilt, 0x180, 0x280, 0x200, sbyte.MinValue, sbyte.MaxValue, 0);
            set => m_Tilt = (short) IntegerAxisControl.Convert(value, sbyte.MinValue, sbyte.MaxValue, 0, 0x180, 0x280, 0x200);
        }

        public bool spPedal
        {
            get => (buttons & PS3Button.R2) != 0;
            set => buttons.SetBit(PS3Button.R2, value);
        }

        public byte rawTouchBar
        {
            get => m_TouchBar;
            set => m_TouchBar = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3GuitarHeroGuitarState_ReportId : IGuitarHeroGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3GuitarHeroGuitarState_NoReportId state;

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
        public bool spPedal { get => state.spPedal; set => state.spPedal = value; }
        public byte rawTouchBar { get => state.rawTouchBar; set => state.rawTouchBar = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3GuitarHeroGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedGuitarHeroGuitarState.Format;

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.System, displayName = "PlayStation")]
        public TranslatedGuitarHeroGuitarState state;
    }

    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarLayout), displayName = "PlayStation 3 Guitar Hero Guitar")]
    internal class PS3GuitarHeroGuitar : TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_NoReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3GuitarHeroGuitar_ReportId, PS3GuitarHeroGuitar>(0x12BA, 0x0100);
            HidLayoutFinder.RegisterLayout<PS3GuitarHeroGuitar_GH5, PS3GuitarHeroGuitar_GH5_ReportId>(
                0x12BA, 0x0100, productName: "Guitar Hero5 for PlayStation (R) 3"
            );
        }
    }

    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarLayout), displayName = "PlayStation 3 Guitar Hero Guitar", hideInUI = true)]
    internal class PS3GuitarHeroGuitar_ReportId : TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_ReportId> { }

    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarLayout), displayName = "PlayStation 3 Guitar Hero Guitar", hideInUI = true)]
    internal class PS3GuitarHeroGuitar_GH5 : TranslatingGuitarHeroGuitar_Discrete<PS3GuitarHeroGuitarState_NoReportId> { }

    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarLayout), displayName = "PlayStation 3 Guitar Hero Guitar", hideInUI = true)]
    internal class PS3GuitarHeroGuitar_GH5_ReportId : TranslatingGuitarHeroGuitar_Discrete<PS3GuitarHeroGuitarState_ReportId> { }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "PC Guitar Hero Guitar")]
    internal class PCGuitarHeroGuitar : TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_NoReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PCGuitarHeroGuitar_ReportId, PCGuitarHeroGuitar>(0x1430, 0x474C);
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), displayName = "PC Guitar Hero Guitar", hideInUI = true)]
    internal class PCGuitarHeroGuitar_ReportId : TranslatingGuitarHeroGuitar_Ranges<PS3GuitarHeroGuitarState_NoReportId> { }
}
