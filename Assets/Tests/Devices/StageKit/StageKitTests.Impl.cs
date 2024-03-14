using PlasticBand.Devices;
using PlasticBand.LowLevel;

namespace PlasticBand.Tests.Devices
{
    using SantrollerHIDButton = SantrollerHidStageKitState.Button;

    internal class XInputStageKitTests
        : StageKitTests<XInputStageKit, XInputStageKitState>
    {
        protected override XInputStageKitState CreateState()
            => new XInputStageKitState();

        protected override void SetDpad(ref XInputStageKitState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetFaceButtons(ref XInputStageKitState state, FaceButton buttons)
            => XInputDeviceHandling.SetFaceButtons(ref state.buttons, buttons);

        protected override void SetMenuButtons(ref XInputStageKitState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetShoulderButtons(ref XInputStageKitState state, SideButton buttons)
            => XInputStageKitHandling.SetShoulderButtons(ref state.buttons, buttons);

        protected override void SetStickButtons(ref XInputStageKitState state, SideButton buttons)
            => XInputStageKitHandling.SetStickButtons(ref state.buttons, buttons);

        protected override void SetLeftTrigger(ref XInputStageKitState state, float value)
        {
            state.leftTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetRightTrigger(ref XInputStageKitState state, float value)
        {
            state.rightTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetLeftStick(ref XInputStageKitState state, float x, float y)
        {
            state.leftStickX = DeviceHandling.DenormalizeInt16(x);
            state.leftStickY = DeviceHandling.DenormalizeInt16(y);
        }

        protected override void SetRightStick(ref XInputStageKitState state, float x, float y)
        {
            state.rightStickX = DeviceHandling.DenormalizeInt16(x);
            state.rightStickY = DeviceHandling.DenormalizeInt16(y);
        }
    }

    internal class SantrollerXInputStageKitTests
        : StageKitTests<SantrollerXInputStageKit, XInputStageKitState>
    {
        protected override XInputStageKitState CreateState()
            => new XInputStageKitState();

        protected override void SetDpad(ref XInputStageKitState state, DpadDirection dpad)
            => XInputDeviceHandling.SetDpad(ref state.buttons, dpad);

        protected override void SetFaceButtons(ref XInputStageKitState state, FaceButton buttons)
            => XInputDeviceHandling.SetFaceButtons(ref state.buttons, buttons);

        protected override void SetMenuButtons(ref XInputStageKitState state, MenuButton buttons)
            => XInputDeviceHandling.SetMenuButtons(ref state.buttons, buttons);

        protected override void SetShoulderButtons(ref XInputStageKitState state, SideButton buttons)
            => XInputStageKitHandling.SetShoulderButtons(ref state.buttons, buttons);

        protected override void SetStickButtons(ref XInputStageKitState state, SideButton buttons)
            => XInputStageKitHandling.SetStickButtons(ref state.buttons, buttons);

        protected override void SetLeftTrigger(ref XInputStageKitState state, float value)
        {
            state.leftTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetRightTrigger(ref XInputStageKitState state, float value)
        {
            state.rightTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetLeftStick(ref XInputStageKitState state, float x, float y)
        {
            state.leftStickX = DeviceHandling.DenormalizeInt16(x);
            state.leftStickY = DeviceHandling.DenormalizeInt16(y);
        }

        protected override void SetRightStick(ref XInputStageKitState state, float x, float y)
        {
            state.rightStickX = DeviceHandling.DenormalizeInt16(x);
            state.rightStickY = DeviceHandling.DenormalizeInt16(y);
        }
    }

    internal class SantrollerHidStageKitTests
        : StageKitTests<SantrollerHidStageKit, SantrollerHidStageKitState>
    {
        protected override SantrollerHidStageKitState CreateState()
            => new SantrollerHidStageKitState()
        {
            dpad = 8,
            leftStickX = 0x80,
            leftStickY = 0x80,
            rightStickX = 0x80,
            rightStickY = 0x80,
        };

        protected override void SetDpad(ref SantrollerHidStageKitState state, DpadDirection dpad)
        {
            state.dpad = HidDeviceHandling.GetDpadByte(dpad);
        }

        protected override void SetFaceButtons(ref SantrollerHidStageKitState state, FaceButton buttons)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.South, (buttons & FaceButton.South) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.East, (buttons & FaceButton.East) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.West, (buttons & FaceButton.West) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.North, (buttons & FaceButton.North) != 0);
        }

        protected override void SetMenuButtons(ref SantrollerHidStageKitState state, MenuButton buttons)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.Start, (buttons & MenuButton.Start) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.Select, (buttons & MenuButton.Select) != 0);
        }

        protected override void SetShoulderButtons(ref SantrollerHidStageKitState state, SideButton buttons)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.LeftShoulder, (buttons & SideButton.Left) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.RightShoulder, (buttons & SideButton.Right) != 0);
        }

        protected override void SetStickButtons(ref SantrollerHidStageKitState state, SideButton buttons)
        {
            state.buttons.SetBit((ushort)SantrollerHIDButton.LeftStickClick, (buttons & SideButton.Left) != 0);
            state.buttons.SetBit((ushort)SantrollerHIDButton.RightStickClick, (buttons & SideButton.Right) != 0);
        }

        protected override void SetLeftTrigger(ref SantrollerHidStageKitState state, float value)
        {
            state.leftTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetRightTrigger(ref SantrollerHidStageKitState state, float value)
        {
            state.rightTrigger = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        protected override void SetLeftStick(ref SantrollerHidStageKitState state, float x, float y)
        {
            state.leftStickX = DeviceHandling.DenormalizeByteSigned(x);
            state.leftStickY = DeviceHandling.DenormalizeByteSigned(-y);
        }

        protected override void SetRightStick(ref SantrollerHidStageKitState state, float x, float y)
        {
            state.rightStickX = DeviceHandling.DenormalizeByteSigned(x);
            state.rightStickY = DeviceHandling.DenormalizeByteSigned(-y);
        }
    }

    public static class XInputStageKitHandling
    {
        public static void SetShoulderButtons(ref ushort buttonsField, SideButton buttons)
        {
            buttonsField.SetBit((ushort)XInputButton.LeftShoulder, (buttons & SideButton.Left) != 0);
            buttonsField.SetBit((ushort)XInputButton.RightShoulder, (buttons & SideButton.Right) != 0);
        }

        public static void SetStickButtons(ref ushort buttonsField, SideButton buttons)
        {
            buttonsField.SetBit((ushort)XInputButton.LeftThumb, (buttons & SideButton.Left) != 0);
            buttonsField.SetBit((ushort)XInputButton.RightThumb, (buttons & SideButton.Right) != 0);
        }
    }
}