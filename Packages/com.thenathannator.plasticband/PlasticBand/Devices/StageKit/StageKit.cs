using System.Collections.Generic;
using PlasticBand.Haptics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace PlasticBand.Devices
{
    /// <summary>
    /// A Rock Band stage kit.
    /// </summary>
    [InputControlLayout(displayName = "Rock Band Stage Kit")]
    public class StageKit : InputDevice, IStageKitHaptics
    {
        /// <summary>
        /// The current <see cref="StageKit"/>.
        /// </summary>
        public static StageKit current { get; private set; }

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

        /// <summary>
        /// The stage kit's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The bottom face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The Start button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the stage kit.
        /// </summary>
        [InputControl(displayName = "Select")]
        public ButtonControl selectButton { get; private set; }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            buttonSouth = GetChildControl<ButtonControl>(nameof(buttonSouth));
            buttonEast = GetChildControl<ButtonControl>(nameof(buttonEast));
            buttonWest = GetChildControl<ButtonControl>(nameof(buttonWest));
            buttonNorth = GetChildControl<ButtonControl>(nameof(buttonNorth));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));
        }

        /// <summary>
        /// Sets this device as the current <see cref="StageKit"/>.
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

        // Current haptics state
        protected bool m_HapticsEnabled;
        protected bool m_FogEnabled;
        protected StageKitStrobeSpeed m_StrobeSpeed;
        protected StageKitLed m_RedLeds;
        protected StageKitLed m_YellowLeds;
        protected StageKitLed m_BlueLeds;
        protected StageKitLed m_GreenLeds;

        /// <summary>
        /// Resets the state of the stage kit without resetting the stored state.
        /// </summary>
        public void PauseHaptics()
        {
            m_HapticsEnabled = false;
            SendReset();
        }

        /// <summary>
        /// Restores the stored state of the stage kit.
        /// </summary>
        public void ResumeHaptics()
        {
            m_HapticsEnabled = true;
            SendFog(m_FogEnabled);
            SendStrobe(m_StrobeSpeed);
            SendLeds(StageKitLedColor.Red, m_RedLeds);
            SendLeds(StageKitLedColor.Yellow, m_YellowLeds);
            SendLeds(StageKitLedColor.Blue, m_BlueLeds);
            SendLeds(StageKitLedColor.Green, m_GreenLeds);
        }

        /// <summary>
        /// Resets the state of the stage kit.
        /// </summary>
        public void ResetHaptics()
        {
            m_FogEnabled = false;
            m_StrobeSpeed = StageKitStrobeSpeed.Off;
            m_RedLeds = StageKitLed.None;
            m_YellowLeds = StageKitLed.None;
            m_BlueLeds = StageKitLed.None;
            m_GreenLeds = StageKitLed.None;
            SendReset();
        }

        /// <inheritdoc cref="IStageKitHaptics.SetFogMachine(bool)"/>
        public void SetFogMachine(bool enabled)
        {
            m_FogEnabled = enabled;
            if (m_HapticsEnabled)
                SendFog(enabled);
        }

        /// <inheritdoc cref="IStageKitHaptics.SetStrobeSpeed(StageKitStrobeSpeed)"/>
        public void SetStrobeSpeed(StageKitStrobeSpeed speed)
        {
            m_StrobeSpeed = speed;
            if (m_HapticsEnabled)
                SendStrobe(speed);
        }

        /// <inheritdoc cref="IStageKitHaptics.SetLeds(StageKitLedColor, StageKitLed)"/>
        public void SetLeds(StageKitLedColor color, StageKitLed leds)
        {
            if ((color & StageKitLedColor.Red) != 0)
                m_RedLeds = leds;
            if ((color & StageKitLedColor.Yellow) != 0)
                m_YellowLeds = leds;
            if ((color & StageKitLedColor.Blue) != 0)
                m_BlueLeds = leds;
            if ((color & StageKitLedColor.Green) != 0)
                m_GreenLeds = leds;

            if (m_HapticsEnabled)
                SendLeds(color, leds);
        }

        /// <inheritdoc cref="IStageKitHaptics.SetRedLeds(StageKitLed)"/>
        public void SetRedLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Red, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetYellowLeds(StageKitLed)"/>
        public void SetYellowLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Yellow, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetBlueLeds(StageKitLed)"/>
        public void SetBlueLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Blue, leds);

        /// <inheritdoc cref="IStageKitHaptics.SetGreenLeds(StageKitLed)"/>
        public void SetGreenLeds(StageKitLed leds)
            => SetLeds(StageKitLedColor.Green, leds);

        // Virtual methods for the implementation details
        protected virtual void SendFog(bool enabled) { }
        protected virtual void SendStrobe(StageKitStrobeSpeed speed) { }
        protected virtual void SendLeds(StageKitLedColor color, StageKitLed leds) { }
        protected virtual void SendReset() { }
    }
}