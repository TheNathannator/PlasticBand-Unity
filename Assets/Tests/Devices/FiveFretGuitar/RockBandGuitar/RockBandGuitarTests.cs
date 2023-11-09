using NUnit.Framework;
using PlasticBand.Devices;
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

        // Not handled by RockBandPickupSwitchControlTests, 
        // as not all RB guitar layouts use RockBandPickupSwitchControl
        protected abstract void SetPickupSwitch(ref TState state, int value);

        [Test]
        public void GetSoloFretReturnsCorrectFrets()
            => CreateAndRun(RockBandGuitarTests._GetSoloFretReturnsCorrectFrets);

        [Test]
        public void GetSoloFretThrowsCorrectly()
            => CreateAndRun(RockBandGuitarTests._GetSoloFretThrowsCorrectly);

        [Test]
        [Ignore("Does not work correctly due to regular and solo frets registering at the same time on some guitars")]
        public void HandlesSoloFrets() => CreateAndRun((guitar) =>
        {
            _RecognizesFrets(guitar, CreateState(), SetSoloFrets,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });

        [Test]
        // [Ignore("Does not work correctly due to regular and solo frets registering at the same time on some guitars")]
        public void GetSoloFretMaskReturnsCorrectFrets() => CreateAndRun((guitar) =>
        {
            _GetFretMaskReturnsCorrectFrets(guitar, CreateState(), SetSoloFrets, guitar.GetSoloFretMask, guitar.GetSoloFretMask,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });

        [Test]
        public void HandlesPickupSwitch() => CreateAndRun((guitar) =>
        {
            // Not handled by RockBandPickupSwitchControlTests, 
            // as not all RB guitar layouts use RockBandPickupSwitchControl
            var state = CreateState();
            for (int notch = 0; notch < RockBandGuitar.PickupNotchCount; notch++)
            {
                SetPickupSwitch(ref state, notch);
                AssertIntegerValue(guitar, state, notch, guitar.pickupSwitch);
            }
        });
    }
}