using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/PS3.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS3TurntableState_NoReportId : ITurntableState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        public PS3Button buttons;

        [FieldOffset(2)]
        public HidDpad dpad;

        [FieldOffset(5)]
        private byte m_LeftTableVelocity;

        [FieldOffset(6)]
        private byte m_RightTableVelocity;

        [FieldOffset(19)]
        private short m_EffectsDial;

        [FieldOffset(21)]
        private short m_Crossfader;

        [FieldOffset(23)]
        public short tableButtons;

        public bool west
        {
            get => (buttons & PS3Button.Square) != 0;
            set => buttons.SetBit(PS3Button.Square, value);
        }

        public bool south
        {
            get => (buttons & PS3Button.Cross) != 0;
            set => buttons.SetBit(PS3Button.Cross, value);
        }

        public bool east
        {
            get => (buttons & PS3Button.Circle) != 0;
            set => buttons.SetBit(PS3Button.Circle, value);
        }

        public bool north_euphoria
        {
            get => (buttons & PS3Button.Triangle) != 0;
            set => buttons.SetBit(PS3Button.Triangle, value);
        }

        public bool select
        {
            get => (buttons & PS3Button.Select) != 0;
            set => buttons.SetBit(PS3Button.Select, value);
        }

        public bool start
        {
            get => (buttons & PS3Button.Start) != 0;
            set => buttons.SetBit(PS3Button.Start, value);
        }

        public bool system
        {
            get => (buttons & PS3Button.PlayStation) != 0;
            set => buttons.SetBit(PS3Button.PlayStation, value);
        }

        public bool dpadUp
        {
            get => dpad.IsUp();
            set => dpad.SetUp(value);
        }

        public bool dpadRight
        {
            get => dpad.IsRight();
            set => dpad.SetRight(value);
        }

        public bool dpadDown
        {
            get => dpad.IsDown();
            set => dpad.SetDown(value);
        }

        public bool dpadLeft
        {
            get => dpad.IsLeft();
            set => dpad.SetLeft(value);
        }

        public bool rightGreen
        {
            get => (tableButtons & 0x001) != 0;
            set => tableButtons.SetBit(0x001, value);
        }

        public bool rightRed
        {
            get => (tableButtons & 0x002) != 0;
            set => tableButtons.SetBit(0x002, value);
        }

        public bool rightBlue
        {
            get => (tableButtons & 0x004) != 0;
            set => tableButtons.SetBit(0x004, value);
        }

        public bool leftGreen
        {
            get => (tableButtons & 0x010) != 0;
            set => tableButtons.SetBit(0x010, value);
        }

        public bool leftRed
        {
            get => (tableButtons & 0x020) != 0;
            set => tableButtons.SetBit(0x020, value);
        }

        public bool leftBlue
        {
            get => (tableButtons & 0x040) != 0;
            set => tableButtons.SetBit(0x040, value);
        }

        public sbyte leftVelocity
        {
            get => (sbyte)(m_LeftTableVelocity - 0x80);
            set => m_LeftTableVelocity = (byte)(value + 0x80);
        }

        public sbyte rightVelocity
        {
            get => (sbyte)(m_RightTableVelocity - 0x80);
            set => m_RightTableVelocity = (byte)(value + 0x80);
        }

        public ushort effectsDial
        {
            get => (ushort)(m_EffectsDial << 6);
            set => m_EffectsDial = (short)(value >> 6);
        }

        public sbyte crossfader
        {
            get => (sbyte)((m_Crossfader >> 2) - 0x80);
            set => m_Crossfader = (short)((value + 0x80) << 2);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3TurntableState_ReportId : ITurntableState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3TurntableState_NoReportId state;

        public bool west { get => state.west; set => state.west = value; }
        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool north_euphoria { get => state.north_euphoria; set => state.north_euphoria = value; }

        public bool select { get => state.select; set => state.select = value; }
        public bool start { get => state.start; set => state.start = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }

        public bool leftGreen { get => state.leftGreen; set => state.leftGreen = value; }
        public bool leftRed { get => state.leftRed; set => state.leftRed = value; }
        public bool leftBlue { get => state.leftBlue; set => state.leftBlue = value; }

        public bool rightGreen { get => state.rightGreen; set => state.rightGreen = value; }
        public bool rightRed { get => state.rightRed; set => state.rightRed = value; }
        public bool rightBlue { get => state.rightBlue; set => state.rightBlue = value; }

        public sbyte leftVelocity { get => state.leftVelocity; set => state.leftVelocity = value; }
        public sbyte rightVelocity { get => state.rightVelocity; set => state.rightVelocity = value; }

        public ushort effectsDial { get => state.effectsDial; set => state.effectsDial = value; }
        public sbyte crossfader { get => state.crossfader; set => state.crossfader = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PS3TurntableLayout : IInputStateTypeInfo
    {
        public FourCC format => TranslatedTurntableState.Format;

        [InputControl(name = "buttonSouth", layout = "Button", bit = (int)TranslatedTurntableButton.South, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = (int)TranslatedTurntableButton.East, displayName = "Circle")]
        [InputControl(name = "buttonWest", layout = "Button", bit = (int)TranslatedTurntableButton.West, displayName = "Square")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = (int)TranslatedTurntableButton.North_Euphoria, displayName = "Triangle / Euphoria")]

        [InputControl(name = "systemButton", layout = "Button", bit = (int)TranslatedTurntableButton.System, displayName = "PlayStation")]
        public TranslatedTurntableState state;
    }

    [InputControlLayout(stateType = typeof(PS3TurntableLayout), displayName = "PlayStation 3 DJ Hero Turntable")]
    internal class PS3Turntable : TranslatingTurntable<PS3TurntableState_NoReportId>, IInputUpdateCallbackReceiver
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3Turntable_ReportId, PS3Turntable>(0x12BA, 0x0140);
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            m_Haptics = new PS3TurntableHaptics(this);
        }

        private PS3TurntableHaptics m_Haptics;

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

    [InputControlLayout(stateType = typeof(PS3TurntableLayout), hideInUI = true)]
    internal class PS3Turntable_ReportId : TranslatingTurntable<PS3TurntableState_ReportId> { }
}
