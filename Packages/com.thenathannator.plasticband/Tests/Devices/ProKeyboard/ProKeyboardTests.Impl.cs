using System;
using PlasticBand.Controls;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    internal class XInputProKeyboardTests
        : ProKeyboardTests<XInputProKeyboard, XInputProKeyboardState>
    {
        protected override XInputProKeyboardState CreateState()
            => new XInputProKeyboardState()
        {
            pedal = 0x7F,
        };

        protected override void SetDpad(ref XInputProKeyboardState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetFaceButtons(ref XInputProKeyboardState state, FaceButton buttons)
            => XInputDeviceHandling.SetFaceButtons(ref state.buttons, buttons);

        protected override void SetMenuButtons(ref XInputProKeyboardState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetKey(ref XInputProKeyboardState state, int key, bool pressed)
            => ProKeyboardHandling.SetKey(ref state.keys1, ref state.keys2, ref state.keys3, ref state.velocity1,
                key, pressed);

        protected override void SetKeys(ref XInputProKeyboardState state, int keyMask)
            => ProKeyboardHandling.SetKeys(ref state.keys1, ref state.keys2, ref state.keys3, ref state.velocity1,
                keyMask);

        protected override void SetDigitalPedal(ref XInputProKeyboardState state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.pedal, pressed);

        protected override void SetAnalogPedal(ref XInputProKeyboardState state, float value)
            => ProKeyboardHandling.SetAnalogPedal(ref state.pedal, value);

        protected override void SetOverdriveButton(ref XInputProKeyboardState state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.overdrive, pressed);
    }

    internal class PS3ProKeyboardTests_NoReportId
        : ProKeyboardTests_TouchStrip<PS3ProKeyboard, PS3ProKeyboardState_NoReportId>
    {
        protected override PS3ProKeyboardState_NoReportId CreateState()
            => new PS3ProKeyboardState_NoReportId()
        {
            dpad = 8,
            pedal = 0x7F,
        };

        protected override void SetDpad(ref PS3ProKeyboardState_NoReportId state, DpadDirection dpad)
        {
            state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetFaceButtons(ref PS3ProKeyboardState_NoReportId state, FaceButton buttons)
            => PS3DeviceHandling.SetFaceButtons(ref state.buttons, buttons);

        protected override void SetMenuButtons(ref PS3ProKeyboardState_NoReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetKey(ref PS3ProKeyboardState_NoReportId state, int key, bool pressed)
            => ProKeyboardHandling.SetKey(ref state.keys1, ref state.keys2, ref state.keys3, ref state.velocity1,
                key, pressed);

        protected override void SetKeys(ref PS3ProKeyboardState_NoReportId state, int keyMask)
            => ProKeyboardHandling.SetKeys(ref state.keys1, ref state.keys2, ref state.keys3, ref state.velocity1,
                keyMask);

        protected override void SetDigitalPedal(ref PS3ProKeyboardState_NoReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.pedal, pressed);

        protected override void SetAnalogPedal(ref PS3ProKeyboardState_NoReportId state, float value)
            => ProKeyboardHandling.SetAnalogPedal(ref state.pedal, value);

        protected override void SetOverdriveButton(ref PS3ProKeyboardState_NoReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.overdrive, pressed);

        protected override void SetTouchStrip(ref PS3ProKeyboardState_NoReportId state, float value)
            => ProKeyboardHandling.SetTouchStrip(ref state.touchStrip, value);
    }

    internal class PS3ProKeyboardTests_ReportId
        : ProKeyboardTests_TouchStrip<PS3ProKeyboard_ReportId, PS3ProKeyboardState_ReportId>
    {
        protected override PS3ProKeyboardState_ReportId CreateState()
            => new PS3ProKeyboardState_ReportId()
        {
            state = new PS3ProKeyboardState_NoReportId()
            {
                dpad = 8,
                pedal = 0x7F,
            }
        };

        protected override void SetDpad(ref PS3ProKeyboardState_ReportId state, DpadDirection dpad)
        {
            state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetMenuButtons(ref PS3ProKeyboardState_ReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.buttons, buttons);

        protected override void SetFaceButtons(ref PS3ProKeyboardState_ReportId state, FaceButton buttons)
            => PS3DeviceHandling.SetFaceButtons(ref state.state.buttons, buttons);

        protected override void SetKey(ref PS3ProKeyboardState_ReportId state, int key, bool pressed)
            => ProKeyboardHandling.SetKey(ref state.state.keys1, ref state.state.keys2, ref state.state.keys3,
                ref state.state.velocity1, key, pressed);

        protected override void SetKeys(ref PS3ProKeyboardState_ReportId state, int keyMask)
            => ProKeyboardHandling.SetKeys(ref state.state.keys1, ref state.state.keys2, ref state.state.keys3,
                ref state.state.velocity1, keyMask);

        protected override void SetDigitalPedal(ref PS3ProKeyboardState_ReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.state.pedal, pressed);

        protected override void SetAnalogPedal(ref PS3ProKeyboardState_ReportId state, float value)
            => ProKeyboardHandling.SetAnalogPedal(ref state.state.pedal, value);

        protected override void SetOverdriveButton(ref PS3ProKeyboardState_ReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.state.overdrive, pressed);

        protected override void SetTouchStrip(ref PS3ProKeyboardState_ReportId state, float value)
            => ProKeyboardHandling.SetTouchStrip(ref state.state.touchStrip, value);
    }

    internal class WiiProKeyboardTests_NoReportId
        : ProKeyboardTests_TouchStrip<WiiProKeyboard, WiiProKeyboardState_NoReportId>
    {
        protected override WiiProKeyboardState_NoReportId CreateState()
            => new WiiProKeyboardState_NoReportId()
        {
            state = new PS3ProKeyboardState_NoReportId()
            {
                dpad = 8,
                pedal = 0x7F,
            }
        };

        protected override void SetDpad(ref WiiProKeyboardState_NoReportId state, DpadDirection dpad)
        {
            state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetFaceButtons(ref WiiProKeyboardState_NoReportId state, FaceButton buttons)
            => PS3DeviceHandling.SetFaceButtons(ref state.state.buttons, buttons);

        protected override void SetMenuButtons(ref WiiProKeyboardState_NoReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.buttons, buttons);

        protected override void SetKey(ref WiiProKeyboardState_NoReportId state, int key, bool pressed)
            => ProKeyboardHandling.SetKey(ref state.state.keys1, ref state.state.keys2, ref state.state.keys3,
                ref state.state.velocity1, key, pressed);

        protected override void SetKeys(ref WiiProKeyboardState_NoReportId state, int keyMask)
            => ProKeyboardHandling.SetKeys(ref state.state.keys1, ref state.state.keys2, ref state.state.keys3,
                ref state.state.velocity1, keyMask);

        protected override void SetDigitalPedal(ref WiiProKeyboardState_NoReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.state.pedal, pressed);

        protected override void SetAnalogPedal(ref WiiProKeyboardState_NoReportId state, float value)
            => ProKeyboardHandling.SetAnalogPedal(ref state.state.pedal, value);

        protected override void SetOverdriveButton(ref WiiProKeyboardState_NoReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.state.overdrive, pressed);

        protected override void SetTouchStrip(ref WiiProKeyboardState_NoReportId state, float value)
            => ProKeyboardHandling.SetTouchStrip(ref state.state.touchStrip, value);
    }

    internal class WiiProKeyboardTests_ReportId
        : ProKeyboardTests_TouchStrip<WiiProKeyboard_ReportId, WiiProKeyboardState_ReportId>
    {
        protected override WiiProKeyboardState_ReportId CreateState()
            => new WiiProKeyboardState_ReportId()
        {
            // lol, triple-layered state
            state = new WiiProKeyboardState_NoReportId()
            {
                state = new PS3ProKeyboardState_NoReportId()
                {
                    dpad = 8,
                    pedal = 0x7F,
                }
            }
        };

        protected override void SetDpad(ref WiiProKeyboardState_ReportId state, DpadDirection dpad)
        {
            state.state.state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetFaceButtons(ref WiiProKeyboardState_ReportId state, FaceButton buttons)
            => PS3DeviceHandling.SetFaceButtons(ref state.state.state.buttons, buttons);

        protected override void SetMenuButtons(ref WiiProKeyboardState_ReportId state, MenuButton buttons)
            => PS3DeviceHandling.SetMenuButtons(ref state.state.state.buttons, buttons);

        protected override void SetKey(ref WiiProKeyboardState_ReportId state, int key, bool pressed)
            => ProKeyboardHandling.SetKey(ref state.state.state.keys1, ref state.state.state.keys2,
                ref state.state.state.keys3, ref state.state.state.velocity1, key, pressed);

        protected override void SetKeys(ref WiiProKeyboardState_ReportId state, int keyMask)
            => ProKeyboardHandling.SetKeys(ref state.state.state.keys1, ref state.state.state.keys2,
                ref state.state.state.keys3, ref state.state.state.velocity1, keyMask);

        protected override void SetDigitalPedal(ref WiiProKeyboardState_ReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.state.state.pedal, pressed);

        protected override void SetAnalogPedal(ref WiiProKeyboardState_ReportId state, float value)
            => ProKeyboardHandling.SetAnalogPedal(ref state.state.state.pedal, value);

        protected override void SetOverdriveButton(ref WiiProKeyboardState_ReportId state, bool pressed)
            => ProKeyboardHandling.SetDigitalPedal(ref state.state.state.overdrive, pressed);

        protected override void SetTouchStrip(ref WiiProKeyboardState_ReportId state, float value)
            => ProKeyboardHandling.SetTouchStrip(ref state.state.state.touchStrip, value);
    }

    public static class ProKeyboardHandling
    {
        public static void SetKey(ref byte keys1, ref byte keys2, ref byte keys3, ref byte keys4,
            int key, bool pressed)
        {
            if (key >= ProKeyboard.KeyCount)
                throw new ArgumentOutOfRangeException(nameof(key), key, $"Expected an index less than {nameof(ProKeyboard.KeyCount)} ({ProKeyboard.KeyCount})!");

            ref byte keys = ref keys1;
            if (key < 8)
                keys = ref keys1;
            else if (key < 16)
                keys = ref keys2;
            else if (key < 24)
                keys = ref keys3;
            else
                keys = ref keys4;

            // The key bits are ordered from highest to lowest, e.g. (keys1 & 0x80) will give you key 1
            // (the left-most key), (keys1 & 0x01) will give you key 8, etc.
            int bit = key % 8;
            byte mask = (byte)(1 << (7 - bit));
            keys.SetBit(mask, pressed);
        }

        public static void SetKeys(ref byte keys1, ref byte keys2, ref byte keys3, ref byte keys4, int keyMask)
        {
            int maxMask = DeviceHandling.CreateMask(0, ProKeyboard.KeyCount);
            if (keyMask >= maxMask)
                throw new ArgumentOutOfRangeException(nameof(keyMask), keyMask, $"Expected a mask less than {maxMask:X4}!");

            // The key bits are ordered from highest to lowest, e.g. (keys1 & 0x80) will give you key 1
            // (the left-most key), (keys1 & 0x01) will give you key 8, etc.
            for (int key = 0; key < ProKeyboard.KeyCount; key++)
            {
                ref byte keysByte = ref keys1;
                if (key < 8)
                    keysByte = ref keys1;
                else if (key < 16)
                    keysByte = ref keys2;
                else if (key < 24)
                    keysByte = ref keys3;
                else
                    keysByte = ref keys4;

                int keyBit = 1 << key;
                int bit = key % 8;
                byte rawBit = (byte)(1 << (7 - bit));
                keysByte.SetBit(rawBit, (keyMask & keyBit) != 0);
            }
        }

        public static void SetDigitalPedal(ref byte pedal, bool pressed)
        {
            pedal.SetBit(0x80, pressed);
        }

        public static void SetAnalogPedal(ref byte pedal, float value)
        {
            byte rawValue = (byte)IntegerAxisControl.Denormalize(value, 0x7F, 0, 0x7F);
            pedal.SetMask(rawValue, 0x7F, 0);
        }

        public static void SetOverdriveButton(ref byte overdrive, bool pressed)
        {
            overdrive.SetBit(0x80, pressed);
        }

        public static void SetTouchStrip(ref byte touchStrip, float value)
        {
            byte rawValue = (byte)IntegerAxisControl.Denormalize(value, 0, 0x7F, 0);
            touchStrip.SetMask(rawValue, 0x7F, 0);
        }
    }
}