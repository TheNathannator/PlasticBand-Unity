using System;
using System.Linq;
using NUnit.Framework;
using PlasticBand.LowLevel;
using PlasticBand.Tests.Devices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Tests.LowLevel
{
    using static StateTranslationDevice;
    using StateButton = StateTranslationDevice.IncomingState.Button;

    [InputControlLayout(stateType = typeof(TranslatedState), hideInUI = true)]
    internal class StateTranslationDevice : InputDevice, IInputStateCallbackReceiver
    {
        public struct IncomingState : IInputStateTypeInfo
        {
            [Flags]
            public enum Button : byte
            {
                None = 0,

                Button1 = 0x01,
                Button2 = 0x02,
                Button3 = 0x04,
            }

            public FourCC format => new FourCC('I', 'N', 'C', 'M');

            public Button buttons;
            public short axis1;
            public ushort axis2;
        }

        public struct TranslatedState : IInputStateTypeInfo
        {
            public FourCC format => new FourCC('T', 'R', 'N', 'S');

            [InputControl(name = "button1", layout = "Button", bit = 0)]
            [InputControl(name = "button2", layout = "Button", bit = 1)]
            [InputControl(name = "button3", layout = "Button", bit = 2)]
            public byte buttons;

            [InputControl(layout = "IntAxis", parameters = "minValue=-128,maxValue=127,zeroPoint=0")]
            public sbyte axis1;

            [InputControl(layout = "IntAxis", parameters = "minValue=0,maxValue=255,zeroPoint=0")]
            public byte axis2;
        }

        private TranslateStateHandler<IncomingState, TranslatedState> m_Translator;

        public ButtonControl button1 { get; private set; }
        public ButtonControl button2 { get; private set; }
        public ButtonControl button3 { get; private set; }

        public AxisControl axis1 { get; private set; }
        public AxisControl axis2 { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            button1 = GetChildControl<ButtonControl>(nameof(button1));
            button2 = GetChildControl<ButtonControl>(nameof(button2));
            button3 = GetChildControl<ButtonControl>(nameof(button3));

            axis1 = GetChildControl<AxisControl>(nameof(axis1));
            axis2 = GetChildControl<AxisControl>(nameof(axis2));

            m_Translator = TranslateState;
            StateTranslator<IncomingState, TranslatedState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<IncomingState, TranslatedState>.OnStateEvent(this, eventPtr, m_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator<IncomingState, TranslatedState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, m_Translator);

        private TranslatedState TranslateState(ref IncomingState state)
        {
            return new TranslatedState()
            {
                buttons = (byte)state.buttons,
                axis1 = (sbyte)(state.axis1 >> 8),
                axis2 = (byte)(state.axis2 >> 8),
            };
        }
    }

    internal class StateTranslationControlTests : PlasticBandTestFixture<StateTranslationDevice>
    {
        private const float kEpsilon = 1f / byte.MaxValue;

        public override void Setup()
        {
            base.Setup();
            InputSystem.RegisterLayout<StateTranslationDevice>(nameof(StateTranslationDevice));
        }

        public override void TearDown()
        {
            InputSystem.RemoveLayout(nameof(StateTranslationDevice));
            base.TearDown();
        }

        // Sanity check tests to ensure the controls are set up properly
        [Test]
        public void RecognizesButtons() => CreateAndRun((device) =>
        {
            var deviceState = new IncomingState();
            RecognizesButton(device, deviceState, device.button1, SetButton1);
            RecognizesButton(device, deviceState, device.button2, SetButton2);
            RecognizesButton(device, deviceState, device.button3, SetButton3);

            void SetButton1(ref IncomingState state, bool pressed)
                => SetButton(ref state.buttons, StateButton.Button1, pressed);

            void SetButton2(ref IncomingState state, bool pressed)
                => SetButton(ref state.buttons, StateButton.Button2, pressed);

            void SetButton3(ref IncomingState state, bool pressed)
                => SetButton(ref state.buttons, StateButton.Button3, pressed);

            void SetButton(ref StateButton buttons, StateButton mask, bool pressed)
            {
                if (pressed)
                    buttons |= mask;
                else
                    buttons &= ~mask;
            }
        });

        [Test]
        public void RecognizesAxes() => CreateAndRun((device) =>
        {
            RecognizesSignedAxis(device, new IncomingState(), device.axis1, SetAxis1);
            RecognizesUnsignedAxis(device, new IncomingState(), device.axis2, SetAxis2);

            void SetAxis1(ref IncomingState state, float value)
            {
                state.axis1 = DeviceHandling.DenormalizeInt16(value);
            }

            void SetAxis2(ref IncomingState state, float value)
            {
                state.axis2 = DeviceHandling.DenormalizeUInt16(value);
            }
        });

        // The actual tests
        [Test]
        public void WorksWithEnumerateChangedControls() => CreateAndRun((device) =>
        {
            AssertEventUpdate(device, new IncomingState() { axis1 = 1000 }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.axis1);
            });

            var buttons = StateButton.Button1;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons, axis1 = 0 }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.axis1, device.button1);
            });

            buttons = StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.button1, device.button2, device.button3);
            });

            buttons = StateButton.Button1 | StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons, axis2 = 500 }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.button1, device.axis2);
            });

            AssertEventUpdate(device, new IncomingState(), (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.button1, device.button2, device.button3, device.axis2);
            });

            AssertEventUpdate(device, new IncomingState(), (eventPtr) =>
            {
                var changedControls = eventPtr.EnumerateChangedControls(device);
                Assert.That(changedControls, Is.Empty, "Expected no controls to be changed in the last update!");
            });

            void AssertValueChange(InputEventPtr eventPtr, params InputControl[] controls)
            {
                var changedControls = eventPtr.EnumerateChangedControls(device);
                Assert.That(changedControls.Count(), Is.EqualTo(controls.Length), "Unexpected number of changed controls!");
    
                foreach (var control in device.allControls)
                {
                    if (controls.Contains(control))
                        CollectionAssert.Contains(changedControls, control, $"Expected control {control} to appear in the changed controls!");
                    else
                        CollectionAssert.DoesNotContain(changedControls, control, $"Expected control {control} to NOT appear in the changed controls!");
                }
            }
        });

        [Test]
        public void WorksWithGetAllButtonPresses() => CreateAndRun((device) =>
        {
            var buttons = StateButton.Button1;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertButtonPresses(eventPtr, device.button1);
            });

            buttons = StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertButtonPresses(eventPtr, device.button2, device.button3);
            });

            buttons = StateButton.Button1 | StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertButtonPresses(eventPtr, device.button1, device.button2, device.button3);
            });

            buttons = StateButton.None;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                var presses = eventPtr.GetAllButtonPresses();
                Assert.That(presses, Is.Empty, "Expected no presses in the last update!");
            });

            void AssertButtonPresses(InputEventPtr eventPtr, params InputControl[] controls)
            {
                var presses = eventPtr.GetAllButtonPresses();
                Assert.That(presses.Count(), Is.EqualTo(controls.Length), "Unexpected number of presses!");
    
                foreach (var control in device.allControls)
                {
                    if (controls.Contains(control))
                        CollectionAssert.Contains(presses, control, $"Expected control {control} to appear in the pressed controls!");
                    else
                        CollectionAssert.DoesNotContain(presses, control, $"Expected control {control} to NOT appear in the pressed controls!");
                }
            }
        });

        [Test]
        public void WorksWithGetFirstButtonPressOrNull() => CreateAndRun((device) =>
        {
            var buttons = StateButton.Button1;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertFirstButtonPress(eventPtr, device.button1);
            });

            buttons = StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertFirstButtonPress(eventPtr, device.button2, device.button3);
            });

            buttons = StateButton.Button1 | StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertFirstButtonPress(eventPtr, device.button1, device.button2, device.button3);
            });

            void AssertFirstButtonPress(InputEventPtr eventPtr, params InputControl[] controls)
            {
                var pressed = eventPtr.GetFirstButtonPressOrNull();
                CollectionAssert.Contains(controls, pressed, $"Press {pressed} was not one of the expected controls!");
            }

            buttons = StateButton.None;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                var presses = eventPtr.GetFirstButtonPressOrNull();
                Assert.That(presses, Is.Null, "Expected no presses in the last update!");
            });
        });

        [Test]
        public void WorksWithHasButtonPress() => CreateAndRun((device) =>
        {
            var buttons = StateButton.Button1;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, CheckButtonPress);

            buttons = StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, CheckButtonPress);

            buttons = StateButton.Button1 | StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, CheckButtonPress);

            void CheckButtonPress(InputEventPtr eventPtr)
            {
                Assert.That(eventPtr.HasButtonPress(), Is.True, "Expected a button to be pressed!");
            }

            buttons = StateButton.None;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                Assert.That(eventPtr.HasButtonPress(), Is.False, "Expected no buttons to be pressed!");
            });
        });

        [Test]
        public void WorksWithHasValueChangeInEvent() => CreateAndRun((device) =>
        {
            AssertEventUpdate(device, new IncomingState() { axis1 = 1000 }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.axis1);
            });

            var buttons = StateButton.Button1;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons, axis1 = 0 }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.axis1, device.button1);
            });

            buttons = StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.button1, device.button2, device.button3);
            });

            buttons = StateButton.Button1 | StateButton.Button2 | StateButton.Button3;
            AssertEventUpdate(device, new IncomingState() { buttons = buttons, axis2 = 500 }, (eventPtr) =>
            {
                AssertValueChange(eventPtr, device.button1, device.axis2);
            });

            void AssertValueChange(InputEventPtr eventPtr, params InputControl[] controls)
            {
                foreach (var control in device.allControls)
                {
                    if (controls.Contains(control))
                        Assert.That(control.HasValueChangeInEvent(eventPtr), Is.True, $"Expected control {control} to be changed!");
                    else
                        Assert.That(control.HasValueChangeInEvent(eventPtr), Is.False, $"Expected control {control} to NOT be changed!");
                }
            }
        });
    }
}