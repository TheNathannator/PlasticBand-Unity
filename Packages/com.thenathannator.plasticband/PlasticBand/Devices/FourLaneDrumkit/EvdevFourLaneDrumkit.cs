#if HIDROGEN_0_6_0_OR_LATER
using System;
using HIDrogen.Devices.LowLevel;
using HIDrogen.Imports.Linux;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal static class EvdevFourLaneDrumkit
    {
        internal static void Initialize()
        {
            XpadEvdevFourLaneDrumkit.Initialize();
            SantrollerEvdevFourLaneDrumkit.Initialize();
            SantrollerV2EvdevFourLaneDrumkit.Initialize();
            PS3EvdevFourLaneDrumkit.Initialize();
        }
    }

    internal abstract class EvdevFourLaneDrumkit<TSelf> : FourLaneDrumkit,
        IStateTranslator<TranslatedFourLaneState>
        where TSelf : EvdevFourLaneDrumkit<TSelf>
    {
        protected static input_key_code m_Green;
        protected static input_key_code m_Red;
        protected static input_key_code m_Yellow;
        protected static input_key_code m_Blue;
        protected static input_key_code m_Kick1;
        protected static input_key_code m_Kick2;
        protected static input_key_code m_PadFlag;
        protected static input_key_code m_CymbalFlag;

        protected EvdevStateHandler m_StateHandler;

        // Whether pad/cymbal flags have been seen; Rock Band 1 kits never send them
        private bool m_HasFlags;

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

        public unsafe TranslatedFourLaneState TranslateStateEvent(StateEvent* stateEvent)
        {
            bool green = m_StateHandler.ReadKey(stateEvent, m_Green);
            bool red = m_StateHandler.ReadKey(stateEvent, m_Red);
            bool yellow = m_StateHandler.ReadKey(stateEvent, m_Yellow);
            bool blue = m_StateHandler.ReadKey(stateEvent, m_Blue);
            bool pad = m_StateHandler.ReadKey(stateEvent, m_PadFlag);
            bool cymbal = m_StateHandler.ReadKey(stateEvent, m_CymbalFlag);

            var hat = m_StateHandler.ReadHat(stateEvent, 0);
            bool dpadUp = (hat & EvdevHatValue.Up) != 0;
            bool dpadDown = (hat & EvdevHatValue.Down) != 0;

            var pads = TranslatePads(green, red, yellow, blue, pad, cymbal, dpadUp, dpadDown);

            var buttons = TranslatedFourLaneButtonMask.None;

            // Face buttons; these are ignored if the corresponding pad/cymbal is also active
            if ((pads & (FourLanePad.GreenPad | FourLanePad.GreenCymbal)) == 0 && green)
                buttons |= TranslatedFourLaneButtonMask.South;
            if ((pads & FourLanePad.RedPad) == 0 && red)
                buttons |= TranslatedFourLaneButtonMask.East;
            if ((pads & (FourLanePad.BluePad | FourLanePad.BlueCymbal)) == 0 && blue)
                buttons |= TranslatedFourLaneButtonMask.West;
            if ((pads & (FourLanePad.YellowPad | FourLanePad.YellowCymbal)) == 0 && yellow)
                buttons |= TranslatedFourLaneButtonMask.North;

            // D-pad
            // Up/down are ignored if cymbals are active, as they're used to distinguish cymbal hits
            if ((pads & (FourLanePad.YellowCymbal | FourLanePad.BlueCymbal | FourLanePad.GreenCymbal)) == 0)
            {
                if (dpadUp) buttons |= TranslatedFourLaneButtonMask.DpadUp;
                if (dpadDown) buttons |= TranslatedFourLaneButtonMask.DpadDown;
            }
            if ((hat & EvdevHatValue.Left) != 0) buttons |= TranslatedFourLaneButtonMask.DpadLeft;
            if ((hat & EvdevHatValue.Right) != 0) buttons |= TranslatedFourLaneButtonMask.DpadRight;

            // Menu/system
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_START)) buttons |= TranslatedFourLaneButtonMask.Start;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_SELECT)) buttons |= TranslatedFourLaneButtonMask.Select;
            if (m_StateHandler.ReadKey(stateEvent, input_key_code.BTN_MODE)) buttons |= TranslatedFourLaneButtonMask.System;

            // Kicks
            if (m_StateHandler.ReadKey(stateEvent, m_Kick1)) buttons |= TranslatedFourLaneButtonMask.Kick1;
            if (m_StateHandler.ReadKey(stateEvent, m_Kick2)) buttons |= TranslatedFourLaneButtonMask.Kick2;

            var translated = new TranslatedFourLaneState()
            {
                buttons = (ushort)buttons,
            };
            ReadVelocities(stateEvent, pads, ref translated);
            return translated;
        }

        // PlasticBand reference doc:
        // https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/General%20Notes.md#deciphering-pads-and-cymbals
        private FourLanePad TranslatePads(bool green, bool red, bool yellow, bool blue,
            bool pad, bool cymbal, bool dpadUp, bool dpadDown)
        {
            var pads = FourLanePad.None;

            // Store whether or not pad/cymbal flags are present
            m_HasFlags |= pad | cymbal;

            // Pad + cymbal hits can be ambiguous, we need to resolve this
            if (pad && cymbal)
            {
                // There's only ambiguity between pad + cymbal hits of different colors,
                // same-color pad + cymbal can be used directly
                int colorCount = 0;
                if (red) colorCount++;
                if (yellow || dpadUp) colorCount++;
                if (blue || dpadDown) colorCount++;
                if (green || !(dpadUp || dpadDown)) colorCount++;

                if (colorCount > 1)
                {
                    // The d-pad inputs let us resolve the ambiguity of a pad + cymbal hit
                    if (dpadUp)
                    {
                        pads |= FourLanePad.YellowCymbal;
                        yellow = false;
                        cymbal = false;
                    }

                    if (dpadDown)
                    {
                        pads |= FourLanePad.BlueCymbal;
                        blue = false;
                        cymbal = false;
                    }

                    if (!(dpadUp || dpadDown))
                    {
                        pads |= FourLanePad.GreenCymbal;
                        green = false;
                        cymbal = false;
                    }
                }
            }

            // Rock Band 1 kits don't send the pad or cymbal flags, so a hit without
            // either flag is a pad hit if no flags have been seen yet
            if (pad || (!cymbal && !m_HasFlags))
            {
                if (red) pads |= FourLanePad.RedPad;
                if (yellow) pads |= FourLanePad.YellowPad;
                if (blue) pads |= FourLanePad.BluePad;
                if (green) pads |= FourLanePad.GreenPad;
            }

            if (cymbal)
            {
                if (yellow) pads |= FourLanePad.YellowCymbal;
                if (blue) pads |= FourLanePad.BlueCymbal;
                if (green) pads |= FourLanePad.GreenCymbal;
            }

            return pads;
        }

        // Kits with one velocity per colour, shared between its pad and cymbal
        protected virtual unsafe void ReadVelocities(StateEvent* stateEvent, FourLanePad pads, ref TranslatedFourLaneState translated)
        {
            byte redPad = ReadRedVelocity(stateEvent);
            byte yellowPad = ReadYellowVelocity(stateEvent);
            byte bluePad = ReadBlueVelocity(stateEvent);
            byte greenPad = ReadGreenVelocity(stateEvent);
            byte yellowCymbal = yellowPad;
            byte blueCymbal = bluePad;
            byte greenCymbal = greenPad;

            // Pad+cymbal hits of the same color will put the cymbal hit's velocity onto the red velocity axis
            FixCymbalVelocity(pads, FourLanePad.YellowPad | FourLanePad.YellowCymbal, ref redPad, ref yellowCymbal);
            FixCymbalVelocity(pads, FourLanePad.BluePad | FourLanePad.BlueCymbal, ref redPad, ref blueCymbal);
            FixCymbalVelocity(pads, FourLanePad.GreenPad | FourLanePad.GreenCymbal, ref redPad, ref greenCymbal);

            translated.redPad = (pads & FourLanePad.RedPad) != 0 ? Math.Max(redPad, (byte)1) : (byte)0;
            translated.yellowPad = (pads & FourLanePad.YellowPad) != 0 ? Math.Max(yellowPad, (byte)1) : (byte)0;
            translated.bluePad = (pads & FourLanePad.BluePad) != 0 ? Math.Max(bluePad, (byte)1) : (byte)0;
            translated.greenPad = (pads & FourLanePad.GreenPad) != 0 ? Math.Max(greenPad, (byte)1) : (byte)0;

            translated.yellowCymbal = (pads & FourLanePad.YellowCymbal) != 0 ? Math.Max(yellowCymbal, (byte)1) : (byte)0;
            translated.blueCymbal = (pads & FourLanePad.BlueCymbal) != 0 ? Math.Max(blueCymbal, (byte)1) : (byte)0;
            translated.greenCymbal = (pads & FourLanePad.GreenCymbal) != 0 ? Math.Max(greenCymbal, (byte)1) : (byte)0;
        }

        private static void FixCymbalVelocity(FourLanePad pads, FourLanePad colorPads, ref byte redVelocity, ref byte cymbalVelocity)
        {
            if ((pads & colorPads) == colorPads)
            {
                cymbalVelocity = redVelocity;
                redVelocity = 0;
            }
        }

        protected virtual unsafe byte ReadRedVelocity(StateEvent* stateEvent) => byte.MaxValue;
        protected virtual unsafe byte ReadYellowVelocity(StateEvent* stateEvent) => byte.MaxValue;
        protected virtual unsafe byte ReadBlueVelocity(StateEvent* stateEvent) => byte.MaxValue;
        protected virtual unsafe byte ReadGreenVelocity(StateEvent* stateEvent) => byte.MaxValue;

        protected static byte GetVelocity_Positive(short velocity)
            => (byte)(255 - (byte)(velocity >> 7));

        protected static byte GetVelocity_Negative(short velocity)
        {
            if (velocity == 0)
                return 255;

            return (byte)(255 - (byte)((~velocity) >> 7));
        }
    }
}
#endif
