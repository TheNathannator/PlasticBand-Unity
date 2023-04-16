using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Rock%20Band/Xbox%20360.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Santroller XInput Rock Band Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputRockBandGuitarState), displayName = "Santroller XInput Rock Band Guitar")]
    public class SantrollerXInputRockBandGuitar : XInputRockBandGuitar
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputRockBandGuitar"/>.
        /// </summary>
        public static new SantrollerXInputRockBandGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputRockBandGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputRockBandGuitar> all => s_AllDevices;
        private static readonly List<SantrollerXInputRockBandGuitar> s_AllDevices = new List<SantrollerXInputRockBandGuitar>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputRockBandGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputRockBandGuitar>(XInputController.DeviceSubType.Guitar);
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputRockBandGuitar"/>.
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
