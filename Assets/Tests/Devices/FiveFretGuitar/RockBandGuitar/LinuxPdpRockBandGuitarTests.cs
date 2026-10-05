#if UNITY_STANDALONE_LINUX
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Devices
{
    public class LinuxPdpRockBandGuitarTests : InputTestFixture
    {
        // Captured from the Xbox Riffmaster using xone 0.5.0 and Unity 6000.3.5f2.
        private const string CapturedCapabilities = "{\"controls\":[{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":27,\"bit\":0},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":28,\"bit\":1},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":29,\"bit\":2},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":3},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":4},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":5},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":6},{\"offset\":0,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":7},{\"offset\":1,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":0},{\"offset\":1,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":1},{\"offset\":1,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":2},{\"offset\":1,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":3},{\"offset\":1,\"featureSize\":1,\"featureType\":3,\"usageHint\":0,\"bit\":4},{\"offset\":4,\"featureSize\":4,\"featureType\":1,\"usageHint\":2,\"min\":-32768,\"max\":32767},{\"offset\":8,\"featureSize\":4,\"featureType\":1,\"usageHint\":3,\"min\":-32768,\"max\":32767},{\"offset\":12,\"featureSize\":4,\"featureType\":4,\"usageHint\":12,\"min\":-1,\"max\":1},{\"offset\":16,\"featureSize\":4,\"featureType\":4,\"usageHint\":13,\"min\":-1,\"max\":1}]}";

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct NativeState : IInputStateTypeInfo
        {
            public FourCC format => new FourCC('L', 'J', 'O', 'Y');
            public ushort buttons;
            public ushort padding;
            public int whammy;
            public int tilt;
            public int hatX;
            public int hatY;
        }

        public override void Setup()
        {
            base.Setup();
            Initialization.Initialize();
            var layoutBuilder = typeof(InputSystem).Assembly
                .GetType("UnityEngine.InputSystem.Linux.SDLLayoutBuilder");
            var method = layoutBuilder?.GetMethod("OnFindLayoutForDevice",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.That(method, Is.Not.Null);
            var finder = (InputDeviceFindControlLayoutDelegate)Delegate.CreateDelegate(
                typeof(InputDeviceFindControlLayoutDelegate), method);
            InputSystem.onFindLayoutForDevice -= finder;
        }

        [Test]
        public void CapturedDescriptorCreatesGuitar()
        {
            var guitar = CreateGuitar();
            Assert.That(guitar.layout, Is.EqualTo("LinuxPdpRockBandGuitar"));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void RecognizesMainFrets(int index)
        {
            var guitar = CreateGuitar();
            Update(guitar, new NativeState { buttons = (ushort)(1 << (index + 3)), whammy = -32767, tilt = -32767 });
            for (int fret = 0; fret < FiveFretGuitar.FretCount; fret++)
            {
                Assert.That(guitar.GetFret(fret).isPressed, Is.EqualTo(fret == index));
                Assert.That(guitar.GetSoloFret(fret).isPressed, Is.False);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void RecognizesSoloFretsWithoutMirroring(int index)
        {
            var guitar = CreateGuitar();
            Update(guitar, new NativeState { buttons = (ushort)(1 << (index + 8)), whammy = -32767, tilt = -32767 });
            for (int fret = 0; fret < FiveFretGuitar.FretCount; fret++)
            {
                Assert.That(guitar.GetSoloFret(fret).isPressed, Is.EqualTo(fret == index));
                Assert.That(guitar.GetFret(fret).isPressed, Is.False);
            }
        }

        [TestCase(-1, 0)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(0, 1)]
        public void RecognizesDpadAndStrumming(int x, int y)
        {
            var guitar = CreateGuitar();
            Update(guitar, new NativeState { hatX = x, hatY = y, whammy = -32767, tilt = -32767 });
            Assert.That(guitar.dpad.left.isPressed, Is.EqualTo(x < 0));
            Assert.That(guitar.dpad.right.isPressed, Is.EqualTo(x > 0));
            Assert.That(guitar.strumUp.isPressed, Is.EqualTo(y < 0));
            Assert.That(guitar.strumDown.isPressed, Is.EqualTo(y > 0));
        }

        [TestCase(1, true, false)]
        [TestCase(2, false, true)]
        public void RecognizesMenuButtons(int buttons, bool select, bool start)
        {
            var guitar = CreateGuitar();
            Update(guitar, new NativeState { buttons = (ushort)buttons, whammy = -32767, tilt = -32767 });
            Assert.That(guitar.selectButton.isPressed, Is.EqualTo(select));
            Assert.That(guitar.startButton.isPressed, Is.EqualTo(start));
        }

        [TestCase(-32767, 0f)]
        [TestCase(0, 0.5f)]
        [TestCase(32767, 1f)]
        public void RecognizesWhammy(int raw, float expected)
        {
            var guitar = CreateGuitar();
            Update(guitar, new NativeState { whammy = raw, tilt = -32767 });
            Assert.That(guitar.whammy.ReadValue(), Is.EqualTo(expected).Within(1f / 255));
        }

        [TestCase(-32767, 0f)]
        [TestCase(0, 0.5f)]
        [TestCase(32767, 1f)]
        public void RecognizesTilt(int raw, float expected)
        {
            var guitar = CreateGuitar();
            Update(guitar, new NativeState { whammy = -32767, tilt = raw });
            Assert.That(guitar.tilt.ReadValue(), Is.EqualTo(expected).Within(1f / 127));
        }

        [Test]
        public void CanReconnect()
        {
            var first = CreateGuitar();
            InputSystem.RemoveDevice(first);
            var second = CreateGuitar();
            Assert.That(second, Is.Not.SameAs(first));
            Assert.That(FiveFretGuitar.all, Has.Member(second));
            Assert.That(FiveFretGuitar.all, Has.No.Member(first));
        }

        [Test]
        public void RejectsUnexpectedReportDescriptor()
        {
            var description = Description();
            description.capabilities = "{\"controls\":[]}";
            Assert.Throws<NotSupportedException>(() => InputSystem.AddDevice(description));
        }

        private static RockBandGuitar CreateGuitar()
        {
            var device = InputSystem.AddDevice(Description());
            Assert.That(device, Is.InstanceOf<RockBandGuitar>());
            return (RockBandGuitar)device;
        }

        private static InputDeviceDescription Description() => new()
        {
            interfaceName = "Linux",
            deviceClass = "Joystick",
            product = "PDP Rock Band 4 Jaguar",
            manufacturer = "Logic3",
            version = "256",
            capabilities = CapturedCapabilities
        };

        private static void Update(InputDevice device, NativeState state)
        {
            InputSystem.QueueStateEvent(device, state);
            InputSystem.Update();
        }
    }
}
#endif
