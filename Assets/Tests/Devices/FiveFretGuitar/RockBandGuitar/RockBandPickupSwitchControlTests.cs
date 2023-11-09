using NUnit.Framework;
using PlasticBand.Controls;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Devices
{
    internal struct RockBandPickupSwitchState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('R', 'B', 'P', 'K');

        [InputControl(layout = "RockBandPickupSwitch", parameters = "hasNullValue")]
        public byte pickupSwitch;
    }

    [InputControlLayout(stateType = typeof(RockBandPickupSwitchState), hideInUI = true)]
    internal class RockBandPickupSwitchDevice : InputDevice
    {
        public IntegerControl pickupSwitch { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            pickupSwitch = GetChildControl<RockBandPickupSwitchControl>(nameof(pickupSwitch));
        }
    }

    internal class RockBandPickupSwitchControlTests : PlasticBandTestFixture<RockBandPickupSwitchDevice>
    {
        public override void Setup()
        {
            base.Setup();
            InputSystem.RegisterLayout<RockBandPickupSwitchDevice>(nameof(RockBandPickupSwitchDevice));
        }

        public override void TearDown()
        {
            InputSystem.RemoveLayout(nameof(RockBandPickupSwitchDevice));
            base.TearDown();
        }

        [Test]
        public void HandlesNotches() => CreateAndRun((device) =>
        {
            var state = new RockBandPickupSwitchState();
            AssertIntegerValue(device, state, 0, device.pickupSwitch);

            const float notchSize = (byte.MaxValue + 1) / 5f;
            for (int value = 0; value <= byte.MaxValue; value++)
            {
                // Null value, ignore
                if (value == RockBandPickupSwitchControl.kNullValue)
                    continue;

                state.pickupSwitch = (byte)value;

                // Determine expected notch
                int notch = (int)(value / notchSize);
                Assert.That(notch, Is.InRange(0, RockBandGuitar.PickupNotchCount), "Calculated expected notch is out of bounds!");

                // Check actual notch value
                AssertIntegerValue(device, state, notch, device.pickupSwitch);
                Assert.That(device.pickupSwitch.value, Is.InRange(0, RockBandGuitar.PickupNotchCount));

                // Test that null value does not affect state
                state.pickupSwitch = RockBandPickupSwitchControl.kNullValue;
                AssertIntegerValue(device, state, notch, device.pickupSwitch);
            }
        });
    }
}