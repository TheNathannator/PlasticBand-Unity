using System;
using PlasticBand.Devices.LowLevel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

namespace PlasticBand.Haptics
{
    /// <summary>
    /// Speeds for the stage kit strobe light.
    /// </summary>
    public enum StageKitStrobeSpeed : byte
    {
        Off,
        Slow,
        Medium,
        Fast,
        Fastest
    }

    /// <summary>
    /// Available LED colors on the stage kit.
    /// </summary>
    [Flags]
    public enum StageKitLedColor : byte
    {
        None = 0x00,

        Red = 0x01,
        Yellow = 0x02,
        Blue = 0x04,
        Green = 0x08,

        All = Red | Yellow | Blue | Green
    }

    /// <summary>
    /// Bitmask for the stage kit LEDs.
    /// </summary>
    [Flags]
    public enum StageKitLed : byte
    {
        None = 0x00,

        Led1 = 0x01,
        Led2 = 0x02,
        Led3 = 0x04,
        Led4 = 0x08,
        Led5 = 0x10,
        Led6 = 0x20,
        Led7 = 0x40,
        Led8 = 0x80,

        All = Led1 | Led2 | Led3 | Led4 | Led5 | Led6 | Led7 | Led8
    }

    /// <summary>
    /// Interface for <see cref="StageKit"/> haptics.
    /// </summary>
    public interface IStageKitHaptics : IHaptics
    {
        /// <summary>
        /// Enables or disables the fog machine.
        /// </summary>
        void SetFogMachine(bool enabled);

        /// <summary>
        /// Sets the speed of the strobe light.
        /// </summary>
        void SetStrobeSpeed(StageKitStrobeSpeed speed);

        /// <summary>
        /// Enables the given LEDs for multiple colors in one go.
        /// </summary>
        void SetLeds(StageKitLedColor color, StageKitLed leds);

        /// <summary>
        /// Enables the given red LEDs.
        /// </summary>
        void SetRedLeds(StageKitLed leds);

        /// <summary>
        /// Enables the given yellow LEDs.
        /// </summary>
        void SetYellowLeds(StageKitLed leds);

        /// <summary>
        /// Enables the given blue LEDs.
        /// </summary>
        void SetBlueLeds(StageKitLed leds);

        /// <summary>
        /// Enables the given green LEDs.
        /// </summary>
        void SetGreenLeds(StageKitLed leds);
    }

    internal enum StageKitProtocol
    {
        XInput,
        SantrollerHID,
    }

    /// <summary>
    /// Handles haptics for Rock Band stage kits.
    /// </summary>
    internal struct StageKitHaptics : IStageKitHaptics
    {
        internal enum CommandId : byte
        {
            FogOn = 0x01,
            FogOff = 0x02,

            StrobeSlow = 0x03,
            StrobeMedium = 0x04,
            StrobeFast = 0x05,
            StrobeFastest = 0x06,
            StrobeOff = 0x07,

            BlueLeds = 0x20,
            GreenLeds = 0x40,
            YellowLeds = 0x60,
            RedLeds = 0x80,

            Reset = 0xFF
        }

        private InputDevice m_Device;
        private StageKitProtocol m_Protocol;

        private bool m_FogEnabled;
        private StageKitStrobeSpeed m_StrobeSpeed;
        private StageKitLed m_RedLeds;
        private StageKitLed m_YellowLeds;
        private StageKitLed m_BlueLeds;
        private StageKitLed m_GreenLeds;

        public bool hapticsEnabled { get; private set; }

        public static StageKitHaptics Create(InputDevice device, StageKitProtocol protocol)
            => new StageKitHaptics()
        {
            m_Device = device,
            m_Protocol = protocol,
            m_StrobeSpeed = StageKitStrobeSpeed.Off,
            m_RedLeds = StageKitLed.None,
            m_YellowLeds = StageKitLed.None,
            m_BlueLeds = StageKitLed.None,
            m_GreenLeds = StageKitLed.None,
            m_FogEnabled = false,
            hapticsEnabled = true,
        };

        public void PauseHaptics()
        {
            if (!hapticsEnabled)
                return;

            hapticsEnabled = false;
            SendCommand(CommandId.Reset, 0);
        }

        public void ResumeHaptics()
        {
            if (hapticsEnabled)
                return;

            hapticsEnabled = true;

            SendFogMachine(m_FogEnabled);
            SendStrobeSpeed(m_StrobeSpeed);
            SendCommand(CommandId.RedLeds, (byte)m_RedLeds);
            SendCommand(CommandId.YellowLeds, (byte)m_YellowLeds);
            SendCommand(CommandId.BlueLeds, (byte)m_BlueLeds);
            SendCommand(CommandId.GreenLeds, (byte)m_GreenLeds);
        }

        public void ResetHaptics()
        {
            hapticsEnabled = true;

            m_FogEnabled = false;
            m_StrobeSpeed = StageKitStrobeSpeed.Off;
            m_RedLeds = StageKitLed.None;
            m_YellowLeds = StageKitLed.None;
            m_BlueLeds = StageKitLed.None;
            m_GreenLeds = StageKitLed.None;

            SendCommand(CommandId.Reset, 0);
        }

        public void SetFogMachine(bool enabled)
        {
            if (enabled != m_FogEnabled)
            {
                m_FogEnabled = enabled;
                SendFogMachine(enabled);
            }
        }

        public void SetStrobeSpeed(StageKitStrobeSpeed speed)
        {
            if (speed != m_StrobeSpeed)
            {
                m_StrobeSpeed = speed;
                SendStrobeSpeed(speed);
            }
        }

        public void SetLeds(StageKitLedColor color, StageKitLed leds)
        {
            if ((color & StageKitLedColor.Red) != 0)
            {
                SetRedLeds(leds);
            }

            if ((color & StageKitLedColor.Yellow) != 0)
            {
                SetYellowLeds(leds);
            }

            if ((color & StageKitLedColor.Blue) != 0)
            {
                SetBlueLeds(leds);
            }

            if ((color & StageKitLedColor.Green) != 0)
            {
                SetGreenLeds(leds);
            }
        }

        public void SetRedLeds(StageKitLed leds)
        {
            if (leds != m_RedLeds)
            {
                m_RedLeds = leds;
                SendCommand(CommandId.RedLeds, (byte)leds);
            }
        }

        public void SetYellowLeds(StageKitLed leds)
        {
            if (leds != m_YellowLeds)
            {
                m_YellowLeds = leds;
                SendCommand(CommandId.YellowLeds, (byte)leds);
            }
        }

        public void SetBlueLeds(StageKitLed leds)
        {
            if (leds != m_BlueLeds)
            {
                m_BlueLeds = leds;
                SendCommand(CommandId.BlueLeds, (byte)leds);
            }
        }

        public void SetGreenLeds(StageKitLed leds)
        {
            if (leds != m_GreenLeds)
            {
                m_GreenLeds = leds;
                SendCommand(CommandId.GreenLeds, (byte)leds);
            }
        }

        private void SendFogMachine(bool enabled)
        {
            SendCommand(enabled ? CommandId.FogOn : CommandId.FogOff, 0);
        }

        private void SendStrobeSpeed(StageKitStrobeSpeed speed)
        {
            CommandId id;
            switch (speed)
            {
                default: id = CommandId.StrobeOff; break;
                case StageKitStrobeSpeed.Slow: id = CommandId.StrobeSlow; break;
                case StageKitStrobeSpeed.Medium: id = CommandId.StrobeMedium; break;
                case StageKitStrobeSpeed.Fast: id = CommandId.StrobeFast; break;
                case StageKitStrobeSpeed.Fastest: id = CommandId.StrobeFastest; break;
            }

            SendCommand(id, 0);
        }

        private void SendCommand(CommandId commandId, byte parameter)
        {
            SendCommand((byte)commandId, parameter);
        }

        public void SendCommand(byte commandId, byte parameter)
        {
            if (!hapticsEnabled)
            {
                return;
            }

            switch (m_Protocol)
            {
                case StageKitProtocol.XInput:
                {
                    var command = new XInputVibrationCommand(parameter, commandId);
                    m_Device.LoggedExecuteCommand(ref command);
                    break;
                }
                case StageKitProtocol.SantrollerHID:
                {
                    var command = new PS3OutputCommand(1, 0x5A);
                    unsafe
                    {
                        command.data[0] = parameter;
                        command.data[1] = commandId;
                    }

                    m_Device.LoggedExecuteCommand(ref command);
                    break;
                }
            }
        }
    }
}