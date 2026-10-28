using System;
using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Tests.LowLevel
{
    internal class EvdevXpadTests : InputTestFixture
    {
        public override void Setup()
        {
            base.Setup();
            Initialization.Initialize();
            EvdevXpadSixFretGuitar.Initialize();
            EvdevXpadFourLaneDrumkit.Initialize();
            EvdevXpadFiveLaneDrumkit.Initialize();
            EvdevXpadTurntable.Initialize();
            EvdevXpadProGuitar.Initialize();
            EvdevXpadProKeyboard.Initialize();
        }

        private static InputDeviceDescription Describe(int subType, int vendor = 0x1209, int product = 0x2883, string name = null)
        {
            var keys = new byte[96];
            var axes = new byte[8];
            for (int i = 0; i < axes.Length; i++)
                axes[i] = 0xff;
            return new InputDeviceDescription
            {
                interfaceName = "Evdev",
                product = name,
                capabilities = $"{{\"driver\":\"xpad\",\"vendorId\":{vendor},\"productId\":{product}," +
                    $"\"subType\":{subType},\"keys\":\"{Convert.ToBase64String(keys)}\"," +
                    $"\"axes\":\"{Convert.ToBase64String(axes)}\"}}",
            };
        }

        [TestCase(7, typeof(EvdevXpadGuitarHeroGuitar))]
        [TestCase(6, typeof(EvdevXpadRockBandGuitar))]
        [TestCase(15, typeof(EvdevXpadProKeyboard))]
        [TestCase(23, typeof(EvdevXpadTurntable))]
        [TestCase(25, typeof(EvdevXpadProGuitar))]
        public void MatchesSubtypeBasedXpadInstruments(int subType, Type expected)
        {
            Assert.That(InputSystem.AddDevice(Describe(subType)).GetType(), Is.EqualTo(expected));
        }

        [TestCase("Xbox 360 Wireless Guitar Hero Drum Kit", typeof(EvdevXpadFiveLaneDrumkit))]
        [TestCase("Xbox 360 Wireless Rock Band Drum Kit", typeof(EvdevXpadFourLaneDrumkit))]
        public void MatchesWirelessDrumProfilesByDriverName(string name, Type expected)
        {
            Assert.That(InputSystem.AddDevice(Describe(8, 0x045e, 0x02a8, name)).GetType(), Is.EqualTo(expected));
        }

        [TestCase(0x1BAD, 0x0003, typeof(EvdevXpadFourLaneDrumkit))]
        [TestCase(0x1BAD, 0x0130, typeof(EvdevXpadFourLaneDrumkit))]
        [TestCase(0x15E4, 0x0130, typeof(EvdevXpadFourLaneDrumkit))]
        [TestCase(0x1430, 0x0805, typeof(EvdevXpadFiveLaneDrumkit))]
        public void MatchesKnownWiredDrumIds(int vendor, int product, Type expected)
        {
            Assert.That(InputSystem.AddDevice(Describe(8, vendor, product)).GetType(), Is.EqualTo(expected));
        }

        [Test]
        public void ProKeyboardDecodesKeysOverdriveAndPedalFromXpadAxes()
        {
            var keyboard = (ProKeyboard)InputSystem.AddDevice(Describe(15));
            var state = default(EvdevState);
            state.SetAxis(2, 0x80);
            state.SetAxis(0, unchecked((short)0x8001));
            state.SetAxis(4, unchecked((short)~0x8080));
            InputSystem.QueueStateEvent(keyboard, state);
            InputSystem.Update();
            Assert.That(keyboard.key1.isPressed, Is.True);
            Assert.That(keyboard.GetChildControl<ButtonControl>("key24").isPressed, Is.True);
            Assert.That(keyboard.key25.isPressed, Is.True);
            Assert.That(keyboard.overdrive.isPressed, Is.True);
            Assert.That(keyboard.digitalPedal.isPressed, Is.True);
            Assert.That(keyboard.touchStrip.ReadValue(), Is.Zero);
        }

        [Test]
        public void MatchesKnownXboxGuitarHeroLiveIdWithoutSubtype()
        {
            var description = Describe(7, 0x1430, 0x079B);
            description.capabilities = description.capabilities.Replace("\"subType\":7,", "");
            var device = InputSystem.AddDevice(description);
            Assert.That(device, Is.TypeOf<EvdevXpadSixFretGuitar>());
        }

        [Test]
        public void DoesNotMatchUnknownSixFretProductId()
        {
            Assert.That(InputSystem.AddDevice(Describe(7, 0x1430, 0x079C)).GetType(),
                Is.Not.EqualTo(typeof(EvdevXpadSixFretGuitar)));
        }

        [Test]
        public void XboxGuitarHeroLiveEvdevPreservesFretsAndAnalogControls()
        {
            var guitar = (SixFretGuitar)InputSystem.AddDevice(Describe(7, 0x1430, 0x079B));
            var state = default(EvdevState);
            state.SetKey(0x130, true);
            state.SetAxis(1, -32768);
            state.SetAxis(3, 32767);
            state.SetAxis(4, -32768);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.black1.isPressed, Is.True);
            Assert.That(guitar.dpad.up.isPressed, Is.True);
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
        }

        [Test]
        public void FourLaneDrumkitDecodesVelocitiesAndPadsFromXpadAxes()
        {
            var drums = (FourLaneDrumkit)InputSystem.AddDevice(Describe(8, 0x1BAD, 0x0003));
            var state = default(EvdevState);
            state.SetKey(0x130, true);
            state.SetAxis(4, unchecked((short)~(-32767 + ((120 << 7) | (120 >> 1)))));
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(drums.greenPad.isPressed, Is.True);
            Assert.That(drums.greenPad.ReadValue(), Is.EqualTo(120));
        }

        [TestCase(9)]
        [TestCase(0)]
        [TestCase(8)]
        public void DoesNotClaimUnsupportedXpadSubtype(int subType)
        {
            Assert.That(InputSystem.TryFindMatchingLayout(Describe(subType)), Is.Null.Or.Empty);
        }
    }
}
