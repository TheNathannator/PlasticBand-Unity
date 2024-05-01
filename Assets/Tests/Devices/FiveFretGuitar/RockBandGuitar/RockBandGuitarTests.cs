using System;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    public sealed class RockBandGuitarTests : PlasticBandTestFixture<RockBandGuitar>
    {
        [Test]
        public void GetSoloFretReturnsCorrectFrets()
            => CreateAndRun(_GetSoloFretReturnsCorrectFrets);

        [Test]
        public void GetSoloFretThrowsCorrectly()
            => CreateAndRun(_GetSoloFretThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetSoloFretReturnsCorrectFrets(RockBandGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetSoloFret, guitar.GetSoloFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        }

        public static void _GetSoloFretThrowsCorrectly(RockBandGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetSoloFret, guitar.GetSoloFret);
        }
    }

    internal abstract class RockBandGuitarTests_Base<TGuitar, TState> : FiveFretGuitarTests<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IRockBandGuitarState_Base
    {
        // Common default, can be further overridden by the implementations
        protected override AxisMode tiltMode => AxisMode.Button;

        protected abstract void SetSoloFrets(ref TState state, FiveFret frets);

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

        protected override ButtonControl GetMenuButton(TGuitar guitar, MenuButton button)
        {
            switch (button)
            {
                case MenuButton.Start: return guitar.startButton;
                case MenuButton.Select: return guitar.selectButton;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
        }

        protected override DpadControl GetDpad(TGuitar guitar) => guitar.dpad;

        protected override void SetTilt(ref TState state, float value)
        {
            state.tilt = DeviceHandling.DenormalizeSByte(value);
        }

        protected override void SetWhammy(ref TState state, float value)
        {
            state.whammy = DeviceHandling.DenormalizeByteUnsigned(value);
        }

        [Test]
        public void GetSoloFretReturnsCorrectFrets()
            => CreateAndRun(RockBandGuitarTests._GetSoloFretReturnsCorrectFrets);

        [Test]
        public void GetSoloFretThrowsCorrectly()
            => CreateAndRun(RockBandGuitarTests._GetSoloFretThrowsCorrectly);

        [Test]
        public void GetSoloFretMaskReturnsCorrectFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._GetFretMaskReturnsCorrectFrets(guitar, CreateState(),
                SetSoloFrets, guitar.GetSoloFretMask, guitar.GetSoloFretMask,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });

        [Test]
        public void HandlesSoloFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._RecognizesFrets(guitar, CreateState(), SetSoloFrets,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });

        [Test]
        public void HandlesPickupSwitch() => CreateAndRun((guitar) =>
        {
            var state = CreateState();
            for (int notch = 0; notch < RockBandGuitar.PickupNotchCount; notch++)
            {
                state.pickupSwitch = notch;
                AssertIntegerValue(guitar, state, notch, guitar.pickupSwitch);
            }
        });
    }

    internal abstract class RockBandGuitarTests_Flags<TGuitar, TState> : RockBandGuitarTests_Base<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IRockBandGuitarState_Flags
    {
        protected override void SetFrets(ref TState state, FiveFret frets)
        {
            state.green = (frets & FiveFret.Green) != 0;
            state.red = (frets & FiveFret.Red) != 0;
            state.yellow = (frets & FiveFret.Yellow) != 0;
            state.blue = (frets & FiveFret.Blue) != 0;
            state.orange = (frets & FiveFret.Orange) != 0;
        }

        protected override void SetSoloFrets(ref TState state, FiveFret frets)
        {
            state.solo = frets != FiveFret.None;
            SetFrets(ref state, frets);
        }

        [Test]
        public void SoloFretsAreNotMirrored() => CreateAndRun((guitar) =>
        {
            var state = CreateState();

            // Only regular frets
            SetFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
            SetFrets(ref state, FiveFret.None);

            // Only solo frets
            SetSoloFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);

            // Both regular and solo frets; solo frets should take precedence
            SetFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }

    internal abstract class RockBandGuitarTests_Distinct<TGuitar, TState> : RockBandGuitarTests_Base<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IRockBandGuitarState_Distinct
    {
        protected override void SetFrets(ref TState state, FiveFret frets)
        {
            state.green = (frets & FiveFret.Green) != 0;
            state.red = (frets & FiveFret.Red) != 0;
            state.yellow = (frets & FiveFret.Yellow) != 0;
            state.blue = (frets & FiveFret.Blue) != 0;
            state.orange = (frets & FiveFret.Orange) != 0;
        }

        protected override void SetSoloFrets(ref TState state, FiveFret frets)
        {
            state.soloGreen = (frets & FiveFret.Green) != 0;
            state.soloRed = (frets & FiveFret.Red) != 0;
            state.soloYellow = (frets & FiveFret.Yellow) != 0;
            state.soloBlue = (frets & FiveFret.Blue) != 0;
            state.soloOrange = (frets & FiveFret.Orange) != 0;
        }
    }
}