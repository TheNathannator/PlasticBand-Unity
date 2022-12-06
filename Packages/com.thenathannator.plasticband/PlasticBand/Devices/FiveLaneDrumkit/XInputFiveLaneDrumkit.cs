using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput 5-lane drumkits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputFiveLaneDrumkitState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "kick", layout = "Button", bit = 8)]
        [InputControl(name = "orangeCymbal", layout = "Button", bit = 9)]

        [InputControl(name = "greenPad", layout = "Button", bit = 12)]
        [InputControl(name = "redPad", layout = "Button", bit = 13)]
        [InputControl(name = "bluePad", layout = "Button", bit = 14)]
        [InputControl(name = "yellowCymbal", layout = "Button", bit = 15)]
        public ushort buttons;

        // TODO: Velocity mappings
        public byte leftTrigger;

        public byte rightTrigger;

        public short leftStickX;

        public short leftStickY;

        public short rightStickX;

        public short rightStickY;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput 5-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitState), displayName = "XInput 5-Lane Drumkit")]
    public class XInputFiveLaneDrumkit : FiveLaneDrumkit
    {
        internal new static void Initialize()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<XInputFiveLaneDrumkit>();
            // Sub-type disambiguation is registered by XInputFourLaneDrumkit
#endif
        }
    }
}
