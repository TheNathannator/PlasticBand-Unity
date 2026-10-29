using System;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Xbox%20One.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XboxOneRockBandGuitarState : IRockBandGuitarState_Distinct, IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x20;

        public byte reportId;

        public GameInputButton buttons;

        private byte m_Tilt;
        public byte whammy;
        public byte pickupSwitch;

        public byte frets;
        public byte soloFrets;

        private fixed byte unk1[3];

        public bool green
        {
            get => (frets & 0x01) != 0;
            set => frets.SetBit(0x01, value);
        }

        public bool red
        {
            get => (frets & 0x02) != 0;
            set => frets.SetBit(0x02, value);
        }

        public bool yellow
        {
            get => (frets & 0x04) != 0;
            set => frets.SetBit(0x04, value);
        }

        public bool blue
        {
            get => (frets & 0x08) != 0;
            set => frets.SetBit(0x08, value);
        }

        public bool orange
        {
            get => (frets & 0x10) != 0;
            set => frets.SetBit(0x10, value);
        }

        public bool soloGreen
        {
            get => (soloFrets & 0x01) != 0;
            set => soloFrets.SetBit(0x01, value);
        }

        public bool soloRed
        {
            get => (soloFrets & 0x02) != 0;
            set => soloFrets.SetBit(0x02, value);
        }

        public bool soloYellow
        {
            get => (soloFrets & 0x04) != 0;
            set => soloFrets.SetBit(0x04, value);
        }

        public bool soloBlue
        {
            get => (soloFrets & 0x08) != 0;
            set => soloFrets.SetBit(0x08, value);
        }

        public bool soloOrange
        {
            get => (soloFrets & 0x10) != 0;
            set => soloFrets.SetBit(0x10, value);
        }

        public bool dpadUp
        {
            get => (buttons & GameInputButton.DpadUp) != 0;
            set => buttons.SetBit(GameInputButton.DpadUp, value);
        }

        public bool dpadDown
        {
            get => (buttons & GameInputButton.DpadDown) != 0;
            set => buttons.SetBit(GameInputButton.DpadDown, value);
        }

        public bool dpadLeft
        {
            get => (buttons & GameInputButton.DpadLeft) != 0;
            set => buttons.SetBit(GameInputButton.DpadLeft, value);
        }

        public bool dpadRight
        {
            get => (buttons & GameInputButton.DpadRight) != 0;
            set => buttons.SetBit(GameInputButton.DpadRight, value);
        }

        public bool start
        {
            get => (buttons & GameInputButton.Menu) != 0;
            set => buttons.SetBit(GameInputButton.Menu, value);
        }

        public bool select
        {
            get => (buttons & GameInputButton.View) != 0;
            set => buttons.SetBit(GameInputButton.View, value);
        }

        public bool system
        {
            get => false; // Not exposed through GameInput
            set {}
        }

        byte IFiveFretGuitarState.whammy
        {
            get => whammy;
            set => whammy = value;
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt >> 1);
            set
            {
                byte clamped = (byte)Math.Max(value, (sbyte)0);
                m_Tilt = (byte)((clamped << 1) | (clamped >> 7));
            }
        }

        int IRockBandGuitarState_Base.pickupSwitch
        {
            get => pickupSwitch;
            set => pickupSwitch = (byte)value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneRockBandGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedRockBandGuitarState.Format;

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRockBandGuitarButton.System, displayName = "Guide")]
        public TranslatedRockBandGuitarState state;
    }

    [InputControlLayout(stateType = typeof(XboxOneRockBandGuitarLayout), displayName = "Xbox One Rock Band Guitar")]
    internal class XboxOneRockBandGuitar : TranslatingRockBandGuitar_Distinct<XboxOneRockBandGuitarState>,
        IInputStateCallbackReceiver
    {
        internal new static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneRockBandGuitar>(0x0738, 0x4161);
            GameInputLayoutFinder.RegisterLayout<XboxOneRockBandGuitar>(0x0E6F, 0x0170);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            GameInputStateTranslator<XboxOneRockBandGuitarState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => GameInputStateTranslator<XboxOneRockBandGuitarState, TranslatedRockBandGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => GameInputStateTranslator<XboxOneRockBandGuitarState, TranslatedRockBandGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);
    }
}
