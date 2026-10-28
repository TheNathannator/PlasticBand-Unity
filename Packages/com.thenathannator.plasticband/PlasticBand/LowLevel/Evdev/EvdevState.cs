using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.LowLevel
{
    // Must match HIDrogen's EVDV contract: 768 key bits, then 64 signed ABS values.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct EvdevState : IInputStateTypeInfo
    {
        private static readonly int[] s_SonyInstrumentKeys =
        {
            0x134, 0x130, 0x131, 0x133, 0x136, 0x137, 0x138,
            0x139, 0x13A, 0x13B, 0x13D, 0x13E, 0x13C,
        };
        public FourCC format => new FourCC('E', 'V', 'D', 'V');
        public fixed byte keys[96];
        public fixed int axes[64];

        public bool GetKey(int code)
        {
            if (code < 0 || code >= 768)
                throw new ArgumentOutOfRangeException(nameof(code));
            fixed (byte* bits = keys)
                return (bits[code >> 3] & (1 << (code & 7))) != 0;
        }

        public void SetKey(int code, bool value)
        {
            if (code < 0 || code >= 768)
                throw new ArgumentOutOfRangeException(nameof(code));
            fixed (byte* bits = keys)
            {
                byte mask = (byte)(1 << (code & 7));
                if (value) bits[code >> 3] |= mask;
                else bits[code >> 3] &= (byte)~mask;
            }
        }

        public int GetAxis(int code)
        {
            if (code < 0 || code >= 64)
                throw new ArgumentOutOfRangeException(nameof(code));
            fixed (int* values = axes)
                return values[code];
        }

        public void SetAxis(int code, int value)
        {
            if (code < 0 || code >= 64)
                throw new ArgumentOutOfRangeException(nameof(code));
            fixed (int* values = axes)
                values[code] = value;
        }

        public XInputGamepad ToXInput(bool invertY = true)
        {
            XInputButton buttons = 0;
            if (GetKey(0x130)) buttons |= XInputButton.A;
            if (GetKey(0x131)) buttons |= XInputButton.B;
            if (GetKey(0x133)) buttons |= XInputButton.X;
            if (GetKey(0x134)) buttons |= XInputButton.Y;
            if (GetKey(0x136)) buttons |= XInputButton.LeftShoulder;
            if (GetKey(0x137)) buttons |= XInputButton.RightShoulder;
            if (GetKey(0x13A)) buttons |= XInputButton.Back;
            if (GetKey(0x13B)) buttons |= XInputButton.Start;
            if (GetKey(0x13C)) buttons |= XInputButton.Guide;
            if (GetKey(0x13D)) buttons |= XInputButton.LeftThumb;
            if (GetKey(0x13E)) buttons |= XInputButton.RightThumb;
            if (GetAxis(0x10) < 0 || GetKey(0x222)) buttons |= XInputButton.DpadLeft;
            if (GetAxis(0x10) > 0 || GetKey(0x223)) buttons |= XInputButton.DpadRight;
            if (GetAxis(0x11) < 0 || GetKey(0x220)) buttons |= XInputButton.DpadUp;
            if (GetAxis(0x11) > 0 || GetKey(0x221)) buttons |= XInputButton.DpadDown;
            return new XInputGamepad()
            {
                buttons = (ushort)buttons,
                leftTrigger = (byte)GetAxis(2),
                rightTrigger = (byte)GetAxis(5),
                leftStickX = (short)GetAxis(0),
                // xpad applies a bitwise inversion, not arithmetic negation.
                leftStickY = (short)(invertY ? ~GetAxis(1) : GetAxis(1)),
                rightStickX = (short)GetAxis(3),
                rightStickY = (short)(invertY ? ~GetAxis(4) : GetAxis(4)),
            };
        }

        public PS3Button GetSonyButtons(int buttonBase)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < 13; bit++)
            {
                if (GetKey(buttonBase == 0 ? s_SonyInstrumentKeys[bit] : buttonBase + bit))
                    buttons |= (ushort)(1 << bit);
            }
            return (PS3Button)buttons;
        }

        public HidDpad GetDpad()
        {
            var dpad = HidDpad.Neutral;
            dpad.SetUp(GetAxis(0x11) < 0);
            dpad.SetDown(GetAxis(0x11) > 0);
            dpad.SetLeft(GetAxis(0x10) < 0);
            dpad.SetRight(GetAxis(0x10) > 0);
            return dpad;
        }
    }
}
