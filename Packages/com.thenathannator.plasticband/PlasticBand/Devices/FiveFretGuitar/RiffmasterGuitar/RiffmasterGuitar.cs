using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    internal interface IRiffmasterGuitarState : IRockBandGuitarState_Distinct
    {
        bool p1 { get; set; }

        sbyte leftStickX { get; set; }
        sbyte leftStickY { get; set; }
        bool leftStickPress { get; set; }

        sbyte rightStickX { get; set; }
        sbyte rightStickY { get; set; }
        bool rightStickPress { get; set; }
    }

    /// <summary>
    /// A Riffmaster guitar.
    /// Has all features of Rock Band guitars, excluding the pickup switch.
    /// Also features a joystick on the headstock, although two joysticks are exposed here for CRKD guitars,
    /// the earlier models of which which have both left and right joysticks.
    /// </summary>
    [InputControlLayout(displayName = "Riffmaster 5-Fret Guitar")]
    public class RiffmasterGuitar : RockBandGuitar
    {
        /// <summary>
        /// The current <see cref="RiffmasterGuitar"/>.
        /// </summary>
        public static new RiffmasterGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="RiffmasterGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<RiffmasterGuitar> all => s_AllDevices;
        private static readonly List<RiffmasterGuitar> s_AllDevices = new List<RiffmasterGuitar>();

        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<RiffmasterGuitar>();
        }

        /// <summary>
        /// The left joystick on the guitar.
        /// </summary>
        [InputControl(displayName = "Left Stick")]
        public StickControl leftStick { get; private set; }

        /// <summary>
        /// The right joystick on the guitar.
        /// </summary>
        [InputControl(displayName = "Right Stick")]
        public StickControl rightStick { get; private set; }

        /// <summary>
        /// The click of the left joystick.
        /// </summary>
        [InputControl(displayName = "Left Stick Press")]
        public ButtonControl leftStickPress { get; private set; }

        /// <summary>
        /// The click of the right joystick.
        /// </summary>
        [InputControl(displayName = "Right Stick Press")]
        public ButtonControl rightStickPress { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            leftStick = GetChildControl<StickControl>(nameof(leftStick));
            rightStick = GetChildControl<StickControl>(nameof(rightStick));
            leftStickPress = GetChildControl<ButtonControl>(nameof(leftStickPress));
            rightStickPress = GetChildControl<ButtonControl>(nameof(rightStickPress));
        }

        /// <inheritdoc/>
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
    }
}
