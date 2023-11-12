using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Devices;
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
            _GetFretMaskReturnsCorrectFrets(guitar, CreateState(), SetSoloFrets, guitar.GetSoloFretMask, guitar.GetSoloFretMask,
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
            var state = CreateState();
            var fretList = new List<ButtonControl>(10);
            for (var frets = FiveFret.None; frets <= FiveFretGuitarTests.AllFrets; frets++)
            {
                SetSoloFrets(ref state, frets);

                if ((frets & FiveFret.Green) != 0)
                {
                    // The normal frets must be included since both 
                    // regular and solo frets share the same button bits
                    fretList.Add(guitar.greenFret);
                    fretList.Add(guitar.soloGreen);
                }

                if ((frets & FiveFret.Red) != 0)
                {
                    fretList.Add(guitar.redFret);
                    fretList.Add(guitar.soloRed);
                }

                if ((frets & FiveFret.Yellow) != 0)
                {
                    fretList.Add(guitar.yellowFret);
                    fretList.Add(guitar.soloYellow);
                }

                if ((frets & FiveFret.Blue) != 0)
                {
                    fretList.Add(guitar.blueFret);
                    fretList.Add(guitar.soloBlue);
                }

                if ((frets & FiveFret.Orange) != 0)
                {
                    fretList.Add(guitar.orangeFret);
                    fretList.Add(guitar.soloOrange);
                }

                AssertButtonPress(guitar, state, fretList.ToArray());
                fretList.Clear();
            }
        });
    }

    public abstract class RockBandGuitarTests_SoloDistinct<TGuitar, TState> : RockBandGuitarTests<TGuitar, TState>
        where TGuitar : RockBandGuitar
        where TState : unmanaged, IInputStateTypeInfo
    {
        [Test]
        public void HandlesSoloFrets() => CreateAndRun((guitar) =>
        {
            _RecognizesFrets(guitar, CreateState(), SetSoloFrets,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }
}