using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
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

    internal abstract class FiveLaneDrumkitTests<TDrumkit, TState> : FaceButtonDeviceTestFixture<TDrumkit, TState>
        where TDrumkit : FiveLaneDrumkit
        where TState : unmanaged, IFiveLaneDrumkitState
    {
        protected override ButtonControl GetFaceButton(TDrumkit drumkit, FaceButton button)
        {
            switch (button)
            {
                case FaceButton.South: return drumkit.buttonSouth;
                case FaceButton.East: return drumkit.buttonEast;
                case FaceButton.West: return drumkit.buttonWest;
                case FaceButton.North: return drumkit.buttonNorth;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override ButtonControl GetMenuButton(TDrumkit drumkit, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return drumkit.startButton;
                case MenuButton.Select: return drumkit.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override DpadControl GetDpad(TDrumkit drumkit) => drumkit.dpad;

        protected override void SetDpad(ref TState state, DpadDirection dpad)
        {
            state.dpadUp = dpad.IsUp();
            state.dpadDown = dpad.IsDown();
            state.dpadLeft = dpad.IsLeft();
            state.dpadRight = dpad.IsRight();
        }

        protected override void SetFaceButtons(ref TState state, FaceButton buttons)
        {
            state.green_south = (buttons & FaceButton.South) != 0;
            state.red_east = (buttons & FaceButton.East) != 0;
            state.blue_west = (buttons & FaceButton.West) != 0;
            state.yellow_north = (buttons & FaceButton.North) != 0;
        }

        protected override void SetMenuButtons(ref TState state, MenuButton buttons)
        {
            state.start = (buttons & MenuButton.Start) != 0;
            state.select = (buttons & MenuButton.Select) != 0;
        }

        protected void SetPad(ref TState state, FiveLanePad pad, float velocity)
        {
            bool hit = velocity > 0f;
            byte rawVelocity = DeviceHandling.DenormalizeByteUnsigned(velocity);
            switch (pad)
            {
                case FiveLanePad.Kick:
                    state.kick = hit;
                    state.kickVelocity = rawVelocity;
                    break;

                case FiveLanePad.Red:
                    state.red_east = hit;
                    state.redVelocity = rawVelocity;
                    break;

                case FiveLanePad.Yellow:
                    state.yellow_north = hit;
                    state.yellowVelocity = rawVelocity;
                    break;

                case FiveLanePad.Blue:
                    state.blue_west = hit;
                    state.blueVelocity = rawVelocity;
                    break;

                case FiveLanePad.Orange:
                    state.orange = hit;
                    state.orangeVelocity = rawVelocity;
                    break;

                case FiveLanePad.Green:
                    state.green_south = hit;
                    state.greenVelocity = rawVelocity;
                    break;

                default:
                    throw new ArgumentException($"Invalid pad value {pad}!", nameof(pad));
            }
        }

        [Test]
        public void GetPadReturnsCorrectPads()
            => CreateAndRun(FiveLaneDrumkitTests._GetPadReturnsCorrectPads);

        [Test]
        public void GetPadThrowsCorrectly()
            => CreateAndRun(FiveLaneDrumkitTests._GetPadThrowsCorrectly);

        private List<(FiveLanePad pad, ButtonControl button)> CreatePadMap(TDrumkit drumkit)
        {
            return new List<(FiveLanePad, ButtonControl)>()
            {
                (FiveLanePad.Kick,   drumkit.kick),
                (FiveLanePad.Red,    drumkit.redPad),
                (FiveLanePad.Yellow, drumkit.yellowCymbal),
                (FiveLanePad.Blue,   drumkit.bluePad),
                (FiveLanePad.Orange, drumkit.orangeCymbal),
                (FiveLanePad.Green,  drumkit.greenPad),
            };
        }

        private HashSet<FiveLanePad> CreatePadsList()
        {
            var pads = new FiveLanePad[]
            {
                // Kicks are handled specially
                // FiveLanePad.Kick,
                FiveLanePad.Red,
                FiveLanePad.Yellow,
                FiveLanePad.Blue,
                FiveLanePad.Orange,
                FiveLanePad.Green,
            };

            var padsList = new HashSet<FiveLanePad>() { FiveLanePad.Kick };
            foreach (var pad1 in pads)
            {
                foreach (var pad2 in pads)
                {
                    // Allow even if they're the same, so that single-pad entries are added as well
                    // if (pad1 == pad2)
                    //     continue;

                    padsList.Add(pad1 | pad2);
                    padsList.Add(FiveLanePad.Kick | pad1 | pad2);
                }
            }

            // Test resetting to none at the end
            padsList.Add(FiveLanePad.None);

            return padsList;
        }

        [Test]
        public void RecognizesPads() => CreateAndRun((drumkit) =>
        {
            var padMap = CreatePadMap(drumkit);
            var velocityList = new List<float> { 1f, 0.75f, 0.5f, 0.25f, 0f };

            var state = CreateState();
            foreach (var (pad, button) in padMap)
            {
                foreach (float velocity in velocityList)
                {
                    SetPad(ref state, pad, velocity);

                    if (velocity > 0f)
                        AssertButtonPress(drumkit, state, button);
                    else
                        AssertButtonPress(drumkit, state);

                    AssertAxisValue(drumkit, velocity, 1 / 100f, button);
                }
            }
        });

        [Test]
        public void RecognizesPadChords() => CreateAndRun((drumkit) =>
        {
            var padMap = CreatePadMap(drumkit);
            var padsList = CreatePadsList();

            var state = CreateState();
            var buttonList = new List<ButtonControl>(3);
            foreach (var chord in padsList)
            {
                foreach (var (pad, button) in padMap)
                {
                    if ((chord & pad) != 0)
                    {
                        SetPad(ref state, pad, 1f);
                        buttonList.Add(button);
                    }
                    else
                    {
                        SetPad(ref state, pad, 0f);
                    }
                }

                AssertButtonPress(drumkit, state, buttonList.ToArray());

                // Velocity is not tested here since Wii drumkits only have a
                // single velocity axis shared by all pads

                buttonList.Clear();
            }
        });

        [Test]
        public void GetPadMaskReturnsCorrectPads() => CreateAndRun((drumkit) =>
        {
            var padMap = CreatePadMap(drumkit);
            var padsList = CreatePadsList();

            var state = CreateState();
            foreach (var pads in padsList)
            {
                foreach (var (pad, button) in padMap)
                {
                    SetPad(ref state, pad, (pads & pad) != 0 ? 1f : 0f);
                }

                AssertButtonsWithEventUpdate(drumkit, state, pads, drumkit.GetPadMask, drumkit.GetPadMask, AssertMask);
            }

            void AssertMask(FiveLanePad mask, FiveLanePad targetMask, Func<ButtonControl, bool> buttonPressed)
            {
                Assert.That(mask, Is.EqualTo(targetMask), "Pad mask is not correct!");

                Assert.That((mask & FiveLanePad.Kick) != 0,   Is.EqualTo(buttonPressed(drumkit.kick)), "Kick state is not correct!");
                Assert.That((mask & FiveLanePad.Red) != 0,    Is.EqualTo(buttonPressed(drumkit.redPad)), "Red pad state is not correct!");
                Assert.That((mask & FiveLanePad.Yellow) != 0, Is.EqualTo(buttonPressed(drumkit.yellowCymbal)), "Yellow cymbal state is not correct!");
                Assert.That((mask & FiveLanePad.Blue) != 0,   Is.EqualTo(buttonPressed(drumkit.bluePad)), "Blue pad state is not correct!");
                Assert.That((mask & FiveLanePad.Orange) != 0, Is.EqualTo(buttonPressed(drumkit.orangeCymbal)), "Orange cymbal state is not correct!");
                Assert.That((mask & FiveLanePad.Green) != 0,  Is.EqualTo(buttonPressed(drumkit.greenPad)), "Green pad state is not correct!");
            }
        });
    }
}