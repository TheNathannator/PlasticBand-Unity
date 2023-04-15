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
    /// A Santroller XInput 4-lane drumkit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputFourLaneDrumkitState), displayName = "Santroller device in XInput FourLaneDrum mode")]
    public class SantrollerXInputFourLaneDrumkit : XInputFourLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputFourLaneDrumkit"/>.
        /// </summary>
        public static new SantrollerXInputFourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputFourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputFourLaneDrumkit> all => s_AllDevices;
        private static readonly List<SantrollerXInputFourLaneDrumkit> s_AllDevices = new List<SantrollerXInputFourLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputFourLaneDrumkit"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputFourLaneDrumkit>(XInputController.DeviceSubType.DrumKit);
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputFourLaneDrumkit"/>.
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
