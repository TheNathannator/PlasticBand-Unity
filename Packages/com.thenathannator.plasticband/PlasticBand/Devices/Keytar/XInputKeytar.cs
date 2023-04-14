using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Keytar/Xbox%20360.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput keytars.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputKeytarState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", bit = 0, sizeInBits = 4)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y")]
        public ushort buttons;

        [InputControl(name = "key1",  layout = "Button", bit = 7)]
        [InputControl(name = "key2",  layout = "Button", bit = 6)]
        [InputControl(name = "key3",  layout = "Button", bit = 5)]
        [InputControl(name = "key4",  layout = "Button", bit = 4)]
        [InputControl(name = "key5",  layout = "Button", bit = 3)]
        [InputControl(name = "key6",  layout = "Button", bit = 2)]
        [InputControl(name = "key7",  layout = "Button", bit = 1)]
        [InputControl(name = "key8",  layout = "Button", bit = 0)]
        public byte keys1;

        [InputControl(name = "key9",  layout = "Button", bit = 7)]
        [InputControl(name = "key10", layout = "Button", bit = 6)]
        [InputControl(name = "key11", layout = "Button", bit = 5)]
        [InputControl(name = "key12", layout = "Button", bit = 4)]
        [InputControl(name = "key13", layout = "Button", bit = 3)]
        [InputControl(name = "key14", layout = "Button", bit = 2)]
        [InputControl(name = "key15", layout = "Button", bit = 1)]
        [InputControl(name = "key16", layout = "Button", bit = 0)]
        public byte keys2;

        [InputControl(name = "key17", layout = "Button", bit = 7)]
        [InputControl(name = "key18", layout = "Button", bit = 6)]
        [InputControl(name = "key19", layout = "Button", bit = 5)]
        [InputControl(name = "key20", layout = "Button", bit = 4)]
        [InputControl(name = "key21", layout = "Button", bit = 3)]
        [InputControl(name = "key22", layout = "Button", bit = 2)]
        [InputControl(name = "key23", layout = "Button", bit = 1)]
        [InputControl(name = "key24", layout = "Button", bit = 0)]
        public byte keys3;

        [InputControl(name = "key25", layout = "Button", bit = 7)]
        // TODO: Try to pair velocities with keys
        [InputControl(name = "velocity1", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity1;

        [InputControl(name = "velocity2", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity2;

        [InputControl(name = "velocity3", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity3;

        [InputControl(name = "velocity4", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity4;

        [InputControl(name = "velocity5", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte velocity5;

        [InputControl(name = "overdrive", layout = "Button", bit = 7)]
        // The touchstrip isn't available through XInput, but it has to be there for Keytar
        // These bits are unused, so we place it here
        [InputControl(name = "touchStrip", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte overdrive;

        // TODO: The normalization here needs verification
        [InputControl(name = "analogPedal", layout = "Axis", format = "BIT", sizeInBits = 7, parameters = "normalize,normalizeMin=1,normalizeMax=0,normalizeZero=1")]
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte pedal;
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput keytar controller.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputKeytarState), displayName = "XInput Keytar")]
    public class XInputKeytar : Keytar
    {
        /// <summary>
        /// The current <see cref="XInputKeytar"/>.
        /// </summary>
        public static new XInputKeytar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputKeytar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputKeytar> all => s_AllDevices;
        private static readonly List<XInputKeytar> s_AllDevices = new List<XInputKeytar>();

        /// <summary>
        /// Registers <see cref="XInputKeytar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            XInputDeviceUtils.Register<XInputKeytar>(XInputNonStandardSubType.Keytar);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputKeytar"/>.
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
    }
}
