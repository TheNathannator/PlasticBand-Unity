using System;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public sealed class ProKeyboardTests : PlasticBandTestFixture<ProKeyboard>
    {
        [Test]
        public void GetKeyReturnsCorrectFrets()
            => CreateAndRun(_GetKeyReturnsCorrectFrets);

        [Test]
        public void GetKeyThrowsCorrectly()
            => CreateAndRun(_GetKeyThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetKeyReturnsCorrectFrets(ProKeyboard keyboard)
        {
            Assert.That(keyboard.GetKey(0), Is.EqualTo(keyboard.key1));
            Assert.That(keyboard.GetKey(1), Is.EqualTo(keyboard.key2));
            Assert.That(keyboard.GetKey(2), Is.EqualTo(keyboard.key3));
            Assert.That(keyboard.GetKey(3), Is.EqualTo(keyboard.key4));
            Assert.That(keyboard.GetKey(4), Is.EqualTo(keyboard.key5));
            Assert.That(keyboard.GetKey(5), Is.EqualTo(keyboard.key6));
            Assert.That(keyboard.GetKey(6), Is.EqualTo(keyboard.key7));
            Assert.That(keyboard.GetKey(7), Is.EqualTo(keyboard.key8));
            Assert.That(keyboard.GetKey(8), Is.EqualTo(keyboard.key9));
            Assert.That(keyboard.GetKey(9), Is.EqualTo(keyboard.key10));
            Assert.That(keyboard.GetKey(10), Is.EqualTo(keyboard.key11));
            Assert.That(keyboard.GetKey(11), Is.EqualTo(keyboard.key12));
            Assert.That(keyboard.GetKey(12), Is.EqualTo(keyboard.key13));
            Assert.That(keyboard.GetKey(13), Is.EqualTo(keyboard.key14));
            Assert.That(keyboard.GetKey(14), Is.EqualTo(keyboard.key15));
            Assert.That(keyboard.GetKey(15), Is.EqualTo(keyboard.key16));
            Assert.That(keyboard.GetKey(16), Is.EqualTo(keyboard.key17));
            Assert.That(keyboard.GetKey(17), Is.EqualTo(keyboard.key18));
            Assert.That(keyboard.GetKey(18), Is.EqualTo(keyboard.key19));
            Assert.That(keyboard.GetKey(19), Is.EqualTo(keyboard.key20));
            Assert.That(keyboard.GetKey(20), Is.EqualTo(keyboard.key21));
            Assert.That(keyboard.GetKey(21), Is.EqualTo(keyboard.key22));
            Assert.That(keyboard.GetKey(22), Is.EqualTo(keyboard.key23));
            Assert.That(keyboard.GetKey(23), Is.EqualTo(keyboard.key24));
            Assert.That(keyboard.GetKey(24), Is.EqualTo(keyboard.key25));
        }

        public static void _GetKeyThrowsCorrectly(ProKeyboard keyboard)
        {
            for (int i = -5; i < ProKeyboard.KeyCount + 5; i++)
            {
                if (i < 0 || i >= ProKeyboard.KeyCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => keyboard.GetKey(i));
                else
                    Assert.DoesNotThrow(() => keyboard.GetKey(i));
            }
        }
    }

    internal abstract class ProKeyboardTests<TKeyboard, TState> : FaceButtonDeviceTestFixture<TKeyboard, TState>
        where TKeyboard : ProKeyboard
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected abstract void SetKey(ref TState state, int key, bool pressed);
        protected abstract void SetKeys(ref TState state, int keyMask);

        protected abstract void SetOverdriveButton(ref TState state, bool pressed);

        protected abstract void SetDigitalPedal(ref TState state, bool pressed);
        protected abstract void SetAnalogPedal(ref TState state, float value);

        protected override ButtonControl GetFaceButton(TKeyboard keyboard, FaceButton button)
        {
            switch (button)
            {
                case FaceButton.South: return keyboard.buttonSouth;
                case FaceButton.East: return keyboard.buttonEast;
                case FaceButton.West: return keyboard.buttonWest;
                case FaceButton.North: return keyboard.buttonNorth;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override ButtonControl GetMenuButton(TKeyboard keyboard, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return keyboard.startButton;
                case MenuButton.Select: return keyboard.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override DpadControl GetDpad(TKeyboard keyboard) => keyboard.dpad;

        [Test]
        public void GetKeyReturnsCorrectFrets()
            => CreateAndRun(ProKeyboardTests._GetKeyReturnsCorrectFrets);

        [Test]
        public void GetKeyThrowsCorrectly()
            => CreateAndRun(ProKeyboardTests._GetKeyThrowsCorrectly);

        [Test]
        public void RecognizesKeys() => CreateAndRun((keyboard) =>
        {
            var state = CreateState();
            for (int key = 0; key < ProKeyboard.KeyCount; key++)
            {
                var keyControl = keyboard.GetKey(key);
                SetKey(ref state, key, true);
                AssertButtonPress(keyboard, state, keyControl);

                SetKey(ref state, key, false);
                AssertButtonPress(keyboard, state);
            }
        });

        [Test]
        public void GetKeyMaskReturnsCorrectKeys() => CreateAndRun((keyboard) =>
        {
            var state = CreateState();

            // For performance reasons, we test 5 bits of the mask at a time instead of all
            // 25 bits. This brings the number of permutations down from 33,554,432, which
            // would take around 6 1/2 days to run at 60 updates of the input system per second,
            // to only 156, which takes around 3 seconds at 60 updates per second.
            const int keyIncrement = 5;
            for (int keyStart = 0; keyStart < ProKeyboard.KeyCount; keyStart += keyIncrement)
            {
                int keyEnd = keyStart + keyIncrement;
                if (keyEnd > ProKeyboard.KeyCount)
                    keyEnd = ProKeyboard.KeyCount;

                int maskStart = keyStart == 0 ? 0 : 1 << keyStart;
                int maxMask = DeviceHandling.CreateMask(keyStart, keyEnd);
                for (int keys = maskStart; keys < maxMask; keys += 1 << keyStart)
                {
                    SetKeys(ref state, keys);
                    AssertButtonMask(keyboard, state, keys, keyboard.GetKeyMask, keyboard.GetKeyMask, AssertMask);
                }
            }

            void AssertMask(int mask, int targetMask, Func<ButtonControl, bool> buttonPressed)
            {
                Assert.That(mask, Is.EqualTo(targetMask), "Key mask is not correct!");

                for (int key = 0; key < ProKeyboard.KeyCount; key++)
                {
                    Assert.That((mask & (1 << key)) != 0, Is.EqualTo(buttonPressed(keyboard.GetKey(key))), $"Key {key + 1} state is not correct!");
                }
            }
        });

        [Test]
        public void RecognizesOverdriveButton() => CreateAndRun((keyboard) =>
        {
            RecognizesButton(keyboard, CreateState(), keyboard.overdrive, SetOverdriveButton);
        });

        [Test]
        public void RecognizesDigitalPedal() => CreateAndRun((keyboard) =>
        {
            RecognizesButton(keyboard, CreateState(), keyboard.digitalPedal, SetDigitalPedal);
        });

        [Test]
        public void RecognizesAnalogPedal() => CreateAndRun((keyboard) =>
        {
            RecognizesUnsignedAxis(keyboard, CreateState(), keyboard.analogPedal, SetAnalogPedal);
        });
    }

    internal abstract class ProKeyboardTests_TouchStrip<TKeyboard, TState> : ProKeyboardTests<TKeyboard, TState>
        where TKeyboard : ProKeyboard
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected abstract void SetTouchStrip(ref TState state, float value);

        [Test]
        public void RecognizesTouchStrip() => CreateAndRun((keyboard) =>
        {
            RecognizesUnsignedAxis(keyboard, CreateState(), keyboard.touchStrip, SetTouchStrip);
        });
    }
}