#if HIDROGEN_0_6_0_OR_LATER
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.Controls;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal static class EvdevGuitarHeroGuitar
    {
        internal static void Initialize()
        {
            XpadGuitarHeroGuitar.Initialize();
        }
    }

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
        // protected static input_abs_code m_TouchBar; // TODO

        protected static EvdevAxisOption m_WhammyOptions;
        protected static EvdevAxisOption m_TiltOptions;

        private EvdevStateHandler m_StateHandler;

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

            float normWhammy = m_StateHandler.ReadAbsNormalized(stateEvent, m_Whammy, m_WhammyAbs, m_WhammyOptions);
            byte whammy = (byte)IntegerAxisControl.Denormalize(normWhammy, byte.MinValue, byte.MaxValue, byte.MinValue);

            float normTilt = m_StateHandler.ReadAbsNormalized(stateEvent, m_Tilt, m_TiltAbs, m_TiltOptions);
            sbyte tilt = (sbyte)IntegerAxisControl.Denormalize(normTilt, sbyte.MinValue, sbyte.MaxValue, 0);

            return new TranslatedGuitarHeroGuitarState()
            {
                buttons = buttons,

                frets = frets,
                // touchFrets = TranslatingGuitarHeroGuitar.s_WtTouchFrets[(state.rawTouchBar & 0xF0) >> 4],

                whammy = whammy,
                tilt = tilt,
            };
        }
    }

    [InputControlLayout(stateType = typeof(TranslatedGuitarHeroGuitarState), hideInUI = true)]
    internal class XpadGuitarHeroGuitar : EvdevGuitarHeroGuitar<XpadGuitarHeroGuitar>
    {
        internal new static void Initialize()
        {
            m_GreenFret = input_key_code.BTN_A;
            m_RedFret = input_key_code.BTN_B;
            m_YellowFret = input_key_code.BTN_Y;
            m_BlueFret = input_key_code.BTN_X;
            m_OrangeFret = input_key_code.BTN_TL;
            m_SpPedal = input_key_code.BTN_TR;

            m_Whammy = input_abs_code.ABS_RX;
            m_Tilt = input_abs_code.ABS_RY;
            // m_TouchBar = ; // TODO

            m_WhammyOptions = EvdevAxisOption.Unsigned;
            m_TiltOptions = EvdevAxisOption.None;

            // These mappings are only required for device validation, but may as well fill them out properly
            var xpadMappings = new EvdevDeviceMapping()
                .WithKey(input_key_code.BTN_START, "start")
                .WithKey(input_key_code.BTN_SELECT, "select")
                .WithKey(input_key_code.BTN_A, "greenFret")
                .WithKey(input_key_code.BTN_B, "redFret")
                .WithKey(input_key_code.BTN_Y, "yellowFret")
                .WithKey(input_key_code.BTN_X, "blueFret")
                .WithKey(input_key_code.BTN_TL, "orangeFret")
                .WithHat(0, "dpad")
                .WithAbs(input_abs_code.ABS_RX, "whammy", EvdevAxisOption.Unsigned)
                .WithAbs(input_abs_code.ABS_RY, "tilt")
                .WithKey(input_key_code.BTN_TR, "spPedal");

            EvdevLayoutFinder.RegisterStaticLayout<XpadGuitarHeroGuitar>(
                xpadMappings,
                new EvdevDeviceMatcher()
                    .WithDriver("xpad") // TODO: driver constants
                    .WithXusbSubtype((byte)XInputDeviceSubType.GuitarAlternate)
            );
        }
    }
}
#endif
