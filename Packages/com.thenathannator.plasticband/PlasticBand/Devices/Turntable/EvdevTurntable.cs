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
    internal abstract class EvdevTurntable<TSelf> : Turntable,
        IStateTranslator<TranslatedTurntableState>
        where TSelf : EvdevTurntable<TSelf>
    {
        protected static input_key_code m_SouthButton;
        protected static input_key_code m_EastButton;
        protected static input_key_code m_WestButton;
        protected static input_key_code m_NorthButton;

        protected static input_key_code m_LeftGreen;
        protected static input_key_code m_LeftRed;
        protected static input_key_code m_LeftBlue;
        protected static input_key_code m_RightGreen;
        protected static input_key_code m_RightRed;
        protected static input_key_code m_RightBlue;

        protected static input_abs_code m_LeftVelocity;
        protected static input_abs_code m_RightVelocity;
        protected static input_abs_code m_Crossfader;
        protected static input_abs_code m_EffectsDial;

        protected static EvdevAxisOption m_LeftVelocityOptions;
        protected static EvdevAxisOption m_RightVelocityOptions;
        protected static EvdevAxisOption m_CrossfaderOptions;
        protected static EvdevAxisOption m_EffectsDialOptions;

        protected EvdevStateHandler m_StateHandler;

        private input_absinfo m_LeftVelocityAbs;
        private input_absinfo m_RightVelocityAbs;
        private input_absinfo m_CrossfaderAbs;
        private input_absinfo m_EffectsDialAbs;

        protected override void FinishSetup()
        {
            base.FinishSetup();
            StateTranslator.VerifyDevice(this);

            var capabilities = JsonUtility.FromJson<EvdevCapabilities>(device.description.capabilities);
            m_StateHandler = new EvdevStateHandler(capabilities);

            for (int i = 0; i < capabilities.supportedAbs.Length; i++)
            {
                var abs = capabilities.supportedAbs[i];
                if (abs == m_LeftVelocity)
                {
                    var absInfo = capabilities.absInfo[i];
                    m_LeftVelocityAbs = absInfo;
                }
                else if (abs == m_RightVelocity)
                {
                    var absInfo = capabilities.absInfo[i];
                    m_RightVelocityAbs = absInfo;
                }
                else if (abs == m_Crossfader)
                {
                    var absInfo = capabilities.absInfo[i];
                    m_CrossfaderAbs = absInfo;
                }
                else if (abs == m_EffectsDial)
                {
                    var absInfo = capabilities.absInfo[i];
                    m_EffectsDialAbs = absInfo;
                }
            }
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator.OnStateEvent(this, eventPtr);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => StateTranslator.GetStateOffsetForEvent(this, control, eventPtr, ref offset);

        InputDevice IStateTranslator.ToDevice() => this;

        public unsafe TranslatedTurntableState TranslateStateEvent(StateEvent* stateEvent)
        {
            var left = ReadLeftTableButtons(stateEvent);
            var right = ReadRightTableButtons(stateEvent);
            var tableButtons = left | right;

            var buttons = TranslatedTurntableButtonMask.None;

            // Face buttons; platter buttons also press the matching face button, as on the real turntable,
            // so those are dropped while the platter button is held
            if ((tableButtons & TurntableButton.Green) == 0 && m_StateHandler.ReadKey(stateEvent, m_SouthButton))
                buttons |= TranslatedTurntableButtonMask.South;
            if ((tableButtons & TurntableButton.Red) == 0 && m_StateHandler.ReadKey(stateEvent, m_EastButton))
                buttons |= TranslatedTurntableButtonMask.East;
            if ((tableButtons & TurntableButton.Blue) == 0 && m_StateHandler.ReadKey(stateEvent, m_WestButton))
                buttons |= TranslatedTurntableButtonMask.West;
            if (m_StateHandler.ReadKey(stateEvent, m_NorthButton)) buttons |= TranslatedTurntableButtonMask.North_Euphoria;

            // D-pad
            var hat = ReadDpad(stateEvent);
            if ((hat & EvdevHatValue.Up) != 0) buttons |= TranslatedTurntableButtonMask.DpadUp;
            if ((hat & EvdevHatValue.Down) != 0) buttons |= TranslatedTurntableButtonMask.DpadDown;
            if ((hat & EvdevHatValue.Left) != 0) buttons |= TranslatedTurntableButtonMask.DpadLeft;
            if ((hat & EvdevHatValue.Right) != 0) buttons |= TranslatedTurntableButtonMask.DpadRight;

            // Menu/system
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_START)) buttons |= TranslatedTurntableButtonMask.Start;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_SELECT)) buttons |= TranslatedTurntableButtonMask.Select;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_MODE)) buttons |= TranslatedTurntableButtonMask.System;

            return new TranslatedTurntableState()
            {
                buttons = (ushort)buttons,
                turntableButtons = (byte)((byte)left | ((byte)right << 3)),

                leftTableVelocity = ReadLeftVelocity(stateEvent),
                rightTableVelocity = ReadRightVelocity(stateEvent),

                crossfader = ReadCrossfader(stateEvent),
                effectsDial = ReadEffectsDial(stateEvent),
            };
        }

        protected virtual unsafe EvdevHatValue ReadDpad(StateEvent* stateEvent)
            => m_StateHandler.ReadHat(stateEvent, 0);

        protected virtual unsafe TurntableButton ReadLeftTableButtons(StateEvent* stateEvent)
        {
            var buttons = TurntableButton.None;
            if (m_StateHandler.ReadKey(stateEvent, m_LeftGreen)) buttons |= TurntableButton.Green;
            if (m_StateHandler.ReadKey(stateEvent, m_LeftRed)) buttons |= TurntableButton.Red;
            if (m_StateHandler.ReadKey(stateEvent, m_LeftBlue)) buttons |= TurntableButton.Blue;
            return buttons;
        }

        protected virtual unsafe TurntableButton ReadRightTableButtons(StateEvent* stateEvent)
        {
            var buttons = TurntableButton.None;
            if (m_StateHandler.ReadKey(stateEvent, m_RightGreen)) buttons |= TurntableButton.Green;
            if (m_StateHandler.ReadKey(stateEvent, m_RightRed)) buttons |= TurntableButton.Red;
            if (m_StateHandler.ReadKey(stateEvent, m_RightBlue)) buttons |= TurntableButton.Blue;
            return buttons;
        }

        protected virtual unsafe sbyte ReadLeftVelocity(StateEvent* stateEvent)
        {
            float normVelocity = m_StateHandler.ReadAbsNormalized(stateEvent, m_LeftVelocity, m_LeftVelocityAbs, m_LeftVelocityOptions);
            return (sbyte)IntegerAxisControl.Denormalize(normVelocity, sbyte.MinValue, sbyte.MaxValue, 0);
        }

        protected virtual unsafe sbyte ReadRightVelocity(StateEvent* stateEvent)
        {
            float normVelocity = m_StateHandler.ReadAbsNormalized(stateEvent, m_RightVelocity, m_RightVelocityAbs, m_RightVelocityOptions);
            return (sbyte)IntegerAxisControl.Denormalize(normVelocity, sbyte.MinValue, sbyte.MaxValue, 0);
        }

        protected virtual unsafe sbyte ReadCrossfader(StateEvent* stateEvent)
        {
            float normCrossfader = m_StateHandler.ReadAbsNormalized(stateEvent, m_Crossfader, m_CrossfaderAbs, m_CrossfaderOptions);
            return (sbyte)IntegerAxisControl.Denormalize(normCrossfader, sbyte.MinValue, sbyte.MaxValue, 0);
        }

        protected virtual unsafe ushort ReadEffectsDial(StateEvent* stateEvent)
        {
            float normEffectsDial = m_StateHandler.ReadAbsNormalized(stateEvent, m_EffectsDial, m_EffectsDialAbs, m_EffectsDialOptions);
            return (ushort)IntegerAxisControl.Denormalize(normEffectsDial, ushort.MinValue, ushort.MaxValue, ushort.MinValue);
        }

        // Reads platter buttons from a bitmask in an axis, in the same bits as XInputTurntableState
        protected unsafe TurntableButton ReadPlatterBitmask(StateEvent* stateEvent, input_abs_code abs)
        {
            int field = m_StateHandler.ReadAbs(stateEvent, abs);
            var buttons = TurntableButton.None;
            if ((field & 0x01) != 0) buttons |= TurntableButton.Green;
            if ((field & 0x02) != 0) buttons |= TurntableButton.Red;
            if ((field & 0x04) != 0) buttons |= TurntableButton.Blue;
            return buttons;
        }
    }
}
#endif
