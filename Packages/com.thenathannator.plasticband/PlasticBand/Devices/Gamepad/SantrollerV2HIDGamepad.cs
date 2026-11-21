using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Report for Santroller V2 devices emulating a gamepad in HID mode.
    /// Mirrors the XInput report, except the low nibble of the buttons is a hat.
    /// </summary>
    /// <remarks>
    /// Stick Y axes follow HID convention (down at the maximum, the bitwise inverse of XInput's), unless the
    /// profile's "invert Y axis" HID option is set, in which case they follow XInput's convention and the
    /// device reports <see cref="SantrollerCapabilities.YAxisInverted"/>; <see cref="SantrollerV2HIDGamepad"/>
    /// registers a separate layout for that.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SantrollerV2HIDGamepadState : IGamepadState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public byte reserved;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5,maxValue=7")]

        [InputControl(name = "start", layout = "Button", bit = 4)]
        [InputControl(name = "select", layout = "Button", bit = 5)]
        [InputControl(name = "leftStickPress", layout = "Button", bit = 6)]
        [InputControl(name = "rightStickPress", layout = "Button", bit = 7)]

        [InputControl(name = "leftShoulder", layout = "Button", bit = 8)]
        [InputControl(name = "rightShoulder", layout = "Button", bit = 9)]
        [InputControl(name = "systemButton", layout = "Button", bit = 10)]
        [InputControl(name = "shareButton", layout = "Button", bit = 11)]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12)]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13)]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14)]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15)]
        public SantrollerV2Button buttons;

        [InputControl(name = "leftTrigger", layout = "IntButton", format = "BYTE", parameters = "minValue=0x00,maxValue=0xFF")]
        public byte leftTrigger;

        [InputControl(name = "rightTrigger", layout = "IntButton", format = "BYTE", parameters = "minValue=0x00,maxValue=0xFF")]
        public byte rightTrigger;

        [InputControl(name = "leftStick", layout = "Stick", format = "VC2S")]
        [InputControl(name = "leftStick/x", offset = 0, layout = "IntAxis", format = "SHRT", parameters = "minValue=-32768,maxValue=32767,zeroPoint=0")]
        [InputControl(name = "leftStick/left", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        [InputControl(name = "leftStick/right", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "leftStick/y", offset = 2, layout = "IntAxis", format = "SHRT", parameters = "minValue=32767,maxValue=-32768,zeroPoint=0")]
        [InputControl(name = "leftStick/up", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        [InputControl(name = "leftStick/down", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        public short leftStickX;
        public short leftStickY;

        [InputControl(name = "rightStick", layout = "Stick", format = "VC2S")]
        [InputControl(name = "rightStick/x", offset = 0, layout = "IntAxis", format = "SHRT", parameters = "minValue=-32768,maxValue=32767,zeroPoint=0")]
        [InputControl(name = "rightStick/left", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        [InputControl(name = "rightStick/right", offset = 0, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        // These must be placed up here, otherwise a stack overflow will occur when building the layout
        [InputControl(name = "rightStick/y", offset = 2, layout = "IntAxis", format = "SHRT", parameters = "minValue=32767,maxValue=-32768,zeroPoint=0")]
        [InputControl(name = "rightStick/up", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=-32768")]
        [InputControl(name = "rightStick/down", offset = 2, layout = "IntButton", format = "SHRT", parameters = "minValue=0,maxValue=32767")]
        public short rightStickX;
        public short rightStickY;

        public bool start
        {
            get => (buttons & SantrollerV2Button.Start) != 0;
            set => buttons.SetBit(SantrollerV2Button.Start, value);
        }

        public bool select
        {
            get => (buttons & SantrollerV2Button.Select) != 0;
            set => buttons.SetBit(SantrollerV2Button.Select, value);
        }

        public bool system
        {
            get => (buttons & SantrollerV2Button.System) != 0;
            set => buttons.SetBit(SantrollerV2Button.System, value);
        }

        public bool share
        {
            get => (buttons & SantrollerV2Button.Share) != 0;
            set => buttons.SetBit(SantrollerV2Button.Share, value);
        }

        public bool south
        {
            get => (buttons & SantrollerV2Button.South) != 0;
            set => buttons.SetBit(SantrollerV2Button.South, value);
        }

        public bool east
        {
            get => (buttons & SantrollerV2Button.East) != 0;
            set => buttons.SetBit(SantrollerV2Button.East, value);
        }

        public bool west
        {
            get => (buttons & SantrollerV2Button.West) != 0;
            set => buttons.SetBit(SantrollerV2Button.West, value);
        }

        public bool north
        {
            get => (buttons & SantrollerV2Button.North) != 0;
            set => buttons.SetBit(SantrollerV2Button.North, value);
        }

        public bool dpadUp
        {
            get => buttons.GetDpad().IsUp();
            set => buttons.SetDpadUp(value);
        }

        public bool dpadDown
        {
            get => buttons.GetDpad().IsDown();
            set => buttons.SetDpadDown(value);
        }

        public bool dpadLeft
        {
            get => buttons.GetDpad().IsLeft();
            set => buttons.SetDpadLeft(value);
        }

        public bool dpadRight
        {
            get => buttons.GetDpad().IsRight();
            set => buttons.SetDpadRight(value);
        }

        public bool leftShoulder
        {
            get => (buttons & SantrollerV2Button.LeftShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftShoulder, value);
        }

        public bool rightShoulder
        {
            get => (buttons & SantrollerV2Button.RightShoulder) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightShoulder, value);
        }

        public bool leftTriggerPress
        {
            get => false;
            set { }
        }

        public bool rightTriggerPress
        {
            get => false;
            set { }
        }

        public bool leftStickPress
        {
            get => (buttons & SantrollerV2Button.LeftStickPress) != 0;
            set => buttons.SetBit(SantrollerV2Button.LeftStickPress, value);
        }

        public bool rightStickPress
        {
            get => (buttons & SantrollerV2Button.RightStickPress) != 0;
            set => buttons.SetBit(SantrollerV2Button.RightStickPress, value);
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

        sbyte IGamepadState.leftStickX
        {
            get => XInputDefinitions.GetAxisX(leftStickX);
            set => leftStickX = XInputDefinitions.SetAxisX(value);
        }

        sbyte IGamepadState.leftStickY
        {
            get => XInputDefinitions.GetAxisY((short)~leftStickY);
            set => leftStickY = (short)~XInputDefinitions.SetAxisY(value);
        }

        sbyte IGamepadState.rightStickX
        {
            get => XInputDefinitions.GetAxisX(rightStickX);
            set => rightStickX = XInputDefinitions.SetAxisX(value);
        }

        sbyte IGamepadState.rightStickY
        {
            get => XInputDefinitions.GetAxisY((short)~rightStickY);
            set => rightStickY = (short)~XInputDefinitions.SetAxisY(value);
        }
    }

    /// <summary>
    /// A Santroller V2 device emulating a gamepad in HID mode.
    /// </summary>
    [InputControlLayout(stateType = typeof(SantrollerV2HIDGamepadState), displayName = "Santroller V2 HID Gamepad", hideInUI = true)]
    internal class SantrollerV2HIDGamepad : Gamepad
    {
        /// <summary>
        /// Layout for devices reporting <see cref="SantrollerCapabilities.YAxisInverted"/>,
        /// whose stick Y axes follow XInput's convention (up at the maximum).
        /// </summary>
        internal const string kXInputYAxisLayout = "SantrollerV2HIDGamepadXInputYAxis";

        internal static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerV2HIDGamepad>();
            InputSystem.RegisterLayoutBuilder(() =>
            {
                var builder = new InputControlLayout.Builder()
                    .WithName(kXInputYAxisLayout)
                    .WithDisplayName("Santroller V2 HID Gamepad")
                    .Extend(nameof(SantrollerV2HIDGamepad));
                foreach (string stick in new[] { "leftStick", "rightStick" })
                {
                    builder.AddControl($"{stick}/y").WithParameters("minValue=-32768,maxValue=32767,zeroPoint=0");
                    builder.AddControl($"{stick}/up").WithParameters("minValue=0,maxValue=32767");
                    builder.AddControl($"{stick}/down").WithParameters("minValue=0,maxValue=-32768");
                }
                return builder.Build();
            }, kXInputYAxisLayout, nameof(SantrollerV2HIDGamepad));
        }
    }
}
