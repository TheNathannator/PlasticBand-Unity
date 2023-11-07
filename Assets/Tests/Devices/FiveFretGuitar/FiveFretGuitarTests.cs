using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlasticBand.Controls;
using PlasticBand.Devices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.Devices
{
    public abstract class FiveFretGuitarHandler<TState> : IDeviceHandler<TState>
        where TState : unmanaged, IInputStateTypeInfo
    {
        public readonly FiveFretGuitar guitar;
        public readonly AxisMode tiltMode;

        protected virtual InputDevice device => guitar;
        InputDevice IDeviceHandler<TState>.device => device;

        public DpadControl dpad => guitar.dpad;

        ButtonControl IDeviceHandler<TState>.southButton => null;
        ButtonControl IDeviceHandler<TState>.eastButton => null;
        ButtonControl IDeviceHandler<TState>.westButton => null;
        ButtonControl IDeviceHandler<TState>.northButton => null;

        public ButtonControl startButton => guitar.startButton;
        public ButtonControl selectButton => guitar.selectButton;

        public FiveFretGuitarHandler(FiveFretGuitar guitar, AxisMode tiltMode = AxisMode.Signed)
        {
            this.guitar = guitar;
            this.tiltMode = tiltMode;
        }

        public abstract TState CreateState();
        public abstract void SetDpad(ref TState state, DpadDirection dpad);
        public abstract void SetFaceButtons(ref TState state, FaceButton buttons);

        public abstract void SetFrets(ref TState state, FiveFret frets);
        public abstract void SetWhammy(ref TState state, float value);
        public abstract void SetTilt(ref TState state, float value);
    }

    public class FiveFretGuitarTests : PlasticBandTestFixture
    {
        public delegate void SetFiveFretAction<TState>(ref TState state, FiveFret fret)
            where TState : unmanaged, IInputStateTypeInfo;

        public const FiveFret AllFrets = FiveFret.Green | FiveFret.Red | FiveFret.Yellow | FiveFret.Blue | FiveFret.Orange;

        [Test]
        public void CanCreate()
        {
            AssertDeviceCreation<FiveFretGuitar>(VerifyDevice);
        }

        public static void VerifyDevice(FiveFretGuitar guitar)
        {
            // Ensure strum and d-pad up/down are equivalent
            Assert.That(guitar.strumUp, Is.EqualTo(guitar.dpad.up));
            Assert.That(guitar.strumDown, Is.EqualTo(guitar.dpad.down));

            // Ensure GetFret works correctly
            VerifyFrets(guitar.GetFret, guitar.GetFret,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);
        }

        public static void VerifyFrets(Func<int, ButtonControl> getByIndex, Func<FiveFret, ButtonControl> getByEnum,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
        {
            // Ensure the correct controls are returned
            Assert.That(getByIndex(0), Is.EqualTo(green));
            Assert.That(getByIndex(1), Is.EqualTo(red));
            Assert.That(getByIndex(2), Is.EqualTo(yellow));
            Assert.That(getByIndex(3), Is.EqualTo(blue));
            Assert.That(getByIndex(4), Is.EqualTo(orange));

            Assert.That(getByEnum(FiveFret.Green), Is.EqualTo(green));
            Assert.That(getByEnum(FiveFret.Red), Is.EqualTo(red));
            Assert.That(getByEnum(FiveFret.Yellow), Is.EqualTo(yellow));
            Assert.That(getByEnum(FiveFret.Blue), Is.EqualTo(blue));
            Assert.That(getByEnum(FiveFret.Orange), Is.EqualTo(orange));

            // Ensure correct throw behavior
            for (int i = -5; i < FiveFretGuitar.FretCount + 5; i++)
            {
                if (i < 0 || i >= FiveFretGuitar.FretCount)
                    Assert.Throws<ArgumentOutOfRangeException>(() => getByIndex(i));
                else
                    Assert.DoesNotThrow(() => getByIndex(i));
            }

            for (var frets = FiveFret.None; frets <= AllFrets; frets++)
            {
                int fretCount = 0;
                if ((frets & FiveFret.Green) != 0) fretCount++;
                if ((frets & FiveFret.Red) != 0) fretCount++;
                if ((frets & FiveFret.Yellow) != 0) fretCount++;
                if ((frets & FiveFret.Blue) != 0) fretCount++;
                if ((frets & FiveFret.Orange) != 0) fretCount++;

                if (fretCount != 1)
                    Assert.Throws<ArgumentException>(() => getByEnum(frets));
                else
                    Assert.DoesNotThrow(() => getByEnum(frets));
            }
        }

        // FiveFretGuitar has no concrete state layout, no testing is done for it
        // [Test]
        // public void HandlesState()
        // {
        //     CreateAndRun<FiveFretGuitar>((guitar) => HandlesState(new FiveFretGuitarHandler(guitar)));
        // }

        public static void HandlesState<TState>(FiveFretGuitarHandler<TState> handler)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var guitar = handler.guitar;

            RecognizesCommonControls(handler);
            RecognizesFrets(guitar, handler, handler.SetFrets, guitar.GetFretMask, guitar.GetFretMask,
                guitar.greenFret, guitar.redFret, guitar.yellowFret, guitar.blueFret, guitar.orangeFret);

            RecognizesUnsignedAxis(handler, guitar.whammy, handler.SetWhammy);
            RecognizesAxis(handler, guitar.tilt, handler.tiltMode, handler.SetTilt);
        }

        // This method is re-used for different sets of frets, including solo frets and Pro Guitar emulated frets,
        // so it must take delegates and parameters for each fret control
        public static void RecognizesFrets<TState>(InputDevice device, FiveFretGuitarHandler<TState> handler,
            SetFiveFretAction<TState> setFret, Func<FiveFret> getMask, Func<InputEventPtr, FiveFret> getMaskFromEvent,
            ButtonControl green, ButtonControl red, ButtonControl yellow, ButtonControl blue, ButtonControl orange)
            where TState : unmanaged, IInputStateTypeInfo
        {
            var fretMap = new List<(FiveFret fret, ButtonControl control)>()
            {
                (FiveFret.Green, green),
                (FiveFret.Red, red),
                (FiveFret.Yellow, yellow),
                (FiveFret.Blue, blue),
                (FiveFret.Orange, orange),
            };

            var fretList = new List<ButtonControl>(fretMap.Count);
            for (var frets = FiveFret.None; frets <= AllFrets; frets++)
            {
                var state = handler.CreateState();
                setFret(ref state, frets);

                foreach (var (fret, control) in fretMap)
                {
                    if ((frets & fret) != 0)
                        fretList.Add(control);
                }

                InputSystem.onEvent.CallOnce((eventPtr) => Assert.That(getMaskFromEvent(eventPtr), Is.EqualTo(frets)));
                AssertButtonPress(device, state, fretList.ToArray());
                Assert.That(getMask(), Is.EqualTo(frets));
                fretList.Clear();
            }
        }
    }

    public static class XInputFiveFretGuitarHandling
    {
        public static void SetFrets(ref ushort buttonsField, FiveFret frets)
        {
            buttonsField.SetBit((ushort)XInputButton.A, (frets & FiveFret.Green) != 0);
            buttonsField.SetBit((ushort)XInputButton.B, (frets & FiveFret.Red) != 0);
            buttonsField.SetBit((ushort)XInputButton.Y, (frets & FiveFret.Yellow) != 0);
            buttonsField.SetBit((ushort)XInputButton.X, (frets & FiveFret.Blue) != 0);
            buttonsField.SetBit((ushort)XInputButton.LeftShoulder, (frets & FiveFret.Orange) != 0);
        }

        public static short GetWhammy(float value)
        {
            return (short)IntegerAxisControl.Denormalize(value, short.MinValue, short.MaxValue, short.MinValue);
        }
    }
}