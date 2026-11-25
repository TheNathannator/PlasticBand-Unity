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
    internal abstract class EvdevRockBandGuitar<TSelf> : RockBandGuitar,
        IStateTranslator<TranslatedRockBandGuitarState>
        where TSelf : EvdevRockBandGuitar<TSelf>
    {
        protected static input_key_code m_GreenFret;
        protected static input_key_code m_RedFret;
        protected static input_key_code m_YellowFret;
        protected static input_key_code m_BlueFret;
        protected static input_key_code m_OrangeFret;
        protected static input_key_code m_Solo;

        protected static input_abs_code m_Whammy;
        protected static input_abs_code m_Tilt;
        protected static input_abs_code m_PickupSwitch;

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

        public unsafe TranslatedRockBandGuitarState TranslateStateEvent(StateEvent* stateEvent)
        {
            var buttons = TranslatedRockBandGuitarButtonMask.None;

            // D-pad
            var hat = m_StateHandler.ReadHat(stateEvent, 0);
            if ((hat & EvdevHatValue.Up) != 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadUp;
            if ((hat & EvdevHatValue.Down) != 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadDown;
            if ((hat & EvdevHatValue.Left) != 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadLeft;
            if ((hat & EvdevHatValue.Right) != 0) buttons |= TranslatedRockBandGuitarButtonMask.DpadRight;

            // Menu/system
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_START)) buttons |= TranslatedRockBandGuitarButtonMask.Start;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_SELECT)) buttons |= TranslatedRockBandGuitarButtonMask.Select;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_MODE)) buttons |= TranslatedRockBandGuitarButtonMask.System;

            var frets = FiveFret.None;
            if (m_StateHandler.ReadKey(stateEvent, m_GreenFret)) frets |= FiveFret.Green;
            if (m_StateHandler.ReadKey(stateEvent, m_RedFret)) frets |= FiveFret.Red;
            if (m_StateHandler.ReadKey(stateEvent, m_YellowFret)) frets |= FiveFret.Yellow;
            if (m_StateHandler.ReadKey(stateEvent, m_BlueFret)) frets |= FiveFret.Blue;
            if (m_StateHandler.ReadKey(stateEvent, m_OrangeFret)) frets |= FiveFret.Orange;

            // The solo flag moves the fret presses to the solo frets; only one set is active at a time
            var soloFrets = ReadSoloFrets(stateEvent);
            if (m_StateHandler.ReadKey(stateEvent, m_Solo))
            {
                soloFrets |= frets;
                frets = FiveFret.None;
            }

            return new TranslatedRockBandGuitarState()
            {
                buttons = (ushort)buttons,

                frets = (byte)frets,
                soloFrets = (byte)soloFrets,

                whammy = ReadWhammy(stateEvent),
                tilt = ReadTilt(stateEvent),
                pickupSwitch = (byte)ReadPickupSwitch(stateEvent),
            };
        }

        // For guitars whose solo frets are their own inputs rather than a solo flag
        protected virtual unsafe FiveFret ReadSoloFrets(StateEvent* stateEvent) => FiveFret.None;

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

        protected virtual unsafe int ReadPickupSwitch(StateEvent* stateEvent)
            => RockBandGuitarState.GetPickupSwitchNotch((byte)m_StateHandler.ReadAbs(stateEvent, m_PickupSwitch));
    }
}
#endif
