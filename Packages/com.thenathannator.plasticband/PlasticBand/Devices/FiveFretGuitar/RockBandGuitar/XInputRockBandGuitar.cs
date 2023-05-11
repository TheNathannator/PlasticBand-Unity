using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputRockBandGuitarState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "strumUp", bit = 0)]
        [InputControl(name = "strumDown", bit = 1)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]

        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x1040")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x2040")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x8040")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x4040")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", bit = 0, parameters = "mask=0x0140")]
        public ushort buttons;

        // TODO: Define specific ranges for each of the notches
        [InputControl(name = "pickupSwitch", layout = "Axis", parameters = "scale=true,scaleFactor=4")]
        public byte pickupSwitch;

        public fixed byte unused[5];

        [InputControl(name = "whammy", layout = "Axis", defaultState = short.MinValue, parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;
    }

    [InputControlLayout(stateType = typeof(XInputRockBandGuitarState), displayName = "XInput Rock Band Guitar")]
    internal class XInputRockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputRockBandGuitar>(XInputController.DeviceSubType.Guitar);
        }
    }
}
