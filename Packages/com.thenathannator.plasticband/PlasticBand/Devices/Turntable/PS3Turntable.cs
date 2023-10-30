using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.Haptics;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/PS3.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 27)]
    internal unsafe struct PS3TurntableState_NoReportId : ITurntableState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [FieldOffset(0)]
        public PS3Button buttons;

        [FieldOffset(2)]
        public HidDpad dpad;

        [FieldOffset(5)]
        private readonly byte m_LeftTableVelocity;

        [FieldOffset(6)]
        private readonly byte m_RightTableVelocity;

        [FieldOffset(19)]
        private readonly short m_EffectsDial;

        [FieldOffset(21)]
        private readonly short m_Crossfader;

        [FieldOffset(23)]
        public short tableButtons;

        public bool west => (buttons & PS3Button.Square) != 0;
        public bool south => (buttons & PS3Button.Cross) != 0;
        public bool east => (buttons & PS3Button.Circle) != 0;
        public bool north_euphoria => (buttons & PS3Button.Triangle) != 0;

        public bool select => (buttons & PS3Button.Select) != 0;
        public bool start => (buttons & PS3Button.Start) != 0;
        public bool system => (buttons & PS3Button.PlayStation) != 0;

        public bool dpadUp => dpad.IsUp();
        public bool dpadRight => dpad.IsRight();
        public bool dpadDown => dpad.IsDown();
        public bool dpadLeft => dpad.IsLeft();

        public bool rightGreen => (tableButtons & 0x001) != 0;
        public bool rightRed => (tableButtons & 0x002) != 0;
        public bool rightBlue => (tableButtons & 0x004) != 0;

        public bool leftGreen => (tableButtons & 0x010) != 0;
        public bool leftRed => (tableButtons & 0x020) != 0;
        public bool leftBlue => (tableButtons & 0x040) != 0;

        public sbyte leftVelocity => (sbyte)(m_LeftTableVelocity - 0x80);
        public sbyte rightVelocity => (sbyte)(m_RightTableVelocity - 0x80);

        public sbyte effectsDial => (sbyte)((m_EffectsDial >> 2) - 0x80);
        public sbyte crossfader => (sbyte)((m_Crossfader >> 2) - 0x80);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3TurntableState_ReportId : ITurntableState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3TurntableState_NoReportId state;

        public bool west => state.west;
        public bool south => state.south;
        public bool east => state.east;
        public bool north_euphoria => state.north_euphoria;

        public bool select => state.select;
        public bool start => state.start;
        public bool system => state.system;

        public bool dpadUp => state.dpadUp;
        public bool dpadRight => state.dpadRight;
        public bool dpadDown => state.dpadDown;
        public bool dpadLeft => state.dpadLeft;

        public bool leftGreen => state.leftGreen;
        public bool leftRed => state.leftRed;
        public bool leftBlue => state.leftBlue;

        public bool rightGreen => state.rightGreen;
        public bool rightRed => state.rightRed;
        public bool rightBlue => state.rightBlue;

        public sbyte leftVelocity => state.leftVelocity;
        public sbyte rightVelocity => state.rightVelocity;

        public sbyte effectsDial => state.effectsDial;
        public sbyte crossfader => state.crossfader;
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
    internal class PS3Turntable : TranslatingTurntable<PS3TurntableState_NoReportId>
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
    }

    [InputControlLayout(stateType = typeof(PS3TurntableLayout), hideInUI = true)]
    internal class PS3Turntable_ReportId : TranslatingTurntable<PS3TurntableState_ReportId> { }
}
