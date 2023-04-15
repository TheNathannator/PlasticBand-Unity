using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputTurntableState), displayName = "Santroller device in XInput Turntable mode")]
    public class SantrollerXInputTurntable : XInputTurntable
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputTurntable"/>.
        /// </summary>
        public static new SantrollerXInputTurntable current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputTurntable"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputTurntable> all => s_AllDevices;
        private static readonly List<SantrollerXInputTurntable> s_AllDevices = new List<SantrollerXInputTurntable>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputTurntable"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputTurntable>(XInputNonStandardSubType.Turntable);
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputTurntable"/>.
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
