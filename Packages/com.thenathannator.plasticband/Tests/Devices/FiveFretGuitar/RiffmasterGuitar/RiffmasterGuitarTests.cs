using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    internal abstract class RiffmasterGuitarTests<TGuitar, TState> : RockBandGuitarTests_Distinct<TGuitar, TState>
        where TGuitar : RiffmasterGuitar
        where TState : unmanaged, IRiffmasterGuitarState
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected virtual bool hasRightStick => true;
        protected virtual bool hasP1 => true;

        protected void SetLeftStick(ref TState state, float x, float y)
        {
            state.leftStickX = DeviceHandling.DenormalizeSByte(x);
            state.leftStickY = DeviceHandling.DenormalizeSByte(y);
        }

        protected void SetLeftStickPress(ref TState state, bool value)
        {
            state.leftStickPress = value;
        }

        protected void SetRightStick(ref TState state, float x, float y)
        {
            state.rightStickX = DeviceHandling.DenormalizeSByte(x);
            state.rightStickY = DeviceHandling.DenormalizeSByte(y);
        }

        protected void SetRightStickPress(ref TState state, bool value)
        {
            state.rightStickPress = value;
        }

        protected void SetP1(ref TState state, bool value)
        {
            state.p1 = value;
        }

        [Test]
        public void HandlesLeftStick() => CreateAndRun((guitar) =>
        {
            RecognizesStick(guitar, CreateState(), guitar.leftStick, SetLeftStick);
        });

        [Test]
        public void HandlesLeftStickPress() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), guitar.leftStickPress, SetLeftStickPress);
        });

        [Test]
        public void HandlesRightStick() => CreateAndRun((guitar) =>
        {
            if (!hasRightStick)
            {
                Assert.Pass("No right stick present on this guitar");
                return;
            }

            RecognizesStick(guitar, CreateState(), guitar.rightStick, SetRightStick);
        });

        [Test]
        public void HandlesRightStickPress() => CreateAndRun((guitar) =>
        {
            if (!hasRightStick)
            {
                Assert.Pass("No right stick present on this guitar");
                return;
            }

            RecognizesButton(guitar, CreateState(), guitar.rightStickPress, SetRightStickPress);
        });

        [Test]
        public void HandlesP1() => CreateAndRun((guitar) =>
        {
            var p1Button = guitar.TryGetChildControl<ButtonControl>("p1Button");
            if (p1Button == null || !hasP1)
            {
                Assert.Pass("No P1 button present on this guitar");
                return;
            }

            RecognizesButton(guitar, CreateState(), p1Button, SetP1);
        });

        [Test]
        public void SoloFretsMaskLeftStickPress() => CreateAndRun((guitar) =>
        {
            var state = CreateState();

            // Left stick click only
            SetLeftStickPress(ref state, true);
            AssertButtonPress(guitar, state, guitar.leftStickPress);
            SetLeftStickPress(ref state, false);

            // Solo frets only
            SetSoloFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);

            // Both; solo frets should take precedence
            SetLeftStickPress(ref state, true);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }
}