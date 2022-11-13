using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS3 Guitar Hero guitars.
    /// </summary>
    // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-rockband.html
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3RockBandGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D');
    
        [InputControl(name = "yellowFret", layout = "Button", bit = 0)]
        [InputControl(name = "redFret", layout = "Button", bit = 1)]
        [InputControl(name = "greenFret", layout = "Button", bit = 2)]
        [InputControl(name = "blueFret", layout = "Button", bit = 3)]

        [InputControl(name = "orangeFret", layout = "Button", bit = 4)]
        [InputControl(name = "tilt", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9, displayName = "Start")]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]

        // TODO: These are almost certainly not correct, they're required for RockBandGuitar so they're set here for now
        // Currently the solo flag is set to L1 (bit 6)
        [InputControl(name = "soloGreen", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0044")]
        [InputControl(name = "soloRed", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0042")]
        [InputControl(name = "soloYellow", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0041")]
        [InputControl(name = "soloBlue", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0048")]
        [InputControl(name = "soloOrange", layout = "MaskButton", format = "USHT", offset = 0, bit = 0, parameters = "mask=0x0050")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=0x1F,wrapAtValue=7", alias = "strumUp", displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", alias = "strumDown", displayName = "Down/Strum Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        private fixed byte unused1[2];

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        [InputControl(name = "pickupSwitch", layout = "Axis")]
        public byte pickupSwitch;

        private fixed byte unused2[21];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 Rock Band guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3RockBandGuitarState), displayName = "Harmonix Guitar for PlayStation(R)3")]
    public class PS3RockBandGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3RockBandGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-rockband.html#vid-and-pid
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x0200) // "Harmonix Guitar for PlayStation(R)3"
            );
        }
    }
}