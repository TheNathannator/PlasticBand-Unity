using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Devices
{
    public abstract class RockBandGuitarHandler<TState> : FiveFretGuitarHandler<TState>
        where TState : unmanaged, IInputStateTypeInfo
    {
        public new readonly RockBandGuitar guitar;

        protected override InputDevice device => guitar;

        public RockBandGuitarHandler(RockBandGuitar guitar, AxisMode tiltMode = AxisMode.Button)
            : base(guitar, tiltMode)
        {
            this.guitar = guitar;
        }

        public abstract void SetSoloFrets(ref TState state, FiveFret frets);

        // Not handled by RockBandPickupSwitchControlTests, 
        // as not all RB guitar layouts use RockBandPickupSwitchControl
        public abstract void SetPickupSwitch(ref TState state, int value);
    }

    public partial class RockBandGuitarTests : PlasticBandTestFixture
    {
        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<RockBandGuitar>(VerifyDevice);

            AssertDeviceCreation<XInputRockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<SantrollerXInputRockBandGuitar>(VerifyDevice);

            AssertDeviceCreation<PS3RockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<PS3RockBandGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<WiiRockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<WiiRockBandGuitar_ReportId>(VerifyDevice);
            AssertDeviceCreation<PS4RockBandGuitar>(VerifyDevice);
            AssertDeviceCreation<PS4RockBandGuitar_NoReportId>(VerifyDevice);
            AssertDeviceCreation<SantrollerHIDRockBandGuitar>(VerifyDevice);
        }

        private static void VerifyDevice(RockBandGuitar guitar)
        {
            FiveFretGuitarTests.VerifyDevice(guitar);

            // Ensure GetSoloFret works correctly
            FiveFretGuitarTests.VerifyFrets(guitar.GetSoloFret, guitar.GetSoloFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        }

        [Test]
        public void HandlesState()
        {
            // RockBandGuitar has no concrete state layout, no testing is done for it
            // CreateAndRun<RockBandGuitar>((guitar) => HandlesState(new RockBandGuitarHandler(guitar)));

            CreateAndRun<XInputRockBandGuitar>((guitar) => HandlesState(new XInputRockBandGuitarHandler(guitar)));
            CreateAndRun<SantrollerXInputRockBandGuitar>((guitar) => HandlesState(new XInputRockBandGuitarHandler(guitar)));

            CreateAndRun<PS3RockBandGuitar>((guitar) => HandlesState(new PS3RockBandGuitarHandler(guitar)));
            CreateAndRun<PS3RockBandGuitar_ReportId>((guitar) => HandlesState(new PS3RockBandGuitar_ReportIdHandler(guitar)));
            CreateAndRun<WiiRockBandGuitar>((guitar) => HandlesState(new WiiRockBandGuitarHandler(guitar)));
            CreateAndRun<WiiRockBandGuitar_ReportId>((guitar) => HandlesState(new WiiRockBandGuitar_ReportIdHandler(guitar)));
            CreateAndRun<PS4RockBandGuitar>((guitar) => HandlesState(new PS4RockBandGuitarHandler(guitar)));
            CreateAndRun<PS4RockBandGuitar_NoReportId>((guitar) => HandlesState(new PS4RockBandGuitar_NoReportIdHandler(guitar)));
            CreateAndRun<SantrollerHIDRockBandGuitar>((guitar) => HandlesState(new SantrollerHIDRockBandGuitarHandler(guitar)));
        }

        private static void HandlesState<TState>(RockBandGuitarHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            FiveFretGuitarTests.HandlesState(handler);

            var guitar = handler.guitar;

            // Solo frets
            // TODO: Does not pass due to regular and solo frets registering at the same time on some guitars
            // FiveFretGuitarTests.RecognizesFrets(guitar, handler,
            //     handler.SetSoloFrets, guitar.GetSoloFretMask, guitar.GetSoloFretMask,
            //     guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);

            // Frets excluding solo frets
            RecognizesFretsExcludingSolo(handler);

            // Pickup switch
            // Not handled by RockBandPickupSwitchControlTests, 
            // as not all RB guitar layouts use RockBandPickupSwitchControl
            var state = handler.CreateState();
            for (int notch = 0; notch < RockBandGuitar.PickupNotchCount; notch++)
            {
                handler.SetPickupSwitch(ref state, notch);
                AssertIntegerValue(guitar, state, notch, guitar.pickupSwitch);
            }
        }

        private static void RecognizesFretsExcludingSolo<TState>(RockBandGuitarHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var guitar = handler.guitar;

            for (var frets = FiveFret.None; frets <= FiveFretGuitarTests.AllFrets; frets++)
            {
                var state = handler.CreateState();

                // Both same, cancels out each time
                handler.SetFrets(ref state, frets);
                handler.SetSoloFrets(ref state, frets);

                InputSystem.onEvent.CallOnce((eventPtr) => Assert.That(guitar.GetFretMaskExcludingSolo(eventPtr), Is.EqualTo(FiveFret.None)));
                InputSystem.QueueStateEvent(guitar, state);
                InputSystem.Update();
                Assert.That(guitar.GetFretMaskExcludingSolo(), Is.EqualTo(FiveFret.None));

                // Unfortunately, not all guitars report the solo frets fully separately
                // so we can't reliably do these tests
                // // Regular varies, solo constant
                // const FiveFret testMask = FiveFret.Green | FiveFret.Yellow | FiveFret.Orange;
                // handler.SetFret(ref state, frets);
                // handler.SetSoloFret(ref state, testMask);

                // InputSystem.onEvent.CallOnce((eventPtr) => Assert.That(guitar.GetFretMaskExcludingSolo(eventPtr), Is.EqualTo(frets & ~testMask)));
                // InputSystem.QueueStateEvent(guitar, state);
                // InputSystem.Update();
                // Assert.That(guitar.GetFretMaskExcludingSolo(), Is.EqualTo(frets & ~testMask));

                // // Regular constant, solo varies
                // handler.SetFret(ref state, testMask);
                // handler.SetSoloFret(ref state, frets);

                // InputSystem.onEvent.CallOnce((eventPtr) => Assert.That(guitar.GetFretMaskExcludingSolo(eventPtr), Is.EqualTo(testMask & ~frets)));
                // InputSystem.QueueStateEvent(guitar, state);
                // InputSystem.Update();
                // Assert.That(guitar.GetFretMaskExcludingSolo(), Is.EqualTo(testMask & ~frets));
            }
        }
    }
}