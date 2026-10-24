using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;

namespace PlasticBand.Tests.Devices
{
    public class XboxOneWirelessLegacyAdapterTests : InputTestFixture
    {
        private const byte kGuitar = 1;
        private const byte kDrums = 2;

        private int m_DeviceId;
        private int m_InfoRequests;

        public override unsafe void Setup()
        {
            base.Setup();

            // The input test fixture resets the input system, so we must re-initialize everything here
            Initialization.Initialize();

            m_InfoRequests = 0;
            InputSystem.onDeviceCommand += OnDeviceCommand;
        }

        public override unsafe void TearDown()
        {
            InputSystem.onDeviceCommand -= OnDeviceCommand;
            base.TearDown();
        }

        // Stands in for HIDrogen's GameInput backend, which sends GIPO commands as raw output reports
        private unsafe long? OnDeviceCommand(InputDevice device, InputDeviceCommand* command)
        {
            if (!(device is XboxOneWirelessLegacyAdapter) || command->type != GameInputDefinitions.OutputFormat)
                return null;

            if (*(byte*)command->payloadPtr == 0x24)
                m_InfoRequests++;
            return InputDeviceCommand.GenericSuccess;
        }

        private InputDevice CreateAdapter()
        {
            // Same description HIDrogen's GameInput backend gives a wireless legacy adapter
            var adapter = InputSystem.AddDevice(new InputDeviceDescription()
            {
                interfaceName = GameInputDefinitions.InterfaceName,
                capabilities = "{\"vendorId\":1848,\"productId\":16740}",
            });
            m_DeviceId = adapter.deviceId;
            return adapter;
        }

        // Queues a report the way HIDrogen does: report ID first, padded to at least 10 bytes
        private unsafe void QueueReport(params byte[] report)
        {
            var data = new byte[System.Math.Max(report.Length, 10)];
            report.CopyTo(data, 0);

            int eventSize = data.Length + (sizeof(StateEvent) - 1);
            byte* buffer = stackalloc byte[eventSize];
            var stateEvent = (StateEvent*)buffer;
            *stateEvent = new StateEvent()
            {
                baseEvent = new InputEvent(StateEvent.Type, eventSize, m_DeviceId),
                stateFormat = GameInputDefinitions.InputFormat,
            };
            fixed (byte* src = data)
            {
                UnsafeUtility.MemCpy(stateEvent->state, src, data.Length);
            }

            InputSystem.QueueEvent(new InputEventPtr((InputEvent*)stateEvent));
            InputSystem.Update();
        }

        private static unsafe byte[] ToBytes<TState>(TState state)
            where TState : unmanaged
        {
            var bytes = new byte[sizeof(TState)];
            fixed (byte* dst = bytes)
            {
                *(TState*)dst = state;
            }
            return bytes;
        }

        // Device info as a Santroller emulating the adapter sends it: slot, type, big-endian vendor ID,
        // an unknown byte, the XInput subtype and the name in UTF-16
        private void QueueDeviceInfo(byte slot, byte type)
        {
            switch (type)
            {
                case kGuitar:
                    QueueReport(0x22, slot, type, 0x14, 0x30, 0x00, 0x87,
                        0x67, 0x00, 0x75, 0x00, 0x69, 0x00, 0x74, 0x00, 0x61, 0x00, 0x72, 0x00);
                    break;
                case kDrums:
                    QueueReport(0x22, slot, type, 0x1B, 0xAD, 0x00, 0x88,
                        0x64, 0x00, 0x72, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x73, 0x00);
                    break;
                default:
                    QueueReport(0x22, slot, type, 0x04, 0x5E, 0x00, 0x81,
                        0x67, 0x00, 0x61, 0x00, 0x6D, 0x00, 0x65, 0x00, 0x70, 0x00, 0x61, 0x00, 0x64, 0x00);
                    break;
            }
        }

        // An input report: the adapter's header (report ID, buttons, slot, type), then the instrument's own
        // report without its report ID
        private void QueueInput<TState>(byte slot, byte type, TState instrumentState)
            where TState : unmanaged
        {
            var instrument = ToBytes(instrumentState);
            var report = new List<byte>() { 0x20, instrument[1], instrument[2], slot, type };
            report.AddRange(instrument.Skip(1));
            QueueReport(report.ToArray());
        }

        private void QueueDisconnect(byte slot) => QueueReport(0x23, slot);

        private static List<TDevice> Devices<TDevice>() where TDevice : InputDevice
            => InputSystem.devices.OfType<TDevice>().ToList();

        [Test]
        public void MatchesAndRequestsDeviceInfo()
        {
            var adapter = CreateAdapter();
            Assert.That(adapter, Is.TypeOf<XboxOneWirelessLegacyAdapter>());
            Assert.That(m_InfoRequests, Is.EqualTo(1));
        }

        [Test]
        public void DeviceInfoCreatesInstruments()
        {
            CreateAdapter();
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(2, kDrums);

            Assert.That(Devices<XboxOneRockBandGuitar>(), Has.Count.EqualTo(1));
            Assert.That(Devices<XboxOneFourLaneDrumkit>(), Has.Count.EqualTo(1));
        }

        [Test]
        public void InputReachesTheRightInstrument()
        {
            CreateAdapter();
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(1, kDrums);
            var guitar = Devices<XboxOneRockBandGuitar>().Single();
            var drums = Devices<XboxOneFourLaneDrumkit>().Single();

            QueueInput(0, kGuitar, new XboxOneRockBandGuitarState() { reportId = 0x20, green = true, red = true });
            Assert.That(guitar.greenFret.isPressed, Is.True);
            Assert.That(guitar.redFret.isPressed, Is.True);
            Assert.That(guitar.yellowFret.isPressed, Is.False);
            Assert.That(drums.redPad.isPressed, Is.False);

            QueueInput(1, kDrums, new XboxOneFourLaneDrumkitState() { reportId = 0x20, redPadVelocity = byte.MaxValue });
            Assert.That(drums.redPad.isPressed, Is.True);
            Assert.That(guitar.greenFret.isPressed, Is.True);
        }

        [Test]
        public void DisconnectRemovesTheRightInstrument()
        {
            CreateAdapter();
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(1, kDrums);

            QueueDisconnect(1);
            Assert.That(Devices<XboxOneFourLaneDrumkit>(), Is.Empty);
            Assert.That(Devices<XboxOneRockBandGuitar>(), Has.Count.EqualTo(1));

            QueueDisconnect(0);
            Assert.That(Devices<XboxOneRockBandGuitar>(), Is.Empty);
        }

        [Test]
        public void RepeatedDeviceInfoKeepsUnchangedInstruments()
        {
            CreateAdapter();
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(1, kGuitar);
            var guitars = Devices<XboxOneRockBandGuitar>();

            // The adapter answers a request with device info for every slot
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(1, kGuitar);
            Assert.That(Devices<XboxOneRockBandGuitar>(), Is.EquivalentTo(guitars));

            // A slot that changes type still gets a new device
            QueueDeviceInfo(1, kDrums);
            Assert.That(Devices<XboxOneRockBandGuitar>(), Is.EquivalentTo(new[] { guitars[0] }));
            Assert.That(Devices<XboxOneFourLaneDrumkit>(), Has.Count.EqualTo(1));
        }

        // Moves past the window the adapter has to confirm its slots in after focus comes back
        private void PassConfirmTimeout()
        {
            currentTime += XboxOneWirelessLegacyAdapter.kConfirmTimeout + 0.1;
            InputSystem.Update();
        }

        [Test]
        public void RegainingFocusRequestsDeviceInfo()
        {
            var adapter = (XboxOneWirelessLegacyAdapter)CreateAdapter();
            QueueDeviceInfo(0, kGuitar);

            adapter.OnFocusChanged(false);
            Assert.That(m_InfoRequests, Is.EqualTo(1));

            adapter.OnFocusChanged(true);
            Assert.That(m_InfoRequests, Is.EqualTo(2));
        }

        [Test]
        public void RegainingFocusKeepsConfirmedInstruments()
        {
            var adapter = (XboxOneWirelessLegacyAdapter)CreateAdapter();
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(1, kDrums);
            var guitar = Devices<XboxOneRockBandGuitar>().Single();
            var drums = Devices<XboxOneFourLaneDrumkit>().Single();

            adapter.OnFocusChanged(true);
            QueueDeviceInfo(0, kGuitar);
            // Input confirms a slot as well
            QueueInput(1, kDrums, new XboxOneFourLaneDrumkitState() { reportId = 0x20 });
            PassConfirmTimeout();

            Assert.That(Devices<XboxOneRockBandGuitar>(), Is.EquivalentTo(new[] { guitar }));
            Assert.That(Devices<XboxOneFourLaneDrumkit>(), Is.EquivalentTo(new[] { drums }));
        }

        [Test]
        public void RegainingFocusRemovesInstrumentsThatLeft()
        {
            var adapter = (XboxOneWirelessLegacyAdapter)CreateAdapter();
            QueueDeviceInfo(0, kGuitar);
            QueueDeviceInfo(1, kGuitar);
            var guitars = Devices<XboxOneRockBandGuitar>();

            // The guitar in slot 1 disconnected while the game didn't have focus, so its disconnect was missed
            adapter.OnFocusChanged(true);
            QueueDeviceInfo(0, kGuitar);
            Assert.That(Devices<XboxOneRockBandGuitar>(), Has.Count.EqualTo(2));

            PassConfirmTimeout();
            Assert.That(Devices<XboxOneRockBandGuitar>(), Is.EquivalentTo(new[] { guitars[0] }));
        }

        [Test]
        public void RegainingFocusPicksUpInstrumentsThatChanged()
        {
            var adapter = (XboxOneWirelessLegacyAdapter)CreateAdapter();
            QueueDeviceInfo(0, kGuitar);

            // Swapped for drums while the game didn't have focus
            adapter.OnFocusChanged(true);
            QueueDeviceInfo(0, kDrums);
            PassConfirmTimeout();

            Assert.That(Devices<XboxOneRockBandGuitar>(), Is.Empty);
            Assert.That(Devices<XboxOneFourLaneDrumkit>(), Has.Count.EqualTo(1));
        }

        [Test]
        public void KnownSlotsDoNotRequestDeviceInfo()
        {
            CreateAdapter();
            QueueDeviceInfo(0, kGuitar);

            for (int i = 0; i < 5; i++)
                QueueInput(0, kGuitar, new XboxOneRockBandGuitarState() { reportId = 0x20, green = (i & 1) != 0 });

            Assert.That(m_InfoRequests, Is.EqualTo(1));
        }

        [Test]
        public void UnsupportedSlotsDoNotRequestDeviceInfoRepeatedly()
        {
            CreateAdapter();
            // A gamepad on the adapter, which there's no layout for
            LogAssert.Expect(LogType.Error, new Regex("Unexpected wireless legacy adapter device type 0"));
            QueueDeviceInfo(1, 0);

            for (int i = 0; i < 5; i++)
                QueueReport(0x20, (byte)i, 0x00, 1, 0, 0x00, 0x00);

            Assert.That(m_InfoRequests, Is.EqualTo(1));
        }
    }
}
