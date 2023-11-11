using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;

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

    internal abstract class FourLaneDrumkitTests<TDrumkit, TState> : FaceButtonDeviceTestFixture<TDrumkit, TState>
        where TDrumkit : FourLaneDrumkit
        where TState : unmanaged, IFourLaneDrumkitState_Base
    {
        protected abstract void SetPad(ref TState state, FourLanePad pad, float velocity);

        protected override void SetDpad(ref TState state, DpadDirection dpad)
        {
            state.dpadUp = dpad.IsUp();
            state.dpadDown = dpad.IsDown();
            state.dpadLeft = dpad.IsLeft();
            state.dpadRight = dpad.IsRight();
        }

        protected override void SetMenuButtons(ref TState state, MenuButton buttons)
        {
            state.start = (buttons & MenuButton.Start) != 0;
            state.select = (buttons & MenuButton.Select) != 0;
        }

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

        [Test]
        public void GetPadReturnsCorrectPads()
            => CreateAndRun(FourLaneDrumkitTests._GetPadReturnsCorrectPads);

        [Test]
        public void GetPadThrowsCorrectly()
            => CreateAndRun(FourLaneDrumkitTests._GetPadThrowsCorrectly);

        protected override void InitializeDevice(TDrumkit drumkit)
        {
            // Prime the drumkit so that flag-based kits know they have flags
            // Without this, the face buttons test will fail since they get interpreted as pads
            var state = CreateState();

            SetPad(ref state, FourLanePad.RedPad, 1f);
            InputSystem.QueueStateEvent(drumkit, state);
            InputSystem.Update();

            SetPad(ref state, FourLanePad.RedPad, 0f);
            InputSystem.QueueStateEvent(drumkit, state);
            InputSystem.Update();
        }

        private List<(FourLanePad pad, ButtonControl button)> CreatePadMap(TDrumkit drumkit)
        {
            return new List<(FourLanePad, ButtonControl)>()
            {
                (FourLanePad.Kick1,        drumkit.kick1),
                (FourLanePad.Kick2,        drumkit.kick2),

                (FourLanePad.RedPad,       drumkit.redPad),
                (FourLanePad.YellowPad,    drumkit.yellowPad),
                (FourLanePad.BluePad,      drumkit.bluePad),
                (FourLanePad.GreenPad,     drumkit.greenPad),

                (FourLanePad.YellowCymbal, drumkit.yellowCymbal),
                (FourLanePad.BlueCymbal,   drumkit.blueCymbal),
                (FourLanePad.GreenCymbal,  drumkit.greenCymbal),
            };
        }

        private HashSet<FourLanePad> CreatePadsList()
        {
            var pads = new FourLanePad[]
            {
                // Kicks are handled specially
                // FourLanePad.Kick1,
                // FourLanePad.Kick2,

                FourLanePad.RedPad,
                FourLanePad.YellowPad,
                FourLanePad.BluePad,
                FourLanePad.GreenPad,

                FourLanePad.YellowCymbal,
                FourLanePad.BlueCymbal,
                FourLanePad.GreenCymbal,
            };

            var padsList = new HashSet<FourLanePad>()
            {
                FourLanePad.Kick1,
                FourLanePad.Kick2,
                FourLanePad.Kick1 | FourLanePad.Kick2
            };

            foreach (var pad1 in pads)
            {
                foreach (var pad2 in pads)
                {
                    // Allow even if they're the same, so that single-pad entries are added as well
                    // if (pad1 == pad2)
                    //     continue;

                    padsList.Add(pad1 | pad2);
                    padsList.Add(FourLanePad.Kick1 | pad1 | pad2);
                    padsList.Add(FourLanePad.Kick2 | pad1 | pad2);
                    padsList.Add(FourLanePad.Kick1 | FourLanePad.Kick2 | pad1 | pad2);
                }
            }

            // Test resetting to none at the end
            padsList.Add(FourLanePad.None);

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
                // Kicks need to be handled specially since they don't support velocity
                if (pad == FourLanePad.Kick1 || pad == FourLanePad.Kick2)
                {
                    SetPad(ref state, pad, 1f);
                    AssertButtonPress(drumkit, state, button);

                    SetPad(ref state, pad, 0f);
                    AssertButtonPress(drumkit, state);

                    continue;
                }

                foreach (float velocity in velocityList)
                {
                    SetPad(ref state, pad, velocity);

                    if (velocity > 0f)
                        AssertButtonPress(drumkit, state, button);
                    else
                        AssertButtonPress(drumkit, state);

                    AssertAxisValue(velocity, 1 / 100f, button);
                }
            }
        });

        [Test]
        public void RecognizesPadChords() => CreateAndRun((drumkit) =>
        {
            var padMap = CreatePadMap(drumkit);
            var padsList = CreatePadsList();

            var state = CreateState();
            var buttonList = new List<ButtonControl>(4);
            foreach (var pads in padsList)
            {
                // Set pads
                foreach (var (pad, button) in padMap)
                {
                    if ((pads & pad) != 0)
                    {
                        SetPad(ref state, pad, 1f);
                        buttonList.Add(button);
                    }
                }

                AssertButtonPress(drumkit, state, buttonList.ToArray());

                // Velocity is not tested here since RB drumkits don't support velocity for kicks,
                // and flag-based kits (excluding Santroller kits) have shared velocity axes on each color
                // (so yellow pad and yellow cymbal don't register velocity independently)

                // Clear pads
                buttonList.Clear();
                foreach (var (pad, button) in padMap)
                {
                    if ((pads & pad) != 0)
                        SetPad(ref state, pad, 0f);
                }

                AssertButtonPress(drumkit, state);
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
                    if ((pads & pad) != 0)
                        SetPad(ref state, pad, 1f);
                }

                AssertButtonsWithEventUpdate(drumkit, state, pads, drumkit.GetPadMask, drumkit.GetPadMask, AssertMask);

                foreach (var (pad, button) in padMap)
                {
                    if ((pads & pad) != 0)
                        SetPad(ref state, pad, 0f);
                }

                AssertButtonsWithEventUpdate(drumkit, state, FourLanePad.None, drumkit.GetPadMask, drumkit.GetPadMask, AssertMask);
            }

            void AssertMask(FourLanePad mask, FourLanePad targetMask, Func<ButtonControl, bool> buttonPressed)
            {
                Assert.That(mask, Is.EqualTo(targetMask), "Pad mask is not correct!");

                Assert.That((mask & FourLanePad.Kick1) != 0, Is.EqualTo(buttonPressed(drumkit.kick1)), "Kick 1 state is not correct!");
                Assert.That((mask & FourLanePad.Kick2) != 0, Is.EqualTo(buttonPressed(drumkit.kick2)), "Kick 2 state is not correct!");

                Assert.That((mask & FourLanePad.RedPad) != 0,    Is.EqualTo(buttonPressed(drumkit.redPad)), "Red pad state is not correct!");
                Assert.That((mask & FourLanePad.YellowPad) != 0, Is.EqualTo(buttonPressed(drumkit.yellowPad)), "Yellow pad state is not correct!");
                Assert.That((mask & FourLanePad.BluePad) != 0,   Is.EqualTo(buttonPressed(drumkit.bluePad)), "Blue pad state is not correct!");
                Assert.That((mask & FourLanePad.GreenPad) != 0,  Is.EqualTo(buttonPressed(drumkit.greenPad)), "Green pad state is not correct!");

                Assert.That((mask & FourLanePad.YellowCymbal) != 0, Is.EqualTo(buttonPressed(drumkit.yellowCymbal)), "Yellow cymbal state is not correct!");
                Assert.That((mask & FourLanePad.BlueCymbal) != 0,   Is.EqualTo(buttonPressed(drumkit.blueCymbal)), "Blue cymbal state is not correct!");
                Assert.That((mask & FourLanePad.GreenCymbal) != 0,  Is.EqualTo(buttonPressed(drumkit.greenCymbal)), "Green cymbal state is not correct!");
            }
        });
    }

    internal abstract class FourLaneDrumkitTests_Flags<TDrumkit, TState> : FourLaneDrumkitTests<TDrumkit, TState>
        where TDrumkit : FourLaneDrumkit
        where TState : unmanaged, IFourLaneDrumkitState_Flags
    {
        private static bool s_dummyHasFlags = true;

        protected override void SetFaceButtons(ref TState state, FaceButton buttons)
        {
            state.green_south = (buttons & FaceButton.South) != 0;
            state.red_east = (buttons & FaceButton.East) != 0;
            state.blue_west = (buttons & FaceButton.West) != 0;
            state.yellow_north = (buttons & FaceButton.North) != 0;
        }

        protected override void SetPad(ref TState state, FourLanePad pad, float velocity)
        {
            bool hit = velocity > 0f;
            byte rawVelocity = DeviceHandling.DenormalizeByteUnsigned(velocity);

            int colorCount = 0;
            if (state.red_east) colorCount++;
            if (state.yellow_north) colorCount++;
            if (state.blue_west) colorCount++;
            if (state.green_south) colorCount++;

            switch (pad)
            {
                case FourLanePad.Kick1:
                    state.kick1 = hit;
                    break;

                case FourLanePad.Kick2:
                    VerifyPadCount(ref state);
                    state.kick2 = hit;
                    break;

                case FourLanePad.RedPad:
                    VerifyPadCount(ref state);
                    state.red_east = hit;
                    state.redPadVelocity = rawVelocity;
                    SetPad(ref state);
                    break;

                case FourLanePad.YellowPad:
                    VerifyPadCount(ref state);
                    state.yellow_north = hit;
                    state.yellowPadVelocity = rawVelocity;
                    SetPad(ref state);
                    break;

                case FourLanePad.BluePad:
                    VerifyPadCount(ref state);
                    state.blue_west = hit;
                    state.bluePadVelocity = rawVelocity;
                    SetPad(ref state);
                    break;

                case FourLanePad.GreenPad:
                    VerifyPadCount(ref state);
                    state.green_south = hit;
                    state.greenPadVelocity = rawVelocity;
                    SetPad(ref state);
                    break;

                case FourLanePad.YellowCymbal:
                    VerifyPadCount(ref state);
                    state.yellow_north = hit;
                    state.yellowCymbalVelocity = rawVelocity;
                    SetCymbal(ref state);

                    // The drumkit will only set the dpad to either up or down, not both at the same time
                    state.dpadUp = hit && !state.dpadDown;
                    break;

                case FourLanePad.BlueCymbal:
                    VerifyPadCount(ref state);
                    state.blue_west = hit;
                    state.blueCymbalVelocity = rawVelocity;
                    SetCymbal(ref state);

                    state.dpadDown = hit && !state.dpadUp;
                    break;

                case FourLanePad.GreenCymbal:
                    VerifyPadCount(ref state);
                    state.green_south = hit;
                    state.greenCymbalVelocity = rawVelocity;
                    SetCymbal(ref state);

                    // Green cymbal does not set the d-pad state
                    // Unsure if it prevents the d-pad from being set if it happened to be hit first
                    break;

                default:
                    throw new ArgumentException($"Invalid pad value {pad}!", nameof(pad));
            }

            void VerifyPadCount(ref TState state2)
            {
                if (hit && (colorCount >= 2 || (colorCount == 1 && state2.pad && state2.cymbal)))
                    throw new InvalidOperationException($"Cannot set more than two pads at once!\nAttempted to set {pad}, already have {TranslatingFourLaneDrumkit.TranslatePads(ref state2, ref s_dummyHasFlags)} set");
            }

            void SetPad(ref TState state2)
            {
                // Don't remove if there are still pads active
                // We don't care about the number of pads since only two should ever be active at once
                if (!hit && colorCount > 1 && !state2.cymbal)
                    return;

                state2.pad = hit;
            }

            void SetCymbal(ref TState state2)
            {
                // Same as above
                if (!hit && colorCount > 1 && !state2.pad)
                    return;

                state2.cymbal = hit;
            }
        }
    }

    internal abstract class FourLaneDrumkitTests_Distinct<TDrumkit, TState> : FourLaneDrumkitTests<TDrumkit, TState>
        where TDrumkit : FourLaneDrumkit
        where TState : unmanaged, IFourLaneDrumkitState_Distinct
    {
        protected override void SetFaceButtons(ref TState state, FaceButton buttons)
        {
            state.south = (buttons & FaceButton.South) != 0;
            state.east = (buttons & FaceButton.East) != 0;
            state.west = (buttons & FaceButton.West) != 0;
            state.north = (buttons & FaceButton.North) != 0;
        }

        protected override void SetPad(ref TState state, FourLanePad pad, float velocity)
        {
            byte rawVelocity = DeviceHandling.DenormalizeByteUnsigned(velocity);
            switch (pad)
            {
                case FourLanePad.Kick1:
                    state.kick1 = rawVelocity != 0;
                    break;

                case FourLanePad.Kick2:
                    state.kick2 = rawVelocity != 0;
                    break;

                case FourLanePad.RedPad:
                    state.redPad = rawVelocity;
                    break;

                case FourLanePad.YellowPad:
                    state.yellowPad = rawVelocity;
                    break;

                case FourLanePad.BluePad:
                    state.bluePad = rawVelocity;
                    break;

                case FourLanePad.GreenPad:
                    state.greenPad = rawVelocity;
                    break;

                case FourLanePad.YellowCymbal:
                    state.yellowCymbal = rawVelocity;
                    break;

                case FourLanePad.BlueCymbal:
                    state.blueCymbal = rawVelocity;
                    break;

                case FourLanePad.GreenCymbal:
                    state.greenCymbal = rawVelocity;
                    break;

                default:
                    throw new ArgumentException($"Invalid pad value {pad}!", nameof(pad));
            }
        }
    }
}