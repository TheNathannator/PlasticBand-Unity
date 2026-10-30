using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/Xbox%20One.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XboxOneFourLaneDrumkitState : IFourLaneDrumkitState_DistinctVelocities, IReportIdStateTypeInfo
    {
        public FourCC format => GameInputDefinitions.InputFormat;
        byte IReportIdStateTypeInfo.reportId => 0x20;

        public byte reportId;

        public GameInputButton buttons;

        public byte pads1;
        public byte pads2;
        public byte cymbals1;
        public byte cymbals2;

        // To avoid needing to decimate the velocity precision of other drumkits, a hack was added to
        // HIDrogen's GameInput backend to pad state events to a minimum of 10 bytes.
        // Without it, we wouldn't be able to fit TranslatedFourLaneState into XB1 drumkit state events.
        // These 3 bytes of padding bring this state struct to 10 bytes total to make use of that space.
        private fixed byte padding[3];

        public bool south
        {
            get => (buttons & GameInputButton.A) != 0;
            set => buttons.SetBit(GameInputButton.A, value);
        }

        public bool east
        {
            get => (buttons & GameInputButton.B) != 0;
            set => buttons.SetBit(GameInputButton.B, value);
        }

        public bool west
        {
            get => (buttons & GameInputButton.X) != 0;
            set => buttons.SetBit(GameInputButton.X, value);
        }

        public bool north
        {
            get => (buttons & GameInputButton.Y) != 0;
            set => buttons.SetBit(GameInputButton.Y, value);
        }

        public bool kick1
        {
            get => (buttons & GameInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(GameInputButton.LeftShoulder, value);
        }

        public bool kick2
        {
            get => (buttons & GameInputButton.RightShoulder) != 0;
            set => buttons.SetBit(GameInputButton.RightShoulder, value);
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

        public byte redPadVelocity
        {
            get => GetVelocityHigh(pads1);
            set => SetVelocityHigh(ref pads1, value);
        }

        public byte yellowPadVelocity
        {
            get => GetVelocityLow(pads1);
            set => SetVelocityLow(ref pads1, value);
        }

        public byte bluePadVelocity
        {
            get => GetVelocityHigh(pads2);
            set => SetVelocityHigh(ref pads2, value);
        }

        public byte greenPadVelocity
        {
            get => GetVelocityLow(pads2);
            set => SetVelocityLow(ref pads2, value);
        }

        public byte yellowCymbalVelocity
        {
            get => GetVelocityHigh(cymbals1);
            set => SetVelocityHigh(ref cymbals1, value);
        }

        public byte blueCymbalVelocity
        {
            get => GetVelocityLow(cymbals1);
            set => SetVelocityLow(ref cymbals1, value);
        }

        public byte greenCymbalVelocity
        {
            get => GetVelocityHigh(cymbals2);
            set => SetVelocityHigh(ref cymbals2, value);
        }

        private static byte GetVelocityLow(byte rawValue)
            => (byte)((rawValue << 4) | (rawValue & 0x0F));

        private static byte GetVelocityHigh(byte rawValue)
            => (byte)((rawValue & 0xF0) | (rawValue >> 4));

        private static void SetVelocityLow(ref byte rawValue, byte value)
            => rawValue = (byte)((rawValue & 0xF0) | (value >> 4));

        private static void SetVelocityHigh(ref byte rawValue, byte value)
            => rawValue = (byte)((rawValue & 0x0F) | (value & 0xF0));
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XboxOneFourLaneDrumkitLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedFourLaneState.Format;

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedFourLaneButton.System, displayName = "Guide")]
        public TranslatedFourLaneState state;
    }

    [InputControlLayout(stateType = typeof(XboxOneFourLaneDrumkitLayout), displayName = "Xbox One Rock Band Drumkit")]
    internal class XboxOneFourLaneDrumkit : TranslatingFourLaneDrumkit_Distinct<XboxOneFourLaneDrumkitState>,
        IInputStateCallbackReceiver
    {
        internal new static void Initialize()
        {
            GameInputLayoutFinder.RegisterLayout<XboxOneFourLaneDrumkit>(0x0738, 0x4262);
            GameInputLayoutFinder.RegisterLayout<XboxOneFourLaneDrumkit>(0x0E6F, 0x0171);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            GameInputStateTranslator<XboxOneFourLaneDrumkitState, TranslatedFourLaneState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => GameInputStateTranslator<XboxOneFourLaneDrumkitState, TranslatedFourLaneState>.OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
            => GameInputStateTranslator<XboxOneFourLaneDrumkitState, TranslatedFourLaneState>.GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);
    }
}
