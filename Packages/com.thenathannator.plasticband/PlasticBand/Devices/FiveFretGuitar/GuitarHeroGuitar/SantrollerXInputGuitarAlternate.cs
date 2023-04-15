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
    /// A Santroller XInput Guitar Hero Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarAlternateState), displayName = "Santroller XInput Guitar Hero Guitar")]
    public class SantrollerXInputGuitarAlternate : XInputGuitarAlternate
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputGuitarAlternate"/>.
        /// </summary>
        public static new SantrollerXInputGuitarAlternate current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputGuitarAlternate"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputGuitarAlternate> all => s_AllDevices;
        private static readonly List<SantrollerXInputGuitarAlternate> s_AllDevices = new List<SantrollerXInputGuitarAlternate>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputGuitarAlternate"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputGuitarAlternate>(XInputController.DeviceSubType.GuitarAlternate);
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputGuitarAlternate"/>.
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
