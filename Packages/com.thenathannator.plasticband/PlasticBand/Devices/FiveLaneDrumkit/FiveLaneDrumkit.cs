using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace PlasticBand.Devices
{
    /// <summary>
    /// Bitmask of possible pad/cymbal/kick values on a 5-lane drumkit.
    /// </summary>
    [Flags]
    public enum FiveLanePad
    {
        None = 0,
        Kick = 0x01,
        Red = 0x02,
        Yellow = 0x04,
        Blue = 0x08,
        Orange = 0x10,
        Green = 0x20,
    }

    internal interface IFiveLaneDrumkitState : IInputStateTypeInfo
    {
        bool red_east { get; set; }
        bool yellow_north { get; set; }
        bool blue_west { get; set; }
        bool green_south { get; set; }
        bool orange { get; set; }
        bool kick { get; set; }

        bool dpadUp { get; set; }
        bool dpadDown { get; set; }
        bool dpadLeft { get; set; }
        bool dpadRight { get; set; }

        bool start { get; set; }
        bool select { get; set; }
        bool system { get; set; }

        byte redVelocity { get; set; }
        byte yellowVelocity { get; set; }
        byte blueVelocity { get; set; }
        byte orangeVelocity { get; set; }
        byte greenVelocity { get; set; }
        byte kickVelocity { get; set; }
    }

    /// <summary>
    /// A 5-lane (Guitar Hero) drumkit controller.
    /// </summary>
    [InputControlLayout(displayName = "Guitar Hero (5-Lane) Drumkit")]
    public class FiveLaneDrumkit : InputDevice
    {
        /// <summary>
        /// The current <see cref="FiveLaneDrumkit"/>.
        /// </summary>
        public static FiveLaneDrumkit current { get; private set; }

        /// <summary>
        /// A collection of all <see cref="FiveLaneDrumkit"/>s currently connected to the system.
        /// </summary>
        public new static IReadOnlyList<FiveLaneDrumkit> all => s_AllDevices;
        private static readonly List<FiveLaneDrumkit> s_AllDevices = new List<FiveLaneDrumkit>();

        internal static void Initialize()
        {
            InputSystem.RegisterLayout<FiveLaneDrumkit>();
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
        [InputControl(displayName = "Yellow Cymbal")]
        public ButtonControl yellowCymbal { get; private set; }

        /// <summary>
        /// The blue pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Blue Pad")]
        public ButtonControl bluePad { get; private set; }

        /// <summary>
        /// The green pad on the drumkit.
        /// </summary>
        [InputControl(displayName = "Orange Cymbal")]
        public ButtonControl orangeCymbal { get; private set; }

        /// <summary>
        /// The yellow cymbal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Green Pad", usages = new[] { "PrimaryAction", "Submit" })]
        public ButtonControl greenPad { get; private set; }

        /// <summary>
        /// The kick pedal on the drumkit.
        /// </summary>
        [InputControl(displayName = "Kick")]
        public ButtonControl kick { get; private set; }

        /// <summary>
        /// The number of pads available on the drumkit.
        /// </summary>
        public const int PadCount = 6;

        /// <summary>
        /// Retrieves a pad control by index.<br/>
        /// 0 = kick, 1 = red, 5 = green.
        /// </summary>
        public ButtonControl GetPad(int index)
        {
            switch (index)
            {
                case 0: return kick;
                case 1: return redPad;
                case 2: return yellowCymbal;
                case 3: return bluePad;
                case 4: return orangeCymbal;
                case 5: return greenPad;
                default: throw new ArgumentOutOfRangeException(nameof(index), index, $"Expected an index less than {nameof(PadCount)} ({PadCount})!");
            }
        }

        /// <summary>
        /// Retrieves a pad control by enum value.
        /// </summary>
        public ButtonControl GetPad(FiveLanePad pad)
        {
            switch (pad)
            {
                case FiveLanePad.Kick: return kick;
                case FiveLanePad.Red: return redPad;
                case FiveLanePad.Yellow: return yellowCymbal;
                case FiveLanePad.Blue: return bluePad;
                case FiveLanePad.Orange: return orangeCymbal;
                case FiveLanePad.Green: return greenPad;
                default: throw new ArgumentException($"Invalid pad value {pad}!", nameof(pad));
            }
        }

        /// <summary>
        /// Retrives a bitmask of the current pad states.
        /// </summary>
        public FiveLanePad GetPadMask()
        {
            var mask = FiveLanePad.None;
            if (kick.isPressed) mask |= FiveLanePad.Kick;
            if (redPad.isPressed) mask |= FiveLanePad.Red;
            if (yellowCymbal.isPressed) mask |= FiveLanePad.Yellow;
            if (bluePad.isPressed) mask |= FiveLanePad.Blue;
            if (orangeCymbal.isPressed) mask |= FiveLanePad.Orange;
            if (greenPad.isPressed) mask |= FiveLanePad.Green;
            return mask;
        }

        /// <summary>
        /// Retrives a bitmask of the pad states in the given state event.
        /// </summary>
        public FiveLanePad GetPadMask(InputEventPtr eventPtr)
        {
            var mask = FiveLanePad.None;
            if (kick.IsPressedInEvent(eventPtr)) mask |= FiveLanePad.Kick;
            if (redPad.IsPressedInEvent(eventPtr)) mask |= FiveLanePad.Red;
            if (yellowCymbal.IsPressedInEvent(eventPtr)) mask |= FiveLanePad.Yellow;
            if (bluePad.IsPressedInEvent(eventPtr)) mask |= FiveLanePad.Blue;
            if (orangeCymbal.IsPressedInEvent(eventPtr)) mask |= FiveLanePad.Orange;
            if (greenPad.IsPressedInEvent(eventPtr)) mask |= FiveLanePad.Green;
            return mask;
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();

            dpad = GetChildControl<DpadControl>(nameof(dpad));

            startButton = GetChildControl<ButtonControl>(nameof(startButton));
            selectButton = GetChildControl<ButtonControl>(nameof(selectButton));

            buttonSouth = GetChildControl<ButtonControl>(nameof(buttonSouth));
            buttonEast = GetChildControl<ButtonControl>(nameof(buttonEast));
            buttonWest = GetChildControl<ButtonControl>(nameof(buttonWest));
            buttonNorth = GetChildControl<ButtonControl>(nameof(buttonNorth));

            redPad = GetChildControl<ButtonControl>(nameof(redPad));
            yellowCymbal = GetChildControl<ButtonControl>(nameof(yellowCymbal));
            bluePad = GetChildControl<ButtonControl>(nameof(bluePad));
            orangeCymbal = GetChildControl<ButtonControl>(nameof(orangeCymbal));
            greenPad = GetChildControl<ButtonControl>(nameof(greenPad));

            kick = GetChildControl<ButtonControl>(nameof(kick));
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
    }
}
