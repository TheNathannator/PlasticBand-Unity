using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput Pro Guitar devices.
    /// </summary>
    // https://dwsk.proboards.com/thread/404/song-standard-advancements
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputProGuitarState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('X', 'I', 'N', 'P');

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        // Tilt doesn't seem to be available through XInput, but it has to be there for ProGuitar
        // Dummying it out as a button here, just about all the axis bits are taken already
        [InputControl(name = "tilt", layout = "Button", bit = 8)]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y")]
        public ushort buttons;

        [InputControl(name = "fret1", layout = "Integer", format = "BIT", offset = 2, bit = 0,  sizeInBits = 5)]
        [InputControl(name = "fret2", layout = "Integer", format = "BIT", offset = 2, bit = 5,  sizeInBits = 5)]
        [InputControl(name = "fret3", layout = "Integer", format = "BIT", offset = 2, bit = 10, sizeInBits = 5)]
        [InputControl(name = "fret4", layout = "Integer", format = "BIT", offset = 3, bit = 0,  sizeInBits = 5)]
        [InputControl(name = "fret5", layout = "Integer", format = "BIT", offset = 3, bit = 5,  sizeInBits = 5)]
        [InputControl(name = "fret6", layout = "Integer", format = "BIT", offset = 3, bit = 10, sizeInBits = 5)]
        public fixed ushort frets[2];

        [InputControl(name = "velocity1", layout = "Axis", format = "BIT", offset = 6,  bit = 0, sizeInBits = 7)]
        [InputControl(name = "velocity2", layout = "Axis", format = "BIT", offset = 7,  bit = 0, sizeInBits = 7)]
        [InputControl(name = "velocity3", layout = "Axis", format = "BIT", offset = 8,  bit = 0, sizeInBits = 7)]
        [InputControl(name = "velocity4", layout = "Axis", format = "BIT", offset = 9,  bit = 0, sizeInBits = 7)]
        [InputControl(name = "velocity5", layout = "Axis", format = "BIT", offset = 10, bit = 0, sizeInBits = 7)]
        [InputControl(name = "velocity6", layout = "Axis", format = "BIT", offset = 11, bit = 0, sizeInBits = 7)]

        [InputControl(name = "greenFret", layout = "Button", offset = 6, bit = 7)]
        [InputControl(name = "redFret", layout = "Button", offset = 7, bit = 7)]
        [InputControl(name = "yellowFret", layout = "Button", offset = 8, bit = 7)]
        [InputControl(name = "blueFret", layout = "Button", offset = 9, bit = 7)]
        [InputControl(name = "orangeFret", layout = "Button", offset = 10, bit = 7)]
        public fixed byte velocities[6];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput Pro Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputProGuitarState), displayName = "XInput Pro Guitar")]
    public class XInputProGuitar : RockBandGuitar
    {
        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputProGuitar>(XInputNonStandardSubType.ProGuitar);
        }
    }
}
