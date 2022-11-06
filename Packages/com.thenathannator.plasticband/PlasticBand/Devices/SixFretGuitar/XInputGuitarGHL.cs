using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput GHL devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputGuitarGHLState : IInputStateTypeInfo
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
        [InputControl(name = "ghtvButton", layout = "Button", bit = 6)]

        [InputControl(name = "white2", layout = "Button", bit = 8)]
        [InputControl(name = "white3", layout = "Button", bit = 9)]

        [InputControl(name = "black1", layout = "Button", bit = 12)]
        [InputControl(name = "black2", layout = "Button", bit = 13)]
        [InputControl(name = "white1", layout = "Button", bit = 14)]
        [InputControl(name = "black3", layout = "Button", bit = 15)]
        public ushort buttons;

        public fixed byte unused[6];

        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;

        [InputControl(name = "whammy", layout = "Axis", parameters = "normalize=true,normalizeMin=-1,normalizeMax=1,normalizeZero=-1")]
        public short whammy;
    }
}

namespace PlasticBand.Devices
{
    using XInputFlags = XInputController.DeviceFlags;

    /// <summary>
    /// An XInput GHL guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarGHLState), displayName = "XInput 6-Fret Guitar")]
    public class XInputGuitarGHL : SixFretGuitar
    {
        internal new static void Initialize()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<XInputGuitarGHL>(matches: new InputDeviceMatcher()
                .WithInterface("XInput")
                // Annoyingly, GHL guitars do not have a unique subtype. So, we have to use some other information to identify them.
                .WithCapability("subType", (int)XInputController.DeviceSubType.GuitarAlternate)
                // Strangely, they report having No Navigation. Most likely, none of the other guitars report this information,
                // so we use the flags as the distinguisher.
                .WithCapability("flags", (int)(XInputFlags.VoiceSupported | XInputFlags.PluginModulesSupported | XInputFlags.NoNavigation)) // 28
            );
#endif
        }
    }
}