using System;
using NUnit.Framework;
using PlasticBand.Devices;
using UnityEngine.InputSystem.Controls;

namespace PlasticBand.Tests.Devices
{
    public sealed class ProGuitarTests : PlasticBandTestFixture<ProGuitar>
    {
        [Test]
        public void GetStringFretReturnsCorrectFrets()
            => CreateAndRun(_GetStringFretReturnsCorrectFrets);

        [Test]
        public void GetStringFretThrowsCorrectly()
            => CreateAndRun(_GetStringFretThrowsCorrectly);

        [Test]
        public void GetStringStrumReturnsCorrectStrums()
            => CreateAndRun(_GetStringStrumReturnsCorrectStrums);

        [Test]
        public void GetStringStrumThrowsCorrectly()
            => CreateAndRun(_GetStringStrumThrowsCorrectly);

        [Test]
        public void GetEmulatedFretReturnsCorrectFrets()
            => CreateAndRun(_GetEmulatedFretReturnsCorrectFrets);

        [Test]
        public void GetEmulatedFretThrowsCorrectly()
            => CreateAndRun(_GetEmulatedFretThrowsCorrectly);

        [Test]
        public void GetEmulatedSoloFretReturnsCorrectFrets()
            => CreateAndRun(_GetEmulatedSoloFretReturnsCorrectFrets);

        [Test]
        public void GetEmulatedSoloFretThrowsCorrectly()
            => CreateAndRun(_GetEmulatedSoloFretThrowsCorrectly);

        // These must be named differently from the actual test methods, or else the input system test fixture
        // will fail to get the current method due to name ambiguity from reflection
        public static void _GetStringFretReturnsCorrectFrets(ProGuitar guitar)
        {
            Assert.That(guitar.GetStringFret(0), Is.EqualTo(guitar.fret1));
            Assert.That(guitar.GetStringFret(1), Is.EqualTo(guitar.fret2));
            Assert.That(guitar.GetStringFret(2), Is.EqualTo(guitar.fret3));
            Assert.That(guitar.GetStringFret(3), Is.EqualTo(guitar.fret4));
            Assert.That(guitar.GetStringFret(4), Is.EqualTo(guitar.fret5));
            Assert.That(guitar.GetStringFret(5), Is.EqualTo(guitar.fret6));
        }

        public static void _GetStringFretThrowsCorrectly(ProGuitar guitar)
        {
            for (int i = -5; i < ProGuitar.StringCount + 5; i++)
            {
                if (i < 0 || i >= ProGuitar.StringCount)
                    Assert.Throws<ArgumentOutOfRangeException>(()
                        => guitar.GetStringFret(i));
                else
                    Assert.DoesNotThrow(()
                        => guitar.GetStringFret(i));
            }
        }

        public static void _GetStringStrumReturnsCorrectStrums(ProGuitar guitar)
        {
            Assert.That(guitar.GetStringStrum(0), Is.EqualTo(guitar.strum1));
            Assert.That(guitar.GetStringStrum(1), Is.EqualTo(guitar.strum2));
            Assert.That(guitar.GetStringStrum(2), Is.EqualTo(guitar.strum3));
            Assert.That(guitar.GetStringStrum(3), Is.EqualTo(guitar.strum4));
            Assert.That(guitar.GetStringStrum(4), Is.EqualTo(guitar.strum5));
            Assert.That(guitar.GetStringStrum(5), Is.EqualTo(guitar.strum6));
        }

        public static void _GetStringStrumThrowsCorrectly(ProGuitar guitar)
        {
            for (int i = -5; i < ProGuitar.StringCount + 5; i++)
            {
                if (i < 0 || i >= ProGuitar.StringCount)
                    Assert.Throws<ArgumentOutOfRangeException>(()
                        => guitar.GetStringStrum(i));
                else
                    Assert.DoesNotThrow(()
                        => guitar.GetStringStrum(i));
            }
        }

        public static void _GetEmulatedFretReturnsCorrectFrets(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetEmulatedFret, guitar.GetEmulatedFret,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        }

        public static void _GetEmulatedFretThrowsCorrectly(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetEmulatedFret, guitar.GetEmulatedFret);
        }

        public static void _GetEmulatedSoloFretReturnsCorrectFrets(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretReturnsCorrectFrets(guitar.GetEmulatedSoloFret, guitar.GetEmulatedSoloFret,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        }

        public static void _GetEmulatedSoloFretThrowsCorrectly(ProGuitar guitar)
        {
            FiveFretGuitarTests._GetFretThrowsCorrectly(guitar.GetEmulatedSoloFret, guitar.GetEmulatedSoloFret);
        }
    }

    internal abstract class ProGuitarTests<TGuitar, TState> : FaceButtonDeviceTestFixture<TGuitar, TState>
        where TGuitar : ProGuitar
        where TState : unmanaged, IProGuitarState
    {
        protected override ButtonControl GetFaceButton(TGuitar guitar, FaceButton button)
        {
            switch (button)
            {
                case FaceButton.South: return guitar.buttonSouth;
                case FaceButton.East: return guitar.buttonEast;
                case FaceButton.West: return guitar.buttonWest;
                case FaceButton.North: return guitar.buttonNorth;
                default: throw new ArgumentException($"Invalid button value {button}!", nameof(button));
            }
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

        protected override void SetDpad(ref TState state, DpadDirection dpad)
        {
            state.dpadUp = dpad.IsUp();
            state.dpadDown = dpad.IsDown();
            state.dpadLeft = dpad.IsLeft();
            state.dpadRight = dpad.IsRight();
        }

        protected override void SetFaceButtons(ref TState state, FaceButton buttons)
        {
            state.south = (buttons & FaceButton.South) != 0;
            state.east = (buttons & FaceButton.East) != 0;
            state.west = (buttons & FaceButton.West) != 0;
            state.north = (buttons & FaceButton.North) != 0;
        }

        protected override void SetMenuButtons(ref TState state, MenuButton buttons)
        {
            state.start = (buttons & MenuButton.Start) != 0;
            state.select = (buttons & MenuButton.Select) != 0;
        }

        protected void SetStringFret(ref TState state, int index, byte fret)
        {
            // The fret values are only 5 bits in size
            if (fret > 0x1F)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(ProGuitar.StringCount)} ({ProGuitar.StringCount})!");

            switch (index)
            {
                case 0: state.fret1 = fret; break;
                case 1: state.fret2 = fret; break;
                case 2: state.fret3 = fret; break;
                case 3: state.fret4 = fret; break;
                case 4: state.fret5 = fret; break;
                case 5: state.fret6 = fret; break;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(ProGuitar.StringCount)} ({ProGuitar.StringCount})!");
            }
        }

        protected void SetStringStrum(ref TState state, int index, bool strummed)
        {
            switch (index)
            {
                case 0: state.velocity1 = SetVelocity(state.velocity1); break;
                case 1: state.velocity2 = SetVelocity(state.velocity2); break;
                case 2: state.velocity3 = SetVelocity(state.velocity3); break;
                case 3: state.velocity4 = SetVelocity(state.velocity4); break;
                case 4: state.velocity5 = SetVelocity(state.velocity5); break;
                case 5: state.velocity6 = SetVelocity(state.velocity6); break;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(ProGuitar.StringCount)} ({ProGuitar.StringCount})!");
            }

            byte SetVelocity(byte previousValue)
            {
                if (!strummed)
                    return 0;

                // The strum velocity values only use the bottom 7 bits,
                // the top bit is for the emulated 5-fret frets
                if (previousValue == 0x7F)
                    return 1; // Wrap around and skip 0
                else
                    return ++previousValue;
            }
        }

        protected void SetEmulatedFrets(ref TState state, FiveFret frets)
        {
            state.green = (frets & FiveFret.Green) != 0;
            state.red = (frets & FiveFret.Red) != 0;
            state.yellow = (frets & FiveFret.Yellow) != 0;
            state.blue = (frets & FiveFret.Blue) != 0;
            state.orange = (frets & FiveFret.Orange) != 0;
        }

        protected void SetEmulatedSoloFrets(ref TState state, FiveFret frets)
        {
            state.solo = frets != FiveFret.None;
            SetEmulatedFrets(ref state, frets);
        }

        [Test]
        public void GetStringFretReturnsCorrectFrets()
            => CreateAndRun(ProGuitarTests._GetStringFretReturnsCorrectFrets);

        [Test]
        public void GetStringFretThrowsCorrectly()
            => CreateAndRun(ProGuitarTests._GetStringFretThrowsCorrectly);

        [Test]
        public void GetStringStrumReturnsCorrectStrums()
            => CreateAndRun(ProGuitarTests._GetStringStrumReturnsCorrectStrums);

        [Test]
        public void GetStringStrumThrowsCorrectly()
            => CreateAndRun(ProGuitarTests._GetStringStrumThrowsCorrectly);

        [Test]
        public void GetEmulatedFretReturnsCorrectFrets()
            => CreateAndRun(ProGuitarTests._GetEmulatedFretReturnsCorrectFrets);

        [Test]
        public void GetEmulatedFretThrowsCorrectly()
            => CreateAndRun(ProGuitarTests._GetEmulatedFretThrowsCorrectly);

        [Test]
        public void GetEmulatedSoloFretReturnsCorrectFrets()
            => CreateAndRun(ProGuitarTests._GetEmulatedSoloFretReturnsCorrectFrets);

        [Test]
        public void GetEmulatedSoloFretThrowsCorrectly()
            => CreateAndRun(ProGuitarTests._GetEmulatedSoloFretThrowsCorrectly);

        [Test]
        public void RecognizesStringFrets() => CreateAndRun((guitar) =>
        {
            var state = CreateState();
            for (int index = 0; index < ProGuitar.StringCount; index++)
            {
                // TODO: Add fret count property to Pro Guitar
                for (byte fret = 0; fret <= 22; fret++)
                {
                    SetStringFret(ref state, index, fret);
                    AssertIntegerValue(guitar, state, fret, guitar.GetStringFret(index));
                }

                // Reset afterwards
                SetStringFret(ref state, index, 0);
            }
        });

        [Test]
        public void RecognizesStringStrums() => CreateAndRun((guitar) =>
        {
            var state = CreateState();
            for (int index = 0; index < ProGuitar.StringCount; index++)
            {
                var strum = guitar.GetStringStrum(index);
                SetStringStrum(ref state, index, true);
                AssertButtonPress(guitar, state, strum);

                // The state translation will automatically reset all strum inputs
                // at the start of the next update
                // SetStringStrum(ref state, index, false);
                AssertButtonPress(guitar, state);
            }
        });

        [Test]
        public void RecognizesEmulatedFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._RecognizesFrets(guitar, CreateState(), SetEmulatedFrets,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        });

        [Test]
        public void RecognizesEmulatedSoloFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._RecognizesFrets(guitar, CreateState(), SetEmulatedSoloFrets,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });

        [Test]
        public void GetEmulatedFretMaskReturnsCorrectFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._GetFretMaskReturnsCorrectFrets(guitar, CreateState(),
                SetEmulatedFrets, guitar.GetEmulatedFretMask, guitar.GetEmulatedFretMask,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        });

        [Test]
        public void GetEmulatedSoloFretMaskReturnsCorrectFrets() => CreateAndRun((guitar) =>
        {
            FiveFretGuitarTests._GetFretMaskReturnsCorrectFrets(guitar, CreateState(),
                SetEmulatedSoloFrets, guitar.GetEmulatedSoloFretMask, guitar.GetEmulatedSoloFretMask,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });

        [Test]
        public void EmulatedSoloFretsAreNotMirrored() => CreateAndRun((guitar) =>
        {
            var state = CreateState();

            // Only regular frets
            SetEmulatedFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
            SetEmulatedFrets(ref state, FiveFret.None);

            // Only solo frets
            SetEmulatedSoloFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);

            // Both regular and solo frets; solo frets should take precedence
            SetEmulatedFrets(ref state, FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange);
            AssertButtonPress(guitar, state,
                guitar.soloGreen, guitar.soloRed, guitar.soloYellow, guitar.soloBlue, guitar.soloOrange);
        });
    }

    // XInput Pro Guitars don't support tilt or the digital pedal,
    // so those tests are separated out to here so that they're not included in the list
    internal abstract class ProGuitarTests_TiltPedal<TGuitar, TState> : ProGuitarTests<TGuitar, TState>
        where TGuitar : ProGuitar
        where TState : unmanaged, IProGuitarState
    {
        protected void SetTilt(ref TState state, float value)
        {
            state.tilt = value >= 0.5f;
        }

        protected void SetDigitalPedal(ref TState state, bool pressed)
        {
            state.digitalPedal = pressed;
        }

        [Test]
        public void RecognizesTilt() => CreateAndRun((guitar) =>
        {
            RecognizesButtonAxis(guitar, CreateState(), guitar.tilt, SetTilt);
        });

        [Test]
        public void RecognizesDigitalPedal() => CreateAndRun((guitar) =>
        {
            RecognizesButton(guitar, CreateState(), guitar.digitalPedal, SetDigitalPedal);
        });
    }
}