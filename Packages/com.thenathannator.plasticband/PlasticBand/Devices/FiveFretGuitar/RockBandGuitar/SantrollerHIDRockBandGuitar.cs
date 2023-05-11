using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Santroller.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct SantrollerHIDRockBandGuitarState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "blueFret", layout = "Button", bit = 0)]
        [InputControl(name = "greenFret", layout = "Button", bit = 1)]
        [InputControl(name = "redFret", layout = "Button", bit = 2)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 3)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", offset = 1, bit = 0, parameters = "mask=0x0042")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", offset = 1, bit = 0, parameters = "mask=0x0044")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", offset = 1, bit = 0, parameters = "mask=0x0048")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", offset = 1, bit = 0, parameters = "mask=0x0041")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", offset = 1, bit = 0, parameters = "mask=0x0050")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "strumUp", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "strumDown", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, defaultState = 8, parameters = "minValue=3,maxValue=5,nullValue=8")]
        public byte dpad;

        [InputControl(name = "tilt", layout = "Axis")]
        public byte tilt;

        public byte unused1;

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        // TODO: Define specific ranges for each of the notches
        [InputControl(name = "pickupSwitch", layout = "Axis")]
        public byte pickupSwitch;

        public fixed byte unused2[21];
    }

    [InputControlLayout(stateType = typeof(SantrollerHIDRockBandGuitarState), displayName = "Santroller HID Rock Band Guitar")]
    internal class SantrollerHIDRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterHIDLayout<SantrollerHIDRockBandGuitar>(
                SantrollerDeviceType.Guitar, SantrollerRhythmType.RockBand);
        }
    }
}
