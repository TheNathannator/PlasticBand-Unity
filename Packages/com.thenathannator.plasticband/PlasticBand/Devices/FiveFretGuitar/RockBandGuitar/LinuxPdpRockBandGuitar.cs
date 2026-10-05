using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using PlasticBand.LowLevel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace PlasticBand.Devices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LinuxPdpRockBandGuitarState : IRockBandGuitarState_Distinct, IInputStateTypeInfo
    {
        public FourCC format => new FourCC('L', 'J', 'O', 'Y');
        public ushort buttons;
        private ushort m_Padding;
        public int whammyAxis;
        public int tiltAxis;
        public int hatX;
        public int hatY;

        public bool select { get => (buttons & 1) != 0; set => buttons.SetBit(1, value); }
        public bool start { get => (buttons & 2) != 0; set => buttons.SetBit(2, value); }
        public bool system { get => (buttons & 4) != 0; set => buttons.SetBit(4, value); }
        public bool green { get => (buttons & 8) != 0; set => buttons.SetBit(8, value); }
        public bool red { get => (buttons & 16) != 0; set => buttons.SetBit(16, value); }
        public bool yellow { get => (buttons & 32) != 0; set => buttons.SetBit(32, value); }
        public bool blue { get => (buttons & 64) != 0; set => buttons.SetBit(64, value); }
        public bool orange { get => (buttons & 128) != 0; set => buttons.SetBit(128, value); }
        public bool soloGreen { get => (buttons & 256) != 0; set => buttons.SetBit(256, value); }
        public bool soloRed { get => (buttons & 512) != 0; set => buttons.SetBit(512, value); }
        public bool soloYellow { get => (buttons & 1024) != 0; set => buttons.SetBit(1024, value); }
        public bool soloBlue { get => (buttons & 2048) != 0; set => buttons.SetBit(2048, value); }
        public bool soloOrange { get => (buttons & 4096) != 0; set => buttons.SetBit(4096, value); }

        public bool dpadUp { get => hatY < 0; set => hatY = value ? -1 : hatY < 0 ? 0 : hatY; }
        public bool dpadDown { get => hatY > 0; set => hatY = value ? 1 : hatY > 0 ? 0 : hatY; }
        public bool dpadLeft { get => hatX < 0; set => hatX = value ? -1 : hatX < 0 ? 0 : hatX; }
        public bool dpadRight { get => hatX > 0; set => hatX = value ? 1 : hatX > 0 ? 0 : hatX; }

        public byte whammy
        {
            get => DecodeAxis(whammyAxis);
            set => whammyAxis = EncodeAxis(value);
        }

        public sbyte tilt
        {
            get => (sbyte)(DecodeAxis(tiltAxis) >> 1);
            set => tiltAxis = EncodeAxis((byte)(Math.Max(value, (sbyte)0) << 1));
        }

        // xone's PDP guitar report does not include the pickup selector.
        public int pickupSwitch { get => 0; set {} }

        private static byte DecodeAxis(int value)
            => (byte)(((Math.Min(Math.Max(value, -32767), 32767) + 32767) * 255 + 32767) / 65534);

        private static int EncodeAxis(byte value) => (value * 65534 + 127) / 255 - 32767;
    }

    [InputControlLayout(stateType = typeof(TranslatedRockBandGuitarState),
        displayName = "Xbox PDP Rock Band Guitar (xone)")]
    public class LinuxPdpRockBandGuitar : RockBandGuitar, IInputStateCallbackReceiver
    {
        private const string NativeProduct = "PDP Rock Band 4 Jaguar";
        private static readonly TranslateStateHandler<LinuxPdpRockBandGuitarState, TranslatedRockBandGuitarState>
            s_Translator = TranslatingRockBandGuitar_Distinct<LinuxPdpRockBandGuitarState>.TranslateState;

        [Conditional("UNITY_STANDALONE_LINUX"), Conditional("UNITY_EDITOR_LINUX")]
        internal new static void Initialize()
        {
            InputSystem.RegisterLayout<LinuxPdpRockBandGuitar>();
            InputSystem.RegisterLayoutMatcher(nameof(LinuxPdpRockBandGuitar),
                new InputDeviceMatcher().WithInterface("Linux").WithProduct("^" + NativeProduct + "$"));
        }

        protected override void FinishSetup()
        {
            if (description.interfaceName == "Linux" && !HasSupportedDescriptor(description.capabilities))
            {
                throw new NotSupportedException("Unsupported xone PDP guitar report descriptor.");
            }
            base.FinishSetup();
            StateTranslator<LinuxPdpRockBandGuitarState, TranslatedRockBandGuitarState>.VerifyDevice(this);
        }

        void IInputStateCallbackReceiver.OnNextUpdate() {}
        void IInputStateCallbackReceiver.OnStateEvent(InputEventPtr eventPtr)
            => StateTranslator<LinuxPdpRockBandGuitarState, TranslatedRockBandGuitarState>
                .OnStateEvent(this, eventPtr, s_Translator);
        bool IInputStateCallbackReceiver.GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr,
            ref uint offset)
            => StateTranslator<LinuxPdpRockBandGuitarState, TranslatedRockBandGuitarState>
                .GetStateOffsetForEvent(this, control, eventPtr, ref offset, s_Translator);

        private static bool HasSupportedDescriptor(string capabilities)
        {
            if (string.IsNullOrEmpty(capabilities))
                return false;
            var descriptor = JsonUtility.FromJson<NativeDescriptor>(capabilities);
            var controls = descriptor?.controls;
            if (controls == null || controls.Length != 17)
                return false;
            for (int button = 0; button < 13; button++)
            {
                var control = controls[button];
                if (control.featureType != 3 || control.featureSize != 1 ||
                    control.offset != button / 8 || control.bit != button % 8)
                    return false;
            }
            return MatchesAxis(controls[13], 4, 1, 2) && MatchesAxis(controls[14], 8, 1, 3) &&
                MatchesAxis(controls[15], 12, 4, 12) && MatchesAxis(controls[16], 16, 4, 13);
        }

        private static bool MatchesAxis(NativeControl control, int offset, int type, int usage)
            => control.offset == offset && control.featureSize == 4 &&
                control.featureType == type && control.usageHint == usage;

#pragma warning disable CS0649
        [Serializable]
        private class NativeDescriptor
        {
            public NativeControl[] controls;
        }

        [Serializable]
        private struct NativeControl
        {
            public int offset;
            public int featureSize;
            public int featureType;
            public int usageHint;
            public int bit;
        }
#pragma warning restore CS0649
    }
}
