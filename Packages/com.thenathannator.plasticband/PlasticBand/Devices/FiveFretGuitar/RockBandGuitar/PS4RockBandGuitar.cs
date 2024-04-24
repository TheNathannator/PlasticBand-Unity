using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS4RockBandGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "selectButton", layout = "Button", bit = 12)]
        [InputControl(name = "startButton", layout = "Button", bit = 13)]

        [FieldOffset(4)]
        public ushort buttons1;

        [InputControl(name = "systemButton", layout = "Button", bit = 0, displayName = "PlayStation")]
        [FieldOffset(6)]
        public byte buttons2;

        [InputControl(name = "pickupSwitch", layout = "Integer")]
        [FieldOffset(42)]
        public byte pickupSwitch;

        [InputControl(name = "whammy", layout = "Axis")]
        [FieldOffset(43)]
        public byte whammy;

        [InputControl(name = "tilt", layout = "Axis")]
        [FieldOffset(44)]
        public byte tilt;

        [InputControl(name = "greenFret", layout = "Button", bit = 0)]
        [InputControl(name = "redFret", layout = "Button", bit = 1)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [FieldOffset(45)]
        public byte frets;

        [InputControl(name = "soloGreen", layout = "Button", bit = 0)]
        [InputControl(name = "soloRed", layout = "Button", bit = 1)]
        [InputControl(name = "soloYellow", layout = "Button", bit = 2)]
        [InputControl(name = "soloBlue", layout = "Button", bit = 3)]
        [InputControl(name = "soloOrange", layout = "Button", bit = 4)]
        [FieldOffset(46)]
        public byte soloFrets;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4RockBandGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4RockBandGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS4RockBandGuitarState_NoReportId), displayName = "PlayStation 4 Rock Band Guitar", hideInUI = true)]
    internal class PS4RockBandGuitar_NoReportId : PS4RockBandGuitar { }

    [InputControlLayout(stateType = typeof(PS4RockBandGuitarState_ReportId), displayName = "PlayStation 4 Rock Band Guitar")]
    internal class PS4RockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            // Stratocaster
            HidLayoutFinder.RegisterLayout<PS4RockBandGuitar, PS4RockBandGuitar_NoReportId>(0x0738, 0x8261, reportIdDefault: true);

            // Jaguar
            HidLayoutFinder.RegisterLayout<PS4RockBandGuitar, PS4RockBandGuitar_NoReportId>(0x0E6F, 0x0173, reportIdDefault: true);
        }
    }
}
