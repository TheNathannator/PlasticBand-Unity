using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS4RockBandGuitarState_NoReportId : IRockBandGuitarState_Distinct
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(4)]
        public PS4Button1 buttons1;

        [FieldOffset(6)]
        public PS4Button2 buttons2;

        [FieldOffset(42)]
        public byte pickupSwitch;

        [FieldOffset(43)]
        public byte whammy;

        [FieldOffset(44)]
        private byte m_Tilt;

        [FieldOffset(45)]
        public byte frets;

        [FieldOffset(46)]
        public byte soloFrets;

        public bool green
        {
            get => (frets & 0x01) != 0;
            set => frets.SetBit(0x01, value);
        }

        public bool red
        {
            get => (frets & 0x02) != 0;
            set => frets.SetBit(0x02, value);
        }

        public bool yellow
        {
            get => (frets & 0x04) != 0;
            set => frets.SetBit(0x04, value);
        }

        public bool blue
        {
            get => (frets & 0x08) != 0;
            set => frets.SetBit(0x08, value);
        }

        public bool orange
        {
            get => (frets & 0x10) != 0;
            set => frets.SetBit(0x10, value);
        }

        public bool soloGreen
        {
            get => (soloFrets & 0x01) != 0;
            set => soloFrets.SetBit(0x01, value);
        }

        public bool soloRed
        {
            get => (soloFrets & 0x02) != 0;
            set => soloFrets.SetBit(0x02, value);
        }

        public bool soloYellow
        {
            get => (soloFrets & 0x04) != 0;
            set => soloFrets.SetBit(0x04, value);
        }

        public bool soloBlue
        {
            get => (soloFrets & 0x08) != 0;
            set => soloFrets.SetBit(0x08, value);
        }

        public bool soloOrange
        {
            get => (soloFrets & 0x10) != 0;
            set => soloFrets.SetBit(0x10, value);
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

        byte IFiveFretGuitarState.whammy
        {
            get => whammy;
            set => whammy = value;
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt >> 1);
            set
            {
                byte clamped = (byte)Math.Max(value, (sbyte)0);
                m_Tilt = (byte)((clamped << 1) | (clamped >> 7));
            }
        }

        int IRockBandGuitarState_Base.pickupSwitch
        {
            get => pickupSwitch;
            set => pickupSwitch = (byte)value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4RockBandGuitarState_ReportId : IRockBandGuitarState_Distinct
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4RockBandGuitarState_NoReportId state;

        public bool green { get => state.green; set => state.green = value; }
        public bool red { get => state.red; set => state.red = value; }
        public bool yellow { get => state.yellow; set => state.yellow = value; }
        public bool blue { get => state.blue; set => state.blue = value; }
        public bool orange { get => state.orange; set => state.orange = value; }

        public bool soloGreen { get => state.soloGreen; set => state.soloGreen = value; }
        public bool soloRed { get => state.soloRed; set => state.soloRed = value; }
        public bool soloYellow { get => state.soloYellow; set => state.soloYellow = value; }
        public bool soloBlue { get => state.soloBlue; set => state.soloBlue = value; }
        public bool soloOrange { get => state.soloOrange; set => state.soloOrange = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public byte whammy { get => state.whammy; set => state.whammy = value; }
        public sbyte tilt { get => state.tilt; set => state.tilt = value; }
        public int pickupSwitch { get => state.pickupSwitch; set => state.pickupSwitch = (byte)value; }
    }

    [InputControlLayout(stateType = typeof(PSRockBandGuitarLayout), displayName = "PlayStation 4 Rock Band Guitar", hideInUI = true)]
    internal class PS4RockBandGuitar_NoReportId : TranslatingRockBandGuitar_Distinct<PS4RockBandGuitarState_NoReportId> { }

    [InputControlLayout(stateType = typeof(PSRockBandGuitarLayout), displayName = "PlayStation 4 Rock Band Guitar")]
    internal class PS4RockBandGuitar : TranslatingRockBandGuitar_Distinct<PS4RockBandGuitarState_ReportId>
    {
        internal new static void Initialize()
        {
            // Stratocaster
            HidLayoutFinder.RegisterLayout<PS4RockBandGuitar, PS4RockBandGuitar_NoReportId>(0x0738, 0x8261, reportIdDefault: true);

            // Jaguar
            HidLayoutFinder.RegisterLayout<PS4RockBandGuitar, PS4RockBandGuitar_NoReportId>(0x0E6F, 0x0173, reportIdDefault: true);
        }
    }
}
