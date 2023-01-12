using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices.LowLevel
{
    // https://github.com/RPCS3/rpcs3/blob/master/rpcs3/Emu/Io/Turntable.cpp
    // https://github.com/shockdude/DJHtableUtility
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3TurntableState : IInputStateTypeInfo
    {
        FourCC IInputStateTypeInfo.format => new FourCC('H', 'I', 'D');

        public byte reportId;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle / Euphoria", alias = "euphoria")]

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

        [InputControl(name = "leftTableVelocity", layout = "Axis", noisy = true, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte leftTableVelocity;

        [InputControl(name = "rightTableVelocity", layout = "Axis", noisy = true, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public byte rightTableVelocity;

        public fixed byte unused2[12];

        [InputControl(name = "effectsDial", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
        public short effectsDial;

        [InputControl(name = "crossFader", layout = "Axis", noisy = true, format = "BIT", sizeInBits = 10, parameters = "normalize,normalizeMin=0,normalizeMax=1,normalizeZero=0.5")]
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

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }

        private static readonly PS3OutputCommand s_EuphoriaOnCommand = new PS3OutputCommand(
            0x91,
            new byte[PS3OutputCommand.kDataSize] { 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        private static readonly PS3OutputCommand s_EuphoriaOffCommand = new PS3OutputCommand(
            0x91,
            new byte[PS3OutputCommand.kDataSize] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
        );

        private float m_PreviousBrightness = -1;
        private bool m_Direction; // true == up, false == down
        protected override void OnEuphoriaTick(float brightness)
        {
            PS3OutputCommand command;
            // Force-disable
            if (brightness < 0)
            {
                m_Direction = false;
                command = s_EuphoriaOffCommand;
            }
            // Enable during an increase
            else if ((brightness > m_PreviousBrightness) && !m_Direction)
            {
                m_Direction = true;
                command = s_EuphoriaOnCommand;
            }
            // Disable during a decrease
            else if ((brightness < m_PreviousBrightness) && m_Direction)
            {
                m_Direction = false;
                command = s_EuphoriaOffCommand;
            }
            // Direction hasn't changed, don't send the same one multiple times
            else
            {
                return;
            }

            m_PreviousBrightness = brightness;

            // Send command
            this.ExecuteCommand(ref command);
        }
    }
}
