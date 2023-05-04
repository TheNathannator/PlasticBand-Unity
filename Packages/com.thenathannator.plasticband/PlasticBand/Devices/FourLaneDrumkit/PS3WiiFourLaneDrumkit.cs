using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS3%20and%20Wii.md

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiFourLaneDrumkitState_NoReportId : IInputStateTypeInfo
    {
        const string kPadParameters = "redBit=2,yellowBit=3,blueBit=0,greenBit=1,padBit=10,cymbalBit=11";
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle")]

        [InputControl(name = "kick1", layout = "Button", bit = 4)]
        [InputControl(name = "kick2", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        [InputControl(name = "redPad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "yellowPad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "bluePad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "greenPad", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "yellowCymbal", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "blueCymbal", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        [InputControl(name = "greenCymbal", layout = "FourLanePads", format = "USHT", bit = 0, parameters = kPadParameters)]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        // TODO: D-pad up/down should be ignored when hitting yellow or blue cymbal, but
        // it needs to be ignored without interfering with pad detection
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7", displayName = "Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", displayName = "Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        public fixed byte unused2[8];

        // TODO:
        // - Hardware verification
        // - Input ranges have yet to be determined
        // - Try and pair velocity with pads directly
        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Velocity")]
        public byte yellowVelocity;

        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Velocity")]
        public byte redVelocity;

        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Velocity")]
        public byte greenVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Velocity")]
        public byte blueVelocity;

        public fixed byte unused3[12];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiFourLaneDrumkitState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiFourLaneDrumkitState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState_NoReportId), hideInUI = true)]
    internal class PS3FourLaneDrumkit_NoReportId : PS3FourLaneDrumkit { }

    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState_ReportId), hideInUI = true)]
    internal class PS3FourLaneDrumkit_ReportId : PS3FourLaneDrumkit { }

    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState_NoReportId), hideInUI = true)]
    internal class WiiFourLaneDrumkit_NoReportId : WiiFourLaneDrumkit { }

    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState_ReportId), hideInUI = true)]
    internal class WiiFourLaneDrumkit_ReportId : WiiFourLaneDrumkit { }
}

namespace PlasticBand.Devices
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && HIDROGEN_FORCE_REPORT_IDS)
    using DefaultState = PS3WiiFourLaneDrumkitState_ReportId;
#else
    using DefaultState = PS3WiiFourLaneDrumkitState_NoReportId;
#endif

    /// <summary>
    /// A PS3 4-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(DefaultState), displayName = "PlayStation 3 Rock Band Drumkit")]
    public class PS3FourLaneDrumkit : FourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="PS3FourLaneDrumkit"/>.
        /// </summary>
        public static new PS3FourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3FourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3FourLaneDrumkit> all => s_AllDevices;
        private static readonly List<PS3FourLaneDrumkit> s_AllDevices = new List<PS3FourLaneDrumkit>();

        internal new static void Initialize()
        {
            // Drumkit
            HidReportIdLayoutFinder.RegisterLayout<PS3FourLaneDrumkit,
                PS3FourLaneDrumkit_ReportId, PS3FourLaneDrumkit_NoReportId>(0x12BA, 0x0210);

            // MIDI Pro Adapter
            HidReportIdLayoutFinder.RegisterLayout<PS3FourLaneDrumkit,
                PS3FourLaneDrumkit_ReportId, PS3FourLaneDrumkit_NoReportId>(0x12BA, 0x0218);
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3FourLaneDrumkit"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }
    }

    /// <summary>
    /// A Wii 4-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(DefaultState), displayName = "Wii Rock Band Drumkit")]
    public class WiiFourLaneDrumkit : FourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="WiiFourLaneDrumkit"/>.
        /// </summary>
        public static new WiiFourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="WiiFourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<WiiFourLaneDrumkit> all => s_AllDevices;
        private static readonly List<WiiFourLaneDrumkit> s_AllDevices = new List<WiiFourLaneDrumkit>();

        internal new static void Initialize()
        {
            // RB1
            HidReportIdLayoutFinder.RegisterLayout<WiiFourLaneDrumkit,
                WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit_NoReportId>(0x1BAD, 0x0005);

            // RB2 and later
            HidReportIdLayoutFinder.RegisterLayout<WiiFourLaneDrumkit,
                WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit_NoReportId>(0x1BAD, 0x3110);

            // MIDI Pro Adapter
            HidReportIdLayoutFinder.RegisterLayout<WiiFourLaneDrumkit,
                WiiFourLaneDrumkit_ReportId, WiiFourLaneDrumkit_NoReportId>(0x1BAD, 0x3118);
        }

        /// <summary>
        /// Sets this device as the current <see cref="WiiFourLaneDrumkit"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }
    }
}
