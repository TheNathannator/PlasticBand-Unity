using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    [Flags]
    public enum FourLanePad
    {
        None = 0,
        Kick1 = 0x01,
        Kick2 = 0x02,
        RedPad = 0x04,
        YellowPad = 0x08,
        BluePad = 0x10,
        GreenPad = 0x20,
        YellowCymbal = 0x40,
        BlueCymbal = 0x80,
        GreenCymbal = 0x100,
    }

    /// <summary>
    /// A 4-lane (Rock Band) drumkit controller.
    /// </summary>
    [InputControlLayout(displayName = "Rock Band (4-Lane) Drumkit")]
    public class FourLaneDrumkit : InputDevice
    {
        /// <summary>
        /// The current <see cref="FourLaneDrumkit"/>.
        /// </summary>
        public static FourLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="FourLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<FourLaneDrumkit> all => s_AllDevices;
        private static readonly List<FourLaneDrumkit> s_AllDevices = new List<FourLaneDrumkit>();

        /// <summary>
        /// Registers <see cref="FourLaneDrumkit"/> to the input system.
        /// </summary>
        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FourLaneDrumkit>();
        }

        /// <summary>
        /// The drumkit's d-pad.
        /// </summary>
        [InputControl(displayName = "D-Pad", usage = "Hatswitch")]
        public DpadControl dpad { get; private set; }

        /// <summary>
        /// The Start button on the drumkit.
        /// </summary>
        [InputControl(displayName = "Start", usage = "Menu")]
        public ButtonControl startButton { get; private set; }

        /// <summary>
        /// The Select button on the drumkit.
        /// </summary>
        [InputControl(displayName = "Select")]
        public ButtonControl selectButton { get; private set; }

        /// <summary>
        /// The bottom face button.
        /// </summary>
        [InputControl(displayName = "Button South", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl buttonSouth { get; private set; }

        /// <summary>
        /// The right face button.
        /// </summary>
        [InputControl(displayName = "Button East", usages = new[] { "Back", "Cancel" })]
        public ButtonControl buttonEast { get; private set; }

        /// <summary>
        /// The left face button.
        /// </summary>
        [InputControl(displayName = "Button West")]
        public ButtonControl buttonWest { get; private set; }

        /// <summary>
        /// The top face button.
        /// </summary>
        [InputControl(displayName = "Button North")]
        public ButtonControl buttonNorth { get; private set; }

        /// <summary>
        /// The red pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Red Pad", usages = new[] { "Back", "Cancel" })]
        public ButtonControl redPad { get; private set; }

        /// <summary>
        /// The yellow pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Yellow Pad")]
        public ButtonControl yellowPad { get; private set; }

        /// <summary>
        /// The blue pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Blue Pad")]
        public ButtonControl bluePad { get; private set; }

        /// <summary>
        /// The green pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Green Pad", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenPad { get; private set; }

        /// <summary>
        /// The yellow cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Yellow Cymbal")]
        public ButtonControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Blue Cymbal")]
        public ButtonControl blueCymbal { get; private set; }

        /// <summary>
        /// The green cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Green Cymbal", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenCymbal { get; private set; }

        /// <summary>
        /// The first of two kick pedals on the drumkit.
        /// </summary>
        [InputControl(displayName = "Kick 1")]
        public ButtonControl kick1 { get; private set; }

        /// <summary>
        /// The second of two kick pedals on the drumkit.
        /// </summary>
        [InputControl(displayName = "Kick 2")]
        public ButtonControl kick2 { get; private set; }

        /// <summary>
        /// The number of pads available on the drumkit.
        /// </summary>
        public const int PadCount = 9;

        /// <summary>
        /// Retrieves a pad control by index.<br/>
        /// 0-1 = kicks, 2-5 = pads, 6-8 = cymbals.
        /// </summary>
        public ButtonControl GetPad(int index)
        {
            switch (index)
            {
                case 0: return kick1;
                case 1: return kick2;
                case 2: return redPad;
                case 3: return yellowPad;
                case 4: return bluePad;
                case 5: return greenPad;
                case 6: return yellowCymbal;
                case 7: return blueCymbal;
                case 8: return greenCymbal;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Retrieves a pad control by enum value.
        /// </summary>
        public ButtonControl GetPad(FourLanePad pad)
        {
            switch (pad)
            {
                case FourLanePad.Kick1: return kick1;
                case FourLanePad.Kick2: return kick2;
                case FourLanePad.RedPad: return redPad;
                case FourLanePad.YellowPad: return yellowPad;
                case FourLanePad.BluePad: return bluePad;
                case FourLanePad.GreenPad: return greenPad;
                case FourLanePad.YellowCymbal: return yellowCymbal;
                case FourLanePad.BlueCymbal: return blueCymbal;
                case FourLanePad.GreenCymbal: return greenCymbal;
                default: throw new ArgumentException($"Could not determine the pad to retrieve! Value: '{pad}'", nameof(pad));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current pad states.
        /// </summary>
        public FourLanePad GetPadMask()
        {
            var mask = FourLanePad.None;
            if (kick1.isPressed) mask |= FourLanePad.Kick1;
            if (kick2.isPressed) mask |= FourLanePad.Kick2;
            if (redPad.isPressed) mask |= FourLanePad.RedPad;
            if (yellowPad.isPressed) mask |= FourLanePad.YellowPad;
            if (bluePad.isPressed) mask |= FourLanePad.BluePad;
            if (greenPad.isPressed) mask |= FourLanePad.GreenPad;
            if (yellowCymbal.isPressed) mask |= FourLanePad.YellowCymbal;
            if (blueCymbal.isPressed) mask |= FourLanePad.BlueCymbal;
            if (greenCymbal.isPressed) mask |= FourLanePad.GreenCymbal;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the pad states in the given state event.
        /// </summary>
        public FourLanePad GetPadMask(InputEventPtr eventPtr)
        {
            var mask = FourLanePad.None;
            if (kick1.IsPressedInEvent(eventPtr)) mask |= FourLanePad.Kick1;
            if (kick2.IsPressedInEvent(eventPtr)) mask |= FourLanePad.Kick2;
            if (redPad.IsPressedInEvent(eventPtr)) mask |= FourLanePad.RedPad;
            if (yellowPad.IsPressedInEvent(eventPtr)) mask |= FourLanePad.YellowPad;
            if (bluePad.IsPressedInEvent(eventPtr)) mask |= FourLanePad.BluePad;
            if (greenPad.IsPressedInEvent(eventPtr)) mask |= FourLanePad.GreenPad;
            if (yellowCymbal.IsPressedInEvent(eventPtr)) mask |= FourLanePad.YellowCymbal;
            if (blueCymbal.IsPressedInEvent(eventPtr)) mask |= FourLanePad.BlueCymbal;
            if (greenCymbal.IsPressedInEvent(eventPtr)) mask |= FourLanePad.GreenCymbal;
            return mask;
        }

        /// <summary>
        /// Finishes setup of the device.
        /// </summary>
        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));

            redPad = GetChildControl<ButtonControl>(nameof(redPad));
            yellowPad = GetChildControl<ButtonControl>(nameof(yellowPad));
            bluePad = GetChildControl<ButtonControl>(nameof(bluePad));
            greenPad = GetChildControl<ButtonControl>(nameof(greenPad));

            yellowCymbal = GetChildControl<ButtonControl>(nameof(yellowCymbal));
            blueCymbal = GetChildControl<ButtonControl>(nameof(blueCymbal));
            greenCymbal = GetChildControl<ButtonControl>(nameof(greenCymbal));

            kick1 = GetChildControl<ButtonControl>(nameof(kick1));
            kick2 = GetChildControl<ButtonControl>(nameof(kick2));
        }

        /// <summary>
        /// Sets this device as the current <see cref="FourLaneDrumkit"/>.
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
