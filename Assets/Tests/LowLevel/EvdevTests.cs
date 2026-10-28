using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.LowLevel
{
    internal class EvdevTests : InputTestFixture
    {
        public override void Setup()
        {
            base.Setup();
            Initialization.Initialize();
        }

        private static InputDeviceDescription Describe(string driver, int vendor, int product, string name = null)
        {
            var keys = new byte[96];
            keys[0x130 / 8] = 0xFF;
            if (driver == "sony")
                keys[0x130 / 8] = 0xDB;
            return new InputDeviceDescription()
            {
                interfaceName = "Evdev",
                product = name,
                capabilities = $"{{\"driver\":\"{driver}\",\"vendorId\":{vendor},\"productId\":{product}," +
                    $"\"keys\":\"{Convert.ToBase64String(keys)}\",\"axes\":\"//////////8=\"}}",
            };
        }

        [Test]
        public void WireFormatHasExpectedSizeAndOffsets()
        {
            Assert.That(Marshal.SizeOf<EvdevState>(), Is.EqualTo(352));
            Assert.That(Marshal.OffsetOf<EvdevState>("axes").ToInt32(), Is.EqualTo(96));
            Assert.That(default(EvdevState).format.ToString(), Is.EqualTo("EVDV"));
        }

        [TestCase("xpad", 0x1430, 0x4748, typeof(EvdevXpadGuitarHeroGuitar))]
        [TestCase("sony", 0x12BA, 0x0100, typeof(EvdevSonyGuitarHeroGuitar))]
        [TestCase("xpad", 0x1BAD, 0x0002, typeof(EvdevXpadRockBandGuitar))]
        [TestCase("sony", 0x12BA, 0x0200, typeof(EvdevSonyRockBandGuitar))]
        public void MatchesKnownInstruments(string driver, int vendor, int product, Type expected)
        {
            Assert.That(InputSystem.AddDevice(Describe(driver, vendor, product)).GetType(), Is.EqualTo(expected));
        }

        [TestCase("Xbox 360 Wireless Guitar", typeof(EvdevXpadRockBandGuitar))]
        [TestCase("Xbox 360 Wireless Alternate Guitar", typeof(EvdevXpadGuitarHeroGuitar))]
        public void MatchesWirelessInstrumentIdentity(string name, Type expected)
        {
            Assert.That(InputSystem.AddDevice(Describe("xpad", 0x045E, 0x0719, name)).GetType(), Is.EqualTo(expected));
        }

        [TestCase("hid-generic", 0x12BA, 0x0100)]
        [TestCase("sony", 0x054C, 0x0268)]
        [TestCase("xpad", 0x045E, 0x028E)]
        public void DoesNotClaimOtherProfiles(string driver, int vendor, int product)
        {
            Assert.That(InputSystem.TryFindMatchingLayout(Describe(driver, vendor, product)), Is.Null.Or.Empty);
        }

        [TestCase(6, typeof(EvdevXpadRockBandGuitar))]
        [TestCase(7, typeof(EvdevXpadGuitarHeroGuitar))]
        [TestCase(11, typeof(EvdevXpadRockBandGuitar))]
        public void MatchesXInputDescriptorSubtypeWithoutKnownUsbIds(int subType, Type expected)
        {
            var description = Describe("xpad", 0x1209, 0x2883);
            description.capabilities = description.capabilities.Replace("{", $"{{\"subType\":{subType},");
            Assert.That(InputSystem.AddDevice(description).GetType(), Is.EqualTo(expected));
        }

        [TestCase(0, 0x130)]
        [TestCase(0x130, 0x131)]
        [TestCase(0x120, 0x121)]
        public void SupportsModernAndLegacySonyButtonMappings(int buttonBase, int greenCode)
        {
            EvdevState state = default;
            state.SetKey(greenCode, true);
            Assert.That(state.GetSonyButtons(buttonBase), Is.EqualTo(PS3Button.Cross));
        }

        [Test]
        public void ModernSonyMappingPreservesAllThirteenButtons()
        {
            EvdevState state = default;
            foreach (int code in new[] { 0x134, 0x130, 0x131, 0x133, 0x136, 0x137, 0x138,
                0x139, 0x13A, 0x13B, 0x13D, 0x13E, 0x13C })
                state.SetKey(code, true);
            Assert.That((int)state.GetSonyButtons(0), Is.EqualTo(0x1FFF));
        }

        [TestCase("xpad", 0x1430, 0x4748)]
        [TestCase("sony", 0x12BA, 0x0100)]
        public void GuitarHeroReportsFretsStrumAndWhammy(string driver, int vendor, int product)
        {
            var guitar = (GuitarHeroGuitar)InputSystem.AddDevice(Describe(driver, vendor, product));
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetAxis(0x11, -1);
            state.SetAxis(driver == "xpad" ? 3 : 2, driver == "xpad" ? short.MaxValue : 255);
            state.SetAxis(4, driver == "xpad" ? ~short.MaxValue : 0x280);
            state.SetAxis(driver == "xpad" ? 0 : 5, driver == "xpad" ? 0 : 128);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.greenFret.isPressed, Is.True);
            Assert.That(guitar.redFret.isPressed, Is.False);
            Assert.That(guitar.dpad.up.isPressed, Is.True);
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(1).Within(0.01));
        }

        [TestCase("xpad", 0x1430, 0x4748, -32768, 0f)]
        [TestCase("xpad", 0x1430, 0x4748, 0, 0.5f)]
        [TestCase("xpad", 0x1430, 0x4748, 32767, 1f)]
        [TestCase("sony", 0x12BA, 0x0100, 128, 0f)]
        [TestCase("sony", 0x12BA, 0x0100, 192, 0.5f)]
        [TestCase("sony", 0x12BA, 0x0100, 255, 1f)]
        public void GuitarHeroWhammyUsesInstrumentRange(string driver, int vendor, int product, int raw, float expected)
        {
            var guitar = (GuitarHeroGuitar)InputSystem.AddDevice(Describe(driver, vendor, product));
            EvdevState state = default;
            state.SetAxis(driver == "xpad" ? 3 : 2, raw);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(expected).Within(0.01));
        }

        [Test]
        public void XpadPreservesStrumTransitionsInOneUpdate()
        {
            var guitar = (GuitarHeroGuitar)InputSystem.AddDevice(Describe("xpad", 0x1430, 0x4748));
            EvdevState state = default;
            int changes = 0;
            var monitor = InputState.AddChangeMonitor(guitar.dpad.up, (control, time, eventPtr, index) => changes++);
            try
            {
                state.SetKey(0x220, true);
                InputSystem.QueueStateEvent(guitar, state);
                state.SetKey(0x220, false);
                InputSystem.QueueStateEvent(guitar, state);
                state.SetKey(0x220, true);
                InputSystem.QueueStateEvent(guitar, state);
                InputSystem.Update();
            }
            finally
            {
                InputState.RemoveChangeMonitor(guitar.dpad.up, monitor);
            }
            Assert.That(changes, Is.EqualTo(3));
        }

        [TestCase("xpad", 0x1BAD, 0x0002)]
        [TestCase("sony", 0x12BA, 0x0200)]
        public void RockBandRoutesSoloFrets(string driver, int vendor, int product)
        {
            var guitar = (RockBandGuitar)InputSystem.AddDevice(Describe(driver, vendor, product));
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(driver == "xpad" ? 0x13D : 0x138, true);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.soloGreen.isPressed, Is.True);
            Assert.That(guitar.greenFret.isPressed, Is.False);
        }

        [Test]
        public void MissingInstrumentAxesAreRejected()
        {
            var description = Describe("sony", 0x12BA, 0x0100);
            description.capabilities = description.capabilities.Replace("//////////8=", "AAAAAAAAAAA=");
            Assert.Throws<NotSupportedException>(() => InputSystem.AddDevice(description));
        }

        [Test]
        public void IdenticalInstrumentsHaveIndependentState()
        {
            var description = Describe("xpad", 0x1430, 0x4748);
            var first = (GuitarHeroGuitar)InputSystem.AddDevice(description);
            var second = (GuitarHeroGuitar)InputSystem.AddDevice(description);
            EvdevState state = default;
            state.SetKey(0x130, true);
            InputSystem.QueueStateEvent(first, state);
            state.SetKey(0x130, false);
            state.SetKey(0x131, true);
            InputSystem.QueueStateEvent(second, state);
            InputSystem.Update();
            Assert.That(first.greenFret.isPressed, Is.True);
            Assert.That(first.redFret.isPressed, Is.False);
            Assert.That(second.greenFret.isPressed, Is.False);
            Assert.That(second.redFret.isPressed, Is.True);
        }

        [Test]
        public void SonyRockBandNullValuesRetainPreviousAxes()
        {
            var guitar = (RockBandGuitar)InputSystem.AddDevice(Describe("sony", 0x12BA, 0x0200));
            EvdevState state = default;
            state.SetAxis(2, 255);
            state.SetAxis(5, 255);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            float whammy = guitar.whammy.ReadValue();
            int pickup = guitar.pickupSwitch.ReadValue();
            state.SetAxis(2, 127);
            state.SetAxis(5, 127);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(whammy));
            Assert.That(guitar.pickupSwitch.ReadValue(), Is.EqualTo(pickup));
        }
    }
}
