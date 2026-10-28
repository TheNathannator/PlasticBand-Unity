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
