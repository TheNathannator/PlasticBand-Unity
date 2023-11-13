using PlasticBand.Devices;

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
    }

    internal class SantrollerHidStageKitTests
        : StageKitTests<SantrollerHidStageKit, SantrollerHidStageKitState>
    {
        protected override SantrollerHidStageKitState CreateState()
            => new SantrollerHidStageKitState()
        {
            dpad = 8,
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
    }
}