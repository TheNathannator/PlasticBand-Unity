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
    internal abstract class EvdevGuitarHeroGuitar<TSelf> : GuitarHeroGuitar,
        IStateTranslator<TranslatedGuitarHeroGuitarState>
        where TSelf : EvdevGuitarHeroGuitar<TSelf>
    {
        protected static input_key_code m_GreenFret;
        protected static input_key_code m_RedFret;
        protected static input_key_code m_YellowFret;
        protected static input_key_code m_BlueFret;
        protected static input_key_code m_OrangeFret;
        protected static input_key_code m_SpPedal;

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

        public unsafe TranslatedGuitarHeroGuitarState TranslateStateEvent(StateEvent* stateEvent)
        {
            var buttons = TranslatedGuitarHeroGuitarButtonMask.None;

            // D-pad
            var hat = m_StateHandler.ReadHat(stateEvent, 0);
            if ((hat & EvdevHatValue.Up) != 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadUp;
            if ((hat & EvdevHatValue.Down) != 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadDown;
            if ((hat & EvdevHatValue.Left) != 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadLeft;
            if ((hat & EvdevHatValue.Right) != 0) buttons |= TranslatedGuitarHeroGuitarButtonMask.DpadRight;

            // Menu/system
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_START)) buttons |= TranslatedGuitarHeroGuitarButtonMask.Start;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_SELECT)) buttons |= TranslatedGuitarHeroGuitarButtonMask.Select;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_MODE)) buttons |= TranslatedGuitarHeroGuitarButtonMask.System;

            // Misc.
            if (m_StateHandler.ReadKey(stateEvent, m_SpPedal)) buttons |= TranslatedGuitarHeroGuitarButtonMask.Pedal;

            var frets = FiveFret.None;
            if (m_StateHandler.ReadKey(stateEvent, m_GreenFret)) frets |= FiveFret.Green;
            if (m_StateHandler.ReadKey(stateEvent, m_RedFret)) frets |= FiveFret.Red;
            if (m_StateHandler.ReadKey(stateEvent, m_YellowFret)) frets |= FiveFret.Yellow;
            if (m_StateHandler.ReadKey(stateEvent, m_BlueFret)) frets |= FiveFret.Blue;
            if (m_StateHandler.ReadKey(stateEvent, m_OrangeFret)) frets |= FiveFret.Orange;

            return new TranslatedGuitarHeroGuitarState()
            {
                buttons = buttons,

                frets = frets,
                touchFrets = ReadTouchFrets(stateEvent),

                whammy = ReadWhammy(stateEvent),
                tilt = ReadTilt(stateEvent),
            };
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

        protected virtual unsafe FiveFret ReadTouchFrets(StateEvent* stateEvent) => FiveFret.None;
    }
}
#endif
