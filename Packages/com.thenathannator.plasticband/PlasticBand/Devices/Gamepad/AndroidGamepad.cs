using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    public enum AndroidGamepadButtons1 : byte
    {
        South = 1 << 0,
        East = 1 << 1,
        // ??? = 1 << 2,
        West = 1 << 3,
        North = 1 << 4,
        // ??? = 1 << 5,
        LeftShoulder = 1 << 6,
        RightShoulder = 1 << 7,
    }

    public enum AndroidGamepadButtons2 : byte
    {
        LeftTriggerPress = 1 << 0,
        RightTriggerPress = 1 << 1,
        Select = 1 << 2,
        Start = 1 << 3,
        System = 1 << 4,
        LeftStickPress = 1 << 5,
        RightStickPress = 1 << 6,
        // ??? = 1 << 7,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct AndroidGamepadState_NoReportId : IGamepadState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonSouth", layout = "Button", bit = 0)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 1)]
        [InputControl(name = "buttonWest", layout = "Button", bit = 3)]

        [InputControl(name = "buttonNorth", layout = "Button", bit = 4)]
        [InputControl(name = "leftShoulder", layout = "Button", bit = 6)]
        [InputControl(name = "rightShoulder", layout = "Button", bit = 7)]
        public AndroidGamepadButtons1 buttons1;

        [InputControl(name = "leftTriggerPress", layout = "Button", bit = 0)]
        [InputControl(name = "rightTriggerPress", layout = "Button", bit = 1)]
        [InputControl(name = "select", layout = "Button", bit = 2)]
        [InputControl(name = "start", layout = "Button", bit = 3)]

        [InputControl(name = "system", layout = "Button", bit = 4)]
        [InputControl(name = "leftStickPress", layout = "Button", bit = 5)]
        [InputControl(name = "rightStickPress", layout = "Button", bit = 6)]
        public AndroidGamepadButtons2 buttons2;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = (byte)HidDpad.Neutral)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public HidDpad dpad;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", offset = 0, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        [InputControl(name = "leftStick/left", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "leftStick/right", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", offset = 1, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0x80")]
        [InputControl(name = "leftStick/up", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "leftStick/down", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        public byte m_LeftStickX;
        public byte m_LeftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", offset = 0, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        [InputControl(name = "rightStick/left", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "rightStick/right", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        // These must be included up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", offset = 1, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0x80")]
        [InputControl(name = "rightStick/up", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "rightStick/down", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        public byte m_RightStickX;
        public byte m_RightStickY;

        [InputControl(name = "leftTrigger", layout = "IntButton", parameters = "minValue=0x00,maxValue=0xFF,intPressPoint=0x80")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "IntButton", parameters = "minValue=0x00,maxValue=0xFF,intPressPoint=0x80")]
        public byte rightTrigger;

        public bool start
        {
            get => (buttons2 & AndroidGamepadButtons2.Start) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.Start, value);
        }

        public bool select
        {
            get => (buttons2 & AndroidGamepadButtons2.Select) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.Select, value);
        }

        public bool system
        {
            get => (buttons2 & AndroidGamepadButtons2.System) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.System, value);
        }

        public bool share
        {
            get => false;
            set { }
        }

        public bool south
        {
            get => (buttons1 & AndroidGamepadButtons1.South) != 0;
            set => buttons1.SetBit(AndroidGamepadButtons1.South, value);
        }

        public bool east
        {
            get => (buttons1 & AndroidGamepadButtons1.East) != 0;
            set => buttons1.SetBit(AndroidGamepadButtons1.East, value);
        }

        public bool west
        {
            get => (buttons1 & AndroidGamepadButtons1.West) != 0;
            set => buttons1.SetBit(AndroidGamepadButtons1.West, value);
        }

        public bool north
        {
            get => (buttons1 & AndroidGamepadButtons1.North) != 0;
            set => buttons1.SetBit(AndroidGamepadButtons1.North, value);
        }

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => dpad.SetUp(value);
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

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => dpad.SetRight(value);
        }

        public bool leftShoulder
        {
            get => (buttons1 & AndroidGamepadButtons1.LeftShoulder) != 0;
            set => buttons1.SetBit(AndroidGamepadButtons1.LeftShoulder, value);
        }

        public bool rightShoulder
        {
            get => (buttons1 & AndroidGamepadButtons1.RightShoulder) != 0;
            set => buttons1.SetBit(AndroidGamepadButtons1.RightShoulder, value);
        }

        public bool leftTriggerPress
        {
            get => (buttons2 & AndroidGamepadButtons2.LeftTriggerPress) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.LeftTriggerPress, value);
        }

        public bool rightTriggerPress
        {
            get => (buttons2 & AndroidGamepadButtons2.RightTriggerPress) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.RightTriggerPress, value);
        }

        public bool leftStickPress
        {
            get => (buttons2 & AndroidGamepadButtons2.LeftStickPress) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.LeftStickPress, value);
        }

        public bool rightStickPress
        {
            get => (buttons2 & AndroidGamepadButtons2.RightStickPress) != 0;
            set => buttons2.SetBit(AndroidGamepadButtons2.RightStickPress, value);
        }

        byte IGamepadState.leftTrigger
        {
            get => leftTrigger;
            set => leftTrigger = value;
        }

        byte IGamepadState.rightTrigger
        {
            get => rightTrigger;
            set => rightTrigger = value;
        }

        public sbyte leftStickX
        {
            get => HidDefinitions.GetByteX(m_LeftStickX);
            set => m_LeftStickX = HidDefinitions.SetByteX(value);
        }

        public sbyte leftStickY
        {
            get => HidDefinitions.GetInvertedByteY(m_LeftStickY);
            set => m_LeftStickY = HidDefinitions.SetInvertedByteY(value);
        }

        public sbyte rightStickX
        {
            get => HidDefinitions.GetByteX(m_RightStickX);
            set => m_RightStickX = HidDefinitions.SetByteX(value);
        }

        public sbyte rightStickY
        {
            get => HidDefinitions.GetInvertedByteY(m_RightStickY);
            set => m_RightStickY = HidDefinitions.SetInvertedByteY(value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct AndroidGamepadState_ReportId : IGamepadState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        // BUGFIX: The input system supports composing state layout structs and will automatically adjust the
        // `offset` parameter of controls introduced from these fields to ensure they still line up. However,
        // it does this *unconditionally* to all controls in the hierarchy when it should only offset controls
        // at the root of the device, which means that any child controls of other controls which have a value
        // specified in `offset` will be incorrectly shifted by the offset of the state struct field.
        //
        // In this case, all child controls of leftStick and rightStick end up one byte ahead of where they
        // should be, and we need to correct it after the fact. Unfortunately, re-declaring the controls here
        // will *completely overwrite* several properties specified by the struct field, so we must copy them
        // and all of their properties wholesale, with added offset to position them correctly.

        [InputControl(name = "leftStick", offset = 4, layout = "Stick", format = "VC2B")]
        [InputControl(name = "leftStick/x", offset = 0, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        [InputControl(name = "leftStick/left", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "leftStick/right", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        [InputControl(name = "leftStick/y", offset = 1, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0x80")]
        [InputControl(name = "leftStick/up", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "leftStick/down", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]

        [InputControl(name = "rightStick", offset = 6, layout = "Stick", format = "VC2B")]
        [InputControl(name = "rightStick/x", offset = 0, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x80")]
        [InputControl(name = "rightStick/left", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "rightStick/right", offset = 0, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        [InputControl(name = "rightStick/y", offset = 1, layout = "IntAxis", format = "BYTE", defaultState = 0x80, parameters = "minValue=0xFF,maxValue=0x00,zeroPoint=0x80")]
        [InputControl(name = "rightStick/up", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0x00")]
        [InputControl(name = "rightStick/down", offset = 1, layout = "IntButton", format = "BYTE", parameters = "minValue=0x80,maxValue=0xFF")]
        public AndroidGamepadState_NoReportId state;

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }
        public bool share { get => state.share; set => state.share = value; }

        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool west { get => state.west; set => state.west = value; }
        public bool north { get => state.north; set => state.north = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool leftShoulder { get => state.leftShoulder; set => state.leftShoulder = value; }
        public bool rightShoulder { get => state.rightShoulder; set => state.rightShoulder = value; }
        public bool leftTriggerPress { get => state.leftTriggerPress; set => state.leftTriggerPress = value; }
        public bool rightTriggerPress { get => state.rightTriggerPress; set => state.rightTriggerPress = value; }
        public bool leftStickPress { get => state.leftStickPress; set => state.leftStickPress = value; }
        public bool rightStickPress { get => state.rightStickPress; set => state.rightStickPress = value; }

        public byte leftTrigger { get => state.leftTrigger; set => state.leftTrigger = value; }
        public byte rightTrigger { get => state.rightTrigger; set => state.rightTrigger = value; }

        public sbyte leftStickX { get => state.leftStickX; set => state.leftStickX = value; }
        public sbyte leftStickY { get => state.leftStickY; set => state.leftStickY = value; }

        public sbyte rightStickX { get => state.rightStickX; set => state.rightStickX = value; }
        public sbyte rightStickY { get => state.rightStickY; set => state.rightStickY = value; }
    }

    [InputControlLayout(stateType = typeof(AndroidGamepadState_ReportId), displayName = "Android Gamepad")]
    internal class AndroidGamepad : Gamepad
    {
        internal static void Initialize()
        {
            // CRKD multi-platform guitars in Android mode
            // This is not matched as a guitar layout, as the hardware IDs do not be unique to CRKD
            // ([PCILookup](https://pcilookup.com/?ven=20bc&dev=5500&action=submit) lists the vendor ID
            // as `ShenZhen ShanWan Technology Co., Ltd.` and the product ID as `Frostbite controller`),
            // and the inputs are specifically that of a gamepad.
            HidLayoutFinder.RegisterLayout<AndroidGamepad, AndroidGamepad_NoReportId>(0x20BC, 0x5500, reportIdDefault: true);
        }
    }

    [InputControlLayout(stateType = typeof(AndroidGamepadState_NoReportId), displayName = "Android Gamepad", hideInUI = true)]
    internal class AndroidGamepad_NoReportId : AndroidGamepad { }

    internal static class AndroidGamepadExtensions
    {
        internal static void SetBit(ref this AndroidGamepadButtons1 value, AndroidGamepadButtons1 mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        internal static void SetBit(ref this AndroidGamepadButtons2 value, AndroidGamepadButtons2 mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }
}
