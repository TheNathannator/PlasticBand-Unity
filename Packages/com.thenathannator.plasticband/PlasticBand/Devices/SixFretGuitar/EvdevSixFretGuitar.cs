#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.Controls;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal static class EvdevSixFretGuitar
    {
        internal static void Initialize()
        {
            XpadEvdevSixFretGuitar.Initialize();
            SantrollerEvdevSixFretGuitar.Initialize();
            SantrollerV2EvdevSixFretGuitar.Initialize();
            PS3EvdevSixFretGuitar.Initialize();
            PS4EvdevSixFretGuitar.Initialize();
        }
    }

    internal abstract class EvdevSixFretGuitar<TSelf> : SixFretGuitar,
        IStateTranslator<TranslatedSixFretState>
        where TSelf : EvdevSixFretGuitar<TSelf>
    {
        protected static input_key_code m_Black1;
        protected static input_key_code m_Black2;
        protected static input_key_code m_Black3;
        protected static input_key_code m_White1;
        protected static input_key_code m_White2;
        protected static input_key_code m_White3;
        protected static input_key_code m_Ghtv;
        protected static input_key_code m_Select;

        protected static input_abs_code m_Whammy;
        protected static input_abs_code m_Tilt;

        protected static EvdevAxisOption m_WhammyOptions;
        protected static EvdevAxisOption m_TiltOptions;

        protected EvdevStateHandler m_StateHandler;

        private input_absinfo m_WhammyAbs;
        private input_absinfo m_TiltAbs;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator.VerifyDevice(this);

            var capabilities = JsonUtility.FromJson<EvdevCapabilities>(device.description.capabilities);
            m_StateHandler = new EvdevStateHandler(capabilities);

            for (int i = 0; i < capabilities.supportedAbs.Length; i++)
            {
                var abs = capabilities.supportedAbs[i];
                if (abs == m_Whammy)
                {
                    var absInfo = capabilities.absInfo[i];
                    m_WhammyAbs = absInfo;
                }
                else if (abs == m_Tilt)
                {
                    var absInfo = capabilities.absInfo[i];
                    m_TiltAbs = absInfo;
                }
            }
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator.OnStateEvent(this, eventPtr);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator.GetStateOffsetForEvent(this, control, eventPtr, ref offset);

        InputDevice IStateTranslator.ToDevice() => this;

        public unsafe TranslatedSixFretState TranslateStateEvent(StateEvent* stateEvent)
        {
            var buttons = TranslatedSixFretButtonMask.None;

            // D-pad, which strum is merged into
            var hat = m_StateHandler.ReadHat(stateEvent, 0);
            ReadStrum(stateEvent, out bool strumUp, out bool strumDown);
            if ((hat & EvdevHatValue.Up) != 0 || strumUp) buttons |= TranslatedSixFretButtonMask.DpadUp;
            if ((hat & EvdevHatValue.Down) != 0 || strumDown) buttons |= TranslatedSixFretButtonMask.DpadDown;
            if ((hat & EvdevHatValue.Left) != 0) buttons |= TranslatedSixFretButtonMask.DpadLeft;
            if ((hat & EvdevHatValue.Right) != 0) buttons |= TranslatedSixFretButtonMask.DpadRight;

            // Menu/system
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_START)) buttons |= TranslatedSixFretButtonMask.Start;
            if (m_StateHandler.ReadKey(stateEvent, m_Select)) buttons |= TranslatedSixFretButtonMask.Select;
            if (m_StateHandler.ReadKey(stateEvent, m_Ghtv)) buttons |= TranslatedSixFretButtonMask.GHTV;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_MODE)) buttons |= TranslatedSixFretButtonMask.System;

            var frets = SixFret.None;
            if (m_StateHandler.ReadKey(stateEvent, m_Black1)) frets |= SixFret.Black1;
            if (m_StateHandler.ReadKey(stateEvent, m_Black2)) frets |= SixFret.Black2;
            if (m_StateHandler.ReadKey(stateEvent, m_Black3)) frets |= SixFret.Black3;
            if (m_StateHandler.ReadKey(stateEvent, m_White1)) frets |= SixFret.White1;
            if (m_StateHandler.ReadKey(stateEvent, m_White2)) frets |= SixFret.White2;
            if (m_StateHandler.ReadKey(stateEvent, m_White3)) frets |= SixFret.White3;

            return new TranslatedSixFretState()
            {
                buttons = (ushort)buttons,
                frets = (byte)frets,

                whammy = ReadWhammy(stateEvent),
                tilt = ReadTilt(stateEvent),
            };
        }

        // Reads a strum bar reported separately from the d-pad. By default strum is reported on the d-pad only.
        protected virtual unsafe void ReadStrum(StateEvent* stateEvent, out bool strumUp, out bool strumDown)
        {
            strumUp = false;
            strumDown = false;
        }

        protected virtual unsafe byte ReadWhammy(StateEvent* stateEvent)
        {
            float normWhammy = m_StateHandler.ReadAbsNormalized(stateEvent, m_Whammy, m_WhammyAbs, m_WhammyOptions);
            return (byte)IntegerAxisControl.Denormalize(normWhammy, byte.MinValue, byte.MaxValue, byte.MinValue);
        }

        protected virtual unsafe sbyte ReadTilt(StateEvent* stateEvent)
        {
            float normTilt = m_StateHandler.ReadAbsNormalized(stateEvent, m_Tilt, m_TiltAbs, m_TiltOptions);
            return (sbyte)IntegerAxisControl.Denormalize(normTilt, sbyte.MinValue, sbyte.MaxValue, 0);
        }
    }
}
#endif
