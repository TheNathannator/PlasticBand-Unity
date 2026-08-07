using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct SwitchRiffmasterGuitarState_3f_NoReportId : IRiffmasterGuitarState
    {
        //non full report mode. this is the mode the controller connects to the pc on by default which has only the basic inputs
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        public byte buttons0;
        [FieldOffset(1)]
        public byte buttons1;
        [FieldOffset(2)]
        public byte buttons2;
        [FieldOffset(3)]
        private ushort Joystick0;
        [FieldOffset(5)]
        private ushort Joystick1;
        [FieldOffset(7)]
        public ushort Whammy0;
        public byte whammy
        {
            get => (byte)(255f - Mathf.Clamp(Whammy0 / 257f, 0f, 255f));
            set { }
        }

        public bool green
        {
            get => (buttons0 & 0x02) != 0;
            set => buttons0.SetBit(0x08, value);
        }

        public bool red
        {
            get => (buttons0 & 0x01) != 0;
            set => buttons0.SetBit(0x04, value);
        }

        public bool yellow
        {
            get => (buttons0 & 0x08) != 0;
            set => buttons0.SetBit(0x02, value);
        }

        public bool blue
        {
            get => (buttons0 & 0x04) != 0;
            set => buttons0.SetBit(0x01, value);
        }

        public bool orange
        {
            get => (buttons0 & 0x10) != 0;
            set => buttons2.SetBit(0x40, value);
        }

        public bool solo
        {
            get => (buttons1 & 0x04) != 0;
            set => buttons1.SetBit(0x08, value);
        }

        public bool soloGreen
        {
            get => solo && green;
            set
            {
                solo = value;
                green = value;
            }
        }

        public bool soloRed
        {
            get => solo && red;
            set
            {
                solo = value;
                red = value;
            }
        }

        public bool soloYellow
        {
            get => solo && yellow;
            set
            {
                solo = value;
                yellow = value;
            }
        }

        public bool soloBlue
        {
            get => solo && blue;
            set
            {
                solo = value;
                blue = value;
            }
        }

        public bool soloOrange
        {
            get => solo && orange;
            set
            {
                solo = value;
                orange = value;
            }
        }

        public bool dpadUp
        {
            get => (buttons2 == 0 || buttons2 == 1 || buttons2 == 7);
            set { }
        }

        public bool dpadRight
        {
            get => (buttons2 == 1 || buttons2 == 2 || buttons2 == 3);
            set { }
        }

        public bool dpadDown
        {
            get => (buttons2 == 3 || buttons2 == 4 || buttons2 == 5);
            set { }
        }

        public bool dpadLeft
        {
            get => (buttons2 == 5 || buttons2 == 6 || buttons2 == 7);
            set { }
        }

        public bool select
        {
            get => (buttons1 & 0x01) != 0;
            set => buttons1.SetBit(0x01, value);
        }

        public bool start
        {
            get => (buttons1 & 0x02) != 0;
            set => buttons1.SetBit(0x02, value);
        }

        public bool system
        {
            get => (buttons1 & 0x10) != 0;
            set => buttons1.SetBit(0x10, value);
        }

        public bool p1
        {
            get => (buttons1 & 0x20) != 0;
            set => buttons1.SetBit(0x20, value);
        }

        public bool joystickClick
        {
            // Click input needs to be ignored if the solo buttons0 are active,
            // as it overlaps with the solo fret flag
            get => solo && !soloGreen && !soloRed && !soloYellow && !soloBlue && !soloOrange;
            set => buttons1.SetBit(0x08, value);
        }

        public sbyte joystickX
        {
            get => (sbyte)Mathf.Clamp((Joystick0 - 32768f) / 258f, -128f, 127f);
            set { }
        }

        public sbyte joystickY
        {
            get => (sbyte)-Mathf.Clamp((Joystick1 - 32768f) / 258f, -128f, 127f);
            set { }
        }

        byte IFiveFretGuitarState.whammy
        {
            get => (byte)whammy;
            set { }
        }

        public sbyte tilt
        {
            //cant get tilt when in 3f Report also gyro must be enabled
            get => 0;
            set { }
        }

        public int pickupSwitch
        {
            get => 0;
            set { }
        }

        int IRockBandGuitarState_Base.pickupSwitch
        {
            get => pickupSwitch;
            set => pickupSwitch = (byte)value;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct SwitchRiffmasterGuitarState_30_NoReportId : IRiffmasterGuitarState
    {
        //Full Report modde. this mode must be set for gyro to be used https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/bluetooth_hid_subcommands_notes.md#subcommand-0x03-set-input-report-mode
        // this mode also get set and unset by steam
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(2)]
        public byte buttons0;
        [FieldOffset(3)]
        public byte buttons1;
        [FieldOffset(4)]
        public byte buttons2;
        [FieldOffset(5)]
        private byte Joystick0;
        [FieldOffset(6)]
        private byte Joystick1;
        [FieldOffset(7)]
        private byte Joystick2;
        [FieldOffset(8)]
        public byte Whammy0;
        [FieldOffset(9)]
        public byte Whammy1;
        public byte whammy
        {
            // the value is fliped so the whammy give a positive value when pressed
            get => (byte)(255f - Mathf.Clamp(((uint)(Whammy0 | ((Whammy1 & 0x0F) << 8)) / 16.058f), 0f, 255f));
            set { }
        }
        [FieldOffset(14)]
        private byte Tilt0;
        [FieldOffset(15)]
        private byte Tilt1;

        public bool green
        {
            get => (buttons0 & 0x08) != 0;
            set => buttons0.SetBit(0x08, value);
        }

        public bool red
        {
            get => (buttons0 & 0x04) != 0;
            set => buttons0.SetBit(0x04, value);
        }

        public bool yellow
        {
            get => (buttons0 & 0x02) != 0;
            set => buttons0.SetBit(0x02, value);
        }

        public bool blue
        {
            get => (buttons0 & 0x01) != 0;
            set => buttons0.SetBit(0x01, value);
        }

        public bool orange
        {
            get => (buttons2 & 0x40) != 0;
            set => buttons2.SetBit(0x40, value);
        }

        public bool solo
        {
            get => (buttons1 & 0x08) != 0;
            set => buttons1.SetBit(0x08, value);
        }

        public bool soloGreen
        {
            get => solo && green;
            set
            {
                solo = value;
                green = value;
            }
        }

        public bool soloRed
        {
            get => solo && red;
            set
            {
                solo = value;
                red = value;
            }
        }

        public bool soloYellow
        {
            get => solo && yellow;
            set
            {
                solo = value;
                yellow = value;
            }
        }

        public bool soloBlue
        {
            get => solo && blue;
            set
            {
                solo = value;
                blue = value;
            }
        }

        public bool soloOrange
        {
            get => solo && orange;
            set
            {
                solo = value;
                orange = value;
            }
        }

        public bool dpadUp
        {
            get => (buttons2 & 0x02) != 0;
            set => buttons2.SetBit(0x02, value);
        }

        public bool dpadRight
        {
            get => (buttons2 & 0x04) != 0;
            set => buttons2.SetBit(0x04, value);
        }

        public bool dpadDown
        {
            get => (buttons2 & 0x01) != 0;
            set => buttons2.SetBit(0x01, value);
        }

        public bool dpadLeft
        {
            get => (buttons2 & 0x08) != 0;
            set => buttons2.SetBit(0x08, value);
        }

        public bool select
        {
            get => (buttons1 & 0x01) != 0;
            set => buttons1.SetBit(0x01, value);
        }

        public bool start
        {
            get => (buttons1 & 0x02) != 0;
            set => buttons1.SetBit(0x02, value);
        }

        public bool system
        {
            get => (buttons1 & 0x10) != 0;
            set => buttons1.SetBit(0x10, value);
        }

        public bool p1
        {
            get => (buttons1 & 0x20) != 0;
            set => buttons1.SetBit(0x20, value);
        }

        public bool joystickClick
        {
            // Click input needs to be ignored if the solo buttons0 are active,
            // as it overlaps with the solo fret flag
            get => (buttons1 & 0x08) != 0 && !soloGreen && !soloRed && !soloYellow && !soloBlue && !soloOrange;
            set => buttons1.SetBit(0x08, value);
        }

        public sbyte joystickX
        {
            get => (sbyte)Mathf.Clamp((((uint)(Joystick0 | ((Joystick1 & 0x0F) << 8)) - 2048f) / 16.125f), -128, 127);
            set { }
        }

        public sbyte joystickY
        {
            get => (sbyte)Mathf.Clamp((((uint)(((Joystick1 & 0xF0) >> 4) | (Joystick2 << 4)) - 2048f) / 16.125f), -128, 127);
            set { }
        }

        byte IFiveFretGuitarState.whammy
        {
            get => whammy;
            set => whammy = value;
        }

        public sbyte tilt
        {
            // IMC must be enabled
            // number can be way bigger but seems to  max out at -8000 at full tilt up
            // the value is fliped to give a positve value when tilted
            get => (sbyte)(256f - Mathf.Clamp(((short)(Tilt0 | (Tilt1 << 8)) / 31.25f), -128f, 127f));
            set { }
        }

        public int pickupSwitch
        {
            get => 0;
            set { }
        }

        int IRockBandGuitarState_Base.pickupSwitch
        {
            get => pickupSwitch;
            set => pickupSwitch = (byte)value;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct SwitchRiffmasterGuitarState_ReportId : IRiffmasterGuitarState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        public byte reportId;
        [FieldOffset(1)]
        public SwitchRiffmasterGuitarState_3f_NoReportId state3f;
        [FieldOffset(1)]
        public SwitchRiffmasterGuitarState_30_NoReportId state30;

        public bool green
        {
            get { if (reportId == 0x30) return state30.green; else return state3f.green; }
            set { if (reportId == 0x30) state30.green = value; else state3f.green = value; }
        }
        public bool red
        {
            get { if (reportId == 0x30) return state30.red; else return state3f.red; }
            set { if (reportId == 0x30) state30.red = value; else state3f.red = value; }
        }

        public bool yellow
        {
            get { if (reportId == 0x30) return state30.yellow; else return state3f.yellow; }
            set { if (reportId == 0x30) state30.yellow = value; else state3f.yellow = value; }
        }

        public bool blue
        {
            get { if (reportId == 0x30) return state30.blue; else return state3f.blue; }
            set { if (reportId == 0x30) state30.blue = value; else state3f.blue = value; }
        }

        public bool orange
        {
            get { if (reportId == 0x30) return state30.orange; else return state3f.orange; }
            set { if (reportId == 0x30) state30.orange = value; else state3f.orange = value; }
        }

        public bool soloGreen
        {
            get { if (reportId == 0x30) return state30.soloGreen; else return state3f.soloGreen; }
            set { if (reportId == 0x30) state30.soloGreen = value; else state3f.soloGreen = value; }
        }

        public bool soloRed
        {
            get { if (reportId == 0x30) return state30.soloRed; else return state3f.soloRed; }
            set { if (reportId == 0x30) state30.soloRed = value; else state3f.soloRed = value; }
        }

        public bool soloYellow
        {
            get { if (reportId == 0x30) return state30.soloYellow; else return state3f.soloYellow; }
            set { if (reportId == 0x30) state30.soloYellow = value; else state3f.soloYellow = value; }
        }

        public bool soloBlue
        {
            get { if (reportId == 0x30) return state30.soloBlue; else return state3f.soloBlue; }
            set { if (reportId == 0x30) state30.soloBlue = value; else state3f.soloBlue = value; }
        }

        public bool soloOrange
        {
            get { if (reportId == 0x30) return state30.soloOrange; else return state3f.soloOrange; }
            set { if (reportId == 0x30) state30.soloOrange = value; else state3f.soloOrange = value; }
        }

        public bool dpadUp
        {
            get { if (reportId == 0x30) return state30.dpadUp; else return state3f.dpadUp; }
            set { if (reportId == 0x30) state30.dpadUp = value; else state3f.dpadUp = value; }
        }

        public bool dpadDown
        {
            get { if (reportId == 0x30) return state30.dpadDown; else return state3f.dpadDown; }
            set { if (reportId == 0x30) state30.dpadDown = value; else state3f.dpadDown = value; }
        }

        public bool dpadLeft
        {
            get { if (reportId == 0x30) return state30.dpadLeft; else return state3f.dpadLeft; }
            set { if (reportId == 0x30) state30.dpadLeft = value; else state3f.dpadLeft = value; }
        }

        public bool dpadRight
        {
            get { if (reportId == 0x30) return state30.dpadRight; else return state3f.dpadRight; }
            set { if (reportId == 0x30) state30.dpadRight = value; else state3f.dpadRight = value; }
        }

        public bool start
        {
            get { if (reportId == 0x30) return state30.start; else return state3f.start; }
            set { if (reportId == 0x30) state30.start = value; else state3f.start = value; }
        }

        public bool select
        {
            get { if (reportId == 0x30) return state30.select; else return state3f.select; }
            set { if (reportId == 0x30) state30.select = value; else state3f.select = value; }
        }

        public bool system
        {
            get { if (reportId == 0x30) return state30.system; else return state3f.system; }
            set { if (reportId == 0x30) state30.system = value; else state3f.system = value; }
        }

        public bool p1
        {
            get { if (reportId == 0x30) return state30.p1; else return state3f.p1; }
            set { if (reportId == 0x30) state30.p1 = value; else state3f.p1 = value; }
        }

        public bool joystickClick
        {
            get { if (reportId == 0x30) return state30.joystickClick; else return state3f.joystickClick; }
            set { if (reportId == 0x30) state30.joystickClick = value; else state3f.joystickClick = value; }
        }

        public sbyte joystickX
        {
            get { if (reportId == 0x30) return state30.joystickX; else return state3f.joystickX; }
            set { if (reportId == 0x30) state30.joystickX = value; else state3f.joystickX = value; }
        }

        public sbyte joystickY
        {
            get { if (reportId == 0x30) return state30.joystickY; else return state3f.joystickY; }
            set { if (reportId == 0x30) state30.joystickY = value; else state3f.joystickY = value; }
        }

        public byte whammy
        {
            get { if (reportId == 0x30) return state30.whammy; else return state3f.whammy; }
            set { if (reportId == 0x30) state30.whammy = value; else state3f.whammy = value; }
        }

        public sbyte tilt
        {
            get { if (reportId == 0x30) return state30.tilt; else return state3f.tilt; }
            set { if (reportId == 0x30) state30.tilt = value; else state3f.tilt = value; }
        }

        public int pickupSwitch
        {
            get { if (reportId == 0x30) return state30.pickupSwitch; else return state3f.pickupSwitch; }
            set { if (reportId == 0x30) state30.pickupSwitch = value; else state3f.pickupSwitch = value; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SwitchRiffmasterGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedRiffmasterGuitarState.Format;

        [InputControl(name = "startButton", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.Start, displayName = "Plus")]
        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.Select, displayName = "Minus")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.System, displayName = "Home")]
        [InputControl(name = "p1Button", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.P1, displayName = "Capture")]
        public TranslatedRiffmasterGuitarState state;
    }

    [InputControlLayout(stateType = typeof(SwitchRiffmasterGuitarLayout), displayName = "Nintendo Switch Riffmaster Guitar", hideInUI = true)]
    internal class SwitchRiffmasterGuitar_NoReportId : TranslatingRiffmasterGuitar<SwitchRiffmasterGuitarState_3f_NoReportId> { }

    [InputControlLayout(stateType = typeof(SwitchRiffmasterGuitarLayout), displayName = "Nintendo Switch Riffmaster Guitar")]
    internal class SwitchRiffmasterGuitar : TranslatingRiffmasterGuitar<SwitchRiffmasterGuitarState_ReportId>
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<SwitchRiffmasterGuitar, SwitchRiffmasterGuitar_NoReportId>(0x0, 0x0, reportIdDefault: true);
        }
    }
}