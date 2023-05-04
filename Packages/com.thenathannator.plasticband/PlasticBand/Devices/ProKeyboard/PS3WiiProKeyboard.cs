using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/ProKeyboard/PS3%20and%20Wii.md

namespace PlasticBand.Devices.LowLevel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiProKeyboardState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle")]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        private fixed byte unused1[2];

        [InputControl(name = "key1",  layout = "Button", bit = 7)]
        [InputControl(name = "key2",  layout = "Button", bit = 6)]
        [InputControl(name = "key3",  layout = "Button", bit = 5)]
        [InputControl(name = "key4",  layout = "Button", bit = 4)]
        [InputControl(name = "key5",  layout = "Button", bit = 3)]
        [InputControl(name = "key6",  layout = "Button", bit = 2)]
        [InputControl(name = "key7",  layout = "Button", bit = 1)]
        [InputControl(name = "key8",  layout = "Button", bit = 0)]
        public byte keys1;

        [InputControl(name = "key9",  layout = "Button", bit = 7)]
        [InputControl(name = "key10", layout = "Button", bit = 6)]
        [InputControl(name = "key11", layout = "Button", bit = 5)]
        [InputControl(name = "key12", layout = "Button", bit = 4)]
        [InputControl(name = "key13", layout = "Button", bit = 3)]
        [InputControl(name = "key14", layout = "Button", bit = 2)]
        [InputControl(name = "key15", layout = "Button", bit = 1)]
        [InputControl(name = "key16", layout = "Button", bit = 0)]
        public byte keys2;

        [InputControl(name = "key17", layout = "Button", bit = 7)]
        [InputControl(name = "key18", layout = "Button", bit = 6)]
        [InputControl(name = "key19", layout = "Button", bit = 5)]
        [InputControl(name = "key20", layout = "Button", bit = 4)]
        [InputControl(name = "key21", layout = "Button", bit = 3)]
        [InputControl(name = "key22", layout = "Button", bit = 2)]
        [InputControl(name = "key23", layout = "Button", bit = 1)]
        [InputControl(name = "key24", layout = "Button", bit = 0)]
        public byte keys3;

        [InputControl(name = "key25", layout = "Button", bit = 7)]
        // TODO: Try to pair velocities with keys
        [InputControl(name = "velocity1", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity1;

        [InputControl(name = "velocity2", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity2;

        [InputControl(name = "velocity3", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity3;

        [InputControl(name = "velocity4", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity4;

        [InputControl(name = "velocity5", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity5;

        [InputControl(name = "overdrive", layout = "Button", bit = 7)]
        public byte overdrive;

        // TODO: The normalization here needs verification
        [InputControl(name = "analogPedal", layout = "Axis", format = "BIT", sizeInBits = 7, parameters = "normalize,normalizeMin=1,normalizeMax=0,normalizeZero=1")]
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte pedal;

        [InputControl(name = "touchStrip", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte touchStrip;

        private fixed byte unused2[11];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiProKeyboardState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3WiiProKeyboardState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3WiiProKeyboardState_NoReportId), hideInUI = true)]
    internal class PS3ProKeyboard_NoReportId : PS3ProKeyboard { }

    [InputControlLayout(stateType = typeof(PS3WiiProKeyboardState_ReportId), hideInUI = true)]
    internal class PS3ProKeyboard_ReportId : PS3ProKeyboard { }

    [InputControlLayout(stateType = typeof(PS3WiiProKeyboardState_NoReportId), hideInUI = true)]
    internal class WiiProKeyboard_NoReportId : WiiProKeyboard { }

    [InputControlLayout(stateType = typeof(PS3WiiProKeyboardState_ReportId), hideInUI = true)]
    internal class WiiProKeyboard_ReportId : WiiProKeyboard { }
}

namespace PlasticBand.Devices
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || ((UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && HIDROGEN_FORCE_REPORT_IDS)
    using DefaultState = PS3WiiProKeyboardState_ReportId;
#else
    using DefaultState = PS3WiiProKeyboardState_NoReportId;
#endif

    /// <summary>
    /// A PS3 Pro Keyboard.
    /// </summary>
    [InputControlLayout(stateType = typeof(DefaultState), displayName = "PlayStation 3 Rock Band Pro Keyboard")]
    public class PS3ProKeyboard : ProKeyboard
    {
        /// <summary>
        /// The current <see cref="PS3ProKeyboard"/>.
        /// </summary>
        public static new PS3ProKeyboard current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3ProKeyboard"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3ProKeyboard> all => s_AllDevices;
        private static readonly List<PS3ProKeyboard> s_AllDevices = new List<PS3ProKeyboard>();

        internal new static void Initialize()
        {
            // ProKeyboard
            HidReportIdLayoutFinder.RegisterLayout<PS3ProKeyboard,
                PS3ProKeyboard_ReportId, PS3ProKeyboard_NoReportId>(0x12BA, 0x2330);

            // MIDI Pro Adapter
            HidReportIdLayoutFinder.RegisterLayout<PS3ProKeyboard,
                PS3ProKeyboard_ReportId, PS3ProKeyboard_NoReportId>(0x12BA, 0x2338);
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3ProKeyboard"/>.
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
    /// A Wii Pro Keyboard.
    /// </summary>
    [InputControlLayout(stateType = typeof(DefaultState), displayName = "Wii Rock Band Pro Keyboard")]
    public class WiiProKeyboard : ProKeyboard
    {
        /// <summary>
        /// The current <see cref="WiiProKeyboard"/>.
        /// </summary>
        public static new WiiProKeyboard current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="WiiProKeyboard"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<WiiProKeyboard> all => s_AllDevices;
        private static readonly List<WiiProKeyboard> s_AllDevices = new List<WiiProKeyboard>();

        internal new static void Initialize()
        {
            // ProKeyboard
            HidReportIdLayoutFinder.RegisterLayout<WiiProKeyboard,
                WiiProKeyboard_ReportId, WiiProKeyboard_NoReportId>(0x1BAD, 0x2330);

            // MIDI Pro Adapter
            HidReportIdLayoutFinder.RegisterLayout<WiiProKeyboard,
                WiiProKeyboard_ReportId, WiiProKeyboard_NoReportId>(0x1BAD, 0x2330);
        }

        /// <summary>
        /// Sets this device as the current <see cref="WiiProKeyboard"/>.
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
