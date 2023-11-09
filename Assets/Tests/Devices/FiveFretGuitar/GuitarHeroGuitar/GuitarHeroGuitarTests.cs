using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public sealed class GuitarHeroGuitarTests : PlasticBandTestFixture<GuitarHeroGuitar>
    {
        [Test]
        public void TiltAndAccelerometerXAreEquivalent()
            => CreateAndRun(_TiltAndAccelerometerXAreEquivalent);

        [Test]
        public void GetTouchFretReturnsCorrectFrets()
            => CreateAndRun(_GetTouchFretReturnsCorrectFrets);

        [Test]
        public void GetTouchFretThrowsCorrectly()
            => CreateAndRun(_GetTouchFretThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _TiltAndAccelerometerXAreEquivalent(GuitarHeroGuitar guitar)
        {
            Assert.That(guitar.accelX, Is.EqualTo(guitar.tilt));
        }

        public static void _GetTouchFretReturnsCorrectFrets(GuitarHeroGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetTouchFret, guitar.GetTouchFret,
                guitar.touchGreen, guitar.touchRed, guitar.touchYellow, guitar.touchBlue, guitar.touchOrange);
        }

        public static void _GetTouchFretThrowsCorrectly(GuitarHeroGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetTouchFret, guitar.GetTouchFret);
        }
    }

    public abstract class GuitarHeroGuitarTests<TGuitar, TState> : FiveFretGuitarTests<TGuitar, TState>
        where TGuitar : GuitarHeroGuitar
        where TState : unmanaged, IInputStateTypeInfo
    {
        protected override AxisMode tiltMode => AxisMode.Signed;
        protected virtual bool supportsAccelerometers => true;

        // Handled by GuitarHeroSliderControlTests
        // protected abstract void SetTouchFrets(ref TState state, FiveFret fret);

        protected abstract void SetAccelerometerX(ref TState state, float value);
        protected abstract void SetAccelerometerY(ref TState state, float value);
        protected abstract void SetAccelerometerZ(ref TState state, float value);

        [Test]
        public void TiltAndAccelerometerXAreEquivalent()
            => CreateAndRun(GuitarHeroGuitarTests._TiltAndAccelerometerXAreEquivalent);

        [Test]
        public void GetTouchFretReturnsCorrectFrets()
            => CreateAndRun(GuitarHeroGuitarTests._GetTouchFretReturnsCorrectFrets);

        [Test]
        public void GetTouchFretThrowsCorrectly()
            => CreateAndRun(GuitarHeroGuitarTests._GetTouchFretThrowsCorrectly);

        [Test]
        [Ignore("TODO")]
        public void HandlesTouchFrets() => CreateAndRun((guitar) =>
        {
            Assert.Pass("This test is currently handled by GuitarHeroSliderControlTests.");
            // FiveFretGuitarTests.RecognizesFrets(guitar, handler,
            //     handler.SetTouchFrets, guitar.GetTouchFretMask, guitar.GetTouchFretMask,
            //     guitar.touchGreen, guitar.touchRed, guitar.touchYellow, guitar.touchBlue, guitar.touchOrange);
        });

        [Test]
        public void HandlesAccelerometers() => CreateAndRun((guitar) =>
        {
            // Santroller guitars don't support the accelerometers, so we need to skip on those
            if (!supportsAccelerometers)
                return;

            RecognizesSignedAxis(guitar, CreateState(), guitar.accelX, SetAccelerometerX);
            RecognizesSignedAxis(guitar, CreateState(), guitar.accelY, SetAccelerometerY);
            RecognizesSignedAxis(guitar, CreateState(), guitar.accelZ, SetAccelerometerZ);
        });
    }
}