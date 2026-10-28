using System;
using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Tests.LowLevel
{
    internal class EvdevSonyTests : InputTestFixture
    {
        public override void Setup()
        {
            base.Setup();
            Initialization.Initialize();
        }

        private static InputDeviceDescription Describe(int vendor, int product)
        {
            var keys = new byte[96];
            keys[0x130 / 8] = 0xDB;
            return new InputDeviceDescription()
            {
                interfaceName = "Evdev",
                capabilities = $"{{\"driver\":\"sony\",\"vendorId\":{vendor},\"productId\":{product}," +
                    $"\"keys\":\"{Convert.ToBase64String(keys)}\",\"axes\":\"//////////8=\"}}",
            };
        }

        [TestCase(0x12BA, 0x0100, typeof(EvdevSonyGuitarHeroGuitar))]
        [TestCase(0x1430, 0x474C, typeof(EvdevSonyGuitarHeroGuitar))]
        [TestCase(0x12BA, 0x0200, typeof(EvdevSonyRockBandGuitar))]
        [TestCase(0x1BAD, 0x0004, typeof(EvdevSonyRockBandGuitar))]
        [TestCase(0x1BAD, 0x3010, typeof(EvdevSonyRockBandGuitar))]
        [TestCase(0x12BA, 0x074B, typeof(EvdevSonySixFretGuitar))]
        [TestCase(0x1430, 0x07BB, typeof(EvdevSonySixFretGuitar))]
        [TestCase(0x12BA, 0x0120, typeof(EvdevSonyFiveLaneDrumkit))]
        [TestCase(0x12BA, 0x0210, typeof(EvdevSonyFourLaneDrumkit))]
        [TestCase(0x12BA, 0x0218, typeof(EvdevSonyFourLaneDrumkit))]
        [TestCase(0x1BAD, 0x0005, typeof(EvdevSonyFourLaneDrumkit))]
        [TestCase(0x1BAD, 0x3110, typeof(EvdevSonyFourLaneDrumkit))]
        [TestCase(0x1BAD, 0x3138, typeof(EvdevSonyFourLaneDrumkit))]
        [TestCase(0x12BA, 0x0140, typeof(EvdevSonyTurntable))]
        [TestCase(0x0738, 0x8261, typeof(EvdevSonyPS4RockBandGuitar))]
        [TestCase(0x0E6F, 0x0173, typeof(EvdevSonyPS4RockBandGuitar))]
        [TestCase(0x0E6F, 0x024A, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x0E6F, 0x0249, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x3651, 0x1500, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x3651, 0x5500, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x3651, 0x1600, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x3651, 0x5600, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x3958, 0x5500, typeof(EvdevSonyRiffmasterGuitar))]
        [TestCase(0x3958, 0x5600, typeof(EvdevSonyRiffmasterGuitar))]
        public void MatchesSonyInstrumentIdentity(int vendor, int product, Type expected)
            => Assert.That(InputSystem.AddDevice(Describe(vendor, product)).GetType(), Is.EqualTo(expected));

        [TestCase(0x2330)]
        [TestCase(0x2338)]
        [TestCase(0x2430)]
        [TestCase(0x2438)]
        [TestCase(0x2538)]
        public void DoesNotClaimProInstrumentsWithMissingGameplayData(int product)
            => Assert.That(InputSystem.TryFindMatchingLayout(Describe(0x12BA, product)), Is.Null.Or.Empty);

        [TestCase(0x12BA, 0x074B)]
        [TestCase(0x1430, 0x07BB)]
        public void SixFretReportsFretsAndInstrumentAxes(int vendor, int product)
        {
            var guitar = (SixFretGuitar)InputSystem.AddDevice(Describe(vendor, product));
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(0x136, true);
            state.SetAxis(1, 0);
            state.SetAxis(product == 0x074B ? 5 : 2, 255);
            state.SetAxis(product == 0x074B ? 4 : 5, product == 0x074B ? 1023 : 255);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.black1.isPressed, Is.True);
            Assert.That(guitar.white2.isPressed, Is.True);
            Assert.That(guitar.black2.isPressed, Is.False);
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(1).Within(0.01));
        }

        [TestCase(0x12BA, 0x0120, "redPad", 0x131)]
        [TestCase(0x12BA, 0x0120, "yellowCymbal", 0x133)]
        [TestCase(0x12BA, 0x0120, "orangeCymbal", 0x137)]
        [TestCase(0x12BA, 0x0120, "kick", 0x136)]
        [TestCase(0x12BA, 0x0210, "redPad", 0x131)]
        [TestCase(0x12BA, 0x0210, "greenPad", 0x130)]
        [TestCase(0x12BA, 0x0210, "kick1", 0x136)]
        public void SonyDrumHitsAreDigitalAndRelease(int vendor, int product, string controlName, int key)
        {
            var drums = InputSystem.AddDevice(Describe(vendor, product));
            var control = drums.GetChildControl<ButtonControl>(controlName);
            EvdevState state = default;
            state.SetKey(key, true);
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(control.ReadValue(), Is.EqualTo(1));
            state.SetKey(key, false);
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(control.ReadValue(), Is.Zero);
        }

        [Test]
        public void RockBandDrumCymbalFlagRoutesHitWithoutNavigation()
        {
            var drums = (FourLaneDrumkit)InputSystem.AddDevice(Describe(0x12BA, 0x0210));
            EvdevState state = default;
            state.SetKey(0x133, true);
            state.SetKey(0x13E, true);
            state.SetAxis(0x11, -1);
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(drums.yellowCymbal.ReadValue(), Is.EqualTo(1));
            Assert.That(drums.yellowPad.isPressed, Is.False);
            Assert.That(drums.dpad.up.isPressed, Is.False);
            state.SetKey(0x13E, false);
            state.SetKey(0x13D, true);
            state.SetAxis(0x11, 0);
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(drums.yellowCymbal.isPressed, Is.False);
            Assert.That(drums.yellowPad.ReadValue(), Is.EqualTo(1));
        }

        [TestCase(0x12BA, 0x074B)]
        [TestCase(0x12BA, 0x0120)]
        [TestCase(0x12BA, 0x0140)]
        [TestCase(0x0E6F, 0x0249)]
        public void MissingSonyAxesAreRejected(int vendor, int product)
        {
            var description = Describe(vendor, product);
            description.capabilities = description.capabilities.Replace("//////////8=", "AAAAAAAAAAA=");
            Assert.Throws<NotSupportedException>(() => InputSystem.AddDevice(description));
        }

        [TestCase(0x0738, 0x8261)]
        [TestCase(0x0E6F, 0x024A)]
        [TestCase(0x0E6F, 0x0249)]
        public void NewSonyGuitarsRouteSharedFretsAndPreserveInjectedAxes(int vendor, int product)
        {
            var guitar = (RockBandGuitar)InputSystem.AddDevice(Describe(vendor, product));
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(0x136, true);
            state.SetAxis(0, 128);
            state.SetAxis(1, 128);
            state.SetAxis(2, 255);
            state.SetAxis(5, 255);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.greenFret.isPressed, Is.True);
            Assert.That(guitar.orangeFret.isPressed, Is.True);
            Assert.That(guitar.soloGreen.isPressed, Is.False);
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(1).Within(0.01));
            state.SetKey(0x13D, true);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.greenFret.isPressed, Is.False);
            Assert.That(guitar.soloGreen.isPressed, Is.True);
        }

        [TestCase(0x024A)]
        [TestCase(0x0249)]
        public void RiffmasterJoystickAndP1DoNotInventJoystickClick(int product)
        {
            var guitar = (RiffmasterGuitar)InputSystem.AddDevice(Describe(0x0E6F, product));
            EvdevState state = default;
            state.SetAxis(0, 255);
            state.SetAxis(1, 0);
            state.SetKey(0x13E, true);
            state.SetKey(0x13D, true);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.joystick.x.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.joystick.y.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.GetChildControl<ButtonControl>("p1Button").isPressed, Is.True);
            Assert.That(guitar.joystickClick.isPressed, Is.False);
        }

        [Test]
        public void TurntableReportsPlatterBitsAndAxes()
        {
            var description = Describe(0x12BA, 0x0140);
            var axes = new byte[8];
            axes[0] = 0x3B;
            description.capabilities = description.capabilities.Replace("//////////8=", Convert.ToBase64String(axes));
            var table = (Turntable)InputSystem.AddDevice(description);
            EvdevState state = default;
            state.SetAxis(0, 255);
            state.SetAxis(1, 0);
            state.SetAxis(3, 0x041);
            state.SetAxis(4, 1023);
            state.SetAxis(5, 1023);
            InputSystem.QueueStateEvent(table, state);
            InputSystem.Update();
            Assert.That(table.rightTableGreen.isPressed, Is.True);
            Assert.That(table.leftTableBlue.isPressed, Is.True);
            Assert.That(table.leftTableGreen.isPressed, Is.False);
            Assert.That(table.GetChildControl<AxisControl>("leftTableVelocity").ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(table.GetChildControl<AxisControl>("crossfader").ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(table.dpad.ReadValue(), Is.EqualTo(UnityEngine.Vector2.zero));
        }
    }
}
