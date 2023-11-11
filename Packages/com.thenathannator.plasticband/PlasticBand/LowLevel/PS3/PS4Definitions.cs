using System;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/_Templates/PS4%20Base.md

namespace PlasticBand.Devices.LowLevel
{
    [Flags]
    internal enum PS4Button1 : ushort
    {
        None = 0,

        DpadMask = 0x0F,

        Square = 0x0010,
        Cross = 0x0020,
        Circle = 0x0040,
        Triangle = 0x0080,
        L2 = 0x0100,
        R2 = 0x0200,
        L1 = 0x0400,
        R1 = 0x0800,
        Select = 0x1000,
        Start = 0x2000,
        L3 = 0x4000,
        R3 = 0x8000,
    }

    [Flags]
    internal enum PS4Button2 : byte
    {
        PlayStation = 0x01,
    }

    internal static class PS4Extensions
    {
        public static HidDpad GetDpad(this PS4Button1 buttons)
        {
            return (HidDpad)(buttons & PS4Button1.DpadMask);
        }

        public static void SetDpad(ref this PS4Button1 buttons, HidDpad dpad)
        {
            buttons = (buttons & ~PS4Button1.DpadMask) | ((PS4Button1)dpad & PS4Button1.DpadMask);
        }

        public static void SetDpadUp(ref this PS4Button1 buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetUp(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetDpadRight(ref this PS4Button1 buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetRight(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetDpadDown(ref this PS4Button1 buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetDown(pressed);
            buttons.SetDpad(dpad);
        }

        public static void SetDpadLeft(ref this PS4Button1 buttons, bool pressed)
        {
            var dpad = buttons.GetDpad();
            dpad.SetLeft(pressed);
            buttons.SetDpad(dpad);
        }

        public static PS4Button1 AsPS4Buttons(this HidDpad dpad)
        {
            return (PS4Button1)dpad & PS4Button1.DpadMask;
        }

        public static void SetBit(ref this PS4Button1 value, PS4Button1 mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }

        public static void SetBit(ref this PS4Button2 value, PS4Button2 mask, bool set)
        {
            if (set)
                value |= mask;
            else
                value &= ~mask;
        }
    }
}