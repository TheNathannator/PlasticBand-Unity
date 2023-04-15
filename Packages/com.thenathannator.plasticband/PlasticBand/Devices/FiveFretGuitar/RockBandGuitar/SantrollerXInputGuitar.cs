using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Xbox%20360.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Santroller XInput Rock Band Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarState), displayName = "Santroller XInput Rock Band Guitar")]
    public class SantrollerXInputGuitar : XInputGuitar
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputGuitar"/>.
        /// </summary>
        public static new SantrollerXInputGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputGuitar> all => s_AllDevices;
        private static readonly List<SantrollerXInputGuitar> s_AllDevices = new List<SantrollerXInputGuitar>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.Register<SantrollerXInputGuitar>(XInputController.DeviceSubType.Guitar);
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputGuitar"/>.
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
