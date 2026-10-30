using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    internal interface IGamepadState : IInputStateTypeInfo
    {
        bool start { get; set; }
        bool select { get; set; }
        bool system { get; set; }
        bool share { get; set; }

        bool south { get; set; }
        bool east { get; set; }
        bool west { get; set; }
        bool north { get; set; }

        bool dpadUp { get; set; }
        bool dpadDown { get; set; }
        bool dpadLeft { get; set; }
        bool dpadRight { get; set; }

        bool leftShoulder { get; set; }
        bool rightShoulder { get; set; }
        bool leftTriggerPress { get; set; }
        bool rightTriggerPress { get; set; }
        bool leftStickPress { get; set; }
        bool rightStickPress { get; set; }

        byte leftTrigger { get; set; }
        byte rightTrigger { get; set; }

        sbyte leftStickX { get; set; }
        sbyte leftStickY { get; set; }

        sbyte rightStickX { get; set; }
        sbyte rightStickY { get; set; }
    }
}