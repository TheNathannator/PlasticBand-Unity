using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Base class for custom input device types.
    /// </summary>
    public abstract class BaseDevice<TDevice> : InputDevice
        where TDevice : BaseDevice<TDevice>
    {
        /// <summary>
        /// The current <typeparamref name="TDevice"/>.
        /// </summary>
        public static TDevice current { get; private set; }

        /// <summary>
        /// A collection of all <typeparamref name="TDevice"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<TDevice> all => s_AllDevices;
        private static List<TDevice> s_AllDevices = new List<TDevice>();

        /// <summary>
        /// Sets this device as the current device.
        /// </summary>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = (TDevice)this;
        }

        /// <summary>
        /// Handles when this device is added.
        /// </summary>
        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add((TDevice)this);
        }

        /// <summary>
        /// Handles when this device is removed.
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove((TDevice)this);
            if (current == this)
                current = null;
        }
    }
}
