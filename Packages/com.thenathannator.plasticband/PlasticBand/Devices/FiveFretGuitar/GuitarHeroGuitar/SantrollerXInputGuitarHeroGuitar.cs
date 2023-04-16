using System.Collections.Generic;
using PlasticBand.Devices.LowLevel;
using PlasticBand.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.XInput;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Fret%20Guitar/Guitar%20Hero/Xbox%20360.md
namespace PlasticBand.Devices
{
    /// <summary>
    /// A Santroller XInput Guitar Hero Guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarHeroGuitarState), displayName = "Santroller XInput Guitar Hero Guitar")]
    public class SantrollerXInputGuitarHeroGuitar : XInputGuitarHeroGuitar
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputGuitarHeroGuitar"/>.
        /// </summary>
        public static new SantrollerXInputGuitarHeroGuitar current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputGuitarHeroGuitar"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputGuitarHeroGuitar> all => s_AllDevices;
        private static readonly List<SantrollerXInputGuitarHeroGuitar> s_AllDevices = new List<SantrollerXInputGuitarHeroGuitar>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputGuitarHeroGuitar"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            SantrollerLayoutFinder.RegisterXInputLayout<SantrollerXInputGuitarHeroGuitar>(XInputController.DeviceSubType.GuitarAlternate);
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputGuitarHeroGuitar"/>.
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
