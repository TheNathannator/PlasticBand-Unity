using NUnit.Framework;
using PlasticBand.Devices;

namespace PlasticBand.Tests.Devices
{
    internal abstract class RiffmasterGuitarTests<TGuitar, TState> : RockBandGuitarTests_Distinct<TGuitar, TState>
        where TGuitar : RiffmasterGuitar
        where TState : unmanaged, IRiffmasterGuitarState
    {
        protected override AxisMode tiltMode => AxisMode.Unsigned;

        protected void SetJoystick(ref TState state, float x, float y)
        {
            state.joystickX = DeviceHandling.DenormalizeSByte(x);
            state.joystickY = DeviceHandling.DenormalizeSByte(y);
        }

        protected void SetJoystickClick(ref TState state, bool value)
        {
            state.joystickClick = value;
        }

        protected void SetP1(ref TState state, bool value)
        {
            state.p1 = value;
        }

        [Test]
        [Ignore("Currently takes minutes to execute for some forsaken reason")]
        public void HandlesJoystick() => CreateAndRun((guitar) =>
        {
            RecognizesStick(guitar, CreateState(), guitar.joystick, SetJoystick);
        });

        [Test]
        public void HandlesJoystickClick() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), guitar.joystickClick, SetJoystickClick);
        });

        [Test]
        public void SoloFretsMaskJoystickClick() => CreateAndRun((guitar) =>
        {
            var state = CreateState();

            // Joystick click only
            SetJoystickClick(ref state, true);
            AssertButtonPress(guitar, state, guitar.joystickClick);
            SetJoystickClick(ref state, false);

            // Solo frets only
            SetSoloFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);

            // Both; solo frets should take precedence
            SetJoystickClick(ref state, true);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }
}