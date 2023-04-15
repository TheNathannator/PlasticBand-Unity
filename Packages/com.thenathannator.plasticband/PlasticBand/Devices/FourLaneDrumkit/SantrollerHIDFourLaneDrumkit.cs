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
    /// A Santroller 5-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(PS3WiiFourLaneDrumkitState), displayName = "Santroller device in Guitar Hero Drum Mode")]
    public class SantrollerHIDFourLaneDrumkit : FourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="SantrollerHIDFourLaneDrumkit"/>.
        /// </summary>
        public static new SantrollerHIDFourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerHIDFourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerHIDFourLaneDrumkit> all => s_AllDevices;
        private static readonly List<SantrollerHIDFourLaneDrumkit> s_AllDevices = new List<SantrollerHIDFourLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="SantrollerHIDFourLaneDrumkit"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<SantrollerHIDFourLaneDrumkit>();
            SantrollerLayoutFinder.RegisterLayout(SantrollerDeviceType.Drums, SantrollerRhythmType.RockBand, nameof(SantrollerHIDFourLaneDrumkit));
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerHIDFourLaneDrumkit"/>.
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
