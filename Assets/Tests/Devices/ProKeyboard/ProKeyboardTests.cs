using System;
using NUnit.Framework;
using PlasticBand.Devices;

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
}