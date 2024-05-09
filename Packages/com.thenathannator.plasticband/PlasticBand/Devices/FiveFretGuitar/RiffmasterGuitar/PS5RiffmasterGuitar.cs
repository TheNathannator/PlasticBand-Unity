using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS5%20Riffmaster.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS5RiffmasterGuitarState_NoReportId : IRiffmasterGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        private byte m_JoystickX;
        [FieldOffset(1)]
        private byte m_JoystickY;

        [FieldOffset(7)]
        public PS4Button1 buttons1;
        [FieldOffset(9)]
        public PS4Button2 buttons2;

        [FieldOffset(39)]
        public byte pickupSwitch; // Assumed based on PS4, needed for our interface
        [FieldOffset(40)]
        public byte whammy;
        [FieldOffset(41)]
        private byte m_Tilt;

        [InputControl(name = "greenFret", layout = "Button", bit = 0)]
        [InputControl(name = "redFret", layout = "Button", bit = 1)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [FieldOffset(42)]
        public byte frets;

        [InputControl(name = "soloGreen", layout = "Button", bit = 0)]
        [InputControl(name = "soloRed", layout = "Button", bit = 1)]
        [InputControl(name = "soloYellow", layout = "Button", bit = 2)]
        [InputControl(name = "soloBlue", layout = "Button", bit = 3)]
        [InputControl(name = "soloOrange", layout = "Button", bit = 4)]
        [FieldOffset(43)]
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

        public bool p1
        {
            get => (buttons1 & PS4Button1.R3) != 0;
            set => buttons1.SetBit(PS4Button1.R3, value);
        }

        public bool joystickClick
        {
            // Click input needs to be ignored if the solo frets are active,
            // as it overlaps with the solo fret flag
            get => (buttons1 & PS4Button1.L3) != 0 && soloFrets == 0;
            set => buttons1.SetBit(PS4Button1.L3, value);
        }

        public sbyte joystickX
        {
            get => (sbyte)(m_JoystickX ^ 0x80);
            set => m_JoystickX = (byte)(value ^ 0x80);
        }

        public sbyte joystickY
        {
            get => (sbyte)(m_JoystickY ^ 0x80);
            set => m_JoystickY = (byte)(value ^ 0x80);
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
    internal unsafe struct PS5RiffmasterGuitarState_ReportId : IRiffmasterGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS5RiffmasterGuitarState_NoReportId state;

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

        public bool p1 { get => state.p1; set => state.p1 = value; }
        public bool joystickClick { get => state.joystickClick; set => state.joystickClick = value; }

        public sbyte joystickX { get => state.joystickX; set => state.joystickX = value; }
        public sbyte joystickY { get => state.joystickY; set => state.joystickY = value; }

        public byte whammy { get => state.whammy; set => state.whammy = value; }
        public sbyte tilt { get => state.tilt; set => state.tilt = value; }
        public int pickupSwitch { get => state.pickupSwitch; set => state.pickupSwitch = (byte)value; }
    }

    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "PlayStation 5 Riffmaster Guitar", hideInUI = true)]
    internal class PS5RiffmasterGuitar_NoReportId : TranslatingRiffmasterGuitar<PS5RiffmasterGuitarState_NoReportId> { }

    [InputControlLayout(stateType = typeof(PSRiffmasterGuitarLayout), displayName = "PlayStation 5 Riffmaster Guitar")]
    internal class PS5RiffmasterGuitar : TranslatingRiffmasterGuitar<PS5RiffmasterGuitarState_ReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS5RiffmasterGuitar, PS5RiffmasterGuitar_NoReportId>(0x0E6F, 0x0249, reportIdDefault: true);
        }
    }
}
