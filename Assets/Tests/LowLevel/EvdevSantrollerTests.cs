using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Tests.LowLevel
{
    internal class EvdevSantrollerTests : InputTestFixture
    {
        public override void Setup()
        {
            base.Setup();
            Initialization.Initialize();
        }

        private static InputDeviceDescription Describe(int type, int protocol, int product = 0x2882)
        {
            var keys = new byte[96];
            for (int code = 0x130; code <= 0x13E; code++)
                keys[code / 8] |= (byte)(1 << (code & 7));
            return new InputDeviceDescription()
            {
                interfaceName = "Evdev",
                capabilities = $"{{\"driver\":\"hid-generic\",\"vendorId\":4617,\"productId\":{product}," +
                    $"\"subType\":{type},\"santrollerProtocol\":{protocol}," +
                    $"\"keys\":\"{Convert.ToBase64String(keys)}\",\"axes\":\"//////////8=\"}}",
            };
        }

        private static int ParseIdentity(byte[] descriptors, int targetInterface, out int flags)
        {
            if (UnityEngine.Application.platform != UnityEngine.RuntimePlatform.LinuxEditor &&
                UnityEngine.Application.platform != UnityEngine.RuntimePlatform.LinuxPlayer)
                Assert.Ignore("The evdev backend is Linux-only.");
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(candidate => candidate.GetName().Name == "HIDrogen");
            if (assembly == null)
                Assert.Ignore("Descriptor integration tests require the optional HIDrogen package.");
            var parser = assembly.GetType("HIDrogen.Backend.EvdevBackend")
                .GetMethod("ParseUsbInstrumentIdentity", BindingFlags.Static | BindingFlags.NonPublic);
            object[] arguments = { descriptors, targetInterface, true, 0 };
            int type = (int)parser.Invoke(null, arguments);
            flags = (int)arguments[3];
            return type;
        }

        private static byte[] Interface(byte number = 0, byte alternate = 0, byte deviceClass = 3,
            byte subclass = 0, byte protocol = 0)
            => new byte[] { 9, 4, number, alternate, 2, deviceClass, subclass, protocol, 0 };

        private static byte[] Identity(byte type = 3, byte flags = 0)
            => new byte[] { 9, 0xFF, 0x53, 0x41, 0x4E, 0x54, 2, type, flags };

        private static byte[] Join(params byte[][] descriptors)
            => descriptors.SelectMany(bytes => bytes).ToArray();

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        public void ReadsCurrentIdentityFromUsbDescriptor(int type)
        {
            byte[] descriptors = Join(Interface(), new byte[] { 2, 0x21 },
                new byte[] { 7, 5, 0x81, 3, 64, 0, 1 },
                new byte[] { 7, 5, 0x01, 3, 64, 0, 1 }, Identity((byte)type, 0xA5));
            Assert.That(ParseIdentity(descriptors, 0, out int flags), Is.EqualTo(type));
            Assert.That(flags, Is.EqualTo(0xA5));
        }

        [Test]
        public void CompositeInterfaceIdentityDoesNotLeakBetweenProfiles()
        {
            var descriptors = Join(Interface(), Identity(3, 1), Interface(1), Identity(8, 2));
            Assert.That(ParseIdentity(descriptors, 0, out int firstFlags), Is.EqualTo(3));
            Assert.That(firstFlags, Is.EqualTo(1));
            Assert.That(ParseIdentity(descriptors, 1, out int secondFlags), Is.EqualTo(8));
            Assert.That(secondFlags, Is.EqualTo(2));
            Assert.That(ParseIdentity(descriptors, 2, out int missingFlags), Is.Zero);
            Assert.That(missingFlags, Is.Zero);
        }

        [TestCase("missing")]
        [TestCase("signature")]
        [TestCase("version")]
        [TestCase("length")]
        [TestCase("class")]
        [TestCase("subclass")]
        [TestCase("protocol")]
        [TestCase("alternate")]
        [TestCase("before")]
        [TestCase("next interface")]
        [TestCase("next configuration")]
        public void DoesNotGuessUnknownUsbIdentity(string kind)
        {
            byte[] identity = Identity();
            byte[] header = Interface();
            byte[] descriptors = null;
            switch (kind)
            {
                case "missing": identity = new byte[0]; break;
                case "signature": identity[2] = 0; break;
                case "version": identity[6] = 3; break;
                case "length": identity = new byte[] { 2, 0xFF }; break;
                case "class": header = Interface(deviceClass: 0xFF); break;
                case "subclass": header = Interface(subclass: 1); break;
                case "protocol": header = Interface(protocol: 1); break;
                case "alternate": header = Interface(alternate: 1); break;
                case "before": descriptors = Join(identity, header); break;
                case "next interface": descriptors = Join(header, Interface(1), identity); break;
                case "next configuration": descriptors = Join(header, new byte[] { 2, 2 }, identity); break;
            }
            Assert.That(ParseIdentity(descriptors ?? Join(header, identity), 0, out int flags), Is.Zero);
            Assert.That(flags, Is.Zero);
        }

        [TestCase("header")]
        [TestCase("zero length")]
        [TestCase("one length")]
        [TestCase("truncated")]
        [TestCase("short interface")]
        [TestCase("zero subtype")]
        public void MalformedUsbIdentityFailsExplicitly(string kind)
        {
            byte[] descriptors;
            switch (kind)
            {
                case "header": descriptors = new byte[] { 9 }; break;
                case "zero length": descriptors = new byte[] { 0, 4 }; break;
                case "one length": descriptors = new byte[] { 1, 4 }; break;
                case "truncated": descriptors = Join(Interface(), Identity().Take(8).ToArray()); break;
                case "short interface": descriptors = new byte[] { 2, 4 }; break;
                default: descriptors = Join(Interface(), Identity(0)); break;
            }
            var exception = Assert.Throws<TargetInvocationException>(() => ParseIdentity(descriptors, 0, out _));
            Assert.That(exception.InnerException, Is.TypeOf<IOException>());
        }

        [TestCase(3, typeof(EvdevSantrollerGuitarHeroGuitar), typeof(EvdevSantrollerV2GuitarHeroGuitar))]
        [TestCase(4, typeof(EvdevSantrollerRockBandGuitar), typeof(EvdevSantrollerV2RockBandGuitar))]
        [TestCase(5, typeof(EvdevSantrollerFiveLaneDrumkit), typeof(EvdevSantrollerV2FiveLaneDrumkit))]
        [TestCase(6, typeof(EvdevSantrollerFourLaneDrumkit), typeof(EvdevSantrollerV2FourLaneDrumkit))]
        [TestCase(7, typeof(EvdevSantrollerSixFretGuitar), typeof(EvdevSantrollerV2SixFretGuitar))]
        [TestCase(8, typeof(EvdevSantrollerTurntable), typeof(EvdevSantrollerV2Turntable))]
        public void MatchesLegacyAndCurrentInstrumentIdentity(int type, Type expectedLegacy, Type expectedV2)
        {
            Assert.That(InputSystem.AddDevice(Describe(type, 1)).GetType(), Is.EqualTo(expectedLegacy));
            Assert.That(InputSystem.AddDevice(Describe(type, 2)).GetType(), Is.EqualTo(expectedV2));
            Assert.That(InputSystem.AddDevice(Describe(type, 2, 0x2883)).GetType(), Is.EqualTo(expectedV2));
        }

        [TestCase(3, 0, 0x2882)]
        [TestCase(3, 3, 0x2882)]
        [TestCase(3, 1, 0x2883)]
        [TestCase(3, 2, 0x2884)]
        [TestCase(1, 2, 0x2883)]
        [TestCase(9, 2, 0x2883)]
        [TestCase(16, 1, 0x2882)]
        [TestCase(17, 1, 0x2882)]
        [TestCase(18, 1, 0x2882)]
        public void DoesNotGuessMissingOrUnsupportedIdentity(int type, int protocol, int product)
            => Assert.That(InputSystem.TryFindMatchingLayout(Describe(type, protocol, product)), Is.Null.Or.Empty);

        [TestCase(16, typeof(EvdevSantrollerV2ProKeyboard))]
        [TestCase(17, typeof(EvdevSantrollerV2ProGuitar))]
        [TestCase(18, typeof(EvdevSantrollerV2ProGuitar))]
        public void MatchesCurrentProInstrumentIdentity(int type, Type expected)
        {
            Assert.That(InputSystem.AddDevice(Describe(type, 2)).GetType(), Is.EqualTo(expected));
            Assert.That(InputSystem.AddDevice(Describe(type, 2, 0x2883)).GetType(), Is.EqualTo(expected));
        }

        [Test]
        public void CurrentKeyboardKeepsKeyBitmapOverdriveAndPedal()
        {
            var keyboard = (ProKeyboard)InputSystem.AddDevice(Describe(16, 2));
            EvdevState state = default;
            state.SetAxis(2, 0x80);
            state.SetAxis(0, unchecked((short)0x8001));
            state.SetAxis(4, unchecked((short)0x8080));
            InputSystem.QueueStateEvent(keyboard, state);
            InputSystem.Update();
            Assert.That(keyboard.key1.isPressed, Is.True);
            Assert.That(keyboard.key24.isPressed, Is.True);
            Assert.That(keyboard.key25.isPressed, Is.True);
            Assert.That(keyboard.overdrive.isPressed, Is.True);
            Assert.That(keyboard.digitalPedal.isPressed, Is.True);
            Assert.That(keyboard.touchStrip.ReadValue(), Is.Zero);
        }

        [TestCase(17)]
        [TestCase(18)]
        public unsafe void CurrentProGuitarKeepsFretsAndStrums(int type)
        {
            var guitar = (ProGuitar)InputSystem.AddDevice(Describe(type, 2));
            var report = new XInputProGuitarState()
            {
                fret1 = 7, fret6 = 19, velocity1 = 73, velocity6 = 101,
                south = true, start = true,
            };
            var raw = *(XInputGamepad*)&report;
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(0x13B, true);
            state.SetAxis(2, raw.leftTrigger);
            state.SetAxis(5, raw.rightTrigger);
            state.SetAxis(0, raw.leftStickX);
            state.SetAxis(1, raw.leftStickY);
            state.SetAxis(3, raw.rightStickX);
            state.SetAxis(4, raw.rightStickY);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.GetChildControl<IntegerControl>("fret1").ReadValue(), Is.EqualTo(7));
            Assert.That(guitar.GetChildControl<IntegerControl>("fret6").ReadValue(), Is.EqualTo(19));
            Assert.That(guitar.GetChildControl<ButtonControl>("strum1").isPressed, Is.True);
            Assert.That(guitar.GetChildControl<ButtonControl>("strum6").isPressed, Is.True);
            InputSystem.Update();
            Assert.That(guitar.GetChildControl<ButtonControl>("strum1").isPressed, Is.False);
            Assert.That(guitar.GetChildControl<ButtonControl>("strum6").isPressed, Is.False);
            Assert.That(guitar.buttonSouth.isPressed, Is.True);
            Assert.That(guitar.start.isPressed, Is.True);
            Assert.That(guitar.fret1.ReadValue(), Is.EqualTo(7));
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.strum1.isPressed, Is.False);
            Assert.That(guitar.strum6.isPressed, Is.False);
        }

        [TestCase(3, 1)]
        [TestCase(4, 1)]
        [TestCase(5, 1)]
        [TestCase(6, 1)]
        [TestCase(7, 1)]
        [TestCase(8, 1)]
        [TestCase(3, 2)]
        [TestCase(4, 2)]
        [TestCase(5, 2)]
        [TestCase(6, 2)]
        [TestCase(7, 2)]
        [TestCase(8, 2)]
        public void MissingAxesAreRejected(int type, int protocol)
        {
            var description = Describe(type, protocol);
            description.capabilities = description.capabilities.Replace("//////////8=", "AAAAAAAAAAA=");
            Assert.Throws<NotSupportedException>(() => InputSystem.AddDevice(description));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void MissingButtonsAreRejected(int protocol)
        {
            var description = Describe(3, protocol);
            var caps = UnityEngine.JsonUtility.FromJson<EvdevCapabilities>(description.capabilities);
            caps.keys = Convert.ToBase64String(new byte[96]);
            description.capabilities = UnityEngine.JsonUtility.ToJson(caps);
            Assert.Throws<NotSupportedException>(() => InputSystem.AddDevice(description));
        }

        [Test]
        public void LegacyGuitarReportsFretsPedalNavigationAndAxes()
        {
            var guitar = (GuitarHeroGuitar)InputSystem.AddDevice(Describe(3, 1));
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(0x134, true);
            state.SetKey(0x137, true);
            state.SetKey(0x13A, true);
            state.SetAxis(0, 255);
            state.SetAxis(1, 0x95);
            state.SetAxis(3, 255);
            state.SetAxis(0x11, -1);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.greenFret.isPressed, Is.True);
            Assert.That(guitar.yellowFret.isPressed, Is.True);
            Assert.That(guitar.spPedal.isPressed, Is.True);
            Assert.That(guitar.select.isPressed, Is.True);
            Assert.That(guitar.dpad.up.isPressed, Is.True);
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(1).Within(0.01));
        }

        [TestCase(0x137, "soloGreen")]
        [TestCase(0x138, "soloRed")]
        [TestCase(0x139, "soloYellow")]
        [TestCase(0x132, "soloBlue")]
        [TestCase(0x135, "soloOrange")]
        public void LegacyRockBandKeepsIndependentSoloFrets(int key, string control)
        {
            var guitar = (RockBandGuitar)InputSystem.AddDevice(Describe(4, 1));
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(key, true);
            state.SetAxis(0, 255);
            state.SetAxis(1, 4);
            state.SetAxis(3, 128);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.greenFret.isPressed, Is.True);
            Assert.That(guitar.GetChildControl<ButtonControl>(control).isPressed, Is.True);
            Assert.That(guitar.pickupSwitch.ReadValue(), Is.EqualTo(4));
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.tilt.ReadValue(), Is.Zero);
        }

        [Test]
        public void LegacySixFretReportsFretsGhtvAndAxes()
        {
            var guitar = (SixFretGuitar)InputSystem.AddDevice(Describe(7, 1));
            EvdevState state = default;
            state.SetKey(0x134, true);
            state.SetKey(0x133, true);
            state.SetKey(0x13D, true);
            state.SetAxis(0, 255);
            state.SetAxis(1, 255);
            InputSystem.QueueStateEvent(guitar, state);
            InputSystem.Update();
            Assert.That(guitar.black3.isPressed, Is.True);
            Assert.That(guitar.white1.isPressed, Is.True);
            Assert.That(guitar.ghtvButton.isPressed, Is.True);
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(1).Within(0.01));
        }

        [TestCase(0, "greenPad")]
        [TestCase(1, "redPad")]
        [TestCase(3, "yellowCymbal")]
        [TestCase(4, "bluePad")]
        [TestCase(2, "orangeCymbal")]
        [TestCase(5, "kick")]
        public void LegacyFiveLanePreservesVelocityAndRelease(int axis, string controlName)
        {
            var drums = InputSystem.AddDevice(Describe(5, 1));
            int key = axis == 0 ? 0x130 : axis == 1 ? 0x131 : axis == 3 ? 0x134 :
                axis == 4 ? 0x133 : axis == 2 ? 0x136 : 0x137;
            AssertVelocity(drums, axis, controlName, key);
        }

        [TestCase(0, "greenPad")]
        [TestCase(1, "redPad")]
        [TestCase(3, "yellowPad")]
        [TestCase(4, "bluePad")]
        [TestCase(2, "greenCymbal")]
        [TestCase(5, "yellowCymbal")]
        [TestCase(6, "blueCymbal")]
        public void LegacyFourLanePreservesIndependentVelocityAndRelease(int axis, string controlName)
        {
            var drums = InputSystem.AddDevice(Describe(6, 1));
            AssertVelocity(drums, axis, controlName);
        }

        private static void AssertVelocity(InputDevice drums, int axis, string controlName, int key = -1)
        {
            var control = drums.GetChildControl<ButtonControl>(controlName);
            EvdevState state = default;
            if (key >= 0)
                state.SetKey(key, true);
            state.SetAxis(axis, 183);
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(control.ReadValue(), Is.EqualTo(183f / 255).Within(0.01));
            state.SetAxis(axis, 0);
            if (key >= 0)
                state.SetKey(key, false);
            InputSystem.QueueStateEvent(drums, state);
            InputSystem.Update();
            Assert.That(control.ReadValue(), Is.Zero);
        }

        [TestCase(0x132, "rightTableGreen")]
        [TestCase(0x135, "rightTableRed")]
        [TestCase(0x136, "rightTableBlue")]
        [TestCase(0x137, "leftTableGreen")]
        [TestCase(0x138, "leftTableRed")]
        [TestCase(0x139, "leftTableBlue")]
        public void LegacyTurntableKeepsAllPlatterButtonsAndAxes(int key, string controlName)
        {
            var table = (Turntable)InputSystem.AddDevice(Describe(8, 1));
            EvdevState state = default;
            state.SetKey(key, true);
            state.SetAxis(0, 255);
            state.SetAxis(1, 0);
            state.SetAxis(3, 255);
            state.SetAxis(4, 128);
            InputSystem.QueueStateEvent(table, state);
            InputSystem.Update();
            Assert.That(table.GetChildControl<ButtonControl>(controlName).isPressed, Is.True);
            Assert.That(table.leftTableVelocity.ReadValue(), Is.EqualTo(1).Within(0.01));
            Assert.That(table.rightTableVelocity.ReadValue(), Is.EqualTo(-1).Within(0.01));
            Assert.That(table.crossfader.ReadValue(), Is.EqualTo(0).Within(0.01));
            Assert.That(table.effectsDial.ReadValue(), Is.GreaterThan(0.99));
        }

        private static EvdevState CurrentState()
        {
            EvdevState state = default;
            state.SetKey(0x130, true);
            state.SetKey(0x131, true);
            state.SetKey(0x134, true);
            state.SetKey(0x13A, true);
            state.SetAxis(2, 0x05);
            state.SetAxis(5, 0x06);
            state.SetAxis(0, 0x1234);
            state.SetAxis(1, -0x2345);
            state.SetAxis(3, 0x4567);
            state.SetAxis(4, -0x5678);
            state.SetAxis(0x10, -1);
            return state;
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        public unsafe void CurrentReportsPreservePackedXInputDataWithoutXpadInversion(int type)
        {
            var device = InputSystem.AddDevice(Describe(type, 2));
            var state = CurrentState();
            var raw = new XInputGamepad()
            {
                buttons = (ushort)(XInputButton.A | XInputButton.B | XInputButton.Y | XInputButton.Back | XInputButton.DpadLeft),
                leftTrigger = 0x05, rightTrigger = 0x06,
                leftStickX = 0x1234, leftStickY = -0x2345,
                rightStickX = 0x4567, rightStickY = -0x5678,
            };
            Assert.That(device.layout, Does.StartWith("EvdevSantrollerV2"));
            InputSystem.QueueStateEvent(device, state);
            InputSystem.Update();
            Assert.That(device.GetChildControl<ButtonControl>("select").isPressed, Is.True);
        }
    }
}
