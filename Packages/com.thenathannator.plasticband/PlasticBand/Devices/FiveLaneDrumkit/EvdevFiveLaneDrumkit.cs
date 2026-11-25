#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal abstract class EvdevFiveLaneDrumkit<TSelf> : FiveLaneDrumkit,
        IStateTranslator<TranslatedFiveLaneState>
        where TSelf : EvdevFiveLaneDrumkit<TSelf>
    {
        protected static input_key_code m_GreenPad;
        protected static input_key_code m_RedPad;
        protected static input_key_code m_YellowCymbal;
        protected static input_key_code m_BluePad;
        protected static input_key_code m_OrangeCymbal;
        protected static input_key_code m_Kick;

        protected EvdevStateHandler m_StateHandler;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator.VerifyDevice(this);

            var capabilities = JsonUtility.FromJson<EvdevCapabilities>(device.description.capabilities);
            m_StateHandler = new EvdevStateHandler(capabilities);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator.OnStateEvent(this, eventPtr);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator.GetStateOffsetForEvent(this, control, eventPtr, ref offset);

        InputDevice IStateTranslator.ToDevice() => this;

        public unsafe TranslatedFiveLaneState TranslateStateEvent(StateEvent* stateEvent)
        {
            var hits = ReadHits(stateEvent);
            ReadVelocities(stateEvent, out byte green, out byte red, out byte yellow, out byte blue, out byte orange, out byte kick);

            var translated = new TranslatedFiveLaneState()
            {
                redPad = (hits & FiveLanePad.Red) != 0 ? red : (byte)0,
                yellowCymbal = (hits & FiveLanePad.Yellow) != 0 ? yellow : (byte)0,
                bluePad = (hits & FiveLanePad.Blue) != 0 ? blue : (byte)0,
                orangeCymbal = (hits & FiveLanePad.Orange) != 0 ? orange : (byte)0,
                greenPad = (hits & FiveLanePad.Green) != 0 ? green : (byte)0,
                kick = (hits & FiveLanePad.Kick) != 0 ? kick : (byte)0,
            };

            var buttons = TranslatedFiveLaneButtonMask.None;

            // Face buttons; a hit without a velocity is a face button press rather than a pad hit
            if ((hits & FiveLanePad.Green) != 0 && translated.greenPad == 0) buttons |= TranslatedFiveLaneButtonMask.South;
            if ((hits & FiveLanePad.Red) != 0 && translated.redPad == 0) buttons |= TranslatedFiveLaneButtonMask.East;
            if ((hits & FiveLanePad.Blue) != 0 && translated.bluePad == 0) buttons |= TranslatedFiveLaneButtonMask.West;
            if ((hits & FiveLanePad.Yellow) != 0 && translated.yellowCymbal == 0) buttons |= TranslatedFiveLaneButtonMask.North;

            // D-pad
            var hat = ReadDpad(stateEvent);
            if ((hat & EvdevHatValue.Up) != 0) buttons |= TranslatedFiveLaneButtonMask.DpadUp;
            if ((hat & EvdevHatValue.Down) != 0) buttons |= TranslatedFiveLaneButtonMask.DpadDown;
            if ((hat & EvdevHatValue.Left) != 0) buttons |= TranslatedFiveLaneButtonMask.DpadLeft;
            if ((hat & EvdevHatValue.Right) != 0) buttons |= TranslatedFiveLaneButtonMask.DpadRight;

            // Menu/system
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_START)) buttons |= TranslatedFiveLaneButtonMask.Start;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_SELECT)) buttons |= TranslatedFiveLaneButtonMask.Select;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_MODE)) buttons |= TranslatedFiveLaneButtonMask.System;

            translated.buttons = (ushort)buttons;
            return translated;
        }

        protected virtual unsafe EvdevHatValue ReadDpad(StateEvent* stateEvent)
            => m_StateHandler.ReadHat(stateEvent, 0);

        protected virtual unsafe FiveLanePad ReadHits(StateEvent* stateEvent)
        {
            var hits = FiveLanePad.None;
            if (m_StateHandler.ReadKey(stateEvent, m_GreenPad)) hits |= FiveLanePad.Green;
            if (m_StateHandler.ReadKey(stateEvent, m_RedPad)) hits |= FiveLanePad.Red;
            if (m_StateHandler.ReadKey(stateEvent, m_YellowCymbal)) hits |= FiveLanePad.Yellow;
            if (m_StateHandler.ReadKey(stateEvent, m_BluePad)) hits |= FiveLanePad.Blue;
            if (m_StateHandler.ReadKey(stateEvent, m_OrangeCymbal)) hits |= FiveLanePad.Orange;
            if (m_StateHandler.ReadKey(stateEvent, m_Kick)) hits |= FiveLanePad.Kick;
            return hits;
        }

        // Kits whose velocities can't be read report every hit at full strength
        protected virtual unsafe void ReadVelocities(StateEvent* stateEvent, out byte green, out byte red,
            out byte yellow, out byte blue, out byte orange, out byte kick)
        {
            green = red = yellow = blue = orange = kick = byte.MaxValue;
        }

        protected static byte GetLow(int axis) => (byte)axis;
        protected static byte GetHigh(int axis) => (byte)(axis >> 8);
    }
}
#endif
