using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarHeroGuitarState : IGuitarHeroGuitarState
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        // Accelerometer values are ignored, as they are unreliable between different guitar models
        private byte m_AccelY;
        private byte m_AccelZ;

        public short m_TouchBar;
        public short m_Unused;
        public short m_Whammy;
        public short m_Tilt;

        public bool green
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool red
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool yellow
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
        }

        public bool blue
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool orange
        {
            get => (buttons & XInputButton.LeftShoulder) != 0;
            set => buttons.SetBit(XInputButton.LeftShoulder, value);
        }

        public bool dpadUp
        {
            get => (buttons & XInputButton.DpadUp) != 0;
            set => buttons.SetBit(XInputButton.DpadUp, value);
        }

        public bool dpadDown
        {
            get => (buttons & XInputButton.DpadDown) != 0;
            set => buttons.SetBit(XInputButton.DpadDown, value);
        }

        public bool dpadLeft
        {
            get => (buttons & XInputButton.DpadLeft) != 0;
            set => buttons.SetBit(XInputButton.DpadLeft, value);
        }

        public bool dpadRight
        {
            get => (buttons & XInputButton.DpadRight) != 0;
            set => buttons.SetBit(XInputButton.DpadRight, value);
        }

        public bool start
        {
            get => (buttons & XInputButton.Start) != 0;
            set => buttons.SetBit(XInputButton.Start, value);
        }

        public bool select
        {
            get => (buttons & XInputButton.Back) != 0;
            set => buttons.SetBit(XInputButton.Back, value);
        }

        public bool system
        {
            get => (buttons & XInputButton.Guide) != 0;
            set => buttons.SetBit(XInputButton.Guide, value);
        }

        // Ranges from -32768 to 32767
        public byte whammy
        {
            get => (byte)((m_Whammy >> 8) + 0x80);
            set => m_Whammy = (short)(((value - 0x80) << 8) | value);
        }

        public sbyte tilt
        {
            get => (sbyte)(m_Tilt >> 8);
            set => m_Tilt = (short)((value << 8) | (byte)value);
        }

        public bool spPedal
        {
            get => (buttons & XInputButton.RightShoulder) != 0;
            set => buttons.SetBit(XInputButton.RightShoulder, value);
        }

        public byte rawTouchBar
        {
            get => (byte)((m_TouchBar >> 8) ^ 0x80);
            set => m_TouchBar = (short)((byte)(value ^ 0x80) << 8);
        }
    }

    // Raw touchbar conversion is different on GH5 guitars
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarHeroGuitarState_GH5 : IGuitarHeroGuitarState
    {
        public FourCC format => XInputGamepad.Format;

        public XInputGuitarHeroGuitarState state;

        public bool green { get => state.green; set => state.green = value; }
        public bool red { get => state.red; set => state.red = value; }
        public bool yellow { get => state.yellow; set => state.yellow = value; }
        public bool blue { get => state.blue; set => state.blue = value; }
        public bool orange { get => state.orange; set => state.orange = value; }
        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }
        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }
        public byte whammy { get => state.whammy; set => state.whammy = value; }
        public sbyte tilt { get => state.tilt; set => state.tilt = value; }
        public bool spPedal { get => state.spPedal; set => state.spPedal = value; }

        public byte rawTouchBar
        {
            get => (byte)(state.m_TouchBar ^ 0x80);
            set => state.m_TouchBar = (short)((sbyte)(value ^ 0x80) * 257);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputGuitarHeroGuitarLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedGuitarHeroGuitarState.Format;

        [InputControl(name = "selectButton", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.Select, displayName = "Back")]
        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedGuitarHeroGuitarButton.System, displayName = "Guide")]
        public TranslatedGuitarHeroGuitarState state;
    }

    [InputControlLayout(stateType = typeof(XInputGuitarHeroGuitarLayout), displayName = "XInput Guitar Hero Guitar")]
    internal class XInputGuitarHeroGuitar : TranslatingGuitarHeroGuitar_Ranges<XInputGuitarHeroGuitarState>
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputGuitarHeroGuitar>(XInputController.DeviceSubType.GuitarAlternate);
            XInputLayoutFinder.RegisterLayout<XInputGuitarHeroGuitar_GH5>(
                XInputController.DeviceSubType.GuitarAlternate, 0x1430, 0x705
            );
        }
    }

    [InputControlLayout(stateType = typeof(XInputGuitarHeroGuitarLayout), displayName = "XInput Guitar Hero Guitar", hideInUI = true)]
    internal class XInputGuitarHeroGuitar_GH5 : TranslatingGuitarHeroGuitar_Discrete<XInputGuitarHeroGuitarState_GH5> { }
}
