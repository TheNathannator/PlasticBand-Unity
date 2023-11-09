using System;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public sealed class FourLaneDrumkitTests : PlasticBandTestFixture<FourLaneDrumkit>
    {
        public const FourLanePad AllPads = FourLanePad.Kick1 | FourLanePad.Kick2 |
            FourLanePad.RedPad | FourLanePad.YellowPad | FourLanePad.BluePad | FourLanePad.GreenPad |
            FourLanePad.YellowCymbal | FourLanePad.BlueCymbal | FourLanePad.GreenCymbal;

        [Test]
        public void GetPadReturnsCorrectPads()
            => CreateAndRun(_GetPadReturnsCorrectPads);

        [Test]
        public void GetPadThrowsCorrectly()
            => CreateAndRun(_GetPadThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetPadReturnsCorrectPads(FourLaneDrumkit drumkit)
        {
            // Ensure GetPad returns the correct controls
            Assert.That(drumkit.GetPad(0), Is.EqualTo(drumkit.kick1));
            Assert.That(drumkit.GetPad(1), Is.EqualTo(drumkit.kick2));
            Assert.That(drumkit.GetPad(2), Is.EqualTo(drumkit.redPad));
            Assert.That(drumkit.GetPad(3), Is.EqualTo(drumkit.yellowPad));
            Assert.That(drumkit.GetPad(4), Is.EqualTo(drumkit.bluePad));
            Assert.That(drumkit.GetPad(5), Is.EqualTo(drumkit.greenPad));
            Assert.That(drumkit.GetPad(6), Is.EqualTo(drumkit.yellowCymbal));
            Assert.That(drumkit.GetPad(7), Is.EqualTo(drumkit.blueCymbal));
            Assert.That(drumkit.GetPad(8), Is.EqualTo(drumkit.greenCymbal));

            Assert.That(drumkit.GetPad(FourLanePad.Kick1), Is.EqualTo(drumkit.kick1));
            Assert.That(drumkit.GetPad(FourLanePad.Kick2), Is.EqualTo(drumkit.kick2));
            Assert.That(drumkit.GetPad(FourLanePad.RedPad), Is.EqualTo(drumkit.redPad));
            Assert.That(drumkit.GetPad(FourLanePad.YellowPad), Is.EqualTo(drumkit.yellowPad));
            Assert.That(drumkit.GetPad(FourLanePad.BluePad), Is.EqualTo(drumkit.bluePad));
            Assert.That(drumkit.GetPad(FourLanePad.GreenPad), Is.EqualTo(drumkit.greenPad));
            Assert.That(drumkit.GetPad(FourLanePad.YellowCymbal), Is.EqualTo(drumkit.yellowCymbal));
            Assert.That(drumkit.GetPad(FourLanePad.BlueCymbal), Is.EqualTo(drumkit.blueCymbal));
            Assert.That(drumkit.GetPad(FourLanePad.GreenCymbal), Is.EqualTo(drumkit.greenCymbal));
        }

        public static void _GetPadThrowsCorrectly(FourLaneDrumkit drumkit)
        {
            for (int i = -5; i < FourLaneDrumkit.PadCount + 5; i++)
            {
                if (i < 0 || i >= FourLaneDrumkit.PadCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => drumkit.GetPad(i));
                else
                    Assert.DoesNotThrow(() => drumkit.GetPad(i));
            }

            for (var pads = FourLanePad.None; pads <= AllPads; pads++)
            {
                int padCount = 0;
                if ((pads & FourLanePad.Kick1) != 0) padCount++;
                if ((pads & FourLanePad.Kick2) != 0) padCount++;
                if ((pads & FourLanePad.RedPad) != 0) padCount++;
                if ((pads & FourLanePad.YellowPad) != 0) padCount++;
                if ((pads & FourLanePad.BluePad) != 0) padCount++;
                if ((pads & FourLanePad.GreenPad) != 0) padCount++;
                if ((pads & FourLanePad.YellowCymbal) != 0) padCount++;
                if ((pads & FourLanePad.BlueCymbal) != 0) padCount++;
                if ((pads & FourLanePad.GreenCymbal) != 0) padCount++;

                if (padCount != 1)
                    Assert.Throws<ArgumentException>(() => drumkit.GetPad(pads));
                else
                    Assert.DoesNotThrow(() => drumkit.GetPad(pads));
            }
        }
    }
}