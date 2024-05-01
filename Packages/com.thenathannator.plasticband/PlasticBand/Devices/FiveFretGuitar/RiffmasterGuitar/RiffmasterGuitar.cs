using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    internal interface IRiffmasterGuitarState : IRockBandGuitarState_Distinct
    {
        bool p1 { get; set; }

        sbyte joystickX { get; set; }
        sbyte joystickY { get; set; }

        bool joystickClick { get; set; }
    }

    /// <summary>
    /// A Riffmaster Rock Band guitar.
    /// Features the standard Rock Band solo frets, as well as a joystick on the back of its headstock.
    /// Does not have the pickup switch present on other Rock Band guitars.
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
        /// The joystick on the guitar's headstock.
        /// </summary>
        [InputControl(displayName = "Joystick")]
        public StickControl joystick { get; private set; }

        /// <summary>
        /// The click of the joystick on the guitar's headstock.
        /// </summary>
        [InputControl(displayName = "Joystick")]
        public ButtonControl joystickClick { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            joystick = GetChildControl<StickControl>(nameof(joystick));
            joystickClick = GetChildControl<ButtonControl>(nameof(joystickClick));
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
