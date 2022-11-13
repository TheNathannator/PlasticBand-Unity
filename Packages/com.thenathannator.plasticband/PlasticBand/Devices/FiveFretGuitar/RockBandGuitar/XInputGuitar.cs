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
    /// The state format for XInput Guitar devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0, displayName = "Up/Strum Up", alias = "strumUp")]
        [InputControl(name = "dpad/down", bit = 1, displayName = "Down/Strum Down", alias = "strumDown")]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "orangeFret", layout = "Button", bit = 8)]

        [InputControl(name = "greenFret", layout = "Button", bit = 12)]
        [InputControl(name = "redFret", layout = "Button", bit = 13)]
        [InputControl(name = "blueFret", layout = "Button", bit = 14)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 15)]

        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x1040")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x2040")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x8040")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x4040")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0140")]
        public ushort buttons;

        [InputControl(name = "pickupSwitch", layout = "Axis", parameters = "scale=true,scaleFactor=4")]
        public byte pickupSwitch;

        public fixed byte unused[5];

        [InputControl(name = "whammy", layout = "Axis", parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarState), displayName = "XInput Guitar")]
    public class XInputGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputGuitar>(XInputController.DeviceSubType.Guitar);
        }
    }
}
