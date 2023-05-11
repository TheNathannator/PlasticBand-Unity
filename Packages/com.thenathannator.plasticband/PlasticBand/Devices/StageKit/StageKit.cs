using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band stage kit.
    /// </summary>
    [InputControlLayout(displayName = "Rock Band Stage Kit")]
    public class StageKit : InputDevice
    {
        /// <summary>
        /// The current <see cref="StageKit"/>.
        /// </summary>
        public static StageKit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="StageKit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<StageKit> all => s_AllDevices;
        private static readonly List<StageKit> s_AllDevices = new List<StageKit>();

        /// <summary>
        /// Registers <see cref="StageKit"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<StageKit>();
        }

        /// <summary>
        /// The stage kit's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The bottom face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The Start button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Select")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// Sets this device as the current <see cref="StageKit"/>.
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