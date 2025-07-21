using System.Runtime.InteropServices;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
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

        private short m_LeftTableVelocity;
        private short m_RightTableVelocity;
        private ushort m_EffectsDial;
        private short m_Crossfader;

        public bool south
        {
            get => (buttons & XInputButton.A) != 0;
            set => buttons.SetBit(XInputButton.A, value);
        }

        public bool east
        {
            get => (buttons & XInputButton.B) != 0;
            set => buttons.SetBit(XInputButton.B, value);
        }

        public bool west
        {
            get => (buttons & XInputButton.X) != 0;
            set => buttons.SetBit(XInputButton.X, value);
        }

        public bool north_euphoria
        {
            get => (buttons & XInputButton.Y) != 0;
            set => buttons.SetBit(XInputButton.Y, value);
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

        public bool leftGreen
        {
            get => (leftTableButtons & 0x01) != 0;
            set => leftTableButtons.SetBit(0x01, value);
        }

        public bool leftRed
        {
            get => (leftTableButtons & 0x02) != 0;
            set => leftTableButtons.SetBit(0x02, value);
        }

        public bool leftBlue
        {
            get => (leftTableButtons & 0x04) != 0;
            set => leftTableButtons.SetBit(0x04, value);
        }

        public bool rightGreen
        {
            get => (rightTableButtons & 0x01) != 0;
            set => rightTableButtons.SetBit(0x01, value);
        }

        public bool rightRed
        {
            get => (rightTableButtons & 0x02) != 0;
            set => rightTableButtons.SetBit(0x02, value);
        }

        public bool rightBlue
        {
            get => (rightTableButtons & 0x04) != 0;
            set => rightTableButtons.SetBit(0x04, value);
        }

        // Turntable velocity on Xbox 360 tables is really small in range for some reason,
        // so we only take the bottom byte (two's compliment ensures no sign copying needs to be done)
        public sbyte leftVelocity
        {
            get => (sbyte)(m_LeftTableVelocity & 0xFF);
            set => m_LeftTableVelocity = value;
        }

        public sbyte rightVelocity
        {
            get => (sbyte)(m_RightTableVelocity & 0xFF);
            set => m_RightTableVelocity = value;
        }

        public ushort effectsDial
        {
            get => m_EffectsDial;
            set => m_EffectsDial = value;
        }

        public sbyte crossfader
        {
            get => (sbyte)(m_Crossfader >> 8);
            set => m_Crossfader = (short)(value << 8);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct XInputTurntableLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedTurntableState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedTurntableButton.South, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedTurntableButton.East, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedTurntableButton.West, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedTurntableButton.North_Euphoria, displayName = "Y / Euphoria")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedTurntableButton.System, displayName = "Guide")]
        public TranslatedTurntableState state;
    }

    [InputControlLayout(stateType = typeof(XInputTurntableLayout), displayName = "XInput DJ Hero Turntable")]
    internal class XInputTurntable : TranslatingTurntable<XInputTurntableState>, IInputUpdateCallbackReceiver
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

        private XInputTurntableHaptics m_Haptics;

        /// <inheritdoc cref="IHaptics.PauseHaptics()"/>
        public override void PauseHaptics() => m_Haptics?.PauseHaptics();

        /// <inheritdoc cref="IHaptics.ResumeHaptics()"/>
        public override void ResumeHaptics() => m_Haptics?.ResumeHaptics();

        /// <inheritdoc cref="IHaptics.ResetHaptics()"/>
        public override void ResetHaptics() => m_Haptics?.ResetHaptics();

        /// <inheritdoc cref="ITurntableHaptics.SetEuphoriaBlink(bool)"/>
        public override void SetEuphoriaBlink(bool enable) => m_Haptics?.SetEuphoriaBlink(enable);

        void IInputUpdateCallbackReceiver.OnUpdate() => m_Haptics?.OnUpdate();
    }
}
