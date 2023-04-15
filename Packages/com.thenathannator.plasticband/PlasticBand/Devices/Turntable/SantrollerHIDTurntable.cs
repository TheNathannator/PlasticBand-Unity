using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/Turntable/PS3.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Santroller HID Turntable.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3TurntableState), displayName = "Santroller HID Turntable")]
    public class SantrollerHIDTurntable : PS3Turntable
    {
        /// <summary>
        /// The current <see cref="SantrollerHIDTurntable"/>.
        /// </summary>
        public static new SantrollerHIDTurntable current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerHIDTurntable"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerHIDTurntable> all => s_AllDevices;
        private static readonly List<SantrollerHIDTurntable> s_AllDevices = new List<SantrollerHIDTurntable>();

        /// <summary>
        /// Registers <see cref="SantrollerHIDTurntable"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDTurntable>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.DjHeroTurntable, null, nameof(SantrollerHIDTurntable));
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerHIDTurntable"/>.
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
