using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/PS3.md

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3TurntableState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle / Euphoria")]

        [InputControl(name = "euphoria", layout = "Button", bit = 3, displayName = "Euphoria")]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 15)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=15,wrapAtValue=7", displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", displayName = "Down/Strum Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        public fixed byte unused1[2];

        [InputControl(name = "leftTableVelocity", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte leftTableVelocity;

        [InputControl(name = "rightTableVelocity", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte rightTableVelocity;

        public fixed byte unused2[12];

        [InputControl(name = "effectsDial", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short effectsDial;

        [InputControl(name = "crossFader", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short crossFader;

        [InputControl(name = "rightTableGreen", layout = "Button", bit = 0)]
        [InputControl(name = "rightTableRed", layout = "Button", bit = 1)]
        [InputControl(name = "rightTableBlue", layout = "Button", bit = 2)]
        [InputControl(name = "leftTableGreen", layout = "Button", bit = 3)]
        [InputControl(name = "leftTableRed", layout = "Button", bit = 4)]
        [InputControl(name = "leftTableBlue", layout = "Button", bit = 5)]
        public short tableButtons;

        public short unused3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3TurntableState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3TurntableState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3TurntableState_NoReportId), hideInUI = true)]
    internal class PS3Turntable_NoReportId : PS3Turntable { }

    [InputControlLayout(stateType = typeof(PS3TurntableState_ReportId), hideInUI = true)]
    internal class PS3Turntable_ReportId : PS3Turntable { }
}

namespace PlasticBand.Devices
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && HIDROGEN_FORCE_REPORT_IDS)
    using DefaultState = PS3TurntableState_ReportId;
#else
    using DefaultState = PS3TurntableState_NoReportId;
#endif

    [InputControlLayout(stateType = typeof(DefaultState), displayName = "PlayStation 3 DJ Hero Turntable")]
    internal class PS3Turntable : Turntable
    {
        internal new static void Initialize()
        {
            HidReportIdLayoutFinder.RegisterLayout<PS3Turntable,
                PS3Turntable_ReportId, PS3Turntable_NoReportId>(0x12BA, 0x0140);
        }

        /// <summary>
        /// Enables the euphoria light on the turntable.
        /// </summary>
        private static readonly PS3OutputCommand s_EuphoriaOnCommand = new PS3OutputCommand(
            0x91,
            new byte[PS3OutputCommand.kDataSize] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        /// <summary>
        /// Disables the euphoria light on the turntable.
        /// </summary>
        private static readonly PS3OutputCommand s_EuphoriaOffCommand = new PS3OutputCommand(
            0x91,
            new byte[PS3OutputCommand.kDataSize] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        /// <summary>
        /// The previous brightness of the euphoria effect.
        /// Used for determining direction.
        /// </summary>
        private float m_PreviousBrightness = -1;

        /// <summary>
        /// The current direction of the euphoria effect.
        /// </summary>
        /// <remarks>
        /// true == up, false == down.
        /// </remarks>
        private bool m_Direction;

        protected override void OnEuphoriaTick(float brightness)
        {
            PS3OutputCommand command;
            // Force-disable
            if (brightness < 0)
            {
                m_Direction = false;
                command = s_EuphoriaOffCommand;
            }
            // Enable at the start of an increase
            else if ((brightness > m_PreviousBrightness) && !m_Direction)
            {
                m_Direction = true;
                command = s_EuphoriaOnCommand;
            }
            // Disable at the start of a decrease
            else if ((brightness < m_PreviousBrightness) && m_Direction)
            {
                m_Direction = false;
                command = s_EuphoriaOffCommand;
            }
            // Direction hasn't changed, don't send the same one multiple times
            else
            {
                m_PreviousBrightness = brightness;
                return;
            }

            m_PreviousBrightness = brightness;

            // Send command
            this.ExecuteCommand(ref command);
        }
    }
}
