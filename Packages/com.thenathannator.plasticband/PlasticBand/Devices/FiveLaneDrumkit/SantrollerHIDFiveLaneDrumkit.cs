using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Santroller HID Guitar Hero Drum Kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3FiveLaneDrumkitState), displayName = "Santroller HID Guitar Hero Drum Kit")]
    public class SantrollerHIDFiveLaneDrumkit : FiveLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="SantrollerHIDFiveLaneDrumkit"/>.
        /// </summary>
        public static new SantrollerHIDFiveLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerHIDFiveLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerHIDFiveLaneDrumkit> all => s_AllDevices;
        private static readonly List<SantrollerHIDFiveLaneDrumkit> s_AllDevices = new List<SantrollerHIDFiveLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="SantrollerHIDFiveLaneDrumkit"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDFiveLaneDrumkit>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.Drums, SantrollerRhythmType.GuitarHero, nameof(SantrollerHIDFiveLaneDrumkit));
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerHIDFiveLaneDrumkit"/>.
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
