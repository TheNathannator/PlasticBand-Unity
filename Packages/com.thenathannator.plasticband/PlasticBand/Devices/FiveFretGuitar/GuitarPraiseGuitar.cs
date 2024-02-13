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

    internal static class GuitarPraiseExtensions
    {
        public static HidDpad GetDpad(this GuitarPraiseButton buttons)
        {
            return (HidDpad)(buttons & GuitarPraiseButton.DpadMask);
        }

        public static void SetDpad(ref this GuitarPraiseButton buttons, HidDpad dpad)
        {
            buttons = (buttons & ~GuitarPraiseButton.DpadMask) | ((GuitarPraiseButton)dpad & GuitarPraiseButton.DpadMask);
        }

        public static void SetDpadUp(ref this GuitarPraiseButton buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetUp(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetDpadRight(ref this GuitarPraiseButton buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetRight(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetDpadDown(ref this GuitarPraiseButton buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetDown(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetDpadLeft(ref this GuitarPraiseButton buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetLeft(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetBit(ref this GuitarPraiseButton value, GuitarPraiseButton mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct GuitarPraiseGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        private fixed byte unused1[3];

        [InputControl(name = "whammy", layout = "IntAxis", parameters = "minValue=0x00,maxValue=0xFF,zeroPoint=0x00,invert")]
        public byte whammy;

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
        [InputControl(name = "startButton", layout = "Button", bit = 10)]
        [InputControl(name = "selectButton", layout = "Button", bit = 11)]
        public ushort buttons;

        private byte unused2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct GuitarPraiseGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public GuitarPraiseGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(GuitarPraiseGuitarState_ReportId), displayName = "Guitar Praise Guitar")]
    internal class GuitarPraiseGuitar : FiveFretGuitar
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<GuitarPraiseGuitar, GuitarPraiseGuitar_NoReportId>(0x0314, 0x0830);
        }
    }

    [InputControlLayout(stateType = typeof(GuitarPraiseGuitarState_NoReportId), displayName = "Guitar Praise Guitar", hideInUI = true)]
    internal class GuitarPraiseGuitar_NoReportId : GuitarPraiseGuitar { }
}
