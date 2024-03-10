using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/PS3.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    internal unsafe struct PS3GuitarHeroGuitarState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "yellowFret", layout = "Button", bit = 0)]
        [InputControl(name = "greenFret", layout = "Button", bit = 1)]
        [InputControl(name = "redFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [InputControl(name = "spPedal", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "systemButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        [FieldOffset(0)]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 0x1F)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=0x1F,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [FieldOffset(2)]
        public byte dpad;

        [InputControl(name = "whammy", layout = "IntAxis", parameters = "minValue=0x7F,maxValue=0xFF,zeroPoint=0x7F")]
        [FieldOffset(5)]
        public byte whammy;

        [InputControl(name = "touchGreen", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchRed", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchYellow", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchBlue", layout = "GuitarHeroSlider", format = "BYTE")]
        [InputControl(name = "touchOrange", layout = "GuitarHeroSlider", format = "BYTE")]

        [FieldOffset(6)]
        public byte slider;

        [InputControl(name = "tilt", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
        [FieldOffset(19)]
        public short tilt;

        [InputControl(name = "accelZ", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
        [FieldOffset(21)]
        public short accelZ;

        [InputControl(name = "accelY", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,invert")]
        [FieldOffset(23)]
        public short accelY;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3GuitarHeroGuitarState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS3GuitarHeroGuitarState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarState_NoReportId), displayName = "PlayStation 3 Guitar Hero Guitar")]
    internal class PS3GuitarHeroGuitar : GuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            // PS3 guitars
            HidLayoutFinder.RegisterLayout<PS3GuitarHeroGuitar_ReportId, PS3GuitarHeroGuitar>(0x12BA, 0x0100);

            // World Tour PC guitar
            HidLayoutFinder.RegisterLayout<PS3GuitarHeroGuitar_ReportId, PS3GuitarHeroGuitar>(0x1430, 0x474C);
        }
    }

    [InputControlLayout(stateType = typeof(PS3GuitarHeroGuitarState_ReportId), displayName = "PlayStation 3 Guitar Hero Guitar", hideInUI = true)]
    internal class PS3GuitarHeroGuitar_ReportId : PS3GuitarHeroGuitar { }
}
