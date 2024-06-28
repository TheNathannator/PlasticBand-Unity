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
    internal unsafe struct XboxOneRiffmasterGuitarState : IRiffmasterGuitarState, IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x20;

        public XboxOneRockBandGuitarState baseState;

        private short m_JoystickX;
        private short m_JoystickY;

        public byte reportId { get => baseState.reportId; set => baseState.reportId = value; }

        public bool green { get => baseState.green; set => baseState.green = value; }
        public bool red { get => baseState.red; set => baseState.red = value; }
        public bool yellow { get => baseState.yellow; set => baseState.yellow = value; }
        public bool blue { get => baseState.blue; set => baseState.blue = value; }
        public bool orange { get => baseState.orange; set => baseState.orange = value; }

        public bool soloGreen { get => baseState.soloGreen; set => baseState.soloGreen = value; }
        public bool soloRed { get => baseState.soloRed; set => baseState.soloRed = value; }
        public bool soloYellow { get => baseState.soloYellow; set => baseState.soloYellow = value; }
        public bool soloBlue { get => baseState.soloBlue; set => baseState.soloBlue = value; }
        public bool soloOrange { get => baseState.soloOrange; set => baseState.soloOrange = value; }

        public bool dpadUp { get => baseState.dpadUp; set => baseState.dpadUp = value; }
        public bool dpadDown { get => baseState.dpadDown; set => baseState.dpadDown = value; }
        public bool dpadLeft { get => baseState.dpadLeft; set => baseState.dpadLeft = value; }
        public bool dpadRight { get => baseState.dpadRight; set => baseState.dpadRight = value; }

        public bool start { get => baseState.start; set => baseState.start = value; }
        public bool select { get => baseState.select; set => baseState.select = value; }
        public bool system { get => baseState.system; set => baseState.system = value; }

        public bool p1
        {
            get => false; // Not available on the Xbox One model
            set {}
        }

        public bool joystickClick
        {
            // Click input needs to be ignored if the solo frets are active,
            // as it overlaps with the solo fret flag
            get => (baseState.buttons & GameInputButton.LeftThumb) != 0 && baseState.soloFrets == 0;
            set => baseState.buttons.SetBit(GameInputButton.LeftThumb, value);
        }

        public sbyte joystickX
        {
            get => (sbyte)(m_JoystickX >> 8);
            set => m_JoystickX = (short)((value << 8) | (byte)value);
        }

        public sbyte joystickY
        {
            get => (sbyte)(m_JoystickY >> 8);
            set => m_JoystickY = (short)((value << 8) | (byte)value);
        }

        byte IFiveFretGuitarState.whammy { get => baseState.whammy; set => baseState.whammy = value; }
        public sbyte tilt { get => baseState.tilt; set => baseState.tilt = value; }
        int IRockBandGuitarState_Base.pickupSwitch { get => baseState.pickupSwitch; set => baseState.pickupSwitch = (byte)value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneRiffmasterGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedRiffmasterGuitarState.Format;

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedRiffmasterGuitarButton.System, displayName = "Guide")]
        public TranslatedRiffmasterGuitarState state;
    }

    [InputControlLayout(stateType = typeof(XboxOneRiffmasterGuitarLayout), displayName = "Xbox One Riffmaster Guitar")]
    internal class XboxOneRiffmasterGuitar : TranslatingRiffmasterGuitar<XboxOneRiffmasterGuitarState>,
        IInputStateCallbackReceiver
    {
        internal new static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneRiffmasterGuitar>(0x0E6F, 0x0248);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            GameInputStateTranslator<XboxOneRiffmasterGuitarState, TranslatedRiffmasterGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => GameInputStateTranslator<XboxOneRiffmasterGuitarState, TranslatedRiffmasterGuitarState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => GameInputStateTranslator<XboxOneRiffmasterGuitarState, TranslatedRiffmasterGuitarState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);
    }
}
