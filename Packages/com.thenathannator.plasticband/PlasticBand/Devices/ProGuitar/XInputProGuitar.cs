using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Pro%20Guitar/Xbox%20360.md

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputProGuitarState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

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

        [InputControl(name = "fret1", layout = "Integer", format = "BIT", bit = 0,  sizeInBits = 5)]
        [InputControl(name = "fret2", layout = "Integer", format = "BIT", bit = 5,  sizeInBits = 5)]
        [InputControl(name = "fret3", layout = "Integer", format = "BIT", bit = 10, sizeInBits = 5)]
        public ushort frets1;

        [InputControl(name = "fret4", layout = "Integer", format = "BIT", bit = 0,  sizeInBits = 5)]
        [InputControl(name = "fret5", layout = "Integer", format = "BIT", bit = 5,  sizeInBits = 5)]
        [InputControl(name = "fret6", layout = "Integer", format = "BIT", bit = 10, sizeInBits = 5)]
        public ushort frets2;

        [InputControl(name = "velocity1", layout = "Axis", format = "BIT", bit = 0, sizeInBits = 7)]
        [InputControl(name = "greenFret", layout = "Button", bit = 7)]
        public byte velocity1;

        [InputControl(name = "velocity2", layout = "Axis", format = "BIT", bit = 0, sizeInBits = 7)]
        [InputControl(name = "redFret", layout = "Button", bit = 7)]
        public byte velocity2;

        [InputControl(name = "velocity3", layout = "Axis", format = "BIT", bit = 0, sizeInBits = 7)]
        [InputControl(name = "yellowFret", layout = "Button", bit = 7)]
        public byte velocity3;

        [InputControl(name = "velocity4", layout = "Axis", format = "BIT", bit = 0, sizeInBits = 7)]
        [InputControl(name = "blueFret", layout = "Button", bit = 7)]
        public byte velocity4;

        [InputControl(name = "velocity5", layout = "Axis", format = "BIT", bit = 0, sizeInBits = 7)]
        [InputControl(name = "orangeFret", layout = "Button", bit = 7)]
        public byte velocity5;

        [InputControl(name = "velocity6", layout = "Axis", format = "BIT", bit = 0, sizeInBits = 7)]
        public byte velocity6;
    }

    [InputControlLayout(stateType = typeof(XInputProGuitarState), displayName = "XInput Rock Band Pro Guitar")]
    internal class XInputProGuitar : ProGuitar
    {
        internal new static void Initialize()
        {
            XInputLayoutFinder.RegisterLayout<XInputProGuitar>(XInputNonStandardSubType.ProGuitar);
        }
    }
}
