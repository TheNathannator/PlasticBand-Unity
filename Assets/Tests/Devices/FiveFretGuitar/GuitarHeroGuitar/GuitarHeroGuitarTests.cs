using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Tests.Devices
{
    public abstract class GuitarHeroGuitarHandler<TState> : FiveFretGuitarHandler<TState>
        where TState : unmanaged, IInputStateTypeInfo
    {
        public new readonly GuitarHeroGuitar guitar;
        public readonly bool supportsAccelerometers;

        protected override InputDevice device => guitar;

        public GuitarHeroGuitarHandler(GuitarHeroGuitar guitar, AxisMode tiltMode = AxisMode.Signed,
            bool accelerometers = true)
            : base(guitar, tiltMode)
        {
            this.guitar = guitar;
            supportsAccelerometers = accelerometers;
        }

        // Handled by GuitarHeroSliderControlTests
        // public abstract void SetTouchFrets(ref TState state, FiveFret fret);

        public abstract void SetAccelerometerX(ref TState state, float value);
        public abstract void SetAccelerometerY(ref TState state, float value);
        public abstract void SetAccelerometerZ(ref TState state, float value);
    }

    public partial class GuitarHeroGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<GuitarHeroGuitar>(VerifyDevice);

            AssertDeviceCreation<XInputGuitarHeroGuitar>(VerifyDevice);
            AssertDeviceCreation<SantrollerXInputGuitarHeroGuitar>(VerifyDevice);

            AssertDeviceCreation<PS3GuitarHeroGuitar>(VerifyDevice);
            AssertDeviceCreation<PS3GuitarHeroGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<SantrollerHIDGuitarHeroGuitar>(VerifyDevice);
        }

        private static void VerifyDevice(GuitarHeroGuitar guitar)
        {
            FiveFretGuitarTests.VerifyDevice(guitar);

            // Ensure accelerometer X and tilt are equivalent
            Assert.That(guitar.accelX, Is.EqualTo(guitar.tilt));

            // Ensure GetTouchFret works correctly
            FiveFretGuitarTests.VerifyFrets(guitar.GetTouchFret, guitar.GetTouchFret,
                guitar.touchGreen, guitar.touchRed, guitar.touchYellow, guitar.touchBlue, guitar.touchOrange);
        }

        [Test]
        public void HandlesState()
        {
            // GuitarHeroGuitar has no concrete state layout, no testing is done for it
            // CreateAndRun<GuitarHeroGuitar>((guitar) => HandlesState(new GuitarHeroGuitarHandler(guitar)));

            CreateAndRun<XInputGuitarHeroGuitar>((guitar) => HandlesState(new XInputGuitarHeroGuitarHandler(guitar)));
            CreateAndRun<SantrollerXInputGuitarHeroGuitar>((guitar) => HandlesState(new SantrollerXInputGuitarHeroGuitarHandler(guitar)));

            CreateAndRun<PS3GuitarHeroGuitar>((guitar) => HandlesState(new PS3GuitarHeroGuitarHandler(guitar)));
            CreateAndRun<PS3GuitarHeroGuitar_ReportId>((guitar) => HandlesState(new PS3GuitarHeroGuitar_ReportIdHandler(guitar)));
            CreateAndRun<SantrollerHIDGuitarHeroGuitar>((guitar) => HandlesState(new SantrollerHIDGuitarHeroGuitarHandler(guitar)));
        }

        private static void HandlesState<TState>(GuitarHeroGuitarHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            FiveFretGuitarTests.HandlesState(handler);

            var guitar = handler.guitar;

            // Touch frets
            // Handled by GuitarHeroSliderControlTests
            // FiveFretGuitarTests.RecognizesFrets(guitar, handler,
            //     handler.SetTouchFrets, guitar.GetTouchFretMask, guitar.GetTouchFretMask,
            //     guitar.touchGreen, guitar.touchRed, guitar.touchYellow, guitar.touchBlue, guitar.touchOrange);

            // Accelerometers
            // Santroller guitars don't support the accelerometers, so we need to skip on those
            if (handler.supportsAccelerometers)
            {
                RecognizesSignedAxis(handler, guitar.accelX, handler.SetAccelerometerX);
                RecognizesSignedAxis(handler, guitar.accelY, handler.SetAccelerometerY);
                RecognizesSignedAxis(handler, guitar.accelZ, handler.SetAccelerometerZ);
            }
        }
    }
}