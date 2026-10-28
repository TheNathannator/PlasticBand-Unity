using System;
using PlasticBand.Devices;
using UnityEngine.InputSystem;

namespace PlasticBand.LowLevel
{
    internal static class EvdevSantrollerState
    {
        private static readonly int[] s_GuitarKeys = { 0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x13A, 0x13B, 0x13C };
        private static readonly int[] s_RockBandKeys = { 0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x138, 0x139, 0x132, 0x135, 0x13A, 0x13B, 0x13C };
        private static readonly int[] s_SixFretKeys = { 0x130, 0x131, 0x134, 0x133, 0x136, 0x137, 0x13A, 0x13B, 0x13D, 0x13C };
        private static readonly int[] s_DrumKeys = { 0x130, 0x131, 0x133, 0x134, 0x13E, 0x137, 0x136, 0x13D, 0x13A, 0x13B, 0x13C };
        private static readonly int[] s_TurntableKeys = { 0x130, 0x131, 0x133, 0x134, 0x13A, 0x13B, 0x13C, 0x132, 0x135, 0x136, 0x137, 0x138, 0x139 };
        private static readonly int[] s_CurrentKeys = { 0x130, 0x131, 0x133, 0x134, 0x136, 0x137, 0x13A, 0x13B, 0x13C, 0x13D, 0x13E };

        internal static int Validate(InputDevice device, SantrollerDeviceType type)
        {
            var caps = EvdevLayoutFinder.GetCapabilities(device);
            if (caps.driver != "hid-generic" || caps.vendorId != SantrollerLayoutFinder.VendorID ||
                (caps.productId != 0x2882 && caps.productId != 0x2883) || caps.subType != (int)type ||
                (caps.santrollerProtocol != 1 && caps.santrollerProtocol != 2) ||
                (caps.santrollerProtocol == 1 && caps.productId != 0x2882))
                throw new NotSupportedException("Missing or unsupported Santroller evdev identity.");

            var keys = s_CurrentKeys;
            caps.RequireAxes(0x10, 0x11);
            if (caps.santrollerProtocol == 2)
                caps.RequireAxes(0, 1, 2, 3, 4, 5);
            else
            {
                switch (type)
                {
                    case SantrollerDeviceType.GuitarHeroGuitar:
                        keys = s_GuitarKeys;
                        caps.RequireAxes(0, 1, 3);
                        break;
                    case SantrollerDeviceType.RockBandGuitar:
                        keys = s_RockBandKeys;
                        caps.RequireAxes(0, 1, 3);
                        break;
                    case SantrollerDeviceType.LiveGuitar:
                        keys = s_SixFretKeys;
                        caps.RequireAxes(0, 1);
                        break;
                    case SantrollerDeviceType.GuitarHeroDrums:
                        keys = s_GuitarKeys;
                        caps.RequireAxes(0, 1, 2, 3, 4, 5);
                        break;
                    case SantrollerDeviceType.RockBandDrums:
                        keys = s_DrumKeys;
                        caps.RequireAxes(0, 1, 2, 3, 4, 5, 6);
                        break;
                    case SantrollerDeviceType.DjHeroTurntable:
                        keys = s_TurntableKeys;
                        caps.RequireAxes(0, 1, 3, 4);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported Santroller evdev instrument.");
                }
            }
            byte[] bits = Convert.FromBase64String(caps.keys);
            foreach (int code in keys)
            {
                if (!EvdevCapabilities.HasBit(bits, code))
                    throw new NotSupportedException($"Santroller evdev instrument is missing required KEY code {code}.");
            }
            return caps.santrollerProtocol;
        }

        private static ushort Buttons(ref EvdevState state, int[] keys)
        {
            ushort buttons = 0;
            for (int bit = 0; bit < keys.Length; bit++)
            {
                if (state.GetKey(keys[bit]))
                    buttons |= (ushort)(1 << bit);
            }
            return buttons;
        }

        internal static unsafe TranslatedGuitarHeroGuitarState GuitarHero(ref EvdevState state, int protocol)
        {
            if (protocol == 2)
            {
                var raw = state.ToXInput(false);
                var guitar = new XInputGuitarHeroGuitarState_GH5() { state = *(XInputGuitarHeroGuitarState*)&raw };
                return TranslatingGuitarHeroGuitar_Discrete<XInputGuitarHeroGuitarState_GH5>.TranslateState(ref guitar);
            }
            var legacy = new SantrollerHIDGuitarHeroGuitarState()
            {
                buttons = (SantrollerHIDGuitarHeroGuitarState.Button)Buttons(ref state, s_GuitarKeys),
                dpad = state.GetDpad(), m_Whammy = (byte)state.GetAxis(0),
                m_TouchBar = (byte)state.GetAxis(1), m_Tilt = (byte)state.GetAxis(3),
            };
            return TranslatingGuitarHeroGuitar_Discrete<SantrollerHIDGuitarHeroGuitarState>.TranslateState(ref legacy);
        }

        internal static unsafe TranslatedRockBandGuitarState RockBand(ref EvdevState state, int protocol)
        {
            if (protocol == 2)
            {
                var raw = state.ToXInput(false);
                var guitar = *(XInputRockBandGuitarState*)&raw;
                return TranslatingRockBandGuitar_Flags<XInputRockBandGuitarState>.TranslateState(ref guitar);
            }
            var legacy = new SantrollerHIDRockBandGuitarState()
            {
                buttons = (SantrollerHIDRockBandGuitarState.Button)Buttons(ref state, s_RockBandKeys),
                dpad = state.GetDpad(), whammy = (byte)state.GetAxis(0),
                pickupSwitch = (byte)state.GetAxis(1), tilt = (sbyte)(state.GetAxis(3) - 128),
            };
            return TranslatingRockBandGuitar_Distinct<SantrollerHIDRockBandGuitarState>.TranslateState(ref legacy);
        }

        internal static unsafe TranslatedSixFretState SixFret(ref EvdevState state, int protocol)
        {
            if (protocol == 2)
            {
                var raw = state.ToXInput(false);
                var guitar = *(XInputSixFretGuitarState*)&raw;
                return TranslatingSixFretGuitar<XInputSixFretGuitarState>.TranslateState(ref guitar);
            }
            var legacy = new SantrollerHIDSixFretGuitarState()
            {
                buttons = (SantrollerHIDSixFretGuitarState.Button)Buttons(ref state, s_SixFretKeys),
                dpad = state.GetDpad(), whammy = (byte)state.GetAxis(0),
                tilt = (sbyte)(state.GetAxis(1) - 128),
            };
            return TranslatingSixFretGuitar<SantrollerHIDSixFretGuitarState>.TranslateState(ref legacy);
        }

        internal static unsafe TranslatedFiveLaneState FiveLane(ref EvdevState state, int protocol)
        {
            if (protocol == 2)
            {
                var raw = state.ToXInput(false);
                var drums = *(XInputFiveLaneDrumkitState*)&raw;
                return TranslatingFiveLaneDrumkit<XInputFiveLaneDrumkitState>.TranslateState(ref drums);
            }
            var legacy = new SantrollerFiveLaneDrumkitState()
            {
                buttons = (SantrollerFiveLaneDrumkitState.Button)Buttons(ref state, s_GuitarKeys),
                dpad = state.GetDpad(), greenVelocity = (byte)state.GetAxis(0),
                redVelocity = (byte)state.GetAxis(1), yellowVelocity = (byte)state.GetAxis(3),
                blueVelocity = (byte)state.GetAxis(4), orangeVelocity = (byte)state.GetAxis(2),
                kickVelocity = (byte)state.GetAxis(5),
            };
            return TranslatingFiveLaneDrumkit<SantrollerFiveLaneDrumkitState>.TranslateState(ref legacy);
        }

        internal static unsafe TranslatedFourLaneState FourLane(ref EvdevState state, int protocol, ref bool hasFlags)
        {
            if (protocol == 2)
            {
                var raw = state.ToXInput(false);
                var drums = *(XInputFourLaneDrumkitState*)&raw;
                return TranslatingFourLaneDrumkit_Flags<XInputFourLaneDrumkitState>.TranslateState(ref drums, ref hasFlags);
            }
            var legacy = new SantrollerFourLaneDrumkitState()
            {
                buttons = (SantrollerFourLaneDrumkitState.Button)Buttons(ref state, s_DrumKeys),
                dpad = state.GetDpad(), greenPadVelocity = (byte)state.GetAxis(0),
                redPadVelocity = (byte)state.GetAxis(1), yellowPadVelocity = (byte)state.GetAxis(3),
                bluePadVelocity = (byte)state.GetAxis(4), greenCymbalVelocity = (byte)state.GetAxis(2),
                yellowCymbalVelocity = (byte)state.GetAxis(5), blueCymbalVelocity = (byte)state.GetAxis(6),
            };
            return TranslatingFourLaneDrumkit_Hybrid<SantrollerFourLaneDrumkitState>.TranslateState(ref legacy, ref hasFlags);
        }

        internal static unsafe TranslatedTurntableState Turntable(ref EvdevState state, int protocol)
        {
            if (protocol == 2)
            {
                var raw = state.ToXInput(false);
                var table = *(XInputTurntableState*)&raw;
                return TranslatingTurntable<XInputTurntableState>.TranslateState(ref table);
            }
            var legacy = new SantrollerHIDTurntableState()
            {
                buttons = (SantrollerHIDTurntableState.Button)Buttons(ref state, s_TurntableKeys),
                dpad = state.GetDpad(), leftVelocity = (sbyte)(state.GetAxis(0) - 128),
                rightVelocity = (sbyte)(state.GetAxis(1) - 128),
                effectsDial = (ushort)(state.GetAxis(3) << 8), crossfader = (sbyte)(state.GetAxis(4) - 128),
            };
            return TranslatingTurntable<SantrollerHIDTurntableState>.TranslateState(ref legacy);
        }
    }
}
