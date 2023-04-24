using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A 6-fret guitar controller.
    /// </summary>
    [InputControlLayout(displayName = "6-Fret Guitar")]
    public class SixFretGuitar : InputDevice
    {
        /// <summary>
        /// The current <see cref="SixFretGuitar"/>.
        /// </summary>
        public static SixFretGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SixFretGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SixFretGuitar> all => s_AllDevices;
        private static readonly List<SixFretGuitar> s_AllDevices = new List<SixFretGuitar>();

        /// <summary>
        /// Registers <see cref="SixFretGuitar"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<SixFretGuitar>();
        }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Black 1", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl black1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Black 2", usages = new[] { "Back", "Cancel" })]
        public ButtonControl black2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "Black 3")]
        public ButtonControl black3 { get; private set; }

        /// <summary>
        /// The first black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "White 1")]
        public ButtonControl white1 { get; private set; }

        /// <summary>
        /// The second black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "White 2")]
        public ButtonControl white2 { get; private set; }

        /// <summary>
        /// The third black fret on the guitar.
        /// </summary>
        [InputControl(displayName = "White 3")]
        public ButtonControl white3 { get; private set; }

        /// <summary>
        /// The guitar's strum up input.
        /// On 6-fret guitars, this is *not* equivalent to d-pad up (though strumming may still trigger the d-pad).
        /// </summary>
        [InputControl(displayName = "Strum Up")]
        public ButtonControl strumUp { get; private set; }

        /// <summary>
        /// The guitar's strum down input.
        /// On 6-fret guitars, this is *not* equivalent to d-pad down (though strumming may still trigger the d-pad).
        /// </summary>
        [InputControl(displayName = "Strum Down")]
        public ButtonControl strumDown { get; private set; }

        /// <summary>
        /// The guitar's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The guitar's tilt orientation.
        /// </summary>
        [InputControl(displayName = "Tilt", noisy = true)]
        public AxisControl tilt { get; private set; }

        /// <summary>
        /// The guitar's whammy bar.
        /// </summary>
        [InputControl(displayName = "Whammy")]
        public AxisControl whammy { get; private set; }

        /// <summary>
        /// The Start button on the guitar.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the guitar.
        /// </summary>
        [InputControl(displayName = "Hero Power")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The GHTV button on the guitar.
        /// </summary>
        [InputControl(displayName = "GHTV Button")]
        public ButtonControl ghtvButton { get; private set; }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            black1 = GetChildControl<ButtonControl>(nameof(black1));
            black2 = GetChildControl<ButtonControl>(nameof(black2));
            black3 = GetChildControl<ButtonControl>(nameof(black3));
            white1 = GetChildControl<ButtonControl>(nameof(white1));
            white2 = GetChildControl<ButtonControl>(nameof(white2));
            white3 = GetChildControl<ButtonControl>(nameof(white3));

            strumUp = GetChildControl<ButtonControl>(nameof(strumUp));
            strumDown = GetChildControl<ButtonControl>(nameof(strumDown));

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            tilt = GetChildControl<AxisControl>(nameof(tilt));
            whammy = GetChildControl<AxisControl>(nameof(whammy));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));
            ghtvButton = GetChildControl<ButtonControl>(nameof(ghtvButton));
        }

        /// <summary>
        /// Sets this device as the current <see cref="SixFretGuitar"/>.
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
