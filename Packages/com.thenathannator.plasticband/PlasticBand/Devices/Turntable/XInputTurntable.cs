using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputTurntableState : ITurntableState
    {
        public FourCC format => XInputGamepad.Format;

        public XInputButton buttons;

        public byte leftTableButtons;
        public byte rightTableButtons;

        private readonly short m_LeftTableVelocity;
        private readonly short m_RightTableVelocity;
        private readonly short m_EffectsDial;
        private readonly short m_Crossfader;

        public bool south => (buttons & XInputButton.A) != 0;
        public bool east => (buttons & XInputButton.B) != 0;
        public bool west => (buttons & XInputButton.X) != 0;
        public bool north_euphoria => (buttons & XInputButton.Y) != 0;

        public bool dpadUp => (buttons & XInputButton.DpadUp) != 0;
        public bool dpadDown => (buttons & XInputButton.DpadDown) != 0;
        public bool dpadLeft => (buttons & XInputButton.DpadLeft) != 0;
        public bool dpadRight => (buttons & XInputButton.DpadRight) != 0;

        public bool start => (buttons & XInputButton.Start) != 0;
        public bool select => (buttons & XInputButton.Back) != 0;
        public bool system => (buttons & XInputButton.Guide) != 0;

        public bool leftGreen => (leftTableButtons & 0x01) != 0;
        public bool leftRed => (leftTableButtons & 0x02) != 0;
        public bool leftBlue => (leftTableButtons & 0x04) != 0;

        public bool rightGreen => (rightTableButtons & 0x01) != 0;
        public bool rightRed => (rightTableButtons & 0x02) != 0;
        public bool rightBlue => (rightTableButtons & 0x04) != 0;

        // Turntable velocity on Xbox 360 tables is really small in range for some reason,
        // so we only take the bottom byte (two's compliment ensures no sign copying needs to be done)
        public sbyte leftVelocity => (sbyte)(m_LeftTableVelocity & 0xFF);
        public sbyte rightVelocity => (sbyte)(m_RightTableVelocity & 0xFF);

        public sbyte effectsDial => (sbyte)(m_EffectsDial >> 8);
        public sbyte crossfader => (sbyte)(m_Crossfader >> 8);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedTurntableState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedTurntableButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedTurntableButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedTurntableButton.West, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedTurntableButton.North_Euphoria, displayName = "Y / Euphoria")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedTurntableButton.System, displayName = "Guide")]
        public TranslatedTurntableState state;
    }

    [InputControlLayout(stateType = typeof(XInputTurntableState), displayName = "XInput DJ Hero Turntable")]
    internal class XInputTurntable : Turntable
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputTurntable>(XInputNonStandardSubType.Turntable);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new XInputTurntableHaptics(this);
        }
    }
}
