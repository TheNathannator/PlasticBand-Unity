using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Keytar/PS3%20and%20Wii.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for PS3 and Wii keytars.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct PS3WiiKeytarState : IInputStateTypeInfo
    {
        public FourCC format => HidDefinitions.InputFormat;

        public byte reportId;

        [InputControl(name = "buttonWest", layout = "Button", bit = 0, displayName = "Square")]
        [InputControl(name = "buttonSouth", layout = "Button", bit = 1, displayName = "Cross")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 2, displayName = "Circle")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 3, displayName = "Triangle")]

        [InputControl(name = "selectButton", layout = "Button", bit = 8)]
        [InputControl(name = "startButton", layout = "Button", bit = 9)]

        [InputControl(name = "psButton", layout = "Button", bit = 12, displayName = "PlayStation")]
        public ushort buttons;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, defaultState = 8)]
        [InputControl(name = "dpad/up", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=7,maxValue=1,nullValue=8,wrapAtValue=7")]
        [InputControl(name = "dpad/right", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=1,maxValue=3")]
        [InputControl(name = "dpad/down", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=3,maxValue=5")]
        [InputControl(name = "dpad/left", layout = "DiscreteButton", format = "BIT", bit = 0, sizeInBits = 4, parameters = "minValue=5, maxValue=7")]
        public byte dpad;

        private fixed byte unused1[2];

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
        public byte overdrive;

        // TODO: The normalization here needs verification
        [InputControl(name = "analogPedal", layout = "Axis", format = "BIT", sizeInBits = 7, parameters = "normalize,normalizeMin=1,normalizeMax=0,normalizeZero=1")]
        [InputControl(name = "digitalPedal", layout = "Button", bit = 7)]
        public byte pedal;

        [InputControl(name = "touchStrip", layout = "Axis", format = "BIT", sizeInBits = 7)]
        public byte touchStrip;

        private fixed byte unused2[11];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// A PS3 keytar controller.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3WiiKeytarState), displayName = "Harmonix Pro Keyboard for PlayStation(R)3")]
    public class PS3Keytar : Keytar
    {
        /// <summary>
        /// The current <see cref="PS3Keytar"/>.
        /// </summary>
        public static new PS3Keytar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="PS3Keytar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<PS3Keytar> all => s_AllDevices;
        private static readonly List<PS3Keytar> s_AllDevices = new List<PS3Keytar>();

        /// <summary>
        /// Registers <see cref="PS3Keytar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            // Keytar
            InputSystem.RegisterLayout<PS3ProGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x12BA)
                .WithCapability("productId", 0x2330)
            );

            // MIDI Pro Adapter
            InputSystem.RegisterLayout<PS3ProGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x12BA)
                .WithCapability("productId", 0x2338)
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="PS3Keytar"/>.
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

    /// <summary>
    /// A Wii keytar controller.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3WiiKeytarState), displayName = "Harmonix Pro Keyboard for Nintendo Wii")]
    public class WiiKeytar : Keytar
    {
        /// <summary>
        /// The current <see cref="WiiKeytar"/>.
        /// </summary>
        public static new WiiKeytar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="WiiKeytar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<WiiKeytar> all => s_AllDevices;
        private static readonly List<WiiKeytar> s_AllDevices = new List<WiiKeytar>();

        /// <summary>
        /// Registers <see cref="WiiKeytar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            // Keytar
            InputSystem.RegisterLayout<PS3ProGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x12BA)
                .WithCapability("productId", 0x3330)
            );

            // MIDI Pro Adapter
            InputSystem.RegisterLayout<PS3ProGuitar>(matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 0x12BA)
                .WithCapability("productId", 0x3338)
            );
        }

        /// <summary>
        /// Sets this device as the current <see cref="WiiKeytar"/>.
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
