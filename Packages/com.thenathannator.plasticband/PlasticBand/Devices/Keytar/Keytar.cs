using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band 3 keytar controller.
    /// </summary>
    [InputControlLayout(displayName = "Keytar")]
    public class Keytar : InputDevice
    {
        /// <summary>
        /// The current <see cref="Keytar"/>.
        /// </summary>
        public static Keytar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="Keytar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<Keytar> all => s_AllDevices;
        private static readonly List<Keytar> s_AllDevices = new List<Keytar>();

        /// <summary>
        /// Registers <see cref="Keytar"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<Keytar>();
        }

        /// <summary>
        /// The keytar's d-pad.
        /// </summary>
        [InputControl(name = "dpad", displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The bottom face button on the keytar.
        /// </summary>
        [InputControl(name = "buttonSouth", displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button on the keytar.
        /// </summary>
        [InputControl(name = "buttonEast", displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button on the keytar.
        /// </summary>
        [InputControl(name = "buttonWest", displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button on the keytar.
        /// </summary>
        [InputControl(name = "buttonNorth", displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The Start button on the keytar.
        /// </summary>
        [InputControl(name = "startButton", displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the keytar.
        /// </summary>
        [InputControl(name = "selectButton", displayName = "Back")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The 1st (leftmost) key on the keytar (C-1).
        /// </summary>
        [InputControl(name = "key1", displayName = "C-1")]
        public ButtonControl key1 { get; private set; }

        /// <summary>
        /// The 2nd key on the keytar (C#/Db-1).
        /// </summary>
        [InputControl(name = "key2", displayName = "C#/Db-1")]
        public ButtonControl key2 { get; private set; }

        /// <summary>
        /// The 3rd key on the keytar (D-1).
        /// </summary>
        [InputControl(name = "key3", displayName = "D-1")]
        public ButtonControl key3 { get; private set; }

        /// <summary>
        /// The 4th key on the keytar (D#/Eb-1).
        /// </summary>
        [InputControl(name = "key4", displayName = "D#/Eb-1")]
        public ButtonControl key4 { get; private set; }

        /// <summary>
        /// The 5th key on the keytar (E-1).
        /// </summary>
        [InputControl(name = "key5", displayName = "E-1")]
        public ButtonControl key5 { get; private set; }

        /// <summary>
        /// The 6th key on the keytar (F-1).
        /// </summary>
        [InputControl(name = "key6", displayName = "F-1")]
        public ButtonControl key6 { get; private set; }

        /// <summary>
        /// The 7th key on the keytar (F#/Gb-1).
        /// </summary>
        [InputControl(name = "key7", displayName = "F#/Gb-1")]
        public ButtonControl key7 { get; private set; }

        /// <summary>
        /// The 8th key on the keytar (G-1).
        /// </summary>
        [InputControl(name = "key8", displayName = "G-1")]
        public ButtonControl key8 { get; private set; }

        /// <summary>
        /// The 9th key on the keytar (Gb/Ab-1).
        /// </summary>
        [InputControl(name = "key9", displayName = "Gb/Ab-1")]
        public ButtonControl key9 { get; private set; }

        /// <summary>
        /// The 10th key on the keytar (A-1).
        /// </summary>
        [InputControl(name = "key10", displayName = "A-1")]
        public ButtonControl key10 { get; private set; }

        /// <summary>
        /// The 11th key on the keytar (A#/Bb-1).
        /// </summary>
        [InputControl(name = "key11", displayName = "A#/Bb-1")]
        public ButtonControl key11 { get; private set; }

        /// <summary>
        /// The 12th key on the keytar (B-1).
        /// </summary>
        [InputControl(name = "key12", displayName = "B-1")]
        public ButtonControl key12 { get; private set; }

        /// <summary>
        /// The 13th key on the keytar (C-2).
        /// </summary>
        [InputControl(name = "key13", displayName = "C-2")]
        public ButtonControl key13 { get; private set; }

        /// <summary>
        /// The 14th key on the keytar (C#/Db-2).
        /// </summary>
        [InputControl(name = "key14", displayName = "C#/Db-2")]
        public ButtonControl key14 { get; private set; }

        /// <summary>
        /// The 15th key on the keytar (D-2).
        /// </summary>
        [InputControl(name = "key15", displayName = "D-2")]
        public ButtonControl key15 { get; private set; }

        /// <summary>
        /// The 16th key on the keytar (D#/Eb-2).
        /// </summary>
        [InputControl(name = "key16", displayName = "D#/Eb-2")]
        public ButtonControl key16 { get; private set; }

        /// <summary>
        /// The 17th key on the keytar (E-2).
        /// </summary>
        [InputControl(name = "key17", displayName = "E-2")]
        public ButtonControl key17 { get; private set; }

        /// <summary>
        /// The 18th key on the keytar (F-2).
        /// </summary>
        [InputControl(name = "key18", displayName = "F-2")]
        public ButtonControl key18 { get; private set; }

        /// <summary>
        /// The 19th key on the keytar (F#/Gb-2).
        /// </summary>
        [InputControl(name = "key19", displayName = "F#/Gb-2")]
        public ButtonControl key19 { get; private set; }

        /// <summary>
        /// The 20th key on the keytar (G-2).
        /// </summary>
        [InputControl(name = "key20", displayName = "G-2")]
        public ButtonControl key20 { get; private set; }

        /// <summary>
        /// The 21st key on the keytar (Gb/Ab-2).
        /// </summary>
        [InputControl(name = "key21", displayName = "Gb/Ab-2")]
        public ButtonControl key21 { get; private set; }

        /// <summary>
        /// The 22nd key on the keytar (A-2).
        /// </summary>
        [InputControl(name = "key22", displayName = "A-2")]
        public ButtonControl key22 { get; private set; }

        /// <summary>
        /// The 23rd key on the keytar (A#/Bb-2).
        /// </summary>
        [InputControl(name = "key23", displayName = "A#/Bb-2")]
        public ButtonControl key23 { get; private set; }

        /// <summary>
        /// The 24th key on the keytar (B-2).
        /// </summary>
        [InputControl(name = "key24", displayName = "B-2")]
        public ButtonControl key24 { get; private set; }

        /// <summary>
        /// The 25th (rightmost) key on the keytar (C-3).
        /// </summary>
        [InputControl(name = "key25", displayName = "C-3")]
        public ButtonControl key25 { get; private set; }

        /// <summary>
        /// The keytar's overdrive button.
        /// </summary>
        [InputControl(name = "overdrive", displayName = "Overdrive")]
        public ButtonControl overdrive { get; private set; }

        /// <summary>
        /// The digital pedal input on the keytar.
        /// </summary>
        [InputControl(name = "digitalPedal", displayName = "Digital Pedal")]
        public ButtonControl digitalPedal { get; private set; }

        /// <summary>
        /// The analog pedal input on the keytar.
        /// </summary>
        [InputControl(name = "analogPedal", displayName = "Analog Pedal")]
        public AxisControl analogPedal { get; private set; }

        /// <summary>
        /// The touch strip on the keytar.
        /// </summary>
        [InputControl(name = "touchStrip", displayName = "Touch Strip")]
        public AxisControl touchStrip { get; private set; }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            key1 = GetChildControl<ButtonControl>("key1");
            key2 = GetChildControl<ButtonControl>("key2");
            key3 = GetChildControl<ButtonControl>("key3");
            key4 = GetChildControl<ButtonControl>("key4");
            key5 = GetChildControl<ButtonControl>("key5");
            key6 = GetChildControl<ButtonControl>("key6");
            key7 = GetChildControl<ButtonControl>("key7");
            key8 = GetChildControl<ButtonControl>("key8");
            key9 = GetChildControl<ButtonControl>("key9");
            key10 = GetChildControl<ButtonControl>("key10");
            key11 = GetChildControl<ButtonControl>("key11");
            key12 = GetChildControl<ButtonControl>("key12");
            key13 = GetChildControl<ButtonControl>("key13");
            key14 = GetChildControl<ButtonControl>("key14");
            key15 = GetChildControl<ButtonControl>("key15");
            key16 = GetChildControl<ButtonControl>("key16");
            key17 = GetChildControl<ButtonControl>("key17");
            key18 = GetChildControl<ButtonControl>("key18");
            key19 = GetChildControl<ButtonControl>("key19");
            key20 = GetChildControl<ButtonControl>("key20");
            key21 = GetChildControl<ButtonControl>("key21");
            key22 = GetChildControl<ButtonControl>("key22");
            key23 = GetChildControl<ButtonControl>("key23");
            key24 = GetChildControl<ButtonControl>("key24");
            key25 = GetChildControl<ButtonControl>("key25");

            dpad = GetChildControl<DpadControl>("dpad");

            buttonSouth = GetChildControl<ButtonControl>("buttonSouth");
            buttonEast = GetChildControl<ButtonControl>("buttonEast");
            buttonWest = GetChildControl<ButtonControl>("buttonWest");
            buttonNorth = GetChildControl<ButtonControl>("buttonNorth");

            startButton = GetChildControl<ButtonControl>("startButton");
            selectButton = GetChildControl<ButtonControl>("selectButton");

            overdrive = GetChildControl<ButtonControl>("overdrive");
            digitalPedal = GetChildControl<ButtonControl>("digitalPedal");

            analogPedal = GetChildControl<AxisControl>("analogPedal");
            touchStrip = GetChildControl<AxisControl>("touchStrip");
        }

        /// <summary>
        /// Sets this device as the current <see cref="Keytar"/>.
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
