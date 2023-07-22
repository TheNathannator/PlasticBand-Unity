using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS3%20and%20Wii.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3RockBandGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "blueFret", layout = "Button", bit = 0)]
        [InputControl(name = "greenFret", layout = "Button", bit = 1)]
        [InputControl(name = "redFret", layout = "Button", bit = 2)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 3)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [InputControl(name = "tilt", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x0042")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x0044")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x0048")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x0041")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x0050")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=3,maxValue=5,nullValue=8")]
        public byte dpad;

        public fixed byte unused1[2];

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        [InputControl(name = "pickupSwitch", layout = "RockBandPickupSwitch")]
        public byte pickupSwitch;

        public fixed byte unused2[21];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3RockBandGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3RockBandGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3RockBandGuitarState_NoReportId), displayName = "PlayStation 3 Rock Band Guitar")]
    internal class PS3RockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            HidLayoutFinder.RegisterLayout<PS3RockBandGuitar_ReportId, PS3RockBandGuitar>(0x12BA, 0x0200);
        }
    }

    [InputControlLayout(stateType = typeof(PS3RockBandGuitarState_ReportId), hideInUI = true)]
    internal class PS3RockBandGuitar_ReportId : PS3RockBandGuitar { }
}
