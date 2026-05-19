using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarHeroGuitarState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]
        [InputControl(name = "spPedal", layout = "Button", bit = 9)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]
        public ushort buttons;

        // Was gonna use these parameters based on my Les Paul, but maybe it would be best to
        // leave that up to calibration systems rather than assuming all guitars will be the same
        // parameters = "normalize=true,normalizeMin=0.1,normalizeMax=0.6,normalizeZero=0.36,clamp=2,clampMin=-1,clampMax=1"
        [InputControl(name = "accelY", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize=true,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte accelY;

        // parameters = "normalize=true,normalizeMin=0.2,normalizeMax=0.75,normalizeZero=0.45,clamp=2,clampMin=-1,clampMax=1"
        [InputControl(name = "accelZ", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize=true,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte accelZ;

        [InputControl(name = "touchGreen", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchRed", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchYellow", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchBlue", layout = "GuitarHeroSlider", format = "SHRT")]
        [InputControl(name = "touchOrange", layout = "GuitarHeroSlider", format = "SHRT")]
        public short slider;

        public short unused;

        [InputControl(name = "whammy", layout = "Axis", defaultState = short.MinValue, parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        // parameters = "normalize=true,normalizeMin=-0.85,normalizeMax=1,normalizeZero=0,clamp=2,clampMin=-1,clampMax=1"
        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;
    }

    [InputControlLayout(stateType = typeof(XInputGuitarHeroGuitarState), displayName = "XInput Guitar Hero Guitar")]
    internal class XInputGuitarHeroGuitar : GuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputGuitarHeroGuitar>(XInputController.DeviceSubType.GuitarAlternate);
        }
    }
}
