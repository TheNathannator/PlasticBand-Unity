using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS3/PS4/Wii U GHL devices.
    /// </summary>
    // https://github.com/ghlre/GHLtarUtility/blob/master/PS3Guitar.cs
    // Also https://sanjay900.github.io/guitar-configurator/controller-reverse-engineering/ps3-controllers.html for more general format
    // guidance and some additional inputs, as this does follow the same layout as other PS3 controllers
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PSWiiUSixFretGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('H', 'I', 'D');

        [InputControl(name = "white1", layout = "Button", bit = 0)]
        [InputControl(name = "black1", layout = "Button", bit = 1)]
        [InputControl(name = "black2", layout = "Button", bit = 2)]
        [InputControl(name = "black3", layout = "Button", bit = 3)]

        [InputControl(name = "white2", layout = "Button", bit = 4)]
        [InputControl(name = "white3", layout = "Button", bit = 5)]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]
        [InputControl(name = "ghtvButton", layout = "Button", bit = 10)]

        [InputControl(name = "syncButton", layout = "Button", bit = 12, displayName = "D-pad Center")]
        public ushort buttons;

        [InputControl(name = "dpad", format = "BIT", layout = "Dpad", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", format = "BIT", layout = "DiscreteButton", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", format = "BIT", layout = "DiscreteButton", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", format = "BIT", layout = "DiscreteButton", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", format = "BIT", layout = "DiscreteButton", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        private byte unused1;

        [InputControl(name = "strumUp", layout = "Button", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0,clampMax=0.5,invert")]
        [InputControl(name = "strumDown", layout = "Button", parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5,clamp=2,clampMin=0.5,clampMax=1,invert=false")]
        public byte strumBar;

        private byte unused2;

        [InputControl(name = "whammy", layout = "Axis")]
        public byte whammy;

        private fixed byte unused3[12];

        // TODO: Verify whether or not additional processing needs to happen
        [InputControl(name = "tilt", layout = "Axis", noisy = true)]
        public short tilt;

        private fixed short unused4[3];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3/Wii U GHL guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(PSWiiUSixFretGuitarState), displayName = "PS3/Wii U 6-Fret Guitar")]
    public class PS3WiiUSixFretGuitar : PokedSixFretGuitar
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3WiiUSixFretGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // https://github.com/evilynux/hid-ghlive-dkms/blob/main/hid-ghlive/src/hid-ids.h
                // https://github.com/evilynux/hid-ghlive-dkms/blob/main/hid-ghlive/src/hid-ghlive.c#L194
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x074B) // (Not registered)
            );
        }

        // Magic data to be sent periodically to unlock full input data.
        // https://github.com/ghlre/GHLtarUtility/blob/master/PS3Guitar.cs#L104
        // These also seem to work, given their use in other projects:
        // [0x02, 0x08, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00] (https://github.com/evilynux/hid-ghlive-dkms/blob/main/hid-ghlive/src/hid-ghlive.c#L32)
        // [0x02, 0x02, 0x08, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00] (https://github.com/Octave13/GHLPokeMachine/blob/master/GHL_Library/GHLPoke.h#L25)
        private static byte[] pokeData = new byte[SixFretHidPokeCommand.DataSize] { 0x02, 0x08, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private static SixFretHidPokeCommand pokeCommand = SixFretHidPokeCommand.Create(pokeData);

        protected override void OnPoke() => device.ExecuteCommand(ref pokeCommand);
    }

    /// <summary>
    /// A PS4 GHL guitar.
    /// </summary>
    // TODO: The control layout could differ, it's assumed it's the same as the PS3/Wii U guitar
    [InputControlLayout(stateType = typeof(PSWiiUSixFretGuitarState), displayName = "PS4 6-Fret Guitar")]
    public class PS4SixFretGuitar : PokedSixFretGuitar
    {
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS4SixFretGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // https://github.com/evilynux/hid-ghlive-dkms/blob/main/hid-ghlive/src/hid-ids.h
                // https://github.com/evilynux/hid-ghlive-dkms/blob/main/hid-ghlive/src/hid-ghlive.c#L196
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x1430) // RedOctane
                .WithCapability("productId", 0x07BB) // (Not registered)
            );
        }

        // Magic data to be sent periodically to unlock full input data.
        // https://github.com/evilynux/hid-ghlive-dkms/blob/main/hid-ghlive/src/hid-ghlive.c#L37
        private static byte[] pokeData = new byte[SixFretHidPokeCommand.DataSize] { 0x30, 0x02, 0x08, 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private static SixFretHidPokeCommand pokeCommand = SixFretHidPokeCommand.Create(pokeData);

        protected override void OnPoke() => device.ExecuteCommand(ref pokeCommand);
    }
}