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
    /// A Santroller XInput GHL guitar.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputGuitarGHLState), displayName = "Santroller device in XInput GHL Guitar mode")]
    public class SantrollerXInputGuitarGHL : XInputGuitarGHL
    {
        /// <summary>
        /// The current <see cref="SantrollerXInputGuitarGHL"/>.
        /// </summary>
        public static new SantrollerXInputGuitarGHL current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="SantrollerXInputGuitarGHL"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<SantrollerXInputGuitarGHL> all => s_AllDevices;
        private static readonly List<SantrollerXInputGuitarGHL> s_AllDevices = new List<SantrollerXInputGuitarGHL>();

        /// <summary>
        /// Registers <see cref="SantrollerXInputGuitarGHL"/> to the input system.
        /// </summary>
        internal new static void Initialize()
        {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            InputSystem.RegisterLayout<SantrollerXInputGuitarGHL>(matches: new InputDeviceMatcher()
                // Annoyingly, GHL guitars do not have a unique subtype. So, we have to use some other information to identify them.
                .WithInterface(XInputOther.kInterfaceName)
                .WithCapability("subType", XInputController.DeviceSubType.GuitarAlternate)
                .WithCapability("leftStickX", SantrollerLayoutFinder.SantrollerVendorID)
                .WithCapability("leftStickY", SantrollerLayoutFinder.SantrollerProductID)
                // so we use the flags as the distinguisher.
                .WithCapability("flags", (int)(XInputFlags.VoiceSupported | XInputFlags.PluginModulesSupported | XInputFlags.NoNavigation)) // 28
            );
            #endif
        }

        /// <summary>
        /// Sets this device as the current <see cref="SantrollerXInputGuitarGHL"/>.
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
