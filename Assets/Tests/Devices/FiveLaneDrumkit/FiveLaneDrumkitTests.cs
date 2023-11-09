using System;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public sealed class FiveLaneDrumkitTests : PlasticBandTestFixture<FiveLaneDrumkit>
    {
        public const FiveLanePad AllPads = FiveLanePad.Kick | FiveLanePad.Red | FiveLanePad.Yellow |
            FiveLanePad.Blue | FiveLanePad.Orange | FiveLanePad.Green;

        [Test]
        public void GetPadReturnsCorrectPads()
            => CreateAndRun(_GetPadReturnsCorrectPads);

        [Test]
        public void GetPadThrowsCorrectly()
            => CreateAndRun(_GetPadThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetPadReturnsCorrectPads(FiveLaneDrumkit drumkit)
        {
            Assert.That(drumkit.GetPad(0), Is.EqualTo(drumkit.kick));
            Assert.That(drumkit.GetPad(1), Is.EqualTo(drumkit.redPad));
            Assert.That(drumkit.GetPad(2), Is.EqualTo(drumkit.yellowCymbal));
            Assert.That(drumkit.GetPad(3), Is.EqualTo(drumkit.bluePad));
            Assert.That(drumkit.GetPad(4), Is.EqualTo(drumkit.orangeCymbal));
            Assert.That(drumkit.GetPad(5), Is.EqualTo(drumkit.greenPad));

            Assert.That(drumkit.GetPad(FiveLanePad.Kick), Is.EqualTo(drumkit.kick));
            Assert.That(drumkit.GetPad(FiveLanePad.Red), Is.EqualTo(drumkit.redPad));
            Assert.That(drumkit.GetPad(FiveLanePad.Yellow), Is.EqualTo(drumkit.yellowCymbal));
            Assert.That(drumkit.GetPad(FiveLanePad.Blue), Is.EqualTo(drumkit.bluePad));
            Assert.That(drumkit.GetPad(FiveLanePad.Orange), Is.EqualTo(drumkit.orangeCymbal));
            Assert.That(drumkit.GetPad(FiveLanePad.Green), Is.EqualTo(drumkit.greenPad));
        }

        public static void _GetPadThrowsCorrectly(FiveLaneDrumkit drumkit)
        {
            for (int i = -5; i < FiveLaneDrumkit.PadCount + 5; i++)
            {
                if (i < 0 || i >= FiveLaneDrumkit.PadCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => drumkit.GetPad(i));
                else
                    Assert.DoesNotThrow(() => drumkit.GetPad(i));
            }

            for (var pads = FiveLanePad.None; pads <= AllPads; pads++)
            {
                int padCount = 0;
                if ((pads & FiveLanePad.Kick) != 0) padCount++;
                if ((pads & FiveLanePad.Red) != 0) padCount++;
                if ((pads & FiveLanePad.Yellow) != 0) padCount++;
                if ((pads & FiveLanePad.Blue) != 0) padCount++;
                if ((pads & FiveLanePad.Orange) != 0) padCount++;
                if ((pads & FiveLanePad.Green) != 0) padCount++;

                if (padCount != 1)
                    Assert.Throws<ArgumentException>(() => drumkit.GetPad(pads));
                else
                    Assert.DoesNotThrow(() => drumkit.GetPad(pads));
            }
        }
    }
}