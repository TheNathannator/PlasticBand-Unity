using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

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

        public static void HandlesSoloFrets_Flags<TState>(InputDevice device, TState state,
            SetFiveFretAction<TState> setFrets,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange,
            ButtonControl sGreen, ButtonControl sRed, ButtonControl sYellow, ButtonControl sBlue, ButtonControl sOrange)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var fretList = new List<ButtonControl>(10);
            for (var frets = FiveFret.None; frets <= FiveFretGuitarTests.AllFrets; frets++)
            {
                setFrets(ref state, frets);

                if ((frets & FiveFret.Green) != 0)
                {
                    // The normal frets must be included since both 
                    // regular and solo frets share the same button bits
                    fretList.Add(green);
                    fretList.Add(sGreen);
                }

                if ((frets & FiveFret.Red) != 0)
                {
                    fretList.Add(red);
                    fretList.Add(sRed);
                }

                if ((frets & FiveFret.Yellow) != 0)
                {
                    fretList.Add(yellow);
                    fretList.Add(sYellow);
                }

                if ((frets & FiveFret.Blue) != 0)
                {
                    fretList.Add(blue);
                    fretList.Add(sBlue);
                }

                if ((frets & FiveFret.Orange) != 0)
                {
                    fretList.Add(orange);
                    fretList.Add(sOrange);
                }

                AssertButtonPress(device, state, fretList.ToArray());
                fretList.Clear();
            }
        }
    }

    public abstract class RockBandGuitarTests<TGuitar, TState> : FiveFretGuitarTests<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected override AxisMode tiltMode => AxisMode.Button;

        protected abstract void SetSoloFrets(ref TState state, FiveFret frets);

        protected abstract void SetPickupSwitch(ref TState state, int value);

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
        public void HandlesPickupSwitch() => CreateAndRun((guitar) =>
        {
            var state = CreateState();
            for (int notch = 0; notch < RockBandGuitar.PickupNotchCount; notch++)
            {
                SetPickupSwitch(ref state, notch);
                AssertIntegerValue(guitar, state, notch, guitar.pickupSwitch);
            }
        });
    }

    public abstract class RockBandGuitarTests_SoloFlag<TGuitar, TState> : RockBandGuitarTests<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IInputStateTypeInfo
    {
        [Test]
        public void HandlesSoloFrets() => CreateAndRun((guitar) =>
        {
            RockBandGuitarTests.HandlesSoloFrets_Flags(guitar, CreateState(), SetSoloFrets,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }

    public abstract class RockBandGuitarTests_SoloDistinct<TGuitar, TState> : RockBandGuitarTests<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IInputStateTypeInfo
    {
        [Test]
        public void HandlesSoloFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._RecognizesFrets(guitar, CreateState(), SetSoloFrets,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }
}