using System;
using System.Collections.Generic;
using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band stage kit.
    /// </summary>
    [InputControlLayout(displayName = "Rock Band Stage Kit")]
    public class StageKit : Gamepad, IStageKitHaptics
    {
        /// <summary>
        /// The current <see cref="StageKit"/>.
        /// </summary>
        public static new StageKit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="StageKit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<StageKit> all => s_AllDevices;
        private static readonly List<StageKit> s_AllDevices = new List<StageKit>();

        /// <summary>
        /// Registers <see cref="StageKit"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<StageKit>();
        }

        /// <inheritdoc/>
        public override void MakeCurrent()
        {
            base.MakeCurrent();
            current = this;
        }

        protected override void OnAdded()
        {
            base.OnAdded();
            s_AllDevices.Add(this);
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
            s_AllDevices.Remove(this);
            if (current == this)
                current = null;
        }

        // Override to prevent standard rumble commands from being sent
        /// <summary>
        /// Stage kits do not support rumble, do not use this method.
        /// </summary>
        [Obsolete("Stage kits do not support rumble, do not use this method.", error: true)]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override void SetMotorSpeeds(float lowFrequency, float highFrequency)
        {
            // Explicitly do nothing, stage kits use rumble for their own features
            // No exceptions are thrown or errors logged, as this is accessible from
            // an interface where device type most likely won't be known ahead of time
        }
#pragma warning restore CS0809

        /// <inheritdoc cref="IStageKitHaptics.SetFogMachine(bool)"/>
        public virtual void SetFogMachine(bool enabled) {}

        /// <inheritdoc cref="IStageKitHaptics.SetStrobeSpeed(StageKitStrobeSpeed)"/>
        public virtual void SetStrobeSpeed(StageKitStrobeSpeed speed) {}

        /// <inheritdoc cref="IStageKitHaptics.SetLeds(StageKitLedColor, StageKitLed)"/>
        public virtual void SetLeds(StageKitLedColor color, StageKitLed leds) {}

        /// <inheritdoc cref="IStageKitHaptics.SetRedLeds(StageKitLed)"/>
        public virtual void SetRedLeds(StageKitLed leds) {}

        /// <inheritdoc cref="IStageKitHaptics.SetYellowLeds(StageKitLed)"/>
        public virtual void SetYellowLeds(StageKitLed leds) {}

        /// <inheritdoc cref="IStageKitHaptics.SetBlueLeds(StageKitLed)"/>
        public virtual void SetBlueLeds(StageKitLed leds) {}

        /// <inheritdoc cref="IStageKitHaptics.SetGreenLeds(StageKitLed)"/>
        public virtual void SetGreenLeds(StageKitLed leds) {}
    }
}