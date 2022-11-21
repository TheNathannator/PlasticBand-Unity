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
    /// The state format for XInput drumkits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputFourLaneDrumkitState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)] // layout = "ExclusiveButton", format = "USHT", offset = 0, bit = 0, parameters = "bit=0, excludeMask=0x8200")]
        [InputControl(name = "dpad/down", bit = 1)] // layout = "ExclusiveButton", format = "USHT", offset = 0, bit = 0, parameters = "bit=1, excludeMask=0x4200")]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]
        [InputControl(name = "kick2", layout = "Button", bit = 6)]

        [InputControl(name = "kick1", layout = "Button", bit = 8)]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y")]

        [InputControl(name = "pads", layout = "FourLanePads", format = "USHT", offset = 0, bit = 0,
            parameters = "redBit=13,yellowBit=15,blueBit=14,greenBit=12,padBit=7,cymbalBit=9", displayName = "Kit")]
        public ushort buttons;

        private fixed byte unused[2];

        // TODO: More proper velocity mappings
        [InputControl(name = "redVelocity", layout = "Axis", displayName = "Red Velocity")]
        public short redVelocity;

        [InputControl(name = "yellowVelocity", layout = "Axis", displayName = "Yellow Velocity")]
        public short yellowVelocity;

        [InputControl(name = "blueVelocity", layout = "Axis", displayName = "Blue Velocity")]
        public short blueVelocity;

        [InputControl(name = "greenVelocity", layout = "Axis", displayName = "Green Velocity")]
        public short greenVelocity;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitState), displayName = "XInput 4-Lane Drumkit")]
    public class XInputFourLaneDrumkit : FourLaneDrumkit
    {
        internal new static void Initialize()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<XInputFourLaneDrumkit>();
            XInputLayoutFixup.RegisterLayoutResolver(XInputController.DeviceSubType.DrumKit, (state) => {
                if ((state.buttons & (ushort)XInputGamepad.Button.LeftThumb) != 0)
                    return typeof(XInputFiveLaneDrumkit).Name;

                return typeof(XInputFourLaneDrumkit).Name;
            });
#endif
        }
    }
}
