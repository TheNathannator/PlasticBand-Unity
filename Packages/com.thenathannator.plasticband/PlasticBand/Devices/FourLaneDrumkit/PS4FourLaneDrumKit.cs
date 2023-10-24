using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/4-Lane%20Drums/PS4.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 77)]
    internal unsafe struct PS4FourLaneDrumkitState_NoReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]

        [InputControl(name = "buttonWest", layout = "Button", bit = 4, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 5, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 6, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 7, displayName = "Triangle")]

        [InputControl(name = "kick1", layout = "Button", bit = 8)]
        [InputControl(name = "kick2", layout = "Button", bit = 9)]

        [InputControl(name = "selectButton", layout = "Button", bit = 12)]
        [InputControl(name = "startButton", layout = "Button", bit = 13)]

        [FieldOffset(4)]
        public ushort buttons1;

        [InputControl(name = "systemButton", layout = "Button", bit = 0, displayName = "PlayStation")]
        [FieldOffset(6)]
        public byte buttons2;

        // TODO: Currently these just act like buttons, when velocity support is implemented for the other drumkits
        // this needs to be adjusted to match how those will then behave
        [InputControl(name = "redPad", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(42)]
        public byte redPadVelocity;

        [InputControl(name = "bluePad", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(43)]
        public byte bluePadVelocity;

        [InputControl(name = "yellowPad", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(44)]
        public byte yellowPadVelocity;

        [InputControl(name = "greenPad", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(45)]
        public byte greenPadVelocity;

        [InputControl(name = "yellowCymbal", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(46)]
        public byte yellowCymbalVelocity;

        [InputControl(name = "blueCymbal", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(47)]
        public byte blueCymbalVelocity;

        [InputControl(name = "greenCymbal", layout = "DiscreteButton", parameters = "minValue=1, maxValue=255")]
        [FieldOffset(48)]
        public byte greenCymbalVelocity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS4FourLaneDrumkitState_ReportId : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;
        public PS4FourLaneDrumkitState_NoReportId state;
    }

    [InputControlLayout(stateType = typeof(PS4FourLaneDrumkitState_ReportId), displayName = "PlayStation 4 Rock Band Drumkit")]
    internal class PS4FourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
            // MadCatz
            HidLayoutFinder.RegisterLayout<PS4FourLaneDrumkit, PS4FourLaneDrumkit_NoReportId>(0x0738, 0x8262, reportIdDefault: true);

            // PDP
            HidLayoutFinder.RegisterLayout<PS4FourLaneDrumkit, PS4FourLaneDrumkit_NoReportId>(0x0E6F, 0x0174, reportIdDefault: true);
        }
    }

    [InputControlLayout(stateType = typeof(PS4FourLaneDrumkitState_NoReportId), hideInUI = true)]
    internal class PS4FourLaneDrumkit_NoReportId : PS4FourLaneDrumkit { }
}
