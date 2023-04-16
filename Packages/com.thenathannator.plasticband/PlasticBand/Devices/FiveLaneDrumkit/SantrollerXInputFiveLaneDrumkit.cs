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
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Instruments/5-Lane%20Drums/Xbox%20360.md

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Santroller XInput Guitar Hero Drum Kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputFiveLaneDrumkitState), displayName = "Santroller XInput Guitar Hero Drum Kit")]
    public class SantrollerXInputFiveLaneDrumkit : XInputFiveLaneDrumkit
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputFiveLaneDrumkit"/>.
        /// </summary>
        public static new SantrollerXInputFiveLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputFiveLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputFiveLaneDrumkit> all => s_AllDevices;
        private static readonly List<SantrollerXInputFiveLaneDrumkit> s_AllDevices = new List<SantrollerXInputFiveLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputFiveLaneDrumkit"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<SantrollerXInputFiveLaneDrumkit>();
            // 4-lane kits and 5-lane kits share the same subtype, they need to be differentiated in another way
            // 5-lane kits always hold the left-stick click input, 4-lane kits use that for the second kick but
            // realistically that isn't likely to be held when powering on
            // May be some more specific capability data that also distinguishes them, but that probably isn't reliable
            XInputLayoutFixup.RegisterLayoutResolver(XInputController.DeviceSubType.DrumKit, (capabilities, state) => {
                if (capabilities.gamepad.leftStickX == SantrollerLayoutFinder.SantrollerVendorID && capabilities.gamepad.leftStickY == SantrollerLayoutFinder.SantrollerProductID && (state.buttons & (ushort)XInputGamepad.Button.LeftThumb) != 0)
                    return nameof(SantrollerXInputFiveLaneDrumkit);

                return null;
            });
            #endif
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputFiveLaneDrumkit"/>.
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
