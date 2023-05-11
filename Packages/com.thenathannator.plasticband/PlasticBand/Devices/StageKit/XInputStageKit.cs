using System.Collections.Generic;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// PlasticBand reference doc:
// https://github.com/TheNathannator/PlasticBand/blob/main/Docs/Other/Xbox%20360%20Rock%20Band%20Stage%20Kit.md

namespace PlasticBand.Devices.LowLevel
{
    /// <summary>
    /// The state format for XInput stage kits.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct XInputStageKitState : IInputStateTypeInfo
    {
        public FourCC format => XInputGamepad.Format;

        [InputControl(name = "dpad", layout = "Dpad", format = "BIT", sizeInBits = 4, bit = 0)]
        [InputControl(name = "dpad/up", bit = 0)]
        [InputControl(name = "dpad/down", bit = 1)]
        [InputControl(name = "dpad/left", bit = 2)]
        [InputControl(name = "dpad/right", bit = 3)]

        [InputControl(name = "startButton", layout = "Button", bit = 4)]
        [InputControl(name = "selectButton", layout = "Button", bit = 5, displayName = "Back")]

        [InputControl(name = "buttonSouth", layout = "Button", bit = 12, displayName = "A")]
        [InputControl(name = "buttonEast", layout = "Button", bit = 13, displayName = "B")]
        [InputControl(name = "buttonWest", layout = "Button", bit = 14, displayName = "X")]
        [InputControl(name = "buttonNorth", layout = "Button", bit = 15, displayName = "Y")]
        public ushort buttons;

        private fixed byte unused[10];
    }
}

namespace PlasticBand.Devices
{
    /// <summary>
    /// An XInput Rock Band stage kit.
    /// </summary>
    [InputControlLayout(stateType = typeof(XInputStageKitState), displayName = "XInput Rock Band Stage Kit")]
    public class XInputStageKit : StageKit
    {
        /// <summary>
        /// The current <see cref="XInputStageKit"/>.
        /// </summary>
        public static new XInputStageKit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="XInputStageKit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<XInputStageKit> all => s_AllDevices;
        private static readonly List<XInputStageKit> s_AllDevices = new List<XInputStageKit>();

        /// <summary>
        /// Registers <see cref="XInputStageKit"/> to the input system.
        /// </summary>
        internal static new void Initialize()
        {
            XInputDeviceUtils.Register<XInputStageKit>(XInputNonStandardSubType.StageKit);
        }

        /// <summary>
        /// Sets this device as the current <see cref="XInputStageKit"/>.
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