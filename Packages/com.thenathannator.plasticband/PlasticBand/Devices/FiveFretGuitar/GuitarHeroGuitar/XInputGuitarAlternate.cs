using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput GuitarAlternate devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarAlternateState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]
        [InputControl(name = "strumUp", layout = "Button", bit = 0)]
        [InputControl(name = "strumDown", layout = "Button", bit = 1)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]
        [InputControl(name = "spPedal", layout = "Button", bit = 9)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]
        public ushort buttons;

        [InputControl(name = "accel1", layout = "Axis", noisy = true)]
        public byte accel1;

        [InputControl(name = "accel2", layout = "Axis", noisy = true)]
        public byte accel2;

        // TODO
        // [InputControl(name = "touchGreen", layout = "Button")]
        // [InputControl(name = "touchRed", layout = "Button")]
        // [InputControl(name = "touchYellow", layout = "Button")]
        // [InputControl(name = "touchBlue", layout = "Button")]
        // [InputControl(name = "touchOrange", layout = "Button")]
        [InputControl(name = "sliderBar", layout = "Integer", format = "BYTE")] // The top and bottom bytes are the same, only one is needed
        public short slider;

        private short unused;

        [InputControl(name = "whammy", layout = "Axis", parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        [InputControl(name = "tilt", layout = "Axis")]
        public short tilt;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput GuitarAlternate guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarAlternateState), displayName = "XInput Guitar Alternate")]
    public class XInputGuitarAlternate : GuitarHeroGuitar
    {
        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputGuitarAlternate>(XInputController.DeviceSubType.GuitarAlternate);
        }
    }
}
