using System.Collections.Generic;
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
    /// The state format for PS3 DJ Hero turntables.
    /// </summary>
    // https://github.com/RPCS3/rpcs3/blob/master/rpcs3/Emu/Io/Turntable.cpp
    // https://github.com/shockdude/DJHtableUtility
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3TurntableState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle / Euphoria")]

        [InputControl(name = "euphoria", layout = "Button", bit = 3, displayName = "Euphoria")]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 15)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=15,wrapAtValue=7", displayName = "Up/Strum Up")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5", displayName = "Down/Strum Down")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        public fixed byte unused1[2];

        [InputControl(name = "leftTableVelocity", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte leftTableVelocity;

        [InputControl(name = "rightTableVelocity", layout = "Axis", noisy = true, defaultState = 0x80, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte rightTableVelocity;

        public fixed byte unused2[12];

        [InputControl(name = "effectsDial", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short effectsDial;

        [InputControl(name = "crossFader", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, defaultState = 0x200, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short crossFader;

        [InputControl(name = "rightTableGreen", layout = "Button", bit = 0)]
        [InputControl(name = "rightTableRed", layout = "Button", bit = 1)]
        [InputControl(name = "rightTableBlue", layout = "Button", bit = 2)]
        [InputControl(name = "leftTableGreen", layout = "Button", bit = 3)]
        [InputControl(name = "leftTableRed", layout = "Button", bit = 4)]
        [InputControl(name = "leftTableBlue", layout = "Button", bit = 5)]
        public short tableButtons;

        public short unused3;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 DJ Hero turntable.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3TurntableState), displayName = "PS3 Turntable")]
    internal class PS3Turntable : Turntable
    {
        /// <summary>
        /// The current <see cref="PS3Turntable"/>.
        /// </summary>
        public static new PS3Turntable current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3Turntable"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3Turntable> all => s_AllDevices;
        private static readonly List<PS3Turntable> s_AllDevices = new List<PS3Turntable>();

        /// <summary>
        /// Registers <see cref="PS3Turntable"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<PS3Turntable>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                // https://github.com/shockdude/DJHtableUtility/blob/turntable/MainWindow.cs#L361
                // Names retrieved from https://www.pcilookup.com
                .WithCapability("vendorId", 0x12BA) // "Licensed by Sony Computer Entertainment America"
                .WithCapability("productId", 0x0140) // (Not registered)
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3Turntable"/>.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        /// <summary>
        /// Processes when this device is added to the system.
        /// </summary>
        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        /// <summary>
        /// Processes when this device is removed from the system.
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }

        /// <summary>
        /// Enables the euphoria light on the turntable.
        /// </summary>
        private static readonly PS3OutputCommand s_EuphoriaOnCommand = new PS3OutputCommand(
            0x91,
            new byte[PS3OutputCommand.kDataSize] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        /// <summary>
        /// Disables the euphoria light on the turntable.
        /// </summary>
        private static readonly PS3OutputCommand s_EuphoriaOffCommand = new PS3OutputCommand(
            0x91,
            new byte[PS3OutputCommand.kDataSize] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        /// <summary>
        /// The previous brightness of the euphoria effect.
        /// Used for determining direction.
        /// </summary>
        private float m_PreviousBrightness = -1;

        /// <summary>
        /// The current direction of the euphoria effect.
        /// </summary>
        /// <remarks>
        /// true == up, false == down.
        /// </remarks>
        private bool m_Direction;

        /// <inheritdoc/>
        protected override void OnEuphoriaTick(float brightness)
        {
            PS3OutputCommand command;
            // Force-disable
            if (brightness < 0)
            {
                m_Direction = false;
                command = s_EuphoriaOffCommand;
            }
            // Enable at the start of an increase
            else if ((brightness > m_PreviousBrightness) && !m_Direction)
            {
                m_Direction = true;
                command = s_EuphoriaOnCommand;
            }
            // Disable at the start of a decrease
            else if ((brightness < m_PreviousBrightness) && m_Direction)
            {
                m_Direction = false;
                command = s_EuphoriaOffCommand;
            }
            // Direction hasn't changed, don't send the same one multiple times
            else
            {
                m_PreviousBrightness = brightness;
                return;
            }

            m_PreviousBrightness = brightness;

            // Send command
            this.ExecuteCommand(ref command);
        }
    }
}
