using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Keyboard/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiProKeyboardState_NoReportId : IProKeyboardState
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "1 Button", shortDisplayName = "1")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "A Button", shortDisplayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "B Button", shortDisplayName = "B")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "2 Button", shortDisplayName = "2")]

        [InputControl(name = "select", layout = "Button", bit = 8, displayName = "Minus", shortDisplayName = "-")]
        [InputControl(name = "start", layout = "Button", bit = 9, displayName = "Plus", shortDisplayName = "+")]

        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "System")]
        public PS3ProKeyboardState_NoReportId state;

        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool west { get => state.west; set => state.west = value; }
        public bool north { get => state.north; set => state.north = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool key1 { get => state.key1; set => state.key1 = value; }
        public bool key2 { get => state.key2; set => state.key2 = value; }
        public bool key3 { get => state.key3; set => state.key3 = value; }
        public bool key4 { get => state.key4; set => state.key4 = value; }
        public bool key5 { get => state.key5; set => state.key5 = value; }
        public bool key6 { get => state.key6; set => state.key6 = value; }
        public bool key7 { get => state.key7; set => state.key7 = value; }
        public bool key8 { get => state.key8; set => state.key8 = value; }
        public bool key9 { get => state.key9; set => state.key9 = value; }
        public bool key10 { get => state.key10; set => state.key10 = value; }
        public bool key11 { get => state.key11; set => state.key11 = value; }
        public bool key12 { get => state.key12; set => state.key12 = value; }
        public bool key13 { get => state.key13; set => state.key13 = value; }
        public bool key14 { get => state.key14; set => state.key14 = value; }
        public bool key15 { get => state.key15; set => state.key15 = value; }
        public bool key16 { get => state.key16; set => state.key16 = value; }
        public bool key17 { get => state.key17; set => state.key17 = value; }
        public bool key18 { get => state.key18; set => state.key18 = value; }
        public bool key19 { get => state.key19; set => state.key19 = value; }
        public bool key20 { get => state.key20; set => state.key20 = value; }
        public bool key21 { get => state.key21; set => state.key21 = value; }
        public bool key22 { get => state.key22; set => state.key22 = value; }
        public bool key23 { get => state.key23; set => state.key23 = value; }
        public bool key24 { get => state.key24; set => state.key24 = value; }
        public bool key25 { get => state.key25; set => state.key25 = value; }

        public bool overdrive { get => state.overdrive; set => state.overdrive = value; }
        public bool digitalPedal { get => state.digitalPedal; set => state.digitalPedal = value; }
        public byte analogPedal { get => state.analogPedal; set => state.analogPedal = value; }
        public byte touchStrip { get => state.touchStrip; set => state.touchStrip = value; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct WiiProKeyboardState_ReportId : IProKeyboardState
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public WiiProKeyboardState_NoReportId state;

        public bool south { get => state.south; set => state.south = value; }
        public bool east { get => state.east; set => state.east = value; }
        public bool west { get => state.west; set => state.west = value; }
        public bool north { get => state.north; set => state.north = value; }

        public bool dpadUp { get => state.dpadUp; set => state.dpadUp = value; }
        public bool dpadDown { get => state.dpadDown; set => state.dpadDown = value; }
        public bool dpadLeft { get => state.dpadLeft; set => state.dpadLeft = value; }
        public bool dpadRight { get => state.dpadRight; set => state.dpadRight = value; }

        public bool start { get => state.start; set => state.start = value; }
        public bool select { get => state.select; set => state.select = value; }
        public bool system { get => state.system; set => state.system = value; }

        public bool key1 { get => state.key1; set => state.key1 = value; }
        public bool key2 { get => state.key2; set => state.key2 = value; }
        public bool key3 { get => state.key3; set => state.key3 = value; }
        public bool key4 { get => state.key4; set => state.key4 = value; }
        public bool key5 { get => state.key5; set => state.key5 = value; }
        public bool key6 { get => state.key6; set => state.key6 = value; }
        public bool key7 { get => state.key7; set => state.key7 = value; }
        public bool key8 { get => state.key8; set => state.key8 = value; }
        public bool key9 { get => state.key9; set => state.key9 = value; }
        public bool key10 { get => state.key10; set => state.key10 = value; }
        public bool key11 { get => state.key11; set => state.key11 = value; }
        public bool key12 { get => state.key12; set => state.key12 = value; }
        public bool key13 { get => state.key13; set => state.key13 = value; }
        public bool key14 { get => state.key14; set => state.key14 = value; }
        public bool key15 { get => state.key15; set => state.key15 = value; }
        public bool key16 { get => state.key16; set => state.key16 = value; }
        public bool key17 { get => state.key17; set => state.key17 = value; }
        public bool key18 { get => state.key18; set => state.key18 = value; }
        public bool key19 { get => state.key19; set => state.key19 = value; }
        public bool key20 { get => state.key20; set => state.key20 = value; }
        public bool key21 { get => state.key21; set => state.key21 = value; }
        public bool key22 { get => state.key22; set => state.key22 = value; }
        public bool key23 { get => state.key23; set => state.key23 = value; }
        public bool key24 { get => state.key24; set => state.key24 = value; }
        public bool key25 { get => state.key25; set => state.key25 = value; }

        public bool overdrive { get => state.overdrive; set => state.overdrive = value; }
        public bool digitalPedal { get => state.digitalPedal; set => state.digitalPedal = value; }
        public byte analogPedal { get => state.analogPedal; set => state.analogPedal = value; }
        public byte touchStrip { get => state.touchStrip; set => state.touchStrip = value; }
    }

    [InputControlLayout(stateType = typeof(WiiProKeyboardState_NoReportId), displayName = "Wii Rock Band Pro Keyboard")]
    internal class WiiProKeyboard : ProKeyboard
    {
        internal new static void Initialize()
        {
            // MadCatz Rock Band 3 Pro Keyboard for Wii
            HidLayoutFinder.RegisterLayout<WiiProKeyboard_ReportId, WiiProKeyboard>(0x1BAD, 0x3330);

            // MadCatz Rock Band 3 MIDI Pro Adapter for Wii (Keys mode)
            HidLayoutFinder.RegisterLayout<WiiProKeyboard_ReportId, WiiProKeyboard>(0x1BAD, 0x3338);
        }
    }

    [InputControlLayout(stateType = typeof(WiiProKeyboardState_ReportId), displayName = "Wii Rock Band Pro Keyboard", hideInUI = true)]
    internal class WiiProKeyboard_ReportId : WiiProKeyboard { }
}