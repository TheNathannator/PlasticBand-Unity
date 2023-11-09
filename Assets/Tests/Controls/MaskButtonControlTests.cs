using System;
using NUnit.Framework;
using PlasticBand.Controls;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Controls
{
    [Flags]
    internal enum MaskButton : int
    {
        None = 0,

        Button1 = 0x01,
        Button2 = 0x02,
        Button3 = 0x04,

        All = Button1 | Button2 | Button3
    }

    internal struct MaskButtonState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('M', 'A', 'S', 'K');

        public const MaskButton ActiveMask = MaskButton.Button1 | MaskButton.Button3;

        [InputControl(layout = "MaskButton", parameters = "mask=0x00000005")]
        public int maskButton;
    }

    [InputControlLayout(stateType = typeof(MaskButtonState), hideInUI = true)]
    internal class MaskButtonDevice : InputDevice
    {
        public MaskButtonControl maskButton { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            maskButton = GetChildControl<MaskButtonControl>(nameof(maskButton));
        }
    }

    internal class MaskButtonControlTests : PlasticBandTestFixture<MaskButtonDevice>
    {
        public override void Setup()
        {
            base.Setup();
            InputSystem.RegisterLayout<MaskButtonDevice>(nameof(MaskButtonDevice));
        }

        public override void TearDown()
        {
            InputSystem.RemoveLayout(nameof(MaskButtonDevice));
            base.TearDown();
        }

        [Test]
        public void SetsMask() => CreateAndRun((device) =>
        {
            Assert.That(device.maskButton.mask, Is.EqualTo((int)MaskButtonState.ActiveMask));
        });

        [Test]
        public void HandlesMask() => CreateAndRun((device) =>
        {
            for (var value = MaskButton.None; value <= MaskButton.All; value++)
            {
                var state = new MaskButtonState() { maskButton = (int)value };
                InputSystem.QueueStateEvent(device, state);
                InputSystem.Update();
                Assert.That(device.maskButton.isPressed,
                    Is.EqualTo((value & MaskButtonState.ActiveMask) == MaskButtonState.ActiveMask));
            }
        });
    }
}